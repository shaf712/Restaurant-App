using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebApplication2.Areas.Admin.Controllers
{
    public class ChefController : Controller
    {
        // GET: Admin/Chef
        public ActionResult Menu()
        {
            return View(MenuViewModel.GetMenu());
        }

		[HttpPost]
		public JsonResult Create(int TypeID, string TypeName, int ChefID, string Name, string Description, double Price, string Image)
		{
			var raw_image = Image.Split(',');
			var imageBase64 = raw_image.Length <= 1 ? "" : raw_image[1];
			imageBase64 = imageBase64.Replace("\"", "");
			byte[] imagebytes = Convert.FromBase64String(imageBase64);

			return Json(MenuViewModel.Create(TypeID, ChefID, Name, Description, Price, imagebytes)); 
		}

		[HttpPost]
		public JsonResult Update(int ID, int TypeID, int ChefID, string Name, string Description, double Price, string Image)
		{
			var raw_image = Image.Split(',');
			var imageBase64 = raw_image.Length <= 1 ? "" : raw_image[1];
			imageBase64 = imageBase64.Replace("\"", "");
			byte[] imagebytes = Convert.FromBase64String(imageBase64);
			MenuViewModel.Update(ID, TypeID, ChefID, Name, Description, Price, imagebytes); 
			return Json("Saved"); 
		}

		[HttpGet]
		public JsonResult GetChefs()
		{
			return Json(MenuViewModel.GetChefs(), JsonRequestBehavior.AllowGet); 
		}

		[HttpPost]
		public JsonResult Delete(int ID)
		{
			MenuViewModel.Delete(ID); 
			return Json("Deleted"); 
		}
	}
}