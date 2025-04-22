using System;
using ComputerServiceManager.ViewModels;

public class ViewMediator
{
    private static readonly ViewMediator _instance = new();
    public static ViewMediator Instance => _instance;

    private ViewModelBase? _activeViewModel;

    public ViewModelBase? ActiveViewModel
    {
        get => _activeViewModel;
        private set
        {
            _activeViewModel = value;
            ViewChanged?.Invoke(value);
        }
    }

    public event Action<ViewModelBase>? ViewChanged;
    
    public void ChangeView(ViewModelBase newView)
    {
        ActiveViewModel = newView;
    }
}