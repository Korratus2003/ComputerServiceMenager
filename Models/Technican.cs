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
    public int Id { get; set; }

    [Required, MaxLength(255)]
    public string Name { get; set; }

    [Required, MaxLength(255)]
    public string Surname { get; set; }

    [MaxLength(20)]
    public string PhoneNumber { get; set; }

    public DateTimeOffset? EmploymentDate { get; set; }

    public bool IsActive { get; set; }
    
    [MaxLength(2083)]
    public string? ImageUrl { get; set; }

    public ICollection<Service> Services { get; set; }
    
    public ICollection<User>? User { get; set; }

    // Automatyczne rzutowanie ImageUrl na Bitmap
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