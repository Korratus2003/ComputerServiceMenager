using Avalonia;
using System;
using ComputerServiceManager.Database;
using ComputerServiceManager.Seeders;
using Microsoft.EntityFrameworkCore;

namespace ComputerServiceManager;

sealed class Program
{
    [STAThread]
    public static void Main(string[] args)
    {
        using (var dbContext = new AppDbContext())
        {
            dbContext.Database.EnsureCreated();
            
            MagazineSeeder.Seed(dbContext);
            ClientSeeder.Seed(dbContext);
            TechnicianSeeder.Seed(dbContext);
            DeviceSeeder.Seed(dbContext);
            UserSeeder.Seed(dbContext);
            ServiceSeeder.Seed(dbContext);
        }

        BuildAvaloniaApp().StartWithClassicDesktopLifetime(args);
    }
    
    public static AppBuilder BuildAvaloniaApp()
        => AppBuilder.Configure<App>()
            .UsePlatformDetect()
            .WithInterFont()
            .LogToTrace();
}