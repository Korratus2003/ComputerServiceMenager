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
            
            IsSellMode = true;
            IsServiceMode = false;

            LoadAvailableMagazines();
        }

        [ObservableProperty]
        private ObservableCollection<InvoiceItem> invoiceItems = new();

        [ObservableProperty]
        private ObservableCollection<SaleDevice> availableSaleDevices = new();

        [ObservableProperty]
        private SaleDevice selectedSaleDevice;

        [ObservableProperty]
        private bool isSellMode;

        [ObservableProperty]
        private bool isServiceMode;

        [ObservableProperty]
        private string serviceDescription;

        [ObservableProperty]
        private string servicePrice;
        
        [ObservableProperty]
        private string searchQuery;
        
        public decimal TotalNet => InvoiceItems.Sum(i => i.LineNet);
        
        public decimal TotalVat => InvoiceItems.Sum(i => i.LineVat);
        
        public decimal TotalGross => InvoiceItems.Sum(i => i.LineGross * i.Quantity);

        private void LoadAvailableMagazines()
        {
            var devices = _context.SaleDevices
                .Where(d => d.Quantity > 0)
                .OrderByDescending(d => d.Id)
                .Take(20)
                .AsNoTracking()
                .ToList();

            AvailableSaleDevices.Clear();
            foreach (var device in devices)
                AvailableSaleDevices.Add(device);
        }

        public string AddButtonText => IsSellMode ? "Dodaj urządzenie" : "Dodaj usługę";

        [RelayCommand]
        private void SwitchToSell()
        {
            IsSellMode = true;
            IsServiceMode = false;
        }

        [RelayCommand]
        private void SwitchToService()
        {
            IsSellMode = false;
            IsServiceMode = true;
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
            OnPropertyChanged(nameof(TotalNet));
            OnPropertyChanged(nameof(TotalVat));
            OnPropertyChanged(nameof(TotalGross));
        }

        [RelayCommand]
        private void Search()
        {
            if (string.IsNullOrWhiteSpace(SearchQuery))
            {
                LoadAvailableMagazines();
            }
            else
            {
                var filtered = _context.SaleDevices
                    .Where(d => d.Quantity > 0 && d.Name.ToLower().Contains(SearchQuery.ToLower()))
                    .OrderBy(d => d.Name)
                    .AsNoTracking()
                    .ToList();

                AvailableSaleDevices.Clear();
                foreach (var device in filtered)
                    AvailableSaleDevices.Add(device);
            }
        }

        [RelayCommand]
        private void AddInvoiceItem()
        {
            if (SelectedSaleDevice == null)
                return;
            
            var existing = InvoiceItems.FirstOrDefault(x => x.DeviceId == SelectedSaleDevice.Id);
            if (existing != null)
            {
                existing.Quantity++;
                existing.OnQuantityChanged();
            }
            else
            {
                var newItem = new InvoiceItem
                {
                    DeviceId = SelectedSaleDevice.Id,
                    Name = SelectedSaleDevice.Name,
                    Type = SelectedSaleDevice.Category,
                    NetPrice = SelectedSaleDevice.DefaultPrice,
                    Quantity = 1
                };
                InvoiceItems.Add(newItem);
            }
            
            OnPropertyChanged(nameof(TotalNet));
            OnPropertyChanged(nameof(TotalVat));
            OnPropertyChanged(nameof(TotalGross));
        }
    }

    public partial class InvoiceItem : ObservableObject
    {
        [ObservableProperty]
        private int deviceId;

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
