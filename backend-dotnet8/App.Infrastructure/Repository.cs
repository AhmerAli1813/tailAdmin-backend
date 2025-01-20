using App.DataAccessLayer.EntityModel.SQL.Data;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Data.Common;
using System.Linq.Expressions;

namespace App.Infrastructure;

public class Repository<T> : IRepository<T> where T : class
{
    private readonly JSIL_IdentityDbContext _context;
    internal DbSet<T> _dbset;

    public Repository(JSIL_IdentityDbContext context)
    {
        _context = context;
        _dbset = _context.Set<T>();
    }

    public void Add(T entity)
    {
        _dbset.Add(entity);
    }

    public async Task<T> AddAsync(T entity)
    {
        await _dbset.AddAsync(entity);
        return entity;
    }

    public void Delete(T entity)
    {
        if (_context.Entry(entity).State == EntityState.Detached)
        {
            _dbset.Attach(entity);
        }
        _dbset.Remove(entity);
    }

    public async Task<T> DeleteAsync(T entity)
    {
        if (_context.Entry(entity).State == EntityState.Detached)
        {
            _dbset.Attach(entity);
        }
        _dbset.Remove(entity);
        return entity;
    }

    public void DeleteRange(T entity)
    {
        _dbset.RemoveRange(entity);
    }

    public IEnumerable<T> GetAll(
        Expression<Func<T, bool>> filter = null,
        Func<IQueryable<T>, IOrderedQueryable<T>> orderby = null,
        string IncludeProperties = ""
        )
    {
        IQueryable<T> query = _dbset;

        if (filter != null)
        {
            query = query.AsNoTracking().Where(filter);
        }
        foreach (var Include in
            IncludeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
        {
            query = query.AsNoTracking().Include(Include);
        }
        if (orderby != null)
        {
            return orderby(query).AsNoTracking().ToList();
        }
        else
        {
            return query.AsNoTracking().ToList();
        }
    }

    public async Task<IEnumerable<T>> GetAllAsync(Expression<Func<T, bool>> filter = null,
        Func<IQueryable<T>, IOrderedQueryable<T>> orderby = null,
        string IncludeProperties = ""
      )
    {
        IQueryable<T> query = _dbset;

        if (filter != null)
        {
            query = query.AsNoTracking().Where(filter);
        }
        foreach (var Include in
            IncludeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
        {
            query = query.AsNoTracking().Include(Include);
        }
        if (orderby != null)
        {
            return await orderby(query).AsNoTracking().ToListAsync();
        }
        else
        {
            return await query.AsNoTracking().ToListAsync();
        }
    }

    public T GetT(Expression<Func<T, bool>> filter = null,
        Func<IQueryable<T>, IOrderedQueryable<T>> orderby = null,
        string IncludeProperties = "")
    {
        IQueryable<T> query = _dbset;

        if (filter != null)
        {
            query = query.AsNoTracking().Where(filter);
        }
        foreach (var Include in
            IncludeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
        {
            query = query.AsNoTracking().Include(Include);
        }
        if (orderby != null)
        {
            return orderby(query).AsNoTracking().FirstOrDefault();
        }
        else
        {
            return query.AsNoTracking().FirstOrDefault();
        }

    }

    public async Task<T> GetTAsync(Expression<Func<T, bool>> filter = null,
                            Func<IQueryable<T>,
                                IOrderedQueryable<T>> orderby = null,
                            string IncludeProperties = "")

    {

        IQueryable<T> query = _dbset;

        if (filter != null)
        {
            query = query.AsNoTracking().Where(filter);
        }
        foreach (var Include in
            IncludeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
        {
            query = query.AsNoTracking().Include(Include);
        }
        if (orderby != null)
        {
            return await orderby(query).AsNoTracking().FirstOrDefaultAsync();
        }
        else
        {
            return await query.AsNoTracking().FirstOrDefaultAsync();
        }
    }

    public void Update(T entity)
    {

        _dbset.Attach(entity);
        _context.Entry(entity).State = EntityState.Modified;
    }

    public async Task<T> UpdateAsync(T entity)
    {

        _dbset.Attach(entity);
        _context.Entry(entity).State = EntityState.Modified;
        return entity;
    }

    public IEnumerable<T> SqlQuery<T>(string query, List<DbParameter>? parameters = null)
    {
        if (string.IsNullOrEmpty(query))
        {
            throw new ArgumentException("Query cannot be null or empty.", nameof(query));
        }

        if (parameters == null || !parameters.Any())
        {
            return _context.Database.SqlQueryRaw<T>(query).ToList();
        }

        // Add parameters to the query execution
        return _context.Database.SqlQueryRaw<T>(query, parameters.ToArray()).ToList();
    }

    public IEnumerable<T> ExecuteStoredProcedure<T>(string storedProcedureName, List<DbParameter>? parameters = null) where T : new()
    {
        if (string.IsNullOrWhiteSpace(storedProcedureName))
            throw new ArgumentException("Stored procedure name cannot be null or empty.", nameof(storedProcedureName));

        using var command = _context.Database.GetDbConnection().CreateCommand();
        command.CommandText = storedProcedureName;
        command.CommandType = CommandType.StoredProcedure;

        if (parameters != null)
            foreach (var param in parameters)
                command.Parameters.Add(param);

        _context.Database.OpenConnection();
        using var reader = command.ExecuteReader();
        var result = new List<T>();

        while (reader.Read())
        {
            var value = typeof(T).IsValueType || typeof(T) == typeof(string)
                ? reader.GetFieldValue<T>(0)
                : MapToModel<T>(reader); // Calls MapToModel<T> for non-primitive types.
            result.Add(value);
        }

        return result;
    }

    private T MapToModel<T>(IDataReader reader) where T : new()
    {
        var obj = new T();
        var properties = typeof(T).GetProperties();

        foreach (var prop in properties)
        {
            if (!reader.IsDBNull(reader.GetOrdinal(prop.Name)))
            {
                var value = reader.GetValue(reader.GetOrdinal(prop.Name));
                prop.SetValue(obj, Convert.ChangeType(value, prop.PropertyType));
            }
        }

        return obj;
    }

}

