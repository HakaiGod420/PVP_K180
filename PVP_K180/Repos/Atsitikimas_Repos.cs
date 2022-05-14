using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PVP_K180.ModelView;
using PVP_K180.Models;
using System.Configuration;
using MySql.Data.MySqlClient;
using System.Data;

namespace PVP_K180.Repos
{
    public class Atsitikimas_Repos
    {
        public List<Atsitikimo_Tipas> GautiAtsitikimoTipus()
        {
            try
            {
                List<Atsitikimo_Tipas> tipai = new List<Atsitikimo_Tipas>();
                string conn = ConfigurationManager.ConnectionStrings["MysqlConnection"].ConnectionString;
                MySqlConnection mySqlConnection = new MySqlConnection(conn);
                string sqlquery = "SELECT * FROM `Atsitikimo_Tipas` WHERE 1";
                MySqlCommand mySqlCommand = new MySqlCommand(sqlquery, mySqlConnection);
                mySqlConnection.Open();
                MySqlDataAdapter mda = new MySqlDataAdapter(mySqlCommand);
                DataTable dt = new DataTable();
                mda.Fill(dt);
                mySqlConnection.Close();

                foreach (DataRow item in dt.Rows)
                {
                    tipai.Add(new Atsitikimo_Tipas
                    {

                        id_Atsitikimas_Tipas = Convert.ToInt32(item["id_Atsitikimo_Tipas"]),
                        name = Convert.ToString(item["name"])
                    }); ;
                }
                return tipai;
            }

            catch (Exception ex)
            {
                return null;
            }
        }

        public bool PridetiAtsitikima(Atsitikimas atsitikimas)
        {
            try
            {
                string conn = ConfigurationManager.ConnectionStrings["MysqlConnection"].ConnectionString;
                MySqlConnection mySqlConnection = new MySqlConnection(conn);
                string sqlquery = "INSERT INTO `Atsitikimas`( `paskelbimo_data`, `aprasymas`, `zemelapis_ilguma`, `zemelapis_platuma`, `atsitikimo_tipas`, `atsitikimo_busena`, `fk_Vartotojasid_Pranesejas`) " +
                    "VALUES (?paskelbimo_data,?aprasymas,?zemelapis_ilguma,?zemelapis_platuma,?atsitikimo_tipas,?atsitikimo_busena,?fk_Vartotojasid_Pranesejas)";
                MySqlCommand mySqlCommand = new MySqlCommand(sqlquery, mySqlConnection);
                mySqlCommand.Parameters.Add("?paskelbimo_data", MySqlDbType.Date).Value = atsitikimas.paskelbimo_data;
                mySqlCommand.Parameters.Add("?aprasymas", MySqlDbType.VarChar).Value = atsitikimas.aprasymas;
                mySqlCommand.Parameters.Add("?zemelapis_ilguma", MySqlDbType.Double).Value = atsitikimas.zemelapio_ilguma;
                mySqlCommand.Parameters.Add("?zemelapis_platuma", MySqlDbType.Double).Value = atsitikimas.zemelapio_platuma;
                mySqlCommand.Parameters.Add("?atsitikimo_busena", MySqlDbType.Int32).Value = atsitikimas.atsitikimo_busena;
                mySqlCommand.Parameters.Add("?atsitikimo_tipas", MySqlDbType.Int32).Value = atsitikimas.atsitikimo_tipas;
                mySqlCommand.Parameters.Add("?fk_Vartotojasid_Pranesejas", MySqlDbType.Int32).Value = atsitikimas.fk_Vartotojasid_Pranesejas;
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


        public List<Atsitikimas> GautiAtsitikimus()
        {
            try
            {
                List<Atsitikimas> atsitikimai = new List<Atsitikimas>();
                string conn = ConfigurationManager.ConnectionStrings["MysqlConnection"].ConnectionString;
                MySqlConnection mySqlConnection = new MySqlConnection(conn);
                string sqlquery = "SELECT id_Atsitikimas, paskelbimo_data,komentaras, aprasymas,zemelapis_ilguma,zemelapis_platuma,atsitikimo_tipas,atsitikimo_busena,fk_Vartotojasid_Pranesejas,fk_Vartotojasid_Tvirtintojas," +
                    "Atsitikimo_Tipas.name AS 'atsitikimas',Atsitikimo_Busena.name as 'busena', g.slapyvardis as 'tvirtintojas', Vartotojas.slapyvardis as 'pranesejas' FROM `Atsitikimas`" +
                    " LEFT JOIN Atsitikimo_Tipas ON Atsitikimo_Tipas.id_Atsitikimo_Tipas = atsitikimo_tipas " +
                    "LEFT JOIN Atsitikimo_Busena ON Atsitikimo_Busena.id_Atsitikimo_Busena = atsitikimo_busena " +
                    "LEFT JOIN Vartotojas ON Vartotojas.id_Vartotojas = fk_Vartotojasid_Pranesejas LEFT JOIN Vartotojas as g ON g.id_Vartotojas = fk_Vartotojasid_Tvirtintojas";
                MySqlCommand mySqlCommand = new MySqlCommand(sqlquery, mySqlConnection);
                mySqlConnection.Open();
                MySqlDataAdapter mda = new MySqlDataAdapter(mySqlCommand);
                DataTable dt = new DataTable();
                mda.Fill(dt);
                mySqlConnection.Close();

                foreach (DataRow item in dt.Rows)
                {
                    atsitikimai.Add(new Atsitikimas
                    {

                        id_Atstikimas = Convert.ToInt32(item["id_Atsitikimas"]),
                        paskelbimo_data = Convert.ToDateTime(item["paskelbimo_data"]),
                        aprasymas = CheckIfDataNull(item, "aprasymas"),
                        zemelapio_ilguma = Convert.ToDouble(item["zemelapis_ilguma"]),
                        zemelapio_platuma = Convert.ToDouble(item["zemelapis_platuma"]),
                        atsitikimo_tipas = Convert.ToInt32(item["atsitikimo_tipas"]),
                        atsitikimo_busena = Convert.ToInt32(item["atsitikimo_busena"]),
                        fk_Vartotojasid_Pranesejas = Convert.ToInt32(item["fk_Vartotojasid_Pranesejas"]),
                        fk_Vartotojasid_Tvirtintojas = CheckIfDataNull(item, "fk_Vartotojasid_Tvirtintojas"),
                        tipas = Convert.ToString(item["atsitikimas"]),
                        busena = Convert.ToString(item["busena"]),
                        tvirtintojas = CheckIfDataNull(item, "tvirtintojas"),
                        pranesejas = Convert.ToString(item["pranesejas"]),
                        komentaras = CheckIfDataNull(item, "komentaras"),
                    }); ;
                }
                return atsitikimai;
            }

            catch (Exception ex)
            {
                return null;
            }
        }


        public Atsitikimas Gauti_Atsitikima(int id)
        {
            Atsitikimas atsitikimas = new Atsitikimas();
            string conn = ConfigurationManager.ConnectionStrings["MysqlConnection"].ConnectionString;
            MySqlConnection mySqlConnection = new MySqlConnection(conn);
            string sqlquery = "SELECT id_Atsitikimas, paskelbimo_data,komentaras, aprasymas,zemelapis_ilguma,zemelapis_platuma,atsitikimo_tipas,atsitikimo_busena,fk_Vartotojasid_Pranesejas,fk_Vartotojasid_Tvirtintojas," +
            "Atsitikimo_Tipas.name AS 'atsitikimas',Atsitikimo_Busena.name as 'busena', g.slapyvardis as 'tvirtintojas', Vartotojas.slapyvardis as 'pranesejas' FROM `Atsitikimas`" +
            " LEFT JOIN Atsitikimo_Tipas ON Atsitikimo_Tipas.id_Atsitikimo_Tipas = atsitikimo_tipas " +
            "LEFT JOIN Atsitikimo_Busena ON Atsitikimo_Busena.id_Atsitikimo_Busena = atsitikimo_busena " +
            "LEFT JOIN Vartotojas ON Vartotojas.id_Vartotojas = fk_Vartotojasid_Pranesejas LEFT JOIN Vartotojas as g ON g.id_Vartotojas = fk_Vartotojasid_Tvirtintojas where id_Atsitikimas="+id;

            MySqlCommand mySqlCommand = new MySqlCommand(sqlquery, mySqlConnection);
            mySqlConnection.Open();
            MySqlDataAdapter mda = new MySqlDataAdapter(mySqlCommand);
            DataTable dt = new DataTable();
            mda.Fill(dt);
            mySqlConnection.Close();

            foreach (DataRow item in dt.Rows)
            {
                {
                    atsitikimas.id_Atstikimas = Convert.ToInt32(item["id_Atsitikimas"]);
                    atsitikimas.paskelbimo_data = Convert.ToDateTime(item["paskelbimo_data"]);
                    atsitikimas.aprasymas = CheckIfDataNull(item, "aprasymas");
                    atsitikimas.zemelapio_ilguma = Convert.ToDouble(item["zemelapis_ilguma"]);
                    atsitikimas.zemelapio_platuma = Convert.ToDouble(item["zemelapis_platuma"]);
                    atsitikimas.atsitikimo_tipas = Convert.ToInt32(item["atsitikimo_tipas"]);
                    atsitikimas.atsitikimo_busena = Convert.ToInt32(item["atsitikimo_busena"]);
                    atsitikimas.fk_Vartotojasid_Pranesejas = Convert.ToInt32(item["fk_Vartotojasid_Pranesejas"]);
                    atsitikimas.fk_Vartotojasid_Tvirtintojas = CheckIfDataNull(item, "fk_Vartotojasid_Tvirtintojas");
                    atsitikimas.tipas = Convert.ToString(item["atsitikimas"]);
                    atsitikimas.busena = Convert.ToString(item["busena"]);
                    atsitikimas.tvirtintojas = CheckIfDataNull(item, "tvirtintojas");
                    atsitikimas.pranesejas = Convert.ToString(item["pranesejas"]);
                    atsitikimas.komentaras = CheckIfDataNull(item, "komentaras");
                }
            }
            return atsitikimas;
        }

        private dynamic CheckIfDataNull(DataRow data, string type)
        {
            if (type == "fk_Vartotojasid_Tvirtintojas")
            {
                if (data["fk_Vartotojasid_Tvirtintojas"] == DBNull.Value)
                {
                    return null;
                }
                else
                {
                    return Convert.ToInt32(data["fk_Vartotojasid_Tvirtintojas"]);
                }
            }

            if (type == "tvirtintojas")
            {
                if (data["tvirtintojas"] == DBNull.Value)
                {
                    return null;
                }
                else
                {
                    return Convert.ToString(data["tvirtintojas"]);
                }
            }

            if (type == "komentaras")
            {
                if (data["komentaras"] == DBNull.Value)
                {
                    return null;
                }
                else
                {
                    return Convert.ToString(data["komentaras"]);
                }
            }

            if (type == "aprasymas")
            {
                if (data["aprasymas"] == DBNull.Value)
                {
                    return null;
                }
                else
                {
                    return Convert.ToString(data["aprasymas"]);
                }
            }

           
            return null;

        }


        public int Gauti_Paskutini_Prideto_Index()
        {
            try
            {
                int index = -1;
                string conn = ConfigurationManager.ConnectionStrings["MysqlConnection"].ConnectionString;
                MySqlConnection mySqlConnection = new MySqlConnection(conn);
                string sqlquery = "SELECT MAX(id_Atsitikimas) as 'max_id' FROM `Atsitikimas`";
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