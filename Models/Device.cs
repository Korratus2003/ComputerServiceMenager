using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ComputerServiceManager.Database
{
    [Table("Devices")]
    public class Device
    {
        [Key]
        public int Id { get; set; }

        [Required, MaxLength(255)]
        public string Name { get; set; } = null!;

        [MaxLength(100)]
        public string? SerialNumber { get; set; }

        public string? Description { get; set; }

        public DateTimeOffset? AddedAt { get; set; }

        [ForeignKey(nameof(OwnerClient))]
        public int OwnerClientId { get; set; }

        [Required]
        public virtual Client OwnerClient { get; set; } = null!;

        public virtual ICollection<Service> Services { get; set; } = new List<Service>();
    }
}