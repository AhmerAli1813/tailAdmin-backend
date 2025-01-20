using Microsoft.EntityFrameworkCore;

namespace App.Infrastructure
{
    public interface IUnitOfWork<TContext> where TContext : DbContext
    {
        IRepository<T> GenericRepository<T>() where T : class;
        void Save();
        Task SaveAsync();
    }

}
