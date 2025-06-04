using System;
using System.Collections.Generic;
using System.Linq;
using Bogus;
using ComputerServiceManager.Database;

namespace ComputerServiceManager.Seeders
{
    public static class ClientSeeder
    {
        public static void Seed(AppDbContext context)
        {
            if (context.Clients.Any())
                return;
            
            var clientFaker = new Faker<Client>()
                .RuleFor(c => c.Name, f => f.Name.FirstName())
                .RuleFor(c => c.Surname, f => f.Name.LastName())
                .RuleFor(c => c.PhoneNumber, f => f.Phone.PhoneNumber("### ### ###"))
                .RuleFor(c => c.Email, f => f.Internet.Email())
                .RuleFor(c => c.CreatedAt, f => f.Date.Past(2).ToUniversalTime());

            var clients = clientFaker.Generate(20); 

            context.Clients.AddRange(clients);
            context.SaveChanges();
        }
    }
}