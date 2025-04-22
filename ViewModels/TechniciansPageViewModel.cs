using System.Collections.ObjectModel;
using System.Linq;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ComputerServiceManager.Database;
using Microsoft.EntityFrameworkCore;

namespace ComputerServiceManager.ViewModels;

public partial class TechniciansPageViewModel : ViewModelBase
{
    private readonly AppDbContext _context = new AppDbContext();
    public ObservableCollection<Technician> Technicians { get; set; } = new();
    
    private Technician _selectedTechnician;
    public Technician SelectedTechnician
    {
        get => _selectedTechnician;
        set
        {
            if (SetProperty(ref _selectedTechnician, value))
            {
                EditTechnician(_selectedTechnician);
            }
        }
    }
    public TechniciansPageViewModel()
    {
        var technicians = _context.Technicians
            .OrderBy(t => t.EmploymentDate)
            .ThenBy(t => t.Name)
            .ThenBy(t=>t.Surname)
            .ToList();

        Technicians.Clear();
        foreach (var technician in technicians)
        {
            Technicians.Add(technician);
        }
    }
    
    [RelayCommand]
    private void AddTechnician()
    {
        ViewMediator.Instance.ChangeView(new AddTechnicianPageViewModel());
        System.Console.WriteLine("Adding Technician");
    }

    [RelayCommand]
    private void EditTechnician(Technician technician)
    {
        ViewMediator.Instance.ChangeView(new EditTechnicianPageViewModel(technician));
    }

}