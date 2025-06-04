using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using ComputerServiceManager.Database;

namespace ComputerServiceManager.ViewModels
{
    public partial class EditDevicePageViewModel : ViewModelBase
    {
        private readonly AppDbContext _dbContext = new AppDbContext();

        [ObservableProperty]
        private Device _device;

        [ObservableProperty]
        private ObservableCollection<Client> _clients;

        [ObservableProperty]
        private Client _selectedClient;

        [ObservableProperty]
        private string _error = "";

        public EditDevicePageViewModel(int deviceId)
        {
            // Załaduj urządzenie po ID
            Device = _dbContext.Devices.FirstOrDefault(d => d.Id == deviceId);

            if (Device == null)
            {
                Error = "Device not found.";
                Clients = new ObservableCollection<Client>();
                return;
            }

            // Załaduj listę klientów
            Clients = new ObservableCollection<Client>(_dbContext.Clients.ToList());

            // Ustaw wybranego klienta właściciela urządzenia (jeśli jest)
            SelectedClient = Clients.FirstOrDefault(c => c.Id == Device.OwnerClientId);
        }

        [RelayCommand]
        public void Save()
        {
            if (!IsValid())
                return;

            try
            {
                // Przypisz OwnerClientId z wybranego klienta
                Device.OwnerClientId = (int)SelectedClient?.Id;

                _dbContext.Devices.Update(Device);
                _dbContext.SaveChanges();

                // Przejście do widoku urządzeń po zapisaniu
                ViewMediator.Instance.ChangeView(new DevicesPageViewModel());
            }
            catch (Exception e)
            {
                Error = $"Failed to save device: {e.Message}";
            }
        }

        private bool IsValid()
        {
            if (string.IsNullOrWhiteSpace(Device.Name))
            {
                Error = "Name is required.";
                return false;
            }

            if (string.IsNullOrWhiteSpace(Device.SerialNumber))
            {
                Error = "Serial Number is required.";
                return false;
            }

            if (SelectedClient == null)
            {
                Error = "Please select an owner client.";
                return false;
            }

            Error = "";
            return true;
        }

        [RelayCommand]
        private void Cancel()
        {
            ViewMediator.Instance.ChangeView(new DevicesPageViewModel());
        }
    }
}
