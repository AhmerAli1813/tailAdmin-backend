using App.Services.Helper;
using System.ComponentModel.DataAnnotations;

namespace App.Services.Dto.Auth;

public class UpdateRoleDto
{
    [Required(ErrorMessage = " UserName is required")]
    public string UserName { get; set; }
    public userRole NewRole { get; set; }

}


