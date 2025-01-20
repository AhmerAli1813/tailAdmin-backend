using App.DataAccessLayer.EntityModel.SQL.Data;
using App.DataAccessLayer.EntityModel.SQL.Model;
using App.Services.Helper;
using App.Services.Interface;
using Microsoft.AspNetCore.Identity;

namespace App.Services.Implemantation;

public class DbInitializer : IDbInitializer
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly JSIL_IdentityDbContext _context;

    public DbInitializer(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, JSIL_IdentityDbContext context)
    {
        _userManager = userManager;
        _roleManager = roleManager;
        _context = context;
    }

    public async Task InitializeAsync()
    {
        // Log the initialization process
        Console.WriteLine("Initializing database...");
        //if you doest have any role and user then commite these condition they help you to create a user and roles ,Condition if (!await _context.Database.EnsureCreatedAsync())
        // Check if the database exists
        if (!_context.Users.Any())
        {


            // Check and create roles
            foreach (var role in Enum.GetValues(typeof(userRole)).Cast<userRole>())
            {
                var roleExists = await _roleManager.RoleExistsAsync(role.ToString());
                if (!roleExists)
                {
                    await _roleManager.CreateAsync(new IdentityRole(role.ToString()));
                    Console.WriteLine($"Role {role} created.");
                }
                else
                {
                    Console.WriteLine($"Role {role} already exists.");
                }
            }

            // Create a dummy user if it doesn't exist
            var dummyUserEmail = "admin@outlook.com";
            var existingUser = await _userManager.FindByEmailAsync(dummyUserEmail);

            if (existingUser == null)
            {
                var user = new ApplicationUser
                {
                    UserName = "admin",
                    FullName = "Ahmer Ali",
                    Email = dummyUserEmail,
                    PhoneNumber = "1234567890",
                    EmailConfirmed = true,
                    FatherName = "Afzal",
                    Address="Karachi"
                };

                var result = await _userManager.CreateAsync(user, "Admin@123");
                if (result.Succeeded)
                {
                    // Assign a role to the dummy user
                    await _userManager.AddToRoleAsync(user, userRole.SuperAdmin.ToString());
                    Console.WriteLine("Dummy user created and assigned to admin role.");
                }
                else
                {
                    Console.WriteLine("Error creating dummy user: " + string.Join(", ", result.Errors.Select(e => e.Description)));
                }
            }
            else
            {
                Console.WriteLine("Dummy user already exists.");
            }
        }
    }

}
