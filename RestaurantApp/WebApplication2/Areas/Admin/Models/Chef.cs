using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication2.Areas.Admin.Models
{
	public class Chef
	{
		public int ID { get; set; }
		public string FullName { get; set; }
		public int DemotionCount { get; set; }
		public int Salary { get; set; }
		public string Username { get; set; }
		public string Password { get; set; }
		public int Approval { get; set; }
		public int PromotionCount { get; set; }
		public string LastOrder { get; set; }
	}
}