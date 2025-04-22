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
                
            var magazines = context.Magazines.ToList();
            if (magazines.Count == 0)
                throw new Exception("Brak wpisów w tabeli Magazine. Uruchom najpierw MagazineSeeder.");

            var devices = new List<Device>
            {
                new Device
                {
                    // Urządzenie z kategorii Laptop
                    MagazineId = magazines.First(m => m.Name == "Laptop").Id,
                    Name = "Dell XPS 15",
                    SerialNumber = "LAPTOP-001",
                    Description = "Laptop Dell z wysoką wydajnością."
                },
                new Device
                {
                    // Urządzenie z kategorii Smartphone
                    MagazineId = magazines.First(m => m.Name == "Smartphone").Id,
                    Name = "Samsung Galaxy S21",
                    SerialNumber = "PHONE-001",
                    Description = "Smartphone z flagowym aparatem."
                },
                new Device
                {
                    // Urządzenie z kategorii Tablet
                    MagazineId = magazines.First(m => m.Name == "Tablet").Id,
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