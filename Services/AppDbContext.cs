using System;
using Microsoft.EntityFrameworkCore;

namespace ComputerServiceManager.Services;

public class AppDbContext : DbContext
{
    public DbSet<Client> Clients { get; set; }
    public DbSet<Device> Devices { get; set; }
    public DbSet<Technician> Technicians { get; set; }
    public DbSet<Service> Services { get; set; }
    public DbSet<InventoryDevice> InventoryDevices { get; set; }
    public DbSet<Sale> Sales { get; set; }
    public DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        DotNetEnv.Env.Load();
            
        String connectionString = $"Host={Environment.GetEnvironmentVariable("DB_HOST")};" +
                           $"Port={Environment.GetEnvironmentVariable("DB_PORT")};" +
                           $"Username={Environment.GetEnvironmentVariable("DB_USERNAME")};" +
                           $"Password={Environment.GetEnvironmentVariable("DB_PASSWORD")};" +
                           $"Database={Environment.GetEnvironmentVariable("DB_NAME")}";
        optionsBuilder.UseNpgsql(connectionString);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Client>()
            .HasMany(c => c.Devices)
            .WithOne(d => d.Client)
            .HasForeignKey(d => d.ClientId);

        modelBuilder.Entity<Device>()
            .HasMany(d => d.Services)
            .WithOne(s => s.Device)
            .HasForeignKey(s => s.DeviceId);

        modelBuilder.Entity<Device>()
            .HasMany(d => d.Sales)
            .WithOne(s => s.Device)
            .HasForeignKey(s => s.DeviceId);

        modelBuilder.Entity<Technician>()
            .HasMany(t => t.Services)
            .WithOne(s => s.Technician)
            .HasForeignKey(s => s.TechnicianId);

        modelBuilder.Entity<Technician>()
            .HasOne(t => t.User)
            .WithOne(u => u.Technician)
            .HasForeignKey<User>(u => u.TechnicianId);

        modelBuilder.Entity<Sale>()
            .HasOne(s => s.InventoryDevice)
            .WithMany(i => i.Sales)
            .HasForeignKey(s => s.InventoryDeviceId);

        modelBuilder.Entity<Sale>()
            .HasOne(s => s.Device)
            .WithMany(d => d.Sales)
            .HasForeignKey(s => s.DeviceId);
    }
}
