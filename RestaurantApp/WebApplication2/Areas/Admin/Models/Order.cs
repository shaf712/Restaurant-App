using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication2.Areas.Admin.Models
{
    public class Order
    {
        public int ID { get; set; }
        public string Username { get; set; }
        public int  UserID { get; set; }
        public string Item { get; set; }
        public string ChefName { get; set; }
        public string Address { get; set; }
        public double Cost { get; set; }
        public double Quantity { get; set; }
        public string Timestamp { get; set; }
    }
}