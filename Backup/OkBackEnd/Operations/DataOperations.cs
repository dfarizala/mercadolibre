using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Configuration;
using System.Security.Cryptography.X509Certificates;
using System.Web;
using Npgsql;
using NpgsqlTypes;


namespace OkBackEnd
{
    class DataOperations
    {
        public static string MakeConString()
        {
            try
            {
                string server = "okapp.cbualopuhzsq.us-east-1.rds.amazonaws.com";//"54.236.60.7";
                string database = "okappdev";
                string uid = "okappdev";
                string password = "G2Cfyi5!A93pt6X";
                string connectionString;
                connectionString = "Host = " + server + "; Username = " + uid + "; Password = " + password + "; Database = " + database + "";
                return connectionString;
            }
            catch
            {
                return "";
            }
        }

        public NpgsqlConnection dbConnection()
        {
            try
            {
                NpgsqlConnection oConn = new NpgsqlConnection();
                oConn.ConnectionString = MakeConString();
                return oConn;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public static bool Insert(string sTable, string sItems, string sValues)
        {
            try
            {
                string ConnString = MakeConString();
                string makeString = "INSERT INTO {0} ({1}) VALUES ({2})";
                string SqlString = string.Format(makeString, sTable, sItems, sValues);

                using (NpgsqlConnection conn = new NpgsqlConnection(ConnString))
                {
                    using (NpgsqlCommand cmd = new NpgsqlCommand(SqlString, conn))
                    {
                        cmd.CommandType = CommandType.Text;
                        conn.Open();
                        cmd.ExecuteNonQuery();
                        return true;
                    }
                }
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public static bool Update(string sTable, string sItems, string sFilter)
        {
            try
            {
                string ConnString = MakeConString();
                string makeString = "UPDATE {0} SET {1} WHERE {2}";
                string SqlString = string.Format(makeString, sTable, sItems, sFilter);

                using (NpgsqlConnection conn = new NpgsqlConnection(ConnString))
                {
                    using (NpgsqlCommand cmd = new NpgsqlCommand(SqlString, conn))
                    {
                        cmd.CommandType = CommandType.Text;
                        conn.Open();
                        cmd.ExecuteNonQuery();
                        return true;
                    }
                }
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public static bool Delete(string sTable, string sFilter)
        {
            try
            {
                string ConnString = MakeConString();
                string makeString = "DELETE FROM {0} WHERE ({1})";
                string SqlString = string.Format(makeString, sTable, sFilter);

                using (NpgsqlConnection conn = new NpgsqlConnection(ConnString))
                {
                    using (NpgsqlCommand cmd = new NpgsqlCommand(SqlString, conn))
                    {
                        cmd.CommandType = CommandType.Text;
                        conn.Open();
                        cmd.ExecuteNonQuery();
                        return true;
                    }
                }
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public static DataSet Select(string sColumns, string sTables, string sBasicFilters = "", string sJoinFilters = "", string sOrder = "")
        {
            try
            {
                string makeString = "";
                string sqlString = "";
                DataSet dsResult = new DataSet();

                if (sBasicFilters != "")
                {
                    makeString = "SELECT {0} FROM {1} WHERE {2}";
                    sqlString = string.Format(makeString, sColumns, sTables, sBasicFilters);
                }

                if (sJoinFilters != "")
                {
                    makeString = "SELECT {0} {1} FROM {2} WHERE {2}";
                    sqlString = string.Format(makeString, sColumns, sJoinFilters, sTables, sBasicFilters);
                }

                if (sBasicFilters == "" && sBasicFilters == "" && sJoinFilters == "" && sOrder == "")
                {
                    makeString = "SELECT {0} FROM {1}";
                    sqlString = string.Format(makeString, sColumns, sTables);
                }

                if (sOrder != "")
                {
                    makeString = makeString + " ORDER BY {0}";
                    sqlString = string.Format(makeString, sOrder);
                }

                if (sqlString != "")
                {
                    string ConnString = MakeConString();
                    NpgsqlDataAdapter da = new NpgsqlDataAdapter(sqlString, ConnString);
                    da.Fill(dsResult);
                    return dsResult;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public static DataSet SelectSimple(string sQery)
        {
            try
            {
                DataSet dsResult = new DataSet();

                if (sQery != "")
                {
                    string ConnString = MakeConString();
                    NpgsqlDataAdapter da = new NpgsqlDataAdapter(sQery, ConnString);
                    da.Fill(dsResult);
                    return dsResult;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }
}
