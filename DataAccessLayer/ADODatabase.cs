using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using Global.Class.Library;

namespace DataAccessLayer
{
    public class ADODatabase
    {

        public enum SqlType : int
        {
            StoredProcedure = 1,
            Text = 2
        }

        //public static string connectionString = "Server= .;Database= Northwind; Trusted_Connection= True";

        public static SqlConnection Connection()
        {
            return GlobalMsADO.MainDataAccessConnection();
            ////////SqlConnection connection = new SqlConnection(connectionString);
            ////////return connection;
        }


        public static DataTable GetDataTable(string query)
        {
            return ADODatabase.GetDataTable(query, CommandType.Text);
        }


        public static DataTable GetDataTable(string commandText, CommandType commandType)
        {
            SqlCommand sqlCommand = new SqlCommand(commandText, ADODatabase.Connection());
            sqlCommand.CommandType = commandType;

            using (SqlDataAdapter adapter = new SqlDataAdapter(sqlCommand))
            {
                DataTable dataTable = new DataTable();
                adapter.Fill(dataTable);
                return dataTable;
            }
        }


        public static DataTable GetDataTable(string commandText, IDataParameter[] dataParameter)
        {
            SqlCommand sqlCommand = new SqlCommand();
            sqlCommand = AddParameter(dataParameter, commandText);
            using (SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(sqlCommand))
            {
                DataTable dataTable = new DataTable();
                sqlDataAdapter.MissingSchemaAction = MissingSchemaAction.AddWithKey;
                sqlDataAdapter.Fill(dataTable);
                return dataTable;
            }

        }






        public static int InsertData(string sql, IDataParameter[] parameter)
        {
            SqlCommand command = new SqlCommand();
            int id = -1;
            command = AddParameter(parameter, sql);
            command.ExecuteNonQuery();
            return id;
        }



        public static int UpdateData(string sql, IDataParameter[] parameter)
        {
            SqlCommand command = new SqlCommand();
            command = AddParameter(parameter, sql);
            return command.ExecuteNonQuery();
        }

        public static SqlCommand AddParameter(IDataParameter[] objectParameter, string storeName)
        {
            SqlConnection conn = Connection();
            SqlCommand command = new SqlCommand();
            if (conn.State == 0) { conn.Open(); }
            command.Connection = conn;
            command.CommandType = CommandType.StoredProcedure;
            command.CommandText = storeName;
            //đưa tham số vào
            foreach (SqlParameter item in objectParameter)
            {
                command.Parameters.Add(item);
            }
            return command;
        }



        //BELOW IS OK STATEMENT ----------------------------------------------

        public static int ExecuteNonQuery(string commandText)
        {
            return ADODatabase.ExecuteNonQuery(commandText, CommandType.Text);
        }

        
        public static int ExecuteNonQuery(string commandText, CommandType commandType)
        {
            return ADODatabase.ExecuteNonQuery(commandText, commandType, GlobalMsADO.ConnectionString());
        }

        public static int ExecuteNonQuery(string commandText, string connectionString)
        {
            return ADODatabase.ExecuteNonQuery(commandText, CommandType.Text, connectionString);
        }

        public static int ExecuteNonQuery(string commandText, CommandType commandType, string connectionString)
        {
            Console.WriteLine(commandText);
            using (SqlConnection sqlConnection = new SqlConnection(connectionString))
            {
                try
                {
                    sqlConnection.Open();

                    SqlCommand sqlCommand = new SqlCommand(commandText, sqlConnection);
                    sqlCommand.CommandType = commandType;

                    return sqlCommand.ExecuteNonQuery();
                }
                catch (Exception exception)
                {
                    throw exception;
                }
            }
        }


        public static int ExecuteTransaction(string commandText)
        {
            return ADODatabase.ExecuteTransaction(commandText, CommandType.Text);
        }

        public static int ExecuteTransaction(string commandText, CommandType commandType)
        {
            using (SqlConnection sqlConnection = new SqlConnection(GlobalMsADO.ConnectionString()))
            {
                int rowsAffected = -1;
                SqlTransaction sqlTransaction = null;
                try
                {
                    sqlConnection.Open();
                    sqlTransaction = sqlConnection.BeginTransaction("MyTransaction");


                    //Begin add command here
                    using (SqlCommand sqlCommand = new SqlCommand(commandText, sqlConnection, sqlTransaction))
                    {
                        sqlCommand.CommandType = commandType;
                        rowsAffected = sqlCommand.ExecuteNonQuery();
                    }
                    //End add command here


                    sqlTransaction.Commit();

                    return rowsAffected;

                }
                catch (Exception exception)
                {
                    try
                    {
                        sqlTransaction.Rollback();
                        throw exception;
                    }
                    catch (Exception rollbackException)
                    {
                        throw rollbackException;
                    }
                }
            }
        }








        public static bool CreateFunction(string functionName, string sqlQuery)
        {
            DataTable dataTable = ADODatabase.GetDataTable("SELECT name FROM sysobjects WHERE name = '" + functionName + "' AND (type = 'FN' OR type = 'TF') ");
            if (dataTable.Rows.Count > 0) ADODatabase.ExecuteNonQuery("DROP FUNCTION " + functionName);

            ADODatabase.ExecuteNonQuery("CREATE FUNCTION " + functionName + char.ConvertFromUtf32(13) + sqlQuery);

            return true;
        }



        public static bool CreateFunction_FNSplitUpIds()
        {
            string sqlQuery = "";
            try
            {
                sqlQuery = " (@Param_Ids varchar(5000)) RETURNS @Id_Table TABLE(IDField int) ";
                sqlQuery = sqlQuery + " WITH ENCRYPTION " + char.ConvertFromUtf32(13);
                sqlQuery = sqlQuery + " AS " + char.ConvertFromUtf32(13);

                sqlQuery = sqlQuery + "   BEGIN    " + char.ConvertFromUtf32(13);
                sqlQuery = sqlQuery + "       IF (LEN(@Param_Ids) <= 0)   " + char.ConvertFromUtf32(13);
                sqlQuery = sqlQuery + "           RETURN  " + char.ConvertFromUtf32(13);

                sqlQuery = sqlQuery + "       DECLARE @CommaPos smallint  " + char.ConvertFromUtf32(13);
                sqlQuery = sqlQuery + "       SET @CommaPos = CHARINDEX(';', RTRIM(LTRIM(@Param_Ids)))    " + char.ConvertFromUtf32(13);

                sqlQuery = sqlQuery + "       IF @CommaPos = 0  " + char.ConvertFromUtf32(13);
                sqlQuery = sqlQuery + "           INSERT INTO @Id_Table VALUES(CONVERT(BIGINT, RTRIM(LTRIM(@Param_Ids))))  " + char.ConvertFromUtf32(13);
                sqlQuery = sqlQuery + "       ELSE   " + char.ConvertFromUtf32(13);
                sqlQuery = sqlQuery + "           BEGIN  " + char.ConvertFromUtf32(13);
                sqlQuery = sqlQuery + "               WHILE LEN(@Param_Ids) > 1  " + char.ConvertFromUtf32(13);
                sqlQuery = sqlQuery + "                   BEGIN  " + char.ConvertFromUtf32(13);
                sqlQuery = sqlQuery + "                       SET @CommaPos = CHARINDEX(';', RTRIM(LTRIM(@Param_Ids)))  " + char.ConvertFromUtf32(13);
                sqlQuery = sqlQuery + "                       INSERT INTO @Id_Table VALUES(CONVERT(INT ,SUBSTRING(RTRIM(LTRIM(@Param_Ids)),1, @CommaPos - 1))) " + char.ConvertFromUtf32(13);
                sqlQuery = sqlQuery + "                       SET @Param_Ids = SUBSTRING(RTRIM(LTRIM(@Param_Ids)), @CommaPos + 1 , LEN(RTRIM(LTRIM(@Param_Ids))))  " + char.ConvertFromUtf32(13);
                sqlQuery = sqlQuery + "                       SET @CommaPos = CHARINDEX(';', RTRIM(LTRIM(@Param_Ids)))  " + char.ConvertFromUtf32(13);
                sqlQuery = sqlQuery + "                       IF @CommaPos = 0  " + char.ConvertFromUtf32(13);
                sqlQuery = sqlQuery + "                           BEGIN " + char.ConvertFromUtf32(13);
                sqlQuery = sqlQuery + "                               INSERT INTO @Id_Table VALUES(CONVERT(INT ,RTRIM(LTRIM(@Param_Ids))))  " + char.ConvertFromUtf32(13);
                sqlQuery = sqlQuery + "                               BREAK  " + char.ConvertFromUtf32(13);
                sqlQuery = sqlQuery + "                           END  " + char.ConvertFromUtf32(13);
                sqlQuery = sqlQuery + "                   END  " + char.ConvertFromUtf32(13);
                sqlQuery = sqlQuery + "           END  " + char.ConvertFromUtf32(13);
                sqlQuery = sqlQuery + "       RETURN  " + char.ConvertFromUtf32(13);
                sqlQuery = sqlQuery + "   END " + char.ConvertFromUtf32(13);

                return CreateFunction("FNSplitUpIds", sqlQuery);
            }
            catch
            {
                return false;
            }
        }





    }
}






//Later: We should study about this, to improve code, with ADO.NET
//3 layer application using ADO.NET  - Should study. THIS IS A MUST
//http://msdn.microsoft.com/en-us/library/aa581771.aspx
//http://msdn.microsoft.com/en-us/library/bb288041.aspx


//Should this about this (This is official guide line from Microsoft
//http://msdn.microsoft.com/en-us/library/system.data.dataset.aspx

//DataSet Class

//In a typical multiple-tier implementation, the steps for creating and refreshing a DataSet, and in turn, updating the original data are to:
//1.Build and fill each DataTable in a DataSet with data from a data source using a DataAdapter.
//2.Change the data in individual DataTable objects by adding, updating, or deleting DataRow objects.
//3.Invoke the GetChanges method to create a second DataSet that features only the changes to the data.
//4.Call the Update method of the DataAdapter, passing the second DataSet as an argument.
//5.Invoke the Merge method to merge the changes from the second DataSet into the first.
//6.Invoke the AcceptChanges on the DataSet. Alternatively, invoke RejectChanges to cancel the changes.