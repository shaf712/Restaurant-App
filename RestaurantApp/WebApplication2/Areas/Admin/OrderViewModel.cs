using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using WebApplication2.Areas.Admin.Models;
using WebApplication2.Areas.Admin;
using WebApplication2.Objects;

namespace WebApplication2.Areas
{
    public class OrderViewModel
    {
        public static List<Order> GetOrders()
        {
            DataSet ds;
            var orders = new List<Order>();
            using (MySqlConnection conn = new MySqlConnection(ConfigurationManager.ConnectionStrings["DB"].ConnectionString))
            {
                ds = MySqlHelper.ExecuteDataset(conn, "GetOrders");
            }
            foreach(DataRow row in ds.Tables[0].Rows)
            {
                Order order = new Order();
                order.ID = (int)row["ID"];
                order.Username = row["username"].ToString();
                order.UserID = (int)row["user_id"]; 
                order.Item = MenuViewModel.GetMenu().Where(x => x.ID == (int)row["ItemID"]).FirstOrDefault().Name;
                order.ChefName = row["chef_name"].ToString();
                order.Address = row["Address"].ToString();
                order.Cost = (double)row["Cost"];
                order.Quantity = (double)row["Quantity"];
                order.Timestamp = row["Timestamp"].ToString();

                orders.Add(order);
            }

            return orders;
        }

        public static void DeleteOrder(int ID)
        {
            DataTable dt = new DataTable();
            using (MySqlConnection conn = new MySqlConnection(ConfigurationManager.ConnectionStrings["DB"].ConnectionString))
            {
                using (MySqlCommand cmd = new MySqlCommand("DeleteOrder", conn))
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