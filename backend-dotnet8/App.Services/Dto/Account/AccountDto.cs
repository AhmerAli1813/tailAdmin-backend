using App.Services.Helper;
using System.ComponentModel.DataAnnotations;

namespace App.Services.Dto.Account;



public class AccountDto
{
    public string? FullName { get; set; }
    public string? PhoneNumber { get; set; }


    [Required(ErrorMessage = "UserName is required")]
    public string? UserName { get; set; }
    [Required(ErrorMessage = "Email is required")]
    public string? Email { get; set; }

    public string? DC_Code { get; set; } // Only for Regional Managers
    public string? RegionHeadId { get; set; } // Regional Head managing the region
    public string? LineManagerId { get; set; } //
    public int? RegionId { get; set; } //
    public string? Designation { get; set; } //
    public string? Address { get; set; }
    public string? CNIC { get; set; }


}


public class UsersFilterDto
{
    public string? Email { get; set; }
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public string[]? UserRoles { get; set; }

}
public class UserInfoResult
{
    public string? Id { get; set; }
    public string? FullName { get; set; }
    public string? UserName { get; set; }
    public string? PhoneNumber { get; set; }
    public string? Address { get; set; }
    public string? DC_Code { get; set; } // Only for Regional Managers
    public string? RegionHeadId { get; set; } // Regional Head managing the region
    public string? LineManagerId { get; set; } //
    public int? RegionId { get; set; } //
    public string? Designation { get; set; } //
    public string? Email { get; set; }
    public DateTime CreatedAt { get; set; }
    public IEnumerable<string>? Roles { get; set; }
    public string? Role { get; set; }
}
public class UserInfoResultlist
{
    public string? Id { get; set; }
    public string? FullName { get; set; }
    public string? UserName { get; set; }
    public string? PhoneNumber { get; set; }
    public string? Address { get; set; }
    public string? DC_Code { get; set; } // Only for Regional Managers

    public string? Region { get; set; } //
    public string? Designation { get; set; } //
    public string? Email { get; set; }
    public DateTime CreatedAt { get; set; }
    public string? Role { get; set; }
    public bool LockoutEnabled { get; set; }
}
public class UpdateRoleDto
{
    [Required(ErrorMessage = " UserName is required")]
    public string? UserName { get; set; }
    public userRole NewRole { get; set; }

}

public class MeDto
{
    public string? Token { get; set; }
}
public class LoginServiceResponseDto
{
    public string? NewToken { get; set; }
    public int ResponseCode { get; set; }
    // This would be returned to front-end
    public UserInfoResult? UserInfo { get; set; }
}

public class LoginDto
{
    [Required(ErrorMessage = "UserName is required")]
    public string? UserName { get; set; }

    [Required(ErrorMessage = "Password is required")]
    public string? Password { get; set; }
}
public class UserStatusDto
{
    [Required(ErrorMessage = "UserName is required")]
    public string? UserName { get; set; }
    [Required(ErrorMessage = "Status  is required")]

    public bool LockoutEnabled { get; set; }
}
public class ResetPasswordSetDefaultDto
{
    [Required(ErrorMessage = "UserName is required")]
    public string? UserName { get; set; }
}

public class SelectDropDownDto
{
    public string? Id { get; set; }
    public string? Name { get; set; }
}
public class UserNameListDto
{
    public IEnumerable<SelectDropDownDto>? RegionalUser { get; set; }
    public IEnumerable<SelectDropDownDto>? RelationShipUser { get; set; }
    public IEnumerable<SelectDropDownDto>? Regions { get; set; }
}
