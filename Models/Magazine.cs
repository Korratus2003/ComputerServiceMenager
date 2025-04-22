using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

[Table("Magazine")]
public class Magazine
{
    public int Id { get; set; }

    [Required, MaxLength(255)]
    public string Name { get; set; }

    public string Description { get; set; }

    [Column(TypeName = "decimal(10,2)")]
    public decimal DefaultPrice { get; set; }

    [Required]
    public int Quantity { get; set; }
    
    public ICollection<Device> Devices { get; set; }
}