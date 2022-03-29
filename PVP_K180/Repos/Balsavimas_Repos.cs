using MySql.Data.MySqlClient;
using PVP_K180.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;

namespace PVP_K180.Repos
{
    public class Balsavimas_Repos
    {
        public bool Sukurti_Balsavima(Balsavimas balsavimas)
        {
            try
            {
                string conn = ConfigurationManager.ConnectionStrings["MysqlConnection"].ConnectionString;
                MySqlConnection mySqlConnection = new MySqlConnection(conn);
                string sqlquery = "INSERT INTO `Balsavimas`(`balsavimo_aprasymas`, `klausimas`, `sukurimo_data`,`pabaigos_data`, `busena`," +
                    " `fk_Vartotojasid_Sukurejas`) VALUES (?balsavimo_aprasymas,?klausimas,?sukurimo_Data,?pabaigos_data,?busena,?fk_Vartotojasid_Sukurejas)";
                MySqlCommand mySqlCommand = new MySqlCommand(sqlquery, mySqlConnection);
                mySqlCommand.Parameters.Add("?balsavimo_aprasymas", MySqlDbType.VarChar).Value = balsavimas.balsavimo_aprasymas;
                mySqlCommand.Parameters.Add("?klausimas", MySqlDbType.VarChar).Value = balsavimas.klausimas;
                mySqlCommand.Parameters.Add("?sukurimo_Data", MySqlDbType.DateTime).Value = balsavimas.sukurimo_data;
                mySqlCommand.Parameters.Add("?pabaigos_data", MySqlDbType.DateTime).Value = balsavimas.pabaigos_data;
                mySqlCommand.Parameters.Add("?busena", MySqlDbType.Int32).Value = balsavimas.busena;
                mySqlCommand.Parameters.Add("?fk_Vartotojasid_Sukurejas", MySqlDbType.Int32).Value = balsavimas.fk_Vartotojasid_Sukurejas;
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

        public int Gauti_Paskutini_Prideto_Index()
        {
            try
            {
                int index = -1;
                string conn = ConfigurationManager.ConnectionStrings["MysqlConnection"].ConnectionString;
                MySqlConnection mySqlConnection = new MySqlConnection(conn);
                string sqlquery = "SELECT MAX(id_Balsavimas) as 'max_id' FROM `Balsavimas`";
                MySqlCommand mySqlCommand = new MySqlCommand(sqlquery, mySqlConnection);
                mySqlConnection.Open();
                MySqlDataAdapter mda = new MySqlDataAdapter(mySqlCommand);
                DataTable dt = new DataTable();
                mda.Fill(dt);
                mySqlConnection.Close();

                foreach (DataRow item in dt.Rows)
                {
                    index = Convert.ToInt32(item["max_id"]);
                }
                return index;
            }
            catch(Exception ex)
            {
                return -1;
            }
        }

        public bool Prideti_Varianta(Balsavimo_Variantas balsavimo_Variantas)
        {
            try
            {
                string conn = ConfigurationManager.ConnectionStrings["MysqlConnection"].ConnectionString;
                MySqlConnection mySqlConnection = new MySqlConnection(conn);
                string sqlquery = "INSERT INTO `Balsavimo_Variantas`(`balsavimo_variantas`, `fk_Balsavimasid_Balsavimas`) " +
                    "VALUES (?balsavimo_variantas,?fk_Balsavimasid_Balsavimas)";
                MySqlCommand mySqlCommand = new MySqlCommand(sqlquery, mySqlConnection);
                mySqlCommand.Parameters.Add("?balsavimo_variantas", MySqlDbType.VarChar).Value = balsavimo_Variantas.balsavimo_variantas;
                mySqlCommand.Parameters.Add("?fk_Balsavimasid_Balsavimas", MySqlDbType.Int32).Value = balsavimo_Variantas.fk_Balsavimasid_Balsavimas;
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

        public List<Balsavimas> GautiBalsavimus()
        {
            try
            {
                List<Balsavimas> balsavimai = new List<Balsavimas>();
                string conn = ConfigurationManager.ConnectionStrings["MysqlConnection"].ConnectionString;
                MySqlConnection mySqlConnection = new MySqlConnection(conn);
                string sqlquery = "SELECT id_Balsavimas,balsavimo_aprasymas,klausimas,dalyviu_skaicius,sukurimo_data,pabaigos_data,Balsavimo_Busena.name as 'busena',Vartotojas.slapyvardis as 'kurejas' FROM `Balsavimas`" +
                    "LEFT JOIN Balsavimo_Busena on Balsavimas.busena = Balsavimo_Busena.id_Balsavimo_Busena " +
                    "LEFT JOIN Vartotojas ON Vartotojas.id_Vartotojas = Balsavimas.fk_Vartotojasid_Sukurejas";
                MySqlCommand mySqlCommand = new MySqlCommand(sqlquery, mySqlConnection);
                mySqlConnection.Open();
                MySqlDataAdapter mda = new MySqlDataAdapter(mySqlCommand);
                DataTable dt = new DataTable();
                mda.Fill(dt);
                mySqlConnection.Close();

                foreach (DataRow item in dt.Rows)
                {
                    balsavimai.Add(new Balsavimas
                    {
                        
                        id_Balsavimas = Convert.ToInt32(item["id_Balsavimas"]),
                        balsavimo_aprasymas = Convert.ToString(item["balsavimo_aprasymas"]),
                        klausimas = Convert.ToString(item["klausimas"]),
                        dalyviu_skaicius = Convert.ToInt32(item["dalyviu_skaicius"]),
                        sukurimo_data = Convert.ToDateTime(item["sukurimo_data"]),
                        pabaigos_data = CheckIfDateNull(item),
                        busenos_pavadinimas = Convert.ToString(item["busena"]),
                        Sukurejas = Convert.ToString(item["kurejas"])
                    }); ;
                }
                return balsavimai;
            }

            catch(Exception ex)
            {
                return null;
            }
        }

        private DateTime? CheckIfDateNull(DataRow data)
        {
            if(data["pabaigos_data"] == DBNull.Value)
            {
                return null;
            }
            else
            {
                return Convert.ToDateTime(data["sukurimo_data"]);
            }
        }

        public bool Trinti_Balsavima(int id)
        {
            string conn = ConfigurationManager.ConnectionStrings["MysqlConnection"].ConnectionString;
            MySqlConnection mySqlConnection = new MySqlConnection(conn);
            string sqlquery = "DELETE FROM `Balsavimas` WHERE id_Balsavimas=?id";
            MySqlCommand mySqlCommand = new MySqlCommand(sqlquery, mySqlConnection);
            mySqlCommand.Parameters.Add("?id", MySqlDbType.Int32).Value = Convert.ToInt32(id);
            mySqlConnection.Open();
            mySqlCommand.ExecuteNonQuery();
            mySqlConnection.Close();
            return true;
        }
    }
}