
using App.DataAccessLayer.EntityModel.SQL.Model;
using App.Services.Dto.Auth;
using App.Services.Dto.General;
using App.Services.Helper;
using App.Services.Interface;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace App.Services.Implemantation;

public class AccountService : IAccountService
{
    #region Constructor & DI
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly ILogService _logService;
    private readonly IConfiguration _configuration;

    public AccountService(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, ILogService logService, IConfiguration configuration)
    {
        _userManager = userManager;
        _roleManager = roleManager;
        _logService = logService;
        _configuration = configuration;
    }
    #endregion

    #region RegisterAsync
    public async Task<ResponseDto> RegisterAsync(RegisterDto registerDto)
    {
        var isExistsUser = await _userManager.FindByNameAsync(registerDto.UserName);
        if (isExistsUser is not null)
            return new ResponseDto()
            {
                IsSucceed = false,
                StatusCode = 409,
                Message = "UserName Already Exists"
            };

        ApplicationUser newUser = new ApplicationUser()
        {
            Email = registerDto.Email,
            UserName = registerDto.UserName,
            Address = registerDto.Address,
            SecurityStamp = Guid.NewGuid().ToString()
        };

        var createUserResult = await _userManager.CreateAsync(newUser, registerDto.Password);

        if (!createUserResult.Succeeded)
        {
            var errorString = "User Creation failed because: ";
            foreach (var error in createUserResult.Errors)
            {
                errorString += " # " + error.Description;
            }
            return new ResponseDto()
            {
                IsSucceed = false,
                StatusCode = 400,
                Message = errorString
            };
        }

        // Add a Default USER Role to all users
        await _userManager.AddToRoleAsync(newUser, userRole.AppUser.ToString());
        await _logService.SaveNewLog(newUser.UserName, "Registered to Website");

        return new ResponseDto()
        {
            IsSucceed = true,
            StatusCode = 201,
            Message = "User Created Successfully"
        };
    }
    #endregion

    #region LoginAsync
    public async Task<LoginServiceResponseDto?> LoginAsync(LoginDto loginDto)
    {
        // Find user with username
        var user = await _userManager.FindByNameAsync(loginDto.UserName);
        if (user is null)
            return null;

        // check password of user
        var isPasswordCorrect = await _userManager.CheckPasswordAsync(user, loginDto.Password);
        if (!isPasswordCorrect)
            return null;

        // Return Token and userInfo to front-end
        var newToken = await GenerateJWTTokenAsync(user);
        var roles = await _userManager.GetRolesAsync(user);
        var userInfo = GenerateUserInfoObject(user, roles);
        await _logService.SaveNewLog(user.UserName, "New Login");

        return new LoginServiceResponseDto()
        {
            NewToken = newToken,
            UserInfo = userInfo
        };
    }
    #endregion

    #region UpdateRoleAsync
    public async Task<ResponseDto> UpdateRoleAsync(ClaimsPrincipal User, UpdateRoleDto updateRoleDto)
    {
        var user = await _userManager.FindByNameAsync(updateRoleDto.UserName);
        if (user is null)
            return new ResponseDto()
            {
                IsSucceed = false,
                StatusCode = 404,
                Message = "Invalid UserName"
            };

        var userRoles = await _userManager.GetRolesAsync(user);
        // Just The OWNER and ADMIN can update roles
        if (User.IsInRole(userRole.Admin.ToString()))
        {
            // User is admin
            if (updateRoleDto.NewRole == userRole.AppUser || updateRoleDto.NewRole == userRole.Manager)
            {
                // admin can change the role of everyone except for owners and admins
                if (userRoles.Any(q => q.Equals(userRole.Manager.ToString()) || q.Equals(userRole.Admin.ToString())))
                {
                    return new ResponseDto()
                    {
                        IsSucceed = false,
                        StatusCode = 403,
                        Message = "You are not allowed to change role of this user"
                    };
                }
                else
                {
                    await _userManager.RemoveFromRolesAsync(user, userRoles);
                    await _userManager.AddToRoleAsync(user, updateRoleDto.NewRole.ToString());
                    await _logService.SaveNewLog(user.UserName, "User Roles Updated");
                    return new ResponseDto()
                    {
                        IsSucceed = true,
                        StatusCode = 200,
                        Message = "Role updated successfully"
                    };
                }
            }
            else return new ResponseDto()
            {
                IsSucceed = false,
                StatusCode = 403,
                Message = "You are not allowed to change role of this user"
            };
        }
        else
        {
            // user is owner
            if (userRoles.Any(q => q.Equals(userRole.SuperAdmin.ToString())))
            {
                return new ResponseDto()
                {
                    IsSucceed = false,
                    StatusCode = 403,
                    Message = "You are not allowed to change role of this user"
                };
            }
            else
            {
                await _userManager.RemoveFromRolesAsync(user, userRoles);
                await _userManager.AddToRoleAsync(user, updateRoleDto.NewRole.ToString());
                await _logService.SaveNewLog(user.UserName, "User Roles Updated");

                return new ResponseDto()
                {
                    IsSucceed = true,
                    StatusCode = 200,
                    Message = "Role updated successfully"
                };
            }
        }
    }
    #endregion

    #region MeAsync
    public async Task<LoginServiceResponseDto?> MeAsync(MeDto meDto)
    {
        ClaimsPrincipal handler = new JwtSecurityTokenHandler().ValidateToken(meDto.Token, new TokenValidationParameters()
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidIssuer = _configuration["JWT:ValidIssuer"],
            ValidAudience = _configuration["JWT:ValidAudience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"]))
        }, out SecurityToken securityToken);

        string decodedUserName = handler.Claims.First(q => q.Type == ClaimTypes.Name).Value;
        if (decodedUserName is null)
            return null;

        var user = await _userManager.FindByNameAsync(decodedUserName);
        if (user is null)
            return null;

        var newToken = await GenerateJWTTokenAsync(user);
        var roles = await _userManager.GetRolesAsync(user);
        var userInfo = GenerateUserInfoObject(user, roles);
        await _logService.SaveNewLog(user.UserName, "New Token Generated");

        return new LoginServiceResponseDto()
        {
            NewToken = newToken,
            UserInfo = userInfo
        };
    }
    #endregion

    #region GetUsersListAsync
    public async Task<IEnumerable<UserInfoResult>> GetUsersListAsync()
    {
        var users = await _userManager.Users.ToListAsync();

        List<UserInfoResult> userInfoResults = new List<UserInfoResult>();

        foreach (var user in users)
        {
            var roles = await _userManager.GetRolesAsync(user);
            var userInfo = GenerateUserInfoObject(user, roles);
            userInfoResults.Add(userInfo);
        }

        return userInfoResults;
    }
    #endregion

    #region GetUserDetailsByUserNameAsync
    public async Task<UserInfoResult?> GetUserDetailsByUserNameAsync(string userName)
    {
        var user = await _userManager.FindByNameAsync(userName);
        if (user is null)
            return null;

        var roles = await _userManager.GetRolesAsync(user);
        var userInfo = GenerateUserInfoObject(user, roles);
        return userInfo;
    }
    #endregion

    #region GetUsernamesListAsync
    public async Task<IEnumerable<string>> GetUsernamesListAsync()
    {
        var userNames = await _userManager.Users
            .Select(q => q.UserName)
            .ToListAsync();

        return userNames;
    }
    #endregion

    #region GenerateJWTTokenAsync
    private async Task<string> GenerateJWTTokenAsync(ApplicationUser user)
    {
        var userRoles = await _userManager.GetRolesAsync(user);

        var authClaims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, user.UserName),
            new Claim(ClaimTypes.NameIdentifier, user.Id),
            new Claim("FullName", user.FullName),
        };

        foreach (var userRole in userRoles)
        {
            authClaims.Add(new Claim(ClaimTypes.Role, userRole));
        }

        var authSecret = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"]));
        var signingCredentials = new SigningCredentials(authSecret, SecurityAlgorithms.HmacSha256);

        var tokenObject = new JwtSecurityToken(
            issuer: _configuration["JWT:ValidIssuer"],
            audience: _configuration["JWT:ValidAudience"],
            notBefore: DateTime.Now,
            expires: DateTime.Now.AddHours(3),
            claims: authClaims,
            signingCredentials: signingCredentials
            );

        string token = new JwtSecurityTokenHandler().WriteToken(tokenObject);
        return token;
    }
    #endregion

    #region GenerateUserInfoObject
    private UserInfoResult GenerateUserInfoObject(ApplicationUser user, IEnumerable<string> Roles)
    {
        // Instead of this, You can use Automapper packages. But i don't want it in this project
        return new UserInfoResult()
        {
            Id = user.Id,
            FullName = user.FullName,
            UserName = user.UserName,
            Email = user.Email,
            CreatedAt = user.CreatedAt,
            Roles = Roles
        };
    }
    #endregion

}
