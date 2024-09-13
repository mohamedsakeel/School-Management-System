using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using School_Management_System.Models;
using SchoolManagementSystem.AppCore.Interfaces;

namespace School_Management_System.Controllers
{
    public class RoleController : Controller
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IRoleRepository _roleRepo;

        public RoleController(RoleManager<IdentityRole> roleManager, IRoleRepository roleRepo)
        {
            _roleManager = roleManager;
            _roleRepo = roleRepo;
        }
        public async Task<IActionResult> Index()
        {
            var roles = _roleManager.Roles.ToList();
            var model = roles.Select(r => new RoleViewModel
            {
                RoleId = r.Id,
                RoleName = r.Name
            });

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> AddOrEditRole(RoleViewModel model)
        {
                if (string.IsNullOrEmpty(model.RoleId))
                {
                    // Add new role
                    var role = new IdentityRole { Name = model.RoleName };
                    await _roleManager.CreateAsync(role);
                }
                else
                {
                    // Edit existing role
                    var role = await _roleManager.FindByIdAsync(model.RoleId);
                    if (role != null)
                    {
                        role.Name = model.RoleName;
                        await _roleManager.UpdateAsync(role);
                    }
                }
                return RedirectToAction("Index");
            
            return View("Index", model);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteRole(string roleId)
        {
            var role = await _roleManager.FindByIdAsync(roleId);
            if (role != null)
            {
                await _roleManager.DeleteAsync(role);
            }
            return RedirectToAction("Index");
        }
    }
}
