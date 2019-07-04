using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using BusinessLogicLayer;

namespace PresentationLayer
{
    public partial class EmployeeCategory : Form
    {

        private EmployeeCategoryBLL employeeCategoryBLL;
        public EmployeeCategory()
        {
            InitializeComponent();
            employeeCategoryBLL = new EmployeeCategoryBLL();
        }

        private void FormEmployeeCategory_Load(object sender, EventArgs e)
        {
            this.dataGridView1.DataSource = employeeCategoryBLL.GetAllEmployeeCategory();


            employeeCategoryBLL.EmployeeCategoryID = 3;


            //this.textBox1.DataBindings.Add("Text", employeeCategoryBLL, "EmployeeCategoryName");


            this.textBox1.DataBindings.Add("Text", employeeCategoryBLL.EmployeeCategory, "EmployeeCategoryName");//,true,DataSourceUpdateMode.OnPropertyChanged)



            this.textBox2.DataBindings.Add("Text", employeeCategoryBLL.EmployeeCategory, "SerialName");
            this.textBox3.DataBindings.Add("Text", employeeCategoryBLL.EmployeeCategory, "Description");
            this.textBox4.DataBindings.Add("Text", employeeCategoryBLL.EmployeeCategory, "Remarks");



            //NumberFormatInfo nfi = new NumberFormatInfo();
            //nfi.NumberDecimalDigits = 0;
            //this.textBox7.DataBindings.Add("Text", a, "Age", true, DataSourceUpdateMode.OnValidation, 0, "N", nfi);


        }

        private void button1_Click(object sender, EventArgs e)
        {
            //employeeCategoryBLL.EmployeeCategory.EmployeeCategoryName = employeeCategoryBLL.EmployeeCategory.EmployeeCategoryName + "HH";
            //employeeCategoryBLL.EmployeeCategory.SerialName = employeeCategoryBLL.EmployeeCategory.SerialName + "HH";

            employeeCategoryBLL.EmployeeCategoryID = employeeCategoryBLL.EmployeeCategoryID + 1;
            this.textBox1.DataBindings.RemoveAt(0);
            this.textBox1.DataBindings.Add("Text", employeeCategoryBLL.EmployeeCategory, "EmployeeCategoryName");

            //employeeCategoryBLL.EmployeeCategory.ABC();

            //employeeCategoryBLL.EmployeeCategory.SerialName = employeeCategoryBLL.EmployeeCategory.SerialName + "HH";
        }
    }
}
