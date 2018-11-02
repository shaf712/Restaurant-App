using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebApplication2.Objects
{
	public class Login
	{
		[Required(ErrorMessage= "Please enter a username.", AllowEmptyStrings = false)]
		public string Username { get; set; }
		[Required(ErrorMessage = "Please enter a password.", AllowEmptyStrings = false)]
		[DataType(System.ComponentModel.DataAnnotations.DataType.Password)]
		public int ID { get; set; }
		public string Password { get; set; }
		public bool RememberMe { get; set; }
		public bool isVIP { get; set; }
		public int VIP { get; set; }
		public  bool isVerified { get; set; }
		public int Verified { get; set; }
		public int WarningCount { get; set; }
		public bool isSuspended { get; set; }
		public string Balance { get; set; }
        public string MoneySpent { get; set; }
        public int DemotionCount { get; set; }
	}
}