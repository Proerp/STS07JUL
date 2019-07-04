using System;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel;
using System.Text;
using Global.Class.Library;

namespace DataTransferObject.MetaDataList
{
    public class ListCoilMaster : NotifyPropertyChangeObject
    {
        private int coilID;

        private DateTime entryDate;

        private string description;
        private string remarks;

        

        public ListCoilMaster()
        {
            GlobalDefaultValue.Apply(this);
            this.EntryDate = DateTime.Now;
        }

        #region Properties


        [DefaultValue(-1)]
        public int CoilID
        {
            get { return this.coilID; }
            set { ApplyPropertyChange<ListCoilMaster, int>(ref this.coilID, o => o.CoilID, value); }
        }

        [DefaultValue(typeof(DateTime), "01/01/1900")]
        public DateTime EntryDate
        {
            get { return this.entryDate; }
            set { ApplyPropertyChange<ListCoilMaster, DateTime>(ref this.entryDate, o => o.EntryDate, value); }
        }

        
        [DefaultValue("")]
        public string Description
        {
            get { return this.description; }
            set { ApplyPropertyChange<ListCoilMaster, string>(ref this.description, o => o.Description, value); }
        }
        

        [DefaultValue("")]
        public string Remarks
        {
            get { return this.remarks; }
            set { ApplyPropertyChange<ListCoilMaster, string>(ref this.remarks, o => o.Remarks, value); }
        }
        
        #endregion
        
    }
}
