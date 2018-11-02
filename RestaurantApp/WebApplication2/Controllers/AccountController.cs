using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using WebApplication2.Areas.Admin;
using WebApplication2.Objects;

namespace WebApplication2.Controllers
{
    public class AccountController : Controller
    {
        // GET: MyAccount
        public ActionResult Login()
        {
            return View();
        }


		[HttpPost]
		public ActionResult Login(Login l)
		{
			DataSet ds;
			List<Login> Users = new List<Login>(); 
			using (MySqlConnection conn = new MySqlConnection(ConfigurationManager.ConnectionStrings["DB"].ConnectionString))
			{
				ds = MySqlHelper.ExecuteDataset(conn, "GetUsers");
			}
			foreach (DataRow row in ds.Tables[0].Rows)
			{
				Login u = new Login();
				u.Username = row["username"].ToString();
				u.ID = (int)row["ID"];
				u.Password = row["password"].ToString();
				u.isVerified = (bool)row["is_verified"]; 
				u.isVIP = (bool)row["is_VIP"];
				u.isSuspended = Convert.ToBoolean(row["is_suspended"]); 
				Users.Add(u); 
			}

			var Chefs = ManagerViewModel.GetChefs(); 

			var user = Users.Where(x => x.Username.Equals(l.Username) && x.Password.Equals(l.Password)).FirstOrDefault(); 
			if(user == null)
			{
				var chef = Chefs.Where(x => x.Username.Equals(l.Username) && x.Password.Equals(l.Password)).FirstOrDefault();
				if(chef != null)
				{
					FormsAuthentication.SetAuthCookie(chef.Username, true);
					return RedirectToAction("Menu", "Admin/Chef");
				}
				else if(l.Username == "Manager" && l.Password == "theboss")
				{
					FormsAuthentication.SetAuthCookie(l.Username, true);
					return RedirectToAction("Users", "Admin/Manager");
				}
			}

			if(user!= null && user.isSuspended)
			{
				ModelState.AddModelError("Password", "Your account has been suspended and your balance has been emptied.");
				return View();
			}

			if(user != null && user.isVerified)
			{
				FormsAuthentication.SetAuthCookie(user.Username, true);
				return RedirectToAction("MyMenu", "Home", new { ID = user.ID }); 
			}

			if(user != null && !user.isVerified)
			{
				ModelState.AddModelError("Password", "A manager has not verified your account yet. Please wait 3-5 business days.");
				return View();
			}

			ModelState.Remove("Password");
			ModelState.AddModelError("Password", "The username or password is incorrect."); 
			return View(); 
		}
		[Authorize]
		public ActionResult Logout()
		{
			FormsAuthentication.SignOut();
			Session["cart"] = null;
			return RedirectToAction("Index", "Home"); 
		}
    }
}