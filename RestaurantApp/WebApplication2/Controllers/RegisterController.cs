using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication2.Models;

namespace WebApplication2.Controllers
{
    public class RegisterController : Controller
    {
        // GET: Register
        public ActionResult Index()
        {
            return View();
        }

		public ActionResult Validate(string Username)
		{
			var Users = UserViewModel.GetUsers(); 
			for(int i = 0; i < Users.Count; i++)
			{
				if (Users[i].Username == Username)
					return Json(-1);  
			}
			return Json(1); 
		}

		public ActionResult Register(string Username, string Password, double Deposit)
		{
			UserViewModel.RegisterUser(Username, Password, Deposit);
			return Json(1); 
		}
    }
}