using System;
using System.Collections.Generic;
using System.Linq;
using Bogus;
using ComputerServiceManager.Database;

namespace ComputerServiceManager.Seeders
{
    public static class DeviceSeeder
    {
        public static void Seed(AppDbContext context)
        {
            if (context.Devices.Any())
                return;

            var clients = context.Clients.ToList();
            if (clients.Count == 0)
                throw new Exception("Brak wpisów w tabeli Clients. Uruchom najpierw ClientSeeder.");

            var deviceFaker = new Faker<Device>()
                .RuleFor(d => d.OwnerClientId, f => f.PickRandom(clients).Id)
                .RuleFor(d => d.Name, f => f.Commerce.ProductName())
                .RuleFor(d => d.SerialNumber, f => f.Random.Replace("####"))
                .RuleFor(d => d.Description, f => f.Lorem.Sentence())
                .RuleFor(d => d.AddedAt, f => f.Date.Past(3).ToUniversalTime());

            var devices = deviceFaker.Generate(20);

            context.Devices.AddRange(devices);
            context.SaveChanges();
        }
    }
}