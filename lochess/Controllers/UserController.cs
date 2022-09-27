using Microsoft.AspNetCore.Mvc;
using System.Data;
using MySql.Data.MySqlClient;
using lochess.Models;
using lochess.Classes;
using lochess.Data;
using lochess.Areas.Identity.Data;

namespace lochess.Controllers
{
    public class UserController : Controller
    {
        // Setting up connection to the context file and appsettings
        // Necessary for each controller
        private IConfiguration configuration;
        private LochessIdentityContext context;
        public UserController(IConfiguration _configuration, LochessIdentityContext cc)
        {
            configuration = _configuration;
            context = cc;
        }

        // GET: UserController
        public ActionResult Index()
        {
            List<string> users = context.Users.Select(a => a.UserName).ToList();
            return View(users);
        }

        // GET: UserController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: UserController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: UserController/Create
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Create(User user)
        //{
        //    // Insert User object into users table using parameterization for security against SQL injections
        //    string tableConnectionString = configuration.GetConnectionString("UserTable");
        //    string commandString = $"INSERT INTO {tableConnectionString} (first_name, last_name, email, password) " +
        //                           $"VALUES (@firstName, @lastName, @email, @password);";

        //    List<MySqlParameter> commParameters = new List<MySqlParameter>();
        //    commParameters.Add(new MySqlParameter("@firstName", user.FirstName));
        //    commParameters.Add(new MySqlParameter("@lastName", user.LastName));
        //    commParameters.Add(new MySqlParameter("@email", user.Email));
        //    commParameters.Add(new MySqlParameter("@password", user.Password));

        //    bool success = Utility.InsertMySqlData(commandString, commParameters, configuration);
        //    if (success)
        //    {
        //        // TODO: CREATION SUCCESSFUL POPUP ALERT
        //        return RedirectToAction(nameof(Index));
        //    }
        //    else
        //    {
        //        // TODO: CREATION UNSUCCESSFUL POPUP ALERT
        //        return RedirectToAction(nameof(Index));
        //    }
        //}

        // GET: UserController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: UserController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: UserController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: UserController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
