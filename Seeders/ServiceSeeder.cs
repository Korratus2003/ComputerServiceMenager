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
            
            var firstClient = clients.First();
            
            var clientDevices = devices
                .Where(d => d.OwnerClientId == firstClient.Id)
                .ToList();
            
            if (!clientDevices.Any())
            {
                clientDevices = new List<Device> { devices.First() };
            }
            
            var repairServiceType = serviceTypes.FirstOrDefault(st => st.Name.Contains("Naprawa")) 
                                     ?? serviceTypes.First();
            var installServiceType = serviceTypes.FirstOrDefault(st => st.Name.Contains("Instalacja")) 
                                     ?? serviceTypes.FirstOrDefault(st => st.Id != repairServiceType.Id) 
                                     ?? serviceTypes.First();

            var services = new List<Service>
            {
                new Service
                {
                    DeviceId = clientDevices[0].Id,
                    TechnicianId = technicians.First().Id,
                    ServiceTypeId = repairServiceType.Id,
                    Description = "Naprawa laptopa Dell XPS 15 dla pierwszego klienta",
                    Price = repairServiceType.DefaultPrice,
                    Status = ServiceStatus.Completed,
                    IsPaid = false,
                    Date = DateTimeOffset.UtcNow.AddDays(-3)
                },
                new Service
                {
                    DeviceId = clientDevices[0].Id,
                    TechnicianId = technicians.First().Id,
                    ServiceTypeId = installServiceType.Id,
                    Description = "Instalacja systemu Windows 10 na laptopie",
                    Price = installServiceType.DefaultPrice,
                    Status = ServiceStatus.Completed,
                    IsPaid = false,
                    Date = DateTimeOffset.UtcNow.AddDays(-2)
                }
            };
            if (clientDevices.Count > 1)
            {
                services.Add(new Service
                {
                    DeviceId = clientDevices[1].Id,
                    TechnicianId = technicians.First().Id,
                    ServiceTypeId = repairServiceType.Id,
                    Description = "Aktualizacja oprogramowania na drugim urządzeniu pierwszego klienta",
                    Price = repairServiceType.DefaultPrice,
                    Status = ServiceStatus.InProgress,
                    IsPaid = false,
                    Date = DateTimeOffset.UtcNow.AddDays(-1)
                });
            }

            context.Services.AddRange(services);
            context.SaveChanges();
        }
    }
}
