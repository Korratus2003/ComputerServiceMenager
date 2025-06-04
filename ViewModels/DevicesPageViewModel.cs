using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ComputerServiceManager.Database;
using Microsoft.EntityFrameworkCore;

namespace ComputerServiceManager.ViewModels
{
    public partial class DevicesPageViewModel : ViewModelBase
    {
        private readonly AppDbContext _dbContext = new AppDbContext();
        private List<Device> _allDevices = new();

        [ObservableProperty]
        private ObservableCollection<Device> devices = new();

        [ObservableProperty]
        private Device selectedDevice;

        [ObservableProperty]
        private string searchText = string.Empty;

        public DevicesPageViewModel()
        {
            LoadDevices();
            ApplyFilters();

            PropertyChanged += (s, e) =>
            {
                if (e.PropertyName == nameof(SearchText))
                {
                    ApplyFilters();
                }
                else if (e.PropertyName == nameof(SelectedDevice) && SelectedDevice != null)
                {
                    // Automatycznie wywołaj edycję po wyborze, jeśli chcesz
                    EditDevice(SelectedDevice.Id);
                }
            };
        }

        private void LoadDevices()
        {
            _allDevices = _dbContext.Devices
                .Include(d => d.OwnerClient)
                .OrderBy(d => d.Name)
                .ToList();
        }

        private void ApplyFilters()
        {
            var filtered = _allDevices.AsEnumerable();

            if (!string.IsNullOrWhiteSpace(SearchText))
            {
                var lower = SearchText.Trim().ToLower();
                filtered = filtered.Where(d =>
                    d.Name.ToLower().Contains(lower)
                    || (d.SerialNumber?.ToLower().Contains(lower) ?? false)
                    || (d.OwnerClient.Name.ToLower().Contains(lower))
                    || (d.OwnerClient.Surname.ToLower().Contains(lower))
                );
            }

            Devices.Clear();
            foreach (var device in filtered)
            {
                Devices.Add(device);
            }
        }

        [RelayCommand]
        private void ClearFilters()
        {
            SearchText = string.Empty;
            ApplyFilters();
        }

        [RelayCommand]
        private void AddDevice()
        {
            ViewMediator.Instance.ChangeView(new AddDevicePageViewModel());
        }

        [RelayCommand]
        private void EditDevice(int deviceId)
        {
            ViewMediator.Instance.ChangeView(new EditDevicePageViewModel(deviceId));
        }
    }
}
