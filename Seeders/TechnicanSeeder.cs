using System;
using System.Collections.Generic;
using System.Linq;
using Bogus;
using ComputerServiceManager.Database;

namespace ComputerServiceManager.Seeders
{
    public static class TechnicianSeeder
    {
        public static void Seed(AppDbContext context)
        {
            if (context.Technicians.Any())
                return;

            var faker = new Faker<Technician>()
                .RuleFor(t => t.Name, f => f.Name.FirstName())
                .RuleFor(t => t.Surname, f => f.Name.LastName())
                .RuleFor(t => t.PhoneNumber, f => f.Phone.PhoneNumber("### ### ###"))
                .RuleFor(t => t.EmploymentDate, f => f.Date.PastOffset(5).ToUniversalTime()) 
                .RuleFor(t => t.IsActive, f => f.Random.Bool())
                .RuleFor(t => t.ImageUrl, f => (string?)null);


            var technicians = faker.Generate(20);

            context.Technicians.AddRange(technicians);
            context.SaveChanges();
        }
    }
}