
using App.Services.Dto.Account;
using App.Services.Dto.General;
using System.Security.Claims;

namespace App.Services.Interface;

public interface IAccountService
{
    Task<ResponseDto> RegisterAsync(AccountDto registerDto);
    Task<ResponseDto> UpdateAsync(AccountDto registerDto);
    Task<ResponseDto> LockoutEnabled(UserStatusDto model);
    Task<ResponseDto> ResetPasswordSetDefault(ResetPasswordSetDefaultDto model);
    Task<LoginServiceResponseDto?> LoginAsync(LoginDto loginDto);
    Task<ResponseDto> UpdateRoleAsync(ClaimsPrincipal User, UpdateRoleDto updateRoleDto);
    Task<LoginServiceResponseDto?> MeAsync(MeDto meDto);
    Task<IEnumerable<UserInfoResultlist>> GetUsersListAsync(UsersFilterDto filter);
    Task<UserInfoResult?> GetUserDetailsByUserNameAsync(string userName);
    Task<IEnumerable<string>> GetUsernamesListAsync();
    Task<ResponseDto> GetAllRegionalAndRelationshipUsersAsync();


}
