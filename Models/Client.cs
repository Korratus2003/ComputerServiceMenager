using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

[Table("Clients")]
public class Client
{
    public int Id { get; set; }

    [Required, MaxLength(255)]
    public string Name { get; set; }

    [Required, MaxLength(255)]
    public string Surname { get; set; }

    [MaxLength(20)]
    public string PhoneNumber { get; set; }

    [MaxLength(255)]
    public string Email { get; set; }

    public bool VisitsRegularly { get; set; }

    public DateTime CreatedAt { get; set; }
    
    public ICollection<Service> Services { get; set; }
}