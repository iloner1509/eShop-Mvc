using eShop_Mvc.Core.Entities;
using eShop_Mvc.Core.Interfaces;
using eShop_Mvc.Core.Services;
using eShop_Mvc.Core.Services.Command.BillCommand;
using eShop_Mvc.Infrastructure.Data;
using eShop_Mvc.SharedKernel;
using eShop_Mvc.SharedKernel.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;
using eShop_Mvc.Core.Services.Query.BillQuery;
using eShop_Mvc.Core.Services.Query.CategoryQuery;
using eShop_Mvc.Core.Services.Query.FunctionQuery;
using eShop_Mvc.Core.Services.Query.TagQuery;

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

            // Host information
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            // CQRS
            // Command
            services.AddTransient<IRequestHandler<CreateBillCommand>, CreateBillCommandHandler>();
            services.AddTransient<IRequestHandler<UpdateBillCommand>, UpdateBillCommandHandler>();
            services.AddTransient<IRequestHandler<UpdateBillStatusCommand>, UpdateBillStatusCommandHandler>();
            // Query
            services
                .AddTransient<IRequestHandler<GetAllBillPagingQuery, PagedResult<Bill>>, GetAllBillPagingQueryHandler
                >();
            services.AddTransient<IRequestHandler<GetBillDetailByIdQuery, Bill>, GetBillDetailByIdQueryHandler>();
            services
                .AddTransient<IRequestHandler<GetListBillDetailByIdQuery, IReadOnlyList<BillDetail>>,
                    GetListBillDetailByIdQueryHandler>();
            services
                .AddTransient<IRequestHandler<GetAllFunctionWithFilterQuery, IReadOnlyList<Function>>,
                    GetAllFunctionWithFilterQueryHandler>();
            services.AddTransient<IRequestHandler<GetAllProductTagNameQuery, List<string>>, GetAllProductTagNameQueryHandler>();
            services
                .AddTransient<IRequestHandler<GetAllCategoryQuery, IReadOnlyList<ProductCategory>>,
                    GetAllCategoryQueryHandler>();
            return services;
        }
    }
}