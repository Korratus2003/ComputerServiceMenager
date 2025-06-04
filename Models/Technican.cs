using Avalonia.Media.Imaging;
using Avalonia.Platform;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.IO;

[Table("Technicians")]
public class Technician
{
    [Key]
    public int Id { get; set; }

    [Required, MaxLength(255)]
    public string Name { get; set; } = null!;

    [Required, MaxLength(255)]
    public string Surname { get; set; } = null!;

    [MaxLength(20)]
    public string? PhoneNumber { get; set; }

    public DateTimeOffset? EmploymentDate { get; set; }

    public bool IsActive { get; set; }
    
    [MaxLength(2083)]
    public string? ImageUrl { get; set; }

    public virtual ICollection<Service> Services { get; set; } = new List<Service>();
    
    public virtual ICollection<User>? Users { get; set; } = new List<User>();
    
    [NotMapped]
    public Bitmap? Image
    {
        get
        {
            if (string.IsNullOrEmpty(ImageUrl))
                return null;

            try
            {
                if (ImageUrl.StartsWith("avares://"))
                {
                    return new Bitmap(AssetLoader.Open(new Uri(ImageUrl)));
                }
                else if (File.Exists(ImageUrl))
                {
                    return new Bitmap(ImageUrl);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Błąd ładowania obrazu: {ex.Message}");
            }

            return null;
        }
    }
}