using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication2.Models;

namespace WebApplication2.Controllers
{
    public class ItemController : Controller
    {
        // GET: Item
        public ActionResult Index(int id)
        {
			object food_item = ItemViewModel.GetFoodItem(id); 
            return View(food_item);
        }

		[HttpPost]
		public ActionResult SubmitReview(int UserID, int isVVIP, int ItemID, int ChefID, int DeliverymanID, int Rating, int DeliveryRating, int Approval, string Comment)
		{
			UserViewModel.UserSubmitReview(UserID, isVVIP, ItemID, ChefID, DeliverymanID, Rating, DeliveryRating, Approval, Comment); 
			return Json("Saved");
		}
    }
}