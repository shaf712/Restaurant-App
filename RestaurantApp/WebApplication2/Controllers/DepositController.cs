using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication2.Models;

namespace WebApplication2.Controllers
{
    public class DepositController : Controller
    {
        // GET: Deposit
        public ActionResult Index()
        {
            return View();
        }

		public ActionResult Deposit(int ID, double Deposit)
		{
			UserViewModel.Deposit(ID, Deposit);
			return Json(1); 
		}
    }
}