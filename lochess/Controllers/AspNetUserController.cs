using Microsoft.AspNetCore.Mvc;
using System.Data;
using MySql.Data.MySqlClient;
using lochess.Models;
using lochess.Classes;
using lochess.Data;
using lochess.Areas.Identity.Data;

namespace lochess.Controllers
{
    public class AspNetUserController : Controller
    {
        // Setting up connection to the context file and appsettings
        // Necessary for each controller
        private IConfiguration configuration;
        private LochessIdentityContext context;
        public AspNetUserController(IConfiguration _configuration, LochessIdentityContext cc)
        {
            configuration = _configuration;
            context = cc;
        }

        // GET: UserController
        public ActionResult Index()
        {
            //List<string> users = context.Users.Select(a => a.UserName).ToList();
            //return View(users);
            return View();
        }
    }
}
