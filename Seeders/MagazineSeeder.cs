using System.Collections.Generic;
using System.Linq;
using ComputerServiceManager.Database;

namespace ComputerServiceManager.Seeders
{
    public static class MagazineSeeder
    {
        public static void Seed(AppDbContext context)
        {
            if (context.Magazines.Any())
                return;
                
            var magazines = new List<Magazine>
            {
                new Magazine
                {
                    Name = "Laptop",
                    Description = "Przenośny komputer",
                    DefaultPrice = 2500.00m,
                    Quantity = 15
                },
                new Magazine
                {
                    Name = "Smartphone",
                    Description = "Nowoczesny telefon komórkowy",
                    DefaultPrice = 1200.50m,
                    Quantity = 30
                },
                new Magazine
                {
                    Name = "Tablet",
                    Description = "Wielofunkcyjne urządzenie mobilne",
                    DefaultPrice = 1800.00m,
                    Quantity = 20
                }
            };

            context.Magazines.AddRange(magazines);
            context.SaveChanges();
        }
    }
}