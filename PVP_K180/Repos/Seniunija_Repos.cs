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
    public class Seniunija_Repos
    {
        public Seniunija Gauti_Seniunija()
        {
            Seniunija seniunija = new Seniunija();
            string conn = ConfigurationManager.ConnectionStrings["MysqlConnection"].ConnectionString;
            MySqlConnection mySqlConnection = new MySqlConnection(conn);
            string sqlquery = "select * from " + "Seniunyja where id_Seniunyja=" + 1;
            MySqlCommand mySqlCommand = new MySqlCommand(sqlquery, mySqlConnection);
            mySqlConnection.Open();
            MySqlDataAdapter mda = new MySqlDataAdapter(mySqlCommand);
            DataTable dt = new DataTable();
            mda.Fill(dt);
            mySqlConnection.Close();

            foreach (DataRow item in dt.Rows)
            {
                seniunija.id_Seniunyja = Convert.ToInt32(item["id_Seniunyja"]);
                seniunija.seniunyjos_pavadinimas = Convert.ToString(item["seniunyjos_pavadinimas"]);
                seniunija.aprasymas = Convert.ToString(item["aprasymas"]);
                if (item["zemelapis_ilguma"] == DBNull.Value)
                {
                    seniunija.zemelapis_ilguma = null;
                }
                else
                {
                    seniunija.zemelapis_ilguma = (float)Convert.ToDouble(item["zemelapis_ilguma"]);
                }

                if (item["zemelapis_platuma"] == DBNull.Value)
                {
                    seniunija.zemelapis_platuma = null;
                }
                else
                {
                    seniunija.zemelapis_platuma = (float)Convert.ToDouble(item["zemelapis_platuma"]);
                }
            }
            return seniunija;
        }

        public bool AtnaujintiSeniunijosInfo(Seniunija seniunijaData)
        {
            try
            {
                string conn = ConfigurationManager.ConnectionStrings["MysqlConnection"].ConnectionString;
                MySqlConnection mySqlConnection = new MySqlConnection(conn);
                string sqlquery = "UPDATE `Seniunyja` SET `seniunyjos_pavadinimas`=?pavadinimas,`aprasymas`=?aprasymas,`zemelapis_ilguma`=?zemelapis_ilguma,`zemelapis_platuma`=?zemelapis_platuma WHERE id_Seniunyja=" +1;
                MySqlCommand mySqlCommand = new MySqlCommand(sqlquery, mySqlConnection);
                mySqlCommand.Parameters.Add("?pavadinimas", MySqlDbType.VarChar).Value = seniunijaData.seniunyjos_pavadinimas;
                mySqlCommand.Parameters.Add("?aprasymas", MySqlDbType.VarChar).Value = seniunijaData.aprasymas;
                mySqlCommand.Parameters.Add("?zemelapis_ilguma", MySqlDbType.Float).Value = seniunijaData.zemelapis_ilguma;
                mySqlCommand.Parameters.Add("?zemelapis_platuma", MySqlDbType.Float).Value = seniunijaData.zemelapis_platuma;
                mySqlConnection.Open();
                mySqlCommand.ExecuteNonQuery();
                mySqlConnection.Close();
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }
    }
}