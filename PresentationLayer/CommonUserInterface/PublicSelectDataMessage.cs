using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using Global.Class.Library;
using DataTransferObject;
using DataAccessLayer;
using DataAccessLayer.DataMessageDTSTableAdapters;

namespace PresentationLayer
{
    public partial class PublicSelectDataMessage : Form
    {

        private DataMessageMasterTableAdapter masterTableAdapter;
        protected DataMessageMasterTableAdapter MasterTableAdapter
        {
            get
            {
                if (masterTableAdapter == null) masterTableAdapter = new DataMessageMasterTableAdapter();
                return masterTableAdapter;
            }
        }

        public DataMessageMaster DataMessageMaster {get; set;}

        public PublicSelectDataMessage(GlobalEnum.DataStatusID dataStatusID, string textTitle)
        {
            InitializeComponent();

            this.Text = textTitle;

            DataMessageDTS.DataMessageMasterDataTable dataMessageMasterDataTable = this.MasterTableAdapter.GetDataByDate(GlobalVariables.GlobalOptionSetting.LowerFillterDate, GlobalVariables.GlobalOptionSetting.UpperFillterDate);

            DataView dataMessageMasterDataView = new DataView(dataMessageMasterDataTable);
            dataMessageMasterDataView.RowFilter = "DataStatusID = " + (int)dataStatusID;

            this.dataGridViewDataMessageMaster.AutoGenerateColumns = false;
            this.dataGridViewDataMessageMaster.DataSource = dataMessageMasterDataView;
        }

        private void dataGridViewDataMessageMaster_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {
            try
            {
                using (SolidBrush b = new SolidBrush(dataGridViewDataMessageMaster.RowHeadersDefaultCellStyle.ForeColor))
                {
                    e.Graphics.DrawString((e.RowIndex + 1).ToString(), e.InheritedRowStyle.Font, b, e.RowBounds.Location.X + 15, e.RowBounds.Location.Y + 6);
                }
            }
            catch (Exception exception)
            {
                GlobalExceptionHandler.ShowExceptionMessageBox(this, exception);
            }
        }

        private void toolStripButtonEscapeAndOK_Click(object sender, EventArgs e)
        {
            try
            {
                if (sender.Equals(this.toolStripButtonOK)  && this.dataGridViewDataMessageMaster.SelectedRows.Count > 0 && this.dataGridViewDataMessageMaster.CurrentRow != null)
                {
                    DataGridViewRow dataGridViewRow = this.dataGridViewDataMessageMaster.CurrentRow;
                    if (dataGridViewRow != null)
                    {
                        DataMessageDTS.DataMessageMasterRow dataMessageMasterRow = (DataMessageDTS.DataMessageMasterRow)((DataRowView)dataGridViewRow.DataBoundItem).Row;

                        if (dataMessageMasterRow != null)
                            this.DataMessageMaster = new DataMessageMaster(dataMessageMasterRow.DataMessageID, dataMessageMasterRow.ForeignMessageID, dataMessageMasterRow.RequestedEmployeeID, dataMessageMasterRow.DataStatusID, dataMessageMasterRow.Verified, dataMessageMasterRow.VerifiedDate, dataMessageMasterRow.ProductionDate, dataMessageMasterRow.ProductionDatePrintable, dataMessageMasterRow.EntryDate, dataMessageMasterRow.BeginingDate, dataMessageMasterRow.EndingDate, dataMessageMasterRow.LogoID, dataMessageMasterRow.LogoName, dataMessageMasterRow.LogoLogo, dataMessageMasterRow.LogoPrintable, dataMessageMasterRow.FactoryID, dataMessageMasterRow.FactoryName, dataMessageMasterRow.FactoryLogo, dataMessageMasterRow.FactoryPrintable, dataMessageMasterRow.OwnerID, dataMessageMasterRow.OwnerName, dataMessageMasterRow.OwnerLogo, dataMessageMasterRow.OwnerPrintable, dataMessageMasterRow.CategoryID, dataMessageMasterRow.CategoryName, dataMessageMasterRow.CategoryLogo, dataMessageMasterRow.CategoryPrintable, dataMessageMasterRow.ProductID, dataMessageMasterRow.ProductName, dataMessageMasterRow.ProductLogo, dataMessageMasterRow.ProductPrintable, dataMessageMasterRow.CoilID, dataMessageMasterRow.CoilCode, dataMessageMasterRow.CoilExtension, dataMessageMasterRow.CoilPrintable, dataMessageMasterRow.CounterValue, dataMessageMasterRow.CounterAutonics, dataMessageMasterRow.CounterPrintable, dataMessageMasterRow.Remarks); 
                        
                    }
                }
                this.DialogResult = sender.Equals(this.toolStripButtonOK) ? System.Windows.Forms.DialogResult.Yes : System.Windows.Forms.DialogResult.No;
            }
            catch (Exception exception)
            {
                this.DialogResult = System.Windows.Forms.DialogResult.None;
                GlobalExceptionHandler.ShowExceptionMessageBox(this, exception);
            }
        }

        
        
    }
}
