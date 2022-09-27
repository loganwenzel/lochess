using lochess.Areas.Identity.Data;
using lochess.Hubs;
using lochess.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace lochess.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private LochessIdentityContext context;
        public HomeController(ILogger<HomeController> logger, LochessIdentityContext cc)
        {
            _logger = logger;
            context = cc;
        }

        public IActionResult Index()
        {
            List<string> userNames = context.Users.Select(a => a.UserName).ToList();
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