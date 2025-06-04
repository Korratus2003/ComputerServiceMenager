using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using ComputerServiceManager.Database;
using ComputerServiceManager.Utils;

namespace ComputerServiceManager.ViewModels
{
    public partial class AddDevicePageViewModel : ViewModelBase
    {
        private readonly AppDbContext _dbContext = new AppDbContext();

        [ObservableProperty]
        private Device _device = new Device();

        [ObservableProperty]
        private string _error = "";

        [ObservableProperty]
        private ObservableCollection<Client> _clients;

        [ObservableProperty]
        private Client _selectedClient;

        public AddDevicePageViewModel()
        {
            // Załaduj listę klientów do wyboru
            Clients = new ObservableCollection<Client>(_dbContext.Clients.ToList());

            Device = new Device
            {
                Name = "",
                SerialNumber = "",
                Description = "",
                AddedAt = DateTime.UtcNow
            };
        }

        [RelayCommand]
        public void Save()
        {
            if (SelectedClient == null)
            {
                Error = "You must select an owner client.";
                return;
            }

            if (IsValid())
            {
                Device.Name = Texts.Capitalize(Device.Name);
                Device.SerialNumber = Device.SerialNumber.ToUpper().Replace(" ", "");
                Device.AddedAt = Device.AddedAt;

                // Ustaw właściciela urządzenia
                Device.OwnerClientId = SelectedClient.Id;

                try
                {
                    _dbContext.Devices.Add(Device);
                    _dbContext.SaveChanges();

                    ViewMediator.Instance.ChangeView(new DevicesPageViewModel());
                }
                catch (Exception e)
                {
                    Error = $"Failed to save device: {e.Message}";
                    Console.WriteLine(e);
                }
            }
        }

        public bool IsValid()
        {
            if (string.IsNullOrWhiteSpace(Device.Name) || Device.Name.Length < 2)
            {
                Error = "Device name is required and must be at least 2 characters.";
                return false;
            }

            if (string.IsNullOrWhiteSpace(Device.SerialNumber) || Device.SerialNumber.Length < 4)
            {
                Error = "Serial number is required and must be at least 4 characters.";
                return false;
            }

            if (string.IsNullOrWhiteSpace(Device.Description))
            {
                Error = "Description is required.";
                return false;
            }

            if (Device.AddedAt == default)
            {
                Error = "Added date is required.";
                return false;
            }

            return true;
        }

        [RelayCommand]
        private void Cancel()
        {
            ViewMediator.Instance.ChangeView(new DevicesPageViewModel());
        }
    }
}
