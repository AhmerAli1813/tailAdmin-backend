
using App.DataAccessLayer.EntityModel.SQL.Data;
using App.DataAccessLayer.EntityModel.SQL.Model;
using App.Infrastructure;
using App.Services.Dto.Log;
using App.Services.Helper;
using App.Services.Interface;
using System.Security.Claims;

namespace App.Services.Implemantation;

public class LogService : ILogService
{
    #region Constructor & DI
    private readonly IUnitOfWork<JSIL_IdentityDbContext> _context;

    public LogService(IUnitOfWork<JSIL_IdentityDbContext> context)
    {
        _context = context;
    }

    #endregion

    #region SaveNewLog
    public async Task SaveNewLog(string UserName, string Description)
    {
        var newLog = new Log()
        {
            UserName = UserName,
            Description = Description
        };

        await _context.GenericRepository<Log>().AddAsync(newLog);
        await _context.SaveAsync();
    }
    #endregion

    #region GetLogsAsync
    public async Task<IEnumerable<GetLogDto>> GetLogsAsync()
    {
        var logs = await _context.GenericRepository<Log>().GetAllAsync(orderby: o => o.OrderByDescending(x => x.CreatedAt));
        return ModelConverter.ConvertTo<Log, GetLogDto>(logs);

    }
    #endregion

    #region GetMyLogsAsync
    public async Task<IEnumerable<GetLogDto>> GetMyLogsAsync(ClaimsPrincipal User)
    {
        var logs = await _context.GenericRepository<Log>().GetAllAsync(q => q.UserName == User.Identity.Name, orderby: x => x.OrderByDescending(a => a.CreatedAt));
        return ModelConverter.ConvertTo<Log, GetLogDto>(logs);

    }
    #endregion

}
