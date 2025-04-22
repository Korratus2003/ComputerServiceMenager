using System;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ComputerServiceManager.Views;

namespace ComputerServiceManager.ViewModels;


public partial class MainWindowViewModel : ViewModelBase
{
    public User LogedUser = null;
    
    [ObservableProperty]
    private ViewModelBase _currentView;

    public MainWindowViewModel()
    {
        _currentView = new LoginPageViewModel(this);
        
        if (LogedUser == null)
        {
            CurrentView = _currentView;
        }
        else{
            CurrentView = new MainPageViewModel(this);
        }
        //comment if you want not login
        CurrentView = new MainPageViewModel(this);
    }
}
