using System.Linq;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ComputerServiceManager.Database;
using Microsoft.EntityFrameworkCore;

namespace ComputerServiceManager.ViewModels;

public partial class CreateAdminAccountPageViewModel : ViewModelBase
{
    private readonly MainWindowViewModel _mainWindowViewModel;
        
    [ObservableProperty]
    private string _username;

    [ObservableProperty]
    private string _password;
    
    [ObservableProperty]
    private string _rePassword;
        
    [ObservableProperty]
    private string _errorMessage = "";

    [ObservableProperty] 
    private bool _showImportantInfo = false;
        
    public CreateAdminAccountPageViewModel(MainWindowViewModel mainWindowViewModel)
    {
        _mainWindowViewModel = mainWindowViewModel;
    }
        
        
    [RelayCommand]
    private async Task CreateAdminAccount()
    {
        if (!await Validate())
            return;

        using var context = new AppDbContext();
        
        var newUser = new User
        {
            Login = Username,
            PasswordHash = Password,
            Range = UserRange.Admin
        };

        context.Users.Add(newUser);
        await context.SaveChangesAsync();
        ShowImportantInfo = true;
           
    }


    private async Task<bool> Validate()
    {
        ErrorMessage = "";

        if (string.IsNullOrWhiteSpace(Username))
        {
            ErrorMessage = "Username cannot be empty.";
            return false;
        }
        
        using var context = new AppDbContext();
        var usernameExists = await context.Users.AnyAsync(u => u.Login == Username);

        if (usernameExists)
        {
            ErrorMessage = "Username already exists.";
            return false;
        }

        if (string.IsNullOrWhiteSpace(Password) || string.IsNullOrWhiteSpace(RePassword))
        {
            ErrorMessage = "Password and confirmation are required.";
            return false;
        }

        if (Password != RePassword)
        {
            ErrorMessage = "Passwords do not match.";
            return false;
        }

        return true;
    }

    [RelayCommand]
    private void Agree()
    {
        _mainWindowViewModel.CurrentView = new LoginPageViewModel(_mainWindowViewModel);
    }

}