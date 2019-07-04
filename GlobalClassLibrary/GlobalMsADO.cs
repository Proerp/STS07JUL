using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace Global.Class.Library
{
    public class GlobalMsADO
    {
        public static string ServerName = "(LOCAL)";
        public static string DatabaseName = "Northwind";


        public static string ConnectionString()
        { return GlobalMsADO.ConnectionString(GlobalMsADO.ServerName, GlobalMsADO.DatabaseName); }
        //{ return "Server = " + ServerName + "; Database= " + DatabaseName + "; Trusted_Connection= True"; } 

        public static string ConnectionString(string serverName, string databaseName)
        { return "Server = " + serverName + "; Database= " + databaseName + "; User Id=songthan;Password=songthan"; }
        //{ return "Server = " + ServerName + "; Database= " + DatabaseName + "; Trusted_Connection= True"; }



        public static string OcracleConnectionString()
        { return "Data Source=LMST;Persist Security Info=True;User ID=printer;Password=123;Unicode=True"; }

        private static SqlConnection mainDataAccessConnection;

        public static SqlConnection MainDataAccessConnection(bool openConnection)
        {
            try
            {
                if (mainDataAccessConnection == null || mainDataAccessConnection.State != ConnectionState.Open)
                {
                    mainDataAccessConnection = new SqlConnection(GlobalMsADO.ConnectionString() );
                    mainDataAccessConnection.Open();
                }

                return mainDataAccessConnection;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public static SqlConnection MainDataAccessConnection()
        {
            return mainDataAccessConnection;
        }
    }


}
