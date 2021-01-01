using eShop_Mvc.Core.Entities;
using eShop_Mvc.Core.Enums;
using eShop_Mvc.Core.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace eShop_Mvc.Infrastructure.Data
{
    public class DbInitializer : IDbInitializer
    {
        private readonly IServiceScopeFactory _scopeFactory;

        public DbInitializer(IServiceScopeFactory scopeFactory)
        {
            _scopeFactory = scopeFactory;
        }

        public async Task Initialize()
        {
            using var serviceScope = _scopeFactory.CreateScope();
            await using var context = serviceScope.ServiceProvider.GetService<AppDbContext>();
            await context.Database.MigrateAsync();
        }

        public async Task SeedData()
        {
            using var serviceScope = _scopeFactory.CreateScope();
            using (var roleManager = serviceScope.ServiceProvider.GetService<RoleManager<AppRole>>())
            {
                if (!roleManager.Roles.Any())
                {
                    await roleManager.CreateAsync(new AppRole()
                    {
                        Name = "Admin",
                        NormalizedName = "Admin",
                        Description = "Admin"
                    });
                    await roleManager.CreateAsync(new AppRole()
                    {
                        Name = "Staff",
                        NormalizedName = "Staff",
                        Description = "Staff"
                    });
                    await roleManager.CreateAsync(new AppRole()
                    {
                        Name = "Customer",
                        NormalizedName = "Customer",
                        Description = "Customer"
                    });
                }
            }
            using (var userManager = serviceScope.ServiceProvider.GetService<UserManager<AppUser>>())
            {
                if (!userManager.Users.Any())
                {
                    await userManager.CreateAsync(new AppUser()
                    {
                        UserName = "admin",
                        FullName = "Administrator",
                        Email = "admin@gmail.com",
                        Balance = 0,
                        DateModified = DateTime.Now,
                        DateCreated = DateTime.Now,
                        Status = Status.Active
                    }, "123456");
                    var user = await userManager.FindByNameAsync("admin");
                    await userManager.AddToRoleAsync(user, "Admin");
                }
            }

            using (var context = serviceScope.ServiceProvider.GetService<AppDbContext>())
            {
                if (!context.Functions.Any())
                {
                    await context.Functions.AddRangeAsync(new List<Function>()
                    {
                        new Function() {Id = "SYSTEM", Name = "System",ParentId = null,SortOrder = 1,Status = Status.Active,URL = "/",IconCss = "fa-desktop"  },
                        new Function() {Id = "ROLE", Name = "Role",ParentId = "SYSTEM",SortOrder = 1,Status = Status.Active,URL = "/admin/role/index",IconCss = "fa-home"  },
                        new Function() {Id = "FUNCTION", Name = "Function",ParentId = "SYSTEM",SortOrder = 2,Status = Status.Active,URL = "/admin/function/index",IconCss = "fa-home"  },
                        new Function() {Id = "USER", Name = "User",ParentId = "SYSTEM",SortOrder =3,Status = Status.Active,URL = "/admin/user/index",IconCss = "fa-home"  },
                        new Function() {Id = "ACTIVITY", Name = "Activity",ParentId = "SYSTEM",SortOrder = 4,Status = Status.Active,URL = "/admin/activity/index",IconCss = "fa-home"  },
                        new Function() {Id = "ERROR", Name = "Error",ParentId = "SYSTEM",SortOrder = 5,Status = Status.Active,URL = "/admin/error/index",IconCss = "fa-home"  },
                        new Function() {Id = "SETTING", Name = "Configuration",ParentId = "SYSTEM",SortOrder = 6,Status = Status.Active,URL = "/admin/setting/index",IconCss = "fa-home"  },

                        new Function() {Id = "PRODUCT",Name = "Product Management",ParentId = null,SortOrder = 2,Status = Status.Active,URL = "/",IconCss = "fa-chevron-down"  },
                        new Function() {Id = "PRODUCT_CATEGORY",Name = "Category",ParentId = "PRODUCT",SortOrder =1,Status = Status.Active,URL = "/admin/productcategory/index",IconCss = "fa-chevron-down"  },
                        new Function() {Id = "PRODUCT_LIST",Name = "Product",ParentId = "PRODUCT",SortOrder = 2,Status = Status.Active,URL = "/admin/product/index",IconCss = "fa-chevron-down"  },
                        new Function() {Id = "BILL",Name = "Bill",ParentId = "PRODUCT",SortOrder = 3,Status = Status.Active,URL = "/admin/bill/index",IconCss = "fa-chevron-down"  },

                        new Function() {Id = "CONTENT",Name = "Content",ParentId = null,SortOrder = 3,Status = Status.Active,URL = "/",IconCss = "fa-table"  },
                        new Function() {Id = "BLOG",Name = "Blog",ParentId = "CONTENT",SortOrder = 1,Status = Status.Active,URL = "/admin/blog/index",IconCss = "fa-table"  },
                        new Function() {Id = "PAGE",Name = "Page",ParentId = "CONTENT",SortOrder = 2,Status = Status.Active,URL = "/admin/page/index",IconCss = "fa-table"  },

                        new Function() {Id = "UTILITY",Name = "Utilities",ParentId = null,SortOrder = 4,Status = Status.Active,URL = "/",IconCss = "fa-clone"  },
                        new Function() {Id = "FOOTER",Name = "Footer",ParentId = "UTILITY",SortOrder = 1,Status = Status.Active,URL = "/admin/footer/index",IconCss = "fa-clone"  },
                        new Function() {Id = "FEEDBACK",Name = "Feedback",ParentId = "UTILITY",SortOrder = 2,Status = Status.Active,URL = "/admin/feedback/index",IconCss = "fa-clone"  },
                        new Function() {Id = "ANNOUNCEMENT",Name = "Announcement",ParentId = "UTILITY",SortOrder = 3,Status = Status.Active,URL = "/admin/announcement/index",IconCss = "fa-clone"  },
                        new Function() {Id = "CONTACT",Name = "Contact",ParentId = "UTILITY",SortOrder = 4,Status = Status.Active,URL = "/admin/contact/index",IconCss = "fa-clone"  },
                        new Function() {Id = "SLIDE",Name = "Slide",ParentId = "UTILITY",SortOrder = 5,Status = Status.Active,URL = "/admin/slide/index",IconCss = "fa-clone"  },
                        new Function() {Id = "ADVERTISEMENT",Name = "Advertisement",ParentId = "UTILITY",SortOrder = 6,Status = Status.Active,URL = "/admin/advertisement/index",IconCss = "fa-clone"  },

                        new Function() {Id = "REPORT",Name = "Report",ParentId = null,SortOrder = 5,Status = Status.Active,URL = "/",IconCss = "fa-bar-chart-o"  },
                        new Function() {Id = "REVENUES",Name = "Revenue report",ParentId = "REPORT",SortOrder = 1,Status = Status.Active,URL = "/admin/report/revenues",IconCss = "fa-bar-chart-o"  },
                        new Function() {Id = "ACCESS",Name = "Visitor Report",ParentId = "REPORT",SortOrder = 2,Status = Status.Active,URL = "/admin/report/visitor",IconCss = "fa-bar-chart-o"  },
                        new Function() {Id = "READER",Name = "Reader Report",ParentId = "REPORT",SortOrder = 3,Status = Status.Active,URL = "/admin/report/reader",IconCss = "fa-bar-chart-o"  }
                    });
                }

                if (!context.Footers.Any())
                {
                    await context.Footers.AddAsync(new Footer()
                    {
                        Id = "MainFooterId",
                        Content = "Footer"
                    });
                }

                if (!context.AdvertisementPages.Any())
                {
                    await context.AdvertisementPages.AddRangeAsync(new List<AdvertisementPage>()
                    {
                        new AdvertisementPage()
                        {
                            Id = "Home", Name = "Home", AdvertisementPositions = new List<AdvertisementPosition>()
                            {
                                new AdvertisementPosition() {Id = "Home-left", Name = "HomePageLeft"}
                            }
                        },
                        new AdvertisementPage()
                        {
                            Id = "Product-category", Name = "Product category", AdvertisementPositions =
                                new List<AdvertisementPosition>()
                                {
                                    new AdvertisementPosition()
                                        {Id = "Product-category-left", Name = "ProductCategoryLeft"}
                                }
                        },
                        new AdvertisementPage()
                        {
                            Id = "Product-detail", Name = "Product detail", AdvertisementPositions =
                                new List<AdvertisementPosition>()
                                {
                                    new AdvertisementPosition() {Id = "Product-detail-left", Name = "ProductDetailLeft"}
                                }
                        }
                    });
                }

                if (!context.Slides.Any())
                {
                    await context.Slides.AddRangeAsync(new List<Slide>()
                    {
                        new Slide()
                        {
                            Name = "Slide 1", Image = "/client-side/images/slider/slide-1.jpg", Url = "#",
                            DisplayOrder = 0, GroupAlias = "top", Status = Status.Active
                        },

                        new Slide()
                        {
                            Name = "Slide 2", Image = "/client-side/images/slider/slide-2.jpg", Url = "#",
                            DisplayOrder = 1, GroupAlias = "top", Status = Status.Active
                        },
                        new Slide()
                        {
                            Name = "Slide 3", Image = "/client-side/images/slider/slide-3.jpg", Url = "#",
                            DisplayOrder = 2, GroupAlias = "top", Status = Status.Active
                        },

                        new Slide()
                        {
                            Name = "Brand 1", Image = "/client-side/images/brand1.png", Url = "#", DisplayOrder = 1,
                            GroupAlias = "brand", Status = Status.Active
                        },
                        new Slide()
                        {
                            Name = "Brand 2", Image = "/client-side/images/brand2.png", Url = "#", DisplayOrder = 2,
                            GroupAlias = "brand", Status = Status.Active
                        },
                        new Slide()
                        {
                            Name = "Brand 3", Image = "/client-side/images/brand3.png", Url = "#", DisplayOrder = 3,
                            GroupAlias = "brand", Status = Status.Active
                        },
                        new Slide()
                        {
                            Name = "Brand 4", Image = "/client-side/images/brand4.png", Url = "#", DisplayOrder = 4,
                            GroupAlias = "brand", Status = Status.Active
                        },
                        new Slide()
                        {
                            Name = "Brand 5", Image = "/client-side/images/brand5.png", Url = "#", DisplayOrder = 5,
                            GroupAlias = "brand", Status = Status.Active
                        },
                        new Slide()
                        {
                            Name = "Brand 6", Image = "/client-side/images/brand6.png", Url = "#", DisplayOrder = 6,
                            GroupAlias = "brand", Status = Status.Active
                        },
                        new Slide()
                        {
                            Name = "Brand 7", Image = "/client-side/images/brand7.png", Url = "#", DisplayOrder = 7,
                            GroupAlias = "brand", Status = Status.Active
                        },
                        new Slide()
                        {
                            Name = "Brand 8", Image = "/client-side/images/brand8.png", Url = "#", DisplayOrder = 8,
                            GroupAlias = "brand", Status = Status.Active
                        },
                        new Slide()
                        {
                            Name = "Brand 9", Image = "/client-side/images/brand9.png", Url = "#", DisplayOrder = 9,
                            GroupAlias = "brand", Status = Status.Active
                        },
                        new Slide()
                        {
                            Name = "Brand 10", Image = "/client-side/images/brand10.png", Url = "#", DisplayOrder = 10,
                            GroupAlias = "brand", Status = Status.Active
                        },
                        new Slide()
                        {
                            Name = "Brand 11", Image = "/client-side/images/brand11.png", Url = "#", DisplayOrder = 11,
                            GroupAlias = "brand", Status = Status.Active
                        },
                    });
                }

                if (!context.ProductCategories.Any())
                {
                    await context.AddRangeAsync(new List<ProductCategory>()
                    {
                        new ProductCategory()
                        {
                            Name = "Kamen rider", SeoAlias = "kamen-rider", ParentId = null, Status = Status.Active,
                            SortOrder = 1,
                            Products = new List<Product>()
                            {
                                new Product()
                                {
                                    Name = "DX Zaia Thousandriver", DateCreated = DateTime.Now,
                                    Image = "/client-side/images/products/product-1.jpg", SeoAlias = "thousandriver",
                                    Price = 1500000, Status = Status.Active, OriginalPrice = 1800000
                                },
                                new Product()
                                {
                                    Name = "DX Zaia Slashriser + Burning Falcon Progrise key",
                                    DateCreated = DateTime.Now, Image = "/client-side/images/products/product-2.jpg",
                                    SeoAlias = "driver-slashriser", Price = 1500000, Status = Status.Active,
                                    OriginalPrice = 1950000
                                },
                                new Product()
                                {
                                    Name = "DX A.I.M.S. Shotriser", DateCreated = DateTime.Now,
                                    Image = "/client-side/images/products/product-1.jpg", SeoAlias = "shotriser",
                                    Price = 1400000, Status = Status.Active, OriginalPrice = 18000000
                                },
                                new Product()
                                {
                                    Name = "DX Hiden Zero-One Driver", DateCreated = DateTime.Now,
                                    Image = "/client-side/images/products/product-1.jpg", SeoAlias = "zero-one-driver",
                                    Price = 12000000, Status = Status.Active, OriginalPrice = 1600000
                                },
                                new Product()
                                {
                                    Name = "DX Metsubojinrai Forceriser", DateCreated = DateTime.Now,
                                    Image = "/client-side/images/products/product-1.jpg", SeoAlias = "forceriser",
                                    Price = 1100000, Status = Status.Active, OriginalPrice = 1400000
                                },
                            }
                        },
                        new ProductCategory()
                        {
                            Name = "Super sentai", SeoAlias = "super-sentai", ParentId = null, Status = Status.Active,
                            SortOrder = 2,
                            Products = new List<Product>()
                            {
                                new Product()
                                {
                                    Name = "DX Dino Minder", DateCreated = DateTime.Now,
                                    Image = "/client-side/images/products/product-1.jpg", SeoAlias = "dino-minder",
                                    Price = 700000, Status = Status.Active, OriginalPrice = 1000000
                                },
                                new Product()
                                {
                                    Name = "DX Beast King Sword", DateCreated = DateTime.Now,
                                    Image = "/client-side/images/products/product-2.jpg",
                                    SeoAlias = "driver-slashriser", Price = 400000, Status = Status.Active,
                                    OriginalPrice = 600000
                                },
                                new Product()
                                {
                                    Name = "DX Gourai Changer", DateCreated = DateTime.Now,
                                    Image = "/client-side/images/products/product-1.jpg", SeoAlias = "shotriser",
                                    Price = 1400000, Status = Status.Active, OriginalPrice = 18000000
                                },
                                new Product()
                                {
                                    Name = "DX SHURIKEN BALL", DateCreated = DateTime.Now,
                                    Image = "/client-side/images/products/product-1.jpg", SeoAlias = "zero-one-driver",
                                    Price = 12000000, Status = Status.Active, OriginalPrice = 1600000
                                },
                                new Product()
                                {
                                    Name = "DX Metsubojinrai Forceriser", DateCreated = DateTime.Now,
                                    Image = "/client-side/images/products/product-1.jpg", SeoAlias = "forceriser",
                                    Price = 1100000, Status = Status.Active, OriginalPrice = 1400000
                                },
                            }
                        }
                    });
                }

                await context.SaveChangesAsync();
            }
        }
    }
}