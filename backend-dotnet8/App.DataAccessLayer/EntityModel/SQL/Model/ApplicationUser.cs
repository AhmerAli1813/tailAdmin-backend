using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace App.DataAccessLayer.EntityModel.SQL.Model
{
    public class ApplicationUser : IdentityUser
    {
        public string? CINC { get; set; }
        public string? FolioNumber { get; set; }
        public string? FatherName { get; set; }
        public string? MotherName { get; set; }
        public string? FullName { get; set; }
        public string? Address { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        [NotMapped]
        public IList<string> Roles { get; set; }
    }
}
