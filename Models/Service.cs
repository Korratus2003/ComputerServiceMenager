// Tabela usług (serwis + sprzedaż)

using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

[Table("Services")]
public class Service
{
    public int Id { get; set; }

    [Required]
    public ServiceType Type { get; set; } 

    public int DeviceId { get; set; }
    public int ClientId { get; set; }
    public int TechnicianId { get; set; }

    public string Description { get; set; }

    [Column(TypeName = "decimal(10,2)")]
    public decimal Price { get; set; }

    public ServiceStatus Status { get; set; }

    public DateTime Date { get; set; }
    
    public Device Device { get; set; }
    public Client Client { get; set; }
    public Technician Technician { get; set; }
}