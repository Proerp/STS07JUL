using System;

using System.Collections;

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
using DataTransferObject;
using System.Globalization;
using BrightIdeasSoftware;
using Equin.ApplicationFramework;

namespace PresentationLayer
{
    public partial class MarketingIncentive : Form, IMergeToolStrip, ICallToolStrip
    {

        #region <Implement Interface>

        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null) PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        public GlobalEnum.TaskID TaskID
        {
            get { return this.marketingIncentiveBLL.TaskID; }
        }


        public ToolStrip ChildToolStrip
        {
            get
            {
                return this.toolStripChildForm;
            }
            set
            {
                this.toolStripChildForm = value;
            }
        }





        #region Context toolbar


        public bool IsDirty
        {
            get { return this.marketingIncentiveBLL.IsDirty; }
        }

        public bool IsValid
        {
            get { return this.marketingIncentiveBLL.IsValid; }
        }

        public bool Closable
        {
            get { return true; }
        }

        public bool Loadable
        {
            get { return true; }
        }

        public bool Newable
        {
            get { return !this.marketingIncentiveBLL.ReadOnly; }
        }

        public bool Editable
        {
            get { return this.marketingIncentiveBLL.Editable; }
        }

        public bool Deletable
        {
            get { return this.marketingIncentiveBLL.Editable; }
        }

        public bool Importable
        {
            get { return true; }
        }

        public bool Exportable
        {
            get { return true; }
        }

        public bool Verifiable
        {
            get { return this.marketingIncentiveBLL.Verifiable; }
        }

        public bool Unverifiable
        {
            get { return this.marketingIncentiveBLL.Unverifiable; }
        }

        public bool Printable
        {
            get { return false; }
        }

        public bool Searchable
        {
            get { return true; }
        }

        /// <summary>
        /// Edit Mode: True, Read Mode: False
        /// </summary>
        private bool editableMode;
        private int lastMarketingIncentiveID;
        public bool EditableMode { get { return this.editableMode; } }
        /// <summary>
        /// This reverse of the EditableMode
        /// </summary>
        public bool ReadonlyMode { get { return !this.editableMode; } }

        /// <summary>
        /// Set the editableMode
        /// </summary>
        /// <param name="editableMode"></param>
        private void SetEditableMode(bool editableMode)
        {
            if (this.editableMode != editableMode)
            {
                this.lastMarketingIncentiveID = this.marketingIncentiveBLL.MarketingIncentiveID;
                this.editableMode = editableMode;
                this.NotifyPropertyChanged("EditableMode");
            }
        }


        private void CancelDirty(bool restoreSavedData)
        {
            try
            {
                if (restoreSavedData || this.marketingIncentiveBLL.MarketingIncentiveID <= 0)
                    this.marketingIncentiveBLL.Fill(this.lastMarketingIncentiveID);

                this.SetEditableMode(false);
            }
            catch (Exception exception)
            {
                throw exception;
            }
        }



        #endregion Context toolbar



        #region ICallToolStrip

        public void Escape()
        {
            if (this.EditableMode)
            {
                if (this.IsDirty)
                {
                    DialogResult dialogResult = MessageBox.Show(this, "Do you want to save your change?", "Warning", MessageBoxButtons.YesNoCancel);
                    if (dialogResult == DialogResult.Yes)
                    {
                        this.Save();
                        if (!this.IsDirty) this.CancelDirty(false);
                    }
                    else if (dialogResult == DialogResult.No)
                    {
                        this.CancelDirty(true);
                    }
                    else
                        return;
                }
                else
                    this.CancelDirty(false);
            }
            else
                this.Close(); //Unload this module
        }

        //Can phai xem lai trong VB de xem lai this.SetEditableMode () khi can thiet

        public void Loading()
        {
            this.GetMasterList();
        }

        public void New()
        {
            this.marketingIncentiveBLL.New();
            this.SetEditableMode(true);
        }

        public void Edit()
        {
            this.SetEditableMode(true);
        }

        public void Save()
        {
            this.marketingIncentiveBLL.Save();
            this.GetMasterList();
        }

        public void Delete()
        {
            DialogResult dialogResult = MessageBox.Show(this, "Are you sure you want to delete " + this.marketingIncentiveBLL.MarketingIncentiveMaster.PaymentPeriod + "?", "Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Stop);
            if (dialogResult == DialogResult.Yes)
            {
                try
                {
                    this.marketingIncentiveBLL.Delete();
                    this.GetMasterList();
                }
                catch (Exception exception)
                {
                    GlobalExceptionHandler.ShowExceptionMessageBox(this, exception);
                }
            }
        }

        public void Import()
        {
            ImportExcel();
        }

        public void Export()
        {
            try
            {
                if (this.ActiveControl.Equals(this.dataListViewMaster))
                {
                    DataTable dataTableExport;
                    dataTableExport = this.dataListViewMaster.DataSource as DataTable;
                    if (dataTableExport != null) CommonFormAction.Export(dataTableExport);
                }
                else
                {
                    List<MarketingIncentiveDetail> listExport;
                    listExport = this.marketingIncentiveBLL.MarketingIncentiveDetailList.ToList();
                    if (listExport != null) CommonFormAction.Export(listExport);
                }
            }
            catch (Exception exception)
            {
                GlobalExceptionHandler.ShowExceptionMessageBox(this, exception);
            }
        }

        public void Verify()
        {
            MessageBox.Show("Verify");
        }

        public void Print(PrintDestination printDestination)
        {
            MessageBox.Show("Print");
        }

        public void SearchText(string searchText)
        {
            CommonFormAction.OLVFilter(this.dataListViewMaster, searchText);
        }

        #endregion


        #endregion <Implement Interface>








        #region <Storage>
        private MarketingIncentiveBLL marketingIncentiveBLL;

        #endregion <Storage>


        public MarketingIncentive()
        {
            try
            {
                InitializeComponent();

                InitializeTabControl();


                this.marketingIncentiveBLL = new MarketingIncentiveBLL();

                this.marketingIncentiveBLL.PropertyChanged += new PropertyChangedEventHandler(marketingIncentiveBLL_PropertyChanged);

            }
            catch (Exception exception)
            {
                GlobalExceptionHandler.ShowExceptionMessageBox(this, exception);
            }
        }

        void marketingIncentiveBLL_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            this.NotifyPropertyChanged(e.PropertyName);
        }


        private void InitializeTabControl()
        {
            CustomTabControl customTabControlDetail = new CustomTabControl();

            customTabControlDetail.ImageList = this.imageListTabControl;


            customTabControlDetail.TabPages.Add("Detail", "Request Detail   ", 1);
            customTabControlDetail.TabPages[0].Controls.Add(this.dataGridViewMarketingIncentiveDetail);
            this.dataGridViewMarketingIncentiveDetail.Dock = DockStyle.Fill;


            customTabControlDetail.DisplayStyle = TabStyle.VisualStudio;
            customTabControlDetail.DisplayStyleProvider.ImageAlign = ContentAlignment.MiddleLeft;
            this.naviGroupDetails.Controls.Add(customTabControlDetail);
            this.naviGroupDetails.Controls.SetChildIndex(customTabControlDetail, 0);
            customTabControlDetail.Dock = DockStyle.Fill;
            
        }

        private void MarketingIncentive_Load(object sender, EventArgs e)
        {
            try
            {
                InitializeCommonControlBinding();

                InitializeDataGridBinding();

                InitializeReadOnlyModeBinding();
            }
            catch (Exception exception)
            {
                GlobalExceptionHandler.ShowExceptionMessageBox(this, exception);
            }
        }


        #region <InitializeBinding>

        Binding marketingProgramIDBinding;
        Binding marketingPaymentTypeIDBinding;

        Binding requestedEmployeeIDBinding;
        Binding notedEmployeeIDBinding;
        Binding approvedEmployeeIDBinding;


        Binding paymentPeriodBinding;
        Binding paymentMachanicsBinding;
        Binding remarksBinding;

        Binding requestedDateBinding;
        Binding entryDateBinding;

        Binding isDirtyBinding;
        Binding isDirtyBLLBinding;

        private void InitializeCommonControlBinding()
        {
            CommonMetaList commonMetaList = new CommonMetaList();


            ERmgrUIP.MarketingProgramListDataTable marketingProgramListDataTable = commonMetaList.GetMarketingProgramList();
            this.comboBoxMarketingProgramID.DataSource = marketingProgramListDataTable;
            this.comboBoxMarketingProgramID.DisplayMember = marketingProgramListDataTable.ReferenceColumn.ColumnName;
            this.comboBoxMarketingProgramID.ValueMember = marketingProgramListDataTable.MarketingProgramIDColumn.ColumnName;
            this.marketingProgramIDBinding = this.comboBoxMarketingProgramID.DataBindings.Add("SelectedValue", this.marketingIncentiveBLL.MarketingIncentiveMaster, "MarketingProgramID", true);


            ERmgrUIP.ListMarketingPaymentTypeDataTable marketingPaymentTypeListDataTable = commonMetaList.GetMarketingPaymentType();
            this.comboBoxMarketingPaymentTypeID.DataSource = marketingPaymentTypeListDataTable;
            this.comboBoxMarketingPaymentTypeID.DisplayMember = marketingPaymentTypeListDataTable.DescriptionColumn.ColumnName;
            this.comboBoxMarketingPaymentTypeID.ValueMember = marketingPaymentTypeListDataTable.MarketingPaymentTypeIDColumn.ColumnName;
            this.marketingPaymentTypeIDBinding = this.comboBoxMarketingPaymentTypeID.DataBindings.Add("SelectedValue", this.marketingIncentiveBLL.MarketingIncentiveMaster, "MarketingPaymentTypeID", true);



            ERmgrUIP.ListStaffNameDataTable listStaffNameDataTable = commonMetaList.GetStaffName();
            this.comboBoxRequestedEmployeeID.DataSource = listStaffNameDataTable;
            this.comboBoxRequestedEmployeeID.DisplayMember = listStaffNameDataTable.DescriptionOfficialColumn.ColumnName;
            this.comboBoxRequestedEmployeeID.ValueMember = listStaffNameDataTable.StaffIDColumn.ColumnName;
            this.requestedEmployeeIDBinding = this.comboBoxRequestedEmployeeID.DataBindings.Add("SelectedValue", this.marketingIncentiveBLL.MarketingIncentiveMaster, "RequestedEmployeeID", true);

            listStaffNameDataTable = (ERmgrUIP.ListStaffNameDataTable)listStaffNameDataTable.Copy();
            this.comboBoxNotedEmployeeID.DataSource = listStaffNameDataTable;
            this.comboBoxNotedEmployeeID.DisplayMember = listStaffNameDataTable.DescriptionOfficialColumn.ColumnName;
            this.comboBoxNotedEmployeeID.ValueMember = listStaffNameDataTable.StaffIDColumn.ColumnName;
            this.notedEmployeeIDBinding = this.comboBoxNotedEmployeeID.DataBindings.Add("SelectedValue", this.marketingIncentiveBLL.MarketingIncentiveMaster, "NotedEmployeeID", true);

            listStaffNameDataTable = (ERmgrUIP.ListStaffNameDataTable)listStaffNameDataTable.Copy();
            this.comboBoxApprovedEmployeeID.DataSource = listStaffNameDataTable;
            this.comboBoxApprovedEmployeeID.DisplayMember = listStaffNameDataTable.DescriptionOfficialColumn.ColumnName;
            this.comboBoxApprovedEmployeeID.ValueMember = listStaffNameDataTable.StaffIDColumn.ColumnName;
            this.approvedEmployeeIDBinding = this.comboBoxApprovedEmployeeID.DataBindings.Add("SelectedValue", this.marketingIncentiveBLL.MarketingIncentiveMaster, "ApprovedEmployeeID", true);


            this.paymentPeriodBinding = this.textBoxPaymentPeriod.DataBindings.Add("Text", this.marketingIncentiveBLL.MarketingIncentiveMaster, "PaymentPeriod", true);
            this.paymentMachanicsBinding = this.textBoxPaymentMachanics.DataBindings.Add("Text", this.marketingIncentiveBLL.MarketingIncentiveMaster, "PaymentMachanics", true);
            this.remarksBinding = this.textBoxRemarks.DataBindings.Add("Text", this.marketingIncentiveBLL.MarketingIncentiveMaster, "Remarks", true);


            this.requestedDateBinding = this.dateTimePickerRequestedDate.DataBindings.Add("Value", this.marketingIncentiveBLL.MarketingIncentiveMaster, "RequestedDate", true);

            this.isDirtyBinding = this.checkBoxIsDirty.DataBindings.Add("Checked", this.marketingIncentiveBLL.MarketingIncentiveMaster, "IsDirty", true);
            this.isDirtyBLLBinding = this.checkBoxIsDirtyBLL.DataBindings.Add("Checked", this.marketingIncentiveBLL, "IsDirty", true);



            this.marketingProgramIDBinding.BindingComplete += new BindingCompleteEventHandler(CommonControl_BindingComplete);
            this.marketingPaymentTypeIDBinding.BindingComplete += new BindingCompleteEventHandler(CommonControl_BindingComplete);
            this.requestedEmployeeIDBinding.BindingComplete += new BindingCompleteEventHandler(CommonControl_BindingComplete);
            this.notedEmployeeIDBinding.BindingComplete += new BindingCompleteEventHandler(CommonControl_BindingComplete);
            this.approvedEmployeeIDBinding.BindingComplete += new BindingCompleteEventHandler(CommonControl_BindingComplete);


            this.paymentPeriodBinding.BindingComplete += new BindingCompleteEventHandler(CommonControl_BindingComplete);
            this.paymentMachanicsBinding.BindingComplete += new BindingCompleteEventHandler(CommonControl_BindingComplete);
            this.remarksBinding.BindingComplete += new BindingCompleteEventHandler(CommonControl_BindingComplete);

            this.requestedDateBinding.BindingComplete += new BindingCompleteEventHandler(CommonControl_BindingComplete);

            this.isDirtyBinding.BindingComplete += new BindingCompleteEventHandler(CommonControl_BindingComplete);
            this.isDirtyBLLBinding.BindingComplete += new BindingCompleteEventHandler(CommonControl_BindingComplete);

            this.naviGroupDetails.DataBindings.Add("ExpandedHeight", this.numericUpDownSizingDetail, "Value", true, DataSourceUpdateMode.OnPropertyChanged);
            this.numericUpDownSizingDetail.Minimum = this.naviGroupDetails.HeaderHeight * 2;
            this.numericUpDownSizingDetail.Maximum = this.naviGroupDetails.Height + this.dataListViewMaster.Height;

            this.tableLayoutPanelMaster.ColumnStyles[this.tableLayoutPanelMaster.ColumnCount - 1].SizeType = SizeType.Absolute; this.tableLayoutPanelMaster.ColumnStyles[this.tableLayoutPanelMaster.ColumnCount - 1].Width = 10;
            this.tableLayoutPanelExtend.ColumnStyles[this.tableLayoutPanelExtend.ColumnCount - 1].SizeType = SizeType.Absolute; this.tableLayoutPanelExtend.ColumnStyles[this.tableLayoutPanelExtend.ColumnCount - 1].Width = 10;

            this.errorProviderMaster.DataSource = this.marketingIncentiveBLL.MarketingIncentiveMaster;

        }


        private void CommonControl_BindingComplete(object sender, BindingCompleteEventArgs e)
        {
            if (e.BindingCompleteState == BindingCompleteState.Exception) { GlobalExceptionHandler.ShowExceptionMessageBox(this, e.ErrorText); e.Cancel = true; }
        }

        private void numericUpDownSizingDetail_ValueChanged(object sender, EventArgs e)
        {
            this.naviGroupDetails.Expand();
        }

        private void naviGroupDetails_HeaderMouseClick(object sender, MouseEventArgs e)
        {
            this.numericUpDownSizingDetail.Visible = this.naviGroupDetails.Expanded;
            this.toolStripNaviGroupDetails.Visible = this.naviGroupDetails.Expanded;
        }

        private void toolStripButtonShowDetailsExtend_Click(object sender, EventArgs e)
        {
            this.naviGroupDetailsExtend.Expanded = !this.naviGroupDetailsExtend.Expanded;
            this.toolStripButtonShowDetailsExtend.Image = this.naviGroupDetailsExtend.Expanded ? ResourceIcon.Chevron_Collapse.ToBitmap() : ResourceIcon.Chevron_Expand.ToBitmap();
        }


        private void GetMasterList()
        {
            foreach (OLVColumn olvColumn in dataListViewMaster.Columns) { olvColumn.Renderer = null; }

            DataTable marketingIncentiveMasterList = this.marketingIncentiveBLL.MarketingIncentiveMasterList(GlobalVariables.GlobalOptionSetting.LowerFillterDate, GlobalVariables.GlobalOptionSetting.UpperFillterDate);
            marketingIncentiveMasterList.TableName = "MarketingIncentiveListing";

            DataSet bindingDataSet = new DataSet();
            bindingDataSet.Tables.Add(marketingIncentiveMasterList);

            this.dataListViewMaster.AutoGenerateColumns = false;
            this.dataListViewMaster.DataSource = marketingIncentiveMasterList;
        }

        private void InitializeDataGridBinding()
        {

            this.GetMasterList();



            #region <dataGridViewDetail>

            CommonMetaList commonMetaList = new CommonMetaList();
            DataGridViewComboBoxColumn comboBoxColumn;

            //<dataGridViewMarketingIncentiveDetail>
            ERmgrUIP.ListCustomerNameDataTable listCustomerNameDataTable = commonMetaList.GetCustomerName(true);
            comboBoxColumn = (DataGridViewComboBoxColumn)this.dataGridViewMarketingIncentiveDetail.Columns[listCustomerNameDataTable.CustomerIDColumn.ColumnName];
            comboBoxColumn.DataSource = listCustomerNameDataTable;
            comboBoxColumn.DisplayMember = listCustomerNameDataTable.DescriptionColumn.ColumnName;
            comboBoxColumn.ValueMember = listCustomerNameDataTable.CustomerIDColumn.ColumnName;

            //--Display the second column for customer (Readonly): DescriptionOfficial -- Later: Try other way, instead of current DataGridViewComboBoxColumn.Datasource  
            comboBoxColumn = (DataGridViewComboBoxColumn)this.dataGridViewMarketingIncentiveDetail.Columns[listCustomerNameDataTable.DescriptionOfficialColumn.ColumnName];
            comboBoxColumn.DataSource = listCustomerNameDataTable;
            comboBoxColumn.DisplayMember = listCustomerNameDataTable.DescriptionOfficialColumn.ColumnName;
            comboBoxColumn.ValueMember = listCustomerNameDataTable.CustomerIDColumn.ColumnName;

            ERmgrUIP.ListMarketingPaymentTermDataTable listMarketingPaymentTermDataTable = commonMetaList.GetMarketingPaymentTerm(true);
            comboBoxColumn = (DataGridViewComboBoxColumn)this.dataGridViewMarketingIncentiveDetail.Columns[listMarketingPaymentTermDataTable.MarketingPaymentTermIDColumn.ColumnName];
            comboBoxColumn.DataSource = listMarketingPaymentTermDataTable;
            comboBoxColumn.DisplayMember = listMarketingPaymentTermDataTable.DescriptionColumn.ColumnName;
            comboBoxColumn.ValueMember = listMarketingPaymentTermDataTable.MarketingPaymentTermIDColumn.ColumnName;


            this.dataGridViewMarketingIncentiveDetail.AutoGenerateColumns = false;
            marketingIncentiveDetailListView = new BindingListView<MarketingIncentiveDetail>(this.marketingIncentiveBLL.MarketingIncentiveDetailList);
            this.dataGridViewMarketingIncentiveDetail.DataSource = marketingIncentiveDetailListView;

            StackedHeaderDecorator stackedHeaderDecorator = new StackedHeaderDecorator(this.dataGridViewMarketingIncentiveDetail);

            #endregion <dataGridViewDetail>
        }


        private void InitializeReadOnlyModeBinding()
        {
            List<Control> controlList = GlobalStaticFunction.GetAllControls(this);

            foreach (Control control in controlList)
            {
                //if (control is TextBox) control.DataBindings.Add("Readonly", this, "ReadonlyMode");
                if (control is TextBox) control.DataBindings.Add("Enabled", this, "EditableMode");
                else if (control is ComboBox || control is DateTimePicker) control.DataBindings.Add("Enabled", this, "EditableMode");
                else if (control is DataGridView)
                {
                    control.DataBindings.Add("Readonly", this, "ReadonlyMode");
                    control.DataBindings.Add("AllowUserToAddRows", this, "EditableMode");
                    control.DataBindings.Add("AllowUserToDeleteRows", this, "EditableMode");
                }
            }

            this.dataListViewMaster.DataBindings.Add("Enabled", this, "ReadonlyMode");
        }

        BindingListView<MarketingIncentiveDetail> marketingIncentiveDetailListView;
        private void filterTextBox_TextChanged(object sender, EventArgs e)
        {// Change the filter of the view.
            //marketingIncentiveDetailListView.ApplyFilter(
            //    delegate(MarketingIncentiveDetail marketingPaymentDetail)
            //    {
            //        // uses ToLower() to ignore case of text.
            //        return marketingPaymentDetail.CustomerCode.ToLower().Contains(filterTextBox.Text.ToLower());
            //    }
            //);


        }

        #endregion <InitializeBinding>





        #region Import Excel

        private void ImportExcel()
        {
            try
            {
                OpenFileDialog openFileDialog = new OpenFileDialog();
                openFileDialog.Filter = "Excel File (.xlsx)|*.xlsx";

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    DialogMapExcelColumn dialogMapExcelColumn = new DialogMapExcelColumn(OleDbDatabase.MappingTaskID.MarketingIncentive, openFileDialog.FileName);

                    if (dialogMapExcelColumn.ShowDialog() == DialogResult.OK)
                    {
                        dialogMapExcelColumn.Dispose();
                        if (this.marketingIncentiveBLL.ImportExcel(openFileDialog.FileName))
                            MessageBox.Show(this, "Congratulation!" + "\r\n" + "\r\n" + "File: " + openFileDialog.FileName + " is imported successfull!" + "\r\n" + "\r\n" + "Please press OK to finish.", "Importing", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }

            }
            catch (Exception exception)
            {
                GlobalExceptionHandler.ShowExceptionMessageBox(this, exception);
            }

        }








        #endregion Import Excel


        private void dataListViewMaster_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                ObjectListView objectListView = (ObjectListView)sender;
                DataRowView row = (DataRowView)objectListView.SelectedObject;

                if (row != null)
                {
                    int marketingIncentiveID;

                    if (int.TryParse(row.Row["MarketingIncentiveID"].ToString(), out marketingIncentiveID)) this.marketingIncentiveBLL.Fill(marketingIncentiveID);
                    else this.marketingIncentiveBLL.Fill(0);
                }
                else this.marketingIncentiveBLL.Fill(0);
            }
            catch (Exception exception)
            {
                GlobalExceptionHandler.ShowExceptionMessageBox(this, exception);
            }
        }




    }




}
