using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

[Table("Technicians")]
public class Technician
{
    public int Id { get; set; }

    [Required, MaxLength(255)]
    public string Name { get; set; }

    [Required, MaxLength(255)]
    public string Surname { get; set; }

    [MaxLength(20)]
    public string PhoneNumber { get; set; }

    public DateTimeOffset? EmploymentDate { get; set; }

    public bool IsActive { get; set; }
    
    public ICollection<Service> Services { get; set; }
    
    public ICollection<User>? User { get; set; }
}