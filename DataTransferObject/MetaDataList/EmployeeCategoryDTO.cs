using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace DataTransferObject
{
    public class EmployeeCategoryDTO : INotifyPropertyChanged
    {

        private int employeeCategoryID;
        private string employeeCategoryName;
        private string serialName;
        private string description;
        private string remarks;

        #region Constructor


        public EmployeeCategoryDTO()
        {
            this.EmployeeCategoryID = -1;
            this.EmployeeCategoryName = "";
            this.SerialName = "";
            this.Description = "";
            this.Remarks = "";
        }

        public EmployeeCategoryDTO(int employeeCategoryID, string employeeCategoryName, string description)
        {
            this.EmployeeCategoryID = employeeCategoryID;
            this.EmployeeCategoryName = employeeCategoryName;
            this.SerialName = "";
            this.Description = description;
            this.Remarks = "";
        }

        #endregion Constructor


        #region Implement INotifyPropertyChanged

        public event PropertyChangedEventHandler PropertyChanged;

        // This method is called by the Set accessor of each property. 
        // The CallerMemberName attribute that is applied to the optional propertyName 
        // parameter causes the property name of the caller to be substituted as an argument. 
        private void NotifyPropertyChanged(String propertyName = "") //[CallerMemberName] 
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        #endregion Implement INotifyPropertyChanged

       
        #region Public Properties

        public int EmployeeCategoryID
        {
            get
            {
                return this.employeeCategoryID;
            }

            set
            {
                if (value != this.employeeCategoryID)
                {
                    this.employeeCategoryID = value;
                    NotifyPropertyChanged("EmployeeCategoryID");
                }
            }
        }


        public string EmployeeCategoryName
        {
            get
            {
                return this.employeeCategoryName;
            }

            set
            {
                if (value != this.employeeCategoryName)
                {
                    this.employeeCategoryName = value;
                    NotifyPropertyChanged("EmployeeCategoryName");
                }
            }
        }


        public string SerialName
        {
            get
            {
                return this.serialName;
            }

            set
            {
                if (value != this.serialName)
                {
                    this.serialName = value;
                    NotifyPropertyChanged("SerialName");
                }
            }
        }


        public string Description
        {
            get
            {
                return this.description;
            }

            set
            {
                if (value != this.description)
                {
                    this.description = value;
                    NotifyPropertyChanged("Description");
                }
            }
        }


        public string Remarks
        {
            get
            {
                return this.remarks;
            }

            set
            {
                if (value != this.remarks)
                {
                    this.remarks = value;
                    NotifyPropertyChanged("Remarks");
                }
            }
        }


        #endregion Public Properties



    }
}
