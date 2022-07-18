using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Configuration;
using MySql.Data.MySqlClient;
using lochess.Models;

namespace lochess.Controllers
{
    public class UsersController : Controller
    {
        // Setting up connection to the context file and appsettings
        // Necessary for each controller
        private IConfiguration Configuration;
        public UsersController(IConfiguration _configuration)
        {
            Configuration = _configuration;
        }
        // GET: UsersController
        public ActionResult Index()
        {
            List<Users> users = new List<Users>();
            string connString = this.Configuration.GetConnectionString("lochess");
            using (MySqlConnection con = new MySqlConnection(connString))
            {
                string query = "SELECT first_name, last_name, email, password FROM users";
                using (MySqlCommand cmd = new MySqlCommand(query))
                {
                    cmd.Connection = con;
                    con.Open();
                    using (MySqlDataReader sdr = cmd.ExecuteReader())
                    {
                        while (sdr.Read())
                        {
                            users.Add(new Users
                            {
                                FirstName = sdr["first_name"].ToString(),
                                LastName = sdr["last_name"].ToString(),
                                Email = sdr["email"].ToString(),
                                Password = sdr["password"].ToString()
                            });
                        }
                    }
                    con.Close();
                }
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

        public void PostCreate(Users user)
        {
            string connString = this.Configuration.GetConnectionString("lochess");
            using (MySqlConnection con = new MySqlConnection(connString))
            {
                string query = "INSERT INTO users (first_name, last_name, email, password)" +
                              $"VALUES ({user.FirstName}, {user.LastName}, {user.Email}, {user.Password})";
                //Need to use parameterization for SQL insert from c#
                //MySqlCommand cmd = new MySqlCommand("Insert into student (Name,Address,Mobile,Email )  
                //                                        values(@Name, @Address, @Mobile, @Email)", conn);  

                //cmd.Parameters.AddWithValue("@Name", txtName.Text);
                //                cmd.Parameters.AddWithValue("@Address", txtAddress.Text);
                //                cmd.Parameters.AddWithValue("@Mobile", txtMobile.Text);
                //                cmd.Parameters.AddWithValue("@Email", txtEmail.Text);
                using (MySqlCommand cmd = new MySqlCommand(query))
                {
                    cmd.Connection = con;
                    con.Open();
                    using (MySqlDataReader sdr = cmd.ExecuteReader())
                    {
                        while (sdr.Read())
                        {
                            users.Add(new Users
                            {
                                FirstName = sdr["first_name"].ToString(),
                                LastName = sdr["last_name"].ToString(),
                                Email = sdr["email"].ToString(),
                                Password = sdr["password"].ToString()
                            });
                        }
                    }
                    con.Close();
                }
            }
            // TODO: Save to DB
        }

        // POST: UsersController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
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
