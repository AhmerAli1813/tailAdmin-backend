using App.DataAccessLayer.EntityModel.SQL.Data;
using App.Infrastructure;
using App.Services.Implemantation;
using App.Services.Interface;

namespace App.API.Helper
{
    public class ServiceRegistrationHelper
    {
        public static void RegisterServices(IServiceCollection services)
        {

            // Dependency Injection
            services.AddScoped<IUnitOfWork<JSIL_IdentityDbContext>, UnitOfWork>();
            services.AddScoped<IAccountService, AccountService>();
            services.AddScoped<IDbInitializer, DbInitializer>();
            services.AddScoped<ILogService, LogService>();
            services.AddScoped<IEmailService, EmailService>();

        }
    }
}
