using Microsoft.AspNetCore.Identity;

namespace App.DataAccessLayer.EntityModel.SQL.Model
{
    public class ApplicationUser : IdentityUser
    {
        public string? CNIC { get; set; }
        public string? FullName { get; set; }
        public string? Designation { get; set; }
        public string? Address { get; set; }
        public bool DefaultPassword { get; set; } = true;
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        


    }

}
