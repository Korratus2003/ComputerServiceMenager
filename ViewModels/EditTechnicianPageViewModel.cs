using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Avalonia.Controls;
using Avalonia.Media.Imaging;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ComputerServiceManager.Database;
using ComputerServiceManager.Utils;

namespace ComputerServiceManager.ViewModels;

public partial class EditTechnicianPageViewModel : ViewModelBase
{
    private readonly AppDbContext _dbContext = new AppDbContext();
    
    [ObservableProperty]
    private Technician _technician = new Technician();

    [ObservableProperty] 
    private Bitmap _image;

    [ObservableProperty] 
    private bool _isActive;
    
    [ObservableProperty] 
    private string _error = "";
    
    public EditTechnicianPageViewModel(int technicianId)
    {
        Technician =_dbContext.Technicians.Find(technicianId);
        Image = Technician.Image;
        IsActive = Technician.IsActive;
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
                    _dbContext.Technicians.Update(Technician);
                    _dbContext.SaveChanges();
                    
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

            
        var digitsOnly = new string(Technician.PhoneNumber.Where(char.IsDigit).ToArray());
        if (digitsOnly.Length != 9)
        {
            Error = "Phone number must contain exactly 9 digits.";
            return false;
        }

        var phonePattern = @"^\+?[0-9\s\-]*$";
        if (!Regex.IsMatch(Technician.PhoneNumber, phonePattern))
        {
            Error = "Phone number contains invalid characters.";
            return false;
        }

        if (Technician.EmploymentDate == null)
        {
            Error = "Employment date is required";
            return false;
        }

        return true;
    }

        
        [RelayCommand]
        private void Cancel()
        {
            ViewMediator.Instance.ChangeView(new TechniciansPageViewModel());
        }

        [RelayCommand]
        private async Task AddPhoto()
        {
            string photoPath = null;
            try
            {
                photoPath = await FileDialog.OpenImageDialog();
                Image = new Bitmap(photoPath);
                Technician.ImageUrl = photoPath;
            }
            catch (Exception e)
            {
                
            }
            
        }
        
        
        
}