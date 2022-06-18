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

        public List<Vartotojas> GautiVartotojus()
        {
            try
            {
                List<Vartotojas> vartotojai = new List<Vartotojas>();
                string conn = ConfigurationManager.ConnectionStrings["MysqlConnection"].ConnectionString;
                MySqlConnection mySqlConnection = new MySqlConnection(conn);
                string sqlquery = "SELECT * FROM `Vartotojas`";
                MySqlCommand mySqlCommand = new MySqlCommand(sqlquery, mySqlConnection);
                mySqlConnection.Open();
                MySqlDataAdapter mda = new MySqlDataAdapter(mySqlCommand);
                DataTable dt = new DataTable();
                mda.Fill(dt);
                mySqlConnection.Close();

                foreach (DataRow item in dt.Rows)
                {
                    vartotojai.Add(new Vartotojas
                    {

                        id_Vartotojas = Convert.ToInt32(item["id_Vartotojas"]),
                        slapyvardis = Convert.ToString(item["slapyvardis"]),
                        slaptazodis = Convert.ToString(item["slaptazodis"]),
                        el_pastas = Convert.ToString(item["el_pastas"]),
                        gimimo_data = CheckIfDateNull(item,"gimimo_data"),
                        nuotrauka = Convert.ToString(item["nuotrauka"]),
                        vardas = CheckIfDateNull(item, "vardas"),
                        pavarde = CheckIfDateNull(item, "pavarde"),
                        tel_nr = CheckIfDateNull(item, "tel_nr"),
                        sukurimo_data = Convert.ToDateTime(item["sukurymo_data"]),
                        role = Convert.ToInt32(item["role"]),
                        vartotojo_busena = CheckIfDateNull(item, "fk_Vartotojo_Busenaid_Vartotojo_Busena"),
                    }); ;
                }
                return vartotojai;
            }

            catch (Exception ex)
            {
                return null;
            }
        }

        private dynamic CheckIfDateNull(DataRow data, string type)
        {
            if (type == "gimimo_data")
            {
                if (data["gimimo_data"] == DBNull.Value)
                {
                    return null;
                }
                else
                {
                    return Convert.ToDateTime(data["gimimo_data"]);
                }
            }

            if (type == "vardas")
            {
                if (data["vardas"] == DBNull.Value)
                {
                    return null;
                }
                else
                {
                    return Convert.ToString(data["vardas"]);
                }
            }

            if (type == "pavarde")
            {
                if (data["pavarde"] == DBNull.Value)
                {
                    return null;
                }
                else
                {
                    return Convert.ToString(data["pavarde"]);
                }
            }

            if (type == "tel_nr")
            {
                if (data["tel_nr"] == DBNull.Value)
                {
                    return null;
                }
                else
                {
                    return Convert.ToString(data["tel_nr"]);
                }
            }

            if (type == "fk_Vartotojo_Busenaid_Vartotojo_Busena")
            {
                if (data["fk_Vartotojo_Busenaid_Vartotojo_Busena"] == DBNull.Value)
                {
                    return null;
                }
                else
                {
                    return Convert.ToInt32(data["fk_Vartotojo_Busenaid_Vartotojo_Busena"]);
                }
            }
            return null;

        }


        public bool TrintiVartotoja(int id)
        {
            string conn = ConfigurationManager.ConnectionStrings["MysqlConnection"].ConnectionString;
            MySqlConnection mySqlConnection = new MySqlConnection(conn);
            string sqlquery = @"DELETE FROM `Vartotojas` where id_Vartotojas=?id";
            MySqlCommand mySqlCommand = new MySqlCommand(sqlquery, mySqlConnection);
            mySqlCommand.Parameters.Add("?id", MySqlDbType.Int32).Value = Convert.ToInt32(id);
            mySqlConnection.Open();
            mySqlCommand.ExecuteNonQuery();
            mySqlConnection.Close();
            return true;
        }

        public List<Vartotojo_Busena> GautiBusenas()
        {
            try
            {
                List<Vartotojo_Busena> vartotojo_Busenas = new List<Vartotojo_Busena>();
                string conn = ConfigurationManager.ConnectionStrings["MysqlConnection"].ConnectionString;
                MySqlConnection mySqlConnection = new MySqlConnection(conn);
                string sqlquery = "SELECT * FROM `Vartotojo_Busena` WHERE 1";
                MySqlCommand mySqlCommand = new MySqlCommand(sqlquery, mySqlConnection);
                mySqlConnection.Open();
                MySqlDataAdapter mda = new MySqlDataAdapter(mySqlCommand);
                DataTable dt = new DataTable();
                mda.Fill(dt);
                mySqlConnection.Close();

                foreach (DataRow item in dt.Rows)
                {
                    vartotojo_Busenas.Add(new Vartotojo_Busena
                    {

                        id_Vartotojo_Busena = Convert.ToInt32(item["id_Vartotojo_Busena"]),
                        name = Convert.ToString(item["name"])
                    }); ;
                }
                return vartotojo_Busenas;
            }

            catch (Exception ex)
            {
                return null;
            }
        }

        public bool AtnaujintiVartotojoBusena(int vartotojo_id, int? busenos_id)
        {
            try
            {
                string conn = ConfigurationManager.ConnectionStrings["MysqlConnection"].ConnectionString;
                MySqlConnection mySqlConnection = new MySqlConnection(conn);
                string sqlquery = "UPDATE `Vartotojas` SET `fk_Vartotojo_Busenaid_Vartotojo_Busena`=?busena WHERE id_Vartotojas=" + vartotojo_id;
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

        public bool AtnaujintiVartotojoRole(int vartotojo_id, int roles_id)
        {
            try
            {
                string conn = ConfigurationManager.ConnectionStrings["MysqlConnection"].ConnectionString;
                MySqlConnection mySqlConnection = new MySqlConnection(conn);
                string sqlquery = "UPDATE `Vartotojas` SET `role`=?role WHERE id_Vartotojas=" + vartotojo_id;
                MySqlCommand mySqlCommand = new MySqlCommand(sqlquery, mySqlConnection);
                mySqlCommand.Parameters.Add("?role", MySqlDbType.Int32).Value = roles_id;
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

        public int Gauti_Naujienlaiskio_Prenumerata(int id)
        {
            try
            {
                int index = -1;
                string conn = ConfigurationManager.ConnectionStrings["MysqlConnection"].ConnectionString;
                MySqlConnection mySqlConnection = new MySqlConnection(conn);
                string sqlquery = "SELECT * FROM `Vartotojas` WHERE id_Vartotojas="+id;
                MySqlCommand mySqlCommand = new MySqlCommand(sqlquery, mySqlConnection);
                mySqlConnection.Open();
                MySqlDataAdapter mda = new MySqlDataAdapter(mySqlCommand);
                DataTable dt = new DataTable();
                mda.Fill(dt);
                mySqlConnection.Close();

                foreach (DataRow item in dt.Rows)
                {
                    index = Convert.ToInt32(item["aktyvaves_naujienlaiski"]);
                }
                return index;
            }
            catch (Exception ex)
            {
                return -1;
            }
        }

        public bool AtnaujintiNaujienlaiskioPrenumerata(int vartotojo_id, int prenumerata)
        {
            try
            {
                string conn = ConfigurationManager.ConnectionStrings["MysqlConnection"].ConnectionString;
                MySqlConnection mySqlConnection = new MySqlConnection(conn);
                string sqlquery = "UPDATE `Vartotojas` SET `aktyvaves_naujienlaiski`=?aktyvaves_naujienlaiski WHERE id_Vartotojas=" + vartotojo_id;
                MySqlCommand mySqlCommand = new MySqlCommand(sqlquery, mySqlConnection);
                mySqlCommand.Parameters.Add("?aktyvaves_naujienlaiski", MySqlDbType.Int32).Value = prenumerata;
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

        public List<string> GautiEmailSub()
        {
            try
            {
                List<string> email = new List<string>();
                string conn = ConfigurationManager.ConnectionStrings["MysqlConnection"].ConnectionString;
                MySqlConnection mySqlConnection = new MySqlConnection(conn);
                string sqlquery = "SELECT el_pastas FROM `Vartotojas` WHERE aktyvaves_naujienlaiski=1";
                MySqlCommand mySqlCommand = new MySqlCommand(sqlquery, mySqlConnection);
                mySqlConnection.Open();
                MySqlDataAdapter mda = new MySqlDataAdapter(mySqlCommand);
                DataTable dt = new DataTable();
                mda.Fill(dt);
                mySqlConnection.Close();

                foreach (DataRow item in dt.Rows)
                {
                    email.Add(Convert.ToString(item["el_pastas"]));
                }
                return email;
            }

            catch (Exception ex)
            {
                return null;
            }
        }
    }
}