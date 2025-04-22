using System.Collections.ObjectModel;
using System.Linq;
using CommunityToolkit.Mvvm.Input;
using ComputerServiceManager.Database;
using Microsoft.EntityFrameworkCore;

namespace ComputerServiceManager.ViewModels;

public partial class TechniciansPageViewModel : ViewModelBase
{
    private readonly AppDbContext _context = new AppDbContext();
    public ObservableCollection<Technician> Technicians { get; set; } = new();
    
    public TechniciansPageViewModel()
    {
        var technicians = _context.Technicians
            .Where(t => t.IsActive) 
            .OrderBy(t => t.Surname)
            .ThenBy(t => t.Name)
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
        
        System.Console.WriteLine("Adding Technician");
    }
    

}