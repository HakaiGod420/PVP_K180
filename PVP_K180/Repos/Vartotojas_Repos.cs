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
    public class Vartotojas_Repos
    {
        public bool Registruoti_Vartotoja(Vartotojas vartotojas)
        {
            try
            {
                string conn = ConfigurationManager.ConnectionStrings["MysqlConnection"].ConnectionString;
                MySqlConnection mySqlConnection = new MySqlConnection(conn);
                string sqlquery = "INSERT INTO `Vartotojas`(`slapyvardis`, `slaptazodis`,`el_pastas`, `sukurymo_data`, `role`) " +
                    "VALUES(?slapyvardis,?slaptazodis,?el_pastas,?sukurimo_data,?role)";
                MySqlCommand mySqlCommand = new MySqlCommand(sqlquery, mySqlConnection);
                mySqlCommand.Parameters.Add("?slapyvardis", MySqlDbType.VarChar).Value = vartotojas.slapyvardis;
                mySqlCommand.Parameters.Add("?slaptazodis", MySqlDbType.VarChar).Value = vartotojas.slaptazodis;
                mySqlCommand.Parameters.Add("?el_pastas", MySqlDbType.VarChar).Value = vartotojas.el_pastas;
                mySqlCommand.Parameters.Add("?sukurimo_data", MySqlDbType.DateTime).Value = vartotojas.sukurimo_data;
                mySqlCommand.Parameters.Add("?role", MySqlDbType.Int32).Value = vartotojas.role;
                mySqlConnection.Open();
                mySqlCommand.ExecuteNonQuery();
                mySqlConnection.Close();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
            return false;
        }

        public bool Patikrinti_Duomenys(Prisijungimo_Duomenys duomenys)
        {
            try
            {
                string conn = ConfigurationManager.ConnectionStrings["MysqlConnection"].ConnectionString;
                MySqlConnection mySqlConnection = new MySqlConnection(conn);
                string sqlquery = "SELECT * FROM `Vartotojas` where slapyvardis = ?slapyvardis And slaptazodis=?slaptazodis";
                MySqlCommand mySqlCommand = new MySqlCommand(sqlquery, mySqlConnection);
                mySqlCommand.Parameters.Add("?slapyvardis", MySqlDbType.VarChar).Value = duomenys.slapyvardis;
                mySqlCommand.Parameters.Add("?slaptazodis", MySqlDbType.VarChar).Value = duomenys.slaptazodis;
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

        public int Gauti_Vartotojo_ID(Prisijungimo_Duomenys vartotojas)
        {
            int id = -1;
            try
            {
                string conn = ConfigurationManager.ConnectionStrings["MysqlConnection"].ConnectionString;
                MySqlConnection mySqlConnection = new MySqlConnection(conn);
                string sqlquery = "SELECT id_Vartotojas FROM Vartotojas WHERE slapyvardis=?slapyvardis and slaptazodis = ?slaptazodis";
                MySqlCommand mySqlCommand = new MySqlCommand(sqlquery, mySqlConnection);
                mySqlCommand.Parameters.Add("?slapyvardis", MySqlDbType.VarChar).Value = vartotojas.slapyvardis;
                mySqlCommand.Parameters.Add("?slaptazodis", MySqlDbType.VarChar).Value = vartotojas.slaptazodis;
                mySqlConnection.Open();
                MySqlDataAdapter mda = new MySqlDataAdapter(mySqlCommand);
                DataTable dt = new DataTable();
                mda.Fill(dt);
                mySqlConnection.Close();

                foreach (DataRow item in dt.Rows)
                {
                    id = Convert.ToInt32(item["id_Vartotojas"]);
                }
            }
            catch (Exception ex)
            {
                return id;
            }

            return id;
        }

        public Vartotojas Gauti_Vartotoja(int id)
        {
            Vartotojas vartotojas = new Vartotojas();
            try
            {
                string conn = ConfigurationManager.ConnectionStrings["MysqlConnection"].ConnectionString;
                MySqlConnection mySqlConnection = new MySqlConnection(conn);
                string sqlquery = "select * from " + "Vartotojas where id_Vartotojas=?id";
                MySqlCommand mySqlCommand = new MySqlCommand(sqlquery, mySqlConnection);
                mySqlCommand.Parameters.Add("?id", MySqlDbType.VarChar).Value = id;
                mySqlConnection.Open();
                MySqlDataAdapter mda = new MySqlDataAdapter(mySqlCommand);
                DataTable dt = new DataTable();
                mda.Fill(dt);
                mySqlConnection.Close();

                foreach (DataRow item in dt.Rows)
                {
                    vartotojas.id_Vartotojas = Convert.ToInt32(item["id_Vartotojas"]);
                    vartotojas.slapyvardis = Convert.ToString(item["slapyvardis"]);
                    vartotojas.slaptazodis = Convert.ToString(item["slaptazodis"]);
                    if (item["gimimo_Data"] == DBNull.Value)
                    {
                        vartotojas.gimimo_data = null;
                    }
                    else
                    {
                        vartotojas.gimimo_data = Convert.ToDateTime(item["gimimo_Data"]);
                    }
                    vartotojas.nuotrauka = Convert.ToString(item["nuotrauka"]);
                    vartotojas.vardas = Convert.ToString(item["vardas"]);
                    vartotojas.pavarde = Convert.ToString(item["pavarde"]);
                    vartotojas.tel_nr = Convert.ToString(item["tel_nr"]);
                    vartotojas.sukurimo_data = Convert.ToDateTime(item["sukurymo_data"]);
                    vartotojas.role = Convert.ToInt32(item["role"]);
                    if (item["fk_Vartotojo_Busenaid_Vartotojo_Busena"] == DBNull.Value)
                    {
                        vartotojas.vartotojo_busena = null;
                    }
                    else
                    {
                        vartotojas.vartotojo_busena = Convert.ToInt32(item["fk_Vartotojo_Busenaid_Vartotojo_Busena"]);
                    }

                }

                return vartotojas;
            }
            catch (Exception ex)
            {

            }
            return null;
        }
    }
}