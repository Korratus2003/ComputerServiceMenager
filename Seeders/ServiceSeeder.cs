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
            var serviceTypes = context.ServiceTypes.ToList();

            if (!clients.Any() || !devices.Any() || !technicians.Any() || !serviceTypes.Any())
                throw new Exception("Brak klienta, urządzenia, technika lub typu usługi. Upewnij się, że poprzednie seedery działają poprawnie.");

            var repairServiceType = serviceTypes.FirstOrDefault(st => st.Name.Contains("Naprawa")) ?? serviceTypes.First();
            var saleServiceType = serviceTypes.FirstOrDefault(st => st.Name.Contains("Sprzedaż")) ?? serviceTypes.First();

            var services = new List<Service>
            {
                new Service
                {
                    DeviceId = devices.First().Id,
                    TechnicianId = technicians.First().Id,
                    ServiceTypeId = repairServiceType.Id,
                    Description = "Naprawa laptopa Dell XPS 15",
                    Price = repairServiceType.DefaultPrice,
                    Status = ServiceStatus.Pending,
                    Date = DateTimeOffset.UtcNow.AddDays(-1)
                },
                new Service
                {
                    DeviceId = devices.ElementAtOrDefault(1)?.Id ?? devices.First().Id,
                    TechnicianId = technicians.ElementAtOrDefault(1)?.Id ?? technicians.First().Id,
                    ServiceTypeId = saleServiceType.Id,
                    Description = "Sprzedaż smartfona Samsung Galaxy S21",
                    Price = saleServiceType.DefaultPrice,
                    Status = ServiceStatus.Completed,
                    Date = DateTimeOffset.UtcNow.AddDays(-2)
                },
                new Service
                {
                    DeviceId = devices.ElementAtOrDefault(2)?.Id ?? devices.First().Id,
                    TechnicianId = technicians.First().Id,
                    ServiceTypeId = repairServiceType.Id,
                    Description = "Aktualizacja oprogramowania tabletu Apple iPad Pro",
                    Price = repairServiceType.DefaultPrice,
                    Status = ServiceStatus.InProgress,
                    Date = DateTimeOffset.UtcNow
                }
            };

            context.Services.AddRange(services);
            context.SaveChanges();
        }
    }
}
