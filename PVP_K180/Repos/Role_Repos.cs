using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using MySql.Data.MySqlClient;
using PVP_K180.Models;


namespace PVP_K180.Repos
{
    public class Role_Repos
    {
        public Role Gauti_Role(int id)
        {
            Role role = new Role();
            string conn = ConfigurationManager.ConnectionStrings["MysqlConnection"].ConnectionString;
            MySqlConnection mySqlConnection = new MySqlConnection(conn);
            string sqlquery = "select * from " + "Role where id_Role=" + id;
            MySqlCommand mySqlCommand = new MySqlCommand(sqlquery, mySqlConnection);
            mySqlCommand.Parameters.Add("?id", MySqlDbType.VarChar).Value = id;
            mySqlConnection.Open();
            MySqlDataAdapter mda = new MySqlDataAdapter(mySqlCommand);
            DataTable dt = new DataTable();
            mda.Fill(dt);
            mySqlConnection.Close();

            foreach (DataRow item in dt.Rows)
            {
                role.id_Role = Convert.ToInt32(item["id_Role"]);
                role.name = Convert.ToString(item["name"]);
            }
            return role;
        }

        public List<Role> GautiRoles()
        {
            try
            {
                List<Role> roles = new List<Role>();
                string conn = ConfigurationManager.ConnectionStrings["MysqlConnection"].ConnectionString;
                MySqlConnection mySqlConnection = new MySqlConnection(conn);
                string sqlquery = "SELECT * FROM `Role` WHERE 1";
                MySqlCommand mySqlCommand = new MySqlCommand(sqlquery, mySqlConnection);
                mySqlConnection.Open();
                MySqlDataAdapter mda = new MySqlDataAdapter(mySqlCommand);
                DataTable dt = new DataTable();
                mda.Fill(dt);
                mySqlConnection.Close();

                foreach (DataRow item in dt.Rows)
                {
                    roles.Add(new Role
                    {

                        id_Role = Convert.ToInt32(item["id_Role"]),
                        name = Convert.ToString(item["name"])
                    }); ;
                }
                return roles;
            }

            catch (Exception ex)
            {
                return null;
            }
        }
    }
}