// Tabela użytkowników

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

[Table("Users")]
public class User
{
    public int Id { get; set; }
    public int TechnicianId { get; set; }

    [Required, MaxLength(50)]
    public string Login { get; set; }

    [Required, MaxLength(255)]
    public string PasswordHash { get; set; }

    [Required]
    public UserRange Range { get; set; }

    public Technician Technician { get; set; }
}