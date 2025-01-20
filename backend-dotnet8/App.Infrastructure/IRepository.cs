using System.Data.Common;
using System.Linq.Expressions;

namespace App.Infrastructure
{
    public interface IRepository<T>
    {
        IEnumerable<T> GetAll(
           Expression<Func<T, bool>> filter = null,
           Func<IQueryable<T>, IOrderedQueryable<T>> orderby = null,
           string IncludeProperties = ""

           );
        Task<IEnumerable<T>> GetAllAsync(
            Expression<Func<T, bool>> filter = null,
            Func<IQueryable<T>, IOrderedQueryable<T>> orderby = null,
            string IncludeProperties = ""

            );
        T GetT(Expression<Func<T, bool>> filter = null,
            Func<IQueryable<T>, IOrderedQueryable<T>> orderby = null,
            string IncludeProperties = "");
        Task<T> GetTAsync(Expression<Func<T, bool>> filter = null,
            Func<IQueryable<T>, IOrderedQueryable<T>> orderby = null,
            string IncludeProperties = "");
        void Add(T entity);
        Task<T> AddAsync(T entity);
        void Update(T entity);
        Task<T> UpdateAsync(T entity);
        void Delete(T entity);
        Task<T> DeleteAsync(T entity);
        IEnumerable<T> SqlQuery<T>(string query, List<DbParameter>? parameters = null);

        IEnumerable<T> ExecuteStoredProcedure<T>(string storedProcedureName, List<DbParameter>? parameters = null) where T : new();

    }
}
