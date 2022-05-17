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
    public class Renginys_Repos
    {
        public bool Sukurti_Rengini(Renginys renginys)
        {
            try
            {
                string conn = ConfigurationManager.ConnectionStrings["MysqlConnection"].ConnectionString;
                MySqlConnection mySqlConnection = new MySqlConnection(conn);
                string sqlquery = "INSERT INTO `Renginys`(`pavadinimas`, `aprasymas`, `reitingas`, `paskelbimo_data`,`pradzios_data`, `pabaigos_data`, `zemelapis_ilguma`, `zemelapis_platuma`,`adresas`,`kaina`, `renginio_busena`, `fk_Vartotojasid_Vartotojas`)" +
                    " VALUES (?pavadinimas, ?aprasymas, ?reitingas, ?paskelbimo_data,?pradzios_data, ?pabaigos_data, ?zemelapis_ilguma, ?zemelapis_platuma,?adresas,?kaina, ?renginio_busena, ?fk_Vartotojasid_Vartotojas)";
                MySqlCommand mySqlCommand = new MySqlCommand(sqlquery, mySqlConnection);
                mySqlCommand.Parameters.Add("?pavadinimas", MySqlDbType.VarChar).Value = renginys.pavadinimas;
                mySqlCommand.Parameters.Add("?aprasymas", MySqlDbType.VarChar).Value = renginys.aprasymas;
                mySqlCommand.Parameters.Add("?reitingas", MySqlDbType.Int32).Value = renginys.reitingas;
                mySqlCommand.Parameters.Add("?paskelbimo_data", MySqlDbType.DateTime).Value = renginys.paskelbimo_data;
                mySqlCommand.Parameters.Add("?pradzios_data", MySqlDbType.DateTime).Value = renginys.pradzios_data;
                mySqlCommand.Parameters.Add("?pabaigos_data", MySqlDbType.DateTime).Value = renginys.pabaigos_data;
                mySqlCommand.Parameters.Add("?zemelapis_ilguma", MySqlDbType.Float).Value = renginys.zemelapis_ilguma;
                mySqlCommand.Parameters.Add("?zemelapis_platuma", MySqlDbType.Float).Value = renginys.zemelapis_platuma;
                mySqlCommand.Parameters.Add("?adresas", MySqlDbType.VarChar).Value = renginys.adresas;
                mySqlCommand.Parameters.Add("?kaina", MySqlDbType.Double).Value = renginys.kaina;
                mySqlCommand.Parameters.Add("?renginio_busena", MySqlDbType.Int32).Value = renginys.renginio_busena;
                mySqlCommand.Parameters.Add("?fk_Vartotojasid_Vartotojas", MySqlDbType.Int32).Value = renginys.vartotojas_id;
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

        public Renginys Gauti_Rengini(int id)
        {
            Renginys renginys = new Renginys();
            string conn = ConfigurationManager.ConnectionStrings["MysqlConnection"].ConnectionString;
            MySqlConnection mySqlConnection = new MySqlConnection(conn);
            string sqlquery = "select * from `Renginys` where id_Renginys=" + id;
            MySqlCommand mySqlCommand = new MySqlCommand(sqlquery, mySqlConnection);
            mySqlConnection.Open();
            MySqlDataAdapter mda = new MySqlDataAdapter(mySqlCommand);
            DataTable dt = new DataTable();
            mda.Fill(dt);
            mySqlConnection.Close();

            foreach (DataRow item in dt.Rows)
            {
                {
                    renginys.id_Renginys = Convert.ToInt32(item["id_Renginys"]);
                    renginys.pavadinimas = Convert.ToString(item["pavadinimas"]);
                    renginys.aprasymas = Convert.ToString(item["aprasymas"]);
                    renginys.reitingas = Convert.ToInt32(item["reitingas"]);
                    renginys.paskelbimo_data = Convert.ToDateTime(item["paskelbimo_data"]);
                    renginys.pradzios_data = Convert.ToDateTime(item["pradzios_data"]);
                    renginys.pabaigos_data = Convert.ToDateTime(item["pabaigos_data"]);
                    renginys.zemelapis_ilguma = Convert.ToInt64(item["zemelapis_ilguma"]);
                    renginys.zemelapis_platuma = Convert.ToInt64(item["zemelapis_platuma"]);
                    renginys.adresas = Convert.ToString(item["adresas"]);
                    renginys.kaina = Convert.ToDouble(item["kaina"]);
                    renginys.renginio_busena = Convert.ToInt32(item["renginio_busena"]);
                }
            }
            return renginys;
        }

        public List<Renginys> Gauti_Renginius()
        {
            List<Renginys> renginys = new List<Renginys>();
            string conn = ConfigurationManager.ConnectionStrings["MysqlConnection"].ConnectionString;
            MySqlConnection mySqlConnection = new MySqlConnection(conn);
            string sqlquery = "select * from " + "Renginys";
            MySqlCommand mySqlCommand = new MySqlCommand(sqlquery, mySqlConnection);
            mySqlConnection.Open();
            MySqlDataAdapter mda = new MySqlDataAdapter(mySqlCommand);
            DataTable dt = new DataTable();
            mda.Fill(dt);
            mySqlConnection.Close();

            foreach (DataRow item in dt.Rows)
            {
                renginys.Add(new Renginys
                {
                    id_Renginys = Convert.ToInt32(item["id_Renginys"]),
                    pavadinimas = Convert.ToString(item["pavadinimas"]),
                    aprasymas = Convert.ToString(item["aprasymas"]),
                    reitingas = Convert.ToInt32(item["reitingas"]),
                    paskelbimo_data = Convert.ToDateTime(item["paskelbimo_data"]),
                    pradzios_data = Convert.ToDateTime(item["pradzios_data"]),
                    pabaigos_data = Convert.ToDateTime(item["pabaigos_data"]),
                    zemelapis_ilguma = Convert.ToInt64(item["zemelapis_ilguma"]),
                    zemelapis_platuma = Convert.ToInt64(item["zemelapis_platuma"]),
                    renginio_busena = Convert.ToInt32(item["renginio_busena"]),
                    adresas = Convert.ToString(item["adresas"]),
                    kaina = Convert.ToDouble(item["kaina"]),
                vartotojas_id = Convert.ToInt32(item["fk_Vartotojasid_Vartotojas"]),
                }); ;
            }
            return renginys;
        }

        public List<Renginys> Gauti_Renginius(int renginio_busena)
        {
            List<Renginys> renginys = new List<Renginys>();
            string conn = ConfigurationManager.ConnectionStrings["MysqlConnection"].ConnectionString;
            MySqlConnection mySqlConnection = new MySqlConnection(conn);
            string sqlquery = "select * from " + "Renginys WHERE renginio_busena =" + renginio_busena;
            MySqlCommand mySqlCommand = new MySqlCommand(sqlquery, mySqlConnection);
            mySqlConnection.Open();
            MySqlDataAdapter mda = new MySqlDataAdapter(mySqlCommand);
            DataTable dt = new DataTable();
            mda.Fill(dt);
            mySqlConnection.Close();

            foreach (DataRow item in dt.Rows)
            {
                renginys.Add(new Renginys
                {
                    id_Renginys = Convert.ToInt32(item["id_Renginys"]),
                    pavadinimas = Convert.ToString(item["pavadinimas"]),
                    aprasymas = Convert.ToString(item["aprasymas"]),
                    reitingas = Convert.ToInt32(item["reitingas"]),
                    paskelbimo_data = Convert.ToDateTime(item["paskelbimo_data"]),
                    pradzios_data = Convert.ToDateTime(item["pradzios_data"]),
                    pabaigos_data = Convert.ToDateTime(item["pabaigos_data"]),
                    zemelapis_ilguma = Convert.ToInt64(item["zemelapis_ilguma"]),
                    zemelapis_platuma = Convert.ToInt64(item["zemelapis_platuma"]),
                    adresas = Convert.ToString(item["adresas"]),
                    kaina = Convert.ToDouble(item["kaina"]),
                    renginio_busena = Convert.ToInt32(item["renginio_busena"]),
                    vartotojas_id = Convert.ToInt32(item["fk_Vartotojasid_Vartotojas"]),
                }); ;
            }
            return renginys;
        }

        public bool Redaguoti_Rengini(Renginys renginys)
        {
            try
            {
                string conn = ConfigurationManager.ConnectionStrings["MysqlConnection"].ConnectionString;
                MySqlConnection mySqlConnection = new MySqlConnection(conn);
                string sqlquery = "UPDATE `Renginys` SET `pavadinimas`=?pavadinimas,`aprasymas`=?aprasymas,`paskelbimo_data`=?paskelbimo_data,`pabaigos_data`=?pabaigos_data,`pradzios_data`=?pradzios_data,`zemelapis_ilguma`=?zemelapis_ilguma,`zemelapis_platuma`=?zemelapis_platuma,`adresas`=?adresas,`kaina`= ?kaina,`renginio_busena`=?renginio_busena,`fk_Vartotojasid_Vartotojas`=?fk_Vartotojasid_Vartotojas WHERE id_Renginys=" + renginys.id_Renginys;
                MySqlCommand mySqlCommand = new MySqlCommand(sqlquery, mySqlConnection);
                mySqlCommand.Parameters.Add("?pavadinimas", MySqlDbType.Text).Value = renginys.pavadinimas;
                mySqlCommand.Parameters.Add("?aprasymas", MySqlDbType.Text).Value = renginys.aprasymas;
                mySqlCommand.Parameters.Add("?paskelbimo_data", MySqlDbType.DateTime).Value = renginys.paskelbimo_data;
                mySqlCommand.Parameters.Add("?pradzios_data", MySqlDbType.DateTime).Value = renginys.pradzios_data;
                mySqlCommand.Parameters.Add("?pabaigos_data", MySqlDbType.DateTime).Value = renginys.pabaigos_data;
                mySqlCommand.Parameters.Add("?zemelapis_ilguma", MySqlDbType.Float).Value = renginys.zemelapis_ilguma;
                mySqlCommand.Parameters.Add("?zemelapis_platuma", MySqlDbType.Float).Value = renginys.zemelapis_platuma;
                mySqlCommand.Parameters.Add("?adresas", MySqlDbType.VarChar).Value = renginys.adresas;
                mySqlCommand.Parameters.Add("?kaina", MySqlDbType.Double).Value = renginys.kaina;
                mySqlCommand.Parameters.Add("?renginio_busena", MySqlDbType.Int32).Value = renginys.renginio_busena;
                mySqlCommand.Parameters.Add("?fk_Vartotojasid_Vartotojas", MySqlDbType.Int32).Value = renginys.vartotojas_id;
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

        public List<Renginys> RastiRenginius(Renginys renginys)
        {
            string sqlquery = "";
            List <Renginys> rasti_renginiai = new List<Renginys>();
            string conn = ConfigurationManager.ConnectionStrings["MysqlConnection"].ConnectionString;
            MySqlConnection mySqlConnection = new MySqlConnection(conn);
            if (renginys.renginio_busena != 0)
            {
                sqlquery = "select * from `Renginys` where renginio_busena=" + renginys.renginio_busena;
            }
            sqlquery = "select * from " + "Renginys";
            MySqlCommand mySqlCommand = new MySqlCommand(sqlquery, mySqlConnection);
            mySqlConnection.Open();
            MySqlDataAdapter mda = new MySqlDataAdapter(mySqlCommand);
            DataTable dt = new DataTable();
            mda.Fill(dt);
            mySqlConnection.Close();

            foreach (DataRow item in dt.Rows)
            {
                rasti_renginiai.Add(new Renginys
                {
                    id_Renginys = Convert.ToInt32(item["id_Renginys"]),
                    pavadinimas = Convert.ToString(item["pavadinimas"]),
                    aprasymas = Convert.ToString(item["aprasymas"]),
                    reitingas = Convert.ToInt32(item["reitingas"]),
                    paskelbimo_data = Convert.ToDateTime(item["paskelbimo_data"]),
                    pradzios_data = Convert.ToDateTime(item["pradzios_data"]),
                    pabaigos_data = Convert.ToDateTime(item["pabaigos_data"]),
                    zemelapis_ilguma = Convert.ToInt64(item["zemelapis_ilguma"]),
                    zemelapis_platuma = Convert.ToInt64(item["zemelapis_platuma"]),
                    adresas = Convert.ToString(item["adresas"]),
                    kaina = Convert.ToDouble(item["kaina"]),
                    renginio_busena = Convert.ToInt32(item["renginio_busena"]),
                    vartotojas_id = Convert.ToInt32(item["fk_Vartotojasid_Vartotojas"]),
                }); ;
            }
            return rasti_renginiai;
        }

        public bool Rasyti_Komentara(Komentaras komentaras)
        {
            try
            {
                string conn = ConfigurationManager.ConnectionStrings["MysqlConnection"].ConnectionString;
                MySqlConnection mySqlConnection = new MySqlConnection(conn);
                string sqlquery = "INSERT INTO `Komentaras`(`komentaro_tekstas`, `parasymo_data`, `fk_Renginysid_Renginys`, `fk_Vartotojasid_Vartotojas`) " +
                    "VALUES (?komentaro_tekstas,?parasymo_data,?fk_Renginysid_Renginys,?fk_Vartotojasid_Vartotojas)";
                MySqlCommand mySqlCommand = new MySqlCommand(sqlquery, mySqlConnection);
                mySqlCommand.Parameters.Add("?komentaro_tekstas", MySqlDbType.VarChar).Value = komentaras.komentaro_tekstas;
                mySqlCommand.Parameters.Add("?parasymo_data", MySqlDbType.DateTime).Value = komentaras.parasymo_data;
                mySqlCommand.Parameters.Add("?fk_Renginysid_Renginys", MySqlDbType.Int32).Value = komentaras.priskirtas_id;
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

        public List<Komentaras> Gauti_Renginio_Komentarus(int id)
        {
            List<Komentaras> komentarai = new List<Komentaras>();
            string conn = ConfigurationManager.ConnectionStrings["MysqlConnection"].ConnectionString;
            MySqlConnection mySqlConnection = new MySqlConnection(conn);
            string sqlquery = "SELECT id_Komentaras,komentaro_tekstas,parasymo_data,fk_Renginysid_Renginys," +
                "fk_Projektasid_Projektas,fk_Vartotojasid_Vartotojas,Vartotojas.slapyvardis,Vartotojas.nuotrauka FROM `Komentaras`" +
                "LEFT JOIN Vartotojas ON Vartotojas.id_Vartotojas = fk_Vartotojasid_Vartotojas WHERE fk_Renginysid_Renginys=" + id;
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
                    priskirtas_id = Convert.ToInt32(item["fk_Renginysid_Renginys"]),
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

        public int Gauti_Paskutini_Prideto_Index()
        {
            try
            {
                int index = -1;
                string conn = ConfigurationManager.ConnectionStrings["MysqlConnection"].ConnectionString;
                MySqlConnection mySqlConnection = new MySqlConnection(conn);
                string sqlquery = "SELECT MAX(id_Renginys) as 'max_id' FROM `Renginys`";
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

        public IEnumerable<Renginio_Busena> GautiBusenas()
        {
            try
            {
                List<Renginio_Busena> renginio_Busenas = new List<Renginio_Busena>();
                string conn = ConfigurationManager.ConnectionStrings["MysqlConnection"].ConnectionString;
                MySqlConnection mySqlConnection = new MySqlConnection(conn);
                string sqlquery = "SELECT * FROM `Renginio_busena` WHERE 1";
                MySqlCommand mySqlCommand = new MySqlCommand(sqlquery, mySqlConnection);
                mySqlConnection.Open();
                MySqlDataAdapter mda = new MySqlDataAdapter(mySqlCommand);
                DataTable dt = new DataTable();
                mda.Fill(dt);
                mySqlConnection.Close();

                foreach (DataRow item in dt.Rows)
                {
                    renginio_Busenas.Add(new Renginio_Busena
                    {

                        id_Renginio_busena = Convert.ToInt32(item["id_Renginio_Busena"]),
                        name = Convert.ToString(item["name"])
                    }); ;
                }
                return renginio_Busenas;
            }

            catch (Exception ex)
            {
                return null;
            }
        }

        public bool Atnaujinti_Renginio_Busena(int renginio_id, int busenos_id)
        {
            try
            {
                string conn = ConfigurationManager.ConnectionStrings["MysqlConnection"].ConnectionString;
                MySqlConnection mySqlConnection = new MySqlConnection(conn);
                string sqlquery = "UPDATE `Renginys` SET `renginio_busena`=?busena WHERE id_Renginys=" + renginio_id;
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
    }
}