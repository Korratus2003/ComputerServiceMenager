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
            LoadAvailableServiceTypes();
            LoadClients();
            
            IsServiceSelectionVisible = false;
            IsClientSelectionVisible = true;
        }

        private ObservableCollection<ServiceType> _allAvailableServiceTypes = new();

        [ObservableProperty]
        private ObservableCollection<ServiceType> availableServiceTypes = new();

        [ObservableProperty]
        private ServiceType selectedServiceType;

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
                    FilterAvailableServices();
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

        private void LoadAvailableServiceTypes()
        {
            var services = _context.ServiceTypes
                .OrderBy(s => s.Name)
                .AsNoTracking()
                .ToList();

            _allAvailableServiceTypes = new ObservableCollection<ServiceType>(services);
            AvailableServiceTypes = new ObservableCollection<ServiceType>(_allAvailableServiceTypes);
        }

        private void LoadClients()
        {
            var clientsFromDb = _context.Clients
                .OrderBy(c => c.Name)
                .AsNoTracking()
                .ToList();

            Clients = new ObservableCollection<Client>(clientsFromDb);
            FilteredClients = new ObservableCollection<Client>(clientsFromDb);
        }

        private void FilterAvailableServices()
        {
            if (string.IsNullOrWhiteSpace(ServiceSearchText))
            {
                AvailableServiceTypes = new ObservableCollection<ServiceType>(_allAvailableServiceTypes);
            }
            else
            {
                var filtered = _allAvailableServiceTypes
                    .Where(s => s.Name.Contains(ServiceSearchText, StringComparison.OrdinalIgnoreCase))
                    .ToList();

                AvailableServiceTypes = new ObservableCollection<ServiceType>(filtered);
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
            if (SelectedServiceType == null)
                return;

            var existing = InvoiceItems.FirstOrDefault(x => x.ServiceTypeId == SelectedServiceType.Id);
            if (existing != null)
            {
                existing.Quantity++;
                existing.OnQuantityChanged();
            }
            else
            {
                var newItem = new InvoiceItem
                {
                    ServiceTypeId = SelectedServiceType.Id,
                    Name = SelectedServiceType.Name,
                    Type = "Usługa",
                    NetPrice = SelectedServiceType.DefaultPrice,
                    Quantity = 1
                };
                InvoiceItems.Add(newItem);
            }

            RaiseTotalsChanged();
        }
        
        [RelayCommand]  
        private void GenerateBill()
        {
            var produkty = InvoiceItems.Select(item => new Produkt
            {
                Nazwa = item.Name,
                Ilosc = item.Quantity,
                Cena = item.LineGross,
                StawkaVAT = 'A'
            }).ToList();

            GenerateBillUtility.DrukujParagon(produkty);

            InvoiceItems.Clear();
            RaiseTotalsChanged();
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
                    InvoiceItems.Remove(SelectedInvoiceItem);
                    SelectedInvoiceItem = null;
                }

                RaiseTotalsChanged();
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
