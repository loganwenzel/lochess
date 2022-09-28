using lochess.Areas.Identity.Data;
using lochess.Hubs;
using lochess.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace lochess.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private LochessIdentityContext context;
        UserManager<AspNetUser> UserManager;
        public HomeController(ILogger<HomeController> logger, LochessIdentityContext cc, UserManager<AspNetUser> userManager)
        {
            _logger = logger;
            context = cc;
            UserManager = userManager;
        }

        public IActionResult Index()
        {
            List<string> userNames = context.AspNetUsers.Where(a => a.UserName != UserManager.GetUserName(User)).Select(a => a.UserName).ToList();
            ViewBag.UserNames = userNames;
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