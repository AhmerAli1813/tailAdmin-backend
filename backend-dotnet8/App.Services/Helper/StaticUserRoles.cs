namespace App.Services.Helper;

// This class will be used to avoid typing errors
public static class StaticUserRoles
{
    public const string SuperAdmin = "SuperAdmin";
    public const string Admin = "Admin";
    public const string Manager = "Manager";
    public const string AppUser = "AppUser";
    public const string SuperAdminAdmin = "SuperAdmin,ADMIN";
    public const string SuperAdminAdminManager = "SuperAdmin,ADMIN,MANAGER";
    public const string SuperAdminAdminManagerUser = "SuperAdmin,ADMIN,MANAGER,USER";
}
public enum userRole
{
    SuperAdmin = 1,
    Admin = 2,
    Manager = 3,
    AppUser = 4
}