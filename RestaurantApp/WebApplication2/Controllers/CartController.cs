using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication2.Areas.Admin;
using WebApplication2.Areas.Admin.Models;
using WebApplication2.Models;
using WebApplication2.Objects;

namespace WebApplication2.Controllers
{
    public class CartController : Controller
    {
        // GET: Cart
        public ActionResult Index()
        {
            return View();
        }

		public ActionResult Clear()
		{
			Session["cart"] = null;
			return View("Index");
		}

		private int Exists(int ID)
		{
			List<Food> cart = (List<Food>)Session["cart"];
			for(int i = 0; i < cart.Count; i++)
			{
				if(cart[i].ID == ID)
				{
					return i; 
				}
			}
			return -1; 
		}

		public ActionResult Order(int ID, int Quantity)
		{
			Food selected_item = new Food(); 
			List<Food> Menu = new List<Food>();
			Menu = MenuViewModel.GetMenu(); 
			for(int i = 0; i < Menu.Count; i++)
			{
				if (Menu[i].ID == ID)
					selected_item = Menu[i]; 
			}

			if(Session["cart"] == null)
			{
				List<Food> food_items = new List<Food>();
				selected_item.Quantity = Quantity;
				food_items.Add(selected_item);
				Session["cart"] = food_items; 
			}
			else
			{
				List<Food> food_items = (List<Food>)Session["cart"];
				int index = Exists(ID);
				if (index == -1)
				{
					selected_item.Quantity = Quantity; 
					food_items.Add(selected_item);
				}
				else
				{
					food_items[index].Quantity += Quantity;
				}
				Session["cart"] = food_items;

			}
			return View("Index");
		}


		public ActionResult Checkout(int ID, string isVVIP, string Address)
		{
			double total = 0;
			List<Food> food_items = (List<Food>)Session["cart"];
			for (int i = 0; i < food_items.Count; i++)
			{
				total += (food_items[i].Price * food_items[i].Quantity);
			}
			if(isVVIP == "1")
			{
				total *= .9; 
			}
			List<Login> Users = new List<Login>();
			Users = UserViewModel.GetUsers(); 

			var user = Users.Where(x => x.ID.Equals(ID)).FirstOrDefault();
			var new_balance = Convert.ToDouble(user.Balance) - total;
			UserViewModel.UserSubstractFunds(ID, new_balance);
			for (int i = 0; i < food_items.Count; i++)
			{
				UserViewModel.InsertOrder(ID, food_items[i].ID, Address, food_items[i].Price, food_items[i].Quantity);
			}
			Session["cart"] = null;
			return Json(1);
		}
	}
}