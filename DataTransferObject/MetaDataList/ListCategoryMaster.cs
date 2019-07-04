using System;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel;
using System.Text;
using Global.Class.Library;

namespace DataTransferObject.MetaDataList
{
    public class ListCategoryMaster : NotifyPropertyChangeObject
    {
        private int categoryID;

        private DateTime entryDate;

        private string description;
        private string logoGenerator;
        private string remarks;

        

        public ListCategoryMaster()
        {
            GlobalDefaultValue.Apply(this);
            this.EntryDate = DateTime.Now;
        }

        #region Properties


        [DefaultValue(-1)]
        public int CategoryID
        {
            get { return this.categoryID; }
            set { ApplyPropertyChange<ListCategoryMaster, int>(ref this.categoryID, o => o.CategoryID, value); }
        }

        [DefaultValue(typeof(DateTime), "01/01/1900")]
        public DateTime EntryDate
        {
            get { return this.entryDate; }
            set { ApplyPropertyChange<ListCategoryMaster, DateTime>(ref this.entryDate, o => o.EntryDate, value); }
        }

        
        [DefaultValue("")]
        public string Description
        {
            get { return this.description; }
            set { ApplyPropertyChange<ListCategoryMaster, string>(ref this.description, o => o.Description, value); }
        }
        
        [DefaultValue("")]
        public string LogoGenerator
        {
            get { return this.logoGenerator; }
            set { ApplyPropertyChange<ListCategoryMaster, string>(ref this.logoGenerator, o => o.LogoGenerator, value); }
        }


        [DefaultValue("")]
        public string Remarks
        {
            get { return this.remarks; }
            set { ApplyPropertyChange<ListCategoryMaster, string>(ref this.remarks, o => o.Remarks, value); }
        }
        
        #endregion
        
    }
}
