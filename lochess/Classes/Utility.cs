using lochess.Models;
using Microsoft.Data.SqlClient;
using MySql.Data.MySqlClient;
using MySqlX.XDevAPI.Relational;
using System.Configuration;
using System.Data;

namespace lochess.Classes
{
    // This class stores public utility/helper methods 
    public class Utility
    {
        // This is a general MySQL querying helper method that simplifies calls to the DB
        public static DataRowCollection SelectMySqlData(string tableConnectionString, string commandString, IConfiguration configuration)
        {
            DataRowCollection rows;
            try
            {
                using (MySqlConnection conn = new MySqlConnection(configuration.GetConnectionString("MySqlConnection")))
                {
                    conn.Open();

                    MySqlCommand comm = conn.CreateCommand();
                    comm.Connection = conn;
                    comm.CommandText = commandString;
                    MySqlDataAdapter adapter = new MySqlDataAdapter(comm);
                    DataSet dataset = new DataSet();
                    adapter.Fill(dataset, tableConnectionString);
                    rows = dataset.Tables[tableConnectionString].Rows;

                    conn.Close();
                }
                return rows;
            }
            catch (Exception e)
            {
                // TODO: Figure out better way to throw exception to display on the view
                Console.WriteLine("An error occured: " + e.Message);
                throw e;
                return null;
            }
        }

        public static bool InsertMySqlData(string commandString, List<MySqlParameter> commParameters, IConfiguration configuration)
        {
            using (MySqlConnection conn = new MySqlConnection(configuration.GetConnectionString("MySqlConnection")))
            {
                conn.Open();

                MySqlCommand comm = conn.CreateCommand();
                MySqlTransaction myTrans = conn.BeginTransaction();
                comm.Connection = conn;
                comm.Transaction = myTrans;

                try
                {
                    comm.CommandText = commandString;
                    comm.Parameters.AddRange(commParameters.ToArray<MySqlParameter>());
                    int result = comm.ExecuteNonQuery();
                    myTrans.Commit();
                }
                catch (Exception e)
                {
                    try
                    {
                        myTrans.Rollback();
                        return false;
                    }
                    catch (Exception exRollBack)
                    {
                        return false;
                    }
                    throw e;
                }

                conn.Close();
            }
            return true;
        }
    }
}
