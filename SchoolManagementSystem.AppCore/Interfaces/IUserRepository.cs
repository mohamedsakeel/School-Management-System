using Microsoft.AspNetCore.Identity;
using SchoolManagementSystem.Domain.Models;

namespace SchoolManagementSystem.AppCore.Interfaces
{
    public interface IUserRepository
    {
        Task<IEnumerable<ApplicationUser>> GetUsersAsync();
        Task<ApplicationUser> FindByIdAsync(string userId);
        Task<IdentityResult> UpdateAsync(ApplicationUser user);
        Task<IdentityResult> AddUserAsync(ApplicationUser user, string password);
        Task<IdentityResult> DeleteUserAsync(string userId);
        Task<IdentityResult> AddToRoleAsync(ApplicationUser user, string roleName);
        Task<IdentityResult> RemoveUserFromRoleAsync(ApplicationUser user, string role);
    }
}