using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Net;
using System.Net.Sockets;
using System.IO;
using System.Threading;

using Global.Class.Library;
using BusinessLogicLayer;
using BusinessLogicLayer.InkjetDominoPrinter;
using BusinessLogicLayer.BarcodeScanner;
using DataTransferObject;

using DataAccessLayer;

namespace PresentationLayer
{
    /// <summary>
    /// "C:\Program Files\Microsoft SQL Server\100\Tools\Binn\VSShell\Common7\IDE\Ssms.exe"
    /// </summary>
    public partial class MainControllingInterface : Form, IMergeToolStrip
    {

        #region Declaration

        delegate void SetTextCallback(string text);

        delegate void ThreadShowStatus(object sender, PropertyChangedEventArgs e);

        private InkjetDominoPrinterBLL digitInkjetDominoPrinter;
        private InkjetDominoPrinterBLL barcodeInkjetDominoPrinter;
        private InkjetDominoPrinterBLL cartonInkjetDominoPrinter;


        private Thread digitInkJetDominoPrinterThread;
        private Thread barcodeInkJetDominoPrinterThread;
        private Thread cartonInkJetDominoPrinterThread;



        private BarcodeScannerBLL barcodeScannerMCU;
        private Thread barcodeScannerMCUThread;





        private FillingLineData fillingLineData;


        #endregion Declaration



        #region Contructor & Implement Interface

        public MainControllingInterface()
        {
            InitializeComponent();

            try
            {
                //Test Only
                this.toolStripButton1.Visible = GlobalVariables.MyTest;
                this.toolStripButton2.Visible = GlobalVariables.MyTest;
                this.toolStripButton3.Visible = GlobalVariables.MyTest;
                this.toolStripButton4.Visible = GlobalVariables.MyTest;
                //Test Only


                //DataTable aa = new DataTable();
                //aa.Columns.Add("1");
                //aa.Columns.Add("2");
                //aa.Columns.Add("3");
                //aa.Columns.Add("4");
                //aa.Columns.Add("5");
                //aa.Columns.Add("6");

                //aa.Rows.Add("123456", "123456", "123456", "123456", "123456", "123456");

                //this.dataGridViewPackInOneCarton.DataSource = aa;


                //DataTable a = new DataTable();
                //a.Columns.Add("1", typeof(string));
                //a.Columns.Add("2", typeof(string));
                //a.Columns.Add("3", typeof(string));
                //a.Columns.Add("4", typeof(string));
                ////a.Columns.Add("E", typeof(string));
                ////a.Columns.Add("F", typeof(string));

                //a.Rows.Add("111111111111111111111111111111", "111111111111111111111111111111", "111111111111111111111111111111", "111111111111111111111111111111");
                //a.Rows.Add("111111111111111111111111111111", "111111111111111111111111111111", "111111111111111111111111111111", "111111111111111111111111111111");
                //a.Rows.Add("111111111111111111111111111111", "111111111111111111111111111111", "111111111111111111111111111111", "111111111111111111111111111111");
                //a.Rows.Add("111111111111111111111111111111", "111111111111111111111111111111", "111111111111111111111111111111", "111111111111111111111111111111");
                //a.Rows.Add("111111111111111111111111111111", "111111111111111111111111111111", "111111111111111111111111111111", "111111111111111111111111111111");
                //a.Rows.Add("111111111111111111111111111111", "111111111111111111111111111111", "111111111111111111111111111111", "111111111111111111111111111111");

                //this.dataGridViewPackInOneCarton.DataSource = a;












                this.dataGridViewCartonList.AutoGenerateColumns = false;


                //FillingLineData
                this.fillingLineData = new FillingLineData();

                this.toolStripTextBoxFillingLineID.TextBox.DataBindings.Add("Text", this.fillingLineData, "FillingLineName");
                this.toolStripTextBoxProductID.TextBox.DataBindings.Add("Text", this.fillingLineData, "ProductCode");
                this.toolStripTextBoxProductCodeOriginal.TextBox.DataBindings.Add("Text", this.fillingLineData, "ProductCodeOriginal");
                this.toolStripTextBoxBatchNo.TextBox.DataBindings.Add("Text", this.fillingLineData, "BatchNo");
                this.toolStripTextBoxBatchSerialNo.TextBox.DataBindings.Add("Text", this.fillingLineData, "BatchSerialNumber");
                this.toolStripTextBoxMonthSerialNo.TextBox.DataBindings.Add("Text", this.fillingLineData, "MonthSerialNumber");
                this.toolStripTextBoxBatchCartonNo.TextBox.DataBindings.Add("Text", this.fillingLineData, "BatchCartonNumber");
                this.toolStripTextBoxMonthCartonNo.TextBox.DataBindings.Add("Text", this.fillingLineData, "MonthCartonNumber");



                this.toolStripTextBoxNoItemPerCartonSetByProductID.TextBox.DataBindings.Add("Text", this, "NoItemPerCartonSetByProductID");


                //Inkjet Printer
                digitInkjetDominoPrinter = new InkjetDominoPrinterBLL(GlobalVariables.DominoPrinterName.DegitInkJet, this.fillingLineData);
                barcodeInkjetDominoPrinter = new InkjetDominoPrinterBLL(GlobalVariables.DominoPrinterName.BarCodeInkJet, this.fillingLineData);
                cartonInkjetDominoPrinter = new InkjetDominoPrinterBLL(GlobalVariables.DominoPrinterName.CartonInkJet, this.fillingLineData);



                digitInkjetDominoPrinter.PropertyChanged += new PropertyChangedEventHandler(InkjetDominoPrinter_PropertyChanged);
                barcodeInkjetDominoPrinter.PropertyChanged += new PropertyChangedEventHandler(InkjetDominoPrinter_PropertyChanged);
                cartonInkjetDominoPrinter.PropertyChanged += new PropertyChangedEventHandler(InkjetDominoPrinter_PropertyChanged);


                //Barcode Scanner
                barcodeScannerMCU = new BarcodeScannerBLL(this.fillingLineData);

                barcodeScannerMCU.PropertyChanged += new PropertyChangedEventHandler(InkjetDominoPrinter_PropertyChanged);


                this.splitContainerQuality.SplitterDistance = this.SplitterDistanceQuality();
                this.splitContainerMatching.SplitterDistance = this.SplitterDistanceMatching();
                this.splitContainerCarton.SplitterDistance = this.SplitterDistanceCarton();



                this.toolStripTextBoxCartonDateFrom.TextBox.DataBindings.Add("Text", this.barcodeScannerMCU, "CartonDateFrom");
                this.toolStripTextBoxCartonDateTo.TextBox.DataBindings.Add("Text", this.barcodeScannerMCU, "CartonDateTo");
            }
            catch (Exception exception)
            {
                GlobalExceptionHandler.ShowExceptionMessageBox(this, exception);
            }
        }

        private void MainControllingInterface_Load(object sender, EventArgs e)
        {
            try
            {
                barcodeScannerMCU.Initialize();

                this.comboBoxViewOption.ComboBox.Items.AddRange(new string[] { "Compact View", "Normal View" });
                this.comboBoxViewOption.ComboBox.SelectedIndex = 1;// this.fillingLineData.FillingLineID == GlobalVariables.FillingLine.Pail ? 1 : 0;
                this.comboBoxViewOption.Enabled = this.fillingLineData.FillingLineID != GlobalVariables.FillingLine.Pail;

                this.comboBoxEmptyCarton.ComboBox.Items.AddRange(new string[] { "Ignore Empty Carton", "Keep Empty Carton" });
                this.comboBoxEmptyCarton.ComboBox.SelectedIndex = 0;// (this.fillingLineData.FillingLineID == GlobalVariables.FillingLine.Pail || !GlobalVariables.IgnoreEmptyCarton) ? 1 : 0;
                this.comboBoxEmptyCarton.Enabled = this.fillingLineData.FillingLineID != GlobalVariables.FillingLine.Pail;

            }
            catch (Exception exception)
            {
                GlobalExceptionHandler.ShowExceptionMessageBox(this, exception);
            }
        }

        private void MainControllingInterface_Activated(object sender, EventArgs e)
        {
            if (this.dataGridViewCartonList.CanSelect) this.dataGridViewCartonList.Select();
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

        public int NoItemPerCartonSetByProductID
        {
            get { return GlobalVariables.noItemPerCartonSetByProductID; }
            set
            {
                if (value >= 1 && value <= 24 && GlobalVariables.noItemPerCartonSetByProductID != value)
                {
                    GlobalVariables.noItemPerCartonSetByProductID = value;
                    GlobalRegistry.Write("NoItemPerCartonSetByProductID", value.ToString());
                    for (int i = 1; i <= 24; i++)
                        this.dataGridViewCartonList.Columns[i].Visible = (i > value ? false : true);

                }
            }
        }

        private int SplitterDistanceQuality()
        {
            switch (GlobalVariables.FillingLineID)
            {
                case GlobalVariables.FillingLine.Ocme:
                    return 133; //142
                case GlobalVariables.FillingLine.CO:
                    return 343; //364 
                case GlobalVariables.FillingLine.WH:
                    return 86; 
                case GlobalVariables.FillingLine.CM:
                    return 86;
                case GlobalVariables.FillingLine.Pail:
                    return 0;
                default:
                    return 1;
            }
        }

        private int SplitterDistanceMatching()
        {
            if (this.fillingLineData.FillingLineID == GlobalVariables.FillingLine.CM || this.fillingLineData.FillingLineID == GlobalVariables.FillingLine.WH || this.fillingLineData.FillingLineID == GlobalVariables.FillingLine.Pail)
            {
                for (int i = 1; i <= 24; i++)
                    this.dataGridViewCartonList.Columns[i].Visible = (i > (this.fillingLineData.FillingLineID == GlobalVariables.FillingLine.CM || this.fillingLineData.FillingLineID == GlobalVariables.FillingLine.WH ? GlobalVariables.NoItemPerCarton() : 0)) ? false : true;
            }

            switch (GlobalVariables.FillingLineID)
            {
                case GlobalVariables.FillingLine.Ocme:
                    return 1032;
                case GlobalVariables.FillingLine.CO:
                    return 1199;
                case GlobalVariables.FillingLine.WH:
                    return 900;// GlobalVariables.noItemPerCartonSetByProductID == 6 ? 1160 : 1180;
                case GlobalVariables.FillingLine.CM:
                    return GlobalVariables.noItemPerCartonSetByProductID == 6 ? 1160 : 1180;
                case GlobalVariables.FillingLine.Pail:
                    return 760;//760---24
                default:
                    return 1;
            }
        }

        private int SplitterDistanceCarton()
        {
            switch (GlobalVariables.FillingLineID)
            {
                case GlobalVariables.FillingLine.Ocme:
                    return 430; //480
                case GlobalVariables.FillingLine.CO:
                    return 225; //213
                case GlobalVariables.FillingLine.WH:
                    return 481; //485
                case GlobalVariables.FillingLine.CM:
                    return 481;
                case GlobalVariables.FillingLine.Pail:
                    return 559;
                default:
                    return 1;
            }
        }

        #endregion Contructor & Implement Interface

        #region Toolstrip bar

        private void toolStripButtonConnect_Click(object sender, EventArgs e)
        {
            try
            {
                this.CutTextBox(true);

                //////if (digitInkJetDominoPrinterThread != null && digitInkJetDominoPrinterThread.IsAlive) digitInkJetDominoPrinterThread.Abort();
                //////digitInkJetDominoPrinterThread = new Thread(new ThreadStart(digitInkjetDominoPrinter.ThreadRoutine));
                //////digitInkJetDominoPrinterThread.Start();


                //////if (barcodeInkJetDominoPrinterThread != null && barcodeInkJetDominoPrinterThread.IsAlive) barcodeInkJetDominoPrinterThread.Abort();
                //////barcodeInkJetDominoPrinterThread = new Thread(new ThreadStart(barcodeInkjetDominoPrinter.ThreadRoutine));
                //////barcodeInkJetDominoPrinterThread.Start();

                //////if (cartonInkJetDominoPrinterThread != null && cartonInkJetDominoPrinterThread.IsAlive) cartonInkJetDominoPrinterThread.Abort();
                //////cartonInkJetDominoPrinterThread = new Thread(new ThreadStart(cartonInkjetDominoPrinter.ThreadRoutine));
                //////cartonInkJetDominoPrinterThread.Start();




                if (barcodeScannerMCUThread != null && barcodeScannerMCUThread.IsAlive) barcodeScannerMCUThread.Abort();
                barcodeScannerMCUThread = new Thread(new ThreadStart(barcodeScannerMCU.ThreadRoutine));
                barcodeScannerMCUThread.Start();

            }
            catch (Exception exception)
            {
                GlobalExceptionHandler.ShowExceptionMessageBox(this, exception);
            }
        }


        private void toolStripButtonDisconnect_Click(object sender, EventArgs e)
        {
            try
            {
                ////////digitInkjetDominoPrinter.LoopRoutine = false;
                ////////barcodeInkjetDominoPrinter.LoopRoutine = false;
                ////////cartonInkjetDominoPrinter.LoopRoutine = false;

                barcodeScannerMCU.LoopRoutine = false;

                this.SetToolStripActive();
            }
            catch (Exception exception)
            {
                GlobalExceptionHandler.ShowExceptionMessageBox(this, exception);
            }
        }


        private void toolStripButtonStart_Click(object sender, EventArgs e)
        {
            try
            {
                this.CutTextBox(true);

                this.barcodeScannerMCU.StartPrint();

                this.digitInkjetDominoPrinter.StartPrint();
                this.barcodeInkjetDominoPrinter.StartPrint();
                this.cartonInkjetDominoPrinter.StartPrint();

            }
            catch (Exception exception)
            {
                GlobalExceptionHandler.ShowExceptionMessageBox(this, exception);
            }

        }

        private void toolStripButtonStop_Click(object sender, EventArgs e)
        {
            try
            {
                if (MessageBox.Show("The software will not monitor the scanners affter stopped." + (char)13 + (char)13 + "Are you sure you want to stop?", "Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button2) == System.Windows.Forms.DialogResult.Yes)
                    if (MessageBox.Show("Do you really want to stop?", "Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Stop, MessageBoxDefaultButton.Button2) == System.Windows.Forms.DialogResult.Yes)
                    {
                        this.StopPrint();
                        this.barcodeScannerMCU.StopPrint();
                    }
            }
            catch (Exception exception)
            {
                GlobalExceptionHandler.ShowExceptionMessageBox(this, exception);
            }
        }

        private void StopPrint()
        {
            this.StopPrint(true, true, true);
        }

        private void StopPrint(bool stopDigit, bool stopBarcode, bool stopCarton)
        {
            if (stopDigit) this.digitInkjetDominoPrinter.StopPrint();
            if (stopBarcode) this.barcodeInkjetDominoPrinter.StopPrint();
            if (stopCarton) this.cartonInkjetDominoPrinter.StopPrint();
        }

        #endregion Toolstrip bar


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
                this.SetToolStripActive();

                if (sender.Equals(this.digitInkjetDominoPrinter))
                {
                    if (e.PropertyName == "MainStatus") { this.textBoxDigitStatus.Text = "[" + DateTime.Now.ToString("hh:mm:ss") + "] " + this.digitInkjetDominoPrinter.MainStatus + "\r\n" + this.textBoxDigitStatus.Text; this.CutTextBox(false); return; }
                    if (e.PropertyName == "LedStatus") { this.toolStripDigitLEDGreen.Enabled = this.digitInkjetDominoPrinter.LedGreenOn; this.toolStripDigitLEDAmber.Enabled = this.digitInkjetDominoPrinter.LedAmberOn; this.toolStripDigitLEDRed.Enabled = this.digitInkjetDominoPrinter.LedRedOn; if (this.digitInkjetDominoPrinter.LedRedOn) this.StopPrint(true, true, false); return; }
                }
                else if (sender.Equals(this.barcodeInkjetDominoPrinter))
                {
                    if (e.PropertyName == "MainStatus") { this.textBoxBarcodeStatus.Text = "[" + DateTime.Now.ToString("hh:mm:ss") + "] " + this.barcodeInkjetDominoPrinter.MainStatus + "\r\n" + this.textBoxBarcodeStatus.Text; this.CutTextBox(false); return; }
                    if (e.PropertyName == "LedStatus") { this.toolStripBarcodeLEDGreen.Enabled = this.barcodeInkjetDominoPrinter.LedGreenOn; this.toolStripBarcodeLEDAmber.Enabled = this.barcodeInkjetDominoPrinter.LedAmberOn; this.toolStripBarcodeLEDRed.Enabled = this.barcodeInkjetDominoPrinter.LedRedOn; if (this.barcodeInkjetDominoPrinter.LedRedOn) this.StopPrint(true, true, false); return; }

                    if (e.PropertyName == "MonthSerialNumber") { this.fillingLineData.MonthSerialNumber = this.barcodeInkjetDominoPrinter.MonthSerialNumber; this.fillingLineData.Update(); return; }
                }
                else if (sender.Equals(this.cartonInkjetDominoPrinter))
                {
                    if (e.PropertyName == "MainStatus") { this.textBoxCartonStatus.Text = "[" + DateTime.Now.ToString("hh:mm:ss") + "] " + this.cartonInkjetDominoPrinter.MainStatus + "\r\n" + this.textBoxCartonStatus.Text; this.CutTextBox(false); return; }
                    if (e.PropertyName == "LedStatus") { this.toolStripCartonLEDGreen.Enabled = this.cartonInkjetDominoPrinter.LedGreenOn; this.toolStripCartonLEDAmber.Enabled = this.cartonInkjetDominoPrinter.LedAmberOn; this.toolStripCartonLEDRed.Enabled = this.cartonInkjetDominoPrinter.LedRedOn; return; }//if (this.cartonInkjetDominoPrinter.LedRedOn) this.StopPrint(); 

                    if (e.PropertyName == "BatchCartonNumber") { this.fillingLineData.BatchCartonNumber = this.cartonInkjetDominoPrinter.BatchCartonNumber; this.fillingLineData.MonthCartonNumber = this.cartonInkjetDominoPrinter.MonthCartonNumber; this.fillingLineData.Update(); return; }
                    if (e.PropertyName == "MonthCartonNumber") { this.fillingLineData.BatchCartonNumber = this.cartonInkjetDominoPrinter.BatchCartonNumber; this.fillingLineData.MonthCartonNumber = this.cartonInkjetDominoPrinter.MonthCartonNumber; this.fillingLineData.Update(); return; }
                }
                else if (sender.Equals(this.barcodeScannerMCU))
                {
                    if (e.PropertyName == "MainStatus") { this.textBoxScannerStatus.Text = "[" + DateTime.Now.ToString("hh:mm:ss") + "] " + this.barcodeScannerMCU.MainStatus + "\r\n" + this.textBoxScannerStatus.Text; this.CutTextBox(false); return; }
                    if (e.PropertyName == "LedStatus") { this.toolStripScannerLEDGreen.Enabled = this.barcodeScannerMCU.LedGreenOn; this.toolStripScannerLEDAmber.Enabled = this.barcodeScannerMCU.LedAmberOn; this.toolStripScannerLEDRed.Enabled = this.barcodeScannerMCU.LedRedOn; if (this.barcodeScannerMCU.LedRedOn) this.StopPrint(); return; }

                    if (e.PropertyName == "LedMCU") { this.toolStripMCUQuanlity.Enabled = this.barcodeScannerMCU.LedMCUQualityOn; this.toolStripMCUMatching.Enabled = this.barcodeScannerMCU.LedMCUMatchingOn; this.toolStripMCUCarton.Enabled = this.barcodeScannerMCU.LedMCUCartonOn; return; }


                    if (e.PropertyName == "BatchSerialNumber") { this.fillingLineData.BatchSerialNumber = this.barcodeScannerMCU.BatchSerialNumber; this.fillingLineData.Update(); return; }

                    if (e.PropertyName == "MatchingPackList")
                    {
                        int currentRowIndex = -1; int currentColumnIndex = -1;
                        if (this.dataGridViewMatchingPackList.CurrentCell != null) { currentRowIndex = this.dataGridViewMatchingPackList.CurrentCell.RowIndex; currentColumnIndex = this.dataGridViewMatchingPackList.CurrentCell.ColumnIndex; }

                        this.dataGridViewMatchingPackList.DataSource = this.barcodeScannerMCU.GetMatchingPackList();

                        if (currentRowIndex >= 0 && currentRowIndex < this.dataGridViewMatchingPackList.Rows.Count && currentColumnIndex >= 0 && currentColumnIndex < this.dataGridViewMatchingPackList.ColumnCount) this.dataGridViewMatchingPackList.CurrentCell = this.dataGridViewMatchingPackList[currentColumnIndex, currentRowIndex]; //Keep current cell

                        this.toolStripButtonMessageCount.Text = "[" + this.barcodeScannerMCU.MatchingPackCount.ToString("N0") + "]";
                    }

                    if (e.PropertyName == "PackInOneCarton") { this.dataGridViewPackInOneCarton.DataSource = this.barcodeScannerMCU.GetPackInOneCarton(); }
                    if (e.PropertyName == "CartonList") { this.dataGridViewCartonList.DataSource = null; this.dataGridViewCartonList.DataSource = this.barcodeScannerMCU.GetCartonList(); if (this.dataGridViewCartonList.Rows.Count > 1) this.dataGridViewCartonList.CurrentCell = this.dataGridViewCartonList.Rows[0].Cells[1]; }

                }

            }
            catch (Exception exception)
            {
                GlobalExceptionHandler.ShowExceptionMessageBox(this, exception);
            }
        }

        private void comboBoxEmptyCarton_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                GlobalVariables.IgnoreEmptyCarton = this.comboBoxEmptyCarton.ComboBox.SelectedIndex == 0;
                GlobalRegistry.Write("IgnoreEmptyCarton", GlobalVariables.IgnoreEmptyCarton ? "1" : "0");
            }
            catch
            { }
        }


        private bool displayCommpactView;

        private void comboBoxViewOption_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                displayCommpactView = this.comboBoxViewOption.SelectedIndex == 0;
            }
            catch
            { }
        }

        private void dataGridViewBarcodeList_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            try
            {
                if (displayCommpactView || !sender.Equals(this.dataGridViewCartonList))
                    e.Value = this.GetSerialNumber(e.Value.ToString());
            }
            catch
            { }
        }

        private void dataGridViewBarcodeList_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            try
            {
                if (e.RowIndex >= 0)//e.RowIndex == -1 &&
                {
                    e.PaintBackground(e.CellBounds, true);
                    e.Graphics.TranslateTransform(e.CellBounds.Left, e.CellBounds.Bottom);
                    e.Graphics.RotateTransform(270);
                    e.Graphics.DrawString(e.FormattedValue.ToString(), e.CellStyle.Font, Brushes.Black, 5, 5);
                    e.Graphics.ResetTransform();
                    e.Handled = true;
                }
            }
            catch
            { }
        }

        private void dataGridViewBarcodeList_ColumnAdded(object sender, DataGridViewColumnEventArgs e)
        {
            e.Column.SortMode = DataGridViewColumnSortMode.NotSortable;
        }

        private void dataGridViewMatchingPackList_Enter(object sender, EventArgs e)
        {
            this.dataGridViewMatchingPackList.ScrollBars = ScrollBars.Horizontal;
        }


        private void dataGridViewMatchingPackList_Leave(object sender, EventArgs e)
        {
            this.dataGridViewMatchingPackList.ScrollBars = ScrollBars.None;
        }


        /// <summary>
        /// Find a specific pack number in matching queue
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dataGridViewMatchingPackList_DoubleClick(object sender, EventArgs e)
        {
            try
            {
                string cellValue = "";
                if (CustomInputBox.Show("BP Filling System", "Please input pack number", ref cellValue) == System.Windows.Forms.DialogResult.OK)
                {
                    for (int rowIndex = 0; rowIndex < this.dataGridViewMatchingPackList.Rows.Count; rowIndex++)
                    {
                        for (int columnIndex = 0; columnIndex < this.dataGridViewMatchingPackList.Rows[rowIndex].Cells.Count; columnIndex++)
                        {
                            if (this.GetSerialNumber(this.dataGridViewMatchingPackList[columnIndex, rowIndex].Value.ToString()).IndexOf(cellValue) != -1)
                            {
                                if (rowIndex >= 0 && rowIndex < this.dataGridViewMatchingPackList.Rows.Count && columnIndex >= 0 && columnIndex < this.dataGridViewMatchingPackList.ColumnCount)
                                    this.dataGridViewMatchingPackList.CurrentCell = this.dataGridViewMatchingPackList[columnIndex, rowIndex];
                                else
                                    this.dataGridViewMatchingPackList.CurrentCell = null;
                                break;
                            }
                        }
                    }
                }
            }
            catch
            {
                this.dataGridViewMatchingPackList.CurrentCell = null;
            }
        }

        /// <summary>
        /// Remove a specific pack in matching queue
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dataGridViewMatchingPackList_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete && this.dataGridViewMatchingPackList.CurrentCell != null)
            {
                try
                {                //Handle exception for PackInOneCarton
                    string selectedBarcode = "";
                    int packID = this.GetPackID(this.dataGridViewMatchingPackList.CurrentCell, out selectedBarcode);
                    if (packID > 0 && MessageBox.Show("Are you sure you want to remove this pack:" + (char)13 + (char)13 + selectedBarcode, "Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button2) == System.Windows.Forms.DialogResult.Yes)
                        if (this.barcodeScannerMCU.RemoveItemInMatchingPackList(packID)) MessageBox.Show("Pack: " + selectedBarcode + "\r\nHas been removed successfully.", "Handle exception", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception exception)
                {
                    GlobalExceptionHandler.ShowExceptionMessageBox(this, exception);
                }
            }
        }

        /// <summary>
        /// Remove a specific pack in PackInOneCarton
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dataGridViewPackInOneCarton_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete && this.dataGridViewPackInOneCarton.CurrentCell != null)
            {
                try
                {                //Handle exception for PackInOneCarton
                    string selectedBarcode = "";
                    int packID = this.GetPackID(this.dataGridViewPackInOneCarton.CurrentCell, out selectedBarcode);
                    if (packID > 0 && MessageBox.Show("Are you sure you want to remove this pack:" + (char)13 + (char)13 + selectedBarcode, "Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button2) == System.Windows.Forms.DialogResult.Yes)
                        if (this.barcodeScannerMCU.RemoveItemInPackInOneCarton(packID)) MessageBox.Show("Pack: " + selectedBarcode + "\r\nHas been removed successfully.", "Handle exception", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception exception)
                {
                    GlobalExceptionHandler.ShowExceptionMessageBox(this, exception);
                }
            }
        }

        /// <summary>
        /// Unpacking a specific carton
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dataGridViewCartonList_KeyDown(object sender, KeyEventArgs e)
        {
            if ((e.KeyCode == Keys.Space || e.KeyCode == Keys.Delete) && this.dataGridViewCartonList.CurrentRow != null)
            {
                try
                {                //Handle exception for carton
                    DataGridViewRow dataGridViewRow = this.dataGridViewCartonList.CurrentRow;
                    if (dataGridViewRow != null)
                    {
                        DataRowView dataRowView = dataGridViewRow.DataBoundItem as DataRowView;
                        DataDetail.DataDetailCartonRow selectedCarton = dataRowView.Row as DataDetail.DataDetailCartonRow;

                        if (selectedCarton != null && selectedCarton.CartonStatus == (byte)GlobalVariables.BarcodeStatus.BlankBarcode)
                        {
                            string selectedCartonDescription = this.GetSerialNumber(selectedCarton.Pack00Barcode) + ": " + selectedCarton.Pack00Barcode + (char)13 + "   " + this.GetSerialNumber(selectedCarton.Pack01Barcode) + ": " + selectedCarton.Pack01Barcode + (char)13 + "   " + this.GetSerialNumber(selectedCarton.Pack02Barcode) + ": " + selectedCarton.Pack02Barcode + (char)13 + "   " + this.GetSerialNumber(selectedCarton.Pack03Barcode) + ": " + selectedCarton.Pack03Barcode + (char)13 + "   " + "[...]";

                            if (e.KeyCode == Keys.Space) //Update barcode
                            {
                                string cartonBarcode = "";
                                if (CustomInputBox.Show("BP Filling System", "Please input barcode for this carton:" + (char)13 + (char)13 + selectedCarton.CartonBarcode + (char)13 + "   " + selectedCartonDescription, ref cartonBarcode) == System.Windows.Forms.DialogResult.OK)
                                    if (this.barcodeScannerMCU.UpdateCartonBarcode(selectedCarton.CartonID, cartonBarcode)) MessageBox.Show("Carton: " + (char)13 + cartonBarcode + (char)13 + "   " + selectedCartonDescription + "\r\nHas been updated successfully.", "Handle exception", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            }

                            if (e.KeyCode == Keys.Delete)
                            {
                                if (MessageBox.Show("Are you sure you want to remove this carton:" + (char)13 + (char)13 + selectedCarton.CartonBarcode + (char)13 + "   " + selectedCartonDescription, "Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button2) == System.Windows.Forms.DialogResult.Yes)
                                    if (this.barcodeScannerMCU.UndoCartonToPack(selectedCarton.CartonID)) MessageBox.Show("Carton: " + (char)13 + selectedCarton.CartonBarcode + (char)13 + "   " + selectedCartonDescription + "\r\nHas been removed successfully.", "Handle exception", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            }
                        }
                    }
                }
                catch (Exception exception)
                {
                    GlobalExceptionHandler.ShowExceptionMessageBox(this, exception);
                }
            }
        }




        private int GetPackID(DataGridViewCell dataGridViewCell, out string selectedBarcode)
        {
            int packID;
            if (dataGridViewCell != null)
            {
                selectedBarcode = dataGridViewCell.Value as string;
                if (selectedBarcode != null)
                {
                    int startIndexOfPackID = selectedBarcode.IndexOf(GlobalVariables.doubleTabChar.ToString() + GlobalVariables.doubleTabChar.ToString());
                    if (startIndexOfPackID >= 0 && int.TryParse(selectedBarcode.Substring(startIndexOfPackID + 2), out packID))
                    {
                        selectedBarcode = this.GetSerialNumber(selectedBarcode) + ": " + selectedBarcode.Substring(0, startIndexOfPackID);
                        return packID;
                    }
                }
            }
            selectedBarcode = null;
            return -1;
        }

        private string GetSerialNumber(string printedBarcode)
        {
            if (false)
            {
                if (printedBarcode.IndexOf(GlobalVariables.doubleTabChar.ToString()) == 0) printedBarcode = "";
                //else if (printedBarcode.Length > 6) printedBarcode = printedBarcode.Substring(printedBarcode.Length - 7, 6); //Char[3][4][5]...[9]: Serial Number
                else
                    if (printedBarcode.Length >= 29) printedBarcode = printedBarcode.Substring(23, 6); //Char[3][4][5]...[9]: Serial Number
                    else if (printedBarcode.Length >= 12) printedBarcode = printedBarcode.Substring(6, 5);
            }
            else
                printedBarcode = printedBarcode.Substring(0, 6);

            return printedBarcode;
        }


        private void timerEverySecond_Tick(object sender, EventArgs e)
        {
            try
            {
                this.toolStripTextBoxCurrentDate.TextBox.Text = DateTime.Now.ToString("dd/MM/yy");
                if (this.fillingLineData != null)
                {
                    if (this.fillingLineData.SettingMonthID != GlobalStaticFunction.DateToContinuosMonth())
                    {
                        this.toolStripButtonWarningNewMonth.Visible = !this.toolStripButtonWarningNewMonth.Visible; this.toolStripLabelWarningNewMonth.Visible = !this.toolStripLabelWarningNewMonth.Visible;
                    }
                    else
                    {
                        this.toolStripButtonWarningNewMonth.Visible = false; this.toolStripLabelWarningNewMonth.Visible = false;
                    }
                }
            }
            catch (Exception exception)
            {
                GlobalExceptionHandler.ShowExceptionMessageBox(this, exception);
            }
        }

        private void MainControllingInterface_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                if (digitInkJetDominoPrinterThread != null && digitInkJetDominoPrinterThread.IsAlive) { e.Cancel = true; return; }

                if (barcodeInkJetDominoPrinterThread != null && barcodeInkJetDominoPrinterThread.IsAlive) { e.Cancel = true; return; }

                if (cartonInkJetDominoPrinterThread != null && cartonInkJetDominoPrinterThread.IsAlive) { e.Cancel = true; return; }


                if (barcodeScannerMCUThread != null && barcodeScannerMCUThread.IsAlive) { e.Cancel = true; return; }
            }
            catch (Exception exception)
            {
                GlobalExceptionHandler.ShowExceptionMessageBox(this, exception);
            }
        }

        private void toolStripButtonSetting_Click(object sender, EventArgs e)
        {
            try
            {
                SettingFillingLineData settingFillingLineData = new SettingFillingLineData(this.fillingLineData, this.barcodeScannerMCU.MatchingPackCount == 0 & this.barcodeScannerMCU.PackInOneCartonCount == 0);

                settingFillingLineData.ShowDialog();

                settingFillingLineData.Dispose();

                this.splitContainerMatching.SplitterDistance = this.SplitterDistanceMatching();
            }
            catch (Exception exception)
            {
                GlobalExceptionHandler.ShowExceptionMessageBox(this, exception);
            }


        }


        private void SetToolStripActive()
        {
            bool anyLoopRoutine = digitInkjetDominoPrinter.LoopRoutine | barcodeInkjetDominoPrinter.LoopRoutine | cartonInkjetDominoPrinter.LoopRoutine | barcodeScannerMCU.LoopRoutine;
            bool allLoopRoutine = digitInkjetDominoPrinter.LoopRoutine && barcodeInkjetDominoPrinter.LoopRoutine && cartonInkjetDominoPrinter.LoopRoutine && barcodeScannerMCU.LoopRoutine;

            bool anyOnPrinting = digitInkjetDominoPrinter.OnPrinting | barcodeInkjetDominoPrinter.OnPrinting | cartonInkjetDominoPrinter.OnPrinting | barcodeScannerMCU.OnPrinting;
            //bool allOnPrinting = digitInkjetDominoPrinter.OnPrinting && barcodeInkjetDominoPrinter.OnPrinting && cartonInkjetDominoPrinter.OnPrinting && barcodeScannerMCU.OnPrinting;

            bool allLedGreenOn = digitInkjetDominoPrinter.LedGreenOn && barcodeInkjetDominoPrinter.LedGreenOn && cartonInkjetDominoPrinter.LedGreenOn && barcodeScannerMCU.LedGreenOn;

            this.toolStripButtonConnect.Enabled = !anyLoopRoutine;
            this.toolStripButtonDisconnect.Enabled = anyLoopRoutine && !anyOnPrinting;
            this.toolStripButtonStart.Enabled = allLoopRoutine && !anyOnPrinting && allLedGreenOn;
            this.toolStripButtonStop.Enabled = anyOnPrinting;

            this.toolStripButtonSetting.Enabled = !anyLoopRoutine;



            this.toolStripDigitLEDGreen.Enabled = digitInkjetDominoPrinter.LoopRoutine && this.digitInkjetDominoPrinter.LedGreenOn;
            this.toolStripBarcodeLEDGreen.Enabled = barcodeInkjetDominoPrinter.LoopRoutine && this.barcodeInkjetDominoPrinter.LedGreenOn;
            this.toolStripCartonLEDGreen.Enabled = cartonInkjetDominoPrinter.LoopRoutine && this.cartonInkjetDominoPrinter.LedGreenOn;
            this.toolStripScannerLEDGreen.Enabled = barcodeScannerMCU.LoopRoutine && this.barcodeScannerMCU.LedGreenOn;


            this.toolStripDigitOnPrinting.Enabled = digitInkjetDominoPrinter.OnPrinting && this.digitInkjetDominoPrinter.LedGreenOn;
            this.toolStripBarcodeOnPrinting.Enabled = barcodeInkjetDominoPrinter.OnPrinting && this.barcodeInkjetDominoPrinter.LedGreenOn;
            this.toolStripCartonOnPrinting.Enabled = cartonInkjetDominoPrinter.OnPrinting && this.cartonInkjetDominoPrinter.LedGreenOn;
            this.toolStripScannerOnPrinting.Enabled = barcodeScannerMCU.OnPrinting && this.barcodeScannerMCU.LedGreenOn;
        }

        private void CutTextBox(bool clearTextBox)
        {
            if (clearTextBox)
            {
                this.textBoxBarcodeStatus.Text = "";
                this.textBoxCartonStatus.Text = "";
                this.textBoxDigitStatus.Text = "";
                this.textBoxScannerStatus.Text = "";
            }
            else
            {
                if (this.textBoxBarcodeStatus.TextLength > 1000) this.textBoxBarcodeStatus.Text = this.textBoxBarcodeStatus.Text.Substring(0, 1000);
                if (this.textBoxCartonStatus.TextLength > 1000) this.textBoxCartonStatus.Text = this.textBoxCartonStatus.Text.Substring(0, 1000);
                if (this.textBoxDigitStatus.TextLength > 1000) this.textBoxDigitStatus.Text = this.textBoxDigitStatus.Text.Substring(0, 1000);
                if (this.textBoxScannerStatus.TextLength > 1000) this.textBoxScannerStatus.Text = this.textBoxScannerStatus.Text.Substring(0, 1000);
            }
        }


        #region Test only
        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            //this.splitContainerQuality.SplitterDistance = this.SplitterDistanceQuality();
            //this.splitContainerMatching.SplitterDistance = this.SplitterDistanceMatching();
            //this.splitContainerCarton.SplitterDistance = this.SplitterDistanceCarton();

            barcodeScannerMCU.MyTest = true;

            if (barcodeScannerMCUThread != null && barcodeScannerMCUThread.IsAlive) barcodeScannerMCUThread.Abort();
            barcodeScannerMCUThread = new Thread(new ThreadStart(barcodeScannerMCU.ThreadRoutine));
            barcodeScannerMCUThread.Start();

            Thread.Sleep(1000); //Delay 1s, then Start print
            barcodeScannerMCU.StartPrint();
        }

        private void toolStripButton4_Click(object sender, EventArgs e)
        {
            if (barcodeScannerMCU.OnPrinting)
                barcodeScannerMCU.StopPrint();
            else
                barcodeScannerMCU.StartPrint();
        }

        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            barcodeScannerMCU.LoopRoutine = false;
        }

        private void toolStripButton3_Click(object sender, EventArgs e)
        {
            barcodeScannerMCU.MyHold = !barcodeScannerMCU.MyHold;
        }

        #endregion Test only

        private void toolStripButtonMessageCount_Click(object sender, EventArgs e)
        {
            try
            {
                if (MessageBox.Show("Are you sure you want to reallocate the matching pack queue?", "Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button2) == System.Windows.Forms.DialogResult.Yes)
                    this.barcodeScannerMCU.ReAllocation();
            }
            catch (Exception exception)
            {
                GlobalExceptionHandler.ShowExceptionMessageBox(this, exception);
            }
        }

        private void dataGridViewCartonList_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {
            try
            {
                using (SolidBrush b = new SolidBrush(dataGridViewCartonList.RowHeadersDefaultCellStyle.ForeColor))
                {
                    e.Graphics.DrawString((e.RowIndex + 1).ToString(), e.InheritedRowStyle.Font, b, e.RowBounds.Location.X + 15, e.RowBounds.Location.Y + 4);
                }
            }
            catch (Exception exception)
            {
                GlobalExceptionHandler.ShowExceptionMessageBox(this, exception);
            }
        }

        private void toolStripButtonLoadData_Click(object sender, EventArgs e)
        {
            try
            {
                this.barcodeScannerMCU.LoadCartonDataTable();
            }
            catch (Exception exception)
            {
                GlobalExceptionHandler.ShowExceptionMessageBox(this, exception);
            }
        }

























    }
}


