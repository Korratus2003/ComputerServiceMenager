using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text.RegularExpressions;
using ComputerServiceManager.Database;
using ComputerServiceManager.Utils;

namespace ComputerServiceManager.ViewModels
{
    public partial class AddTechnicianPageViewModel : ViewModelBase
    {
        private readonly AppDbContext _dbContext = new AppDbContext();

        [ObservableProperty]
        private Technician _technician = new Technician();

        [ObservableProperty] 
        private string _error = "";

        public AddTechnicianPageViewModel()
        {
            Technician = new Technician
            {
                Name = "",
                Surname = "",
                PhoneNumber = "",
                EmploymentDate = null,
                IsActive = true
            };
        }

        [RelayCommand]
        public void Save()
        {
            if (IsValidate())
            {
                Technician.Name = Texts.Capitalize(Technician.Name);
                Technician.Surname = Texts.Capitalize(Technician.Surname);
                Technician.EmploymentDate = Technician.EmploymentDate.Value.ToUniversalTime();
                
                try
                {
                    _dbContext.Technicians.Add(Technician);
                    _dbContext.SaveChanges();
            
                    ViewMediator.Instance.ChangeView(new TechniciansPageViewModel());
                }
                catch (Exception e)
                {
                    Error = $"Failed to save technician: {e.Message}";
                    Console.WriteLine(e);
                }
                ViewMediator.Instance.ChangeView(new TechniciansPageViewModel());
            }
            
        }



        public bool IsValidate()
        {

            if (string.IsNullOrWhiteSpace(Technician.Name) || Technician.Name.Length < 2)
            {
                Error = "Name is required";
                return false;
            }

            if (string.IsNullOrWhiteSpace(Technician.Surname) || Technician.Surname.Length < 2)
            {
                Error = "Surname is required";
                return false;
            }

            
            var phonePattern = @"^\+?[0-9\s\-]{7,15}$";
            if (string.IsNullOrWhiteSpace(Technician.PhoneNumber))
            {
                Error = "Phone number is required";
                return false;
            }
            else if (!Regex.IsMatch(Technician.PhoneNumber, phonePattern))
            {
                Error = "Phone number is invalid";
                return false;
            }

            if (Technician.EmploymentDate == null)
            {
                Error = "Employment date is required";
                return false;
            }

            return true;
        }

    }
}