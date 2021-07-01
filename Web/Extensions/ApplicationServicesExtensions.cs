using eShop_Mvc.Core.Interfaces;
using eShop_Mvc.Core.Services;
using eShop_Mvc.Infrastructure.Data;
using eShop_Mvc.SharedKernel.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace eShop_Mvc.Extensions
{
    public static class ApplicationServicesExtensions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            // Repository pattern and unit of work
            services.AddScoped(typeof(IRepository<,>), typeof(EfRepository<,>));
            services.AddTransient(typeof(IUnitOfWork), typeof(EfUnitOfWork));

            // Services
            services.AddTransient<IProductCategoryService, ProductCategoryService>();
            services.AddTransient<IFunctionService, FunctionService>();
            services.AddTransient<IProductService, ProductService>();
            services.AddTransient<IRoleService, RoleService>();
            services.AddTransient<IBillService, BillService>();

            return services;
        }
    }
}