using System;
using MySql;
using MySql.Data.MySqlClient;
using MySql.Data;
using System.Data;

namespace Mercadolibre
{
    public class DataOperations
    {
        public static MySqlConnection ConectarDB()
        {
            string server = "xmendna.csub7veervzk.us-east-1.rds.amazonaws.com"; 
            string database = "mercadolibre";
            string uid = "admin";
            string password = "$Mercadolibre2021*$";
            string connectionString;
            connectionString = "DATASOURCE=" + server + ";" + "DATABASE=" +
            database + ";" + "UID=" + uid + ";" + "PASSWORD=" + password + ";";

            MySqlConnection connection = new MySqlConnection(connectionString);
            return connection;
        }

        public static DataTable Select(string Query)
        {
            MySqlConnection oConn = ConectarDB();

            MySqlDataAdapter oCmd = new MySqlDataAdapter(Query, oConn);
            oCmd.SelectCommand.CommandType = CommandType.Text;
            DataTable dt = new DataTable();
            oCmd.Fill(dt);
            return dt;
        }

        public static bool Insert(string Query)
        {
            try
            {
                MySqlConnection oConn = ConectarDB();
                oConn.Open();
                MySqlCommand cmd = new MySqlCommand(Query, oConn);
                cmd.ExecuteNonQuery();
                oConn.Close();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public static bool Update(string Query)
        {
            try
            {
                MySqlConnection oConn = ConectarDB();
                MySqlCommand cmd = new MySqlCommand();
                cmd.CommandText = Query;
                oConn.Open();
                cmd.Connection = oConn;
                cmd.ExecuteNonQuery();
                oConn.Close();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public static bool Delete(string Query)
        {
            try
            {
                MySqlConnection oConn = ConectarDB();
                MySqlCommand cmd = new MySqlCommand();
                cmd.CommandText = Query;
                oConn.Open();
                cmd.Connection = oConn;
                cmd.ExecuteNonQuery();
                oConn.Close();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

    }
}
