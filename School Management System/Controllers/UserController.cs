using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using School_Management_System.BackgroundTasks;
using School_Management_System.Models;
using SchoolManagementSystem.AppCore.Interfaces;
using SchoolManagementSystem.AppCore.Repositories;
using SchoolManagementSystem.Domain.Models;
using SchoolManagementSystem.Notification;
using System.Text;

namespace School_Management_System.Controllers
{
    public class UserController : Controller
    {
        private readonly IUserRepository _userRepo;
        private readonly IEmailSender _sender;
        private readonly IConfiguration _configuration;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IBackgroundTasks _backgroundTasks;

        public UserController(IBackgroundTasks bgtasks, IUserRepository userRepository, UserManager<ApplicationUser> userManager, IEmailSender emailSender, IConfiguration configuration, RoleManager<IdentityRole> roleManager)
        {
            _userRepo = userRepository;
            _sender = emailSender;
            _configuration = configuration;
            _userManager = userManager;
            _roleManager = roleManager;
            _backgroundTasks = bgtasks;
        }

        public async Task<IActionResult> Index()
        {
            try
            {
                // Get all users
                var users = await _userRepo.GetUsersAsync();
                var roles = await _roleManager.Roles.ToListAsync();

                var userViewModels = users.Select(user => new UserViewModel
                {
                    Id = user.Id,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Email = user.Email,
                    PhoneNumber = user.PhoneNumber,
                    SelectedRole = roles.FirstOrDefault(r => _userManager.IsInRoleAsync(user, r.Name).Result)?.Name,
                    RoleId = roles.FirstOrDefault(r => _userManager.IsInRoleAsync(user, r.Name).Result).Id,
                    Roles = roles.Select(r => new SelectListItem { Value = r.Id, Text = r.Name }).ToList()
                }).ToList();

                return View(userViewModels);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> SaveUser(UserViewModel model)
        {
            if (string.IsNullOrEmpty(model.Id))
            {
                var defaultPassword = GenerateRandomPassword();
                // Add new user
                var user = new ApplicationUser
                {
                    UserName = model.Email,
                    Email = model.Email,
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    PhoneNumber = model.PhoneNumber
                };

                // Assuming Password is passed for new user creation
                var result = await _userRepo.AddUserAsync(user, defaultPassword);
                if (result.Succeeded)
                {
                    var userEmailMessage = $"Your account has been created. Your default password is: {defaultPassword}";
                    await _backgroundTasks.SendEmailAsync(model.Email, "Your Account Details", userEmailMessage);

                    // Send notification to admin
                    var adminEmailMessage = $"A new user has been created:\n\nName: {model.FirstName} {model.LastName}\nEmail: {model.Email}\nDefault Password: {defaultPassword}";
                    await _backgroundTasks.SendEmailAsync(_configuration["EmailSettings:AdminEmail"], "New User Created", adminEmailMessage);

                    //Assign Role
                    if (model.SelectedRole != null)
                    {
                        var role = await _roleManager.FindByIdAsync(model.SelectedRole);
                        if (role != null)
                        {
                            await _userRepo.AddToRoleAsync(user, role.NormalizedName);
                        }
                    }

                    return RedirectToAction("Index", "User");
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }
            else
            {
                // Update existing user
                var user = await _userRepo.FindByIdAsync(model.Id);
                if (user != null)
                {
                    user.FirstName = model.FirstName;
                    user.LastName = model.LastName;
                    user.Email = model.Email;
                    user.UserName = model.Email;
                    user.PhoneNumber = model.PhoneNumber;

                    var result = await _userRepo.UpdateAsync(user);
                    if (result.Succeeded)
                    {
                        var existingRole = await _userManager.GetRolesAsync(user);
                        if (existingRole.Any())
                        {
                            await _userRepo.RemoveUserFromRoleAsync(user, existingRole.FirstOrDefault());
                        }

                        if (model.SelectedRole != null)
                        {

                            var role = await _roleManager.FindByIdAsync(model.SelectedRole);
                            if (role != null)
                            {
                                await _userRepo.AddToRoleAsync(user, role.NormalizedName);
                            }
                        }


                        return RedirectToAction("Index", "User");
                    }

                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                }
            }

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> DeleteUser(string id)
        {
            var result = new { success = false, error = string.Empty };

            try
            {
                var deleteResult = await _userRepo.DeleteUserAsync(id);
                if (deleteResult.Succeeded)
                {
                    result = new { success = true, error = string.Empty };
                }
                else
                {
                    result = new { success = false, error = deleteResult.Errors.Select(e => e.Description).FirstOrDefault() };
                }
            }
            catch (Exception ex)
            {
                result = new { success = false, error = ex.Message };
            }

            return Json(result);
        }

        private string GenerateRandomPassword(int length = 12)
        {
            // Ensure minimum length of 12
            if (length < 12)
            {
                length = 12;
            }

            const string lowercase = "abcdefghijklmnopqrstuvwxyz";
            const string uppercase = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            const string digits = "1234567890";
            const string special = "!@#$%^&*()_+";

            var random = new Random();
            var password = new StringBuilder();

            // Ensure at least one character from each category
            password.Append(lowercase[random.Next(lowercase.Length)]);
            password.Append(uppercase[random.Next(uppercase.Length)]);
            password.Append(digits[random.Next(digits.Length)]);
            password.Append(special[random.Next(special.Length)]);

            // Fill the rest of the password length with random characters from all categories
            var allCharacters = lowercase + uppercase + digits + special;
            for (int i = 4; i < length; i++)
            {
                password.Append(allCharacters[random.Next(allCharacters.Length)]);
            }

            // Shuffle the password to ensure randomness
            var shuffledPassword = password.ToString().OrderBy(c => random.Next()).ToArray();

            return new string(shuffledPassword);
        }
    }
}
