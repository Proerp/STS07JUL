using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using Global.Class.Library;
using System.Data.OracleClient;

namespace DataAccessLayer
{
    public class OcracleDatabase
    {
        public static int ExecuteNonQuery(string commandText)
        {
            return SQLDatabase.ExecuteNonQuery(commandText, CommandType.Text);
        }


        public static int ExecuteNonQuery(string commandText, CommandType commandType)
        {
            Console.WriteLine(commandText);
            using (OracleConnection sqlConnection = new OracleConnection(GlobalMsADO.ConnectionString()))
            {
                try
                {
                    sqlConnection.Open();

                    OracleCommand sqlCommand = new OracleCommand(commandText, sqlConnection);
                    sqlCommand.CommandType = commandType;

                    return sqlCommand.ExecuteNonQuery();
                }
                catch (Exception exception)
                {
                    throw exception;
                }
            }
        }

    }
}
