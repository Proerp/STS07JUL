using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Data;
using System.Transactions;
using System.Data.SqlClient;

using Global.Class.Library;
using DataTransferObject;
using DataTransferObject.MetaDataList;
using DataAccessLayer;
using DataAccessLayer.MetaDataList;
using DataAccessLayer.MetaDataList.ListEmployeeDTSTableAdapters;

namespace BusinessLogicLayer.MetaDataList
{
    public class ListEmployeeBLL : NotifyPropertyChangeObject
    {
        public GlobalEnum.TaskID TaskID { get { return GlobalEnum.TaskID.ListEmployee; } }

        private UserInformation userOrganization;

        private ListEmployeeMaster listEmployeeMaster;

        private BindingList<ListEmployeeDetail> listEmployeeDetailList;



        public ListEmployeeBLL()
        {
            try
            {
                if (GlobalVariables.shouldRestoreProcedure) RestoreProcedure();


                userOrganization = new UserInformation();

                listEmployeeMaster = new ListEmployeeMaster();

                this.listEmployeeDetailList = new BindingList<ListEmployeeDetail>();

                GlobalDefaultValue.Apply(this);


                this.ListEmployeeMaster.PropertyChanged += new PropertyChangedEventHandler(ListEmployeeMaster_PropertyChanged);

                this.ListEmployeeDetailList.ListChanged += new ListChangedEventHandler(ListEmployeeDetailList_ListChanged);

            }
            catch (Exception exception)
            {
                throw exception;
            }
        }

        private void ListEmployeeMaster_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            this.SetDirty();
        }

        private void ListEmployeeDetailList_ListChanged(object sender, ListChangedEventArgs e)
        {
            this.SetDirty();
        }




        #region <Adapter>

        private ListEmployeeListingTableAdapter listingTableAdapter;
        protected ListEmployeeListingTableAdapter ListingTableAdapter
        {
            get
            {
                if (listingTableAdapter == null) listingTableAdapter = new ListEmployeeListingTableAdapter();
                return listingTableAdapter;
            }
        }

        private ListEmployeeTableAdapter masterTableAdapter;
        protected ListEmployeeTableAdapter MasterTableAdapter
        {
            get
            {
                if (masterTableAdapter == null) masterTableAdapter = new ListEmployeeTableAdapter();
                return masterTableAdapter;
            }
        }

        private ListEmployeeDetailTableAdapter detailTableAdapter;
        protected ListEmployeeDetailTableAdapter DetailTableAdapter
        {
            get
            {
                if (detailTableAdapter == null) detailTableAdapter = new ListEmployeeDetailTableAdapter();
                return detailTableAdapter;
            }
        }


        #endregion <Adapter>

        #region <Storage>

        public UserInformation UserOrganization
        {
            get { return this.userOrganization; }
            set { this.userOrganization = value; }
        }

        public ListEmployeeDTS.ListEmployeeListingDataTable ListEmployeeListing()
        {
            return this.ListingTableAdapter.GetData();
        }

        public ListEmployeeMaster ListEmployeeMaster
        {
            get { return this.listEmployeeMaster; }
        }

        public BindingList<ListEmployeeDetail> ListEmployeeDetailList
        {
            get { return this.listEmployeeDetailList; }
        }


        #endregion <Storage>

        #region Properties

        #region <Primary Key>

        public int EmployeeID   //Primary Key
        {
            get { return this.ListEmployeeMaster.EmployeeID; }
            private set
            {
                if (this.ListEmployeeMaster.EmployeeID != value)
                {
                    this.StopTracking();

                    this.ListEmployeeMaster.EmployeeID = value;

                    this.ListEmployeeGetMaster();
                    this.ListEmployeeGetDetail();

                    this.StartTracking();
                    this.Reset();
                }

            }
        }

        #endregion <Primary Key>

        private void ListEmployeeGetMaster()
        {
            if (this.EmployeeID > 0)
            {
                ListEmployeeDTS.ListEmployeeDataTable masterDataTable = this.MasterTableAdapter.GetData(this.EmployeeID);

                if (masterDataTable.Count > 0)
                {
                    this.ListEmployeeMaster.StopTracking();

                    this.ListEmployeeMaster.EntryDate = masterDataTable[0].EntryDate;

                    this.ListEmployeeMaster.Description = masterDataTable[0].Description;
                    this.ListEmployeeMaster.Remarks = masterDataTable[0].Remarks;
                    this.ListEmployeeMaster.Password = masterDataTable[0].Password;

                    this.ListEmployeeMaster.StartTracking();

                    this.ListEmployeeMaster.Reset();

                    this.UserOrganization.UserID = masterDataTable[0].UserID;
                    this.UserOrganization.UserOrganizationID = masterDataTable[0].UserOrganizationID;
                }
                else throw new System.ArgumentException("Insufficient get data");
            }
            else
            {
                GlobalDefaultValue.Apply(this.ListEmployeeMaster);
                this.ListEmployeeMaster.EntryDate = DateTime.Today;
                this.ListEmployeeMaster.Reset();
            }
        }


        private void ListEmployeeGetDetail()
        {
            this.listEmployeeDetailList.RaiseListChangedEvents = false;
            this.listEmployeeDetailList.Clear();
            if (this.EmployeeID >= 0)
            {

                ListEmployeeDTS.ListEmployeeDetailDataTable detailDataTable = this.DetailTableAdapter.GetData(this.EmployeeID);

                if (detailDataTable.Count > 0)
                {
                    foreach (ListEmployeeDTS.ListEmployeeDetailRow detailRow in detailDataTable.Rows)
                    {
                        this.listEmployeeDetailList.Add(new ListEmployeeDetail(detailRow.CommonID, detailRow.CommonValue, detailRow.Remarks));
                    }
                }
            }
            this.listEmployeeDetailList.RaiseListChangedEvents = true;
            this.listEmployeeDetailList.ResetBindings();
        }

        #endregion Properties





        #region Object Permission

        public override bool IsValid
        {
            get
            {
                List<ListEmployeeDetail> inValidListEmployeeDetail = this.ListEmployeeDetailList.Where(listEmployeeDetail => !listEmployeeDetail.IsValid).ToList();
                return this.ListEmployeeMaster.IsValid && inValidListEmployeeDetail.Count == 0;
            }
        }

        public bool ReadOnly
        {
            get
            {
                try
                {
                    return GlobalUserPermission.GetUserReadOnly(GlobalVariables.GlobalUserInformation.UserID, this.TaskID);
                }
                catch
                {
                    return true;
                }
            }
        }

        public bool Editable
        {
            get
            {
                try
                {
                    if (!GlobalUserPermission.GetUserEditable(GlobalVariables.GlobalUserInformation.UserID, this.TaskID, this.UserOrganization.UserOrganizationID)) return false;

                    return GlobalUserPermission.GetEditable("ListEmployeeEditable", this.EmployeeID);
                }
                catch
                {
                    return false;
                }
            }
        }

        public bool Verifiable
        {
            get
            {
                try
                {
                    if (!GlobalUserPermission.GetUserEditable(GlobalVariables.GlobalUserInformation.UserID, GlobalEnum.TaskID.ListEmployeeVerifiable, this.UserOrganization.UserOrganizationID)) return false;

                    return GlobalUserPermission.GetEditable("ListEmployeeEditable", this.EmployeeID);
                }
                catch
                {
                    return false;
                }
            }
        }

        public bool Unverifiable
        {
            get
            {
                try
                {
                    if (!GlobalUserPermission.GetUserEditable(GlobalVariables.GlobalUserInformation.UserID, GlobalEnum.TaskID.ListEmployeeUnverifiable, this.UserOrganization.UserOrganizationID)) return false;

                    return GlobalUserPermission.GetEditable("ListEmployeeEditable", this.EmployeeID);
                }
                catch
                {
                    return false;
                }
            }
        }


        #endregion Object Permission






        public void Fill(int employeeID)
        {
            if (this.EmployeeID == employeeID) this.EmployeeID = -1; //Enforce to reload
            this.EmployeeID = employeeID;
        }

        public void New()
        {
            if (this.EmployeeID == 0) this.EmployeeID = -1;
            this.EmployeeID = 0;
        }

        public void Edit()
        {

        }

        #region Save & Delete Method


        public bool Save()
        {
            int employeeID = 0;

            try
            {

                if (!this.SaveValidate()) throw new System.ArgumentException("Insufficient save", "Save validate");

                using (TransactionScope transactionScope = new TransactionScope())
                {
                    if (this.ReadOnly) throw new System.ArgumentException("Insufficient save", "Uneditable");

                    if (!this.SaveMaster(ref employeeID)) throw new System.ArgumentException("Insufficient save", "Save master");

                    if (!this.SaveDetail(employeeID)) throw new System.ArgumentException("Insufficient save", "Save detail");

                    transactionScope.Complete();
                }

                this.Fill(employeeID);
                return true;
            }
            catch (System.Exception exception)
            {
                throw exception;
            }
        }


        public bool Delete()
        {
            if (this.listEmployeeMaster.EmployeeID <= 0) return false;

            try
            {

                if (!this.SaveValidate()) throw new System.ArgumentException("Insufficient delete", "Delete validate");

                using (TransactionScope transactionScope = new TransactionScope())
                {
                    if (!this.Editable) throw new System.ArgumentException("Insufficient delete", "Uneditable");

                    if (!this.SaveUndo(this.listEmployeeMaster.EmployeeID)) throw new System.ArgumentException("Insufficient delete", "Delete detail");

                    if (this.MasterTableAdapter.Delete(this.listEmployeeMaster.EmployeeID) != 1) throw new System.ArgumentException("Insufficient delete", "Delete master");

                    transactionScope.Complete();
                }
                this.Fill(0);
                return true;
            }
            catch (System.Exception exception)
            {
                throw exception;
            }
        }


        private bool SaveValidate()
        {
            ExceptionTable exceptionTable = new ExceptionTable(new string[2, 2] { { "ExceptionCategory", "System.String" }, { "ExceptionDescription", "System.String" } });

            this.UserOrganization = GlobalUserPermission.GetUserInformation(GlobalVariables.GlobalUserInformation.UserID, this.ListEmployeeMaster.EntryDate);

            if (this.UserOrganization.UserID <= 0 || this.UserOrganization.UserOrganizationID <= 0) exceptionTable.AddException(new string[] { GlobalVariables.stringFieldRequired, "User information" });

            if (exceptionTable.Table.Rows.Count <= 0 && this.IsValid) return true; else throw new CustomException("Save validate", exceptionTable.Table);
        }


        private bool SaveMaster(ref int employeeID)
        {
            ListEmployeeDTS.ListEmployeeDataTable masterDataTable;
            ListEmployeeDTS.ListEmployeeRow masterRow;

            if (this.listEmployeeMaster.EmployeeID <= 0) //Add
            {
                masterDataTable = new ListEmployeeDTS.ListEmployeeDataTable();
                masterRow = masterDataTable.NewListEmployeeRow();
                masterRow.Password = this.listEmployeeMaster.Password;
            }
            else //Edit
            {
                if (!this.SaveUndo(listEmployeeMaster.EmployeeID)) throw new System.ArgumentException("Insufficient save", "Save undo");
                masterDataTable = this.MasterTableAdapter.GetData(listEmployeeMaster.EmployeeID);
                if (masterDataTable.Count > 0) masterRow = masterDataTable[0]; else throw new System.ArgumentException("Insufficient save", "Get for edit");
            }

            masterRow.EntryDate = DateTime.Now;

            masterRow.Description = this.listEmployeeMaster.Description;
            masterRow.Remarks = this.listEmployeeMaster.Remarks;

            masterRow.UserID = this.UserOrganization.UserID;
            masterRow.UserOrganizationID = this.UserOrganization.UserOrganizationID;

            masterRow.EntryStatusID = this.listEmployeeMaster.EmployeeID <= 0 || (int)masterDataTable[0]["EntryStatusID"] == (int)GlobalEnum.EntryStatusID.IsNew ? (int)GlobalEnum.EntryStatusID.IsNew : (int)GlobalEnum.EntryStatusID.IsEdited;

            if (this.listEmployeeMaster.EmployeeID <= 0) masterDataTable.AddListEmployeeRow(masterRow);

            int rowsAffected = this.MasterTableAdapter.Update(masterRow);

            employeeID = masterRow.EmployeeID;

            return rowsAffected == 1;

        }


        private bool SaveDetail(int employeeID)
        {
            int serialID = 0; int rowsAffected = 0;


            #region <Save collection>

            serialID = 0;

            ListEmployeeDTS.ListEmployeeDetailDataTable detailDataTable = new ListEmployeeDTS.ListEmployeeDetailDataTable();

            foreach (ListEmployeeDetail listEmployeeDetail in this.ListEmployeeDetailList)
            {
                ListEmployeeDTS.ListEmployeeDetailRow detailRow = detailDataTable.NewListEmployeeDetailRow();

                detailRow.EmployeeID = employeeID;
                detailRow.SerialID = ++serialID;

                detailRow.CommonID = listEmployeeDetail.CommonID;
                detailRow.CommonValue = listEmployeeDetail.CommonValue;

                detailRow.Remarks = listEmployeeDetail.Remarks;

                detailDataTable.AddListEmployeeDetailRow(detailRow);
            }

            rowsAffected = this.DetailTableAdapter.Update(detailDataTable);
            if (rowsAffected != this.listEmployeeDetailList.Count) throw new System.ArgumentException("Insufficient save", "Save detail");


            #endregion <Save collection>


            return true;
        }


        private bool SaveUndo(int employeeID)
        {
            this.DetailTableAdapter.Delete(employeeID);

            return true;
        }



        #endregion



        private void RestoreProcedure()
        {
            string queryString;
            string[] queryArray;



            queryString = "     " + "\r\n";
            queryString = queryString + " WITH ENCRYPTION " + "\r\n";
            queryString = queryString + " AS " + "\r\n";
            queryString = queryString + "       SELECT      EmployeeID, Description, EntryDate, Remarks " + "\r\n";
            queryString = queryString + "       FROM        ListEmployee " + "\r\n";
            queryString = queryString + "       ORDER BY    Description " + "\r\n";

            SQLDatabase.CreateStoredProcedure("ListEmployeeListing", queryString);



            queryString = "     @EmployeeID Int " + "\r\n";
            queryString = queryString + " WITH ENCRYPTION " + "\r\n";
            queryString = queryString + " AS " + "\r\n";
            queryString = queryString + "       SELECT      EmployeeID, Description, EntryDate, Remarks, Password, UserID, UserOrganizationID, EntryStatusID " + "\r\n";
            queryString = queryString + "       FROM        ListEmployee " + "\r\n";
            queryString = queryString + "       WHERE       EmployeeID = @EmployeeID " + "\r\n";

            SQLDatabase.CreateStoredProcedure("ListEmployeeSelect", queryString);


            queryString = "     @Description nvarchar(100),	@EntryDate datetime, @Remarks nvarchar(100), @Password nvarchar(100), @UserID int, @UserOrganizationID int, @EntryStatusID int " + "\r\n";
            queryString = queryString + " WITH ENCRYPTION " + "\r\n";
            queryString = queryString + " AS " + "\r\n";
            queryString = queryString + "       INSERT INTO ListEmployee ([Description], [EntryDate], [Remarks], [Password], [UserID], [UserOrganizationID], [EntryStatusID]) VALUES (@Description, @EntryDate, @Remarks, @Password, @UserID, @UserOrganizationID, @EntryStatusID) " + "\r\n";
            queryString = queryString + "       SELECT      EmployeeID, Description, EntryDate, Remarks, Password, UserID, UserOrganizationID, EntryStatusID FROM ListEmployee WHERE EmployeeID = SCOPE_IDENTITY() " + "\r\n";

            SQLDatabase.CreateStoredProcedure("ListEmployeeInsert", queryString);


            queryString = "     @EmployeeID int, @Description nvarchar(100), @EntryDate datetime, @Remarks nvarchar(100), @Password nvarchar(100), @UserID int, @UserOrganizationID int, @EntryStatusID int " + "\r\n";
            queryString = queryString + " WITH ENCRYPTION " + "\r\n";
            queryString = queryString + " AS " + "\r\n";
            queryString = queryString + "       UPDATE      ListEmployee SET [Description] = @Description, [EntryDate] = @EntryDate, [Remarks] = @Remarks, [Password] = @Password, [UserID] = @UserID, [UserOrganizationID] = @UserOrganizationID, [EntryStatusID] = @EntryStatusID WHERE EmployeeID = @EmployeeID " + "\r\n";
            queryString = queryString + "       SELECT      EmployeeID, Description, EntryDate, Remarks, Password, UserID, UserOrganizationID, EntryStatusID FROM ListEmployee WHERE EmployeeID = @EmployeeID " + "\r\n";

            SQLDatabase.CreateStoredProcedure("ListEmployeeUpdate", queryString);


            queryString = " @EmployeeID int ";
            queryString = queryString + " WITH ENCRYPTION " + "\r\n";
            queryString = queryString + " AS " + "\r\n";
            queryString = queryString + "       DELETE FROM ListEmployee WHERE EmployeeID = @EmployeeID " + "\r\n";
            SQLDatabase.CreateStoredProcedure("ListEmployeeDelete", queryString);






            queryString = " @EmployeeID int " + "\r\n";
            queryString = queryString + " WITH ENCRYPTION " + "\r\n";
            queryString = queryString + " AS " + "\r\n";
            queryString = queryString + "       SELECT      EmployeeID, SerialID, CommonID, CommonValue, Remarks " + "\r\n";
            queryString = queryString + "       FROM        ListEmployeeDetail " + "\r\n";
            queryString = queryString + "       WHERE       EmployeeID = @EmployeeID " + "\r\n";

            SQLDatabase.CreateStoredProcedure("ListEmployeeDetailSelect", queryString);


            queryString = " @EmployeeID int, @SerialID int,	@CommonID int, @CommonValue float, @Remarks nvarchar(100) " + "\r\n";
            queryString = queryString + " WITH ENCRYPTION " + "\r\n";
            queryString = queryString + " AS " + "\r\n";
            queryString = queryString + "       INSERT INTO ListEmployeeDetail (EmployeeID, SerialID, CommonID, CommonValue, Remarks) VALUES (@EmployeeID, @SerialID, @CommonID, @CommonValue, @Remarks) " + "\r\n";
            queryString = queryString + "       SELECT      EmployeeID, SerialID, CommonID, CommonValue, Remarks FROM ListEmployeeDetail WHERE (EmployeeID = @EmployeeID) AND (SerialID = @SerialID) " + "\r\n";

            SQLDatabase.CreateStoredProcedure("ListEmployeeDetailInsert", queryString);



            queryString = " @EmployeeID int, @SerialID int,	@CommonID int, @CommonValue float, @Remarks nvarchar(100) " + "\r\n";
            queryString = queryString + " WITH ENCRYPTION " + "\r\n";
            queryString = queryString + " AS " + "\r\n";
            queryString = queryString + "       UPDATE      ListEmployeeDetail SET CommonID = @CommonID, CommonValue = @CommonValue, Remarks = @Remarks WHERE EmployeeID = @EmployeeID AND SerialID = @SerialID " + "\r\n";
            queryString = queryString + "       SELECT      EmployeeID, SerialID, CommonID, CommonValue, Remarks FROM ListEmployeeDetail WHERE EmployeeID = @EmployeeID AND SerialID = @SerialID " + "\r\n";

            SQLDatabase.CreateStoredProcedure("ListEmployeeDetailUpdate", queryString);



            queryString = " @EmployeeID int " + "\r\n";
            queryString = queryString + " WITH ENCRYPTION " + "\r\n";
            queryString = queryString + " AS " + "\r\n";
            queryString = queryString + "       DELETE FROM ListEmployeeDetail WHERE EmployeeID = @EmployeeID " + "\r\n";
            SQLDatabase.CreateStoredProcedure("ListEmployeeDetailDelete", queryString);



            /// <summary>
            /// Check for editable
            /// </summary>
            queryArray = new string[7];
            queryArray[0] = "   SELECT      TOP 1 UserID FROM ListLogo WHERE UserID = @FindIdentityID ";
            queryArray[1] = "   SELECT      TOP 1 UserID FROM ListFactory WHERE UserID = @FindIdentityID ";
            queryArray[2] = "   SELECT      TOP 1 UserID FROM ListOwner WHERE UserID = @FindIdentityID ";
            queryArray[3] = "   SELECT      TOP 1 UserID FROM ListCategory WHERE UserID = @FindIdentityID ";
            queryArray[4] = "   SELECT      TOP 1 UserID FROM ListProduct WHERE UserID = @FindIdentityID ";
            queryArray[5] = "   SELECT      TOP 1 UserID FROM ListCoil WHERE UserID = @FindIdentityID ";
            queryArray[6] = "   SELECT      TOP 1 UserID FROM DataMessageMaster WHERE UserID = @FindIdentityID OR PrintedUserID = @FindIdentityID ";

            SQLDatabase.CreateProcedureToCheckExisting("ListEmployeeEditable", queryArray);
        }
    }
}
