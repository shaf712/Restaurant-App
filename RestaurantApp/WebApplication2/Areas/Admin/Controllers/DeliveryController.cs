using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebApplication2.Areas.Admin.Controllers
{
    public class DeliveryController : Controller
    {
        // GET: Admin/Delivery
        public ActionResult Routes()
        {
            return View(OrderViewModel.GetOrders());
        }

        public ActionResult Orders()
        {
            return View(OrderViewModel.GetOrders());
        }

        [HttpGet]
        public JsonResult GetOrders()
        {
            return Json(OrderViewModel.GetOrders(), JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult DeleteOrder(int ID)
        {
            OrderViewModel.DeleteOrder(ID);
            return Json("Deleted");
        }

    }
}