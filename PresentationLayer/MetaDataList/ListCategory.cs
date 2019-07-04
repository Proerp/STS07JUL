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
using BusinessLogicLayer.MetaDataList;
using DataAccessLayer;
using DataTransferObject;
using DataTransferObject.MetaDataList;
using System.Globalization;
using BrightIdeasSoftware;
using Equin.ApplicationFramework;

namespace PresentationLayer
{
    public partial class ListCategory : Form, IMergeToolStrip, ICallToolStrip
    {

        #region <Implement Interface>

        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null) PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        public GlobalEnum.TaskID TaskID
        {
            get { return this.listCategoryBLL.TaskID; }
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
            get { return this.listCategoryBLL.IsDirty; }
        }

        public bool IsValid
        {
            get { return this.listCategoryBLL.IsValid; }
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
            get { return !this.listCategoryBLL.ReadOnly; }
        }

        public bool Editable
        {
            get { return !this.listCategoryBLL.ReadOnly; }
        }

        public bool Deletable
        {
            get { return this.listCategoryBLL.Editable; }
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
            get { return this.listCategoryBLL.Verifiable; }
        }

        public bool Unverifiable
        {
            get { return this.listCategoryBLL.Unverifiable; }
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
        private int lastAddressAreaID;
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
                this.lastAddressAreaID = this.listCategoryBLL.CategoryID;
                this.editableMode = editableMode;
                this.NotifyPropertyChanged("EditableMode");

                this.naviGroupDetails.Expanded = editableMode;
            }
        }


        private void CancelDirty(bool restoreSavedData)
        {
            try
            {
                if (restoreSavedData || this.listCategoryBLL.CategoryID <= 0)
                    this.listCategoryBLL.Fill(this.lastAddressAreaID);

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

        //Can phai xem lai trong VB de xem lai this.SetEditableMode () khi can thiet

        public void Loading()
        {
            this.GetMasterList();
        }

        public void New()
        {
            this.listCategoryBLL.New();
            this.SetEditableMode(true);
        }

        public void Edit()
        {
            this.SetEditableMode(true);
        }

        public void Save()
        {
            this.listCategoryBLL.Save();
            this.GetMasterList();
        }

        public void Delete()
        {
            DialogResult dialogResult = MessageBox.Show(this, "Bạn muốn xóa dữ liệu này " + this.listCategoryBLL.ListCategoryMaster.Description + "?", "Cảnh báo", MessageBoxButtons.YesNo, MessageBoxIcon.Stop);
            if (dialogResult == DialogResult.Yes)
            {
                try
                {
                    this.listCategoryBLL.Delete();
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
        private ListCategoryBLL listCategoryBLL;

        #endregion <Storage>


        public ListCategory()
        {
            try
            {
                InitializeComponent();

                this.listCategoryBLL = new ListCategoryBLL();

                this.listCategoryBLL.PropertyChanged += new PropertyChangedEventHandler(listCategoryBLL_PropertyChanged);

            }
            catch (Exception exception)
            {
                GlobalExceptionHandler.ShowExceptionMessageBox(this, exception);
            }
        }

        void listCategoryBLL_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            this.NotifyPropertyChanged(e.PropertyName);
        }


        private void ListCategory_Load(object sender, EventArgs e)
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

        Binding descriptionBinding;
        Binding logoGeneratorBinding;
        Binding remarksBinding;

        Binding isDirtyBinding;
        Binding isDirtyBLLBinding;

        private void InitializeCommonControlBinding()
        {
            this.descriptionBinding = this.textBoxDescription.DataBindings.Add("Text", this.listCategoryBLL.ListCategoryMaster, "Description", true);
            this.logoGeneratorBinding = this.textBoxLogoGenerator.DataBindings.Add("Text", this.listCategoryBLL.ListCategoryMaster, "LogoGenerator", true);
            this.remarksBinding = this.textBoxRemarks.DataBindings.Add("Text", this.listCategoryBLL.ListCategoryMaster, "Remarks", true);

            this.isDirtyBinding = this.checkBoxIsDirty.DataBindings.Add("Checked", this.listCategoryBLL.ListCategoryMaster, "IsDirty", true);
            this.isDirtyBLLBinding = this.checkBoxIsDirtyBLL.DataBindings.Add("Checked", this.listCategoryBLL, "IsDirty", true);


            this.descriptionBinding.BindingComplete += new BindingCompleteEventHandler(CommonControl_BindingComplete);
            this.logoGeneratorBinding.BindingComplete += new BindingCompleteEventHandler(CommonControl_BindingComplete);
            this.remarksBinding.BindingComplete += new BindingCompleteEventHandler(CommonControl_BindingComplete);

            this.isDirtyBinding.BindingComplete += new BindingCompleteEventHandler(CommonControl_BindingComplete);
            this.isDirtyBLLBinding.BindingComplete += new BindingCompleteEventHandler(CommonControl_BindingComplete);

            this.naviGroupDetails.DataBindings.Add("ExpandedHeight", this.numericUpDownSizingDetail, "Value", true, DataSourceUpdateMode.OnPropertyChanged);
            this.numericUpDownSizingDetail.Minimum = this.naviGroupDetails.HeaderHeight * 2;
            this.numericUpDownSizingDetail.Maximum = this.naviGroupDetails.Height + this.dataListViewMaster.Height;

            this.tableLayoutPanelMaster.ColumnStyles[this.tableLayoutPanelMaster.ColumnCount - 1].SizeType = SizeType.Absolute; this.tableLayoutPanelMaster.ColumnStyles[this.tableLayoutPanelMaster.ColumnCount - 1].Width = 10;

            this.errorProviderMaster.DataSource = this.listCategoryBLL.ListCategoryMaster;
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
            this.numericUpDownSizingDetail.Visible = false;// this.naviGroupDetails.Expanded;
        }


        private void GetMasterList()
        {

            foreach (OLVColumn olvColumn in dataListViewMaster.Columns) { olvColumn.Renderer = null; }

            DataTable dataTableCategoryListing = this.listCategoryBLL.ListCategoryListing();
            dataTableCategoryListing.TableName = "ListCategoryListing";

            DataSet bindingDataSet = new DataSet();
            bindingDataSet.Tables.Add(dataTableCategoryListing);

            this.dataListViewMaster.AutoGenerateColumns = false;
            this.dataListViewMaster.DataSource = dataTableCategoryListing;
            
        }

        private void InitializeDataGridBinding()
        {

            this.GetMasterList();



            #region <dataGridViewDetail>
            
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

        BindingListView<ListCategoryDetail> listAddressAreaDetailView;

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

        private void dataListViewMaster_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                ObjectListView objectListView = (ObjectListView)sender;
                DataRowView row = (DataRowView)objectListView.SelectedObject;

                if (row != null)
                {
                    int categoryID;

                    if (int.TryParse(row.Row["CategoryID"].ToString(), out categoryID)) this.listCategoryBLL.Fill(categoryID);
                    else this.listCategoryBLL.Fill(0);
                }
                else this.listCategoryBLL.Fill(0);
            }
            catch (Exception exception)
            {
                GlobalExceptionHandler.ShowExceptionMessageBox(this, exception);
            }
        }

        private void dataListViewMaster_DoubleClick(object sender, EventArgs e)
        {
            this.naviGroupDetails.Expanded = true;
        }



    }




}
