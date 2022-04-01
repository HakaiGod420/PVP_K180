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
                    vartotojas.el_pastas = Convert.ToString(item["el_pastas"]);
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

        public bool AtnaujintiEmail(Pasto_duomenys pasto_Duomenys)
        {
            try
            {
                string conn = ConfigurationManager.ConnectionStrings["MysqlConnection"].ConnectionString;
                MySqlConnection mySqlConnection = new MySqlConnection(conn);
                string sqlquery = "UPDATE `Vartotojas` SET el_pastas=?el_pastas WHERE `id_Vartotojas` = " + pasto_Duomenys.id_vartotojas;
                MySqlCommand mySqlCommand = new MySqlCommand(sqlquery, mySqlConnection);
                mySqlCommand.Parameters.Add("?el_pastas", MySqlDbType.VarChar).Value = pasto_Duomenys.el_pastas;
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

        public bool AtnaujintiPass(Slaptazodzio_Atnaujinimo_Duomenys passData)
        {
            try
            {
                string conn = ConfigurationManager.ConnectionStrings["MysqlConnection"].ConnectionString;
                MySqlConnection mySqlConnection = new MySqlConnection(conn);
                string sqlquery = "UPDATE `Vartotojas` SET slaptazodis=?slaptazodis WHERE `id_Vartotojas` = " + passData.id_Vartotojas;
                MySqlCommand mySqlCommand = new MySqlCommand(sqlquery, mySqlConnection);
                mySqlCommand.Parameters.Add("?slaptazodis", MySqlDbType.VarChar).Value = passData.naujas_slaptazodis;
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

        public bool AtnaujintiPagrDuomenis(Atnaujinami_Vartotojo_Duomenys duomenys)
        {
            try
            {
                string conn = ConfigurationManager.ConnectionStrings["MysqlConnection"].ConnectionString;
                MySqlConnection mySqlConnection = new MySqlConnection(conn);
                string sqlquery = "UPDATE `Vartotojas` SET gimimo_data=?gimimo_data,vardas=?vardas,pavarde=?pavarde,tel_nr=?tel_nr WHERE `id_Vartotojas` = " + duomenys.id_Vartotojas;
                MySqlCommand mySqlCommand = new MySqlCommand(sqlquery, mySqlConnection);
                mySqlCommand.Parameters.Add("?gimimo_data", MySqlDbType.DateTime).Value = duomenys.gimimo_data;
                mySqlCommand.Parameters.Add("?vardas", MySqlDbType.VarChar).Value = duomenys.vardas;
                mySqlCommand.Parameters.Add("?pavarde", MySqlDbType.VarChar).Value = duomenys.pavarde;
                mySqlCommand.Parameters.Add("?tel_nr", MySqlDbType.VarChar).Value = duomenys.tel_nr;
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

        public bool AtnaujintiNuotrauka(Nuotrauka nuotrauka)
        {
            try
            {
                string conn = ConfigurationManager.ConnectionStrings["MysqlConnection"].ConnectionString;
                MySqlConnection mySqlConnection = new MySqlConnection(conn);
                string sqlquery = "UPDATE `Vartotojas` SET nuotrauka=?nuotrauka WHERE `id_Vartotojas` = " + nuotrauka.priskirtas_id;
                MySqlCommand mySqlCommand = new MySqlCommand(sqlquery, mySqlConnection);
                mySqlCommand.Parameters.Add("?nuotrauka", MySqlDbType.VarChar).Value = nuotrauka.nuotraukos_nuoroda;
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