using System;
using System.Collections.Generic;
using System.Linq;
using ComputerServiceManager.Database;

namespace ComputerServiceManager.Seeders
{
    public static class DeviceSeeder
    {
        public static void Seed(AppDbContext context)
        {
            if (context.Devices.Any())
                return;

            var saleDevices = context.SaleDevices.ToList();
            if (saleDevices.Count == 0)
                throw new Exception("Brak wpisów w tabeli Magazine (SaleDevice). Uruchom najpierw SaleDevicesSeeder.");

            var devices = new List<Device>
            {
                new Device
                {
                    SaleDeviceId = saleDevices.First(m => m.Name.Contains("Dell XPS 15") || m.Name.Contains("MacBook") || m.Name.Contains("ThinkBook") || m.Name.Contains("ROG Zephyrus") || m.Name.Contains("Surface Pro") || m.Name.Contains("MacBook Air") || m.Name.Contains("ThinkBook Plus")).Id,
                    Name = "Dell XPS 15",
                    SerialNumber = "LAPTOP-001",
                    Description = "Laptop Dell z wysoką wydajnością."
                },
                new Device
                {
                    SaleDeviceId = saleDevices.First(m => m.Name.Contains("Samsung Galaxy S21") || m.Name.Contains("Samsung Galaxy Z Fold6") || m.Name.Contains("Samsung Galaxy Watch")).Id,
                    Name = "Samsung Galaxy S21",
                    SerialNumber = "PHONE-001",
                    Description = "Smartphone z flagowym aparatem."
                },
                new Device
                {
                    SaleDeviceId = saleDevices.First(m => m.Name.Contains("iPad Pro") || m.Name.Contains("Tablet") || m.Name.Contains("Surface Pro")).Id,
                    Name = "Apple iPad Pro",
                    SerialNumber = "TABLET-001",
                    Description = "Tablet idealny do pracy i rozrywki."
                }
            };

            context.Devices.AddRange(devices);
            context.SaveChanges();
        }
    }
}
