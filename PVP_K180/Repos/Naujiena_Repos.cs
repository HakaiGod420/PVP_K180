using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using MySql.Data.MySqlClient;
using PVP_K180.Models;
using PVP_K180.ModelView;

namespace PVP_K180.Repos
{
    public class Naujiena_Repos
    {
        public bool Sukurti_Naujiena(Naujiena naujiena)
        {
            try
            {
                string conn = ConfigurationManager.ConnectionStrings["MysqlConnection"].ConnectionString;
                MySqlConnection mySqlConnection = new MySqlConnection(conn);
                string sqlquery = "INSERT INTO `Naujiena`(`pavadinimas`,`naujienos_tekstas`, `sukurimo_data`, `fk_Vartotojasid_Sukurejas`) " +
                    "VALUES(?pavadinimas,?naujienos_tekstas,?sukurimo_data,?fk_Vartotojasid_Sukurejas)";
                MySqlCommand mySqlCommand = new MySqlCommand(sqlquery, mySqlConnection);
                mySqlCommand.Parameters.Add("?pavadinimas", MySqlDbType.VarChar).Value = naujiena.pavadinimas;
                mySqlCommand.Parameters.Add("?naujienos_tekstas", MySqlDbType.VarChar).Value = naujiena.naujienos_tekstas;
                mySqlCommand.Parameters.Add("?sukurimo_data", MySqlDbType.DateTime).Value = naujiena.naujienos_sukurimo_data;
                mySqlCommand.Parameters.Add("?fk_Vartotojasid_Sukurejas", MySqlDbType.Int32).Value = naujiena.naujienos_kurejo_ID;
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

        public List<Naujiena> Gauti_Naujienas()
        {
            List<Naujiena> naujienos = new List<Naujiena>();
            string conn = ConfigurationManager.ConnectionStrings["MysqlConnection"].ConnectionString;
            MySqlConnection mySqlConnection = new MySqlConnection(conn);
            string sqlquery = "select * from " + "Naujiena";
            MySqlCommand mySqlCommand = new MySqlCommand(sqlquery, mySqlConnection);
            mySqlConnection.Open();
            MySqlDataAdapter mda = new MySqlDataAdapter(mySqlCommand);
            DataTable dt = new DataTable();
            mda.Fill(dt);
            mySqlConnection.Close();

            foreach (DataRow item in dt.Rows)
            {
                naujienos.Add(new Naujiena
                {
                    id_Naujiena = Convert.ToInt32(item["id_Naujiena"]),
                    pavadinimas = Convert.ToString(item["pavadinimas"]),
                    naujienos_tekstas = Convert.ToString(item["naujienos_tekstas"]),
                    naujienos_sukurimo_data = Convert.ToDateTime(item["sukurimo_data"]),
                    naujienos_kurejo_ID = Convert.ToInt32(item["fk_Vartotojasid_Sukurejas"]),
                }); ;
            }
            return naujienos;
        }

        public bool Trinti_Naujiena(int id)
        {
            string conn = ConfigurationManager.ConnectionStrings["MysqlConnection"].ConnectionString;
            MySqlConnection mySqlConnection = new MySqlConnection(conn);
            string sqlquery = @"DELETE FROM `Naujiena` where id_Naujiena=?id";
            MySqlCommand mySqlCommand = new MySqlCommand(sqlquery, mySqlConnection);
            mySqlCommand.Parameters.Add("?id", MySqlDbType.Int32).Value = Convert.ToInt32(id);
            mySqlConnection.Open();
            mySqlCommand.ExecuteNonQuery();
            mySqlConnection.Close();
            return true;
        }

        public Naujiena Gauti_Naujiena(int id)
        {
            Naujiena naujiena = new Naujiena();
            string conn = ConfigurationManager.ConnectionStrings["MysqlConnection"].ConnectionString;
            MySqlConnection mySqlConnection = new MySqlConnection(conn);
            string sqlquery = "select * from `Naujiena` where id_Naujiena=" + id;
            MySqlCommand mySqlCommand = new MySqlCommand(sqlquery, mySqlConnection);
            mySqlConnection.Open();
            MySqlDataAdapter mda = new MySqlDataAdapter(mySqlCommand);
            DataTable dt = new DataTable();
            mda.Fill(dt);
            mySqlConnection.Close();

            foreach (DataRow item in dt.Rows)
            {
                {
                    naujiena.id_Naujiena = Convert.ToInt32(item["id_Naujiena"]);
                    naujiena.pavadinimas = Convert.ToString(item["pavadinimas"]);
                    naujiena.naujienos_tekstas = Convert.ToString(item["naujienos_tekstas"]);
                    naujiena.naujienos_sukurimo_data = Convert.ToDateTime(item["sukurimo_data"]);
                    naujiena.naujienos_kurejo_ID = Convert.ToInt32(item["fk_Vartotojasid_Sukurejas"]);
                }
            }
            return naujiena;
        }

        public bool Redaguoti_Naujiena(Naujiena naujiena)
        {
            try
            {
                string conn = ConfigurationManager.ConnectionStrings["MysqlConnection"].ConnectionString;
                MySqlConnection mySqlConnection = new MySqlConnection(conn);
                string sqlquery = "UPDATE `Naujiena` SET `pavadinimas`=?pavadinimas,`naujienos_tekstas`=?naujienos_tekstas WHERE id_Naujiena="+naujiena.id_Naujiena;
                MySqlCommand mySqlCommand = new MySqlCommand(sqlquery, mySqlConnection);
                mySqlCommand.Parameters.Add("?pavadinimas", MySqlDbType.Text).Value = naujiena.pavadinimas;
                mySqlCommand.Parameters.Add("?naujienos_tekstas", MySqlDbType.Text).Value = naujiena.naujienos_tekstas;
                mySqlConnection.Open();
                mySqlCommand.ExecuteNonQuery();
                mySqlConnection.Close();
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return false;
            }
        }
    }
}