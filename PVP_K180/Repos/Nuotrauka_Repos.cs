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

        public bool Prideti_Renginio_Nuotraukas(Nuotrauka nuotrauka)
        {
            try
            {
                string conn = ConfigurationManager.ConnectionStrings["MysqlConnection"].ConnectionString;
                MySqlConnection mySqlConnection = new MySqlConnection(conn);
                string sqlquery = "INSERT INTO `Nuotrauka`(`nuotraukos_nuroda`,`fk_Renginysid_Renginys`) " +
                    "VALUES (?nuotraukos_nuoroda,?fk_Renginysid_Renginys)";
                MySqlCommand mySqlCommand = new MySqlCommand(sqlquery, mySqlConnection);
                mySqlCommand.Parameters.Add("?nuotraukos_nuoroda", MySqlDbType.VarChar).Value = nuotrauka.nuotraukos_nuoroda;
                mySqlCommand.Parameters.Add("?fk_Renginysid_Renginys", MySqlDbType.VarChar).Value = nuotrauka.priskirtas_id;
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

        public bool Prideti_Projekto_Nuotraukas(Nuotrauka nuotrauka)
        {
            try
            {
                string conn = ConfigurationManager.ConnectionStrings["MysqlConnection"].ConnectionString;
                MySqlConnection mySqlConnection = new MySqlConnection(conn);
                string sqlquery = "INSERT INTO `Nuotrauka`(`nuotraukos_nuroda`,`fk_Projektasid_Projektas`) " +
                    "VALUES (?nuotraukos_nuoroda,?fk_Projektasid_Projektas)";
                MySqlCommand mySqlCommand = new MySqlCommand(sqlquery, mySqlConnection);
                mySqlCommand.Parameters.Add("?nuotraukos_nuoroda", MySqlDbType.VarChar).Value = nuotrauka.nuotraukos_nuoroda;
                mySqlCommand.Parameters.Add("?fk_Projektasid_Projektas", MySqlDbType.VarChar).Value = nuotrauka.priskirtas_id;
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

        public bool Prideti_Atsitikimo_Nuotraukas(Nuotrauka nuotrauka)
        {
            try
            {
                string conn = ConfigurationManager.ConnectionStrings["MysqlConnection"].ConnectionString;
                MySqlConnection mySqlConnection = new MySqlConnection(conn);
                string sqlquery = "INSERT INTO `Nuotrauka`(`nuotraukos_nuroda`,`fk_Atsitikimasid_Atsitikimas`) " +
                    "VALUES (?nuotraukos_nuoroda,?fk_Atsitikimasid_Atsitikimas)";
                MySqlCommand mySqlCommand = new MySqlCommand(sqlquery, mySqlConnection);
                mySqlCommand.Parameters.Add("?nuotraukos_nuoroda", MySqlDbType.VarChar).Value = nuotrauka.nuotraukos_nuoroda;
                mySqlCommand.Parameters.Add("?fk_Atsitikimasid_Atsitikimas", MySqlDbType.VarChar).Value = nuotrauka.priskirtas_id;
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

        public List<Nuotrauka> Gauti__Naujienu_Nuotraukas(int id)
        {
            List<Nuotrauka> nuotraukos = new List<Nuotrauka>();
            string conn = ConfigurationManager.ConnectionStrings["MysqlConnection"].ConnectionString;
            MySqlConnection mySqlConnection = new MySqlConnection(conn);
            string sqlquery = "select * from `Nuotrauka` where fk_Naujienaid_Naujiena=" + id;
            MySqlCommand mySqlCommand = new MySqlCommand(sqlquery, mySqlConnection);
            mySqlConnection.Open();
            MySqlDataAdapter mda = new MySqlDataAdapter(mySqlCommand);
            DataTable dt = new DataTable();
            mda.Fill(dt);
            mySqlConnection.Close();

            foreach (DataRow item in dt.Rows)
            {
                nuotraukos.Add(new Nuotrauka
                {
                    id_Nuotrauka = Convert.ToInt32(item["id_Nuotrauka"]),
                    nuotraukos_nuoroda = Convert.ToString(item["nuotraukos_nuroda"]),
                    priskirtas_id = Convert.ToInt32(item["fk_Naujienaid_Naujiena"]),
                });
            }
            return nuotraukos;
        }

        public List<Nuotrauka> Gauti__Renginiu_Nuotraukas(int id)
        {
            List<Nuotrauka> nuotraukos = new List<Nuotrauka>();
            string conn = ConfigurationManager.ConnectionStrings["MysqlConnection"].ConnectionString;
            MySqlConnection mySqlConnection = new MySqlConnection(conn);
            string sqlquery = "select * from `Nuotrauka` where fk_Renginysid_Renginys=" + id;
            MySqlCommand mySqlCommand = new MySqlCommand(sqlquery, mySqlConnection);
            mySqlConnection.Open();
            MySqlDataAdapter mda = new MySqlDataAdapter(mySqlCommand);
            DataTable dt = new DataTable();
            mda.Fill(dt);
            mySqlConnection.Close();

            foreach (DataRow item in dt.Rows)
            {
                nuotraukos.Add(new Nuotrauka
                {
                    id_Nuotrauka = Convert.ToInt32(item["id_Nuotrauka"]),
                    nuotraukos_nuoroda = Convert.ToString(item["nuotraukos_nuroda"]),
                    priskirtas_id = Convert.ToInt32(item["fk_Renginysid_Renginys"]),
                });
            }
            return nuotraukos;
        }

        public List<Nuotrauka> Gauti_Projektu_Nuotraukas(int id)
        {
            List<Nuotrauka> nuotraukos = new List<Nuotrauka>();
            string conn = ConfigurationManager.ConnectionStrings["MysqlConnection"].ConnectionString;
            MySqlConnection mySqlConnection = new MySqlConnection(conn);
            string sqlquery = "select * from `Nuotrauka` where fk_Projektasid_Projektas=" + id;
            MySqlCommand mySqlCommand = new MySqlCommand(sqlquery, mySqlConnection);
            mySqlConnection.Open();
            MySqlDataAdapter mda = new MySqlDataAdapter(mySqlCommand);
            DataTable dt = new DataTable();
            mda.Fill(dt);
            mySqlConnection.Close();

            foreach (DataRow item in dt.Rows)
            {
                nuotraukos.Add(new Nuotrauka
                {
                    id_Nuotrauka = Convert.ToInt32(item["id_Nuotrauka"]),
                    nuotraukos_nuoroda = Convert.ToString(item["nuotraukos_nuroda"]),
                    priskirtas_id = Convert.ToInt32(item["fk_Projektasid_Projektas"]),
                });
            }
            return nuotraukos;
        }

        public Nuotrauka Gauti_Nuotrauka(int id)
        {
            Nuotrauka nuotrauka = new Nuotrauka();
            string conn = ConfigurationManager.ConnectionStrings["MysqlConnection"].ConnectionString;
            MySqlConnection mySqlConnection = new MySqlConnection(conn);
            string sqlquery = "select * from `Nuotrauka` where id_Nuotrauka=" + id;
            MySqlCommand mySqlCommand = new MySqlCommand(sqlquery, mySqlConnection);
            mySqlConnection.Open();
            MySqlDataAdapter mda = new MySqlDataAdapter(mySqlCommand);
            DataTable dt = new DataTable();
            mda.Fill(dt);
            mySqlConnection.Close();

            foreach (DataRow item in dt.Rows)
            {
                nuotrauka.id_Nuotrauka = Convert.ToInt32(item["id_Nuotrauka"]);
                nuotrauka.nuotraukos_nuoroda = Convert.ToString(item["nuotraukos_nuroda"]);
                nuotrauka.priskirtas_id = Convert.ToInt32(item["fk_Naujienaid_Naujiena"]);
            }
            
            return nuotrauka;
        }

        public Nuotrauka Gauti_Renginio_Nuotrauka(int id)
        {
            Nuotrauka nuotrauka = new Nuotrauka();
            string conn = ConfigurationManager.ConnectionStrings["MysqlConnection"].ConnectionString;
            MySqlConnection mySqlConnection = new MySqlConnection(conn);
            string sqlquery = "select * from `Nuotrauka` where id_Nuotrauka=" + id;
            MySqlCommand mySqlCommand = new MySqlCommand(sqlquery, mySqlConnection);
            mySqlConnection.Open();
            MySqlDataAdapter mda = new MySqlDataAdapter(mySqlCommand);
            DataTable dt = new DataTable();
            mda.Fill(dt);
            mySqlConnection.Close();

            foreach (DataRow item in dt.Rows)
            {
                nuotrauka.id_Nuotrauka = Convert.ToInt32(item["id_Nuotrauka"]);
                nuotrauka.nuotraukos_nuoroda = Convert.ToString(item["nuotraukos_nuroda"]);
                nuotrauka.priskirtas_id = Convert.ToInt32(item["fk_Renginysid_Renginys"]);
            }

            return nuotrauka;
        }

        public Nuotrauka Gauti_Projekto_Nuotrauka(int id)
        {
            Nuotrauka nuotrauka = new Nuotrauka();
            string conn = ConfigurationManager.ConnectionStrings["MysqlConnection"].ConnectionString;
            MySqlConnection mySqlConnection = new MySqlConnection(conn);
            string sqlquery = "select * from `Nuotrauka` where id_Nuotrauka=" + id;
            MySqlCommand mySqlCommand = new MySqlCommand(sqlquery, mySqlConnection);
            mySqlConnection.Open();
            MySqlDataAdapter mda = new MySqlDataAdapter(mySqlCommand);
            DataTable dt = new DataTable();
            mda.Fill(dt);
            mySqlConnection.Close();

            foreach (DataRow item in dt.Rows)
            {
                nuotrauka.id_Nuotrauka = Convert.ToInt32(item["id_Nuotrauka"]);
                nuotrauka.nuotraukos_nuoroda = Convert.ToString(item["nuotraukos_nuroda"]);
                nuotrauka.priskirtas_id = Convert.ToInt32(item["fk_Projektasid_Projektas"]);
            }

            return nuotrauka;
        }

        public bool Trinti_Nuotrauka(int id)
        {
            string conn = ConfigurationManager.ConnectionStrings["MysqlConnection"].ConnectionString;
            MySqlConnection mySqlConnection = new MySqlConnection(conn);
            string sqlquery = @"DELETE FROM `Nuotrauka` where id_Nuotrauka=?id";
            MySqlCommand mySqlCommand = new MySqlCommand(sqlquery, mySqlConnection);
            mySqlCommand.Parameters.Add("?id", MySqlDbType.Int32).Value = Convert.ToInt32(id);
            mySqlConnection.Open();
            mySqlCommand.ExecuteNonQuery();
            mySqlConnection.Close();
            return true;
        }

        public List<Nuotrauka> Gauti__Atsitikimu_Nuotraukas(int id)
        {
            List<Nuotrauka> nuotraukos = new List<Nuotrauka>();
            string conn = ConfigurationManager.ConnectionStrings["MysqlConnection"].ConnectionString;
            MySqlConnection mySqlConnection = new MySqlConnection(conn);
            string sqlquery = "select * from `Nuotrauka` where fk_Atsitikimasid_Atsitikimas=" + id;
            MySqlCommand mySqlCommand = new MySqlCommand(sqlquery, mySqlConnection);
            mySqlConnection.Open();
            MySqlDataAdapter mda = new MySqlDataAdapter(mySqlCommand);
            DataTable dt = new DataTable();
            mda.Fill(dt);
            mySqlConnection.Close();

            foreach (DataRow item in dt.Rows)
            {
                nuotraukos.Add(new Nuotrauka
                {
                    id_Nuotrauka = Convert.ToInt32(item["id_Nuotrauka"]),
                    nuotraukos_nuoroda = Convert.ToString(item["nuotraukos_nuroda"]),
                    priskirtas_id = Convert.ToInt32(item["fk_Atsitikimasid_Atsitikimas"]),
                });
            }
            return nuotraukos;
        }
    }
}