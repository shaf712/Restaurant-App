using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Data;
using WebApplication2.Areas.Admin.Models;

namespace WebApplication2.Areas.Admin
{
    public class UserViewModel
    {
        public static List<User> GetUsers()
        {
            DataSet ds;
            var uList = new List<User>();
            using (MySqlConnection conn = new MySqlConnection(ConfigurationManager.ConnectionStrings["DB"].ConnectionString))
            {
                ds = MySqlHelper.ExecuteDataset(conn, "GetUsers");
            }
            foreach (DataRow row in ds.Tables[0].Rows)
            {
                User u = new User();
                u.ID = (int)row["id"];
                u.Username = row["username"].ToString();
                u.Password = row["password"].ToString();
                u.Permission = (int)row["permission"];
                uList.Add(u);
            }

            return uList;
        }

        public static int Create(string username, string password, int permission)
        {
            DataTable dt = new DataTable();
            using (MySqlConnection conn = new MySqlConnection(ConfigurationManager.ConnectionStrings["DB"].ConnectionString))
            {
                using (MySqlCommand cmd = new MySqlCommand("CreateUser", conn))
                {
                    try
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("username", username);
                        cmd.Parameters.AddWithValue("password", password);
                        cmd.Parameters.AddWithValue("permission", permission);
                        using (MySqlDataAdapter sda = new MySqlDataAdapter(cmd))
                        {
                            sda.Fill(dt);
                        }
                        int ID = int.Parse(dt.Rows[0].ItemArray[0].ToString());
                        return ID;

                    }
                    
                    catch(MySqlException ex)
                    {
                        switch(ex.Number)
                        {
                            case 1062:
                                Console.WriteLine("Error: User already exists.");
                                break;
                            default:
                                break;

                        }

                    }
                }

            }

            return 0;

        }

        public static void Update(int id, string n_username, string password, int permission)
        {
            DataTable dt = new DataTable();
            using (MySqlConnection conn = new MySqlConnection(ConfigurationManager.ConnectionStrings["DB"].ConnectionString))
            {
                using (MySqlCommand cmd = new MySqlCommand("UpdateUser", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("id", id);
                    cmd.Parameters.AddWithValue("username", n_username);
                    cmd.Parameters.AddWithValue("password", password);
                    cmd.Parameters.AddWithValue("permission", permission);
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
                using (MySqlCommand cmd = new MySqlCommand("DeleteUser", conn))
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