using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using Global.Class.Library;


namespace DataAccessLayer
{
    public class SQLDatabase
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





        /// <summary>
        /// Create a new store procedure
        /// </summary>
        /// <param name="storedProcedureName"></param>
        /// <param name="queryString"></param>
        public static void CreateStoredProcedure(string storedProcedureName, string queryString)
        {
            DataTable dataTable = SQLDatabase.GetDataTable("SELECT name FROM sysobjects WHERE name = '" + storedProcedureName + "' AND type = 'P'");
            if (dataTable.Rows.Count > 0) SQLDatabase.ExecuteNonQuery("DROP PROCEDURE " + storedProcedureName);

            SQLDatabase.ExecuteNonQuery("CREATE PROC " + storedProcedureName + "\r\n" + queryString);
        }


        /// <summary>
        /// Create new stored procedure to check a pecific existing, for example: Editable, Revisable, ...
        /// </summary>
        /// <param name="storedProcedureName"></param>
        /// <param name="parameterString"></param>
        /// <param name="queryArray"></param>
        public static void CreateProcedureToCheckExisting(string storedProcedureName, string[] queryArray)
        {
            string queryString = "";

            queryString = " @FindIdentityID int " + "\r\n";
            queryString = queryString + " WITH ENCRYPTION " + "\r\n";
            queryString = queryString + " AS " + "\r\n";
            queryString = queryString + "   BEGIN " + "\r\n";

            queryString = queryString + "       DECLARE @ExistIdentityID Int " + "\r\n";
            queryString = queryString + "       SET @ExistIdentityID = -1 " + "\r\n";

            if (queryArray != null)
            {
                foreach (string subQuery in queryArray)
                {
                    queryString = queryString + "       DECLARE CursorLocal CURSOR LOCAL FOR " + subQuery + "\r\n";
                    queryString = queryString + "       OPEN CursorLocal " + "\r\n";
                    queryString = queryString + "       FETCH NEXT FROM CursorLocal INTO @ExistIdentityID " + "\r\n";
                    queryString = queryString + "       CLOSE CursorLocal " + "\r\n";
                    queryString = queryString + "       DEALLOCATE CursorLocal " + "\r\n";
                    queryString = queryString + "       IF @ExistIdentityID != -1  RETURN @ExistIdentityID " + "\r\n";
                }
            }

            queryString = queryString + "       RETURN @ExistIdentityID " + "\r\n";
            queryString = queryString + "   END " + "\r\n";

            SQLDatabase.CreateStoredProcedure(storedProcedureName, queryString);
        }

        public static void CreateProcedureToCheckExisting(string storedProcedureName)
        {
            SQLDatabase.CreateProcedureToCheckExisting(storedProcedureName, null);
        }





        public static int GetScalarValue(string queryString, CommandType commandType, IDataParameter[] dataParameter)
        {
            using (SqlConnection sqlConnection = new SqlConnection(GlobalMsADO.ConnectionString()))
            {
                SqlCommand sqlCommand = new SqlCommand(queryString, sqlConnection);
                sqlCommand.CommandType = commandType;
                if (dataParameter != null) sqlCommand.Parameters.AddRange(dataParameter);

                sqlConnection.Open();


                int returnValue;
                object returnObject = sqlCommand.ExecuteScalar();
                if (returnObject != DBNull.Value && returnObject != null && int.TryParse(returnObject.ToString(), out returnValue))
                    return (int)returnValue;
                else return -1;
            }
        }

        public static int GetScalarValue(string queryString, CommandType commandType)
        {
            return SQLDatabase.GetScalarValue(queryString, commandType, null);
        }

        public static int GetScalarValue(string queryString)
        {
            return SQLDatabase.GetScalarValue(queryString, CommandType.Text, null);
        }





        public static int GetReturnValue(string queryString, CommandType commandType, IDataParameter[] dataParameter)
        {
            using (SqlConnection sqlConnection = new SqlConnection(GlobalMsADO.ConnectionString()))
            {
                SqlCommand sqlCommand = new SqlCommand(queryString, sqlConnection);
                sqlCommand.CommandType = commandType;
                if (dataParameter != null) sqlCommand.Parameters.AddRange(dataParameter);


                sqlConnection.Open();


                SqlParameter returnParameter = sqlCommand.Parameters.Add("@ReturnValue", SqlDbType.Int);
                returnParameter.Direction = ParameterDirection.ReturnValue;

                sqlCommand.ExecuteNonQuery();
                return (int)returnParameter.Value;
            }
        }

        public static int GetReturnValue(string queryString, CommandType commandType)
        {
            return SQLDatabase.GetReturnValue(queryString, commandType, null);
        }

        public static int GetReturnValue(string queryString)
        {
            return SQLDatabase.GetReturnValue(queryString, CommandType.Text, null);
        }

        #region Get Datatable
        /// <summary>
        /// GetDataTable
        /// </summary>
        /// <param name="queryString"></param>
        /// <returns></returns>
        public static DataTable GetDataTable(string queryString, CommandType commandType, IDataParameter[] dataParameter)
        {
            using (SqlConnection sqlConnection = new SqlConnection(GlobalMsADO.ConnectionString()))
            {
                SqlCommand sqlCommand = new SqlCommand(queryString, sqlConnection);
                sqlCommand.CommandType = commandType;
                if (dataParameter != null) sqlCommand.Parameters.AddRange(dataParameter);

                using (SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(sqlCommand))
                {
                    DataTable dataTable = new DataTable();
                    sqlDataAdapter.MissingSchemaAction = MissingSchemaAction.AddWithKey;
                    sqlDataAdapter.Fill(dataTable);
                    return dataTable;
                }
            }
        }

        public static DataTable GetDataTable(string queryString, CommandType commandType)
        {
            return SQLDatabase.GetDataTable(queryString, commandType, null);
        }

        public static DataTable GetDataTable(string queryString)
        {
            return SQLDatabase.GetDataTable(queryString, CommandType.Text, null);
        }

        #endregion Get Datatable




        //BELOW IS OK STATEMENT ----------------------------------------------

        public static int ExecuteNonQuery(string commandText)
        {
            return SQLDatabase.ExecuteNonQuery(commandText, CommandType.Text);
        }


        public static int ExecuteNonQuery(string commandText, CommandType commandType)
        {
            Console.WriteLine(commandText);
            using (SqlConnection sqlConnection = new SqlConnection(GlobalMsADO.ConnectionString()))
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
            return SQLDatabase.ExecuteTransaction(commandText, CommandType.Text);
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




        public static void UpdateNullableColumns(int dataMessageID, int splitDataMessageID)
        {
            using (SqlConnection sqlConnection = new SqlConnection(GlobalMsADO.ConnectionString()))
            {
                try
                {
                    sqlConnection.Open(); //According to SONG THAN USER: This following code is used to update these NULLABLE column(s) of table DataMessageMaster (These NULLABLE column(s) are created by SONG THAN USER)


                    DataSet dataSetDataMessageMaster = new DataSet();
                    SqlDataAdapter sqlDataAdapterDataMessage;
                    SqlDataAdapter sqlDataAdapterSplitDataMessage;
                    SqlCommandBuilder sqlCommandBuilder;


                    sqlDataAdapterDataMessage = new SqlDataAdapter("SELECT * FROM DataMessageMaster WHERE DataMessageID = " + dataMessageID, sqlConnection);
                    sqlDataAdapterDataMessage.Fill(dataSetDataMessageMaster, "DataMessageMaster"); //Fill the base datatable


                    //Initialize the SqlDataAdapter object by specifying a Select command that retrieves data from the DataMessageMaster table.
                    sqlDataAdapterSplitDataMessage = new SqlDataAdapter("SELECT * FROM DataMessageMaster WHERE DataMessageID = " + splitDataMessageID, sqlConnection);

                    //Initialize the SqlCommandBuilder object to automatically generate and initialize the UpdateCommand, InsertCommand, and DeleteCommand properties of the SqlDataAdapter.
                    sqlCommandBuilder = new SqlCommandBuilder(sqlDataAdapterSplitDataMessage);

                    //Populate the DataSet by running the Fill method of the SqlDataAdapter.
                    sqlDataAdapterSplitDataMessage.Fill(dataSetDataMessageMaster, "SplitDataMessageMaster");


                    bool hasUpdateNullableColumn = false;
                    foreach (DataColumn dataColumn in dataSetDataMessageMaster.Tables["SplitDataMessageMaster"].Columns)
                    {
                        if (dataSetDataMessageMaster.Tables["SplitDataMessageMaster"].Rows[0].IsNull(dataColumn.ColumnName))
                        {
                            dataSetDataMessageMaster.Tables["SplitDataMessageMaster"].Rows[0][dataColumn.ColumnName] = dataSetDataMessageMaster.Tables["DataMessageMaster"].Rows[0][dataColumn.ColumnName];
                            hasUpdateNullableColumn = true;
                        }
                    }


                    if (hasUpdateNullableColumn)
                        sqlDataAdapterSplitDataMessage.Update(dataSetDataMessageMaster, "SplitDataMessageMaster"); //Post the data modification to the database.

                }
                catch (Exception exception)
                {
                    throw exception;
                }
            }
        }











    }
}

//Check these link for implement transactional save
//http://msdn.microsoft.com/en-us/library/ms172152.aspx
//http://msdn.microsoft.com/en-us/library/system.transactions.transactionscope.aspx
//http://msdn.microsoft.com/en-us/library/ms229973.aspx

//http://msdn.microsoft.com/en-us/library/ms229973.aspx
//http://msdn.microsoft.com/en-us/library/ms233770.aspx




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




//Very good code --- Should we foolow this
//http://rajmsdn.wordpress.com/2009/12/09/strongly-typed-dataset-connection-string/
//http://rajmsdn.wordpress.com/2009/12/09/strongly-typed-dataset-connection-string/confighack-c-2/
//http://rajmsdn.files.wordpress.com/2009/12/confighack-c.pdf
