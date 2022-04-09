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

        public Balsavimas Gauti_Balsavima(int id)
        {
            Balsavimas balsavimas = new Balsavimas();
            string conn = ConfigurationManager.ConnectionStrings["MysqlConnection"].ConnectionString;
            MySqlConnection mySqlConnection = new MySqlConnection(conn);
            string sqlquery = "select * from `Balsavimas` where id_Balsavimas=" + id;
            MySqlCommand mySqlCommand = new MySqlCommand(sqlquery, mySqlConnection);
            mySqlConnection.Open();
            MySqlDataAdapter mda = new MySqlDataAdapter(mySqlCommand);
            DataTable dt = new DataTable();
            mda.Fill(dt);
            mySqlConnection.Close();

            foreach (DataRow item in dt.Rows)
            {
                {
                    balsavimas.id_Balsavimas = Convert.ToInt32(item["id_Balsavimas"]);
                    balsavimas.balsavimo_aprasymas = Convert.ToString(item["balsavimo_aprasymas"]);
                    balsavimas.klausimas = Convert.ToString(item["klausimas"]);
                    balsavimas.dalyviu_skaicius = Convert.ToInt32(item["dalyviu_skaicius"]);
                    balsavimas.sukurimo_data = Convert.ToDateTime(item["sukurimo_data"]);
                    balsavimas.pabaigos_data = CheckIfDateNull(item);
                    balsavimas.busena = Convert.ToInt32(item["busena"]);
                    balsavimas.fk_Vartotojasid_Sukurejas = Convert.ToInt32(item["fk_Vartotojasid_Sukurejas"]); ;
                }
            }
            return balsavimas;
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


        public List<Balsavimo_Variantas> GautiVariantus(int id)
        {
            try
            {
                List<Balsavimo_Variantas> balsavimo_Variantas = new List<Balsavimo_Variantas>();
                string conn = ConfigurationManager.ConnectionStrings["MysqlConnection"].ConnectionString;
                MySqlConnection mySqlConnection = new MySqlConnection(conn);
                string sqlquery = "SELECT * FROM `Balsavimo_Variantas` WHERE fk_Balsavimasid_Balsavimas ="+id;
                MySqlCommand mySqlCommand = new MySqlCommand(sqlquery, mySqlConnection);
                mySqlConnection.Open();
                MySqlDataAdapter mda = new MySqlDataAdapter(mySqlCommand);
                DataTable dt = new DataTable();
                mda.Fill(dt);
                mySqlConnection.Close();

                foreach (DataRow item in dt.Rows)
                {
                    balsavimo_Variantas.Add(new Balsavimo_Variantas
                    {

                        id_Balsavimo_Variantas = Convert.ToInt32(item["id_Balsavimo_Variantas"]),
                        balsavimo_variantas = Convert.ToString(item["balsavimo_variantas"]),
                        pasirinkusiu_skaicius = Convert.ToInt32(item["pasirinkusiu_skaicius"]),
                        fk_Balsavimasid_Balsavimas = Convert.ToInt32(item["fk_Balsavimasid_Balsavimas"]),
                    }); ;
                }
                return balsavimo_Variantas;
            }

            catch (Exception ex)
            {
                return null;
            }
        }

        public bool Atnaujinti_Balsavima(Balsavimas balsavimas)
        {
            try
            {
                string conn = ConfigurationManager.ConnectionStrings["MysqlConnection"].ConnectionString;
                MySqlConnection mySqlConnection = new MySqlConnection(conn);
                string sqlquery = "UPDATE `Balsavimas` SET `balsavimo_aprasymas`=?balsavimo_aprasymas,`klausimas`=?klausimas," +
                    "`sukurimo_data`=?sukurimo_data,`pabaigos_data`=?pabaigos_data WHERE id_Balsavimas=?id_Balsavimas";
                MySqlCommand mySqlCommand = new MySqlCommand(sqlquery, mySqlConnection);
                mySqlCommand.Parameters.Add("?id_Balsavimas", MySqlDbType.Int32).Value = balsavimas.id_Balsavimas;
                mySqlCommand.Parameters.Add("?balsavimo_aprasymas", MySqlDbType.VarChar).Value = balsavimas.balsavimo_aprasymas;
                mySqlCommand.Parameters.Add("?klausimas", MySqlDbType.VarChar).Value = balsavimas.klausimas;
                mySqlCommand.Parameters.Add("?sukurimo_data", MySqlDbType.DateTime).Value = balsavimas.sukurimo_data;
                mySqlCommand.Parameters.Add("?pabaigos_data", MySqlDbType.DateTime).Value = balsavimas.pabaigos_data;
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

        public bool Trinti_Variantus(int id)
        {
            string conn = ConfigurationManager.ConnectionStrings["MysqlConnection"].ConnectionString;
            MySqlConnection mySqlConnection = new MySqlConnection(conn);
            string sqlquery = "DELETE FROM `Balsavimo_Variantas` WHERE id_Balsavimo_Variantas=?id";
            MySqlCommand mySqlCommand = new MySqlCommand(sqlquery, mySqlConnection);
            mySqlCommand.Parameters.Add("?id", MySqlDbType.Int32).Value = Convert.ToInt32(id);
            mySqlConnection.Open();
            mySqlCommand.ExecuteNonQuery();
            mySqlConnection.Close();
            return true;
        }

        public List<Balsavimo_Busena> GautiBusenas()
        {
            try
            {
                List<Balsavimo_Busena> balsavimo_Busenas = new List<Balsavimo_Busena>();
                string conn = ConfigurationManager.ConnectionStrings["MysqlConnection"].ConnectionString;
                MySqlConnection mySqlConnection = new MySqlConnection(conn);
                string sqlquery = "SELECT * FROM `Balsavimo_Busena` WHERE 1";
                MySqlCommand mySqlCommand = new MySqlCommand(sqlquery, mySqlConnection);
                mySqlConnection.Open();
                MySqlDataAdapter mda = new MySqlDataAdapter(mySqlCommand);
                DataTable dt = new DataTable();
                mda.Fill(dt);
                mySqlConnection.Close();

                foreach (DataRow item in dt.Rows)
                {
                    balsavimo_Busenas.Add(new Balsavimo_Busena
                    {

                        id_Balsavimo_busena = Convert.ToInt32(item["id_Balsavimo_Busena"]),
                        name = Convert.ToString(item["name"])
                    }); ;
                }
                return balsavimo_Busenas;
            }

            catch (Exception ex)
            {
                return null;
            }
        }

        public bool Atnaujinti_Balsavimo_Busena(int balsavimo_id, int busenos_id)
        {
            try
            {
                string conn = ConfigurationManager.ConnectionStrings["MysqlConnection"].ConnectionString;
                MySqlConnection mySqlConnection = new MySqlConnection(conn);
                string sqlquery = "UPDATE `Balsavimas` SET `busena`=?busena WHERE id_Balsavimas="+balsavimo_id;
                MySqlCommand mySqlCommand = new MySqlCommand(sqlquery, mySqlConnection);
                mySqlCommand.Parameters.Add("?busena", MySqlDbType.Int32).Value = busenos_id;
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

        public int GautiAktyviuSkaiciu()
        {
            int count = -1;
            try
            {
                List<Balsavimo_Busena> balsavimo_Busenas = new List<Balsavimo_Busena>();
                string conn = ConfigurationManager.ConnectionStrings["MysqlConnection"].ConnectionString;
                MySqlConnection mySqlConnection = new MySqlConnection(conn);
                string sqlquery = "SELECT COUNT(id_Balsavimas) as 'count' FROM `Balsavimas` WHERE busena=2";
                MySqlCommand mySqlCommand = new MySqlCommand(sqlquery, mySqlConnection);
                mySqlConnection.Open();
                MySqlDataAdapter mda = new MySqlDataAdapter(mySqlCommand);
                DataTable dt = new DataTable();
                mda.Fill(dt);
                mySqlConnection.Close();

                foreach (DataRow item in dt.Rows)
                {
                    count = Convert.ToInt32(item["count"]);
                }
                return count;
            }

            catch (Exception ex)
            {
                return -1;
            }
        }

        public int Gauti_Aktyvu_Balsavima()
        {
            int activeNum = -1;
            string conn = ConfigurationManager.ConnectionStrings["MysqlConnection"].ConnectionString;
            MySqlConnection mySqlConnection = new MySqlConnection(conn);
            string sqlquery = "select * from `Balsavimas` where busena=" + 2;
            MySqlCommand mySqlCommand = new MySqlCommand(sqlquery, mySqlConnection);
            mySqlConnection.Open();
            MySqlDataAdapter mda = new MySqlDataAdapter(mySqlCommand);
            DataTable dt = new DataTable();
            mda.Fill(dt);
            mySqlConnection.Close();

            foreach (DataRow item in dt.Rows)
            {
                {
                    activeNum = Convert.ToInt32(item["id_Balsavimas"]);
                }
            }
            return activeNum;
        }

        public Balsavimo_Variantas GautiVarianta(int id)
        {
            try
            {
                Balsavimo_Variantas balsavimo_Variantas = new Balsavimo_Variantas();
                string conn = ConfigurationManager.ConnectionStrings["MysqlConnection"].ConnectionString;
                MySqlConnection mySqlConnection = new MySqlConnection(conn);
                string sqlquery = "SELECT * FROM `Balsavimo_Variantas` WHERE id_Balsavimo_Variantas =" + id;
                MySqlCommand mySqlCommand = new MySqlCommand(sqlquery, mySqlConnection);
                mySqlConnection.Open();
                MySqlDataAdapter mda = new MySqlDataAdapter(mySqlCommand);
                DataTable dt = new DataTable();
                mda.Fill(dt);
                mySqlConnection.Close();

                foreach (DataRow item in dt.Rows)
                {

                    balsavimo_Variantas.id_Balsavimo_Variantas = Convert.ToInt32(item["id_Balsavimo_Variantas"]);
                    balsavimo_Variantas.balsavimo_variantas = Convert.ToString(item["balsavimo_variantas"]);
                    balsavimo_Variantas.pasirinkusiu_skaicius = Convert.ToInt32(item["pasirinkusiu_skaicius"]);
                    balsavimo_Variantas.fk_Balsavimasid_Balsavimas = Convert.ToInt32(item["fk_Balsavimasid_Balsavimas"]);
                }
                return balsavimo_Variantas;
            }

            catch (Exception ex)
            {
                return null;
            }
        }

        public bool Prideti_Pasirinkta_Varianta(Varianto_Pasirinkimas varianto_Pasirinkimas)
        {
            try
            {
                string conn = ConfigurationManager.ConnectionStrings["MysqlConnection"].ConnectionString;
                MySqlConnection mySqlConnection = new MySqlConnection(conn);
                string sqlquery = "INSERT INTO `Varianto_Pasirinkimas`(`fk_Vartotojasid_Vartotojas`, `fk_Balsavimo_Variantasid_Balsavimo_Variantas`, `fk_Balsavimo_Id`) VALUES (?fk_Vartotojasid_Vartotojas,?fk_Balsavimo_Variantasid_Balsavimo_Variantas,?fk_Balsavimo_Id)";
                MySqlCommand mySqlCommand = new MySqlCommand(sqlquery, mySqlConnection);
                mySqlCommand.Parameters.Add("?fk_Vartotojasid_Vartotojas", MySqlDbType.Int32).Value = varianto_Pasirinkimas.fk_Vartotojasid_Vartotojas;
                mySqlCommand.Parameters.Add("?fk_Balsavimo_Variantasid_Balsavimo_Variantas", MySqlDbType.Int32).Value = varianto_Pasirinkimas.fk_Balsavimo_Variantasid_Balsavimo_Variantas;
                mySqlCommand.Parameters.Add("?fk_Balsavimo_Id", MySqlDbType.Int32).Value = varianto_Pasirinkimas.fk_Balsavimo_Id;
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

        public bool AtnaujintiBalsavimoSkaiciu(int id)
        {
            try
            {
                string conn = ConfigurationManager.ConnectionStrings["MysqlConnection"].ConnectionString;
                MySqlConnection mySqlConnection = new MySqlConnection(conn);
                string sqlquery = "UPDATE Balsavimas SET dalyviu_skaicius = dalyviu_skaicius + 1 WHERE id_Balsavimas ="+id;
                MySqlCommand mySqlCommand = new MySqlCommand(sqlquery, mySqlConnection);
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

        public bool AtnaujintiVariantoSkaiciu(int id)
        {
            try
            {
                string conn = ConfigurationManager.ConnectionStrings["MysqlConnection"].ConnectionString;
                MySqlConnection mySqlConnection = new MySqlConnection(conn);
                string sqlquery = "UPDATE Balsavimo_Variantas SET pasirinkusiu_skaicius = pasirinkusiu_skaicius + 1 WHERE id_Balsavimo_Variantas =" + id;
                MySqlCommand mySqlCommand = new MySqlCommand(sqlquery, mySqlConnection);
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

        public bool PatikrintiPasirinkima(int activeVote, int userID)
        {
            try
            {
                string conn = ConfigurationManager.ConnectionStrings["MysqlConnection"].ConnectionString;
                MySqlConnection mySqlConnection = new MySqlConnection(conn);
                string sqlquery = "SELECT * FROM `Varianto_Pasirinkimas` WHERE fk_Vartotojasid_Vartotojas=?userID AND fk_Balsavimo_Id = ?activeVote";
                MySqlCommand mySqlCommand = new MySqlCommand(sqlquery, mySqlConnection);
                mySqlCommand.Parameters.Add("?activeVote", MySqlDbType.Int32).Value = activeVote;
                mySqlCommand.Parameters.Add("?userID", MySqlDbType.Int32).Value = userID;
                mySqlConnection.Open();
                MySqlDataAdapter mda = new MySqlDataAdapter(mySqlCommand);
                DataTable dt = new DataTable();
                mda.Fill(dt);
                mySqlConnection.Close();

                if (dt.Rows.Count == 1)
                {
                    return true;
                }
                else return false;
            }
            catch (Exception ex)
            {
                return false;
            }

        }

        public int Gauti_Pasirinkta_Varianta(int userID)
        {
            int activeNum = -1;
            string conn = ConfigurationManager.ConnectionStrings["MysqlConnection"].ConnectionString;
            MySqlConnection mySqlConnection = new MySqlConnection(conn);
            string sqlquery = "select * from `Varianto_Pasirinkimas` where fk_Vartotojasid_Vartotojas=" + userID;
            MySqlCommand mySqlCommand = new MySqlCommand(sqlquery, mySqlConnection);
            mySqlConnection.Open();
            MySqlDataAdapter mda = new MySqlDataAdapter(mySqlCommand);
            DataTable dt = new DataTable();
            mda.Fill(dt);
            mySqlConnection.Close();

            foreach (DataRow item in dt.Rows)
            {
                {
                    activeNum = Convert.ToInt32(item["fk_Balsavimo_Variantasid_Balsavimo_Variantas"]);
                }
            }
            return activeNum;
        }


    }
}