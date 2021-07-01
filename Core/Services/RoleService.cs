using eShop_Mvc.Core.Entities;
using eShop_Mvc.Core.Interfaces;
using eShop_Mvc.SharedKernel;
using eShop_Mvc.SharedKernel.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace eShop_Mvc.Core.Services
{
    public class RoleService : IRoleService
    {
        private readonly RoleManager<AppRole> _roleManager;
        private readonly IRepository<Announcement, string> _announcementRepository;
        private readonly IRepository<AnnouncementUser, int> _announcementUserRepository;
        private readonly IRepository<Permission, int> _permissionRepository;
        private readonly IRepository<Function, string> _functionRepository;
        private readonly IUnitOfWork _unitOfWork;

        public RoleService(RoleManager<AppRole> roleManager, IRepository<Announcement, string> announcementRepository,
                           IRepository<AnnouncementUser, int> announcementUserRepository,
                           IRepository<Permission, int> permissionRepository,
                           IRepository<Function, string> functionRepository, IUnitOfWork unitOfWork)
        {
            _roleManager = roleManager;
            _announcementRepository = announcementRepository;
            _announcementUserRepository = announcementUserRepository;
            _permissionRepository = permissionRepository;
            _functionRepository = functionRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<bool> AddAsync(Announcement announcement,
                                         List<AnnouncementUser> announcementUsers,
                                         AppRole role)
        {
            var newRole = new AppRole()
            {
                Name = role.Name,
                Description = role.Description
            };
            var result = await _roleManager.CreateAsync(newRole);
            await _announcementRepository.AddAsync(announcement);
            foreach (var announcementUser in announcementUsers)
            {
                await _announcementUserRepository.AddAsync(announcementUser);
            }
            _unitOfWork.Commit();
            return result.Succeeded;
        }

        public async Task DeleteAsync(Guid id)
        {
            var role = await _roleManager.FindByIdAsync(id.ToString());
            await _roleManager.DeleteAsync(role);
        }

        public async Task<IReadOnlyList<AppRole>> GetAllAsync()
        {
            return await _roleManager.Roles.ToListAsync();
        }

        public async Task<PagedResult<AppRole>> GetAllPagingAsync(string keyword, int page, int pageSize)
        {
            var query = _roleManager.Roles;
            if (!string.IsNullOrEmpty(keyword))
            {
                query = query.Where(x => x.Name.Contains(keyword) || x.Description.Contains(keyword));
            }

            int totalRow = query.Count();
            query = query.Skip((page - 1) * pageSize).Take(pageSize);
            var data = await query.ToListAsync();
            return new PagedResult<AppRole>()
            {
                Results = data,
                CurrentPage = page,
                RowCount = totalRow,
                PageSize = pageSize
            };
        }

        public async Task<AppRole> GetByIdAsync(Guid id)
        {
            return await _roleManager.FindByIdAsync(id.ToString());
        }

        public async Task UpdateAsync(AppRole role)
        {
            var tempRole = await _roleManager.FindByIdAsync(role.Id.ToString());
            tempRole.Name = role.Name;
            tempRole.Description = role.Description;
            await _roleManager.UpdateAsync(tempRole);
        }

        public async Task<IReadOnlyList<Permission>> GetListPermissionWithRole(Guid id)
        {
            var functions = _functionRepository.FindAll();
            var permissions = _permissionRepository.FindAll();
            var query = from f in functions
                        join p in permissions on f.Id equals p.FunctionId into fp
                        from p in fp.DefaultIfEmpty()
                        where p != null && p.RoleId == id
                        select new Permission()
                        {
                            RoleId = id,
                            FunctionId = f.Id,
                            CanCreate = p != null && p.CanCreate,
                            CanDelete = p != null && p.CanDelete,
                            CanRead = p != null && p.CanRead,
                            CanUpdate = p != null && p.CanUpdate
                        };
            return await query.ToListAsync();
        }

        public async Task SavePermission(List<Permission> permissions, Guid roleId)
        {
            var oldPermission = await _permissionRepository.FindAll().Where(x => x.RoleId == roleId).ToListAsync();
            if (oldPermission.Count > 0)
            {
                _permissionRepository.DeleteMultipleAsync(oldPermission);
            }

            foreach (var permission in permissions)
            {
                await _permissionRepository.AddAsync(permission);
            }
            _unitOfWork.Commit();
        }

        public Task<bool> CheckPermission(string functionId, string action, string[] roles)
        {
            var functions = _functionRepository.FindAll();
            var permissions = _permissionRepository.FindAll();
            var query = from f in functions
                        join p in permissions on f.Id equals p.FunctionId
                        join r in _roleManager.Roles on p.RoleId equals r.Id
                        where roles.Contains(r.Name) && f.Id == functionId && ((p.CanCreate && action == "Create") ||
                                                                               (p.CanUpdate && action == "Update") ||
                                                                               (p.CanDelete && action == "Delete") ||
                                                                               (p.CanRead && action == "Read"))
                        select p;
            return query.AnyAsync();
        }
    }
}