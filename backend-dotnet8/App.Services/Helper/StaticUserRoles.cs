namespace App.Services.Helper;

// This class will be used to avoid typing errors
public static class StaticUserRoles
{
    public const string SuperAdmin = "SuperAdmin";
    public const string Admin = "Admin";
    public const string RegionalManager = "RegionalManager";
    public const string WealthManager = "WealthManager";
    public const string RelationshipManager = "RelationshipManager"; // Fixed typo
    public const string AppUser = "AppUser";

    public const string CountryHeadAdminRole = $"{SuperAdmin},{Admin}";
    public const string SuperAdmin_Admin_Manager = $"{SuperAdmin},{Admin},{RegionalManager}";
    public const string SuperAdmin_Admin_RegionalManager_WealthManager = $"{SuperAdmin},{Admin},{RegionalManager},{WealthManager}";
    public const string SuperAdmin_Admin_RegionalManager_WealthManager_RelationshipManager = $"{SuperAdmin},{Admin},{RegionalManager},{WealthManager},{RelationshipManager}";
}

public enum userRole
{
    SuperAdmin = 1,
    Admin = 2,
    RegionalManager = 3,
    WealthManager = 4,
    RelationshipManager = 5, // Fixed typo
    AppUser = 6
}
