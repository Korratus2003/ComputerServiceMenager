using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using ComputerServiceManager.Services;
using Microsoft.EntityFrameworkCore;

namespace ComputerServiceManager.ViewModels
{
    public class DevicesPageViewModel : ViewModelBase

    {
        public ObservableCollection<DeviceWithTechnician> Items { get; }
        
        public ObservableCollection<DeviceWithTechnician> FilteredItems { get; }

        private string _searchText;
        public string SearchText
        {
            get => _searchText;
            set
            {
                if(_searchText != value)
                {
                    _searchText = value;
                    OnPropertyChanged(nameof(SearchText));
                    ApplyFilter();
                }
            }
        }

        private bool _filterByDeviceName = true;
        public bool FilterByDeviceName
        {
            get => _filterByDeviceName;
            set
            {
                if(_filterByDeviceName != value)
                {
                    _filterByDeviceName = value;
                    OnPropertyChanged(nameof(FilterByDeviceName));
                    ApplyFilter();
                }
            }
        }

        private bool _filterByType = true;
        public bool FilterByType
        {
            get => _filterByType;
            set
            {
                if(_filterByType != value)
                {
                    _filterByType = value;
                    OnPropertyChanged(nameof(FilterByType));
                    ApplyFilter();
                }
            }
        }

        private bool _filterByClient = true;
        public bool FilterByClient
        {
            get => _filterByClient;
            set
            {
                if(_filterByClient != value)
                {
                    _filterByClient = value;
                    OnPropertyChanged(nameof(FilterByClient));
                    ApplyFilter();
                }
            }
        }

        public DevicesPageViewModel()
        {
            using (var context = new AppDbContext())
            {
                var devicesWithTechnicians = context.Devices
                    .Include(d => d.Client)
                    .Select(d => new DeviceWithTechnician
                    {
                        Device = d,
                        LastServiceTechnician = context.Services
                            .Where(s => s.DeviceId == d.Id)
                            .OrderByDescending(s => s.ServiceDate)
                            .Select(s => s.Technician)
                            .FirstOrDefault()
                    })
                    .ToList();

                Items = new ObservableCollection<DeviceWithTechnician>(devicesWithTechnicians);
            }
            FilteredItems = new ObservableCollection<DeviceWithTechnician>(Items);
        }

private void ApplyFilter()
{
    var searchTokens = !string.IsNullOrWhiteSpace(SearchText)
        ? SearchText.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries)
        : Array.Empty<string>();

    var filtered = Items.Where(item =>
    {
        if (string.IsNullOrWhiteSpace(SearchText))
            return true;

        bool matches = FilterByDeviceName &&
                       !string.IsNullOrEmpty(item.Device.Name) &&
                       item.Device.Name.IndexOf(SearchText, StringComparison.OrdinalIgnoreCase) >= 0;

        
        if (FilterByType &&
            !string.IsNullOrEmpty(item.Device.Type) &&
            item.Device.Type.IndexOf(SearchText, StringComparison.OrdinalIgnoreCase) >= 0)
        {
            matches = true;
        }
        
        if (FilterByClient && item.Device.Client != null)
        {
            var client = item.Device.Client;
            
            if (searchTokens.Length >= 1)
            {
                string firstToken = searchTokens[0];
                string secondToken = searchTokens.Length >= 2 ? searchTokens[1] : "";

                if (!string.IsNullOrWhiteSpace(secondToken))
                {
                    bool firstNameMatch = client.Name != null &&
                                          client.Name.IndexOf(firstToken, StringComparison.OrdinalIgnoreCase) >= 0;
                    bool surnameMatch = client.Surname != null &&
                                        client.Surname.IndexOf(secondToken, StringComparison.OrdinalIgnoreCase) >= 0;
                    if (firstNameMatch && surnameMatch)
                        matches = true;
                }
                else
                {
                    bool clientMatch = (client.Name != null &&
                                        client.Name.IndexOf(firstToken, StringComparison.OrdinalIgnoreCase) >= 0) ||
                                       (client.Surname != null &&
                                        client.Surname.IndexOf(firstToken, StringComparison.OrdinalIgnoreCase) >= 0);
                    if (clientMatch)
                        matches = true;
                }
            }
        }

        return matches;
    }).ToList();
    
    FilteredItems.Clear();
    foreach (var f in filtered)
    {
        FilteredItems.Add(f);
    }
}



        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    public class DeviceWithTechnician
    {
        public Device Device { get; set; }
        public Technician? LastServiceTechnician { get; set; }
    }
}
