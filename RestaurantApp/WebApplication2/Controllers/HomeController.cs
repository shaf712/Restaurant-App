using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication2.Areas.Admin;
using WebApplication2.Areas.Admin.Models;
using WebApplication2.Models;

namespace WebApplication2.Controllers
{
	public class HomeController : Controller
	{
		public struct MenuList
		{
			public List<Food> Breakfast { get; set; }
			public List<Food> Lunch { get; set; }
			public List<Food> Dinner { get; set; }
		}

		[AllowAnonymous]
		public ActionResult Index()
		{
			return View(MenuViewModel.GetHomepageItems());
		}

		[AllowAnonymous]
		public ActionResult Menu()
		{
			MenuList menu = new MenuList();
			List<Food> MenuList = new List<Food>();
			MenuList = MenuViewModel.GetMenu();
			menu.Breakfast = MenuList.Where(i => i.TypeName == "Breakfast").ToList();
			menu.Lunch = MenuList.Where(i => i.TypeName == "Lunch").ToList();
			menu.Dinner = MenuList.Where(i => i.TypeName == "Dinner").ToList();
			return View(menu);
		}

		[Authorize]
		public ActionResult MyMenu(int ID)
		{
			return View(UserViewModel.UserGetRecentPurchased(ID)); 
		}

		public ActionResult About()
		{
			ViewBag.Message = "Your application description page.";

			return View();
		}

		public ActionResult Contact()
		{
			ViewBag.Message = "Your contact page.";

			return View();
		}
	}
}