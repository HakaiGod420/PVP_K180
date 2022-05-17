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
    public class Projektas_Repos
    {
        public bool Sukurti_Projekta(Projektas projektas)
        {
            try
            {
                string conn = ConfigurationManager.ConnectionStrings["MysqlConnection"].ConnectionString;
                MySqlConnection mySqlConnection = new MySqlConnection(conn);
                string sqlquery = "INSERT INTO `Projektas`(`pavadinimas`,`aprasymas`, `busena`, `fk_Vartotojasid_Vartotojas`) " +
                    "VALUES(?pavadinimas,?aprasymas,?busena,?fk_Vartotojasid_Vartotojas)";
                MySqlCommand mySqlCommand = new MySqlCommand(sqlquery, mySqlConnection);
                mySqlCommand.Parameters.Add("?pavadinimas", MySqlDbType.VarChar).Value = projektas.pavadinimas;
                mySqlCommand.Parameters.Add("?aprasymas", MySqlDbType.VarChar).Value = projektas.aprasymas;
                mySqlCommand.Parameters.Add("?busena", MySqlDbType.Int32).Value = projektas.projekto_busena;
                mySqlCommand.Parameters.Add("?fk_Vartotojasid_Vartotojas", MySqlDbType.Int32).Value = projektas.fk_Vartotojasid_Vartotojas;
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

        public bool Redaguoti_Projekta(Projektas projektas)
        {
            try
            {
                string conn = ConfigurationManager.ConnectionStrings["MysqlConnection"].ConnectionString;
                MySqlConnection mySqlConnection = new MySqlConnection(conn);
                string sqlquery = "UPDATE `Projektas` SET `pavadinimas`=?pavadinimas,`aprasymas`=?aprasymas,`busena`=?busena WHERE id_Projektas=" + projektas.id_Projektas;
                MySqlCommand mySqlCommand = new MySqlCommand(sqlquery, mySqlConnection);
                mySqlCommand.Parameters.Add("?pavadinimas", MySqlDbType.VarChar).Value = projektas.pavadinimas;
                mySqlCommand.Parameters.Add("?aprasymas", MySqlDbType.String).Value = projektas.aprasymas;
                mySqlCommand.Parameters.Add("?busena", MySqlDbType.Int32).Value = projektas.projekto_busena;
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

        public Projektas Gauti_Projekta(int id)
        {
            Projektas projektas = new Projektas();
            string conn = ConfigurationManager.ConnectionStrings["MysqlConnection"].ConnectionString;
            MySqlConnection mySqlConnection = new MySqlConnection(conn);
            string sqlquery = "select * from `Projektas` where id_Projektas=" + id;
            MySqlCommand mySqlCommand = new MySqlCommand(sqlquery, mySqlConnection);
            mySqlConnection.Open();
            MySqlDataAdapter mda = new MySqlDataAdapter(mySqlCommand);
            DataTable dt = new DataTable();
            mda.Fill(dt);
            mySqlConnection.Close();

            foreach (DataRow item in dt.Rows)
            {
                {
                    projektas.id_Projektas = Convert.ToInt32(item["id_Projektas"]);
                    projektas.pavadinimas = Convert.ToString(item["pavadinimas"]);
                    projektas.aprasymas = Convert.ToString(item["aprasymas"]);
                    projektas.projekto_busena = Convert.ToInt32(item["busena"]);
                    projektas.fk_Vartotojasid_Vartotojas = Convert.ToInt32(item["fk_Vartotojasid_Vartotojas"]);
                }
            }
            return projektas;
        }

        public List<Projektas> Gauti_Projektus()
        {
            List<Projektas> projektas = new List<Projektas>();
            string conn = ConfigurationManager.ConnectionStrings["MysqlConnection"].ConnectionString;
            MySqlConnection mySqlConnection = new MySqlConnection(conn);
            string sqlquery = "select * from " + "Projektas";
            MySqlCommand mySqlCommand = new MySqlCommand(sqlquery, mySqlConnection);
            mySqlConnection.Open();
            MySqlDataAdapter mda = new MySqlDataAdapter(mySqlCommand);
            DataTable dt = new DataTable();
            mda.Fill(dt);
            mySqlConnection.Close();

            foreach (DataRow item in dt.Rows)
            {
                projektas.Add(new Projektas
                {
                    id_Projektas = Convert.ToInt32(item["id_Projektas"]),
                    pavadinimas = Convert.ToString(item["pavadinimas"]),
                    aprasymas = Convert.ToString(item["aprasymas"]),
                    projekto_busena = Convert.ToInt32(item["busena"]),
                    fk_Vartotojasid_Vartotojas = Convert.ToInt32(item["fk_Vartotojasid_Vartotojas"]),
                }); ;
            }
            return projektas;
        }

        public bool Trinti_Projekta(int id)
        {
            string conn = ConfigurationManager.ConnectionStrings["MysqlConnection"].ConnectionString;
            MySqlConnection mySqlConnection = new MySqlConnection(conn);
            string sqlquery = @"DELETE FROM `Projektas` where id_Projektas=?id";
            MySqlCommand mySqlCommand = new MySqlCommand(sqlquery, mySqlConnection);
            mySqlCommand.Parameters.Add("?id", MySqlDbType.Int32).Value = Convert.ToInt32(id);
            mySqlConnection.Open();
            mySqlCommand.ExecuteNonQuery();
            mySqlConnection.Close();
            return true;
        }

        public bool Rasyti_Komentara(Komentaras komentaras)
        {
            try
            {
                    string conn = ConfigurationManager.ConnectionStrings["MysqlConnection"].ConnectionString;
                MySqlConnection mySqlConnection = new MySqlConnection(conn);
                string sqlquery = "INSERT INTO `Komentaras`(`komentaro_tekstas`, `parasymo_data`, `fk_Projektasid_Projektas`, `fk_Vartotojasid_Vartotojas`) " +
                    "VALUES (?komentaro_tekstas,?parasymo_data,?fk_Projektasid_Projektas,?fk_Vartotojasid_Vartotojas)";
                MySqlCommand mySqlCommand = new MySqlCommand(sqlquery, mySqlConnection);
                mySqlCommand.Parameters.Add("?komentaro_tekstas", MySqlDbType.VarChar).Value = komentaras.komentaro_tekstas;
                mySqlCommand.Parameters.Add("?parasymo_data", MySqlDbType.DateTime).Value = komentaras.parasymo_data;
                mySqlCommand.Parameters.Add("?fk_Projektasid_Projektas", MySqlDbType.Int32).Value = komentaras.priskirtas_id;
                mySqlCommand.Parameters.Add("?fk_Vartotojasid_Vartotojas", MySqlDbType.Int32).Value = komentaras.fk_Vartotojasid_Vartotojas;
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

        public List<Komentaras> Gauti_Projekto_Komentarus(int id)
        {
            List<Komentaras> komentarai = new List<Komentaras>();
            string conn = ConfigurationManager.ConnectionStrings["MysqlConnection"].ConnectionString;
            MySqlConnection mySqlConnection = new MySqlConnection(conn);
            string sqlquery = "SELECT id_Komentaras,komentaro_tekstas,parasymo_data,fk_Renginysid_Renginys," +
                "fk_Projektasid_Projektas,fk_Vartotojasid_Vartotojas,Vartotojas.slapyvardis,Vartotojas.nuotrauka FROM `Komentaras`" +
                "LEFT JOIN Vartotojas ON Vartotojas.id_Vartotojas = fk_Vartotojasid_Vartotojas WHERE fk_Projektasid_Projektas=" + id;
            MySqlCommand mySqlCommand = new MySqlCommand(sqlquery, mySqlConnection);
            mySqlConnection.Open();
            MySqlDataAdapter mda = new MySqlDataAdapter(mySqlCommand);
            DataTable dt = new DataTable();
            mda.Fill(dt);
            mySqlConnection.Close();

            foreach (DataRow item in dt.Rows)
            {
                komentarai.Add(new Komentaras
                {
                    id_Komentaras = Convert.ToInt32(item["id_Komentaras"]),
                    komentaro_tekstas = Convert.ToString(item["komentaro_tekstas"]),
                    parasymo_data = Convert.ToDateTime(item["parasymo_data"]),
                    priskirtas_id = Convert.ToInt32(item["fk_Projektasid_Projektas"]),
                    fk_Vartotojasid_Vartotojas = Convert.ToInt32(item["fk_Vartotojasid_Vartotojas"]),
                    varotojo_slapyvardis = Convert.ToString(item["slapyvardis"]),
                    nuotrauka_location = Convert.ToString(item["nuotrauka"])
                }); ;
            }
            return komentarai;
        }

        public bool PatikrintiArGalimaTrinti(int userID, int commentID)
        {
            try
            {
                string conn = ConfigurationManager.ConnectionStrings["MysqlConnection"].ConnectionString;
                MySqlConnection mySqlConnection = new MySqlConnection(conn);
                string sqlquery = "SELECT * FROM `Komentaras` WHERE id_Komentaras=?commentID and fk_Vartotojasid_Vartotojas=?userID";
                MySqlCommand mySqlCommand = new MySqlCommand(sqlquery, mySqlConnection);
                mySqlCommand.Parameters.Add("?commentID", MySqlDbType.Int32).Value = commentID;
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

        public bool Trinti_Komentara(int id)
        {
            string conn = ConfigurationManager.ConnectionStrings["MysqlConnection"].ConnectionString;
            MySqlConnection mySqlConnection = new MySqlConnection(conn);
            string sqlquery = @"DELETE FROM `Komentaras` WHERE id_Komentaras=?id";
            MySqlCommand mySqlCommand = new MySqlCommand(sqlquery, mySqlConnection);
            mySqlCommand.Parameters.Add("?id", MySqlDbType.Int32).Value = Convert.ToInt32(id);
            mySqlConnection.Open();
            mySqlCommand.ExecuteNonQuery();
            mySqlConnection.Close();
            return true;
        }


        public List<ProjektuPerziura> Gauti_Projektus_Atvaizdavimui()
        {
            List<ProjektuPerziura> projektas = new List<ProjektuPerziura>();
            string conn = ConfigurationManager.ConnectionStrings["MysqlConnection"].ConnectionString;
            MySqlConnection mySqlConnection = new MySqlConnection(conn);
            string sqlquery = "SELECT id_Projektas, pavadinimas,aprasymas,sukurimo_data,ivertinimas,Projekto_busena.name as 'busena',COUNT(Komentaras.id_Komentaras) " +
                "as 'komentarai' FROM `Projektas` " +
                "LEFT JOIN Projekto_busena ON Projekto_busena.id_Projekto_busena = busena " +
                "LEFT JOIN Komentaras ON Komentaras.fk_Projektasid_Projektas = Projektas.id_Projektas GROUP BY Projektas.id_Projektas";
            MySqlCommand mySqlCommand = new MySqlCommand(sqlquery, mySqlConnection);
            mySqlConnection.Open();
            MySqlDataAdapter mda = new MySqlDataAdapter(mySqlCommand);
            DataTable dt = new DataTable();
            mda.Fill(dt);
            mySqlConnection.Close();

            foreach (DataRow item in dt.Rows)
            {
                projektas.Add(new ProjektuPerziura
                {
                    id_Projektas = Convert.ToInt32(item["id_Projektas"]),
                    pavadinimas = Convert.ToString(item["pavadinimas"]),
                    aprasymas = Convert.ToString(item["aprasymas"]),
                    busena = Convert.ToString(item["busena"]),
                    komenetarai = Convert.ToInt32(item["komentarai"]),
                    sukurimo_data = Convert.ToDateTime(item["sukurimo_data"]),
                }); ;
            }
            return projektas;
        }

        public int Gauti_Paskutini_Prideto_Index()
        {
            try
            {
                int index = -1;
                string conn = ConfigurationManager.ConnectionStrings["MysqlConnection"].ConnectionString;
                MySqlConnection mySqlConnection = new MySqlConnection(conn);
                string sqlquery = "SELECT MAX(id_Projektas) as 'max_id' FROM `Projektas`";
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
    }
}