using System;
using ComputerServiceManager.Seeders;
using Microsoft.EntityFrameworkCore;

namespace ComputerServiceManager.Database;

public class AppDbContext : DbContext

{
        public DbSet<Client> Clients { get; set; }
        public DbSet<Magazine> Magazines { get; set; }
        public DbSet<Device> Devices { get; set; }
        public DbSet<Technician> Technicians { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Service> Services { get; set; }

        
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
            // Konfiguracja tabeli Clients
            modelBuilder.Entity<Client>(entity =>
            {
                entity.Property(e => e.Name)
                      .HasMaxLength(255)
                      .IsRequired();

                entity.Property(e => e.Surname)
                      .HasMaxLength(255)
                      .IsRequired();

                entity.Property(e => e.PhoneNumber)
                      .HasMaxLength(20);

                entity.Property(e => e.Email)
                      .HasMaxLength(255);

                entity.Property(e => e.CreatedAt).IsRequired();
            });

            // Konfiguracja tabeli Magazine
            modelBuilder.Entity<Magazine>(entity =>
            {
                entity.Property(e => e.Name)
                      .HasMaxLength(255)
                      .IsRequired();

                entity.Property(e => e.DefaultPrice)
                      .HasColumnType("decimal(10,2)");

                entity.Property(e => e.Quantity).IsRequired();
            });

            // Konfiguracja tabeli Devices
            modelBuilder.Entity<Device>(entity =>
            {
                entity.Property(e => e.Name)
                      .HasMaxLength(255)
                      .IsRequired();

                entity.Property(e => e.SerialNumber)
                      .HasMaxLength(100);

                entity.HasOne(d => d.Magazine)
                      .WithMany(m => m.Devices)
                      .HasForeignKey(d => d.MagazineId)
                      .OnDelete(DeleteBehavior.Cascade);
            });

            // Konfiguracja tabeli Technicians
            modelBuilder.Entity<Technician>(entity =>
            {
                entity.Property(e => e.Name)
                      .HasMaxLength(255)
                      .IsRequired();

                entity.Property(e => e.Surname)
                      .HasMaxLength(255)
                      .IsRequired();

                entity.Property(e => e.PhoneNumber)
                      .HasMaxLength(20);
            });

            // Konfiguracja tabeli Users
            modelBuilder.Entity<User>(entity =>
            {
                  entity.Property(e => e.Login)
                        .HasMaxLength(50)
                        .IsRequired();

                  entity.Property(e => e.PasswordHash)
                        .HasMaxLength(255)
                        .IsRequired();

                  entity.Property(e => e.Range)
                        .IsRequired();

                  entity.HasIndex(e => e.Login)
                        .IsUnique();
                  
                  entity.HasOne(u => u.Technician)
                        .WithMany(t => t.User) // Zmieniono relację: technik może mieć wielu użytkowników
                        .HasForeignKey(u => u.TechnicianId)
                        .OnDelete(DeleteBehavior.Restrict);
            });

            // Konfiguracja tabeli Services
            modelBuilder.Entity<Service>(entity =>
            {
                entity.Property(e => e.Type)
                      .IsRequired();

                entity.Property(e => e.Price)
                      .HasColumnType("decimal(10,2)");

                // Relacja z Device
                entity.HasOne(s => s.Device)
                      .WithMany(d => d.Services)
                      .HasForeignKey(s => s.DeviceId)
                      .OnDelete(DeleteBehavior.Cascade);

                // Relacja z Client
                entity.HasOne(s => s.Client)
                      .WithMany(c => c.Services)
                      .HasForeignKey(s => s.ClientId)
                      .OnDelete(DeleteBehavior.Cascade);

                // Relacja z Technician
                entity.HasOne(s => s.Technician)
                      .WithMany(t => t.Services)
                      .HasForeignKey(s => s.TechnicianId)
                      .OnDelete(DeleteBehavior.Cascade);
            });

            base.OnModelCreating(modelBuilder);
            
        }
}
