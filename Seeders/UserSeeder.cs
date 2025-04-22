using System;
using System.Collections.Generic;
using System.Linq;
using ComputerServiceManager.Database;

namespace ComputerServiceManager.Seeders
{
    public static class UserSeeder
    {
        public static void Seed(AppDbContext context)
        {
            if (context.Users.Any())
                return;
                
            var technicians = context.Technicians.ToList();
            if (!technicians.Any())
                throw new Exception("Brak techników. Uruchom najpierw TechnicianSeeder.");

            var users = new List<User>();

            foreach (var technician in technicians)
            {
                users.Add(new User
                {
                    TechnicianId = technician.Id,
                    Login = $"user{technician.Id}",
                    PasswordHash = "1234",
                    Range = UserRange.Technician
                });
            }

            context.Users.AddRange(users);
            context.SaveChanges();
        }
    }
}