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
using eShop_Mvc.Core.Services.Command.CategoryCommand;
using eShop_Mvc.Core.Services.Command.FunctionCommand;
using eShop_Mvc.Core.Services.Command.PermissionCommand;
using eShop_Mvc.Core.Services.Command.ProductCommand;
using eShop_Mvc.Core.Services.Command.RoleCommand;
using eShop_Mvc.Core.Services.Query.AnnouncementQuery;
using eShop_Mvc.Core.Services.Query.BillQuery;
using eShop_Mvc.Core.Services.Query.CategoryQuery;
using eShop_Mvc.Core.Services.Query.FunctionQuery;
using eShop_Mvc.Core.Services.Query.PermissionQuery;
using eShop_Mvc.Core.Services.Query.ProductQuery;
using eShop_Mvc.Core.Services.Query.RoleQuery;
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
            services.AddTransient<IUserService, UserService>();
            services.AddTransient<IFunctionService, FunctionService>();

            // Host information
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            // CQRS
            // Command
            // Product command
            services.AddTransient<IRequestHandler<CreateProductCommand>, CreateProductCommandHandler>();
            services.AddTransient<IRequestHandler<DeleteFunctionCommand>, DeleteFunctionCommandHandler>();
            services.AddTransient<IRequestHandler<ImportProductByExcelFileCommand>, ImportProductByExcelFileCommandHandler>();
            services.AddTransient<IRequestHandler<UpdateProductCommand>, UpdateProductCommandHandler>();
            // Bill command
            services.AddTransient<IRequestHandler<CreateBillCommand>, CreateBillCommandHandler>();
            services.AddTransient<IRequestHandler<UpdateBillCommand>, UpdateBillCommandHandler>();
            services.AddTransient<IRequestHandler<UpdateBillStatusCommand>, UpdateBillStatusCommandHandler>();

            // Permission command
            services.AddTransient<IRequestHandler<SavePermissionCommand>, SavePermissionCommandHandler>();

            // Role Command
            services.AddTransient<IRequestHandler<CreateRoleCommand, bool>, CreateRoleCommandHandler>();
            services.AddTransient<IRequestHandler<DeleteRoleCommand, bool>, DeleteRoleCommandHandler>();
            services.AddTransient<IRequestHandler<UpdateRoleCommand, bool>, UpdateRoleCommandHandler>();
            // Category command
            services.AddTransient<IRequestHandler<CreateCategoryCommand>, CreateCategoryCommandHandler>();
            services.AddTransient<IRequestHandler<DeleteCategoryCommand>, DeleteCategoryCommandHandler>();
            services.AddTransient<IRequestHandler<UpdateCategoryCommand>, UpdateCategoryCommandHandler>();
            services.AddTransient<IRequestHandler<UpdateCategoryOrderCommand>, UpdateCategoryOrderCommandHandler>();
            services.AddTransient<IRequestHandler<UpdateCategoryParentIdCommand>, UpdateCategoryParentIdCommandHandler>();

            // Function command
            services.AddTransient<IRequestHandler<CreateFunctionCommand>, CreateFunctionCommandHandler>();
            services.AddTransient<IRequestHandler<UpdateFunctionCommand>, UpdateFunctionCommandHandler>();
            services.AddTransient<IRequestHandler<DeleteFunctionCommand>, DeleteFunctionCommandHandler>();
            services.AddTransient<IRequestHandler<UpdateFunctionParentIdCommand>, UpdateFunctionParentIdCommandHandler>();
            services.AddTransient<IRequestHandler<UpdateFunctionOrderCommand>, UpdateFunctionOrderCommandHandler>();

            // Query
            // Bill query
            services.AddTransient<IRequestHandler<GetAllBillPagingQuery, PagedResult<Bill>>, GetAllBillPagingQueryHandler>();
            services.AddTransient<IRequestHandler<GetBillWithDetailByIdQuery, Bill>, GetBillWithDetailByIdQueryHandler>();
            services.AddTransient<IRequestHandler<GetListBillDetailByBillIdQuery, IReadOnlyList<BillDetail>>, GetListBillDetailByBillIdQueryHandler>();
            services.AddTransient<IRequestHandler<ExportBillToExcelFileQuery, string>, ExportBillToExcelFileQueryHandler>();

            // Function query
            services.AddTransient<IRequestHandler<GetAllFunctionWithFilterQuery, IReadOnlyList<Function>>, GetAllFunctionWithFilterQueryHandler>();
            services.AddTransient<IRequestHandler<GetAllFunctionByListIdQuery, IReadOnlyList<Function>>, GetAllFunctionByListIdQueryHandler>();
            services.AddTransient<IRequestHandler<GetFunctionByIdQuery, Function>, GetFunctionByIdQueryHandler>();

            // Tag query
            services.AddTransient<IRequestHandler<GetAllProductTagNameQuery, List<string>>, GetAllProductTagNameQueryHandler>();
            services.AddTransient<IRequestHandler<GetTagByProductIdQuery, IReadOnlyList<Tag>>, GetTagByProductIdQueryHandler>();

            // Category query
            services.AddTransient<IRequestHandler<GetAllCategoryQuery, IReadOnlyList<ProductCategory>>, GetAllCategoryQueryHandler>();
            services.AddTransient<IRequestHandler<GetAllCategoryByParentIdQuery, IReadOnlyList<ProductCategory>>, GetAllCategoryByParentIdQueryHandler>();
            services.AddTransient<IRequestHandler<GetCategoryByIdQuery, ProductCategory>, GetCategoryByIdQueryHandler>();
            services.AddTransient<IRequestHandler<GetHomeCategoryQuery, IReadOnlyList<ProductCategory>>, GetHomeCategoryQueryHandler>();

            // Permission query
            services.AddTransient<IRequestHandler<CheckPermissionQuery, bool>, CheckPermissionQueryHandler>();
            services.AddTransient<IRequestHandler<GetPermissionByRoleIdQuery, IReadOnlyList<Permission>>, GetPermissionByRoleIdQueryHandler>();
            // Announcement query
            services.AddTransient<IRequestHandler<GetAllAnnouncementPagingQuery, PagedResult<Announcement>>, GetAllAnnouncementPagingQueryHandler>();
            // Product query
            services.AddTransient<IRequestHandler<ExportProductToExcelQuery, string>, ExportProductToExcelQueryHandler>();
            services.AddTransient<IRequestHandler<GetAllProductPagingQuery, PagedResult<Product>>, GetAllProductPagingQueryHandler>();
            services.AddTransient<IRequestHandler<GetAllProductQuery, IReadOnlyList<Product>>, GetAllProductQueryHandler>();
            services.AddTransient<IRequestHandler<GetHotProductQuery, IReadOnlyList<Product>>, GetHotProductQueryHandler>();
            services.AddTransient<IRequestHandler<GetLatestProductQuery, IReadOnlyList<Product>>, GetLatestProductQueryHandler>();
            services.AddTransient<IRequestHandler<GetProductByIdQuery, Product>, GetProductByIdQueryHandler>();
            services.AddTransient<IRequestHandler<GetPromotionProductQuery, IReadOnlyList<Product>>, GetPromotionProductQueryHandler>();
            services.AddTransient<IRequestHandler<GetRelatedProductQuery, IReadOnlyList<Product>>, GetRelatedProductQueryHandler>();
            // Role query
            services.AddTransient<IRequestHandler<GetAllRolePagingQuery, PagedResult<AppRole>>, GetAllRolePagingQueryHandler>();
            services.AddTransient<IRequestHandler<GetAllRoleQuery, IReadOnlyList<AppRole>>, GetAllRoleQueryHandler>();
            services.AddTransient<IRequestHandler<GetRoleByIdQuery, AppRole>, GetRoleByIdQueryHandler>();

            return services;
        }
    }
}