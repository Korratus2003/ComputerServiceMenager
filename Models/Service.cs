using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ComputerServiceManager.Database;

public enum ServiceStatus
{
    Pending,
    InProgress,
    Completed,
    Canceled
}

[Table("Services")]
public class Service
{
    [Key]
    public int Id { get; set; }

    [ForeignKey(nameof(Device))]
    public int DeviceId { get; set; }
    public virtual Device Device { get; set; } = null!;

    [ForeignKey(nameof(Technician))]
    public int TechnicianId { get; set; }
    public virtual Technician Technician { get; set; } = null!;

    [ForeignKey(nameof(ServiceType))]
    public int ServiceTypeId { get; set; }
    public virtual ServiceType ServiceType { get; set; } = null!;

    public string? Description { get; set; }

    [Column(TypeName = "decimal(10,2)")]
    public decimal Price { get; set; }

    [Required]
    public ServiceStatus Status { get; set; }

    public DateTimeOffset Date { get; set; }
}