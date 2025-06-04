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
                throw new Exception("Missing client, device, technician or service type. Please ensure previous seeders have run correctly.");

            var firstClient = clients.First();

            var clientDevices = devices
                .Where(d => d.OwnerClientId == firstClient.Id)
                .ToList();

            if (!clientDevices.Any())
            {
                clientDevices = new List<Device> { devices.First() };
            }

            var repairServiceType = serviceTypes.FirstOrDefault(st => st.Name.Contains("Repair"))
                                     ?? serviceTypes.First();
            var installServiceType = serviceTypes.FirstOrDefault(st => st.Name.Contains("Install"))
                                     ?? serviceTypes.FirstOrDefault(st => st.Id != repairServiceType.Id)
                                     ?? serviceTypes.First();

            var services = new List<Service>
            {
                new Service
                {
                    DeviceId = clientDevices[0].Id,
                    TechnicianId = technicians.First().Id,
                    ServiceTypeId = repairServiceType.Id,
                    Description = "Repair of Dell XPS 15 laptop for the first client",
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
                    Description = "Windows 10 installation on laptop",
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
                    Description = "Software update on the second device of the first client",
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
