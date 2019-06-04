using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MangeCharacters.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Oracle.ManagedDataAccess.Client;

namespace MangeCharacters.Controllers
{
    public class LoginController : Controller
    {
        private readonly IConfiguration configuration;
        public LoginController(IConfiguration configuration)
        {
            this.configuration = configuration;
        }
        public IActionResult Index(bool invalid)
        {
            if (invalid)
                ViewData["Message"] = "Invalid Username";
            return View();
        }
        public IActionResult Login(LoginViewModel loginViewModel)
        {
            int userID = -1;
            bool isAdmin = false;
            using (OracleConnection connection = new OracleConnection(configuration["OracleConnectionString"]))
            {
                using (OracleCommand command = connection.CreateCommand())
                {
                    connection.Open();
                    command.CommandText = $"SELECT UserID, ISADMIN From FANTASYRPG.\"User\" Where UserName = '{loginViewModel.UserName}'";
                    OracleDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        isAdmin = reader.GetBoolean(1);
                        userID = reader.GetInt32(0);
                    };
                    reader.Dispose();
                    connection.Close();
                }
            }
            if(userID >= 0)
            {
                HttpContext.Session.SetInt32("UserID", userID);
                HttpContext.Session.SetString("UserName", loginViewModel.UserName);
                HttpContext.Session.SetString("Role", isAdmin ? "admin" : "user");
                return RedirectToAction("Index", "Home");
            }
            

            return RedirectToAction("Index", new { invalid = true });
        }
    }
}