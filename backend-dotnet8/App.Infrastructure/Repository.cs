using App.DataAccessLayer.EntityModel.SQL.Data;
using Microsoft.EntityFrameworkCore;
using System.Data;
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
    public bool Exists(Expression<Func<T, bool>> filter)
    {
        return _dbset.AsNoTracking().Any(filter);
    }

    public async Task<bool> ExistsAsync(Expression<Func<T, bool>> filter)
    {
        return await _dbset.AsNoTracking().AnyAsync(filter);
    }
}

