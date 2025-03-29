using System;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ComputerServiceManager.Models;

namespace ComputerServiceManager.ViewModels
{
    public partial class LoginPageViewModel : ViewModelBase
    {
        private readonly MainWindowViewModel _mainWindowViewModel;
        
        public LoginPageViewModel(MainWindowViewModel mainWindowViewModel)
        {
            _mainWindowViewModel = mainWindowViewModel;
        }
        
        [RelayCommand]
        private void Login()
        {
            if (_mainWindowViewModel != null)
            {
                _mainWindowViewModel.LogedUser = new UserModel();
                _mainWindowViewModel.CurrentView = new MainWindowPageViewModel();
            }
        }
    }
}