using System;
using System.Linq;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ComputerServiceManager.Services;
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
            using (var context = new AppDbContext())
            {
                context.Database.EnsureCreated();
                
                string login = Username; 
                var findedUser = context.Users
                    .Include(u => u.Technician)
                    .FirstOrDefault(u => u.Login == login);

                if (findedUser == null)
                {
                    LoginErrorMessage = "INWALID USERNAME OR PASSWORD";
                    return;
                }

                if (!findedUser.Password.Equals(Password))
                {
                    LoginErrorMessage = "INWALID USERNAME OR PASSWORD";
                    return;
                }
                
                _mainWindowViewModel.LogedUser = findedUser;
                _mainWindowViewModel.CurrentView = new MainWindowPageViewModel(_mainWindowViewModel);
                
            }
           
        }
    }
}