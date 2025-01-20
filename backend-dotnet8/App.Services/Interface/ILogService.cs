using App.Services.Dto.Log;
using System.Security.Claims;

namespace App.Services.Interface;

public interface ILogService
{
    Task SaveNewLog(string UserName, string Description);
    Task<IEnumerable<GetLogDto>> GetLogsAsync();
    Task<IEnumerable<GetLogDto>> GetMyLogsAsync(ClaimsPrincipal User);

}
