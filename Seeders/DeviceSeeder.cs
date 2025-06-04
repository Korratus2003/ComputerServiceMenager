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

            // Zakładam, że potrzebujesz przypisać OwnerClientId, więc pobierz klientów z bazy
            var clients = context.Clients.ToList();
            if (clients.Count == 0)
                throw new Exception("Brak wpisów w tabeli Clients. Uruchom najpierw ClientSeeder.");

            var devices = new List<Device>
            {
                new Device
                {
                    OwnerClientId = clients.First().Id,
                    Name = "Dell XPS 15",
                    SerialNumber = "LAPTOP-001",
                    Description = "Laptop Dell z wysoką wydajnością.",
                    AddedAt = DateTimeOffset.UtcNow
                },
                new Device
                {
                    OwnerClientId = clients.Skip(1).FirstOrDefault()?.Id ?? clients.First().Id,
                    Name = "Samsung Galaxy S21",
                    SerialNumber = "PHONE-001",
                    Description = "Smartphone z flagowym aparatem.",
                    AddedAt = DateTimeOffset.UtcNow
                },
                new Device
                {
                    OwnerClientId = clients.Skip(2).FirstOrDefault()?.Id ?? clients.First().Id,
                    Name = "Apple iPad Pro",
                    SerialNumber = "TABLET-001",
                    Description = "Tablet idealny do pracy i rozrywki.",
                    AddedAt = DateTimeOffset.UtcNow
                }
            };

            context.Devices.AddRange(devices);
            context.SaveChanges();
        }
    }
}