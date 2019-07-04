using System;

using System.Collections;

using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using System.Transactions;

using Global.Class.Library;
using BusinessLogicLayer;
using DataAccessLayer;
using DataTransferObject;
using System.Globalization;
using BrightIdeasSoftware;
using Equin.ApplicationFramework;
using BusinessLogicLayer.BarcodeScanner;
using PresentationLayer;

namespace PresentationLayer
{
    public partial class ControllingInterface : Form, IMergeToolStrip, ICallToolStrip
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
            get { return this.dataMessageBLL.Editable; }
        }

        public bool Deletable
        {
            get { return this.dataMessageBLL.Editable; }
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
            get { return true; }
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

            this.SetOnPrintingDataMessage(this.GetDataMessageMasterFromLoadingList(this.imageS8.DataMessageMaster.DataMessageID, GlobalEnum.DataStatusID.OnPrinting));
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
            DialogResult dialogResult = MessageBox.Show(this, "Bạn muốn xóa dữ liệu này: " + this.dataMessageBLL.DataMessageMaster.FactoryName + "?", "Cảnh báo", MessageBoxButtons.YesNo, MessageBoxIcon.Stop);
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

        private System.Timers.Timer timerBlinkWarning;
        private System.Timers.Timer timerLoading;

        delegate void SetTextCallback(string text);

        delegate void ThreadShowStatus(object sender, PropertyChangedEventArgs e);

        delegate void BlinkWarningCallback();
        delegate void LoadingCallBack();


        private ImageS8 imageS8;
        private Thread imageS8Thread;

        #endregion <Storage>


        public ControllingInterface()
        {
            try
            {
                InitializeComponent();

                InitializeTabControl();


                this.dataMessageBLL = new DataMessageBLL();

                this.dataMessageBLL.PropertyChanged += new PropertyChangedEventHandler(dataMessageBLL_PropertyChanged);


                //Barcode Scanner
                imageS8 = new ImageS8();

                imageS8.PropertyChanged += new PropertyChangedEventHandler(InkjetDominoPrinter_PropertyChanged);


                this.timerBlinkWarning = new System.Timers.Timer(1000);
                this.timerBlinkWarning.Elapsed += new System.Timers.ElapsedEventHandler(timerBlinkWarning_Elapsed);

                this.timerLoading = new System.Timers.Timer(60000);
                this.timerLoading.Elapsed += new System.Timers.ElapsedEventHandler(timerLoading_Elapsed);

            }
            catch (Exception exception)
            {
                GlobalExceptionHandler.ShowExceptionMessageBox(this, exception);
            }
        }

        void timerLoading_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            try
            {
                LoadingCallBack delegateLoadingCallBack = new LoadingCallBack(Loading);
                this.Invoke(delegateLoadingCallBack);
            }
            catch { }
        }


        private bool warningSetNextMessage;


        void timerBlinkWarning_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            try
            {
                BlinkWarningCallback delegateBlinkWarningCallback = new BlinkWarningCallback(BlinkWarningAction);
                this.Invoke(delegateBlinkWarningCallback);
            }
            catch { }
        }

        private void BlinkWarningAction()
        {
            try
            {
                if (imageS8.LoopRoutine || true)
                {
                    this.BlinkWarning(this.toolStripButtonSend, this.imageS8.WarningSetMessage, ResourceIcon.PlayNormal, true);
                    this.BlinkWarning(this.toolStripButtonCounterValue, this.imageS8.WarningCounterValue, ResourceIcon.InkJetPrinter, true);
                    this.BlinkWarning(this.toolStripButtonCounterAutonics, this.imageS8.WarningCounterAutonics, ResourceIcon.Counter, true);

                    this.BlinkWarning(this.toolStripButtonWarningSetNextMessage, this.warningSetNextMessage, ResourceIcon.MyWarning, false);





                    bool faultState = false;
                    foreach (GlobalVariables.PrinterFaultCategory printerFaultCategoryID in (GlobalVariables.PrinterFaultCategory[])Enum.GetValues(typeof(GlobalVariables.PrinterFaultCategory)))
                    {
                        if (printerFaultCategoryID != GlobalVariables.PrinterFaultCategory.PrintingSpeed && printerFaultCategoryID != GlobalVariables.PrinterFaultCategory.Head2Unserviceable)
                            faultState = faultState | this.imageS8.LedPrinterFaultState(printerFaultCategoryID);
                    }

                    if (faultState || this.imageS8.WarningSetMessage || this.imageS8.WarningCounterValue || this.imageS8.WarningCounterAutonics || this.warningSetNextMessage)
                        this.imageS8.SetIOPortRS232AutonisDSR(false);
                    else
                        this.imageS8.SetIOPortRS232AutonisDSR(true);



                }
                else
                    this.imageS8.SetIOPortRS232AutonisDSR(true);
            }
            catch { }
        }

        private void BlinkWarning(ToolStripButton blinkObject, bool blinkCondition, Icon blinkIcon, bool invisible)
        {
            try
            {
                if (!blinkCondition)
                {
                    //blinkObject.Enabled = invisible ? true : false;

                    blinkObject.Image = invisible ? blinkIcon.ToBitmap() : null;
                    blinkObject.Text = invisible ? blinkObject.Tag.ToString() : "";
                }
                else
                {
                    //blinkObject.Enabled = !blinkObject.Enabled; 

                    blinkObject.Image = blinkObject.Image == null ? blinkIcon.ToBitmap() : null;
                    blinkObject.Text = blinkObject.Image == null ? "" : blinkObject.Tag.ToString();
                }
            }
            catch { }
        }

        void dataMessageBLL_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            this.NotifyPropertyChanged(e.PropertyName);
            if (sender.Equals(this.dataMessageBLL) && e.PropertyName == "DataMessageID")
            {
                this.naviGroupDetails.Caption = " Nhật ký gửi bản in" + "           < " + this.dataMessageBLL.DataMessageMaster.Display1 + " " + this.dataMessageBLL.DataMessageMaster.Display2 + " " + this.dataMessageBLL.DataMessageMaster.Display3 + " " + this.dataMessageBLL.DataMessageMaster.Display4 + " " + this.dataMessageBLL.DataMessageMaster.Display5 + " " + this.dataMessageBLL.DataMessageMaster.Display6 + " >";
                this.naviGroupDetails.Refresh();
            }
        }

        private void InkjetDominoPrinter_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            try
            {
                ThreadShowStatus delegateThreadShowStatus = new ThreadShowStatus(inkjetDominoPrinterShowStatus);
                this.Invoke(delegateThreadShowStatus, new object[] { sender, e });
            }
            catch (Exception exception)
            {
                GlobalExceptionHandler.ShowExceptionMessageBox(this, exception);
            }
        }

        private void inkjetDominoPrinterShowStatus(object sender, PropertyChangedEventArgs e)
        {
            try
            {
                //this.SetToolStripActive();

                if (sender.Equals(this.imageS8))
                {
                    if (e.PropertyName == "MainStatus") { this.toolStripLabelMainStatus.Text = "[" + DateTime.Now.ToString("hh:mm:ss") + "] " + this.imageS8.MainStatus + "\r\n" + this.toolStripLabelMainStatus.Text; this.CutTextBox(false); return; }
                    //if (e.PropertyName == "LedStatus") { this.toolStripScannerLEDGreen.Enabled = this.imageS8.LedGreenOn; this.toolStripScannerLEDAmber.Enabled = this.imageS8.LedAmberOn; this.toolStripScannerLEDRed.Enabled = this.imageS8.LedRedOn; if (this.imageS8.LedRedOn) this.StopPrint(); return; }

                    if (e.PropertyName == "PrinterReady") { this.toolStripButtonConnect.Text = this.imageS8.PrinterReady ? "Đang kết nối" : (this.imageS8.LoopRoutine ? "Lỗi kết nối" : "Ngắt kết nối"); this.toolStripButtonConnect.Image = this.imageS8.PrinterReady ? Properties.Resources.Connecting : Properties.Resources.Disconnected; return; } //this.toolStripButtonConnect.Enabled = this.imageS8.PrinterReady; 

                    if (e.PropertyName == "CounterValue") { this.toolStripTextBoxImageS8Counter.Text = this.imageS8.CounterValue.ToString("#0"); return; }

                    if (e.PropertyName == "CounterAutonics") { this.toolStripTextBoxAutonicsCounter.Text = this.imageS8.CounterAutonics.ToString("#0"); return; }

                    if (e.PropertyName == "UpdateCounterValue") { this.imageS8.DataMessageMaster.CounterValue = this.imageS8.CounterValue; return; }

                    if (e.PropertyName == "UpdateCounterAutonics") { this.imageS8.DataMessageMaster.CounterAutonics = this.imageS8.CounterAutonics; return; }

                    if (e.PropertyName == "AddDataMessageDetail") { if (this.dataMessageBLL.DataMessageID == this.imageS8.DataMessageMaster.DataMessageID) this.dataMessageBLL.Fill(this.dataMessageBLL.DataMessageID); return; } //Enforce to reload DataMessageDetail

                    if (e.PropertyName == "LedPrinterFaultState") { this.ledInkLevelLow.Enabled = this.imageS8.LedPrinterFaultState(GlobalVariables.PrinterFaultCategory.InkLevelLow); this.ledPressure.Enabled = this.imageS8.LedPrinterFaultState(GlobalVariables.PrinterFaultCategory.Pressure); this.ledHardCPU.Enabled = this.imageS8.LedPrinterFaultState(GlobalVariables.PrinterFaultCategory.HardCPU); this.ledMemoryLost.Enabled = this.imageS8.LedPrinterFaultState(GlobalVariables.PrinterFaultCategory.MemoryLost) || this.imageS8.LedPrinterFaultState(GlobalVariables.PrinterFaultCategory.HardCPU) || this.imageS8.LedPrinterFaultState(GlobalVariables.PrinterFaultCategory.MotoCycle) || this.imageS8.LedPrinterFaultState(GlobalVariables.PrinterFaultCategory.InkPigmentCircuit); this.ledPrintingSpeed.Enabled = this.imageS8.LedPrinterFaultState(GlobalVariables.PrinterFaultCategory.PrintingSpeed); this.ledHead1Unserviceable.Enabled = this.imageS8.LedPrinterFaultState(GlobalVariables.PrinterFaultCategory.Head1Unserviceable); this.ledMotoCycle.Enabled = this.imageS8.LedPrinterFaultState(GlobalVariables.PrinterFaultCategory.MotoCycle); this.ledInkPigmentCircuit.Enabled = this.imageS8.LedPrinterFaultState(GlobalVariables.PrinterFaultCategory.InkPigmentCircuit); return; }

                    if (e.PropertyName == "LedPrinterFaultStatus") { this.ledHead1Unserviceable.ToolTipText = this.imageS8.LedPrinterFaultStatus(GlobalVariables.PrinterFaultCategory.Head1Unserviceable); return; }

                    if (e.PropertyName == "CDChanged")
                    {
                        if (this.UpdateDataStatus(this.imageS8.DataMessageMaster.DataMessageID, GlobalEnum.DataStatusID.PrintFinished))
                        {
                            this.imageS8.DataMessageMaster.DataStatusID = (int)GlobalEnum.DataStatusID.PrintFinished;
                            this.imageS8.AddDataMessageDetail("Kết thúc cuộn", "", this.imageS8.CounterValue, "", "");

                            this.imageS8.MainStatus = "Kết thúc cuộn";

                            //Auto set new coil (if able)
                            DataMessageMaster selectedDataMessageMaster = this.GetDataMessageMasterFromLoadingList(-1, GlobalEnum.DataStatusID.WaitForPrint);

                            if (selectedDataMessageMaster != null && this.UpdateDataStatus(selectedDataMessageMaster.DataMessageID, GlobalEnum.DataStatusID.OnPrinting))
                            {
                                this.SetOnPrintingDataMessage(this.GetDataMessageMasterFromLoadingList(selectedDataMessageMaster.DataMessageID, GlobalEnum.DataStatusID.OnPrinting));
                                if (this.imageS8.DataMessageMaster.DataMessageID > 0) this.imageS8.MainStatus = "Cài đặt cuộn mới";
                            }
                            else
                                this.SetOnPrintingDataMessage(null);



                            this.imageS8.ClearCDChanged();
                            this.imageS8.SetMessage();
                        }
                    }



                }

            }
            catch (Exception exception)
            {
                GlobalExceptionHandler.ShowExceptionMessageBox(this, exception);
            }
        }

        private void CutTextBox(bool clearTextBox)
        {
            if (clearTextBox)
            {
                this.toolStripLabelMainStatus.Text = "";
            }
            else
            {
                if (this.toolStripLabelMainStatus.Text.Length > 300) this.toolStripLabelMainStatus.Text = this.toolStripLabelMainStatus.Text.Substring(0, 300);
            }
        }


        private void InitializeTabControl()
        {

            //CustomTabControl customTabControlMarketingIncentiveDetail = new CustomTabControl();

            //customTabControlMarketingIncentiveDetail.ImageList = this.imageListTabControl;

            //customTabControlMarketingIncentiveDetail.TabPages.Add("MarketingIncentiveDetail", "Apply to Customer(s)", 0);
            //customTabControlMarketingIncentiveDetail.TabPages[0].Controls.Add(this.dataGridViewMarketingIncentiveDetail);
            //this.dataGridViewMarketingIncentiveDetail.Dock = DockStyle.Fill;

            //customTabControlMarketingIncentiveDetail.DisplayStyle = TabStyle.VisualStudio;
            //customTabControlMarketingIncentiveDetail.DisplayStyleProvider.ImageAlign = ContentAlignment.MiddleLeft;
            //this.splitContainerMechanicScheme.Panel2.Controls.Add(customTabControlMarketingIncentiveDetail);
            //customTabControlMarketingIncentiveDetail.Dock = DockStyle.Fill;
        }

        private void ControllingInterface_Load(object sender, EventArgs e)
        {
            try
            {
                InitializeCommonControlBinding();

                InitializeDataGridBinding();

                InitializeReadOnlyModeBinding();

                this.Loading();

                imageS8.Initialize();

                toolStripButtonConnect_Click(this.toolStripButtonConnect, new EventArgs());


                this.timerLoading.Enabled = true;
            }
            catch (Exception exception)
            {
                GlobalExceptionHandler.ShowExceptionMessageBox(this, exception);
            }
        }

        private void ControllingInterface_Activated(object sender, EventArgs e)
        {
            try
            {
                if (!this.timerBlinkWarning.Enabled)
                {
                    Thread.Sleep(500);
                    this.timerBlinkWarning.Enabled = true;
                }
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

        Binding beginingDateBinding;
        Binding endingDateBinding;

        private void InitializeCommonControlBinding()
        {
            this.beginingDateBinding = this.textBoxLowerFillterDate.TextBox.DataBindings.Add("Text", GlobalVariables.GlobalOptionSetting, "LowerFillterDate", true);
            this.endingDateBinding = this.textBoxUpperFillterDate.TextBox.DataBindings.Add("Text", GlobalVariables.GlobalOptionSetting, "UpperFillterDate", true);

            this.beginingDateBinding.BindingComplete += new BindingCompleteEventHandler(CommonControl_BindingComplete);
            this.endingDateBinding.BindingComplete += new BindingCompleteEventHandler(CommonControl_BindingComplete);


            ////CommonMetaList commonMetaList = new CommonMetaList();


            ////ERmgrUIP.MarketingProgramListDataTable marketingProgramListDataTable = commonMetaList.GetMarketingProgramList();
            ////this.comboBoxMarketingProgramID.DataSource = marketingProgramListDataTable;
            ////this.comboBoxMarketingProgramID.DisplayMember = marketingProgramListDataTable.ReferenceColumn.ColumnName;
            ////this.comboBoxMarketingProgramID.ValueMember = marketingProgramListDataTable.MarketingProgramIDColumn.ColumnName;
            ////this.marketingProgramIDBinding = this.comboBoxMarketingProgramID.DataBindings.Add("SelectedValue", this.dataMessageBLL.DataMessageMaster, "MarketingProgramID", true);


            ////ERmgrUIP.ListMarketingPaymentTypeDataTable marketingPaymentTypeListDataTable = commonMetaList.GetMarketingPaymentType();
            ////this.comboBoxMarketingPaymentTypeID.DataSource = marketingPaymentTypeListDataTable;
            ////this.comboBoxMarketingPaymentTypeID.DisplayMember = marketingPaymentTypeListDataTable.DescriptionColumn.ColumnName;
            ////this.comboBoxMarketingPaymentTypeID.ValueMember = marketingPaymentTypeListDataTable.MarketingPaymentTypeIDColumn.ColumnName;
            ////this.marketingPaymentTypeIDBinding = this.comboBoxMarketingPaymentTypeID.DataBindings.Add("SelectedValue", this.dataMessageBLL.DataMessageMaster, "MarketingPaymentTypeID", true);



            ////ERmgrUIP.ListStaffNameDataTable listStaffNameDataTable = commonMetaList.GetStaffName();
            ////this.comboBoxRequestedEmployeeID.DataSource = listStaffNameDataTable;
            ////this.comboBoxRequestedEmployeeID.DisplayMember = listStaffNameDataTable.DescriptionOfficialColumn.ColumnName;
            ////this.comboBoxRequestedEmployeeID.ValueMember = listStaffNameDataTable.StaffIDColumn.ColumnName;
            ////this.requestedEmployeeIDBinding = this.comboBoxRequestedEmployeeID.DataBindings.Add("SelectedValue", this.dataMessageBLL.DataMessageMaster, "RequestedEmployeeID", true);

            ////listStaffNameDataTable = (ERmgrUIP.ListStaffNameDataTable)listStaffNameDataTable.Copy();
            ////this.comboBoxNotedEmployeeID.DataSource = listStaffNameDataTable;
            ////this.comboBoxNotedEmployeeID.DisplayMember = listStaffNameDataTable.DescriptionOfficialColumn.ColumnName;
            ////this.comboBoxNotedEmployeeID.ValueMember = listStaffNameDataTable.StaffIDColumn.ColumnName;
            ////this.notedEmployeeIDBinding = this.comboBoxNotedEmployeeID.DataBindings.Add("SelectedValue", this.dataMessageBLL.DataMessageMaster, "NotedEmployeeID", true);

            ////listStaffNameDataTable = (ERmgrUIP.ListStaffNameDataTable)listStaffNameDataTable.Copy();
            ////this.comboBoxApprovedEmployeeID.DataSource = listStaffNameDataTable;
            ////this.comboBoxApprovedEmployeeID.DisplayMember = listStaffNameDataTable.DescriptionOfficialColumn.ColumnName;
            ////this.comboBoxApprovedEmployeeID.ValueMember = listStaffNameDataTable.StaffIDColumn.ColumnName;
            ////this.approvedEmployeeIDBinding = this.comboBoxApprovedEmployeeID.DataBindings.Add("SelectedValue", this.dataMessageBLL.DataMessageMaster, "ApprovedEmployeeID", true);


            ////this.paymentPeriodBinding = this.textBoxPaymentPeriod.DataBindings.Add("Text", this.dataMessageBLL.DataMessageMaster, "PaymentPeriod", true);
            ////this.paymentMachanicsBinding = this.textBoxPaymentMachanics.DataBindings.Add("Text", this.dataMessageBLL.DataMessageMaster, "PaymentMachanics", true);
            ////this.remarksBinding = this.textBoxRemarks.DataBindings.Add("Text", this.dataMessageBLL.DataMessageMaster, "Remarks", true);


            ////this.requestedDateBinding = this.dateTimePickerRequestedDate.DataBindings.Add("Value", this.dataMessageBLL.DataMessageMaster, "RequestedDate", true);

            this.isDirtyBinding = this.checkBoxIsDirty.DataBindings.Add("Checked", this.dataMessageBLL.DataMessageMaster, "IsDirty", true);
            this.isDirtyBLLBinding = this.checkBoxIsDirtyBLL.DataBindings.Add("Checked", this.dataMessageBLL, "IsDirty", true);



            ////this.marketingProgramIDBinding.BindingComplete += new BindingCompleteEventHandler(CommonControl_BindingComplete);
            ////this.marketingPaymentTypeIDBinding.BindingComplete += new BindingCompleteEventHandler(CommonControl_BindingComplete);
            ////this.requestedEmployeeIDBinding.BindingComplete += new BindingCompleteEventHandler(CommonControl_BindingComplete);
            ////this.notedEmployeeIDBinding.BindingComplete += new BindingCompleteEventHandler(CommonControl_BindingComplete);
            ////this.approvedEmployeeIDBinding.BindingComplete += new BindingCompleteEventHandler(CommonControl_BindingComplete);


            ////this.paymentPeriodBinding.BindingComplete += new BindingCompleteEventHandler(CommonControl_BindingComplete);
            ////this.paymentMachanicsBinding.BindingComplete += new BindingCompleteEventHandler(CommonControl_BindingComplete);
            ////this.remarksBinding.BindingComplete += new BindingCompleteEventHandler(CommonControl_BindingComplete);

            ////this.requestedDateBinding.BindingComplete += new BindingCompleteEventHandler(CommonControl_BindingComplete);



            //this.toolStripTextBoxDisplay1.TextBox.DataBindings.Add("Text", this.imageS8.DataMessageMaster, "Display1", true);
            //this.toolStripTextBoxDisplay2.TextBox.DataBindings.Add("Text", this.imageS8.DataMessageMaster, "Display2", true);
            //this.toolStripTextBoxDisplay3.TextBox.DataBindings.Add("Text", this.imageS8.DataMessageMaster, "Display3", true);
            //this.toolStripTextBoxDisplay4.TextBox.DataBindings.Add("Text", this.imageS8.DataMessageMaster, "Display4", true);




            this.isDirtyBinding.BindingComplete += new BindingCompleteEventHandler(CommonControl_BindingComplete);
            this.isDirtyBLLBinding.BindingComplete += new BindingCompleteEventHandler(CommonControl_BindingComplete);

            this.naviGroupDetails.DataBindings.Add("ExpandedHeight", this.numericUpDownSizingDetail, "Value", true, DataSourceUpdateMode.OnPropertyChanged);
            this.numericUpDownSizingDetail.Minimum = this.naviGroupDetails.HeaderHeight * 2;
            this.numericUpDownSizingDetail.Maximum = this.naviGroupDetails.Height + this.dataGridViewDataMessageMaster.Height;

        }


        private void CommonControl_BindingComplete(object sender, BindingCompleteEventArgs e)
        {
            if (e.BindingCompleteState != BindingCompleteState.Success) { GlobalExceptionHandler.ShowExceptionMessageBox(this, e.ErrorText); e.Cancel = true; }
        }

        private void dataGridViewDataMessageMaster_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            try
            {
                { this.toolStripLabelMainStatus.Text = "[" + DateTime.Now.ToString("hh:mm:ss") + "] " + e.Exception.Message + "\r\n" + this.toolStripLabelMainStatus.Text; this.CutTextBox(false); return; }
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
            this.toolStripNaviGroupDetails.Visible = this.naviGroupDetails.Expanded;
        }

        private void toolStripButtonShowDetailsExtend_Click(object sender, EventArgs e)
        {
            this.naviGroupDetailsExtend.Expanded = !this.naviGroupDetailsExtend.Expanded;
            this.toolStripButtonShowDetailsExtend.Image = this.naviGroupDetailsExtend.Expanded ? ResourceIcon.Chevron_Collapse.ToBitmap() : ResourceIcon.Chevron_Expand.ToBitmap();
        }


        private void InitializeDataGridBinding()
        {
            CommonMetaList commonMetaList = new CommonMetaList();
            DataGridViewComboBoxColumn comboBoxColumn;

            ListMaintenance.ListDataStatusDataTable listDataStatusDataTable = commonMetaList.GetListDataStatus(true);
            comboBoxColumn = (DataGridViewComboBoxColumn)this.dataGridViewDataMessageMaster.Columns[listDataStatusDataTable.DataStatusIDColumn.ColumnName];
            comboBoxColumn.DataSource = listDataStatusDataTable;
            comboBoxColumn.DisplayMember = listDataStatusDataTable.DescriptionColumn.ColumnName;
            comboBoxColumn.ValueMember = listDataStatusDataTable.DataStatusIDColumn.ColumnName;



            //this.dataGridViewDataMessageMaster.ColumnHeadersDefaultCellStyle.Font = new Font(this.dataGridViewDataMessageMaster.DefaultCellStyle.Font.Name, 9, FontStyle.Regular);
            //this.dataGridViewDataMessageMaster.ColumnHeadersDefaultCellStyle.Padding = new Padding(0, 3, 0, 3);
            this.dataGridViewDataMessageMaster.AutoGenerateColumns = false;
            dataMessageMasterListView = new BindingListView<DataMessageMaster>(this.dataMessageBLL.DataMessageMasterList);
            this.dataGridViewDataMessageMaster.DataSource = dataMessageMasterListView;
            this.dataGridViewDataMessageMaster.DataBindingComplete += new DataGridViewBindingCompleteEventHandler(dataGridViewDataMessageMaster_DataBindingComplete);



            #region <dataGridViewDetail>


            ////<dataGridViewMarketingIncentiveDetail>
            //ERmgrUIP.ListCustomerNameDataTable listCustomerNameDataTable = commonMetaList.GetCustomerName(true);
            //comboBoxColumn = (DataGridViewComboBoxColumn)this.dataGridViewMarketingIncentiveDetail.Columns[listCustomerNameDataTable.CustomerIDColumn.ColumnName];
            //comboBoxColumn.DataSource = listCustomerNameDataTable;
            //comboBoxColumn.DisplayMember = listCustomerNameDataTable.DescriptionColumn.ColumnName;
            //comboBoxColumn.ValueMember = listCustomerNameDataTable.CustomerIDColumn.ColumnName;

            ////--Display the second column for customer (Readonly): DescriptionOfficial -- Later: Try other way, instead of current DataGridViewComboBoxColumn.Datasource  
            //comboBoxColumn = (DataGridViewComboBoxColumn)this.dataGridViewMarketingIncentiveDetail.Columns[listCustomerNameDataTable.DescriptionOfficialColumn.ColumnName];
            //comboBoxColumn.DataSource = listCustomerNameDataTable;
            //comboBoxColumn.DisplayMember = listCustomerNameDataTable.DescriptionOfficialColumn.ColumnName;
            //comboBoxColumn.ValueMember = listCustomerNameDataTable.CustomerIDColumn.ColumnName;



            this.dataGridViewDataMessageDetail.AutoGenerateColumns = false;
            dataMessageDetailListView = new BindingListView<DataMessageDetail>(this.dataMessageBLL.DataMessageDetailList);
            this.dataGridViewDataMessageDetail.DataSource = dataMessageDetailListView;

            #endregion <dataGridViewDetail>
        }

        void dataGridViewDataMessageMaster_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            warningSetNextMessage = this.GetDataMessageMasterFromLoadingList(-1, GlobalEnum.DataStatusID.WaitForPrint) == null;
        }



        private void InitializeReadOnlyModeBinding()
        {
            List<Control> controlList = GlobalStaticFunction.GetAllControls(this);

            //foreach (Control control in controlList)
            //{
            //    ////////if (control is TextBox) control.DataBindings.Add("Readonly", this, "ReadonlyMode");
            //    //////if (control is TextBox) control.DataBindings.Add("Enabled", this, "EditableMode");
            //    //////else if (control is ComboBox || control is DateTimePicker) control.DataBindings.Add("Enabled", this, "EditableMode");
            //    //////else if (control is DataGridView)
            //    //////{
            //    //////    control.DataBindings.Add("Readonly", this, "ReadonlyMode");
            //    //////    control.DataBindings.Add("AllowUserToAddRows", this, "EditableMode");
            //    //////    control.DataBindings.Add("AllowUserToDeleteRows", this, "EditableMode");
            //    //////}
            //}

            this.dataGridViewDataMessageMaster.DataBindings.Add("Enabled", this, "ReadonlyMode");
        }

        BindingListView<DataMessageMaster> dataMessageMasterListView;
        BindingListView<DataMessageDetail> dataMessageDetailListView;
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


        private double UserInputCounterValue = -1;
        private DialogResult ShowDataMessage(DataMessageMaster dataMessageMaster, string warningMessage, bool isUserInputCounterValue)
        {
            PublicShowDataMessage publicShowDataMessage = new PublicShowDataMessage(dataMessageMaster, warningMessage, isUserInputCounterValue);
            DialogResult dialogResult = publicShowDataMessage.ShowDialog(this);

            this.UserInputCounterValue = publicShowDataMessage.UserInputCounterValue;

            publicShowDataMessage.Dispose();

            return dialogResult;
        }


        private DataMessageMaster SelectDataMessage(GlobalEnum.DataStatusID dataStatusID, string textTitle)
        {
            PublicSelectDataMessage publicShowDataMessage = new PublicSelectDataMessage(dataStatusID, textTitle);
            DialogResult dialogResult = publicShowDataMessage.ShowDialog(this);

            DataMessageMaster dataMessageMaster = publicShowDataMessage.DataMessageMaster;

            if (dataMessageMaster != null) dataMessageMaster = dataMessageMaster.ShallowClone();

            publicShowDataMessage.Dispose();

            return dataMessageMaster;
        }


        private DataMessageMaster GetDataMessageMasterFromLoadingList(int dataMessageID, GlobalEnum.DataStatusID dataStatusID)
        {
            List<DataMessageMaster> listOnPrintingDataMessageMaster = this.dataMessageBLL.DataMessageMasterList.Where(dataMessageDetail => dataMessageDetail.DataStatusID == (int)dataStatusID).ToList();
            if (listOnPrintingDataMessageMaster.Count != 0 && (dataMessageID == listOnPrintingDataMessageMaster[0].DataMessageID || dataMessageID <= 0))
                if (dataMessageID <= 0 && dataStatusID == GlobalEnum.DataStatusID.OnPrinting && (this.ShowDataMessage(listOnPrintingDataMessageMaster[0], "Vui lòng xác nhận: IN TIẾP TỤC CUỘN TÔN này." + "\n\r" + "Nhấn OK để tiếp tục in, hoặc nhấn Cancel để chọn lại sau.", false) == System.Windows.Forms.DialogResult.No))
                    return null;
                else
                    return listOnPrintingDataMessageMaster[0];
            else
                return null;
        }

        private bool UpdateDataStatus(int dataMessageID, GlobalEnum.DataStatusID dataStatusID)
        {
            if (dataMessageID > 0)
            {
                if (this.dataMessageBLL.DataMessageID != dataMessageID) this.dataMessageBLL.Fill(dataMessageID);

                return this.dataMessageBLL.UpdateDataStatus(dataStatusID);
            }
            else
                return false;
        }

        #endregion <InitializeBinding>





        #region Import Excel

        private void ImportExcel()
        {
            try
            {
                //OpenFileDialog openFileDialog = new OpenFileDialog();
                //openFileDialog.Filter = "Excel File (.xlsx)|*.xlsx";

                //if (openFileDialog.ShowDialog() == DialogResult.OK)
                //{
                //    DialogMapExcelColumn dialogMapExcelColumn = new DialogMapExcelColumn(OleDbDatabase.MappingTaskID.MarketingIncentive, openFileDialog.FileName);

                //    if (dialogMapExcelColumn.ShowDialog() == DialogResult.OK)
                //    {
                //        dialogMapExcelColumn.Dispose();
                //        if (this.marketingIncentiveBLL.ImportExcel(openFileDialog.FileName))
                //            MessageBox.Show(this, "Congratulation!" + "\r\n" + "\r\n" + "File: " + openFileDialog.FileName + " is imported successfull!" + "\r\n" + "\r\n" + "Please press OK to finish.", "Importing", MessageBoxButtons.OK, MessageBoxIcon.Information);
                //    }
                //}


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
                    int dataMessageID;

                    if (int.TryParse(row.Row["DataMessageID"].ToString(), out dataMessageID)) this.dataMessageBLL.Fill(dataMessageID);
                    else this.dataMessageBLL.Fill(0);
                }
                else this.dataMessageBLL.Fill(0);
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

                            //this.dataMessageBLL.DataMessageMaster = (DataMessageMaster)equinObjectView;

                            //this.toolStripTextBoxDisplay1.TextBox.DataBindings.Clear();
                            //this.toolStripTextBoxDisplay1.TextBox.DataBindings.Add("Text", this.dataMessageBLL.DataMessageMaster, "Display1", true);

                            //this.toolStripTextBoxDisplay2.TextBox.DataBindings.Clear();
                            //this.toolStripTextBoxDisplay2.TextBox.DataBindings.Add("Text", this.dataMessageBLL.DataMessageMaster, "Display2", true);

                            //this.toolStripTextBoxDisplay3.TextBox.DataBindings.Clear();
                            //this.toolStripTextBoxDisplay3.TextBox.DataBindings.Add("Text", this.dataMessageBLL.DataMessageMaster, "Display3", true);

                            //this.toolStripTextBoxDisplay4.TextBox.DataBindings.Clear();
                            //this.toolStripTextBoxDisplay4.TextBox.DataBindings.Add("Text", this.dataMessageBLL.DataMessageMaster, "Display4", true);




                            DataMessageMaster selectedDataMessageMaster = (DataMessageMaster)equinObjectView;
                            if (selectedDataMessageMaster != null)
                            {
                                this.toolStripMenuSetNotPrintedYet.Enabled = selectedDataMessageMaster.DataStatusID == (int)GlobalEnum.DataStatusID.OnPrinting | selectedDataMessageMaster.DataStatusID == (int)GlobalEnum.DataStatusID.WaitForPrint;
                                this.toolStripMenuSetNotPrintedYet.Text = selectedDataMessageMaster.DataStatusID == (int)GlobalEnum.DataStatusID.OnPrinting ? "Hủy lệnh in cuộn này" : "Hủy chuẩn bị in cuộn này";

                                this.toolStripMenuSwapCoil.Enabled = selectedDataMessageMaster.DataStatusID == (int)GlobalEnum.DataStatusID.PrintFinished;
                            }

                        }
                        else
                        {
                            this.dataMessageBLL.Fill(0);
                            this.toolStripMenuSetNotPrintedYet.Enabled = false;
                            this.toolStripMenuSwapCoil.Enabled = false;
                        }

                    }
                    else
                    {
                        this.dataMessageBLL.Fill(0);
                        //this.dataGridViewDataMessageMaster.EditMode = DataGridViewEditMode.EditProgrammatically;
                        this.toolStripMenuSetNotPrintedYet.Enabled = false;
                        this.toolStripMenuSwapCoil.Enabled = false;
                    }

                    this.toolStripMenuSetNotPrintedYet.Visible = this.toolStripMenuSetNotPrintedYet.Enabled;
                    this.toolStripMenuSwapCoil.Visible = this.toolStripMenuSwapCoil.Enabled;
                    this.toolStripSeparatorSetNotPrintedYet.Visible = this.toolStripMenuSetNotPrintedYet.Enabled | this.toolStripMenuSwapCoil.Enabled;
                }
                catch (Exception exception)
                {
                    GlobalExceptionHandler.ShowExceptionMessageBox(this, exception);
                }
            }

        }


        private void toolStripButtonSetting_Click(object sender, EventArgs e)
        {
            try
            {
                CommonMDI commonMDI = new CommonMDI(GlobalEnum.TaskID.DataMessage);
                commonMDI.ShowDialog();
                commonMDI.Dispose();

                this.Loading();
            }
            catch (Exception exception)
            {
                GlobalExceptionHandler.ShowExceptionMessageBox(this, exception);
            }

        }



        private void ControllingInterface_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                if (imageS8Thread != null && imageS8Thread.IsAlive)
                {
                    e.Cancel = true; return;
                }
                else
                    this.timerBlinkWarning.Enabled = false;
            }
            catch (Exception exception)
            {
                GlobalExceptionHandler.ShowExceptionMessageBox(this, exception);
            }
        }

        private void toolStripButtonLoad_Click(object sender, EventArgs e)
        {
            GlobalVariables.GlobalOptionSetting.LowerFillterDate = DateTime.Today.AddDays(-1); this.beginingDateBinding.ReadValue();
            GlobalVariables.GlobalOptionSetting.UpperFillterDate = DateTime.Today.AddDays(30); this.endingDateBinding.ReadValue();

            this.Loading();
        }

        private void toolStripButtonConnect_Click(object sender, EventArgs e)
        {
            try
            {
                if (!imageS8.LoopRoutine)
                {
                    if (imageS8Thread != null && imageS8Thread.IsAlive) imageS8Thread.Abort();
                    imageS8Thread = new Thread(new ThreadStart(imageS8.ThreadRoutine));
                    imageS8Thread.Start();


                    this.imageS8.StartPrint();
                    this.imageS8.StopPrint();

                }
                else
                {
                    imageS8.LoopRoutine = false;
                }

            }
            catch (Exception exception)
            {
                GlobalExceptionHandler.ShowExceptionMessageBox(this, exception);
            }

        }



        private void toolStripButtonSend_Click(object sender, EventArgs e)
        {
            try
            {

                this.imageS8.SetMessage();
            }
            catch (Exception exception)
            {
                GlobalExceptionHandler.ShowExceptionMessageBox(this, exception);
            }
        }

        private void toolStripButtonCounterValue_Click(object sender, EventArgs e)
        {
            try
            {
                if (this.imageS8.DataMessageMaster.DataMessageID > 0 && this.ShowDataMessage(this.imageS8.DataMessageMaster, "CÀI ĐẶT LẠI SỐ MÉT trên máy in theo số mét " + (sender.Equals(this.toolStripTextBoxImageS8Counter) ? "NHẬP TRỰC TIẾP" : "đồng hồ phụ.") + "\n\r" + "Nhấn OK để cài, hoặc nhấn Cancel để bỏ qua.", sender.Equals(this.toolStripTextBoxImageS8Counter)) == System.Windows.Forms.DialogResult.Yes)
                {
                    if (sender.Equals(this.toolStripTextBoxImageS8Counter) && this.UserInputCounterValue != -1)
                        this.imageS8.UserInputCounterValue = this.UserInputCounterValue;

                    this.imageS8.SetCounter();
                }
            }
            catch (Exception exception)
            {
                GlobalExceptionHandler.ShowExceptionMessageBox(this, exception);
            }
        }


        private void toolStripTextBoxImageS8Counter_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == System.Windows.Forms.MouseButtons.Right && MessageBox.Show(this, "Nhập trực tiếp số mét in tiếp?", "Cảnh báo", MessageBoxButtons.YesNo, MessageBoxIcon.Stop, MessageBoxDefaultButton.Button2) == System.Windows.Forms.DialogResult.Yes && MessageBox.Show(this, "Bạn biết lý do bạn muốn nhập trực tiếp số mét không?", "Cảnh báo", MessageBoxButtons.YesNo, MessageBoxIcon.Hand, MessageBoxDefaultButton.Button2) == System.Windows.Forms.DialogResult.Yes && MessageBox.Show(this, "Bạn thật sự muốn nhập số mét in tiếp?", "Cảnh báo", MessageBoxButtons.YesNo, MessageBoxIcon.Asterisk, MessageBoxDefaultButton.Button2) == System.Windows.Forms.DialogResult.Yes && MessageBox.Show(this, "Lưu ý: Bạn sẽ phải chịu trách nhiệm về số mét nhập vào?\r\n\r\nBạn có muốn tiếp tục không?", "Cảnh báo", MessageBoxButtons.YesNo, MessageBoxIcon.Asterisk, MessageBoxDefaultButton.Button2) == System.Windows.Forms.DialogResult.Yes && MessageBox.Show(this, "Vui lòng chọn Yes để tiếp tục. Chọn No để thoát.", "Cảnh báo", MessageBoxButtons.YesNo, MessageBoxIcon.Stop, MessageBoxDefaultButton.Button2) == System.Windows.Forms.DialogResult.Yes)
                toolStripButtonCounterValue_Click(sender, new EventArgs());

        }


        private void toolStripMenuContext_Click(object sender, EventArgs e)
        {
            try
            {
                if (this.dataGridViewDataMessageMaster.SelectedRows.Count > 0 && this.dataGridViewDataMessageMaster.CurrentRow != null)
                {
                    DataGridViewRow dataGridViewRow = this.dataGridViewDataMessageMaster.CurrentRow;
                    if (dataGridViewRow != null)
                    {
                        Equin.ApplicationFramework.ObjectView<DataMessageMaster> equinObjectView = dataGridViewRow.DataBoundItem as Equin.ApplicationFramework.ObjectView<DataMessageMaster>;
                        if (equinObjectView != null)
                        {
                            DataMessageMaster selectedDataMessageMaster = (DataMessageMaster)equinObjectView;

                            if (sender.Equals(this.toolStripMenuSplitCoil))
                            {
                                if (this.ShowDataMessage(selectedDataMessageMaster, "Xác nhận CHIA CUỘN NÀY." + "\n\r" + "Nhấn OK để xác nhận đồng ý, hoặc nhấn Cancel để chọn lại sau.", false) == System.Windows.Forms.DialogResult.Yes)
                                {
                                    string coilExtensionString = ""; int coilExtension = 0;
                                    if (CustomInputBox.Show("NMVN", "Vui lòng nhập số cuộn chia", ref coilExtensionString) == System.Windows.Forms.DialogResult.OK)
                                    {
                                        if (int.TryParse(coilExtensionString, out coilExtension))
                                        {
                                            DataMessageBLL selectedDataMessageBLL = new DataMessageBLL();
                                            selectedDataMessageBLL.Fill(selectedDataMessageMaster.DataMessageID);
                                            if (selectedDataMessageBLL.SplitCoil(coilExtension))
                                                this.Loading();
                                        }
                                        else
                                            throw new Exception("Vui lòng nhập số cuộn chia.");
                                    }
                                }
                            }
                            else if (sender.Equals(this.toolStripMenuSwapCoil))
                            {

                                if (MessageBox.Show(this, "Bạn muốn hoán đổi cuộn đã in rồi với một cuộn chưa in?", "Cảnh báo", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == System.Windows.Forms.DialogResult.No) return;

                                if (this.ShowDataMessage(selectedDataMessageMaster, "Xác nhận HOÁN ĐỔI CUỘN NÀY." + "\n\r" + "Lưu ý: Cuộn này sau khi hoán đổi sẽ chuyễn thành trạng thái chưa in." + "\n\r\n\r" + "Nhấn OK để xác nhận đồng ý, hoặc nhấn Cancel để chọn lại sau.", false) == System.Windows.Forms.DialogResult.Yes)
                                {
                                    DataMessageMaster notPrintedYetDataMessageMaster = this.SelectDataMessage(GlobalEnum.DataStatusID.NotPrintedYet, "Vui lòng chọn cuộn để hoán đổi");
                                    if (notPrintedYetDataMessageMaster != null)
                                    {

                                        using (TransactionScope transactionScope = new TransactionScope())
                                        {
                                            int affectedRows = 0;

                                            affectedRows = SQLDatabase.ExecuteNonQuery("DELETE FROM DataMessageDetail WHERE DataMessageID = " + notPrintedYetDataMessageMaster.DataMessageID);
                                            affectedRows = SQLDatabase.ExecuteNonQuery("DELETE FROM DataMessageCounterLog WHERE DataMessageID = " + notPrintedYetDataMessageMaster.DataMessageID);


                                            affectedRows = SQLDatabase.ExecuteNonQuery("UPDATE DataMessageMaster SET DataMessageMaster.ProductionDate = DataMessageMasterFinish.ProductionDate, DataMessageMaster.EntryDate = DataMessageMasterFinish.EntryDate, DataMessageMaster.CounterValue = DataMessageMasterFinish.CounterValue, DataMessageMaster.CounterAutonics = DataMessageMasterFinish.CounterAutonics, DataMessageMaster.DataStatusID = DataMessageMasterFinish.DataStatusID, DataMessageMaster.BeginingDate = DataMessageMasterFinish.BeginingDate, DataMessageMaster.EndingDate = DataMessageMasterFinish.EndingDate, DataMessageMaster.EntryStatusID = DataMessageMasterFinish.EntryStatusID FROM DataMessageMaster INNER JOIN (SELECT " + notPrintedYetDataMessageMaster.DataMessageID + " AS DataMessageID, ProductionDate, EntryDate, CounterValue, CounterAutonics, DataStatusID, BeginingDate, EndingDate, EntryStatusID FROM DataMessageMaster AS DataMessageMaster_1 WHERE DataMessageID = " + selectedDataMessageMaster.DataMessageID + " AND DataStatusID = " + (int)GlobalEnum.DataStatusID.PrintFinished + ") AS DataMessageMasterFinish ON DataMessageMaster.DataMessageID = DataMessageMasterFinish.DataMessageID AND DataMessageMaster.DataStatusID = " + (int)GlobalEnum.DataStatusID.NotPrintedYet);
                                            if (affectedRows != 1) throw new System.ArgumentException("Lỗi khi hoán đổi cuộn (1). Vui lòng kiểm tra trước khi thực hiện lại.");

                                            affectedRows = SQLDatabase.ExecuteNonQuery("UPDATE DataMessageDetail SET DataMessageID = " + notPrintedYetDataMessageMaster.DataMessageID + " WHERE DataMessageID = " + selectedDataMessageMaster.DataMessageID);
                                            affectedRows = SQLDatabase.ExecuteNonQuery("UPDATE DataMessageCounterLog SET DataMessageID = " + notPrintedYetDataMessageMaster.DataMessageID + " WHERE DataMessageID = " + selectedDataMessageMaster.DataMessageID);

                                            affectedRows = SQLDatabase.ExecuteNonQuery("EXECUTE DataMessageMasterUpdateDataStatus " + selectedDataMessageMaster.DataMessageID + ", " + GlobalVariables.GlobalUserInformation.UserID + ", " + (int)GlobalEnum.DataStatusID.NotPrintedYet);
                                            if (affectedRows != 1) throw new System.ArgumentException("Lỗi khi hoán đổi cuộn (2). Vui lòng kiểm tra trước khi thực hiện lại.");


                                            transactionScope.Complete();
                                        }

                                        this.Loading();
                                    }
                                }
                            }
                            else
                            {
                                if (sender.Equals(this.toolStripMenuSetNotPrintedYet))
                                {
                                    if (selectedDataMessageMaster.DataStatusID != (int)GlobalEnum.DataStatusID.OnPrinting && selectedDataMessageMaster.DataStatusID != (int)GlobalEnum.DataStatusID.WaitForPrint)
                                        return;
                                }
                                else
                                {
                                    int countOfDataMessageID = SQLDatabase.GetScalarValue("SELECT COUNT(DataMessageID) AS CountOfDataMessageID FROM DataMessageMaster WHERE CoilID = " + selectedDataMessageMaster.CoilID + " AND CoilExtension < " + selectedDataMessageMaster.CoilExtension + " AND DataStatusID = " + (int)GlobalEnum.DataStatusID.NotPrintedYet);
                                    if (countOfDataMessageID > 0 && MessageBox.Show(this, "Vui lòng xác nhận!\r\n\r\nCòn " + countOfDataMessageID.ToString() + " cuộn có mã số cuộn nhỏ hơn chưa in.\n\rBạn có muốn " + (sender.Equals(this.toolStripMenuSetCurrent) ? "in" : "chuẩn bị in") + " cuộn này hay không?", "Cảnh báo", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2) == System.Windows.Forms.DialogResult.No)
                                        return;
                                }



                                if (this.ShowDataMessage(selectedDataMessageMaster, (sender.Equals(this.toolStripMenuSetNotPrintedYet) ? (selectedDataMessageMaster.DataStatusID == (int)GlobalEnum.DataStatusID.OnPrinting ? "Hủy lệnh in cuộn này" : "Hủy chuẩn bị in cuộn này") : (sender.Equals(this.toolStripMenuSetCurrent) ? "Xác nhận IN CUỘN NÀY." : "Xác nhận CHUẨN BỊ IN CUỘN NÀY.")) + ((!sender.Equals(this.toolStripMenuSetNotPrintedYet) && (selectedDataMessageMaster.CounterValue > 0 || selectedDataMessageMaster.CounterAutonics > 0)) ? "\n\r\n\r" + "Lưu ý: Số mét sẽ bị xóa và IN BẮT ĐẦU BẰNG 0" : "") + "\n\r" + "Nhấn OK để xác nhận đồng ý, hoặc nhấn Cancel để chọn lại sau.", false) == System.Windows.Forms.DialogResult.Yes)
                                {
                                    if (this.UpdateDataStatus(selectedDataMessageMaster.DataMessageID, sender.Equals(this.toolStripMenuSetNotPrintedYet) ? GlobalEnum.DataStatusID.NotPrintedYet : (sender.Equals(this.toolStripMenuSetCurrent) ? GlobalEnum.DataStatusID.OnPrinting : GlobalEnum.DataStatusID.WaitForPrint)))
                                    {
                                        bool warningSetMessage = this.imageS8.WarningSetMessage;
                                        this.SetOnPrintingDataMessage(this.GetDataMessageMasterFromLoadingList(sender.Equals(this.toolStripMenuSetCurrent) ? selectedDataMessageMaster.DataMessageID : this.imageS8.DataMessageMaster.DataMessageID, GlobalEnum.DataStatusID.OnPrinting));
                                        if (!warningSetMessage && sender.Equals(this.toolStripMenuSetNext)) this.imageS8.WarningSetMessage = false; //CLEAR warning when [SET NEXT] if WarningSetMessage = false BEFORE SetOnPrintingDataMessage
                                    }
                                }


                            }
                        }
                    }
                }
            }
            catch (Exception exception)
            {
                GlobalExceptionHandler.ShowExceptionMessageBox(this, exception);
            }
        }


        private void SetOnPrintingDataMessage(DataMessageMaster onPrintingDataMessageMaster)
        {
            if (onPrintingDataMessageMaster == null)
            {
                this.imageS8.DataMessageMaster = new DataMessageMaster();
                GlobalDefaultValue.Apply(this.imageS8.DataMessageMaster);
            }
            else
                this.imageS8.DataMessageMaster = onPrintingDataMessageMaster;

            this.toolStripTextBoxDisplay1.TextBox.DataBindings.Clear();
            this.toolStripTextBoxDisplay1.TextBox.DataBindings.Add("Text", this.imageS8.DataMessageMaster, "Display1", true);

            this.toolStripTextBoxDisplay2.TextBox.DataBindings.Clear();
            this.toolStripTextBoxDisplay2.TextBox.DataBindings.Add("Text", this.imageS8.DataMessageMaster, "Display2", true);

            this.toolStripTextBoxDisplay3.TextBox.DataBindings.Clear();
            this.toolStripTextBoxDisplay3.TextBox.DataBindings.Add("Text", this.imageS8.DataMessageMaster, "Display3", true);

            this.toolStripTextBoxDisplay4.TextBox.DataBindings.Clear();
            this.toolStripTextBoxDisplay4.TextBox.DataBindings.Add("Text", this.imageS8.DataMessageMaster, "Display4", true);
        }

        private void toolStripMenuItemFind_Click(object sender, EventArgs e)
        {
            try
            {
                PublicFind publicFind = new PublicFind(this.dataMessageMasterListView);
                publicFind.Show(this);
            }
            catch (Exception exception)
            {
                GlobalExceptionHandler.ShowExceptionMessageBox(this, exception);
            }
        }

        private void buttonSetCDChanged_Click(object sender, EventArgs e)
        {
            try
            {
                if (MessageBox.Show(this, "Cắt cuộn đang in ngay bây giờ?", "Cảnh báo", MessageBoxButtons.YesNo, MessageBoxIcon.Hand, MessageBoxDefaultButton.Button2) == System.Windows.Forms.DialogResult.Yes)
                    this.imageS8.SetCDChanged(); return; //TEST ONLY
            }
            catch (Exception exception)
            {
                GlobalExceptionHandler.ShowExceptionMessageBox(this, exception);
            }
        }














    }




}
