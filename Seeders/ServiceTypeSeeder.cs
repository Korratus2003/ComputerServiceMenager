using System;
using System.Linq;
using ComputerServiceManager.Database;

namespace ComputerServiceManager.Seeders
{
    public class ServiceTypeSeeder
    {
        public static void Seed(AppDbContext context)
        {
            if (context.ServiceTypes.Any())
                return;

            var serviceTypes = new[]
            {
                new ServiceType { Name = "Diagnostics", DefaultPrice = 50.00m },
                new ServiceType { Name = "SSD Replacement", DefaultPrice = 200.00m },
                new ServiceType { Name = "Cooling System Cleaning", DefaultPrice = 100.00m },
                new ServiceType { Name = "Operating System Installation", DefaultPrice = 150.00m },
                new ServiceType { Name = "Battery Replacement", DefaultPrice = 80.00m },
                new ServiceType { Name = "Screen Repair", DefaultPrice = 250.00m },
                new ServiceType { Name = "Keyboard Replacement", DefaultPrice = 120.00m },
                new ServiceType { Name = "Virus & Malware Removal", DefaultPrice = 90.00m },
                new ServiceType { Name = "Data Recovery", DefaultPrice = 300.00m },
                new ServiceType { Name = "BIOS Update", DefaultPrice = 70.00m },
                new ServiceType { Name = "Software Optimization", DefaultPrice = 110.00m },
                new ServiceType { Name = "GPU Thermal Paste Replacement", DefaultPrice = 130.00m }
            };

            context.ServiceTypes.AddRange(serviceTypes);
            context.SaveChanges();
        }
    }
}