using System;
using System.Collections.Generic;
using System.Linq;
using ComputerServiceManager;
using ComputerServiceManager.Database;

namespace ComputerServiceManager.Seeders
{
    public static class TechnicianSeeder
    {
        public static void Seed(AppDbContext context)
        {
            if (context.Technicians.Any())
                return;

            var technicians = new List<Technician>
            {
                new Technician
                {
                    Name = "Katarzyna",
                    Surname = "Zielińska",
                    PhoneNumber = "600700800",
                    EmploymentDate = new DateTimeOffset(new DateTime(2019, 3, 15, 0, 0, 0), TimeSpan.Zero), // UTC time
                    IsActive = true,
                },
                new Technician
                {
                    Name = "Jarosław",
                    Surname = "Majewski",
                    PhoneNumber = "700800900",
                    EmploymentDate = new DateTimeOffset(new DateTime(2020, 1, 10, 0, 0, 0), TimeSpan.Zero), // UTC time
                    IsActive = false,
                    ImageUrl = "/home/konrad/Pulpit/obrazki/serwisant.jpeg"
                },new Technician
                {
                    Name = "Piotr",
                    Surname = "Wiśniewski",
                    PhoneNumber = "500600700",
                    EmploymentDate = new DateTimeOffset(new DateTime(2018, 6, 1, 0, 0, 0), TimeSpan.Zero), // UTC time
                    IsActive = true,
                },
                
            };

            context.Technicians.AddRange(technicians);
            context.SaveChanges();
        }
    }
}