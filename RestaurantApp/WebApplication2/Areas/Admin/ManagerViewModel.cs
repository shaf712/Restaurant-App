using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using WebApplication2.Areas.Admin.Models;
using WebApplication2.Objects;

namespace WebApplication2.Areas.Admin
{
	public class ManagerViewModel
	{
		public static List<Chef> GetChefs()
		{
			DataSet ds;
			var chefs = new List<Chef>();
			using (MySqlConnection conn = new MySqlConnection(ConfigurationManager.ConnectionStrings["DB"].ConnectionString))
			{
				ds = MySqlHelper.ExecuteDataset(conn, "GetChefManagement");
			}
			foreach (DataRow row in ds.Tables[0].Rows)
			{
				Chef chef = new Chef();
				chef.ID = (int)row["ID"];
				chef.FullName = row["first_name"].ToString() + ' ' + row["last_name"].ToString(); 
				chef.Salary = (int)row["salary"];
				chef.Username = row["username"].ToString(); 
				chef.Password = row["password"].ToString(); 
				chef.Approval = Convert.ToInt32(row["approval"]);
				chef.DemotionCount = (int)row["demotion_count"];
				chef.PromotionCount = Convert.ToInt32(row["promotion_count"]);
				chef.LastOrder = string.IsNullOrEmpty(row["last_order_date"].ToString()) ? "" : row["last_order_date"].ToString(); 
				chefs.Add(chef);
			}

			return chefs;
		}

        public static List<Deliveryman> GetDeliverymen()
        {
            DataSet ds;
            var deliverymen = new List<Deliveryman>();
            using (MySqlConnection conn = new MySqlConnection(ConfigurationManager.ConnectionStrings["DB"].ConnectionString))
            {
                ds = MySqlHelper.ExecuteDataset(conn, "GetDeliverymen");
            }
            foreach (DataRow row in ds.Tables[0].Rows)
            {
                Deliveryman deliveryman = new Deliveryman();
                deliveryman.ID = (int)row["ID"];
                deliveryman.FullName = row["first_name"].ToString() + ' ' + row["last_name"].ToString();
                deliveryman.Salary = (int)row["salary"];
                deliveryman.Username = row["username"].ToString();
                deliveryman.Password = row["password"].ToString();
                deliveryman.Approval = Convert.ToInt32(row["approval"]);
                deliveryman.DemotionCount = (int)row["demotion_count"];
                deliveryman.PromotionCount = Convert.ToInt32(row["promotion_count"]);
                deliverymen.Add(deliveryman);
            }

            return deliverymen;
        }

        public static Chef DemoteDeliveryman(int DeliverymanID)
        {
            DataTable dt = new DataTable();

            var Deliveryman = GetDeliverymen().Where(x => x.ID == DeliverymanID).FirstOrDefault();
            int NewSalary = Deliveryman.Salary - (5000);
            int DemotionCount = Deliveryman.DemotionCount + 1;

            using (MySqlConnection conn = new MySqlConnection(ConfigurationManager.ConnectionStrings["DB"].ConnectionString))
            {
                using (MySqlCommand cmd = new MySqlCommand("DemoteDeliveryman", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("DeliverymanID", DeliverymanID);
                    cmd.Parameters.AddWithValue("NewSalary", NewSalary);
                    cmd.Parameters.AddWithValue("DemotionCount", DemotionCount);
                    using (MySqlDataAdapter sda = new MySqlDataAdapter(cmd))
                    {
                        sda.Fill(dt);
                    }
                }

            }

            Deliveryman deliveryman = new Deliveryman();
            deliveryman.ID = int.Parse(dt.Rows[0].ItemArray[0].ToString());
            deliveryman.Salary = int.Parse(dt.Rows[0].ItemArray[5].ToString());
            deliveryman.PromotionCount = int.Parse(dt.Rows[0].ItemArray[7].ToString());

            return deliveryman;
        }

        public static Chef PromoteDeliveryman(int DeliverymanID)
        {
            DataTable dt = new DataTable();

            var Deliveryman = GetDeliverymen().Where(x => x.ID == DeliverymanID).FirstOrDefault();
            Deliveryman.PromotionCount++;
            int PromotionCount = Deliveryman.PromotionCount;
            int NewSalary = Deliveryman.Salary + (5000 * PromotionCount);
            int DemotionCount = Deliveryman.DemotionCount == 0 ? 0 : Deliveryman.DemotionCount -= 1;

            using (MySqlConnection conn = new MySqlConnection(ConfigurationManager.ConnectionStrings["DB"].ConnectionString))
            {
                using (MySqlCommand cmd = new MySqlCommand("PromoteDeliveryman", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("DeliverymanID", DeliverymanID);
                    cmd.Parameters.AddWithValue("NewSalary", NewSalary);
                    cmd.Parameters.AddWithValue("DemotionCount", DemotionCount);
                    cmd.Parameters.AddWithValue("PromotionCount", PromotionCount);
                    using (MySqlDataAdapter sda = new MySqlDataAdapter(cmd))
                    {
                        sda.Fill(dt);
                    }
                }

            }

            Deliveryman deliveryman = new Deliveryman();
            deliveryman.ID = int.Parse(dt.Rows[0].ItemArray[0].ToString());
            deliveryman.Salary = int.Parse(dt.Rows[0].ItemArray[5].ToString());
            deliveryman.PromotionCount = int.Parse(dt.Rows[0].ItemArray[7].ToString());

            return deliveryman;
        }


        public static Chef PromoteChef(int ChefID)
		{
			DataTable dt = new DataTable();

			var Chef = GetChefs().Where(x => x.ID == ChefID).FirstOrDefault();
			Chef.PromotionCount++;
			int PromotionCount = Chef.PromotionCount;
			int NewSalary = Chef.Salary + (5000 * PromotionCount); 
			int DemotionCount = Chef.DemotionCount == 0 ? 0 : Chef.DemotionCount -= 1; 

			using (MySqlConnection conn = new MySqlConnection(ConfigurationManager.ConnectionStrings["DB"].ConnectionString))
			{
				using (MySqlCommand cmd = new MySqlCommand("PromoteChef", conn))
				{
					cmd.CommandType = CommandType.StoredProcedure;
					cmd.Parameters.AddWithValue("ChefID", ChefID);
					cmd.Parameters.AddWithValue("NewSalary", NewSalary);
					cmd.Parameters.AddWithValue("DemotionCount", DemotionCount);
					cmd.Parameters.AddWithValue("PromotionCount", PromotionCount);
					using (MySqlDataAdapter sda = new MySqlDataAdapter(cmd))
					{
						sda.Fill(dt);
					}
				}

			}

			Chef chef = new Chef();
			chef.ID = int.Parse(dt.Rows[0].ItemArray[0].ToString());
			chef.Salary = int.Parse(dt.Rows[0].ItemArray[5].ToString());
			chef.PromotionCount = int.Parse(dt.Rows[0].ItemArray[7].ToString());

			return chef; 
		}

		public static Chef DemoteChef(int ChefID)
		{
			DataTable dt = new DataTable();

			var Chef = GetChefs().Where(x => x.ID == ChefID).FirstOrDefault();
			int NewSalary = Chef.Salary - (5000);
			int DemotionCount = Chef.DemotionCount + 1; 

			using (MySqlConnection conn = new MySqlConnection(ConfigurationManager.ConnectionStrings["DB"].ConnectionString))
			{
				using (MySqlCommand cmd = new MySqlCommand("DemoteChef", conn))
				{
					cmd.CommandType = CommandType.StoredProcedure;
					cmd.Parameters.AddWithValue("ChefID", ChefID);
					cmd.Parameters.AddWithValue("NewSalary", NewSalary);
					cmd.Parameters.AddWithValue("DemotionCount", DemotionCount);
					using (MySqlDataAdapter sda = new MySqlDataAdapter(cmd))
					{
						sda.Fill(dt);
					}
				}

			}

			Chef chef = new Chef();
			chef.ID = int.Parse(dt.Rows[0].ItemArray[0].ToString());
			chef.Salary = int.Parse(dt.Rows[0].ItemArray[5].ToString());
			chef.PromotionCount = int.Parse(dt.Rows[0].ItemArray[7].ToString());

			return chef;
		}

		public static void FireChef(int ChefID)
		{
			DataTable dt = new DataTable();
			using (MySqlConnection conn = new MySqlConnection(ConfigurationManager.ConnectionStrings["DB"].ConnectionString))
			{
				using (MySqlCommand cmd = new MySqlCommand("FireChef", conn))
				{
					cmd.CommandType = CommandType.StoredProcedure;
					cmd.Parameters.AddWithValue("ChefID", ChefID);
					conn.Open();
					cmd.ExecuteNonQuery();
					conn.Close();
				}

			}
		}

        public static void FireDeliveryman(int ID)
        {
            DataTable dt = new DataTable();
            using (MySqlConnection conn = new MySqlConnection(ConfigurationManager.ConnectionStrings["DB"].ConnectionString))
            {
                using (MySqlCommand cmd = new MySqlCommand("FireDeliveryman", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("DeliverymanID", ID);
                    conn.Open();
                    cmd.ExecuteNonQuery();
                    conn.Close();
                }

            }
        }

        public static void PromoteUser(int userID)
		{
			DataTable dt = new DataTable();
			using (MySqlConnection conn = new MySqlConnection(ConfigurationManager.ConnectionStrings["DB"].ConnectionString))
			{
				using (MySqlCommand cmd = new MySqlCommand("PromoteUser", conn))
				{
					cmd.CommandType = CommandType.StoredProcedure;
					cmd.Parameters.AddWithValue("UserID", userID);
					conn.Open();
					cmd.ExecuteNonQuery();
					conn.Close();
				}

			}
		}
		public static void SuspendUser(int userID)
		{
			DataTable dt = new DataTable();
			using (MySqlConnection conn = new MySqlConnection(ConfigurationManager.ConnectionStrings["DB"].ConnectionString))
			{
				using (MySqlCommand cmd = new MySqlCommand("SuspendUser", conn))
				{
					cmd.CommandType = CommandType.StoredProcedure;
					cmd.Parameters.AddWithValue("UserID", userID);
					conn.Open();
					cmd.ExecuteNonQuery();
					conn.Close();
				}

			}
		}
		public static void Verify(int userID)
		{
			DataTable dt = new DataTable();
			using (MySqlConnection conn = new MySqlConnection(ConfigurationManager.ConnectionStrings["DB"].ConnectionString))
			{
				using (MySqlCommand cmd = new MySqlCommand("VerifyUser", conn))
				{
					cmd.CommandType = CommandType.StoredProcedure;
					cmd.Parameters.AddWithValue("UserID", userID);
					conn.Open();
					cmd.ExecuteNonQuery();
					conn.Close();
				}

			}
		}
		public static void IssueWarning(int userID)
		{
			var user = GetUsers().Where(x => x.ID == userID).FirstOrDefault();
			var warning_count = user.WarningCount; 
			warning_count++;
			int remove_VIP = 0; 
			if(warning_count >= 2 && user.VIP == 1)
			{
				remove_VIP = 1;
			}
			DataTable dt = new DataTable();
			if (remove_VIP == 1)
			{
                user.DemotionCount++; 
				using (MySqlConnection conn = new MySqlConnection(ConfigurationManager.ConnectionStrings["DB"].ConnectionString))
				{
					using (MySqlCommand cmd = new MySqlCommand("RemoveVIPStatus", conn))
					{
						cmd.CommandType = CommandType.StoredProcedure;
						cmd.Parameters.AddWithValue("UserID", userID);
                        cmd.Parameters.AddWithValue("DemotionCount", user.DemotionCount);
                        conn.Open();
						cmd.ExecuteNonQuery();
						conn.Close();
					}
				}
			}
			else
			{
				using (MySqlConnection conn = new MySqlConnection(ConfigurationManager.ConnectionStrings["DB"].ConnectionString))
				{
					using (MySqlCommand cmd = new MySqlCommand("IssueWarning", conn))
					{
						cmd.CommandType = CommandType.StoredProcedure;
						cmd.Parameters.AddWithValue("UserID", userID);
						cmd.Parameters.AddWithValue("WarningCount", warning_count);
						conn.Open();
						cmd.ExecuteNonQuery();
						conn.Close();
					}
				}
			}
		}

		public static int AddChef(string FirstName, string LastName, int Salary)
		{
			DataTable dt = new DataTable();
			using (MySqlConnection conn = new MySqlConnection(ConfigurationManager.ConnectionStrings["DB"].ConnectionString))
			{
				using (MySqlCommand cmd = new MySqlCommand("AddChef", conn))
				{
					cmd.CommandType = CommandType.StoredProcedure;
					cmd.Parameters.AddWithValue("FirstName", FirstName);
					cmd.Parameters.AddWithValue("LastName", LastName);
					cmd.Parameters.AddWithValue("Salary", Salary);
					using (MySqlDataAdapter sda = new MySqlDataAdapter(cmd))
					{
						sda.Fill(dt);
					}
				}

			}
			int ID = int.Parse(dt.Rows[0].ItemArray[0].ToString());
			return ID;

		}

        public static int AddDeliveryman(string FirstName, string LastName, int Salary)
        {
            DataTable dt = new DataTable();
            using (MySqlConnection conn = new MySqlConnection(ConfigurationManager.ConnectionStrings["DB"].ConnectionString))
            {
                using (MySqlCommand cmd = new MySqlCommand("AddDeliveryman", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("FirstName", FirstName);
                    cmd.Parameters.AddWithValue("LastName", LastName);
                    cmd.Parameters.AddWithValue("Salary", Salary);
                    using (MySqlDataAdapter sda = new MySqlDataAdapter(cmd))
                    {
                        sda.Fill(dt);
                    }
                }

            }
            int ID = int.Parse(dt.Rows[0].ItemArray[0].ToString());
            return ID;

        }


        public static List<Login> GetUsers()
		{
			DataSet ds;
			List<Login> Users = new List<Login>();
			using (MySqlConnection conn = new MySqlConnection(ConfigurationManager.ConnectionStrings["DB"].ConnectionString))
			{
				ds = MySqlHelper.ExecuteDataset(conn, "GetUsers");
			}
			foreach (DataRow row in ds.Tables[0].Rows)
			{
				Login u = new Login();
				u.Username = row["username"].ToString();
				u.ID = (int)row["ID"];
				u.WarningCount = Convert.ToInt32(row["warning_count"]); 
				u.Balance = row["balance"].ToString();
				u.isVIP = (bool)row["is_VIP"]; 
				u.Verified = Convert.ToInt32(row["is_verified"]);
				u.isSuspended = Convert.ToBoolean(row["is_suspended"]);
                u.MoneySpent = row["money_spent"].ToString(); 
				u.VIP = Convert.ToInt32(row["is_VIP"]);
                u.DemotionCount = Convert.ToInt32(row["demotion_count"]); 
				Users.Add(u);
			}

			return Users; 
		}

		public static List<UserReview> GetUserDetails(int UserID)
		{
			DataSet ds;
			List<UserReview> list = new List<UserReview>();
			using (MySqlConnection conn = new MySqlConnection(ConfigurationManager.ConnectionStrings["DB"].ConnectionString))
			{
				ds = MySqlHelper.ExecuteDataset(conn, "GetUserDetails");
			}
			foreach (DataRow row in ds.Tables[0].Rows)
			{
				UserReview u = new UserReview();
				u.ID = (int)row["ID"]; 
				u.UserID = (int)row["UserID"]; 
				u.Name = row["Name"].ToString();
				u.PreparedBy = row["PreparedBy"].ToString();
				u.Rating = (int)row["rating"];
				u.Approval = (int)row["Approval"];
				u.Comment = row["Comment"].ToString();
				list.Add(u);
			}

			List<UserReview> UserReview = new List<UserReview>();

			for (int i = 0; i < list.Count; i++)
			{
				if (list[i].UserID == UserID)
					UserReview.Add(list[i]); 
			}

			return UserReview;
		}
	}
}