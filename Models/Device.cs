// Tabela urządzeń przypisanych do klienta/serwisu

using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

[Table("Devices")]
public class Device
{
    public int Id { get; set; }

    // Klucz obcy do Magazine
    public int MagazineId { get; set; }

    [Required, MaxLength(255)]
    public string Name { get; set; } 

    [MaxLength(100)]
    public string SerialNumber { get; set; }

    public string Description { get; set; }
    
    public Magazine Magazine { get; set; }
    public ICollection<Service> Services { get; set; }
}