using System;
using System.Linq;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ComputerServiceManager.Database;
using Microsoft.EntityFrameworkCore;

namespace ComputerServiceManager.ViewModels
{
    public partial class LoginPageViewModel : ViewModelBase
    {
        private readonly MainWindowViewModel _mainWindowViewModel;
        
        [ObservableProperty]
        private string _username;

        [ObservableProperty]
        private string _password;
        
        [ObservableProperty]
        private string _loginErrorMessage = "";
        
        public LoginPageViewModel(MainWindowViewModel mainWindowViewModel)
        {
            _mainWindowViewModel = mainWindowViewModel;
        }
        
        
        [RelayCommand]
        private async Task Login()
        {
            using var context = new AppDbContext();

            if (string.IsNullOrWhiteSpace(Username) || string.IsNullOrWhiteSpace(Password))
            {
                LoginErrorMessage = "Username and password cannot be empty.";
                return;
            }

            var foundUser = await context.Users
                .Include(u => u.Technician)
                .FirstOrDefaultAsync(u => u.Login == Username);

            if (foundUser == null || !BCrypt.Net.BCrypt.Verify(Password, foundUser.PasswordHash))
            {
                LoginErrorMessage = "Invalid username or password.";
                return;
            }

            _mainWindowViewModel.LogedUser = foundUser;
            _mainWindowViewModel.CurrentView = new MainPageViewModel(_mainWindowViewModel);
        }

    }
}