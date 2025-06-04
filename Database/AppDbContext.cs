using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace ComputerServiceManager.Database
{
    public class AppDbContext : DbContext
    {
        public DbSet<Client> Clients { get; set; }
        public DbSet<Device> Devices { get; set; }
        public DbSet<Technician> Technicians { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<ServiceType> ServiceTypes { get; set; }
        public DbSet<Service> Services { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            DotNetEnv.Env.Load();

            string connectionString = $"Host={Environment.GetEnvironmentVariable("DB_HOST")};" +
                                      $"Port={Environment.GetEnvironmentVariable("DB_PORT")};" +
                                      $"Username={Environment.GetEnvironmentVariable("DB_USERNAME")};" +
                                      $"Password={Environment.GetEnvironmentVariable("DB_PASSWORD")};" +
                                      $"Database={Environment.GetEnvironmentVariable("DB_NAME")}";

            optionsBuilder.UseNpgsql(connectionString);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Klienci
            modelBuilder.Entity<Client>(entity =>
            {
                entity.HasKey(e => e.Id);

                entity.Property(e => e.Name)
                      .IsRequired()
                      .HasMaxLength(255);

                entity.Property(e => e.Surname)
                      .IsRequired()
                      .HasMaxLength(255);

                entity.Property(e => e.PhoneNumber)
                      .HasMaxLength(20);

                entity.Property(e => e.Email)
                      .HasMaxLength(255);

                entity.Property(e => e.CreatedAt)
                      .HasColumnType("timestamptz");

                entity.HasMany(c => c.Devices)
                      .WithOne(d => d.OwnerClient)
                      .HasForeignKey(d => d.OwnerClientId)
                      .OnDelete(DeleteBehavior.Cascade);
                
            });

            // Urządzenia
            modelBuilder.Entity<Device>(entity =>
            {
                entity.HasKey(e => e.Id);

                entity.Property(e => e.Name)
                      .IsRequired()
                      .HasMaxLength(255);

                entity.Property(e => e.SerialNumber)
                      .HasMaxLength(100);

                entity.Property(e => e.Description)
                      .HasColumnType("text");

                entity.Property(e => e.AddedAt)
                      .HasColumnType("timestamptz");

                entity.HasOne(d => d.OwnerClient)
                      .WithMany(c => c.Devices)
                      .HasForeignKey(d => d.OwnerClientId)
                      .OnDelete(DeleteBehavior.Cascade);

                entity.HasMany(d => d.Services)
                      .WithOne(s => s.Device)
                      .HasForeignKey(s => s.DeviceId)
                      .OnDelete(DeleteBehavior.Cascade);
            });

            // Technicy
            modelBuilder.Entity<Technician>(entity =>
            {
                entity.HasKey(e => e.Id);

                entity.Property(e => e.Name)
                      .IsRequired()
                      .HasMaxLength(255);

                entity.Property(e => e.Surname)
                      .IsRequired()
                      .HasMaxLength(255);

                entity.Property(e => e.PhoneNumber)
                      .HasMaxLength(20);

                var dateTimeOffsetConverter = new ValueConverter<DateTimeOffset?, DateTime?>(
                    v => v.HasValue ? v.Value.UtcDateTime : (DateTime?)null,
                    v => v.HasValue ? new DateTimeOffset(DateTime.SpecifyKind(v.Value, DateTimeKind.Utc)) : (DateTimeOffset?)null);

                entity.Property(e => e.EmploymentDate)
                      .HasConversion(dateTimeOffsetConverter)
                      .HasColumnType("timestamp with time zone");

                entity.Property(e => e.IsActive);

                entity.HasMany(t => t.Services)
                      .WithOne(s => s.Technician)
                      .HasForeignKey(s => s.TechnicianId)
                      .OnDelete(DeleteBehavior.Cascade);

                entity.HasMany(t => t.Users)
                      .WithOne(u => u.Technician)
                      .HasForeignKey(u => u.TechnicianId)
                      .IsRequired(false)
                      .OnDelete(DeleteBehavior.Restrict);
            });

            // Użytkownicy
            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(e => e.Id);

                entity.Property(e => e.Login)
                      .IsRequired()
                      .HasMaxLength(50);

                entity.HasIndex(e => e.Login)
                      .IsUnique();

                entity.Property(e => e.PasswordHash)
                      .IsRequired()
                      .HasMaxLength(255);

                entity.Property(e => e.Range)
                      .IsRequired();
            });

            // Typy usług
            modelBuilder.Entity<ServiceType>(entity =>
            {
                entity.HasKey(e => e.Id);

                entity.Property(e => e.Name)
                      .IsRequired()
                      .HasMaxLength(255);

                entity.HasIndex(e => e.Name)
                      .IsUnique();

                entity.Property(e => e.DefaultPrice)
                      .HasColumnType("decimal(10,2)");

                entity.HasMany(st => st.Services)
                      .WithOne(s => s.ServiceType)
                      .HasForeignKey(s => s.ServiceTypeId)
                      .OnDelete(DeleteBehavior.Cascade);
            });

            // Usługi
            modelBuilder.Entity<Service>(entity =>
            {
                entity.HasKey(e => e.Id);

                entity.Property(e => e.Description)
                      .HasColumnType("text");

                entity.Property(e => e.Price)
                      .HasColumnType("decimal(10,2)");

                entity.Property(e => e.Status)
                      .IsRequired();

                entity.Property(e => e.Date)
                      .HasColumnType("timestamptz");

                entity.HasOne(s => s.Device)
                      .WithMany(d => d.Services)
                      .HasForeignKey(s => s.DeviceId)
                      .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(s => s.Technician)
                      .WithMany(t => t.Services)
                      .HasForeignKey(s => s.TechnicianId)
                      .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(s => s.ServiceType)
                      .WithMany(st => st.Services)
                      .HasForeignKey(s => s.ServiceTypeId)
                      .OnDelete(DeleteBehavior.Cascade);
            });

            base.OnModelCreating(modelBuilder);
        }
    }
}
