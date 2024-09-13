using Microsoft.AspNetCore.Identity;
using SchoolManagementSystem.AppCore.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolManagementSystem.AppCore.Repositories
{
    public class RoleRepository : IRoleRepository
    {
        private readonly RoleManager<IdentityRole> _roleManager;

        public RoleRepository(RoleManager<IdentityRole> roleManager)
        {
            _roleManager = roleManager;
        }

        public async Task<List<IdentityRole>> GetAllRolesAsync()
        {
            return _roleManager.Roles.ToList();
        }

        public async Task<IdentityRole> GetRoleByIdAsync(string roleId)
        {
            return await _roleManager.FindByIdAsync(roleId);
        }

        public async Task CreateRoleAsync(IdentityRole role)
        {
            await _roleManager.CreateAsync(role);
        }

        public async Task UpdateRoleAsync(IdentityRole role)
        {
            await _roleManager.UpdateAsync(role);
        }

        public async Task DeleteRoleAsync(string roleId)
        {
            var role = await GetRoleByIdAsync(roleId);
            if (role != null)
            {
                await _roleManager.DeleteAsync(role);
            }
        }
    }
}
