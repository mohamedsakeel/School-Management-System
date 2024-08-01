using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using School_Management_System.Models;
using SchoolManagementSystem.Domain.Models;
using System.Diagnostics;
using System.Security.Claims;

namespace School_Management_System.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly UserManager<ApplicationUser> _userManager;

        public HomeController(ILogger<HomeController> logger, UserManager<ApplicationUser> userManager)
        {
            _logger = logger;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (User.Identity?.IsAuthenticated ?? false)
            {
                var user = await _userManager.FindByIdAsync(userId);
                if (user != null)
                {
                    var firstName = user.FirstName;
                    var lastName = user.LastName;
                    ViewBag.Message = $"Welcome, {firstName} {lastName}!";
                }
            }
            else
            {
                ViewBag.Message = "Welcome, Guest!";
            }

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
