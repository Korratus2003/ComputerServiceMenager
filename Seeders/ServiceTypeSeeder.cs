using System;
using System.Linq;
using ComputerServiceManager.Database;

namespace ComputerServiceManager.Seeders;

public class ServiceTypeSeeder
{
    public static void Seed(AppDbContext context)
    {
        var serviceTypes = new[]
        {
            new ServiceType
            {
                Name = "Diagnostyka",
                DefaultPrice = 50.00m
            },
            new ServiceType
            {
                Name = "Wymiana dysku SSD",
                DefaultPrice = 200.00m
            },
            new ServiceType
            {
                Name = "Czyszczenie układu chłodzenia",
                DefaultPrice = 100.00m
            },
            new ServiceType
            {
                Name = "Instalacja systemu operacyjnego",
                DefaultPrice = 150.00m
            }
        };

        context.ServiceTypes.AddRange(serviceTypes);
        context.SaveChanges();
    }
}