using System;
using System.Linq;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ComputerServiceManager.Database;
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

        using (var context = new AppDbContext())
        {
            if (!context.Users.Any())
            {
                CurrentView = new CreateAdminAccountPageViewModel(this);
                return;
            }
        }
        
        if (LogedUser == null)
        {
            CurrentView = _currentView;
        }
        else{
            CurrentView = new MainPageViewModel(this);
        }
        //uncomment if you want not login
        CurrentView = new MainPageViewModel(this);
    }
}
