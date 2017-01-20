using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using ModernStore.Controllers;
using ModernStore.Data.Contexts;
using ModernStore.Data.Repositories;
using ModernStore.Data.Transaction;
using ModernStore.Domain.Handlers;
using ModernStore.Domain.Repositories;

namespace ModernStore
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<ModernStoreDataContext>(options => options.UseInMemoryDatabase());
            services.AddMvc();

            services.AddScoped<ModernStoreDataContext, ModernStoreDataContext>();
            services.AddTransient<IUnitOfWork, UnitOfWork>();
            services.AddTransient<ICustomerRepository, CustomerRepository>();
            services.AddTransient<CustomerCommandHandler, CustomerCommandHandler>();
            services.AddTransient<CustomerController, CustomerController>();
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole();

            if (env.IsDevelopment())
                app.UseDeveloperExceptionPage();

            app.UseMvc();
        }
    }
}
