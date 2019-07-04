using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Global.Class.Library
{
    public class UserInformation
    {
        private int userID;
        private int userOrganizationID;
        private string userDescription;

        public UserInformation() : this(-1, -1, "") { }

        public UserInformation(int userID, int userOrganizationID, string userDescription)
        {
            this.UserID = userID;
            this.UserOrganizationID = userOrganizationID;
            this.UserDescription = userDescription;
        }

        public int UserID
        {
            get { return this.userID; }
            set { this.userID = value; }
        }

        public int UserOrganizationID
        {
            get { return this.userOrganizationID; }
            set { this.userOrganizationID = value; }
        }

        public string UserDescription
        {
            get { return this.userDescription; }
            set { this.userDescription = value; }
        }
    }

    public class OptionSetting
    {
        private DateTime lowerFillterDate;
        private DateTime upperFillterDate;

        public OptionSetting() : this(DateTime.Today.AddDays(-1), DateTime.Today.AddDays(30)) { }

        public OptionSetting(DateTime lowerFillterDate, DateTime upperFillterDate)
        {
            this.LowerFillterDate = lowerFillterDate;
            this.UpperFillterDate = upperFillterDate;
        }

        public DateTime LowerFillterDate
        {
            get { return this.lowerFillterDate; }
            set { this.lowerFillterDate = value; }
        }

        public DateTime UpperFillterDate
        {
            get { return this.upperFillterDate; }
            set { this.upperFillterDate = value; }
        }

    }


}
