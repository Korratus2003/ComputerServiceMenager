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
        }

        [ObservableProperty]
        private ObservableCollection<ServiceType> availableServiceTypes = new();

        [ObservableProperty]
        private ServiceType selectedServiceType;

        [ObservableProperty]
        private ObservableCollection<InvoiceItem> invoiceItems = new();

        public decimal TotalNet => InvoiceItems.Sum(i => i.LineNet);
        public decimal TotalVat => InvoiceItems.Sum(i => i.LineVat);
        public decimal TotalGross => InvoiceItems.Sum(i => i.LineGross * i.Quantity);

        private void LoadAvailableServiceTypes()
        {
            var services = _context.ServiceTypes
                .OrderBy(s => s.Name)
                .AsNoTracking()
                .ToList();

            AvailableServiceTypes.Clear();
            foreach (var service in services)
                AvailableServiceTypes.Add(service);
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

            OnPropertyChanged(nameof(TotalNet));
            OnPropertyChanged(nameof(TotalVat));
            OnPropertyChanged(nameof(TotalGross));
        }

        [RelayCommand]
        private void GenerateBill()
        {
            var produkty = InvoiceItems.Select(item => new Produkt
            {
                Nazwa = item.Name,
                Ilosc = item.Quantity,
                Cena = item.LineGross,
                StawkaVAT = 'A' // Możesz zmienić, jeśli masz różne stawki
            }).ToList();

            GenerateBillUtility.DrukujParagon(produkty);

            InvoiceItems.Clear();
            OnPropertyChanged(nameof(TotalNet));
            OnPropertyChanged(nameof(TotalVat));
            OnPropertyChanged(nameof(TotalGross));
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
