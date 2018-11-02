using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebApplication2.Areas.Admin.Controllers
{
    public class ManagerController : Controller
    {
        // GET: Admin/Manager
        public ActionResult Index()
        {
            return View();
        }

		[Authorize]

		public ActionResult Users()
		{

			return View(ManagerViewModel.GetUsers()); 
		}

        [Authorize]

        public ActionResult Deliverymen()
        {

            return View(ManagerViewModel.GetDeliverymen());
        }

        public ActionResult AddDeliveryman(string FirstName, string LastName, int Salary)
        {
            return Json(ManagerViewModel.AddDeliveryman(FirstName, LastName, Salary));
        }

        public ActionResult DemoteDeliveryman(int ID)
        {
            return Json(ManagerViewModel.DemoteDeliveryman(ID));
        }

        public ActionResult PromoteDeliveryman(int ID)
        {
            return Json(ManagerViewModel.PromoteDeliveryman(ID));
        }

        public ActionResult FireDeliveryman(int ID)
        {
            ManagerViewModel.FireDeliveryman(ID);
            return Json("deleted");

        }

        [Authorize]
		public ActionResult Chefs()
		{
			return View(ManagerViewModel.GetChefs()); 
		}

		public ActionResult Promote(int ChefID)
		{
			return Json(ManagerViewModel.PromoteChef(ChefID)); 
		}

		public ActionResult Demote(int ChefID)
		{
			return Json(ManagerViewModel.DemoteChef(ChefID));
		}
		public ActionResult AddChef(string FirstName, string LastName, int Salary)
		{
			return Json(ManagerViewModel.AddChef(FirstName, LastName, Salary)); 
		}

		public ActionResult FireChef(int ChefID)
		{
			ManagerViewModel.FireChef(ChefID);
			return Json("deleted"); 
		}
		public ActionResult PromoteUser(int UserID)
		{
			ManagerViewModel.PromoteUser(UserID);
			return Json("promoted"); 
		}

		public ActionResult SuspendUser(int UserID)
		{
			ManagerViewModel.SuspendUser(UserID);
			return Json("deleted"); 
		}

		public ActionResult GetUserDetails(int UserID)
		{
			return Json(ManagerViewModel.GetUserDetails(UserID), JsonRequestBehavior.AllowGet); 
		}

		public ActionResult IssueWarning(int UserID)
		{
			ManagerViewModel.IssueWarning(UserID);
			return Json("saved");
		}
		public ActionResult Verify(int UserID)
		{
			ManagerViewModel.Verify(UserID);
			return Json("saved");
		}
	}
}