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
    public partial class DataMessage : Form, IMergeToolStrip, ICallToolStrip
    {

        #region <Implement Interface>

        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null) PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        public GlobalEnum.TaskID TaskID
        {
            get { return this.dataMessageBLL.TaskID; }
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
            get { return this.dataMessageBLL.IsDirty; }
        }

        public bool IsValid
        {
            get { return this.dataMessageBLL.IsValid; }
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
            get { return !this.dataMessageBLL.ReadOnly; }
        }

        public bool Editable
        {
            get { return !this.dataMessageBLL.ReadOnly; }
        }

        public bool Deletable
        {
            get { return this.dataMessageBLL.Editable; }
        }

        public bool Importable
        {
            get { return false; }
        }

        public bool Exportable
        {
            get { return false; }
        }

        public bool Verifiable
        {
            get { return this.dataMessageBLL.Verifiable; }
        }

        public bool Unverifiable
        {
            get { return this.dataMessageBLL.Unverifiable; }
        }

        public bool Printable
        {
            get { return false; }
        }

        public bool Searchable
        {
            get { return false; }
        }

        /// <summary>
        /// Edit Mode: True, Read Mode: False
        /// </summary>
        private bool editableMode;
        private int lastDataMessageID;
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
                this.lastDataMessageID = this.dataMessageBLL.DataMessageID;
                this.editableMode = editableMode;
                this.NotifyPropertyChanged("EditableMode");

                this.naviGroupDetails.Expanded = editableMode;
            }
        }


        private void CancelDirty(bool restoreSavedData)
        {
            try
            {
                if (restoreSavedData || this.dataMessageBLL.DataMessageID <= 0)
                    this.dataMessageBLL.Fill(this.lastDataMessageID);

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
                    DialogResult dialogResult = MessageBox.Show(this, "Bạn có muốn lưu dữ liệu trước khi đóng lại không?", "Cảnh báo", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Warning);
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


        public void Loading()
        {
            this.dataMessageBLL.DataMessageMasterListing(GlobalVariables.GlobalOptionSetting.LowerFillterDate, GlobalVariables.GlobalOptionSetting.UpperFillterDate);
        }

        private void toolStripButtonSplitCoil_Click(object sender, EventArgs e)
        {
            try
            {
                string coilExtensionString = ""; int coilExtension = 0;
                if (CustomInputBox.Show("NMVN", "Vui lòng nhập số cuộn chia", ref coilExtensionString) == System.Windows.Forms.DialogResult.OK)
                {
                    if (int.TryParse(coilExtensionString, out coilExtension))
                    {
                        if (this.dataMessageBLL.SplitCoil(coilExtension))
                            this.Loading();
                    }
                    else
                        throw new Exception("Vui lòng nhập số cuộn chia.");
                }
            }
            catch (Exception exception)
            {
                GlobalExceptionHandler.ShowExceptionMessageBox(this, exception);
            }
        }

        public void New()
        {
            this.dataMessageBLL.New();
            this.SetEditableMode(true);
        }

        public void Edit()
        {
            this.SetEditableMode(true);
        }

        public void Save()
        {
            this.dataMessageBLL.Save();
            this.Loading();
        }

        public void Delete()
        {
            DialogResult dialogResult = MessageBox.Show(this, "Bạn muốn xóa dữ liệu này: " + this.dataMessageBLL.DataMessageMaster.Display4 + "?", "Cảnh báo", MessageBoxButtons.YesNo, MessageBoxIcon.Stop);
            if (dialogResult == DialogResult.Yes)
            {
                try
                {
                    this.dataMessageBLL.Delete();
                    this.Loading();
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

        }

        #endregion


        #endregion <Implement Interface>








        #region <Storage>
        private DataMessageBLL dataMessageBLL;


        #endregion <Storage>

        Binding beginingDateBinding;
        Binding endingDateBinding;

        public DataMessage()
        {
            try
            {
                InitializeComponent();

                this.beginingDateBinding = this.textBoxLowerFillterDate.TextBox.DataBindings.Add("Text", GlobalVariables.GlobalOptionSetting, "LowerFillterDate", true);
                this.endingDateBinding = this.textBoxUpperFillterDate.TextBox.DataBindings.Add("Text", GlobalVariables.GlobalOptionSetting, "UpperFillterDate", true);

                this.beginingDateBinding.BindingComplete += new BindingCompleteEventHandler(CommonControl_BindingComplete);
                this.endingDateBinding.BindingComplete += new BindingCompleteEventHandler(CommonControl_BindingComplete);

                this.dataMessageBLL = new DataMessageBLL();

                this.dataMessageBLL.PropertyChanged += new PropertyChangedEventHandler(dataMessageBLL_PropertyChanged);

            }
            catch (Exception exception)
            {
                GlobalExceptionHandler.ShowExceptionMessageBox(this, exception);
            }
        }

        void dataMessageBLL_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            this.NotifyPropertyChanged(e.PropertyName);
        }


        private void DataMessage_Load(object sender, EventArgs e)
        {
            try
            {
                InitializeCommonControlBinding();

                InitializeDataGridBinding();

                InitializeReadOnlyModeBinding();

                this.Loading();
            }
            catch (Exception exception)
            {
                GlobalExceptionHandler.ShowExceptionMessageBox(this, exception);
            }
        }



        #region <InitializeBinding>


        Binding logoIDBinding;
        Binding factoryIDBinding;
        Binding ownerIDBinding;
        Binding categoryIDBinding;
        Binding productIDBinding;
        Binding coilIDBinding;
        Binding requestedEmployeeIDBinding;

        Binding coilExtensionBinding;
        Binding remarksBinding;

        Binding productionDateBinding;
        Binding entryDateBinding;

        Binding productionDatePrintableBinding;
        Binding logoPrintableBinding;
        Binding factoryPrintableBinding;
        Binding ownerPrintableBinding;
        Binding categoryPrintableBinding;
        Binding productPrintableBinding;
        Binding coilPrintableBinding;
        Binding counterPrintableBinding;

        Binding isDirtyBinding;
        Binding isDirtyBLLBinding;


        private void InitializeCommonControlBinding()
        {
            CommonMetaList commonMetaList = new CommonMetaList();


            ListMaintenance.ListLogoDataTable listLogoDataTable = commonMetaList.GetListLogo();
            this.comboBoxLogoID.DataSource = listLogoDataTable;
            this.comboBoxLogoID.DisplayMember = listLogoDataTable.DescriptionColumn.ColumnName;
            this.comboBoxLogoID.ValueMember = listLogoDataTable.LogoIDColumn.ColumnName;
            this.logoIDBinding = this.comboBoxLogoID.DataBindings.Add("SelectedValue", this.dataMessageBLL.DataMessageMaster, "LogoID", true);


            ListMaintenance.ListFactoryDataTable listFactoryDataTable = commonMetaList.GetListFactory();
            this.comboBoxFactoryID.DataSource = listFactoryDataTable;
            this.comboBoxFactoryID.DisplayMember = listFactoryDataTable.DescriptionColumn.ColumnName;
            this.comboBoxFactoryID.ValueMember = listFactoryDataTable.FactoryIDColumn.ColumnName;
            this.factoryIDBinding = this.comboBoxFactoryID.DataBindings.Add("SelectedValue", this.dataMessageBLL.DataMessageMaster, "FactoryID", true);

            ListMaintenance.ListOwnerDataTable listOwnerDataTable = commonMetaList.GetListOwner();
            this.comboBoxOwnerID.DataSource = listOwnerDataTable;
            this.comboBoxOwnerID.DisplayMember = listOwnerDataTable.DescriptionColumn.ColumnName;
            this.comboBoxOwnerID.ValueMember = listOwnerDataTable.OwnerIDColumn.ColumnName;
            this.ownerIDBinding = this.comboBoxOwnerID.DataBindings.Add("SelectedValue", this.dataMessageBLL.DataMessageMaster, "OwnerID", true);

            ListMaintenance.ListCategoryDataTable listCategoryDataTable = commonMetaList.GetListCategory();
            this.comboBoxCategoryID.DataSource = listCategoryDataTable;
            this.comboBoxCategoryID.DisplayMember = listCategoryDataTable.DescriptionColumn.ColumnName;
            this.comboBoxCategoryID.ValueMember = listCategoryDataTable.CategoryIDColumn.ColumnName;
            this.categoryIDBinding = this.comboBoxCategoryID.DataBindings.Add("SelectedValue", this.dataMessageBLL.DataMessageMaster, "CategoryID", true);

            ListMaintenance.ListProductDataTable listProductDataTable = commonMetaList.GetListProduct();
            this.comboBoxProductID.DataSource = listProductDataTable;
            this.comboBoxProductID.DisplayMember = listProductDataTable.DescriptionColumn.ColumnName;
            this.comboBoxProductID.ValueMember = listProductDataTable.ProductIDColumn.ColumnName;
            this.productIDBinding = this.comboBoxProductID.DataBindings.Add("SelectedValue", this.dataMessageBLL.DataMessageMaster, "ProductID", true);

            ListMaintenance.ListCoilDataTable listCoilDataTable = commonMetaList.GetListCoil();
            this.comboBoxCoilID.DataSource = listCoilDataTable;
            this.comboBoxCoilID.DisplayMember = listCoilDataTable.DescriptionColumn.ColumnName;
            this.comboBoxCoilID.ValueMember = listCoilDataTable.CoilIDColumn.ColumnName;
            this.coilIDBinding = this.comboBoxCoilID.DataBindings.Add("SelectedValue", this.dataMessageBLL.DataMessageMaster, "CoilID", true);

            ListMaintenance.ListStaffNameDataTable listStaffNameDataTable = commonMetaList.GetStaffName();
            this.comboBoxRequestedEmployeeID.DataSource = listStaffNameDataTable;
            this.comboBoxRequestedEmployeeID.DisplayMember = listStaffNameDataTable.DescriptionOfficialColumn.ColumnName;
            this.comboBoxRequestedEmployeeID.ValueMember = listStaffNameDataTable.StaffIDColumn.ColumnName;
            this.requestedEmployeeIDBinding = this.comboBoxRequestedEmployeeID.DataBindings.Add("SelectedValue", this.dataMessageBLL.DataMessageMaster, "RequestedEmployeeID", true);


            this.coilExtensionBinding = this.textBoxCoilExtension.DataBindings.Add("Text", this.dataMessageBLL.DataMessageMaster, "CoilExtension", true);
            this.remarksBinding = this.textBoxRemarks.DataBindings.Add("Text", this.dataMessageBLL.DataMessageMaster, "Remarks", true);

            this.productionDateBinding = this.dateTimePickerProductionDate.DataBindings.Add("Value", this.dataMessageBLL.DataMessageMaster, "ProductionDate", true);

            this.productionDatePrintableBinding = this.checkBoxProductionDatePrintable.DataBindings.Add("Checked", this.dataMessageBLL.DataMessageMaster, "ProductionDatePrintable", true);
            this.logoPrintableBinding = this.checkBoxLogoPrintable.DataBindings.Add("Checked", this.dataMessageBLL.DataMessageMaster, "LogoPrintable", true);
            this.factoryPrintableBinding = this.checkBoxFactoryPrintable.DataBindings.Add("Checked", this.dataMessageBLL.DataMessageMaster, "FactoryPrintable", true);
            this.ownerPrintableBinding = this.checkBoxOwnerPrintable.DataBindings.Add("Checked", this.dataMessageBLL.DataMessageMaster, "OwnerPrintable", true);
            this.categoryPrintableBinding = this.checkBoxCategoryPrintable.DataBindings.Add("Checked", this.dataMessageBLL.DataMessageMaster, "CategoryPrintable", true);
            this.productPrintableBinding = this.checkBoxProductPrintable.DataBindings.Add("Checked", this.dataMessageBLL.DataMessageMaster, "ProductPrintable", true);
            this.coilPrintableBinding = this.checkBoxCoilPrintable.DataBindings.Add("Checked", this.dataMessageBLL.DataMessageMaster, "CoilPrintable", true);
            this.counterPrintableBinding = this.checkBoxCounterPrintable.DataBindings.Add("Checked", this.dataMessageBLL.DataMessageMaster, "CounterPrintable", true);


            this.isDirtyBinding = this.checkBoxIsDirty.DataBindings.Add("Checked", this.dataMessageBLL.DataMessageMaster, "IsDirty", true);
            this.isDirtyBLLBinding = this.checkBoxIsDirtyBLL.DataBindings.Add("Checked", this.dataMessageBLL, "IsDirty", true);


            this.logoIDBinding.BindingComplete += new BindingCompleteEventHandler(CommonControl_BindingComplete);
            this.factoryIDBinding.BindingComplete += new BindingCompleteEventHandler(CommonControl_BindingComplete);
            this.ownerIDBinding.BindingComplete += new BindingCompleteEventHandler(CommonControl_BindingComplete);
            this.categoryIDBinding.BindingComplete += new BindingCompleteEventHandler(CommonControl_BindingComplete);
            this.productIDBinding.BindingComplete += new BindingCompleteEventHandler(CommonControl_BindingComplete);
            this.coilIDBinding.BindingComplete += new BindingCompleteEventHandler(CommonControl_BindingComplete);
            this.requestedEmployeeIDBinding.BindingComplete += new BindingCompleteEventHandler(CommonControl_BindingComplete);


            this.coilExtensionBinding.BindingComplete += new BindingCompleteEventHandler(CommonControl_BindingComplete);
            this.remarksBinding.BindingComplete += new BindingCompleteEventHandler(CommonControl_BindingComplete);

            this.productionDateBinding.BindingComplete += new BindingCompleteEventHandler(CommonControl_BindingComplete);

            this.productionDatePrintableBinding.BindingComplete += new BindingCompleteEventHandler(CommonControl_BindingComplete);
            this.logoPrintableBinding.BindingComplete += new BindingCompleteEventHandler(CommonControl_BindingComplete);
            this.factoryPrintableBinding.BindingComplete += new BindingCompleteEventHandler(CommonControl_BindingComplete);
            this.ownerPrintableBinding.BindingComplete += new BindingCompleteEventHandler(CommonControl_BindingComplete);
            this.categoryPrintableBinding.BindingComplete += new BindingCompleteEventHandler(CommonControl_BindingComplete);
            this.productPrintableBinding.BindingComplete += new BindingCompleteEventHandler(CommonControl_BindingComplete);
            this.coilPrintableBinding.BindingComplete += new BindingCompleteEventHandler(CommonControl_BindingComplete);
            this.counterPrintableBinding.BindingComplete += new BindingCompleteEventHandler(CommonControl_BindingComplete);

            this.isDirtyBinding.BindingComplete += new BindingCompleteEventHandler(CommonControl_BindingComplete);
            this.isDirtyBLLBinding.BindingComplete += new BindingCompleteEventHandler(CommonControl_BindingComplete);

            this.naviGroupDetails.DataBindings.Add("ExpandedHeight", this.numericUpDownSizingDetail, "Value", true, DataSourceUpdateMode.OnPropertyChanged);
            this.numericUpDownSizingDetail.Minimum = this.naviGroupDetails.HeaderHeight * 2;
            this.numericUpDownSizingDetail.Maximum = this.naviGroupDetails.Height + this.dataGridViewDataMessageMaster.Height;

            this.tableLayoutPanelMaster.ColumnStyles[this.tableLayoutPanelMaster.ColumnCount - 1].SizeType = SizeType.Absolute; this.tableLayoutPanelMaster.ColumnStyles[this.tableLayoutPanelMaster.ColumnCount - 1].Width = 15;

            this.errorProviderMaster.DataSource = this.dataMessageBLL.DataMessageMaster;
        }



        private void CommonControl_BindingComplete(object sender, BindingCompleteEventArgs e)
        {
            try
            {
                if (e.BindingCompleteState == BindingCompleteState.Exception)
                { GlobalExceptionHandler.ShowExceptionMessageBox(this, e.ErrorText); e.Cancel = true; }

                else
                    if (sender is Binding)
                    {
                        if (sender.Equals(this.logoIDBinding))
                        {
                            this.dataMessageBLL.DataMessageMaster.LogoName = (this.comboBoxLogoID.SelectedIndex != -1 && this.comboBoxLogoID.SelectedItem != null) ? (string)((DataRowView)this.comboBoxLogoID.SelectedItem)["Description"] : "";
                            this.dataMessageBLL.DataMessageMaster.LogoLogo = (this.comboBoxLogoID.SelectedIndex != -1 && this.comboBoxLogoID.SelectedItem != null) ? (string)((DataRowView)this.comboBoxLogoID.SelectedItem)["LogoGenerator"] : "";
                        }

                        if (sender.Equals(this.factoryIDBinding))
                        {
                            this.dataMessageBLL.DataMessageMaster.FactoryName = (this.comboBoxFactoryID.SelectedIndex != -1 && this.comboBoxFactoryID.SelectedItem != null) ? (string)((DataRowView)this.comboBoxFactoryID.SelectedItem)["Description"] : "";
                            this.dataMessageBLL.DataMessageMaster.FactoryLogo = (this.comboBoxFactoryID.SelectedIndex != -1 && this.comboBoxFactoryID.SelectedItem != null) ? (string)((DataRowView)this.comboBoxFactoryID.SelectedItem)["LogoGenerator"] : "";
                        }

                        if (sender.Equals(this.ownerIDBinding))
                        {
                            this.dataMessageBLL.DataMessageMaster.OwnerName = (this.comboBoxOwnerID.SelectedIndex != -1 && this.comboBoxOwnerID.SelectedItem != null) ? (string)((DataRowView)this.comboBoxOwnerID.SelectedItem)["Description"] : "";
                            this.dataMessageBLL.DataMessageMaster.OwnerLogo = (this.comboBoxOwnerID.SelectedIndex != -1 && this.comboBoxOwnerID.SelectedItem != null) ? (string)((DataRowView)this.comboBoxOwnerID.SelectedItem)["LogoGenerator"] : "";
                        }

                        if (sender.Equals(this.categoryIDBinding))
                        {
                            this.dataMessageBLL.DataMessageMaster.CategoryName = (this.comboBoxCategoryID.SelectedIndex != -1 && this.comboBoxCategoryID.SelectedItem != null) ? (string)((DataRowView)this.comboBoxCategoryID.SelectedItem)["Description"] : "";
                            this.dataMessageBLL.DataMessageMaster.CategoryLogo = (this.comboBoxCategoryID.SelectedIndex != -1 && this.comboBoxCategoryID.SelectedItem != null) ? (string)((DataRowView)this.comboBoxCategoryID.SelectedItem)["LogoGenerator"] : "";
                        }

                        if (sender.Equals(this.productIDBinding))
                        {
                            this.dataMessageBLL.DataMessageMaster.ProductName = (this.comboBoxProductID.SelectedIndex != -1 && this.comboBoxProductID.SelectedItem != null) ? (string)((DataRowView)this.comboBoxProductID.SelectedItem)["Description"] : "";
                            this.dataMessageBLL.DataMessageMaster.ProductLogo = (this.comboBoxProductID.SelectedIndex != -1 && this.comboBoxProductID.SelectedItem != null) ? (string)((DataRowView)this.comboBoxProductID.SelectedItem)["LogoGenerator"] : "";
                        }

                        if (sender.Equals(this.coilIDBinding))
                            this.dataMessageBLL.DataMessageMaster.CoilCode = (this.comboBoxCoilID.SelectedIndex != -1 && this.comboBoxCoilID.SelectedItem != null) ? (string)((DataRowView)this.comboBoxCoilID.SelectedItem)["Description"] : "";

                    }
            }
            catch (Exception exception)
            {
                GlobalExceptionHandler.ShowExceptionMessageBox(this, exception);
            }
        }

        private void numericUpDownSizingDetail_ValueChanged(object sender, EventArgs e)
        {
            this.naviGroupDetails.Expand();
        }

        private void naviGroupDetails_HeaderMouseClick(object sender, MouseEventArgs e)
        {
            this.numericUpDownSizingDetail.Visible = this.naviGroupDetails.Expanded;
        }



        private void InitializeDataGridBinding()
        {

            this.dataGridViewDataMessageMaster.ColumnHeadersDefaultCellStyle.Font = new Font(this.dataGridViewDataMessageMaster.DefaultCellStyle.Font.Name, 9, FontStyle.Regular);
            this.dataGridViewDataMessageMaster.ColumnHeadersDefaultCellStyle.Padding = new Padding(0, 3, 0, 3);
            this.dataGridViewDataMessageMaster.AutoGenerateColumns = false;
            dataMessageMasterListView = new BindingListView<DataMessageMaster>(this.dataMessageBLL.DataMessageMasterList);
            this.dataGridViewDataMessageMaster.DataSource = dataMessageMasterListView;



            #region <dataGridViewDetail>

            #endregion <dataGridViewDetail>
        }


        private void InitializeReadOnlyModeBinding()
        {
            List<Control> controlList = GlobalStaticFunction.GetAllControls(this);

            foreach (Control control in controlList)
            {
                //if (control is TextBox) control.DataBindings.Add("Readonly", this, "ReadonlyMode");
                if (control is TextBox)
                    control.DataBindings.Add("Enabled", this, "EditableMode");
                else if (control is ComboBox || control is DateTimePicker || control is CheckBox) control.DataBindings.Add("Enabled", this, "EditableMode");
                else if (control is DataGridView && !control.Equals(this.dataGridViewDataMessageMaster))
                {
                    control.DataBindings.Add("Readonly", this, "ReadonlyMode");
                    control.DataBindings.Add("AllowUserToAddRows", this, "EditableMode");
                    control.DataBindings.Add("AllowUserToDeleteRows", this, "EditableMode");
                }
            }

            this.dataGridViewDataMessageMaster.DataBindings.Add("Enabled", this, "ReadonlyMode");

        }

        BindingListView<DataMessageMaster> dataMessageMasterListView;
        BindingListView<DataMessageDetail> dataMessageDetailListView;


        #endregion <InitializeBinding>





        #region Import Excel

        private void ImportExcel()
        {
            try
            {

            }
            catch (Exception exception)
            {
                GlobalExceptionHandler.ShowExceptionMessageBox(this, exception);
            }

        }
        #endregion Import Excel




        private void dataGridViewDataMessageMaster_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {
            try
            {
                using (SolidBrush b = new SolidBrush(dataGridViewDataMessageMaster.RowHeadersDefaultCellStyle.ForeColor))
                {
                    e.Graphics.DrawString((e.RowIndex + 1).ToString(), e.InheritedRowStyle.Font, b, e.RowBounds.Location.X + 15, e.RowBounds.Location.Y + 4);
                }
            }
            catch (Exception exception)
            {
                GlobalExceptionHandler.ShowExceptionMessageBox(this, exception);
            }
        }

        private void dataGridViewDataMessageMaster_SelectionChanged(object sender, EventArgs e)
        {
            if (this.dataGridViewDataMessageMaster.SelectedRows.Count > 0 && this.dataGridViewDataMessageMaster.CurrentRow != null)
            {
                try
                {
                    DataGridViewRow dataGridViewRow = this.dataGridViewDataMessageMaster.CurrentRow;
                    if (dataGridViewRow != null)
                    {
                        Equin.ApplicationFramework.ObjectView<DataMessageMaster> equinObjectView = dataGridViewRow.DataBoundItem as Equin.ApplicationFramework.ObjectView<DataMessageMaster>;

                        if (equinObjectView != null)
                        {
                            this.dataMessageBLL.Fill(((DataMessageMaster)equinObjectView).DataMessageID);

                            //this.toolStripTextBoxDisplay1.TextBox.DataBindings.Clear();
                            //this.toolStripTextBoxDisplay1.TextBox.DataBindings.Add("Text", this.dataMessageBLL.DataMessageMaster, "Display1", true);

                            //this.toolStripTextBoxDisplay2.TextBox.DataBindings.Clear();
                            //this.toolStripTextBoxDisplay2.TextBox.DataBindings.Add("Text", this.dataMessageBLL.DataMessageMaster, "Display2", true);

                            //this.toolStripTextBoxDisplay3.TextBox.DataBindings.Clear();
                            //this.toolStripTextBoxDisplay3.TextBox.DataBindings.Add("Text", this.dataMessageBLL.DataMessageMaster, "Display3", true);

                            //this.toolStripTextBoxDisplay4.TextBox.DataBindings.Clear();
                            //this.toolStripTextBoxDisplay4.TextBox.DataBindings.Add("Text", this.dataMessageBLL.DataMessageMaster, "Display4", true);
                        }
                    }

                    this.dataGridViewDataMessageMaster.EditMode = DataGridViewEditMode.EditProgrammatically;
                }
                catch (Exception exception)
                {
                    GlobalExceptionHandler.ShowExceptionMessageBox(this, exception);
                }
            }

        }

        private void dataGridViewDataMessageMaster_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (this.dataMessageBLL != null && this.IsDirty) this.dataMessageBLL.Save();
            }
            catch (Exception exception)
            {
                GlobalExceptionHandler.ShowExceptionMessageBox(this, exception);
            }
        }

        private void dataGridViewDataMessageMaster_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            this.dataGridViewDataMessageMaster.EditMode = DataGridViewEditMode.EditOnF2;
        }

        private void dataGridViewDataMessageMaster_DoubleClick(object sender, EventArgs e)
        {
            this.naviGroupDetails.Expanded = true;
        }


        private void labelOpenCommonMDI_DoubleClick(object sender, EventArgs e)
        {
            try
            {
                GlobalEnum.TaskID taskID = sender.Equals(this.pictureBoxLogo) ? GlobalEnum.TaskID.ListLogo : sender.Equals(this.pictureBoxFactory) ? GlobalEnum.TaskID.ListFactory : sender.Equals(this.pictureBoxOwner) ? GlobalEnum.TaskID.ListOwner : sender.Equals(this.pictureBoxCategory) ? GlobalEnum.TaskID.ListCategory : sender.Equals(this.pictureBoxProduct) ? GlobalEnum.TaskID.ListProduct : sender.Equals(this.pictureBoxCoil) ? GlobalEnum.TaskID.ListCoil : GlobalEnum.TaskID.ListCoil;

                CommonMDI commonMDI = new CommonMDI(taskID);
                if (commonMDI.ShowDialog() == System.Windows.Forms.DialogResult.OK || true)
                {
                    CommonMetaList commonMetaList = new CommonMetaList();
                    switch (taskID)
                    {
                        case GlobalEnum.TaskID.ListLogo:
                            this.comboBoxLogoID.DataSource = commonMetaList.GetListLogo();
                            break;
                        case GlobalEnum.TaskID.ListFactory:
                            this.comboBoxFactoryID.DataSource = commonMetaList.GetListFactory();
                            break;
                        case GlobalEnum.TaskID.ListOwner:
                            this.comboBoxOwnerID.DataSource = commonMetaList.GetListOwner();
                            break;
                        case GlobalEnum.TaskID.ListCategory:
                            this.comboBoxCategoryID.DataSource = commonMetaList.GetListCategory();
                            break;
                        case GlobalEnum.TaskID.ListProduct:
                            this.comboBoxProductID.DataSource = commonMetaList.GetListProduct();
                            break;
                        case GlobalEnum.TaskID.ListCoil:
                            this.comboBoxCoilID.DataSource = commonMetaList.GetListCoil();
                            break;
                        default:
                            break;
                    }
                }
                commonMDI.Dispose();
            }
            catch (Exception exception)
            {
                GlobalExceptionHandler.ShowExceptionMessageBox(this, exception);
            }
        }










    }




}
