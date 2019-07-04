using System;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel;
using System.Text;
using Global.Class.Library;

namespace DataTransferObject.MetaDataList
{
    public class ListProductMaster : NotifyPropertyChangeObject
    {
        private int productID;

        private DateTime entryDate;

        private string description;
        private string logoGenerator;
        private string remarks;

        

        public ListProductMaster()
        {
            GlobalDefaultValue.Apply(this);
            this.EntryDate = DateTime.Now;
        }

        #region Properties


        [DefaultValue(-1)]
        public int ProductID
        {
            get { return this.productID; }
            set { ApplyPropertyChange<ListProductMaster, int>(ref this.productID, o => o.ProductID, value); }
        }

        [DefaultValue(typeof(DateTime), "01/01/1900")]
        public DateTime EntryDate
        {
            get { return this.entryDate; }
            set { ApplyPropertyChange<ListProductMaster, DateTime>(ref this.entryDate, o => o.EntryDate, value); }
        }

        
        [DefaultValue("")]
        public string Description
        {
            get { return this.description; }
            set { ApplyPropertyChange<ListProductMaster, string>(ref this.description, o => o.Description, value); }
        }
        
        [DefaultValue("")]
        public string LogoGenerator
        {
            get { return this.logoGenerator; }
            set { ApplyPropertyChange<ListProductMaster, string>(ref this.logoGenerator, o => o.LogoGenerator, value); }
        }


        [DefaultValue("")]
        public string Remarks
        {
            get { return this.remarks; }
            set { ApplyPropertyChange<ListProductMaster, string>(ref this.remarks, o => o.Remarks, value); }
        }
        
        #endregion
        
    }
}
