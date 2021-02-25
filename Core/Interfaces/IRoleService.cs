using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using eShop_Mvc.Core.Entities;
using eShop_Mvc.SharedKernel;

namespace eShop_Mvc.Core.Interfaces
{
    public interface IRoleService
    {
        Task<bool> AddAsync(Announcement announcement, List<AnnouncementUser> announcementUsers, AppRole role);

        Task DeleteAsync(Guid id);

        Task<IReadOnlyList<AppRole>> GetAllAsync();

        Task<PagedResult<AppRole>> GetAllPagingAsync(string keyword, int page, int pageSize);

        Task<AppRole> GetByIdAsync(Guid id);

        Task UpdateAsync(AppRole role);

        Task<IReadOnlyList<Permission>> GetListPermissionWithRole(Guid id);

        Task SavePermission(List<Permission> permissions, Guid roleId);

        Task<bool> CheckPermission(string functionId, string action, string[] roles);
    }
}