using MySql.Data.MySqlClient;
using PVP_K180.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace PVP_K180.Repos
{
    public class Nuotrauka_Repos
    {
        public bool Prideti_Naujienos_Nuotraukas(Nuotrauka nuotrauka)
        {
            try
            {
                string conn = ConfigurationManager.ConnectionStrings["MysqlConnection"].ConnectionString;
                MySqlConnection mySqlConnection = new MySqlConnection(conn);
                string sqlquery = "INSERT INTO `Nuotrauka`(`nuotraukos_nuroda`,`fk_Naujienaid_Naujiena`) " +
                    "VALUES (?nuotraukos_nuoroda,?fk_Naujienaid_Naujiena)";
                MySqlCommand mySqlCommand = new MySqlCommand(sqlquery, mySqlConnection);
                mySqlCommand.Parameters.Add("?nuotraukos_nuoroda", MySqlDbType.VarChar).Value = nuotrauka.nuotraukos_nuoroda;
                mySqlCommand.Parameters.Add("?fk_Naujienaid_Naujiena", MySqlDbType.VarChar).Value = nuotrauka.priskirtas_id;
                mySqlConnection.Open();
                mySqlCommand.ExecuteNonQuery();
                mySqlConnection.Close();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}