using Microsoft.EntityFrameworkCore;
using System.Data.Common;

namespace App.Infrastructure
{
    public interface IUnitOfWork<TContext> where TContext : DbContext
    {
        IRepository<T> GenericRepository<T>() where T : class;
        void Save();
        Task SaveAsync();
        IEnumerable<T> SqlQuery<T>(string query, List<DbParameter>? parameters = null);

        IEnumerable<T> ExecuteStoredProcedure<T>(string storedProcedureName, List<DbParameter>? parameters = null) where T : new();
    }

}
