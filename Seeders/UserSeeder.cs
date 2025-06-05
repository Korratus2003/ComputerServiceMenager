using System;
using System.Collections.Generic;
using System.Linq;
using ComputerServiceManager.Database;
using BCrypt.Net;

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
                var hashedPassword = BCrypt.Net.BCrypt.HashPassword("1234");
                users.Add(new User
                {
                    TechnicianId = technician.Id,
                    Login = $"user{technician.Id}",
                    PasswordHash = hashedPassword,
                    Range = UserRange.Technician
                });
            }

            var adminHashed = BCrypt.Net.BCrypt.HashPassword("1234");
            users.Add(new User
            {
                TechnicianId = null,
                Login = "admin",
                PasswordHash = adminHashed,
                Range = UserRange.Admin
            });

            context.Users.AddRange(users);
            context.SaveChanges();
        }
    }
}