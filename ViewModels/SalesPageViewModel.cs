using System;
using System.Collections.ObjectModel;
using System.Linq;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.EntityFrameworkCore;
using ComputerServiceManager.Database;
using ComputerServiceManager.Utils;

namespace ComputerServiceManager.ViewModels
{
    public partial class SalesPageViewModel : ViewModelBase
    {
        private readonly AppDbContext _context;

        public SalesPageViewModel()
        {
            _context = new AppDbContext();
            LoadClients();
            
            _allUnpaidServices = new ObservableCollection<Service>();
            UnpaidServices = new ObservableCollection<Service>();

            IsServiceSelectionVisible = false;
            IsClientSelectionVisible = true;
        }
        
        private ObservableCollection<Service> _allUnpaidServices = new();

        [ObservableProperty]
        private ObservableCollection<Service> unpaidServices = new();

        [ObservableProperty]
        private Service selectedService;

        [ObservableProperty]
        private ObservableCollection<InvoiceItem> invoiceItems = new();

        [ObservableProperty]
        private InvoiceItem selectedInvoiceItem;

        [ObservableProperty]
        private ObservableCollection<Client> clients = new();

        [ObservableProperty]
        private Client selectedClient;

        [ObservableProperty]
        private bool isServiceSelectionVisible;

        [ObservableProperty]
        private bool isClientSelectionVisible;

        partial void OnSelectedClientChanged(Client oldValue, Client newValue)
        {
            LoadUnpaidServicesForClient(newValue);
            OnPropertyChanged(nameof(CanGenerateBill));
        }

        private string _serviceSearchText = "";
        public string ServiceSearchText
        {
            get => _serviceSearchText;
            set
            {
                if (SetProperty(ref _serviceSearchText, value))
                {
                    FilterUnpaidServices();
                }
            }
        }

        private string _clientSearchText = "";
        public string ClientSearchText
        {
            get => _clientSearchText;
            set
            {
                if (SetProperty(ref _clientSearchText, value))
                {
                    FilterClients();
                }
            }
        }

        private ObservableCollection<Client> _filteredClients = new();
        public ObservableCollection<Client> FilteredClients
        {
            get => _filteredClients;
            set => SetProperty(ref _filteredClients, value);
        }

        public decimal TotalNet => InvoiceItems.Sum(i => i.LineNet * i.Quantity);
        public decimal TotalVat => InvoiceItems.Sum(i => i.LineVat * i.Quantity);
        public decimal TotalGross => InvoiceItems.Sum(i => i.LineGross * i.Quantity);

        public bool CanGenerateBill => SelectedClient != null && InvoiceItems.Count > 0;

        private void LoadClients()
        {
            var clientsFromDb = _context.Clients
                .OrderBy(c => c.Name)
                .AsNoTracking()
                .ToList();

            Clients = new ObservableCollection<Client>(clientsFromDb);
            FilteredClients = new ObservableCollection<Client>(clientsFromDb);
        }
        
        private void LoadUnpaidServicesForClient(Client client)
        {
            if (client == null)
            {
                _allUnpaidServices.Clear();
                UnpaidServices = new ObservableCollection<Service>();
                return;
            }
            
            var unpaidList = _context.Services
                .Include(s => s.ServiceType)
                .Include(s => s.Device)
                .Where(s =>
                    s.Device.OwnerClientId == client.Id &&
                    s.IsPaid == false)
                .OrderBy(s => s.Date)
                .AsNoTracking()
                .ToList();

            _allUnpaidServices = new ObservableCollection<Service>(unpaidList);
            UnpaidServices = new ObservableCollection<Service>(_allUnpaidServices);
        }
        
        private void FilterUnpaidServices()
        {
            if (string.IsNullOrWhiteSpace(ServiceSearchText))
            {
                UnpaidServices = new ObservableCollection<Service>(_allUnpaidServices);
            }
            else
            {
                var filtered = _allUnpaidServices
                    .Where(s => s.ServiceType.Name.Contains(ServiceSearchText, StringComparison.OrdinalIgnoreCase))
                    .ToList();

                UnpaidServices = new ObservableCollection<Service>(filtered);
            }
        }

        private void FilterClients()
        {
            if (string.IsNullOrWhiteSpace(ClientSearchText))
            {
                FilteredClients = new ObservableCollection<Client>(Clients);
            }
            else
            {
                var searchText = ClientSearchText.Trim();

                var filtered = Clients
                    .Where(c =>
                        (!string.IsNullOrEmpty(c.Name) && c.Name.Contains(searchText, StringComparison.OrdinalIgnoreCase)) ||
                        (!string.IsNullOrEmpty(c.Surname) && c.Surname.Contains(searchText, StringComparison.OrdinalIgnoreCase)) ||
                        (!string.IsNullOrEmpty(c.PhoneNumber) && c.PhoneNumber.Contains(searchText, StringComparison.OrdinalIgnoreCase)) ||
                        (!string.IsNullOrEmpty(c.Email) && c.Email.Contains(searchText, StringComparison.OrdinalIgnoreCase))
                    )
                    .ToList();

                FilteredClients = new ObservableCollection<Client>(filtered);
            }
        }

        [RelayCommand]
        private void AddInvoiceItem()
        {
            if (SelectedService == null)
                return;
            
            var price = SelectedService.Price != 0
                ? SelectedService.Price
                : SelectedService.ServiceType.DefaultPrice;

            var existing = InvoiceItems.FirstOrDefault(x => x.ServiceTypeId == SelectedService.ServiceTypeId);
            if (existing != null)
            {
                existing.Quantity++;
                existing.OnQuantityChanged();
            }
            else
            {
                var newItem = new InvoiceItem
                {
                    ServiceTypeId = SelectedService.ServiceTypeId,
                    Name = SelectedService.ServiceType.Name,
                    Type = "Usługa",
                    NetPrice = price,
                    Quantity = 1
                };
                InvoiceItems.Add(newItem);
            }

            RaiseTotalsChanged();
            RemoveUnpaidService();
        }

        [RelayCommand]
        private void GenerateBill()
        {
            if (SelectedClient == null || InvoiceItems.Count == 0)
                return;

            var produkty = InvoiceItems.Select(item => new Produkt
            {
                Nazwa = item.Name,
                Ilosc = item.Quantity,
                Cena = item.LineGross,
                StawkaVAT = 'A'
            }).ToList();
            GenerateBillUtility.DrukujParagon(produkty);
            
            foreach (var item in InvoiceItems)
            {
                var servicesToMark = _context.Services
                    .Where(s =>
                        s.Device.OwnerClientId == SelectedClient.Id &&
                        s.ServiceTypeId == item.ServiceTypeId &&
                        s.IsPaid == false)
                    .ToList();

                foreach (var serv in servicesToMark)
                {
                    serv.IsPaid = true;
                    _context.Services.Update(serv);
                }
            }
        
            _context.SaveChanges();

            InvoiceItems.Clear();
            RaiseTotalsChanged();

            LoadUnpaidServicesForClient(SelectedClient);
        }

        [RelayCommand]
        private void ClearSearch()
        {
            ServiceSearchText = string.Empty;
        }

        [RelayCommand]
        private void ClearClientSearch()
        {
            ClientSearchText = string.Empty;
        }

        [RelayCommand]
        private void RemoveInvoiceItem()
        {
            if (SelectedInvoiceItem != null && InvoiceItems.Contains(SelectedInvoiceItem))
            {
                if (SelectedInvoiceItem.Quantity > 1)
                {
                    SelectedInvoiceItem.Quantity--;
                    SelectedInvoiceItem.OnQuantityChanged();
                    
                }
                else
                {
                    var serviceTypeId = SelectedInvoiceItem.ServiceTypeId;
                    
                    var servicesToReturn = _context.Services
                        .Include(s => s.ServiceType)
                        .Include(s => s.Device)
                        .Where(s => s.Device.OwnerClientId == SelectedClient.Id &&
                                    s.ServiceTypeId == serviceTypeId &&
                                    s.IsPaid == false)
                        .AsNoTracking()
                        .ToList();
                    
                    foreach (var serv in servicesToReturn)
                    {
                        if (!_allUnpaidServices.Any(s => s.Id == serv.Id))
                        {
                            _allUnpaidServices.Add(serv);
                        }
                    }
                    
                    FilterUnpaidServices();
                    
                    InvoiceItems.Remove(SelectedInvoiceItem);
                    SelectedInvoiceItem = null;
                }

                RaiseTotalsChanged();
            }
        }

        [RelayCommand]
        private void RemoveUnpaidService()
        {
            if (SelectedService != null && UnpaidServices.Contains(SelectedService))
            {
                UnpaidServices.Remove(SelectedService);
                _allUnpaidServices.Remove(SelectedService);
                SelectedService = null;
                
                OnPropertyChanged(nameof(UnpaidServices));
            }
        }


        private void RaiseTotalsChanged()
        {
            OnPropertyChanged(nameof(TotalNet));
            OnPropertyChanged(nameof(TotalVat));
            OnPropertyChanged(nameof(TotalGross));
            OnPropertyChanged(nameof(CanGenerateBill));
        }

        [RelayCommand]
        private void ShowClientSelection()
        {
            
            IsServiceSelectionVisible = false;
            IsClientSelectionVisible = true;
            InvoiceItems.Clear();
            RaiseTotalsChanged();
        }

        [RelayCommand]
        private void ShowServiceSelection()
        {
            IsServiceSelectionVisible = true;
            IsClientSelectionVisible = false;
        }

        [RelayCommand]
        private void ConfirmClient()
        {
            ShowServiceSelection();
        }
    }

    public partial class InvoiceItem : ObservableObject
    {
        [ObservableProperty]
        private int serviceTypeId;

        [ObservableProperty]
        private string name;

        [ObservableProperty]
        private string type;

        [ObservableProperty]
        private decimal netPrice;

        [ObservableProperty]
        private int quantity;

        public decimal LineNet => Math.Round(NetPrice, 2);
        public decimal LineVat => Math.Round(LineNet * 0.23m, 2);
        public decimal LineGross => Math.Round(LineNet + LineVat, 2);

        internal void OnQuantityChanged()
        {
            OnPropertyChanged(nameof(Quantity));
            OnPropertyChanged(nameof(LineNet));
            OnPropertyChanged(nameof(LineVat));
            OnPropertyChanged(nameof(LineGross));
        }
    }
}
