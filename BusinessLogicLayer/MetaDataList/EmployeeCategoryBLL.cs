using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Global.Class.Library;
using DataTransferObject;
using DataAccessLayer;



namespace BusinessLogicLayer
{
    public class EmployeeCategoryBLL
    {
        private EmployeeCategoryDTO employeeCategoryDTO;
        private EmployeeCategoryDAL employeeCategoryDAL;

        public static int ETaskID = (int)GlobalVariables.ENMVNTaskID.EEmployeeCategory;



        public EmployeeCategoryBLL()
        {
            employeeCategoryDTO = new EmployeeCategoryDTO();
            employeeCategoryDAL = new EmployeeCategoryDAL();
        }

        public int EmployeeCategoryID
        {
            get
            {
                return employeeCategoryDTO.EmployeeCategoryID;
            }
            set
            {
                if (value != employeeCategoryDTO.EmployeeCategoryID)
                {
                    employeeCategoryDTO = employeeCategoryDAL.GetEmployeeCategoryDTO(value);

                    //employeeCategoryDTO.EmployeeCategoryID = _employeeCategoryDTO.EmployeeCategoryID;
                    //employeeCategoryDTO.EmployeeCategoryName = _employeeCategoryDTO.EmployeeCategoryName;

                    //string l = employeeCategoryDTO.EmployeeCategoryName;
                    //this.EmployeeCategory.EmployeeCategoryName = "";
                    //this.EmployeeCategory.EmployeeCategoryName = l;
                }
            }
        }

        public EmployeeCategoryDTO EmployeeCategory
        {
            get { return employeeCategoryDTO; }

        }



        //test

        public string EmployeeCategoryName
        {
            get { return employeeCategoryDTO.EmployeeCategoryName; }
        }

        //endtest





        ////private CategoryDao categories;

        //public CategoryBus() { categories = new CategoryDao(); }

        public IList<EmployeeCategoryDTO> GetAllEmployeeCategory()
        {
            return employeeCategoryDAL.GetEmployeeCategoryDTO();
        }



        //public void InsertCategory(Category cat)
        //{
        //    categories.InsertCategory(cat);
        //}

        //public int UpdateCategory(Category cat)
        //{
        //    return categories.UpdateCategory(cat);
        //}

        //public int DeleteCategory(Category cat)
        //{
        //    return categories.DeleteCategory(cat);
        //}
    }
}
