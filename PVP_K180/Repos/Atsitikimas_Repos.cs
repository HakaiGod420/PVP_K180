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
    }
}