using System;
using System.Linq;
using System.Text.RegularExpressions;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ComputerServiceManager.Database;
using ComputerServiceManager.Utils;

namespace ComputerServiceManager.ViewModels;

public partial class EditClientPageViewModel : ViewModelBase
{
    private readonly AppDbContext _dbContext = new AppDbContext();

    [ObservableProperty]
    private Client _client = new Client();

    [ObservableProperty]
    private string _error = "";

    public EditClientPageViewModel(int clientId)
    {
        Client = _dbContext.Clients.Find(clientId);
    }

    [RelayCommand]
    public void Save()
    {
        if (IsValidate())
        {
            Client.Name = Texts.Capitalize(Client.Name);
            Client.Surname = Texts.Capitalize(Client.Surname);

            try
            {
                _dbContext.Clients.Update(Client);
                _dbContext.SaveChanges();
            }
            catch (Exception e)
            {
                Error = $"Failed to save client: {e.Message}";
                Console.WriteLine(e);
                return;
            }

            ViewMediator.Instance.ChangeView(new ClientsPageViewModel());
        }
    }

    private bool IsValidate()
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

        var digitsOnly = new string(Client.PhoneNumber?.Where(char.IsDigit).ToArray() ?? Array.Empty<char>());
        if (digitsOnly.Length != 9)
        {
            Error = "Phone number must contain exactly 9 digits.";
            return false;
        }

        var phonePattern = @"^\+?[0-9\s\-]*$";
        if (!Regex.IsMatch(Client.PhoneNumber ?? "", phonePattern))
        {
            Error = "Phone number contains invalid characters.";
            return false;
        }

        if (!string.IsNullOrWhiteSpace(Client.Email) && !Regex.IsMatch(Client.Email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
        {
            Error = "Invalid email format.";
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
