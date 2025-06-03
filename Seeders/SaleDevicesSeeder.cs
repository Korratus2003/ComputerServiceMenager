using System.Collections.Generic;
using System.Linq;
using ComputerServiceManager.Database;

namespace ComputerServiceManager.Seeders
{
    public static class SaleDevicesSeeder
    {
        public static void Seed(AppDbContext context)
        {
            if (context.SaleDevices.Any())
                return;
                
            var saleDevices = new List<SaleDevice>
            {
                new SaleDevice { Name = "Samsung Galaxy Z Fold6 Ultra 1TB 5G", Category = "Smartphone", Description = "Najnowocześniejszy składany smartfon Samsung", DefaultPrice = 8999.00m, Quantity = 15 },
                new SaleDevice { Name = "Apple MacBook Air M3 15\" 512GB SSD", Category = "Laptop", Description = "Ultraprzenośny laptop Apple z procesorem M3", DefaultPrice = 7499.00m, Quantity = 10 },
                new SaleDevice { Name = "Sony WH-1000XM6 Wireless Noise Cancelling Headphones", Category = "Audio", Description = "Bezprzewodowe słuchawki z zaawansowaną redukcją hałasu", DefaultPrice = 1599.00m, Quantity = 20 },
                new SaleDevice { Name = "Lenovo ThinkBook Plus Gen 6 Rollable Screen 1TB SSD", Category = "Laptop", Description = "Laptop z rozszerzalnym ekranem OLED", DefaultPrice = 9999.00m, Quantity = 8 },
                new SaleDevice { Name = "LG G5 OLED TV 65\" 4K HDR 165Hz", Category = "TV", Description = "Telewizor OLED o wysokiej częstotliwości odświeżania", DefaultPrice = 7999.00m, Quantity = 5 },
                new SaleDevice { Name = "Microsoft Surface Pro 10 16GB RAM 1TB SSD", Category = "Tablet", Description = "Tablet i laptop w jednym z najnowszym procesorem Intel", DefaultPrice = 8999.00m, Quantity = 12 },
                new SaleDevice { Name = "Google Pixel 8 Pro Max 512GB AI Camera", Category = "Smartphone", Description = "Smartfon Google z zaawansowaną sztuczną inteligencją", DefaultPrice = 5999.00m, Quantity = 18 },
                new SaleDevice { Name = "Asus ROG Zephyrus G14 OLED Ryzen 9 32GB RAM", Category = "Laptop", Description = "Gamingowy laptop z ekranem OLED", DefaultPrice = 10999.00m, Quantity = 7 },
                new SaleDevice { Name = "Xiaomi 14 Ultra 1TB AI Camera 200MP", Category = "Smartphone", Description = "Smartfon z najwyższej klasy aparatem", DefaultPrice = 4999.00m, Quantity = 22 },
                new SaleDevice { Name = "Apple iPad Pro M3 12.9\" 1TB Cellular", Category = "Tablet", Description = "Profesjonalny tablet Apple z ekranem mini-LED", DefaultPrice = 6999.00m, Quantity = 16 },
                new SaleDevice { Name = "Samsung Galaxy Watch 6 Pro Titanium 4G LTE", Category = "Wearable", Description = "Smartwatch Samsung z funkcją LTE", DefaultPrice = 1999.00m, Quantity = 20 },
                new SaleDevice { Name = "Bose QuietComfort Ultra Wireless Headphones", Category = "Audio", Description = "Bezprzewodowe słuchawki z doskonałą jakością dźwięku", DefaultPrice = 1799.00m, Quantity = 25 },
                new SaleDevice { Name = "Sony PlayStation 5 Pro 2TB SSD VR Ready", Category = "Console", Description = "Konsola nowej generacji z obsługą VR", DefaultPrice = 3499.00m, Quantity = 30 },
                new SaleDevice { Name = "Marshall Heston 120 Dolby Atmos Soundbar", Category = "Audio", Description = "Soundbar premium z obsługą Dolby Atmos", DefaultPrice = 2999.00m, Quantity = 10 },
                new SaleDevice { Name = "Twelve South PlugBug 50W GaN Charger with Apple Find My", Category = "Accessory", Description = "Ładowarka z funkcją lokalizacji Apple", DefaultPrice = 499.00m, Quantity = 30 }
            };

            context.SaleDevices.AddRange(saleDevices);
            context.SaveChanges();
        }
    }
}
