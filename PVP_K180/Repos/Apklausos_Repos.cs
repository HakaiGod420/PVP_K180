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
    public class Apklausos_Repos
    {

        public bool Sukurti_Apklausa(Apklausa apklausa)
        {
            try
            {
                string conn = ConfigurationManager.ConnectionStrings["MysqlConnection"].ConnectionString;
                MySqlConnection mySqlConnection = new MySqlConnection(conn);
                string sqlquery = "INSERT INTO `Apklausa`(`aprasymas`, `sukurimo_data`, `pabaigos_data`, `busena`, `fk_Vartotojasid_Sukurejas`) " +
                    "VALUES (?aprasymas,?sukurimo_data,?pabaigos_data,?busena,?fk_Vartotojasid_Sukurejas)";
                MySqlCommand mySqlCommand = new MySqlCommand(sqlquery, mySqlConnection);
                mySqlCommand.Parameters.Add("?aprasymas", MySqlDbType.VarChar).Value = apklausa.aprasymas;
                mySqlCommand.Parameters.Add("?sukurimo_data", MySqlDbType.DateTime).Value = apklausa.sukurimo_data;
                mySqlCommand.Parameters.Add("?pabaigos_data", MySqlDbType.DateTime).Value = apklausa.pabaigos_data;
                mySqlCommand.Parameters.Add("?busena", MySqlDbType.Int32).Value = apklausa.busena;
                mySqlCommand.Parameters.Add("?fk_Vartotojasid_Sukurejas", MySqlDbType.Int32).Value = apklausa.fk_Vartotojasid_Sukurejas;
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
                string sqlquery = "SELECT MAX(id_Apklausa) as 'max_id' FROM `Apklausa`";
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
            catch (Exception ex)
            {
                return -1;
            }
        }

        public bool Prideti_Klausima(Klausimas klausimas)
        {
            try
            {
                string conn = ConfigurationManager.ConnectionStrings["MysqlConnection"].ConnectionString;
                MySqlConnection mySqlConnection = new MySqlConnection(conn);
                string sqlquery = "INSERT INTO `Klausimas`(`klausimo_tekstas`, `fk_Apklausaid_Apklausa`) VALUES (?klausimo_tekstas,?fk_Apklausaid_Apklausa)";
                MySqlCommand mySqlCommand = new MySqlCommand(sqlquery, mySqlConnection);
                mySqlCommand.Parameters.Add("?klausimo_tekstas", MySqlDbType.VarChar).Value = klausimas.klausimo_tekstas;
                mySqlCommand.Parameters.Add("?fk_Apklausaid_Apklausa", MySqlDbType.Int32).Value = klausimas.fk_Apklausa_Apklausa;
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

        public List<Apklausa> GautiApklausas()
        {
            try
            {
                List<Apklausa> apklausos = new List<Apklausa>();
                string conn = ConfigurationManager.ConnectionStrings["MysqlConnection"].ConnectionString;
                MySqlConnection mySqlConnection = new MySqlConnection(conn);
                string sqlquery = "SELECT id_Apklausa,aprasymas,dalyviu_skaicius,sukurimo_data,pabaigos_data,Apklausos_Busena.name as 'busena',Vartotojas.slapyvardis as 'kurejas' FROM `Apklausa` " +
                    "LEFT JOIN Apklausos_Busena on Apklausa.busena = Apklausos_Busena.id_Apklausos_Busena " +
                    "LEFT JOIN Vartotojas ON Vartotojas.id_Vartotojas = Apklausa.fk_Vartotojasid_Sukurejas";
                MySqlCommand mySqlCommand = new MySqlCommand(sqlquery, mySqlConnection);
                mySqlConnection.Open();
                MySqlDataAdapter mda = new MySqlDataAdapter(mySqlCommand);
                DataTable dt = new DataTable();
                mda.Fill(dt);
                mySqlConnection.Close();

                foreach (DataRow item in dt.Rows)
                {
                    apklausos.Add(new Apklausa
                    {

                        id_Apklausa = Convert.ToInt32(item["id_Apklausa"]),
                        aprasymas = Convert.ToString(item["aprasymas"]),
                        dalyviu_skaicius = Convert.ToInt32(item["dalyviu_skaicius"]),
                        sukurimo_data = Convert.ToDateTime(item["sukurimo_data"]),
                        pabaigos_data = CheckIfDateNull(item),
                        busenos_pavadinimas = Convert.ToString(item["busena"]),
                        Sukurejas = Convert.ToString(item["kurejas"])
                    }); ;
                }
                return apklausos;
            }

            catch (Exception ex)
            {
                return null;
            }
        }

        private DateTime? CheckIfDateNull(DataRow data)
        {
            if (data["pabaigos_data"] == DBNull.Value)
            {
                return null;
            }
            else
            {
                return Convert.ToDateTime(data["sukurimo_data"]);
            }
        }

        public int Gauti_Aktyvia_Apklausa()
        {
            int activeNum = -1;
            string conn = ConfigurationManager.ConnectionStrings["MysqlConnection"].ConnectionString;
            MySqlConnection mySqlConnection = new MySqlConnection(conn);
            string sqlquery = "select * from `Apklausa` where busena=" + 2;
            MySqlCommand mySqlCommand = new MySqlCommand(sqlquery, mySqlConnection);
            mySqlConnection.Open();
            MySqlDataAdapter mda = new MySqlDataAdapter(mySqlCommand);
            DataTable dt = new DataTable();
            mda.Fill(dt);
            mySqlConnection.Close();

            foreach (DataRow item in dt.Rows)
            {
                {
                    activeNum = Convert.ToInt32(item["id_Apklausa"]);
                }
            }
            return activeNum;
        }

        public Apklausa Gauti_Apklausa(int id)
        {
            Apklausa apklausa = new Apklausa();
            string conn = ConfigurationManager.ConnectionStrings["MysqlConnection"].ConnectionString;
            MySqlConnection mySqlConnection = new MySqlConnection(conn);
            string sqlquery = "select * from `Apklausa` where id_Apklausa=" + id;
            MySqlCommand mySqlCommand = new MySqlCommand(sqlquery, mySqlConnection);
            mySqlConnection.Open();
            MySqlDataAdapter mda = new MySqlDataAdapter(mySqlCommand);
            DataTable dt = new DataTable();
            mda.Fill(dt);
            mySqlConnection.Close();

            foreach (DataRow item in dt.Rows)
            {
                {
                    apklausa.id_Apklausa = Convert.ToInt32(item["id_Apklausa"]);
                    apklausa.aprasymas = Convert.ToString(item["aprasymas"]);
                    apklausa.dalyviu_skaicius = Convert.ToInt32(item["dalyviu_skaicius"]);
                    apklausa.sukurimo_data = Convert.ToDateTime(item["sukurimo_data"]);
                    apklausa.pabaigos_data = CheckIfDateNull(item);
                    apklausa.busena = Convert.ToInt32(item["busena"]);
                    apklausa.fk_Vartotojasid_Sukurejas = Convert.ToInt32(item["fk_Vartotojasid_Sukurejas"]); ;
                }
            }
            return apklausa;
        }

        public List<Klausimas> GautiKlausimus(int id)
        {
            try
            {
                List<Klausimas> klausimai = new List<Klausimas>();
                string conn = ConfigurationManager.ConnectionStrings["MysqlConnection"].ConnectionString;
                MySqlConnection mySqlConnection = new MySqlConnection(conn);
                string sqlquery = "SELECT * FROM `Klausimas` WHERE fk_Apklausaid_Apklausa=" + id;
                MySqlCommand mySqlCommand = new MySqlCommand(sqlquery, mySqlConnection);
                mySqlConnection.Open();
                MySqlDataAdapter mda = new MySqlDataAdapter(mySqlCommand);
                DataTable dt = new DataTable();
                mda.Fill(dt);
                mySqlConnection.Close();

                foreach (DataRow item in dt.Rows)
                {
                    klausimai.Add(new Klausimas
                    {

                        id_Klausimas = Convert.ToInt32(item["id_Klausimas"]),
                        klausimo_tekstas = Convert.ToString(item["klausimo_tekstas"]),
                        fk_Apklausa_Apklausa = Convert.ToInt32(item["fk_Apklausaid_Apklausa"]),
                    }); ;
                }
                return klausimai;
            }

            catch (Exception ex)
            {
                return null;
            }
        }


    }
}