using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;

using DataTransferObject;

namespace DataAccessLayer
{
    public class EmployeeCategoryDAL
    {

        public IList<EmployeeCategoryDTO> GetEmployeeCategoryDTO()
        {
            //DataTable dt = ADODatabase.GetDataTable("Get_All_Categorys", (int)ADODatabase.SqlType.StoredProcedure);
            DataTable dt = ADODatabase.GetDataTable("SELECT * FROM ListEmployeeCategory");
            return MakeEmployeeCategoryDTO(dt);
        }

        public EmployeeCategoryDTO GetEmployeeCategoryDTO(int employeeCategoryID)
        {
            SqlParameter[] sqlParameter = { new SqlParameter("@CategoryId", SqlDbType.Int) };
            sqlParameter[0].Value = employeeCategoryID;

            //DataTable dataTable = ADODatabase.GetDataTable("SELECT * FROM ListEmployeeCategory WHERE EmployeeCategoryID = ?", sqlParameter);
            DataTable dataTable = ADODatabase.GetDataTable("SELECT * FROM ListEmployeeCategory WHERE EmployeeCategoryID = " + employeeCategoryID.ToString());
            return MakeEmployeeCategoryDTO(dataTable.Rows[0]);

        }


        private IList<EmployeeCategoryDTO> MakeEmployeeCategoryDTO(DataTable dataTable)
        {
            IList<EmployeeCategoryDTO> employeeCategoryDTOList = new List<EmployeeCategoryDTO>();
            foreach (DataRow dataRow in dataTable.Rows)
            {
                employeeCategoryDTOList.Add(MakeEmployeeCategoryDTO(dataRow));
            }
            return employeeCategoryDTOList;
        }

        private EmployeeCategoryDTO MakeEmployeeCategoryDTO(DataRow dataRow)
        {
            EmployeeCategoryDTO employeeCategoryDTO = new EmployeeCategoryDTO();

            employeeCategoryDTO.EmployeeCategoryID = int.Parse(dataRow["EmployeeCategoryID"].ToString());
            //employeeCategoryDTO.EmployeeCategoryName = dataRow["EmployeeCategoryName"].ToString();
            employeeCategoryDTO.EmployeeCategoryName = dataRow["Description"].ToString();
            employeeCategoryDTO.Description = dataRow["Description"].ToString();
            employeeCategoryDTO.SerialName = dataRow["SerialName"].ToString();
            employeeCategoryDTO.Remarks = dataRow["Remarks"].ToString();

            return employeeCategoryDTO;
        }

    }
}
