using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

[Table("ServiceTypes")]
public class ServiceType
{
    [Key]
    public int Id { get; set; }

    [Required, MaxLength(255)]
    public string Name { get; set; } = null!;

    [Column(TypeName = "decimal(10,2)")]
    public decimal DefaultPrice { get; set; }

    public virtual ICollection<Service> Services { get; set; } = new List<Service>();
}