using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Data.SqlClient;
using System.Data;

using System.Configuration;
using ConfigurationManager = System.Configuration.ConfigurationManager;

namespace FantasyPL.Pages
{
    public class DBManager
    {

        static string DB_Connection_String = "Data Source=\"fantasyplserver.database.windows.net, 1433\";Initial Catalog = FantasyPLdb; Persist Security Info=True;User ID = Hayakel; Password=12345678#a";

        public SqlConnection myConnection;

        public DBManager()
        {
            myConnection = new SqlConnection(DB_Connection_String);
            try
            {
                myConnection.Open(); //Open a connection with the DB

                // just for illustration when the database is opened, 
                // this should NOT be shown in GUI to the user in the final application
                // but we show it here only to make sure that the database is working
                //MessageBox.Show("Successfully connected to the database!");
            }
            catch (Exception e)
            {
                // this message should not appear to user in the final application
                Console.WriteLine("Exception: " + e.Message);
            }
        }

        public int ExecuteNonQuery(SqlCommand myCommand)
        {
            try
            {
                return myCommand.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                // this message should not appear to user in the final application
                Console.WriteLine("Exception: " + ex.Message);
                return 0;
            }
        }

        public SqlDataReader ExecuteReader(SqlCommand myCommand)

        {
            try
            {
                SqlDataReader reader = myCommand.ExecuteReader();
                return reader;
            }
            catch (Exception ex)
            {
                // this message should not appear to user in the final application
                Console.WriteLine("Exception: " + ex.Message);
                return null;
            }
        }

        public object ExecuteScalar(string query)
        {
            try
            {
                SqlCommand myCommand = new SqlCommand(query, myConnection);
                return myCommand.ExecuteScalar();
            }
            catch (Exception ex)
            {
                // this message should not appear to user in the final application
                Console.WriteLine("Exception: " + ex.Message);
                return 0;
            }
        }

        public void CloseConnection()
        {
            try
            {
                myConnection.Close();
            }
            catch (Exception e)
            {
                // this message should not appear to user in the final application
                Console.WriteLine("Exception: " + e.Message);
            }
        }

    }
}
;
