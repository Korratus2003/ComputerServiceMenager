using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Linq;
using System.Text.RegularExpressions;
using ComputerServiceManager.Database;
using ComputerServiceManager.Utils;

namespace ComputerServiceManager.ViewModels
{
    public partial class AddClientPageViewModel : ViewModelBase
    {
        private readonly AppDbContext _dbContext = new AppDbContext();

        [ObservableProperty]
        private Client _client = new Client();

        [ObservableProperty]
        private string _error = "";

        public AddClientPageViewModel()
        {
            Client = new Client
            {
                Name = "",
                Surname = "",
                PhoneNumber = "",
                Email = "",
                CreatedAt = DateTime.Today.ToUniversalTime()
            };
        }

        [RelayCommand]
        public void Save()
        {
            if (IsValidate())
            {
                Client.Name = Texts.Capitalize(Client.Name);
                Client.Surname = Texts.Capitalize(Client.Surname);
                Client.PhoneNumber = Client.PhoneNumber.Replace(" ", "").Replace("-", "");
                Client.CreatedAt = Client.CreatedAt.ToUniversalTime();

                try
                {
                    _dbContext.Clients.Add(Client);
                    _dbContext.SaveChanges();

                    ViewMediator.Instance.ChangeView(new ClientsPageViewModel());
                }
                catch (Exception e)
                {
                    Error = $"Failed to save client: {e.Message}";
                    Console.WriteLine(e);
                }
            }
        }

        public bool IsValidate()
        {
            if (string.IsNullOrWhiteSpace(Client.Name) || Client.Name.Length < 2)
            {
                Error = "Name is required";
                return false;
            }

            if (string.IsNullOrWhiteSpace(Client.Surname) || Client.Surname.Length < 2)
            {
                Error = "Surname is required";
                return false;
            }

            var digitsOnly = new string(Client.PhoneNumber.Where(char.IsDigit).ToArray());
            if (digitsOnly.Length != 9)
            {
                Error = "Phone number must contain exactly 9 digits.";
                return false;
            }

            var phonePattern = @"^\+?[0-9\s\-]*$";
            if (!Regex.IsMatch(Client.PhoneNumber, phonePattern))
            {
                Error = "Phone number contains invalid characters.";
                return false;
            }

            if (string.IsNullOrWhiteSpace(Client.Email) || !Client.Email.Contains('@'))
            {
                Error = "Valid email is required.";
                return false;
            }

            if (Client.CreatedAt == default)
            {
                Error = "Created date is required";
                return false;
            }

            return true;
        }

        [RelayCommand]
        private void Cancel()
        {
            ViewMediator.Instance.ChangeView(new ClientsPageViewModel());
        }
    }
}
