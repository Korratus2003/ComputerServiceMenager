using System;
using System.Collections.ObjectModel;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Media;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace ComputerServiceManager.ViewModels;

public partial class MainPageViewModel : ViewModelBase
{
    private readonly MainWindowViewModel _mainWindowViewModel;

    [ObservableProperty]
    private bool _isPaneOpen = false;

    [ObservableProperty]
    private ViewModelBase _activeViewModel;

    public ObservableCollection<ListItemTemplate> Items { get; } = new()
    {
        new ListItemTemplate(typeof(TechniciansPageViewModel), "people_settings_regular"),
        new ListItemTemplate(typeof(DevicesPageViewModel), "phone_tablet_regular"),
        new ListItemTemplate(typeof(ClientsPageViewModel), "people_community_regular"),
        new ListItemTemplate(typeof(ServicePageViewModel), "wrench_regular"),
        new ListItemTemplate(typeof(SalesPageViewModel), "money_regular"),
        new ListItemTemplate(typeof(MagazinePageViewModel), "vehicle_truck_regular"),
        new ListItemTemplate(typeof(SettingsPageViewModel), "settings_regular")
    };

    [ObservableProperty]
    private ListItemTemplate _selectedListItem;

    partial void OnSelectedListItemChanged(ListItemTemplate? value)
    {
        if (value is null) return;
        var instance = Activator.CreateInstance(value.ModelType);
        if (instance is null) return;
        ViewMediator.Instance.ChangeView((ViewModelBase)instance);
    }

    [RelayCommand]
    private void TriggerPane()
    {
        IsPaneOpen = !IsPaneOpen;
    }

    [RelayCommand]
    private void Logout()
    {
        _mainWindowViewModel.LogedUser = null;
        _mainWindowViewModel.CurrentView = new LoginPageViewModel(_mainWindowViewModel);
        ViewMediator.Instance.ChangeView(new TechniciansPageViewModel());
    }

    public MainPageViewModel(MainWindowViewModel mainWindowViewModel)
    {
        _mainWindowViewModel = mainWindowViewModel;
        ViewMediator.Instance.ViewChanged += OnViewChanged;
        ActiveViewModel = ViewMediator.Instance.ActiveViewModel;
        SelectedListItem = Items[0];
        
    }

    private void OnViewChanged(ViewModelBase newView)
    {
        ActiveViewModel = newView;
    }
}




public partial class ListItemTemplate
{
    public ListItemTemplate(Type type, String iconKey)
    {
        ModelType = type;
        Label = type.Name.Replace("PageViewModel", "").ToUpper();
        Application.Current!.TryFindResource(iconKey, out var res);
        ListItemIcon = (StreamGeometry)res!;
    }

    public String Label { get; }
    public Type ModelType { get;  }
    public StreamGeometry ListItemIcon { get;  }
}


