using System;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ComputerServiceManager.Models;
using ComputerServiceManager.Views;

namespace ComputerServiceManager.ViewModels;


public partial class MainWindowViewModel : ViewModelBase
{
    public UserModel LogedUser = null;
    
    [ObservableProperty]
    private ViewModelBase _currentView;

    public MainWindowViewModel()
    {
        _currentView = new LoginPageViewModel(this);
        if (LogedUser == null)
        {
            CurrentView = new LoginPageViewModel(this);
        }
        else{
            CurrentView = new MainWindowPageViewModel();
        }
    }
}
