using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using BusinessLogicLayer;
using Global.Class.Library;
using DataTransferObject;
using Equin.ApplicationFramework;

namespace PresentationLayer
{
    public partial class PublicFind : Form
    {

        public string FilterColumnID { get; set; }

        private BindingListView<DataMessageMaster> dataMessageMasterListView;
        public PublicFind(BindingListView<DataMessageMaster> dataMessageMasterListView)
        {
            InitializeComponent();

            this.dataMessageMasterListView = dataMessageMasterListView;

            List<string> listFilterColumnID = new List<string> { EnumFilterColumnID.ProductionDate.Value, EnumFilterColumnID.LogoName.Value, EnumFilterColumnID.FactoryName.Value, EnumFilterColumnID.OwnerName.Value, EnumFilterColumnID.CategoryName.Value, EnumFilterColumnID.ProductName.Value, EnumFilterColumnID.CoilCode.Value, EnumFilterColumnID.CoilExtension.Value, EnumFilterColumnID.DataStatusID.Value, EnumFilterColumnID.Remarks.Value };
            this.FilterColumnID = EnumFilterColumnID.CoilCode.Value;

            this.comboBoxFilterColumnID.DataSource = listFilterColumnID;
            this.comboBoxFilterColumnID.DataBindings.Add("Text", this, "FilterColumnID", true);
        }

        private void PublicFind_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                this.textBoxFilterText.Text = "";
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
                if (sender.Equals(this.toolStripButtonOK))
                    this.textBoxFilterText_TextChanged(sender, e);
                else
                {
                    this.Close();
                }
            }
            catch (Exception exception)
            {
                GlobalExceptionHandler.ShowExceptionMessageBox(this, exception);
            }
        }


        private void textBoxFilterText_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if (this.textBoxFilterText.Text == "")
                    dataMessageMasterListView.RemoveFilter();
                else
                    dataMessageMasterListView.ApplyFilter(
                            delegate(DataMessageMaster dataMessageMaster)
                            {
                                if (this.FilterColumnID == EnumFilterColumnID.ProductionDate.Value)
                                    return this.FilterCompare(dataMessageMaster.ProductionDate.ToString("dd/MM/yyyy"), this.checkBoxMatchCase.Checked, this.checkBoxMatchWholeWord.Checked);
                                else if (this.FilterColumnID == EnumFilterColumnID.LogoName.Value)
                                    return this.FilterCompare(dataMessageMaster.LogoName, this.checkBoxMatchCase.Checked, this.checkBoxMatchWholeWord.Checked);
                                else if (this.FilterColumnID == EnumFilterColumnID.FactoryName.Value)
                                    return this.FilterCompare(dataMessageMaster.FactoryName, this.checkBoxMatchCase.Checked, this.checkBoxMatchWholeWord.Checked);
                                else if (this.FilterColumnID == EnumFilterColumnID.OwnerName.Value)
                                    return this.FilterCompare(dataMessageMaster.OwnerName, this.checkBoxMatchCase.Checked, this.checkBoxMatchWholeWord.Checked);
                                else if (this.FilterColumnID == EnumFilterColumnID.CategoryName.Value)
                                    return this.FilterCompare(dataMessageMaster.CategoryName, this.checkBoxMatchCase.Checked, this.checkBoxMatchWholeWord.Checked);
                                else if (this.FilterColumnID == EnumFilterColumnID.ProductName.Value)
                                    return this.FilterCompare(dataMessageMaster.ProductName, this.checkBoxMatchCase.Checked, this.checkBoxMatchWholeWord.Checked);
                                else if (this.FilterColumnID == EnumFilterColumnID.CoilCode.Value)
                                    return this.FilterCompare(dataMessageMaster.CoilCode, this.checkBoxMatchCase.Checked, this.checkBoxMatchWholeWord.Checked);
                                else if (this.FilterColumnID == EnumFilterColumnID.CoilExtension.Value)
                                    return this.FilterCompare(dataMessageMaster.CoilExtension.ToString(), this.checkBoxMatchCase.Checked, this.checkBoxMatchWholeWord.Checked);
                                else if (this.FilterColumnID == EnumFilterColumnID.Remarks.Value)
                                    return this.FilterCompare(dataMessageMaster.Remarks, this.checkBoxMatchCase.Checked, this.checkBoxMatchWholeWord.Checked);
                                else if (this.FilterColumnID == EnumFilterColumnID.DataStatusID.Value)
                                {
                                    CommonMetaList commonMetaList = new CommonMetaList();
                                    return this.FilterCompare(commonMetaList.GetDataStatusName(dataMessageMaster.DataStatusID), this.checkBoxMatchCase.Checked, this.checkBoxMatchWholeWord.Checked);
                                }
                                else
                                    return false;
                            }
                        );
            }
            catch (Exception exception)
            {
                GlobalExceptionHandler.ShowExceptionMessageBox(this, exception);
            }
        }

        private bool FilterCompare(string columnData, bool matchCase, bool matchWholeWord)
        {
            if (matchCase && matchWholeWord)
                return columnData.Equals(this.textBoxFilterText.Text);
            else if (matchCase)
                return columnData.Contains(this.textBoxFilterText.Text);
            else if (matchWholeWord)
                return columnData.ToLower().Equals(this.textBoxFilterText.Text.ToLower());
            else
                return columnData.ToLower().Contains(this.textBoxFilterText.Text.ToLower());
        }




    }

    public sealed class EnumFilterColumnID
    {
        //Enum constants can only be of ordinal types (int by default), so you can't have string constants in enums.
        //When I want something like a "string-based enum" I create a class to hold the constants like you did, except I make it a static class to prevent both unwanted instantiation and unwanted subclassing.
        //But if you don't want to use string as the type in method signatures and you prefer a safer, more restrictive type (like Operation), you can use the safe enum pattern:

        public static readonly EnumFilterColumnID ProductionDate = new EnumFilterColumnID("Ngày sản xuất");
        public static readonly EnumFilterColumnID LogoName = new EnumFilterColumnID("Logo");
        public static readonly EnumFilterColumnID FactoryName = new EnumFilterColumnID("Đơn vị sản xuất");
        public static readonly EnumFilterColumnID OwnerName = new EnumFilterColumnID("Chủ hàng");
        public static readonly EnumFilterColumnID CategoryName = new EnumFilterColumnID("Loại sản phẩm");
        public static readonly EnumFilterColumnID ProductName = new EnumFilterColumnID("Mã sản phẩm");
        public static readonly EnumFilterColumnID CoilCode = new EnumFilterColumnID("Mã cuộn");
        public static readonly EnumFilterColumnID CoilExtension = new EnumFilterColumnID("Chia cuộn");
        public static readonly EnumFilterColumnID DataStatusID = new EnumFilterColumnID("Tình trạng");
        public static readonly EnumFilterColumnID Remarks = new EnumFilterColumnID("Ghi chú");


        private EnumFilterColumnID(string value)
        {
            Value = value;
        }

        public string Value { get; private set; }
    }

}
