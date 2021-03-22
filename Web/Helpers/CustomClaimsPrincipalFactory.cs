﻿using System;
using eShop_Mvc.Core.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using System.Security.Claims;
using System.Threading.Tasks;

namespace eShop_Mvc.Helpers
{
    public class CustomClaimsPrincipalFactory : UserClaimsPrincipalFactory<AppUser, AppRole>
    {
        private readonly UserManager<AppUser> _userManager;

        public CustomClaimsPrincipalFactory(UserManager<AppUser> userManager, RoleManager<AppRole> roleManager, IOptions<IdentityOptions> options)
            : base(userManager, roleManager, options)
        {
            _userManager = userManager;
        }

        public override async Task<ClaimsPrincipal> CreateAsync(AppUser user)
        {
            var principal = await base.CreateAsync(user);
            var roles = await _userManager.GetRolesAsync(user);
            ((ClaimsIdentity)principal.Identity).AddClaims(new[]
            {
                new Claim("Email",user.Email ?? string.Empty),
                new Claim("FullName",user.FullName ?? string.Empty),
                new Claim("Avatar",user.Avatar ?? string.Empty),
                new Claim("Roles",string.Join(";",roles)),
                new Claim(ClaimTypes.NameIdentifier,user.UserName),
                new Claim("UserId",user.Id.ToString())
        });

            return principal;
        }
    }
}