using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DataTransferObject;
using BusinessLogicLayer;
using DataAccessLayer;

namespace PresentationLayer
{
    public partial class PublicShowDataMessage : Form
    {
        private DataMessageMaster dataMessageMaster;

        public string CoilCodeAndExtention { get { return this.dataMessageMaster.CoilCode + "             [" + this.dataMessageMaster.CoilExtension.ToString("00") + "]"; } }
        public string DataMessageDisplay { get { return this.dataMessageMaster.Display1 + " " + this.dataMessageMaster.Display2 + " " + this.dataMessageMaster.Display3 + " " + this.dataMessageMaster.Display4 + " " + this.dataMessageMaster.Display5 + " " + this.dataMessageMaster.Display6; } }

        public double UserInputCounterValue { get; set; }

        public PublicShowDataMessage(DataMessageMaster dataMessageMaster, string warningMessage, bool isUserInputCounterValue)
        {
            InitializeComponent();

            this.dataMessageMaster = dataMessageMaster;

            this.labelWarningMessage.Text = warningMessage;

            this.labelUserInputCounterValue.Visible = isUserInputCounterValue;
            this.textBoxUserInputCounterValue.Visible = isUserInputCounterValue;

            this.UserInputCounterValue = -1;
        }

        Binding dataStatusIDBinding;
        Binding coilCodeAndExtentionBinding;
        Binding counterValueBinding;
        Binding counterAutonicsBinding;
        Binding dataMessageDisplayBinding;
        Binding userInputCounterValueBinding;

        private void PublicShowDataMessage_Load(object sender, EventArgs e)
        {
            CommonMetaList commonMetaList = new CommonMetaList();

            ListMaintenance.ListDataStatusDataTable listDataStatusDataTable = commonMetaList.GetListDataStatus();
            this.comboBoxDataStatusID.DataSource = listDataStatusDataTable;
            this.comboBoxDataStatusID.DisplayMember = listDataStatusDataTable.DescriptionColumn.ColumnName;
            this.comboBoxDataStatusID.ValueMember = listDataStatusDataTable.DataStatusIDColumn.ColumnName;
            this.dataStatusIDBinding = this.comboBoxDataStatusID.DataBindings.Add("SelectedValue", this.dataMessageMaster, "DataStatusID", true);


            this.coilCodeAndExtentionBinding = this.textBoxCoilCodeAndExtention.DataBindings.Add("Text", this, "CoilCodeAndExtention", true);
            this.counterValueBinding = this.textBoxCounterValue.DataBindings.Add("Text", this.dataMessageMaster, "CounterValue", true);
            this.counterAutonicsBinding = this.textBoxCounterAutonics.DataBindings.Add("Text", this.dataMessageMaster, "CounterAutonics", true);
            this.dataMessageDisplayBinding = this.textBoxDataMessageDisplay.DataBindings.Add("Text", this, "DataMessageDisplay", true);

            this.userInputCounterValueBinding = this.textBoxUserInputCounterValue.DataBindings.Add("Text", this, "UserInputCounterValue", true, DataSourceUpdateMode.OnPropertyChanged);


            this.dataStatusIDBinding.BindingComplete += new BindingCompleteEventHandler(CommonControl_BindingComplete);

            this.coilCodeAndExtentionBinding.BindingComplete += new BindingCompleteEventHandler(CommonControl_BindingComplete);
            this.counterValueBinding.BindingComplete += new BindingCompleteEventHandler(CommonControl_BindingComplete);
            this.counterAutonicsBinding.BindingComplete += new BindingCompleteEventHandler(CommonControl_BindingComplete);
            this.dataMessageDisplayBinding.BindingComplete += new BindingCompleteEventHandler(CommonControl_BindingComplete);

            this.userInputCounterValueBinding.BindingComplete += new BindingCompleteEventHandler(CommonControl_BindingComplete);

        }

        private void CommonControl_BindingComplete(object sender, BindingCompleteEventArgs e)
        {
            if (e.BindingCompleteState == BindingCompleteState.Exception) { GlobalExceptionHandler.ShowExceptionMessageBox(this, e.ErrorText); e.Cancel = true; }
        }

        private void toolStripButtonEscapeAndOK_Click(object sender, EventArgs e)
        {
            this.DialogResult = sender.Equals(this.toolStripButtonOK) ? System.Windows.Forms.DialogResult.Yes : System.Windows.Forms.DialogResult.No;
        }

    }
}
