using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using Global.Class.Library;
using BusinessLogicLayer;
using DataAccessLayer;


namespace PresentationLayer
{
    public partial class SettingFillingLineData : Form
    {

        private FillingLineData privateFillingLineData;
        private FillingLineData globalFillingLineData;

        Binding fillingLineNameBinding;
        Binding productIDBinding;
        Binding batchNoBinding;
        Binding settingDateBinding;

        Binding batchSerialNumberBinding;
        Binding monthSerialNumberBinding;
        Binding batchCartonNumberBinding;
        Binding monthCartonNumberBinding;

        public SettingFillingLineData(FillingLineData fillingLineData, bool isMessageQueueEmpty)
        {
            InitializeComponent();

            this.globalFillingLineData = fillingLineData;
            this.privateFillingLineData = fillingLineData.ShallowClone();


            DataTable dataTable = ADODatabase.GetDataTable("SELECT ProductID, ProductCode, ProductCodeOriginal, ProductName, ProductCode + '    [' + ProductCodeOriginal + '] ' + ProductName AS ProductDisplayName FROM ListProductName");
            this.comboBoxProductID.DataSource = dataTable;
            this.comboBoxProductID.ValueMember = "ProductID";
            this.comboBoxProductID.DisplayMember = "ProductDisplayName";


            fillingLineNameBinding = this.textBoxFillingLineName.DataBindings.Add("Text", this.privateFillingLineData, "FillingLineName", true);
            productIDBinding = this.comboBoxProductID.DataBindings.Add("SelectedValue", this.privateFillingLineData, "ProductID", true, DataSourceUpdateMode.OnPropertyChanged);

            batchNoBinding = this.textBoxBatchNo.DataBindings.Add("Text", this.privateFillingLineData, "BatchNo", true);
            settingDateBinding = this.dateTimePickerSettingDate.DataBindings.Add("Value", this.privateFillingLineData, "SettingDate", true, DataSourceUpdateMode.OnPropertyChanged);

            batchSerialNumberBinding = this.textBoxBatchSerialNumber.DataBindings.Add("Text", this.privateFillingLineData, "BatchSerialNumber", true);
            monthSerialNumberBinding = this.textBoxMonthSerialNumber.DataBindings.Add("Text", this.privateFillingLineData, "MonthSerialNumber", true);

            batchCartonNumberBinding = this.textBoxBatchCartonNumber.DataBindings.Add("Text", this.privateFillingLineData, "BatchCartonNumber", true);
            monthCartonNumberBinding = this.textBoxMonthCartonNumber.DataBindings.Add("Text", this.privateFillingLineData, "MonthCartonNumber", true);


            fillingLineNameBinding.BindingComplete += new BindingCompleteEventHandler(fillingLineNameBinding_BindingComplete);
            productIDBinding.BindingComplete += new BindingCompleteEventHandler(productIDBinding_BindingComplete);

            batchNoBinding.BindingComplete += new BindingCompleteEventHandler(batchNoBinding_BindingComplete);
            settingDateBinding.BindingComplete += new BindingCompleteEventHandler(settingDateBinding_BindingComplete);

            batchSerialNumberBinding.BindingComplete += new BindingCompleteEventHandler(batchSerialNumberBinding_BindingComplete);
            monthSerialNumberBinding.BindingComplete += new BindingCompleteEventHandler(monthSerialNumberBinding_BindingComplete);

            batchCartonNumberBinding.BindingComplete += new BindingCompleteEventHandler(batchCartonNumberBinding_BindingComplete);
            monthCartonNumberBinding.BindingComplete += new BindingCompleteEventHandler(monthCartonNumberBinding_BindingComplete);

            this.privateFillingLineData.PropertyChanged += new PropertyChangedEventHandler(privateFillingLineData_PropertyChanged);

            this.comboBoxProductID.Enabled = isMessageQueueEmpty;
        }


        #region Handle Errors and Exceptions that Occur with Databinding

        void fillingLineNameBinding_BindingComplete(object sender, BindingCompleteEventArgs e)
        {
            if (e.BindingCompleteState != BindingCompleteState.Success) { GlobalExceptionHandler.ShowExceptionMessageBox(this, e.ErrorText); e.Cancel = true; }
        }

        void productIDBinding_BindingComplete(object sender, BindingCompleteEventArgs e)
        {
            if (e.BindingCompleteState != BindingCompleteState.Success)
            { GlobalExceptionHandler.ShowExceptionMessageBox(this, e.ErrorText); e.Cancel = true; }
            else
            {
                this.privateFillingLineData.ProductCode = this.comboBoxProductID.SelectedIndex != -1 ? (string)((DataRowView)this.comboBoxProductID.SelectedItem)["ProductCode"] : "";
                this.privateFillingLineData.ProductCodeOriginal = this.comboBoxProductID.SelectedIndex != -1 ? (string)((DataRowView)this.comboBoxProductID.SelectedItem)["ProductCodeOriginal"] : "";
            }
        }

        void batchNoBinding_BindingComplete(object sender, BindingCompleteEventArgs e)
        {
            if (e.BindingCompleteState != BindingCompleteState.Success) { GlobalExceptionHandler.ShowExceptionMessageBox(this, e.ErrorText); e.Cancel = true; }
        }

        void settingDateBinding_BindingComplete(object sender, BindingCompleteEventArgs e)
        {
            if (e.BindingCompleteState != BindingCompleteState.Success) { GlobalExceptionHandler.ShowExceptionMessageBox(this, e.ErrorText); e.Cancel = true; }

        }



        void batchSerialNumberBinding_BindingComplete(object sender, BindingCompleteEventArgs e)
        {
            if (e.BindingCompleteState != BindingCompleteState.Success) { GlobalExceptionHandler.ShowExceptionMessageBox(this, e.ErrorText); e.Cancel = true; }
        }

        void monthSerialNumberBinding_BindingComplete(object sender, BindingCompleteEventArgs e)
        {
            if (e.BindingCompleteState != BindingCompleteState.Success) { GlobalExceptionHandler.ShowExceptionMessageBox(this, e.ErrorText); e.Cancel = true; }
        }

        void batchCartonNumberBinding_BindingComplete(object sender, BindingCompleteEventArgs e)
        {
            if (e.BindingCompleteState != BindingCompleteState.Success) { GlobalExceptionHandler.ShowExceptionMessageBox(this, e.ErrorText); e.Cancel = true; }
        }

        void monthCartonNumberBinding_BindingComplete(object sender, BindingCompleteEventArgs e)
        {
            if (e.BindingCompleteState != BindingCompleteState.Success) { GlobalExceptionHandler.ShowExceptionMessageBox(this, e.ErrorText); e.Cancel = true; }
        }


        #endregion



        void privateFillingLineData_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case "BatchSerialNumber":
                    this.textBoxBatchSerialNumber.BackColor = Color.Yellow; break;
                case "MonthSerialNumber":
                    this.textBoxMonthSerialNumber.BackColor = Color.Yellow; break;
                case "BatchCartonNumber":
                    this.textBoxBatchCartonNumber.BackColor = Color.Yellow; break;
                case "MonthCartonNumber":
                    this.textBoxMonthCartonNumber.BackColor = Color.Yellow; break;

            }
        }

        private void timerEverySecond_Tick(object sender, EventArgs e)
        {
            if (this.privateFillingLineData != null)
            {
                if (this.privateFillingLineData.SettingMonthID != GlobalStaticFunction.DateToContinuosMonth())
                {
                    this.pictureBoxWarningNewMonth.Visible = !this.pictureBoxWarningNewMonth.Visible; this.labelWarningNewMonth.Visible = !this.labelWarningNewMonth.Visible;
                }
                else
                {
                    this.pictureBoxWarningNewMonth.Visible = false; this.labelWarningNewMonth.Visible = false;
                }
            }
        }



        private void buttonOK_Click(object sender, EventArgs e)
        {
            try
            {
                if (this.checkBoxSaveAgreement.Checked && this.privateFillingLineData.DataValidated())
                {
                    if (!this.privateFillingLineData.Save())
                        this.DialogResult = DialogResult.None;
                    else
                    {
                        GlobalStaticFunction.CopyProperties(this.privateFillingLineData, this.globalFillingLineData);
                        GlobalVariables.noItemPerCartonSetByProductID = SQLDatabase.GetScalarValue("SELECT NoItemPerCarton FROM ListProductName WHERE ProductID = " + this.globalFillingLineData.ProductID);
                    }
                }
                else
                {
                    this.DialogResult = DialogResult.None;
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
