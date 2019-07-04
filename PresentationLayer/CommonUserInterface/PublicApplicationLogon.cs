using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using Global.Class.Library;
using DataAccessLayer;
using BusinessLogicLayer;

namespace PresentationLayer
{
    public partial class PublicApplicationLogon : Form
    {
        public PublicApplicationLogon()
        {
            InitializeComponent();
        }


        public int EmployeeID { get; set; }
        private Binding employeeIDBinding;
        private CommonMetaList commonMetaList; 


        private void PublicApplicationLogon_Load(object sender, EventArgs e)
        {
            try
            {
                this.comboBoxImageS8PortName.DataSource = System.IO.Ports.SerialPort.GetPortNames();
                this.comboBoxAutonicsPortName.DataSource = System.IO.Ports.SerialPort.GetPortNames();

                if (this.comboBoxImageS8PortName.Items.Count == 0)
                {
                    this.comboBoxImageS8PortName.DataSource = null;
                    this.comboBoxImageS8PortName.Items.Add("COM 0");

                    this.comboBoxAutonicsPortName.DataSource = null;
                    this.comboBoxAutonicsPortName.Items.Add("COM 0");
                }

                DataTable dataTablePublicPrinterProperties = SQLDatabase.GetDataTable("SELECT TOP 1 * FROM PublicPrinterProperties");
                if (dataTablePublicPrinterProperties.Rows.Count > 0)
                {
                    this.comboBoxImageS8PortName.Text = (string)dataTablePublicPrinterProperties.Rows[0]["ImageS8PortName"];
                    this.comboBoxAutonicsPortName.Text = (string)dataTablePublicPrinterProperties.Rows[0]["AutonicsPortName"];
                }


                string stringEmployeeID = GlobalRegistry.Read("EmployeeID"); int employeeID = -1;

                if (stringEmployeeID == null || stringEmployeeID.Length <= "string".Length || !int.TryParse(stringEmployeeID.Substring("string".Length), out employeeID)) employeeID = 1;
                this.EmployeeID = employeeID; this.buttonListEmployee.Visible = this.EmployeeID == 1;

                this.commonMetaList = new CommonMetaList();

                ListMaintenance.ListEmployeeDataTable listEmployeeDataTable = this.commonMetaList.GetListEmployee();
                this.comboBoxEmployeeID.DataSource = listEmployeeDataTable;
                this.comboBoxEmployeeID.DisplayMember = listEmployeeDataTable.DescriptionColumn.ColumnName;
                this.comboBoxEmployeeID.ValueMember = listEmployeeDataTable.EmployeeIDColumn.ColumnName;
                this.employeeIDBinding = this.comboBoxEmployeeID.DataBindings.Add("SelectedValue", this, "EmployeeID", true, DataSourceUpdateMode.OnPropertyChanged);

                this.employeeIDBinding.BindingComplete += new BindingCompleteEventHandler(CommonControl_BindingComplete); 


            }
            catch (Exception exception)
            {
                GlobalExceptionHandler.ShowExceptionMessageBox(this, exception);
            }
        }

        private void CommonControl_BindingComplete(object sender, BindingCompleteEventArgs e)
        {
            if (e.BindingCompleteState == BindingCompleteState.Exception)
            { GlobalExceptionHandler.ShowExceptionMessageBox(this, e.ErrorText); e.Cancel = true; }
            else
                this.buttonListEmployee.Visible = this.EmployeeID == 1;
        }

        private void pictureBoxIcon_DoubleClick(object sender, EventArgs e)
        {
            if (this.EmployeeID == 1)
            {
                this.labelChangePassword.Visible = false;


                this.comboBoxImageS8PortName.Visible = true;
                this.comboBoxAutonicsPortName.Visible = true;
                this.labelPortName.Visible = true;
            }
        }

        private void buttonOK_Click(object sender, EventArgs e)
        {
            try
            {
                if (this.comboBoxEmployeeID.SelectedIndex < 0 || this.EmployeeID < 0) throw new System.ArgumentException("Vui lòng chọn tên người sử dụng!");

                if (!this.commonMetaList.CheckPasswordSuccessful(this.EmployeeID, this.textBoxPassword.Text)) throw new System.ArgumentException("Sai mật khẩu! Vui lòng kiểm tra lại trước khi tiếp tục.");

                GlobalVariables.GlobalUserInformation = new UserInformation(this.EmployeeID, 1, this.comboBoxEmployeeID.Text);


                if (sender.Equals(this.buttonListEmployee))
                {
                    CommonMDI commonMDI = new CommonMDI(GlobalEnum.TaskID.ListEmployee);
                    if (commonMDI.ShowDialog() == System.Windows.Forms.DialogResult.OK || true)
                    {
                        this.comboBoxEmployeeID.DataSource = this.commonMetaList.GetListEmployee();
                    }
                    commonMDI.Dispose();
                }


                if (sender.Equals(this.buttonOK))
                {
                    GlobalRegistry.Write("EmployeeID", "string" + this.EmployeeID);

                    if (this.comboBoxImageS8PortName.SelectedIndex < 0 || this.comboBoxAutonicsPortName.SelectedIndex < 0) throw new System.ArgumentException("Vui lòng chọn cổng COM!");

                    if (this.comboBoxImageS8PortName.DataSource == null || this.comboBoxAutonicsPortName.DataSource == null)
                    {
                        GlobalVariables.ImageS8PortName = "COM 0";
                        GlobalVariables.AutonicsPortName = "COM 0";
                    }
                    else
                    {
                        SQLDatabase.ExecuteNonQuery("UPDATE PublicPrinterProperties SET ImageS8PortName = N'" + (string)this.comboBoxImageS8PortName.SelectedValue + "', AutonicsPortName = N'" + (string)this.comboBoxAutonicsPortName.SelectedValue + "' ");

                        GlobalVariables.ImageS8PortName = (string)this.comboBoxImageS8PortName.SelectedValue;
                        GlobalVariables.AutonicsPortName = (string)this.comboBoxAutonicsPortName.SelectedValue;
                    }

                }

                if (sender.Equals(this.labelChangePassword))
                {
                    PublicAuthenticationPassword publicAuthenticationPassword = new PublicAuthenticationPassword();
                    publicAuthenticationPassword.ShowDialog();
                    publicAuthenticationPassword.Dispose();
                }

            }
            catch (Exception exception)
            {
                GlobalExceptionHandler.ShowExceptionMessageBox(this, exception);

                this.DialogResult = DialogResult.None;
            }
        }

        

    }
}
