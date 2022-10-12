using lochess.Areas.Identity.Data;
using lochess.Hubs;
using lochess.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace lochess.Controllers
{
    public class GameController : Controller
    {
        private LochessIdentityContext context;
        UserManager<AspNetUser> UserManager;
        public GameController(LochessIdentityContext cc, UserManager<AspNetUser> userManager)
        {
            context = cc;
            UserManager = userManager;
        }

        [HttpPost]
        // Use optional input set to a default value
        public IActionResult Index(string opponentUserName)
        {
            // Pass in opponentUserName so page AddToGroup method can be called on page load
            ViewBag.opponentUserName = opponentUserName;
            return View();
        }

        public IActionResult Index()
        {
            // Set createGroup to false when this method is posted to from the sender
            ViewBag.groupName = context.Users.Where(a => a.UserName == UserManager.GetUserName(User)).FirstOrDefault().GroupName;
            return View();
        }
    }
}
