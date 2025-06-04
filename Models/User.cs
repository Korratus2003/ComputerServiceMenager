using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public enum UserRange
{
    Admin,
    Technician,
    Manager
}

[Table("Users")]
public class User
{
    [Key]
    public int Id { get; set; }

    [ForeignKey(nameof(Technician))]
    public int? TechnicianId { get; set; }

    [Required, MaxLength(50)]
    public string Login { get; set; } = null!;

    [Required, MaxLength(255)]
    public string PasswordHash { get; set; } = null!;

    [Required]
    public UserRange Range { get; set; }

    public virtual Technician? Technician { get; set; }
}