
using App.DataAccessLayer.EntityModel.SQL.Data;
using App.DataAccessLayer.EntityModel.SQL.Model;
using App.Infrastructure;
using App.Services.Dto.Account;
using App.Services.Dto.General;
using App.Services.Helper;
using App.Services.Interface;
using Microsoft.AspNetCore.Http;
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
    private readonly ILogService _logService;
    private readonly IConfiguration _configuration;
    private readonly IUnitOfWork _unitOfWork;

    public AccountService(UserManager<ApplicationUser> userManager, ILogService logService, IConfiguration configuration, IUnitOfWork unitOfWork)
    {
        _userManager = userManager;
        _logService = logService;
        _configuration = configuration;
        _unitOfWork = unitOfWork;
    }



    #endregion

    #region UpdateAsycx
    public async Task<ResponseDto> UpdateAsync(AccountDto model)
    {
        var existingUser = await _userManager.FindByNameAsync(model.UserName);
        if (existingUser is null)
        {
            return new ResponseDto()
            {
                IsSucceed = false,
                StatusCode = 409,
                Message = "UserName doesn't exist"
            };
        }
        // Update properties of the existing user
        existingUser.FullName = model.FullName;
        existingUser.Email = model.Email;
        existingUser.Address = model.Address;
        existingUser.PhoneNumber = model.PhoneNumber;
        existingUser.Designation = model.Designation;

        var updateUserResult = await _userManager.UpdateAsync(existingUser);

        if (!updateUserResult.Succeeded)
        {
            var errorString = "User update failed because: ";
            foreach (var error in updateUserResult.Errors)
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

        // Log the update operation
        await _logService.LogInformation("Update to Website", existingUser.UserName);

        return new ResponseDto()
        {
            IsSucceed = true,
            StatusCode = 200,
            Message = "User updated successfully"
        };
    }

    #endregion
    #region RegisterAsync
    public async Task<ResponseDto> RegisterAsync(AccountDto model)
    {
        var isExistsUser = await _userManager.FindByNameAsync(model.UserName);
        if (isExistsUser is not null)
            return new ResponseDto()
            {
                IsSucceed = false,
                StatusCode = StatusCodes.Status409Conflict,
                Message = "UserName Already Exists"
            };
        ApplicationUser newUser = ModelConverter.ConvertTo<AccountDto, ApplicationUser>(model);
        newUser.SecurityStamp = Guid.NewGuid().ToString();
        newUser.DefaultPassword = true;

        var createUserResult = await _userManager.CreateAsync(newUser, "P@ss1234");

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
        await _logService.LogInformation("Registered to Website", newUser.UserName);

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
        // Initialize response object
        LoginServiceResponseDto resp = new LoginServiceResponseDto();

        #region User Validation

        // Commit: Find user with username
        var user = await _userManager.FindByNameAsync(loginDto.UserName);
        if (user is null)
        {
            // Commit: Return unauthorized if user is not found
            resp.ResponseCode = StatusCodes.Status401Unauthorized;
            return resp;
        }

        // Commit: Check if user account is enabled
        bool enableUser = await _userManager.GetLockoutEnabledAsync(user);
        if (!enableUser)
        {
            // Commit: Return locked response if user is disabled
            resp.ResponseCode = StatusCodes.Status423Locked;
            return resp;
        }

        #endregion

        #region Password Validation

        // Commit: Check user's password
        var isPasswordCorrect = await _userManager.CheckPasswordAsync(user, loginDto.Password);
        if (!isPasswordCorrect)
        {
            // Commit: Increment failed login count and check access attempts
            await _userManager.AccessFailedAsync(user);
            var countFailed = await _userManager.GetAccessFailedCountAsync(user);

            if (countFailed > 3)
            {
                bool isSuperAdmin = await _userManager.IsInRoleAsync(user, userRole.SuperAdmin.ToString());
                // Commit: Lock the account after 5 failed attempts
                resp.ResponseCode = StatusCodes.Status429TooManyRequests;
                if (!isSuperAdmin)
                    await _userManager.SetLockoutEnabledAsync(user, false);

            }
            else
            {
                // Commit: Failed dependency response for incorrect password
                resp.ResponseCode = StatusCodes.Status424FailedDependency;
            }
            return resp;
        }

        // Commit: Reset failed access count on successful login
        await _userManager.ResetAccessFailedCountAsync(user);

        #endregion

        #region Successful Login

        // Commit: Retrieve user roles
        var roles = await _userManager.GetRolesAsync(user);

        // Commit: Generate user information object
        var userInfo = GenerateUserInfoObject(user, roles);

        // Commit: Log the successful login
        await _logService.LogInformation("New Login", user.UserName);

        // Commit: Generate JWT token for the user
        resp.ResponseCode = StatusCodes.Status200OK;
        resp.NewToken = await GenerateJWTTokenAsync(user);
        resp.UserInfo = userInfo;

        #endregion

        return resp;
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
            if (updateRoleDto.NewRole == userRole.AppUser || updateRoleDto.NewRole == userRole.Investor || updateRoleDto.NewRole == userRole.Salesman)
            {
                // admin can change the role of everyone except for owners and admins
                if (userRoles.Any(q => q.Equals(userRole.Admin.ToString()) || q.Equals(userRole.SuperAdmin.ToString())))
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
                    await _logService.LogInformation("User Roles Updated", user.UserName);
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
                await _logService.LogInformation("User Roles Updated", user.UserName);

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

    #region GenerateTokenInternalAsync
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
        LoginServiceResponseDto resp = new LoginServiceResponseDto();
        string decodedUserName = handler.Claims.First(q => q.Type == ClaimTypes.Name).Value;
        if (decodedUserName is null)
        {
            resp.ResponseCode = StatusCodes.Status401Unauthorized;
            return resp;
        }

        var user = await _userManager.FindByNameAsync(decodedUserName);

        if (user is null)
        {
            resp.ResponseCode = StatusCodes.Status401Unauthorized;
            return resp;
        }
        // Commit: Check if user account is enabled
        bool enableUser = await _userManager.GetLockoutEnabledAsync(user);
        if (!enableUser)
        {
            // Commit: Return locked response if user is disabled
            resp.ResponseCode = StatusCodes.Status423Locked;
            return resp;
        }


        var newToken = await GenerateJWTTokenAsync(user);
        var roles = await _userManager.GetRolesAsync(user);
        var userInfo = GenerateUserInfoObject(user, roles);
        await _logService.LogInformation("New Token Generated", user.UserName);

        return new LoginServiceResponseDto()
        {
            ResponseCode = StatusCodes.Status200OK,
            NewToken = newToken,
            UserInfo = userInfo
        };
    }
    #endregion

    #region GetUsersListAsync
    public async Task<IEnumerable<UserInfoResultlist>> GetUsersListAsync(UsersFilterDto filter)
    {

        string qry = @$"Select u.Id, u.Address, u.Email, u.FullName, u.UserName,  u.PhoneNumber, u.CreatedAt, r.Name as Role,u.LockoutEnabled,
                        u.Designation
                        from AspNetUsers u
                        left join AspNetUserRoles ur on u.Id = ur.UserId
                        left join AspNetRoles r on ur.RoleId = r.Id
				       ";

        List<string> filters = new List<string>();

        if (!string.IsNullOrWhiteSpace(filter.Email))
            filters.Add($"u.Email = '{filter.Email}'");

        if (filter.StartDate != null)
            filters.Add($"u.CreatedAt >= '{filter.StartDate:yyyy-MM-dd}'");

        if (filter.EndDate != null)
        {
            filter.EndDate = filter.EndDate.Value.AddDays(1);
            filters.Add($"u.CreatedAt < '{filter.EndDate:yyyy-MM-dd}'"); // Use '<' for exclusive date range
        }

        if (filter.UserRoles != null && filter.UserRoles.Any())
        {
            string roles = string.Join(", ", filter.UserRoles.Select(r => $"'{r}'"));
            filters.Add($"r.Name IN ({roles})");
        }

        // Combine all filters into the WHERE clause if there are any
        string FilterQry = filters.Count > 0 ? " WHERE " + string.Join(" AND ", filters) : "";

        // Final query
        string finalQuery = qry + FilterQry;


        var data = _unitOfWork.SqlQuery<UserInfoResultlist>(finalQuery);
        //var data = await _dbContext.Database.SqlQueryRaw<UserInfoResultlist>(finalQuery).ToListAsync();
        return data;
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
            new Claim("Designation", string.IsNullOrEmpty(user.Designation)?"--":user.Designation),
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
            Address = user.Address,
            Designation = user.Designation,

            PhoneNumber = user.PhoneNumber,
            CreatedAt = user.CreatedAt,
            Roles = Roles
        };
    }


    public async Task<ResponseDto> LockoutEnabled(UserStatusDto model)
    {
        ResponseDto resp = new ResponseDto();
        var user = await _userManager.FindByNameAsync(model.UserName);
        if (user is null)
        {
            resp.IsSucceed = false;
            resp.Message = "User not found";
            resp.StatusCode = StatusCodes.Status404NotFound;
        }
        await _userManager.SetLockoutEnabledAsync(user, model.LockoutEnabled);
        string Mes = model.LockoutEnabled ? "Unblock" : "Block";
        resp.Message = $"User {Mes} Successfully";
        resp.StatusCode = StatusCodes.Status200OK;
        return resp;
    }

    public async Task<ResponseDto> ResetPasswordSetDefault(ResetPasswordSetDefaultDto model)
    {

        ResponseDto resp = new ResponseDto();
        var user = await _userManager.FindByNameAsync(model.UserName);
        if (user is null)
        {
            resp.IsSucceed = false;
            resp.Message = "User not found";
            resp.StatusCode = StatusCodes.Status404NotFound;
        }
        user.DefaultPassword = true;
        user.SecurityStamp = Guid.NewGuid().ToString();
        await _userManager.UpdateAsync(user);
        string Token = await _userManager.GeneratePasswordResetTokenAsync(user);
        await _userManager.ResetPasswordAsync(user, Token, "P@ss1234");
        resp.Message = "Reset Password Successfully";
        resp.StatusCode = StatusCodes.Status200OK;
        return resp;
    }




    #endregion
    public async Task<ResponseDto> GetAllRegionalAndRelationshipUsersAsync()
    {
        ResponseDto resp = new ResponseDto();
        var data = new UserNameListDto();

        data.Regions = _unitOfWork.SqlQuery<SelectDropDownDto>("SELECT CAST(Id AS VARCHAR)as Id,  Name FROM Regions");
        resp.Result = data;
        return resp;
    }



}
