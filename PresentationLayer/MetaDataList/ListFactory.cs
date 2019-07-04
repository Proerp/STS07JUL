﻿using System;

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
    public partial class ListFactory : Form, IMergeToolStrip, ICallToolStrip
    {

        #region <Implement Interface>

        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null) PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        public GlobalEnum.TaskID TaskID
        {
            get { return this.listFactoryBLL.TaskID; }
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
            get { return this.listFactoryBLL.IsDirty; }
        }

        public bool IsValid
        {
            get { return this.listFactoryBLL.IsValid; }
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
            get { return !this.listFactoryBLL.ReadOnly; }
        }

        public bool Editable
        {
            get { return !this.listFactoryBLL.ReadOnly; }
        }

        public bool Deletable
        {
            get { return this.listFactoryBLL.Editable; }
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
            get { return this.listFactoryBLL.Verifiable; }
        }

        public bool Unverifiable
        {
            get { return this.listFactoryBLL.Unverifiable; }
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
                this.lastAddressAreaID = this.listFactoryBLL.FactoryID;
                this.editableMode = editableMode;
                this.NotifyPropertyChanged("EditableMode");

                this.naviGroupDetails.Expanded = editableMode;
            }
        }


        private void CancelDirty(bool restoreSavedData)
        {
            try
            {
                if (restoreSavedData || this.listFactoryBLL.FactoryID <= 0)
                    this.listFactoryBLL.Fill(this.lastAddressAreaID);

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
            this.listFactoryBLL.New();
            this.SetEditableMode(true);
        }

        public void Edit()
        {
            this.SetEditableMode(true);
        }

        public void Save()
        {
            this.listFactoryBLL.Save();
            this.GetMasterList();
        }

        public void Delete()
        {
            DialogResult dialogResult = MessageBox.Show(this, "Bạn muốn xóa dữ liệu này " + this.listFactoryBLL.ListFactoryMaster.Description + "?", "Cảnh báo", MessageBoxButtons.YesNo, MessageBoxIcon.Stop);
            if (dialogResult == DialogResult.Yes)
            {
                try
                {
                    this.listFactoryBLL.Delete();
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
        private ListFactoryBLL listFactoryBLL;

        #endregion <Storage>


        public ListFactory()
        {
            try
            {
                InitializeComponent();

                this.listFactoryBLL = new ListFactoryBLL();

                this.listFactoryBLL.PropertyChanged += new PropertyChangedEventHandler(listFactoryBLL_PropertyChanged);

            }
            catch (Exception exception)
            {
                GlobalExceptionHandler.ShowExceptionMessageBox(this, exception);
            }
        }

        void listFactoryBLL_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            this.NotifyPropertyChanged(e.PropertyName);
        }


        private void ListFactory_Load(object sender, EventArgs e)
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
            this.descriptionBinding = this.textBoxDescription.DataBindings.Add("Text", this.listFactoryBLL.ListFactoryMaster, "Description", true);
            this.logoGeneratorBinding = this.textBoxLogoGenerator.DataBindings.Add("Text", this.listFactoryBLL.ListFactoryMaster, "LogoGenerator", true);
            this.remarksBinding = this.textBoxRemarks.DataBindings.Add("Text", this.listFactoryBLL.ListFactoryMaster, "Remarks", true);

            this.isDirtyBinding = this.checkBoxIsDirty.DataBindings.Add("Checked", this.listFactoryBLL.ListFactoryMaster, "IsDirty", true);
            this.isDirtyBLLBinding = this.checkBoxIsDirtyBLL.DataBindings.Add("Checked", this.listFactoryBLL, "IsDirty", true);


            this.descriptionBinding.BindingComplete += new BindingCompleteEventHandler(CommonControl_BindingComplete);
            this.logoGeneratorBinding.BindingComplete += new BindingCompleteEventHandler(CommonControl_BindingComplete);
            this.remarksBinding.BindingComplete += new BindingCompleteEventHandler(CommonControl_BindingComplete);

            this.isDirtyBinding.BindingComplete += new BindingCompleteEventHandler(CommonControl_BindingComplete);
            this.isDirtyBLLBinding.BindingComplete += new BindingCompleteEventHandler(CommonControl_BindingComplete);

            this.naviGroupDetails.DataBindings.Add("ExpandedHeight", this.numericUpDownSizingDetail, "Value", true, DataSourceUpdateMode.OnPropertyChanged);
            this.numericUpDownSizingDetail.Minimum = this.naviGroupDetails.HeaderHeight * 2;
            this.numericUpDownSizingDetail.Maximum = this.naviGroupDetails.Height + this.dataListViewMaster.Height;

            this.tableLayoutPanelMaster.ColumnStyles[this.tableLayoutPanelMaster.ColumnCount - 1].SizeType = SizeType.Absolute; this.tableLayoutPanelMaster.ColumnStyles[this.tableLayoutPanelMaster.ColumnCount - 1].Width = 10;

            this.errorProviderMaster.DataSource = this.listFactoryBLL.ListFactoryMaster;
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

            DataTable dataTableFactoryListing = this.listFactoryBLL.ListFactoryListing();
            dataTableFactoryListing.TableName = "ListFactoryListing";

            DataSet bindingDataSet = new DataSet();
            bindingDataSet.Tables.Add(dataTableFactoryListing);

            this.dataListViewMaster.AutoGenerateColumns = false;
            this.dataListViewMaster.DataSource = dataTableFactoryListing;
            
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

        BindingListView<ListFactoryDetail> listAddressAreaDetailView;

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
                    int FactoryID;

                    if (int.TryParse(row.Row["FactoryID"].ToString(), out FactoryID)) this.listFactoryBLL.Fill(FactoryID);
                    else this.listFactoryBLL.Fill(0);
                }
                else this.listFactoryBLL.Fill(0);
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
