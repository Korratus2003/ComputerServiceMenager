using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

[Table("Devices")]
public class Device
{
    public int Id { get; set; }

    [Required, MaxLength(255)]
    public string Name { get; set; } 

    [MaxLength(100)]
    public string SerialNumber { get; set; }

    public string Description { get; set; }
    public int SaleDeviceId { get; set; } 
    
    [ForeignKey("SaleDeviceId")]
    public SaleDevice SaleDevice { get; set; }

    public ICollection<Service> Services { get; set; }
}