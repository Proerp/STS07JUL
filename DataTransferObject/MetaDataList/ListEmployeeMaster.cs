using System;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel;
using System.Text;
using Global.Class.Library;


namespace DataTransferObject.MetaDataList
{
    public class ListEmployeeMaster : NotifyPropertyChangeObject
    {
        private int employeeID;

        private DateTime entryDate;

        private string description;
        private string remarks;
        private string password;

        

        public ListEmployeeMaster()
        {
            GlobalDefaultValue.Apply(this);
            this.EntryDate = DateTime.Now;
        }

        #region Properties


        [DefaultValue(-1)]
        public int EmployeeID
        {
            get { return this.employeeID; }
            set { ApplyPropertyChange<ListEmployeeMaster, int>(ref this.employeeID, o => o.EmployeeID, value); }
        }

        [DefaultValue(typeof(DateTime), "01/01/1900")]
        public DateTime EntryDate
        {
            get { return this.entryDate; }
            set { ApplyPropertyChange<ListEmployeeMaster, DateTime>(ref this.entryDate, o => o.EntryDate, value); }
        }

        
        [DefaultValue("")]
        public string Description
        {
            get { return this.description; }
            set { ApplyPropertyChange<ListEmployeeMaster, string>(ref this.description, o => o.Description, value); }
        }
        

        [DefaultValue("")]
        public string Remarks
        {
            get { return this.remarks; }
            set { ApplyPropertyChange<ListEmployeeMaster, string>(ref this.remarks, o => o.Remarks, value); }
        }

        [DefaultValue("")]
        public string Password
        {
            get { return this.password; }
            set { ApplyPropertyChange<ListEmployeeMaster, string>(ref this.password, o => o.Password, value); }
        }

        #endregion
        

    }
}
