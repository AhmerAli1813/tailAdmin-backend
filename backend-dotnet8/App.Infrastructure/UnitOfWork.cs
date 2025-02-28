using App.DataAccessLayer.EntityModel.SQL.Data;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Data.Common;


namespace App.Infrastructure
{
    public class UnitOfWork : IUnitOfWork<JSIL_IdentityDbContext>
    {
        private readonly JSIL_IdentityDbContext _DbContext;

        public UnitOfWork(JSIL_IdentityDbContext identityDbContext)
        {
            _DbContext = identityDbContext;
        }

        public IRepository<T> GenericRepository<T>() where T : class
        {
            return new Repository<T>(_DbContext);
        }

        public void Save()
        {
            _DbContext.SaveChanges();
        }

        public IEnumerable<T> SqlQuery<T>(string query, List<DbParameter>? parameters = null)
        {
            if (string.IsNullOrEmpty(query))
            {
                throw new ArgumentException("Query cannot be null or empty.", nameof(query));
            }

            if (parameters == null || !parameters.Any())
            {
                return _DbContext.Database.SqlQueryRaw<T>(query).ToList();
            }

            // Add parameters to the query execution
            return _DbContext.Database.SqlQueryRaw<T>(query, parameters.Select(p => (object)p).ToArray()).ToList();
        }

        public IEnumerable<T> ExecuteStoredProcedure<T>(string storedProcedureName, List<DbParameter>? parameters = null) where T : new()
        {
            if (string.IsNullOrWhiteSpace(storedProcedureName))
                throw new ArgumentException("Stored procedure name cannot be null or empty.", nameof(storedProcedureName));

            using var command = _DbContext.Database.GetDbConnection().CreateCommand();
            command.CommandText = storedProcedureName;
            command.CommandType = CommandType.StoredProcedure;

            if (parameters != null)
                foreach (var param in parameters)
                    command.Parameters.Add(param);

            _DbContext.Database.OpenConnection();
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

        public async Task SaveAsync()
        {
            await _DbContext.SaveChangesAsync();
        }
    }

}
