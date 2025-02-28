using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace App.DataAccessLayer.EntityModel.SQL.Model
{
    public class ApplicationUser : IdentityUser
    {
        public string? CNIC { get; set; }
        public string? FullName { get; set; }
        public string? Designation { get; set; }
        public string? Address { get; set; }
        public bool DefaultPassword { get; set; } = true;
        public string? DC_Code { get; set; } // Only for Regional Managers
        public string? RegionHeadId { get; set; } // Regional Head managing the region
        public string? LineManagerId { get; set; } // Direct reporting manager
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        // Foreign key relationship to Region entity
        [ForeignKey(nameof(Region))]
        public int? RegionId { get; set; }
        public Region? Region { get; set; }

        // Navigation properties for hierarchy
        [NotMapped]
        public virtual ApplicationUser? RegionHead { get; set; }

        [NotMapped]
        public virtual ApplicationUser? LineManager { get; set; }

        public ICollection<ApplicationUser>? Subordinates { get; set; } // Team members under this user
    }

}
