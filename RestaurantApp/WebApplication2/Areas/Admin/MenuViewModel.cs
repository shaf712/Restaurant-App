using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using WebApplication2.Areas.Admin.Models;

namespace WebApplication2.Areas.Admin
{
	public class MenuViewModel
	{
		public static List<Food> GetHomepageItems()
		{
			DataSet ds;
			var menu = new List<Food>();
			using (MySqlConnection conn = new MySqlConnection(ConfigurationManager.ConnectionStrings["DB"].ConnectionString))
			{
				ds = MySqlHelper.ExecuteDataset(conn, "GetHomepageItems");
			}
			foreach (DataRow row in ds.Tables[0].Rows)
			{
				Food item = new Food();
				item.ID = (int)row["ID"];
				item.TypeID = (int)row["TypeID"];
				item.Name = row["Name"].ToString();
				item.Description = row["Description"].ToString();
				item.Price = (double)row["Price"];
				item.Image = row["Image"].ToString() == "" ? "" : "data:image/png;base64," + Convert.ToBase64String((byte[])row["Image"]);
				item.Quantity = Convert.ToInt32(row["Quantity"]); 
				menu.Add(item);
			}

			return menu;
		}

		public static List<Food> GetMenu()
		{
			DataSet ds;
			var menu = new List<Food>();
			using (MySqlConnection conn = new MySqlConnection(ConfigurationManager.ConnectionStrings["DB"].ConnectionString))
			{
				ds = MySqlHelper.ExecuteDataset(conn, "GetMenu");
			}
			foreach (DataRow row in ds.Tables[0].Rows)
			{
				Food item = new Food();
				item.ID = (int)row["ID"];
				item.TypeID = (int)row["TypeID"];
				item.Name = row["Name"].ToString();
				item.TypeName = row["TypeName"].ToString();
				item.Description = row["Description"].ToString();
				item.Price = (double)row["Price"];
				item.PreparedBy = GetChefs().Where(x => x.ID == (int)row["ChefID"]).FirstOrDefault().FullName; 
				item.Image = row["Image"].ToString() == "" ? "" : "data:image/png;base64," + Convert.ToBase64String((byte[])row["Image"]);
				item.Rating = Convert.ToInt32(row["Rating"]); 
				menu.Add(item);
			}

			return menu;
		}

		public static List<Chef> GetChefs()
		{
			DataSet ds;
			var chefs = new List<Chef>();
			using (MySqlConnection conn = new MySqlConnection(ConfigurationManager.ConnectionStrings["DB"].ConnectionString))
			{
				ds = MySqlHelper.ExecuteDataset(conn, "GetChefs");
			}
			foreach (DataRow row in ds.Tables[0].Rows)
			{
				Chef chef = new Chef();
				chef.ID = (int)row["ID"];
				chef.FullName = row["first_name"].ToString() + ' ' + row["last_name"].ToString();
				chef.Username = row["username"].ToString();
				chef.Password = row["password"].ToString();
				chefs.Add(chef);
			}

			return chefs;
		}

		public static int Create(int TypeID, int ChefID, string Name, string Description, double Price, byte[] Image)
		{
			DataTable dt = new DataTable();
			using (MySqlConnection conn = new MySqlConnection(ConfigurationManager.ConnectionStrings["DB"].ConnectionString))
			{
				using(MySqlCommand cmd = new MySqlCommand("CreateFoodItem", conn))
				{
					cmd.CommandType = CommandType.StoredProcedure;
					cmd.Parameters.AddWithValue("TypeID", TypeID);
					cmd.Parameters.AddWithValue("ChefID", ChefID);
					cmd.Parameters.AddWithValue("Name", Name);
					cmd.Parameters.AddWithValue("Description", Description);
					cmd.Parameters.AddWithValue("Price", Price);
					cmd.Parameters.AddWithValue("Image", Image);
					using(MySqlDataAdapter sda = new MySqlDataAdapter(cmd))
					{
						sda.Fill(dt); 
					}
				}

			}
			int ID = int.Parse(dt.Rows[0].ItemArray[0].ToString());
			return ID; 
		}

		public static void Update(int ID, int TypeID, int ChefID, string Name, string Description, double Price, byte [] Image)
		{
			DataTable dt = new DataTable();
			using (MySqlConnection conn = new MySqlConnection(ConfigurationManager.ConnectionStrings["DB"].ConnectionString))
			{
				using (MySqlCommand cmd = new MySqlCommand("UpdateFoodItem", conn))
				{
					cmd.CommandType = CommandType.StoredProcedure;
					cmd.Parameters.AddWithValue("ID", ID);
					cmd.Parameters.AddWithValue("TypeID", TypeID);
					cmd.Parameters.AddWithValue("ChefID", ChefID);
					cmd.Parameters.AddWithValue("Name", Name);
					cmd.Parameters.AddWithValue("Description", Description);
					cmd.Parameters.AddWithValue("Price", Price);
					cmd.Parameters.AddWithValue("Image", Image);
					using (MySqlDataAdapter sda = new MySqlDataAdapter(cmd))
					{
						sda.Fill(dt);
					}
				}
			}
		}

		public static void Delete(int ID)
		{
			DataTable dt = new DataTable();
			using (MySqlConnection conn = new MySqlConnection(ConfigurationManager.ConnectionStrings["DB"].ConnectionString))
			{
				using (MySqlCommand cmd = new MySqlCommand("DeleteFoodItem", conn))
				{
					cmd.CommandType = CommandType.StoredProcedure;
					cmd.Parameters.AddWithValue("item_id", ID);
					conn.Open(); 
					cmd.ExecuteNonQuery();
					conn.Close(); 
				}

			}
		}

	}
}