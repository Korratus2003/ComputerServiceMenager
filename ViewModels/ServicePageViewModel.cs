using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ComputerServiceManager.Database;
using Microsoft.EntityFrameworkCore;

namespace ComputerServiceManager.ViewModels
{
    public partial class ServicePageViewModel : ViewModelBase
    {
        private readonly AppDbContext _context = new();

        private List<Service> _allServices = new();

        [ObservableProperty]
        private ObservableCollection<Service> services = new();

        [ObservableProperty]
        private string searchText = string.Empty;

        public ServicePageViewModel()
        {
            LoadServices();
            ApplyFilters();

            PropertyChanged += (s, e) =>
            {
                if (e.PropertyName == nameof(SearchText))
                {
                    ApplyFilters();
                }
            };
        }

        private void LoadServices()
        {
            _allServices = _context.Services
                .Include(s => s.Device)
                .Include(s => s.Technician)
                .Include(s => s.ServiceType)
                .AsNoTracking()
                .ToList();
        }

        private void ApplyFilters()
        {
            var filtered = FilterServices(SearchText);
            Services.Clear();
            foreach (var service in filtered)
            {
                Services.Add(service);
            }
        }

        private IEnumerable<Service> FilterServices(string text)
        {
            if (string.IsNullOrWhiteSpace(text))
                return _allServices;

            var lower = text.Trim().ToLowerInvariant();

            return _allServices.Where(s =>
                (s.Device?.Name?.ToLowerInvariant().Contains(lower) ?? false) ||
                (s.Technician?.Name?.ToLowerInvariant().Contains(lower) ?? false) ||
                (s.ServiceType?.Name?.ToLowerInvariant().Contains(lower) ?? false) ||
                (s.Description?.ToLowerInvariant().Contains(lower) ?? false)
            );
        }

        [RelayCommand]
        private void ClearFilters()
        {
            SearchText = string.Empty;
        }

        [RelayCommand]
        private void EditService()
        {
            System.Diagnostics.Debug.WriteLine("Edytuj");
        }
    }
}
