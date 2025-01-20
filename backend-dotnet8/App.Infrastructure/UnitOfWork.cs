using App.DataAccessLayer.EntityModel.SQL.Data;


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

        public async Task SaveAsync()
        {
            await _DbContext.SaveChangesAsync();
        }
    }

}
