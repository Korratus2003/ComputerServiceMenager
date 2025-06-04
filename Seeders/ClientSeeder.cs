using System;
using System.Collections.Generic;
using System.Linq;
using ComputerServiceManager.Database;

namespace ComputerServiceManager.Seeders
{
    public static class ClientSeeder
    {
        public static void Seed(AppDbContext context)
        {
            if (context.Clients.Any())
                return;
                
            var clients = new List<Client>
            {
                new Client
                {
                    Name = "Jan",
                    Surname = "Kowalski",
                    PhoneNumber = "123456789",
                    Email = "jan.kowalski@przyklad.pl",
                    CreatedAt = DateTime.UtcNow
                },
                new Client
                {
                    Name = "Anna",
                    Surname = "Nowak",
                    PhoneNumber = "987654321",
                    Email = "anna.nowak@przyklad.pl",
                    CreatedAt = DateTime.UtcNow.AddDays(-30)
                },
                new Client
                {
                    Name = "Marek",
                    Surname = "Lewandowski",
                    PhoneNumber = "111222333",
                    Email = "marek.lewandowski@przyklad.pl",
                    CreatedAt = DateTime.UtcNow.AddDays(-60)
                }
            };

            context.Clients.AddRange(clients);
            context.SaveChanges();
        }
    }
}