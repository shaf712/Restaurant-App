using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Web;
using WebApplication2.Areas.Admin;
using WebApplication2.Objects;

namespace WebApplication2.Extensions
{
	public static class IdentityExtensions
	{
		public static string GetVIPStatus(this IIdentity identity)
		{

			var user = ManagerViewModel.GetUsers().Where(x => x.Username.Equals(identity.Name)).FirstOrDefault();
			if (user != null)
			{
				if (user.isVIP == false)
					return "false";
				else
					return "true";
			}
			return "false"; 
		}

		public static string GetBalance(this IIdentity identity)
		{

			var user = ManagerViewModel.GetUsers().Where(x => x.Username.Equals(identity.Name)).FirstOrDefault();
			if (user != null)
			{
				return user.Balance.ToString();
			}
			return "0"; 
		}

		public static int GetUserID(this IIdentity identity)
		{

			var user = ManagerViewModel.GetUsers().Where(x => x.Username.Equals(identity.Name)).FirstOrDefault();
			if (user == null)
				return 0; 
			return user.ID; 
		}

		public static string GetWarningCount(this IIdentity identity)
		{
			var user = ManagerViewModel.GetUsers().Where(x => x.Username.Equals(identity.Name)).FirstOrDefault();
			if (user != null)
			{
				return user.WarningCount.ToString();
			}
			return "0"; 
		}

		public static string GetVVIPStatus(this IIdentity identity)
		{
			DataTable dt = new DataTable();
			var user = ManagerViewModel.GetUsers().Where(x => x.Username.Equals(identity.Name)).FirstOrDefault();
			if (user != null)
			{
				using (MySqlConnection conn = new MySqlConnection(ConfigurationManager.ConnectionStrings["DB"].ConnectionString))
				{
					using (MySqlCommand cmd = new MySqlCommand("GetVVIPStatus", conn))
					{
						cmd.CommandType = CommandType.StoredProcedure;
						cmd.Parameters.AddWithValue("user_ID", user.ID);
						using (MySqlDataAdapter sda = new MySqlDataAdapter(cmd))
						{
							sda.Fill(dt);
						}
					}

				}

				int flag = int.Parse(dt.Rows[0].ItemArray[0].ToString());
				if (user.WarningCount > 0 || !user.isVIP) 
					return "0";

				else if (flag == 1)
					return "1";
				else
				{
					return "0";
				}
			}
			return "0"; 
		}

	}
}