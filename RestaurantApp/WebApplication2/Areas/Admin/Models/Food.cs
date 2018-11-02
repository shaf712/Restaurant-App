using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication2.Areas.Admin.Models
{
	public class Food
	{
		public int ID { get; set; }
		public int TypeID { get; set; }
		public string TypeName { get; set; }
		public string Name { get; set; }
		public string Description { get; set; }
		public string Image { get; set; }
		public double Price { get; set; }
		public int Quantity { get; set; }
		public int Rating { get; set; }
		public string PreparedBy { get; set; }
        public string DeliveredBy { get; set; }
        public int ChefID { get; set; }
        public int DeliverymanID { get; set; }
		public DateTime OrderedDate { get; set; }
	}
}