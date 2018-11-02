using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using WebApplication2.Areas.Admin;
using WebApplication2.Areas.Admin.Models;

namespace WebApplication2.Models
{
	public class ItemViewModel
	{
		public static Food GetFoodItem(int ID)
		{
			Food item = new Food(); 
			var menu = MenuViewModel.GetMenu();
			for(int i = 0; i < menu.Count; i++)
			{
				if (menu[i].ID == ID)
					return menu[i]; 
			}
			return item; 
		}
	}
}