using App.Services.Dto.Log;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace App.Services.Interface;

public interface ILogService
{
    Task<GetLogsDetail> GetLogsAsync();
    Task<GetLogsDetail> GetMyLogsAsync(ClaimsPrincipal User);
    Task SaveRequestLog(HttpContext httpContext, OpenApiReqLogDto req);
    Task<bool> SaveEmailLog(HttpContext httpContext, OpenEmailLogDto req);
    void TextLog(string message, string ContentPath, string? FolderName = null);
    Task<long> LogInformation(string message, string? UserName, Dictionary<string, object>? properties = null);
    Task<long> LogWarning(string message, string? UserName, Dictionary<string, object>? properties = null);
    Task<long> LogError(string message, string? UserName, Dictionary<string, object>? properties = null);
    Task<long> LogException(string message, string? UserName, Exception exception, Dictionary<string, object>? properties = null);
}