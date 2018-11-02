using MySql.Data.MySqlClient;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using WebApplication2.Areas.Admin;
using WebApplication2.Areas.Admin.Models;
using WebApplication2.Objects;

namespace WebApplication2.Models
{
	public class UserViewModel
	{
		public static List<Food> UserGetRecentPurchased(int ID)
		{
			DataTable dt = new DataTable();
			List<Food> list = new List<Food>(); 
			using (MySqlConnection conn = new MySqlConnection(ConfigurationManager.ConnectionStrings["DB"].ConnectionString))
			{
				using (MySqlCommand cmd = new MySqlCommand("UserGetRecentPurchased", conn))
				{
					cmd.CommandType = CommandType.StoredProcedure;
					cmd.Parameters.AddWithValue("UserID", ID);
					using (MySqlDataAdapter sda = new MySqlDataAdapter(cmd))
					{
						sda.Fill(dt);
					}
				}

				foreach (DataRow dr in dt.Rows)
				{
					Food food = new Food();
					food.ID = (int)dr.ItemArray[0];
					food.Name = dr.ItemArray[1].ToString();
					food.Description = dr.ItemArray[2].ToString();
					food.Price = (double)dr.ItemArray[3]; 
					food.Image = "data:image/png;base64," + System.Convert.ToBase64String((byte[])dr.ItemArray[4]);
					food.PreparedBy = dr.ItemArray[5].ToString();
					food.ChefID = (int)dr.ItemArray[6];
					food.OrderedDate = (System.DateTime)dr.ItemArray[7]; 
                    food.DeliverymanID = (int)dr.ItemArray[8];
                    food.DeliveredBy = dr.ItemArray[9].ToString();
                    list.Add(food); 
				}
			}

			var recently_purchased = list.GroupBy(x => x.ID).Select(g => g.First()).ToList();
			return recently_purchased;
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
				u.ID = (int)row["ID"];
				u.Username = row["username"].ToString(); 
				var balance = (double)row["balance"];
				u.Balance = balance.ToString();
				Users.Add(u);
			}
			return Users; 
		}
		
		public static void RegisterUser(string Username, string Password, double Deposit)
		{
			using (MySqlConnection conn = new MySqlConnection(ConfigurationManager.ConnectionStrings["DB"].ConnectionString))
			{
				DataTable dt = new DataTable();
				using (MySqlCommand cmd = new MySqlCommand("AddUser", conn))
				{
					cmd.CommandType = CommandType.StoredProcedure;
					cmd.Parameters.AddWithValue("Username", Username);
					cmd.Parameters.AddWithValue("Password", Password);
					cmd.Parameters.AddWithValue("Deposit", Deposit);
					conn.Open();
					cmd.ExecuteNonQuery();
					conn.Close();
				}
			}
		}

		public static void UserSubstractFunds(int ID, double Balance)
		{
			using (MySqlConnection conn = new MySqlConnection(ConfigurationManager.ConnectionStrings["DB"].ConnectionString))
			{
				DataTable dt = new DataTable();
				using (MySqlCommand cmd = new MySqlCommand("UserSubtractFunds", conn))
				{
					cmd.CommandType = CommandType.StoredProcedure;
					cmd.Parameters.AddWithValue("ID", ID);
					cmd.Parameters.AddWithValue("balance", Balance);
					conn.Open();
					cmd.ExecuteNonQuery();
					conn.Close();
				}
			}
		}

		public static void UserSubmitReview(int UserID, int isVVIP, int ItemID, int ChefID, int DeliverymanID, int Rating, int DeliveryRating, int Approval, string Comment)
		{
            int Delivery_Approval = 0; 
            if(DeliveryRating >= 3)
            {
                Delivery_Approval = 1; 
            }
            else
            {
                Delivery_Approval = -1; 
            }

            Deliveryman deliveryman = new Deliveryman(); 

            var deliverymen = ManagerViewModel.GetDeliverymen(); 
            for(int j = 0; j < deliverymen.Count; j++)
            {
                if(deliverymen[j].ID == DeliverymanID)
                {
                    deliveryman = deliverymen[j]; 
                }
            }


			if(Rating >= 3)
			{
				Approval = 1;
			}
			else
			{
				Approval = -1; 
			}

			Chef chef = new Chef(); 

			var ChefList = ManagerViewModel.GetChefs(); 
			for(int i = 0; i < ChefList.Count; i++)
			{
				if(ChefList[i].ID == ChefID)
				{
					chef = ChefList[i]; 
				}
			}

			if(isVVIP == 1)
			{
				Approval *= 2;
                Delivery_Approval *= 2; 
			}

            chef.Approval += Approval;
            deliveryman.Approval += Delivery_Approval; 
            using (MySqlConnection conn = new MySqlConnection(ConfigurationManager.ConnectionStrings["DB"].ConnectionString))
            {
                DataTable dt = new DataTable();
                using (MySqlCommand cmd = new MySqlCommand("UpdateChefApproval", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("ChefID", ChefID);
                    cmd.Parameters.AddWithValue("Approval", chef.Approval);
                    conn.Open();
                    cmd.ExecuteNonQuery();
                    conn.Close();
                }
            }

            using (MySqlConnection conn = new MySqlConnection(ConfigurationManager.ConnectionStrings["DB"].ConnectionString))
            {
                DataTable dt = new DataTable();
                using (MySqlCommand cmd = new MySqlCommand("UpdateDeliverymanApproval", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("DeliverymanID", DeliverymanID);
                    cmd.Parameters.AddWithValue("Approval", deliveryman.Approval);
                    conn.Open();
                    cmd.ExecuteNonQuery();
                    conn.Close();
                }
            }

            using (MySqlConnection conn = new MySqlConnection(ConfigurationManager.ConnectionStrings["DB"].ConnectionString))
			{
				DataTable dt = new DataTable();
				using (MySqlCommand cmd = new MySqlCommand("UserSubmitReview", conn))
				{
					cmd.CommandType = CommandType.StoredProcedure;
					cmd.Parameters.AddWithValue("UserID", UserID);
					cmd.Parameters.AddWithValue("ItemID", ItemID);
					cmd.Parameters.AddWithValue("ChefID", ChefID);
					cmd.Parameters.AddWithValue("Rating", Rating);
					cmd.Parameters.AddWithValue("Approval", Approval);
					cmd.Parameters.AddWithValue("Comment", Comment);
					conn.Open();
					cmd.ExecuteNonQuery();
					conn.Close();
				}
			}
		}

		public static void InsertOrder(int UserID, int ItemID, string Address, double Cost, int Quantity)
		{
            int random = 0;
            int deliverymanID = 1; 
            var deliverymen = ManagerViewModel.GetDeliverymen(); 
            while(random == 0)
            {
                System.Random r = new System.Random(System.DateTime.Now.Second);
                deliverymanID = r.Next(deliverymen.Count + 1); 
                for(int i = 0; i < deliverymen.Count; i++)
                {
                    if (deliverymanID == deliverymen[i].ID)
                        random = 1; 
                }
            }

			using (MySqlConnection conn = new MySqlConnection(ConfigurationManager.ConnectionStrings["DB"].ConnectionString))
			{
				DataTable dt = new DataTable();
				using (MySqlCommand cmd = new MySqlCommand("InsertOrder", conn))
				{
					cmd.CommandType = CommandType.StoredProcedure;
					cmd.Parameters.AddWithValue("UserID", UserID);
					cmd.Parameters.AddWithValue("ItemID", ItemID);
					cmd.Parameters.AddWithValue("Address", Address);
					cmd.Parameters.AddWithValue("Cost", Cost);
					cmd.Parameters.AddWithValue("Quantity", Quantity);
                    cmd.Parameters.AddWithValue("DeliverymanID", deliverymanID);
                    conn.Open();
					cmd.ExecuteNonQuery();
					conn.Close();
				}
			}
		}

		public static void Deposit(int ID, double Deposit)
		{
			using (MySqlConnection conn = new MySqlConnection(ConfigurationManager.ConnectionStrings["DB"].ConnectionString))
			{
				DataTable dt = new DataTable();
				using (MySqlCommand cmd = new MySqlCommand("UserDepositMoney", conn))
				{
					cmd.CommandType = CommandType.StoredProcedure;
					cmd.Parameters.AddWithValue("UserID", ID);
					cmd.Parameters.AddWithValue("Deposit", Deposit);
					conn.Open();
					cmd.ExecuteNonQuery();
					conn.Close();
				}
			}
		}
	}
}