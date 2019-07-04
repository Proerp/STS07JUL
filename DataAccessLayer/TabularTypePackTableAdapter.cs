using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using DataAccessLayer.DataDetailTableAdapters;


namespace DataAccessLayer
{
    public class TabularTypePackTableAdapter: DataDetailPackTableAdapter
    {
        public TabularTypePackTableAdapter()
        {
            SqlCommand[] sqlCommandArray = base.CommandCollection;
            foreach (SqlCommand sqlCommand in sqlCommandArray)
            {
                foreach (SqlParameter sqlParameter in sqlCommand.Parameters)
                {
                    if (sqlParameter.ParameterName == "@PackIDs" && sqlParameter.DbType == System.Data.DbType.Object)
                    {
                        sqlParameter.TypeName = "TabularType";
                    }
                }
            }
        }
    }
}
