using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication2.Areas.Admin.Models
{
	public class UserReview
	{
		public int ID { get; set; }
		public int UserID { get; set; }
		public string Name { get; set; }
		public string PreparedBy { get; set; }
		public int Rating { get; set; }
		public int Approval { get; set; }
		public string Comment { get; set; }
	}
}