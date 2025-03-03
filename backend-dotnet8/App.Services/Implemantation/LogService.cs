
using App.DataAccessLayer.EntityModel.SQL.Data;
using App.DataAccessLayer.EntityModel.SQL.Model;
using App.Infrastructure;
using App.Services.Dto.Log;
using App.Services.Helper;
using App.Services.Interface;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace App.Services.Implemantation;

public class LogService : ILogService
{
    #region Constructor & DI
    private readonly IUnitOfWork _context;

    public LogService(IUnitOfWork context)
    {
        _context = context;
    }




    #endregion

    #region SaveNewLog
    public async Task SaveNewLog(string UserName, string Description)
    {
        var newLog = new AppsLog()
        {
            UserName = UserName,
            Message = Description,
            CreatedAt = DateTime.Now,
            Level = "Information",

        };

        await _context.GenericRepository<AppsLog>().AddAsync(newLog);
        await _context.SaveAsync();
    }
    #endregion

    #region GetLogsAsync
    public async Task<GetLogsDetail> GetLogsAsync()
    {
        GetLogsDetail resp = new GetLogsDetail();
        var logs = await _context.GenericRepository<AppsLog>().GetAllAsync(orderby: o => o.OrderByDescending(x => x.CreatedAt));
        var list = ModelConverter.ConvertTo<AppsLog, GetLogDto>(logs);
        resp.List = list;
        var Toal = _context.SqlQuery<LogCountDto>("Select count(*) Total, Level  from AppsLogs a group by Level");
        resp.Total = Toal;
        return resp;

    }
    #endregion

    #region GetMyLogsAsync
    public async Task<GetLogsDetail> GetMyLogsAsync(ClaimsPrincipal User)
    {
        GetLogsDetail resp = new GetLogsDetail();
        var logs = await _context.GenericRepository<AppsLog>().GetAllAsync(q => q.UserName == User.Identity.Name, orderby: x => x.OrderByDescending(a => a.CreatedAt));
        var list = ModelConverter.ConvertTo<AppsLog, GetLogDto>(logs);
        resp.List = list;
        var Toal = _context.SqlQuery<LogCountDto>("Select count(*) Total, Level  from AppsLogs a group by Level");
        resp.Total = Toal;
        return resp;

    }



    public async Task SaveRequestLog(HttpContext httpContext, OpenApiReqLogDto req)
    {
        var data = ModelConverter.ConvertTo<OpenApiReqLogDto, OpenApiReqLog>(req);
        data.MachineAddress = CommonHelper.GetIPAddress(httpContext);
        data.UserAgent = CommonHelper.GetComputerName(httpContext);

        await _context.GenericRepository<OpenApiReqLog>().AddAsync(data);
        await _context.SaveAsync();
    }

    public void TextLog(string message, string ContentPath, string? FolderName = null)
    {
        try
        {
            string date = DateTime.Now.ToString("dd-MM-yyyy");
            string mDirectory = string.IsNullOrEmpty(FolderName) ? "ErrorLog" : FolderName;
            string logfile = Path.Combine(ContentPath, (@$"{mDirectory}\") + date + ".txt");

            if (!Directory.Exists(Path.Combine(ContentPath, (mDirectory))))
            {
                Directory.CreateDirectory(Path.Combine(ContentPath, mDirectory));
            }
            using var tw = new StreamWriter(logfile, true);
            tw.WriteLine(DateTime.Now + "\n" + message + "\n");

        }
        catch (Exception)
        {
            throw;
        }
    }

    public async Task<bool> SaveEmailLog(HttpContext httpContext, OpenEmailLogDto req)
    {
        try
        {
            var data = ModelConverter.ConvertTo<OpenEmailLogDto, OpenEmailLog>(req);
            data.MachineAddress = CommonHelper.GetIPAddress(httpContext);
            data.UserAgent = CommonHelper.GetComputerName(httpContext);
            await _context.GenericRepository<OpenEmailLog>().AddAsync(data);
            await _context.SaveAsync();

            return true;
        }
        catch (Exception ex)
        {
            return false;
        }

    }
    public async Task<long> LogInformation(string message, string? UserName = null, Dictionary<string, object>? properties = null)
    {
        return await SaveLogToDatabase("Information", UserName, message, null, properties);
    }

    public async Task<long> LogWarning(string message, string? UserName = null, Dictionary<string, object>? properties = null)
    {
        return await SaveLogToDatabase("Warning", UserName, message, null, properties);
    }

    public async Task<long> LogError(string message, string? UserName, Dictionary<string, object>? properties = null)
    {
        return await SaveLogToDatabase("Error", UserName, message, null, properties);
    }
    public async Task<long> LogException(string message, string? UserName, Exception exception, Dictionary<string, object>? properties = null)
    {
        return await SaveLogToDatabase("Exception", UserName, message, exception, properties);
    }

    private async Task<long> SaveLogToDatabase(string level, string? UserName, string message, Exception? exception, Dictionary<string, object>? properties)
    {
        var log = new AppsLog
        {
            CreatedAt = DateTime.Now,
            Level = level,
            Message = message,
            Exception = exception?.ToString(),
            Properties = properties != null
                ? System.Text.Json.JsonSerializer.Serialize(properties)
                : null,
            UserName = UserName

        };

        await _context.GenericRepository<AppsLog>().AddAsync(log);
        _context.Save();
        return log.Id;
    }
    #endregion

}
