using AutoMapper;
using eShop_Mvc.Authorization;
using eShop_Mvc.Core.Entities;
using eShop_Mvc.Extensions;
using eShop_Mvc.Helpers;
using eShop_Mvc.Helpers.AutoMapper;
using eShop_Mvc.Infrastructure.Data;
using eShop_Mvc.SharedKernel.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using PaulMiami.AspNetCore.Mvc.Recaptcha;
using System;

namespace eShop_Mvc
{
    public class Startup
    {
        private readonly IConfiguration _configuration;

        public Startup(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // Auto Mapper
            services.AddAutoMapper(typeof(MappingProfiles));
            services.AddDbContext<AppDbContext>(options =>
            {
                options.UseSqlServer(
                    _configuration.GetConnectionString("DefaultConnection"),
                    ob => ob.MigrationsAssembly("eShop_Mvc.Infrastructure"));
                options.EnableSensitiveDataLogging();
            });

            services.AddIdentityServices();

            // External login
            services.AddAuthentication()
                .AddGoogle(options =>
                    {
                        IConfigurationSection googleAuthConfigurationSection =
                            _configuration.GetSection("Authentication:Google");
                        options.ClientId = googleAuthConfigurationSection["ClientId"];
                        options.ClientSecret = googleAuthConfigurationSection["ClientSecret"];
                        options.SignInScheme = IdentityConstants.ExternalScheme;
                    }
                )
                .AddFacebook(options =>
                {
                    IConfigurationSection facebookAuthConfigurationSection =
                        _configuration.GetSection("Authentication:Facebook");
                    options.AppId = facebookAuthConfigurationSection["AppId"];
                    options.AppSecret = facebookAuthConfigurationSection["AppSecret"];
                });

            // Seed data
            services.AddScoped<IDbInitializer, DbInitializer>();

            services.AddScoped<IUserClaimsPrincipalFactory<AppUser>, CustomClaimsPrincipalFactory>();
            services.AddMvc(o => o.EnableEndpointRouting = false).AddNewtonsoftJson(o =>
            {
                o.SerializerSettings.ContractResolver = new DefaultContractResolver();
                o.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
            }).AddSessionStateTempDataProvider();

            // recaptcha
            services.AddRecaptcha(new RecaptchaOptions()
            {
                SiteKey = _configuration["Recaptcha:SiteKey"],
                SecretKey = _configuration["Recaptcha:SecretKey"]
            });

            // Session
            services.AddDistributedMemoryCache();
            services.AddSession(option =>
            {
                option.IdleTimeout = TimeSpan.FromDays(7);
                option.Cookie.HttpOnly = true;
                option.Cookie.IsEssential = true;
            });

            services.AddApplicationServices();

            // Authorization
            services.AddTransient<IAuthorizationHandler, ResourceBasedAuthorizationHandler>();
            services.AddControllersWithViews().AddRazorRuntimeCompilation();
            services.AddRazorPages();
            services.AddApplicationInsightsTelemetry();

            services.Configure<SecurityStampValidatorOptions>(options =>
            {
                // Trên 10 giây truy cập lại sẽ nạp lại thông tin User (Role)
                // SecurityStamp trong bảng User đổi -> nạp lại thông tin Security
                options.ValidationInterval = TimeSpan.FromSeconds(5);
            });

            // Image resizer
            services.AddImageResizer();

            // Caching
            services.AddMemoryCache();

            // Min response
            services.AddMinResponse();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseImageResizer();
            app.UseStaticFiles();
            app.UseMinResponse();
            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();
            app.UseSession();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapAreaControllerRoute(
                    name: "Admin",
                    areaName: "Admin",
                    pattern: "Admin/{controller=Login}/{action=Index}/{id?}");
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");

                endpoints.MapRazorPages();
            });
            using var scope = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope();
            var dbInitializer = scope.ServiceProvider.GetService<IDbInitializer>();
            dbInitializer.Initialize().Wait();
            dbInitializer.SeedData().Wait();
        }
    }
}