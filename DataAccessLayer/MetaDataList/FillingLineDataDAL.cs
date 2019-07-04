using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;

using Global.Class.Library;
using DataTransferObject;

namespace DataAccessLayer
{
    class FillingLineDataDAL
    {
        //public IList<FillingLineData> GetFillingLineData(GlobalVariables.FillingLine fillingLineID)
        //{
        //    DataTable dt = ADODatabase.GetDataTable("SELECT * FROM ListFillingLineName", (int)ADODatabase.SqlType.Text);
        //    return MakeFillingLineData(dt);
        //}

        //public FillingLineData GetFillingLineData(int employeeCategoryID)
        //{
        //    SqlParameter[] sqlParameter = { new SqlParameter("@CategoryId", SqlDbType.Int) };
        //    sqlParameter[0].Value = employeeCategoryID;

        //    //DataTable dataTable = ADODatabase.GetDataTable("SELECT * FROM ListFillingLineName WHERE EmployeeCategoryID = ?", sqlParameter);
        //    DataTable dataTable = ADODatabase.GetDataTable("SELECT * FROM ListFillingLineName WHERE EmployeeCategoryID = " + employeeCategoryID.ToString(), (int)ADODatabase.SqlType.Text);
        //    return MakeFillingLineData(dataTable.Rows[0]);
        //}


        //private IList<FillingLineData> MakeFillingLineData(DataTable dataTable)
        //{
        //    IList<FillingLineData> FillingLineDataList = new List<FillingLineData>();
        //    foreach (DataRow dataRow in dataTable.Rows)
        //    {
        //        FillingLineDataList.Add(MakeFillingLineData(dataRow));
        //    }
        //    return FillingLineDataList;
        //}

        //private FillingLineData MakeFillingLineData(DataRow dataRow)
        //{
        //    FillingLineData FillingLineData = new FillingLineData();

        //    //FillingLineData.EmployeeCategoryID = int.Parse(dataRow["EmployeeCategoryID"].ToString());
        //    ////FillingLineData.EmployeeCategoryName = dataRow["EmployeeCategoryName"].ToString();
        //    //FillingLineData.EmployeeCategoryName = dataRow["Description"].ToString();
        //    //FillingLineData.Description = dataRow["Description"].ToString();
        //    //FillingLineData.SerialName = dataRow["SerialName"].ToString();
        //    //FillingLineData.Remarks = dataRow["Remarks"].ToString();

        //    return FillingLineData;
        //}

    }
}
