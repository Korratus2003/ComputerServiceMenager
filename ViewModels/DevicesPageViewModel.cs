using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using ComputerServiceManager.Services;
using Microsoft.EntityFrameworkCore;

namespace ComputerServiceManager.ViewModels
{
    public class DevicesPageViewModel : ViewModelBase
    {
        List<Device> _devices = new();
        public ObservableCollection<DeviceWithTechnician> Items { get; } 

        public DevicesPageViewModel()
        {
            using (var context = new AppDbContext())
            {
                var devicesWithTechnicians = context.Devices
                    .Include(d => d.Client)  
                    .Select(d => new DeviceWithTechnician
                    {
                        Device = d,
                        LastServiceTechnician = context.Services
                            .Where(s => s.DeviceId == d.Id) 
                            .OrderByDescending(s => s.ServiceDate) 
                            .Select(s => s.Technician) 
                            .FirstOrDefault() 
                    })
                    .ToList();

                Items = new ObservableCollection<DeviceWithTechnician>(devicesWithTechnicians);
            }
        }
    }

    public class DeviceWithTechnician
    {
        public Device Device { get; set; }
        public Technician? LastServiceTechnician { get; set; }
    }
}