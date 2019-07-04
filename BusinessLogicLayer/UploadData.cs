using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Data;
using System.Reflection;
using System.Transactions;

using Global.Class.Library;
using DataAccessLayer;
using DataAccessLayer.ListMaintenanceTableAdapters;

namespace BusinessLogicLayer
{
    public class UploadData
    {

        private ListMaintenance.PublicTableUploadDataTable publicTableUploadDataTable;
        public ListMaintenance.PublicTableUploadDataTable PublicTableUploadDataTable
        {
            get
            {
                if (publicTableUploadDataTable == null)
                {
                    PublicTableUploadTableAdapter fillingLineNameTableAdapter = new PublicTableUploadTableAdapter();
                    publicTableUploadDataTable = fillingLineNameTableAdapter.GetData(); ;
                }

                return publicTableUploadDataTable;
            }
        }



        public void DownLoadDataMaster()
        {
            try
            {
                foreach (ListMaintenance.PublicTableUploadRow publicTableUploadRow in this.PublicTableUploadDataTable.Rows)
                {
                    string tableName = (string)publicTableUploadRow["ColumnName"];
                    string listOfColumn = this.GetListOfColumn(tableName);

                    DataTable dataTable = SQLDatabase.GetDataTable("SELECT " + listOfColumn + " FROM " + tableName + " WHERE EntryStatusID <> " + GlobalEnum.EntryStatusID.IsUploaded);

                    foreach (DataRow dataRow in dataTable.Rows)
                    {
                        //if ((int)dataRow["EntryStatusID"] == (int)GlobalEnum.EntryStatusID.IsEdited)
                        //OcracleDatabase.ExecuteNonQuery("DELETE FROM " + tableName + " WHERE EntryStatusID = " + GlobalEnum.EntryStatusID.IsEdited);
                        //OcracleDatabase.ExecuteNonQuery("INSERT INTO " + tableName + " () + )
                    }
                        
                    
                }
            }
            catch (System.Exception exception)
            {
                throw exception;
            }
        }


        private string GetListOfColumn(string tableName)
        {
            try
            {
                string listOfColumn = "";
                DataTable dataTable = SQLDatabase.GetDataTable("SELECT COLUMN_NAME FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = '" + tableName + "' ORDER BY ORDINAL_POSITION ASC");
                foreach (DataRow dataRow in dataTable.Rows)
                {
                    listOfColumn = listOfColumn + listOfColumn != "" ? ", " : "" + (string)dataRow["COLUMN_NAME"];
                }
                return listOfColumn;
            }
            catch (System.Exception exception)
            {
                throw exception;
            }
        }

    }
}
