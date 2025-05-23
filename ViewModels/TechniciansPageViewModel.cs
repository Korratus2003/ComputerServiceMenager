using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ComputerServiceManager.Database;
using Microsoft.EntityFrameworkCore;

namespace ComputerServiceManager.ViewModels
{
    public partial class TechniciansPageViewModel : ViewModelBase
    {
        private readonly AppDbContext _context = new AppDbContext();
        private List<Technician> _allTechnicians;

        [ObservableProperty]
        private ObservableCollection<Technician> technicians = new();
        
        [ObservableProperty]
        private Technician selectedTechnician;

        partial void OnSelectedTechnicianChanged(Technician value)
        {
            if (value is not null)
            {
                EditTechnician(value.Id);
            }
        }


        [ObservableProperty]
        private string searchText = string.Empty;

        [ObservableProperty]
        private bool showActive = true;

        [ObservableProperty]
        private bool showInactive = true;

        public TechniciansPageViewModel()
        {
            LoadTechnicians();
            ApplyFilters();
            
            PropertyChanged += (s, e) =>
            {
                if (e.PropertyName is nameof(SearchText)
                    or nameof(ShowActive)
                    or nameof(ShowInactive))
                {
                    ApplyFilters();
                }
            };
        }

        private void LoadTechnicians()
        {
            _allTechnicians = _context.Technicians
                .OrderBy(t => t.EmploymentDate)
                .ThenBy(t => t.Name)
                .ThenBy(t => t.Surname)
                .ToList();
            
            foreach (var technician in _allTechnicians)
            {
                if (string.IsNullOrWhiteSpace(technician.ImageUrl))
                {
                    technician.ImageUrl = "avares://ComputerServiceManager/Assets/no-user.png";
                }
            }
        }

        private void ApplyFilters()
        {
            var filtered = _allTechnicians.AsEnumerable();

            // Filtr wg statusu aktywności
            filtered = filtered.Where(t =>
                (ShowActive && t.IsActive)
                || (ShowInactive && !t.IsActive));

            // Wyszukiwanie po imieniu, nazwisku lub telefonie
            if (!string.IsNullOrWhiteSpace(SearchText))
            {
                var lower = SearchText.Trim().ToLower();
                filtered = filtered.Where(t =>
                    t.Name.ToLower().Contains(lower)
                    || t.Surname.ToLower().Contains(lower)
                    || (t.PhoneNumber?.ToLower().Contains(lower) ?? false));
            }

            Technicians.Clear();
            foreach (var tech in filtered)
                Technicians.Add(tech);
        }

        [RelayCommand]
        private void ClearFilters()
        {
            SearchText = string.Empty;
            ShowActive = true;
            ShowInactive = true;
        }

        [RelayCommand]
        private void AddTechnician()
        {
            ViewMediator.Instance.ChangeView(new AddTechnicianPageViewModel());
        }

        [RelayCommand]
        private void EditTechnician(int technicianId)
        {
            ViewMediator.Instance.ChangeView(new EditTechnicianPageViewModel(technicianId));
        }
    }
}
