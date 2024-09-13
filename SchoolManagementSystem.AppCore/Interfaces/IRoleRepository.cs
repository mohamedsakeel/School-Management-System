using Microsoft.AspNetCore.Identity;

namespace SchoolManagementSystem.AppCore.Interfaces
{
    public interface IRoleRepository
    {
        Task<List<IdentityRole>> GetAllRolesAsync();
        Task<IdentityRole> GetRoleByIdAsync(string roleId);
        Task CreateRoleAsync(IdentityRole role);
        Task UpdateRoleAsync(IdentityRole role);
        Task DeleteRoleAsync(string roleId);
    }
}