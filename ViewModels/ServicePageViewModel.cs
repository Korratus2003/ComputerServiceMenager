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
                .AsNoTracking() // Opcjonalnie, jeśli nie planujesz edycji
                .ToList();
        }

        private void ApplyFilters()
        {
            var filtered = _allServices.AsEnumerable();

            if (!string.IsNullOrWhiteSpace(SearchText))
            {
                var lower = SearchText.Trim().ToLowerInvariant();
                filtered = filtered.Where(s =>
                    (s.Device?.Name?.ToLowerInvariant().Contains(lower) ?? false) ||
                    (s.Technician?.Name?.ToLowerInvariant().Contains(lower) ?? false) ||
                    (s.ServiceType?.Name?.ToLowerInvariant().Contains(lower) ?? false) ||
                    (s.Description?.ToLowerInvariant().Contains(lower) ?? false)
                );
            }

            Services.Clear();
            foreach (var service in filtered)
            {
                Services.Add(service);
            }
        }

        [RelayCommand]
        private void ClearFilters()
        {
            SearchText = string.Empty;
        }
    }
}
