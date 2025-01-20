
using App.Services.Dto.Auth;
using App.Services.Dto.General;
using System.Security.Claims;

namespace App.Services.Interface;

public interface IAccountService
{
    Task<ResponseDto> RegisterAsync(RegisterDto registerDto);
    Task<LoginServiceResponseDto?> LoginAsync(LoginDto loginDto);
    Task<ResponseDto> UpdateRoleAsync(ClaimsPrincipal User, UpdateRoleDto updateRoleDto);
    Task<LoginServiceResponseDto?> MeAsync(MeDto meDto);
    Task<IEnumerable<UserInfoResult>> GetUsersListAsync();
    Task<UserInfoResult?> GetUserDetailsByUserNameAsync(string userName);
    Task<IEnumerable<string>> GetUsernamesListAsync();
}
