using Microsoft.AspNetCore.Mvc;
using System.Data;using MySql.Data.MySqlClient;
using lochess.Models;
using lochess.Classes;

namespace lochess.Controllers
{
    public class UsersController : Controller
    {
        // Setting up connection to the context file and appsettings
        // Necessary for each controller
        private IConfiguration configuration;
        public UsersController(IConfiguration _configuration)
        {
            configuration = _configuration;
        }

        // GET: UsersController
        public ActionResult Index()
        {
            List<Users> users = new List<Users>();

            // Query users table for all users
            string tableConnectionString = configuration.GetConnectionString("UsersTable");
            string commandString = $"SELECT * FROM {tableConnectionString}";
            DataRowCollection rows = Utility.SelectMySqlData(tableConnectionString, commandString, configuration);

            foreach (DataRow row in rows)
            {
                users.Add(new Users() 
                { 
                    UserId = int.Parse(row["user_id"].ToString()),
                    FirstName = row["first_name"].ToString(),
                    LastName = row["last_name"].ToString(),
                    Email = row["email"].ToString(),
                    Password = row["password"].ToString()
                });
            }
            return View(users);
        }

        // GET: UsersController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: UsersController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: UsersController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Users user)
        {
            // Insert Users object into users table using parameterization for security against SQL injections
            string tableConnectionString = configuration.GetConnectionString("UsersTable");
            string commandString = $"INSERT INTO {tableConnectionString} (first_name, last_name, email, password) " +
                                   $"VALUES (@firstName, @lastName, @email, @password);";

            List<MySqlParameter> commParameters = new List<MySqlParameter>();
            commParameters.Add(new MySqlParameter("@firstName", user.FirstName));
            commParameters.Add(new MySqlParameter("@lastName", user.LastName));
            commParameters.Add(new MySqlParameter("@email", user.Email));
            commParameters.Add(new MySqlParameter("@password", user.Password));

            bool success = Utility.InsertMySqlData(commandString, commParameters, configuration);
            if (success)
            {
                // TODO: CREATION SUCCESSFUL POPUP ALERT
                return RedirectToAction(nameof(Index));
            }
            else
            {
                // TODO: CREATION UNSUCCESSFUL POPUP ALERT
                return RedirectToAction(nameof(Index));
            }
        }

        // GET: UsersController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: UsersController/Edit/5
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

        // GET: UsersController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: UsersController/Delete/5
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
