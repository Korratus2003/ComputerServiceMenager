using System;
using System.Collections.Generic;
using System.Linq;
using ComputerServiceManager.Database;

namespace ComputerServiceManager.Seeders
{
    public static class ServiceSeeder
    {
        public static void Seed(AppDbContext context)
        {
            if (context.Services.Any())
                return;
                
            var clients = context.Clients.ToList();
            var devices = context.Devices.ToList();
            var technicians = context.Technicians.ToList();
            var magazinePrices = context.Magazines.ToDictionary(m => m.Id, m => m.DefaultPrice);

            if (!clients.Any() || !devices.Any() || !technicians.Any())
            {
                throw new Exception("Brak klienta, urządzenia lub technika. Upewnij się, że poprzednie seedery działają poprawnie.");
            }

            var services = new List<Service>
            {
                new Service
                {
                    Type = ServiceType.Service,
                    DeviceId = devices.First().Id,
                    ClientId = clients.First().Id,
                    TechnicianId = technicians.First().Id,
                    Description = "Naprawa laptopa Dell XPS 15",
                    Price = magazinePrices[devices.First().MagazineId],
                    Status = ServiceStatus.Pending,
                    Date = DateTime.UtcNow.AddDays(-1)
                },
                new Service
                {
                    Type = ServiceType.Sale,
                    DeviceId = devices.ElementAtOrDefault(1)?.Id ?? devices.First().Id,
                    ClientId = clients.ElementAtOrDefault(1)?.Id ?? clients.First().Id,
                    TechnicianId = technicians.ElementAtOrDefault(1)?.Id ?? technicians.First().Id,
                    Description = "Sprzedaż smartfona Samsung Galaxy S21",
                    Price = magazinePrices[devices.ElementAtOrDefault(1)?.MagazineId ?? devices.First().MagazineId],
                    Status = ServiceStatus.Completed,
                    Date = DateTime.UtcNow.AddDays(-2)
                },
                new Service
                {
                    Type = ServiceType.Service,
                    DeviceId = devices.ElementAtOrDefault(2)?.Id ?? devices.First().Id,
                    ClientId = clients.ElementAtOrDefault(2)?.Id ?? clients.First().Id,
                    TechnicianId = technicians.First().Id,
                    Description = "Aktualizacja oprogramowania tabletu Apple iPad Pro",
                    Price = magazinePrices[devices.ElementAtOrDefault(2)?.MagazineId ?? devices.First().MagazineId],
                    Status = ServiceStatus.InProgress,
                    Date = DateTime.UtcNow
                }
            };

            context.Services.AddRange(services);
            context.SaveChanges();
        }
    }
}
