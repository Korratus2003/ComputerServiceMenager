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
                    Name = "Piotr",
                    Surname = "Wiśniewski",
                    PhoneNumber = "500600700",
                    EmploymentDate = new DateTime(2018, 6, 1, 0, 0, 0, DateTimeKind.Utc),
                    IsActive = true
                },
                new Technician
                {
                    Name = "Katarzyna",
                    Surname = "Zielińska",
                    PhoneNumber = "600700800",
                    EmploymentDate = new DateTime(2019, 3, 15, 0, 0, 0, DateTimeKind.Utc),
                    IsActive = true
                },
                new Technician
                {
                    Name = "Jarosław",
                    Surname = "Majewski",
                    PhoneNumber = "700800900",
                    EmploymentDate = new DateTime(2020, 1, 10, 0, 0, 0, DateTimeKind.Utc),
                    IsActive = false
                }
            };

            context.Technicians.AddRange(technicians);
            context.SaveChanges();
        }
    }
}