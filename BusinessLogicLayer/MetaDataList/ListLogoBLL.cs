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
using DataAccessLayer.MetaDataList.ListLogoDTSTableAdapters;

namespace BusinessLogicLayer.MetaDataList
{
    public class ListLogoBLL : NotifyPropertyChangeObject
    {
        public GlobalEnum.TaskID TaskID { get { return GlobalEnum.TaskID.ListLogo; } }

        private UserInformation userOrganization;

        private ListLogoMaster listLogoMaster;

        private BindingList<ListLogoDetail> listLogoDetailList;



        public ListLogoBLL()
        {
            try
            {
                if (GlobalVariables.shouldRestoreProcedure) RestoreProcedure();


                userOrganization = new UserInformation();

                listLogoMaster = new ListLogoMaster();

                this.listLogoDetailList = new BindingList<ListLogoDetail>();

                GlobalDefaultValue.Apply(this);


                this.ListLogoMaster.PropertyChanged += new PropertyChangedEventHandler(ListLogoMaster_PropertyChanged);

                this.ListLogoDetailList.ListChanged += new ListChangedEventHandler(ListLogoDetailList_ListChanged);

            }
            catch (Exception exception)
            {
                throw exception;
            }
        }

        private void ListLogoMaster_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            this.SetDirty();
        }

        private void ListLogoDetailList_ListChanged(object sender, ListChangedEventArgs e)
        {
            this.SetDirty();
        }




        #region <Adapter>

        private ListLogoListingTableAdapter listingTableAdapter;
        protected ListLogoListingTableAdapter ListingTableAdapter
        {
            get
            {
                if (listingTableAdapter == null) listingTableAdapter = new ListLogoListingTableAdapter();
                return listingTableAdapter;
            }
        }

        private ListLogoTableAdapter masterTableAdapter;
        protected ListLogoTableAdapter MasterTableAdapter
        {
            get
            {
                if (masterTableAdapter == null) masterTableAdapter = new ListLogoTableAdapter();
                return masterTableAdapter;
            }
        }

        private ListLogoDetailTableAdapter detailTableAdapter;
        protected ListLogoDetailTableAdapter DetailTableAdapter
        {
            get
            {
                if (detailTableAdapter == null) detailTableAdapter = new ListLogoDetailTableAdapter();
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

        public ListLogoDTS.ListLogoListingDataTable ListLogoListing()
        {
            return this.ListingTableAdapter.GetData();
        }

        public ListLogoMaster ListLogoMaster
        {
            get { return this.listLogoMaster; }
        }

        public BindingList<ListLogoDetail> ListLogoDetailList
        {
            get { return this.listLogoDetailList; }
        }


        #endregion <Storage>

        #region Properties

        #region <Primary Key>

        public int LogoID   //Primary Key
        {
            get { return this.ListLogoMaster.LogoID; }
            private set
            {
                if (this.ListLogoMaster.LogoID != value)
                {
                    this.StopTracking();

                    this.ListLogoMaster.LogoID = value;

                    this.ListLogoGetMaster();
                    this.ListLogoGetDetail();

                    this.StartTracking();
                    this.Reset();
                }

            }
        }

        #endregion <Primary Key>

        private void ListLogoGetMaster()
        {
            if (this.LogoID > 0)
            {
                ListLogoDTS.ListLogoDataTable masterDataTable = this.MasterTableAdapter.GetData(this.LogoID);

                if (masterDataTable.Count > 0)
                {
                    this.ListLogoMaster.StopTracking();

                    this.ListLogoMaster.EntryDate = masterDataTable[0].EntryDate;

                    this.ListLogoMaster.Description = masterDataTable[0].Description;
                    this.ListLogoMaster.LogoGenerator = masterDataTable[0].LogoGenerator;
                    this.ListLogoMaster.Remarks = masterDataTable[0].Remarks;

                    this.ListLogoMaster.StartTracking();

                    this.ListLogoMaster.Reset();

                    this.UserOrganization.UserID = masterDataTable[0].UserID;
                    this.UserOrganization.UserOrganizationID = masterDataTable[0].UserOrganizationID;
                }
                else throw new System.ArgumentException("Insufficient get data");
            }
            else
            {
                GlobalDefaultValue.Apply(this.ListLogoMaster);
                this.ListLogoMaster.EntryDate = DateTime.Today;
                this.ListLogoMaster.Reset();
            }
        }


        private void ListLogoGetDetail()
        {
            this.listLogoDetailList.RaiseListChangedEvents = false;
            this.listLogoDetailList.Clear();
            if (this.LogoID >= 0)
            {

                ListLogoDTS.ListLogoDetailDataTable detailDataTable = this.DetailTableAdapter.GetData(this.LogoID);

                if (detailDataTable.Count > 0)
                {
                    foreach (ListLogoDTS.ListLogoDetailRow detailRow in detailDataTable.Rows)
                    {
                        this.listLogoDetailList.Add(new ListLogoDetail(detailRow.CommonID, detailRow.CommonValue, detailRow.Remarks));
                    }
                }
            }
            this.listLogoDetailList.RaiseListChangedEvents = true;
            this.listLogoDetailList.ResetBindings();
        }

        #endregion Properties





        #region Object Permission

        public override bool IsValid
        {
            get
            {
                List<ListLogoDetail> inValidListLogoDetail = this.ListLogoDetailList.Where(listLogoDetail => !listLogoDetail.IsValid).ToList();
                return this.ListLogoMaster.IsValid && inValidListLogoDetail.Count == 0;
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

                    return GlobalUserPermission.GetEditable("ListLogoEditable", this.LogoID);
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
                    if (!GlobalUserPermission.GetUserEditable(GlobalVariables.GlobalUserInformation.UserID, GlobalEnum.TaskID.ListLogoVerifiable, this.UserOrganization.UserOrganizationID)) return false;

                    return GlobalUserPermission.GetEditable("ListLogoEditable", this.LogoID);
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
                    if (!GlobalUserPermission.GetUserEditable(GlobalVariables.GlobalUserInformation.UserID, GlobalEnum.TaskID.ListLogoUnverifiable, this.UserOrganization.UserOrganizationID)) return false;

                    return GlobalUserPermission.GetEditable("ListLogoEditable", this.LogoID);
                }
                catch
                {
                    return false;
                }
            }
        }


        #endregion Object Permission






        public void Fill(int logoID)
        {
            if (this.LogoID == logoID) this.LogoID = -1; //Enforce to reload
            this.LogoID = logoID;
        }

        public void New()
        {
            if (this.LogoID == 0) this.LogoID = -1;
            this.LogoID = 0;
        }

        public void Edit()
        {

        }

        #region Save & Delete Method


        public bool Save()
        {
            int logoID = 0;

            try
            {

                if (!this.SaveValidate()) throw new System.ArgumentException("Insufficient save", "Save validate");

                using (TransactionScope transactionScope = new TransactionScope())
                {
                    if (this.ReadOnly) throw new System.ArgumentException("Insufficient save", "Uneditable");

                    if (!this.SaveMaster(ref logoID)) throw new System.ArgumentException("Insufficient save", "Save master");

                    if (!this.SaveDetail(logoID)) throw new System.ArgumentException("Insufficient save", "Save detail");

                    transactionScope.Complete();
                }

                this.Fill(logoID);
                return true;
            }
            catch (System.Exception exception)
            {
                throw exception;
            }
        }


        public bool Delete()
        {
            if (this.listLogoMaster.LogoID <= 0) return false;

            try
            {

                if (!this.SaveValidate()) throw new System.ArgumentException("Insufficient delete", "Delete validate");

                using (TransactionScope transactionScope = new TransactionScope())
                {
                    if (!this.Editable) throw new System.ArgumentException("Insufficient delete", "Uneditable");

                    if (!this.SaveUndo(this.listLogoMaster.LogoID)) throw new System.ArgumentException("Insufficient delete", "Delete detail");

                    if (this.MasterTableAdapter.Delete(this.listLogoMaster.LogoID) != 1) throw new System.ArgumentException("Insufficient delete", "Delete master");

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

            this.UserOrganization = GlobalUserPermission.GetUserInformation(GlobalVariables.GlobalUserInformation.UserID, this.ListLogoMaster.EntryDate);

            if (this.UserOrganization.UserID <= 0 || this.UserOrganization.UserOrganizationID <= 0) exceptionTable.AddException(new string[] { GlobalVariables.stringFieldRequired, "User information" });

            if (exceptionTable.Table.Rows.Count <= 0 && this.IsValid) return true; else throw new CustomException("Save validate", exceptionTable.Table);
        }


        private bool SaveMaster(ref int logoID)
        {
            ListLogoDTS.ListLogoDataTable masterDataTable;
            ListLogoDTS.ListLogoRow masterRow;

            if (this.listLogoMaster.LogoID <= 0) //Add
            {
                masterDataTable = new ListLogoDTS.ListLogoDataTable();
                masterRow = masterDataTable.NewListLogoRow();
            }
            else //Edit
            {
                if (!this.SaveUndo(listLogoMaster.LogoID)) throw new System.ArgumentException("Insufficient save", "Save undo");
                masterDataTable = this.MasterTableAdapter.GetData(listLogoMaster.LogoID);
                if (masterDataTable.Count > 0) masterRow = masterDataTable[0]; else throw new System.ArgumentException("Insufficient save", "Get for edit");
            }

            masterRow.EntryDate = DateTime.Now;

            masterRow.Description = this.listLogoMaster.Description;
            masterRow.LogoGenerator = this.listLogoMaster.LogoGenerator;
            masterRow.Remarks = this.listLogoMaster.Remarks;

            masterRow.UserID = this.UserOrganization.UserID;
            masterRow.UserOrganizationID = this.UserOrganization.UserOrganizationID;

            masterRow.EntryStatusID = this.listLogoMaster.LogoID <= 0 || (int)masterDataTable[0]["EntryStatusID"] == (int)GlobalEnum.EntryStatusID.IsNew ? (int)GlobalEnum.EntryStatusID.IsNew : (int)GlobalEnum.EntryStatusID.IsEdited; 

            if (this.listLogoMaster.LogoID <= 0) masterDataTable.AddListLogoRow(masterRow);

            int rowsAffected = this.MasterTableAdapter.Update(masterRow);

            logoID = masterRow.LogoID;

            return rowsAffected == 1;

        }


        private bool SaveDetail(int logoID)
        {
            int serialID = 0; int rowsAffected = 0;


            #region <Save collection>

            serialID = 0;

            ListLogoDTS.ListLogoDetailDataTable detailDataTable = new ListLogoDTS.ListLogoDetailDataTable();

            foreach (ListLogoDetail listLogoDetail in this.ListLogoDetailList)
            {
                ListLogoDTS.ListLogoDetailRow detailRow = detailDataTable.NewListLogoDetailRow();

                detailRow.LogoID = logoID;
                detailRow.SerialID = ++serialID;

                detailRow.CommonID = listLogoDetail.CommonID;
                detailRow.CommonValue = listLogoDetail.CommonValue;

                detailRow.Remarks = listLogoDetail.Remarks;

                detailDataTable.AddListLogoDetailRow(detailRow);
            }

            rowsAffected = this.DetailTableAdapter.Update(detailDataTable);
            if (rowsAffected != this.listLogoDetailList.Count) throw new System.ArgumentException("Insufficient save", "Save detail");


            #endregion <Save collection>


            return true;
        }


        private bool SaveUndo(int logoID)
        {
            this.DetailTableAdapter.Delete(logoID);

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
            queryString = queryString + "       SELECT      LogoID, Description, LogoGenerator, EntryDate, Remarks " + "\r\n";
            queryString = queryString + "       FROM        ListLogo " + "\r\n";
            queryString = queryString + "       ORDER BY    Description " + "\r\n";

            SQLDatabase.CreateStoredProcedure("ListLogoListing", queryString);



            queryString = "     @LogoID Int " + "\r\n";
            queryString = queryString + " WITH ENCRYPTION " + "\r\n";
            queryString = queryString + " AS " + "\r\n";
            queryString = queryString + "       SELECT      LogoID, Description, LogoGenerator, EntryDate, Remarks, UserID, UserOrganizationID, EntryStatusID " + "\r\n";
            queryString = queryString + "       FROM        ListLogo " + "\r\n";
            queryString = queryString + "       WHERE       LogoID = @LogoID " + "\r\n";

            SQLDatabase.CreateStoredProcedure("ListLogoSelect", queryString);


            queryString = "     @Description nvarchar(100),	@LogoGenerator nvarchar(100), @EntryDate datetime, @Remarks nchar(10), @UserID int, @UserOrganizationID int, @EntryStatusID int " + "\r\n";
            queryString = queryString + " WITH ENCRYPTION " + "\r\n";
            queryString = queryString + " AS " + "\r\n";
            queryString = queryString + "       INSERT INTO ListLogo ([Description], [LogoGenerator], [EntryDate], [Remarks], [UserID], [UserOrganizationID], [EntryStatusID]) VALUES (@Description, @LogoGenerator, @EntryDate, @Remarks, @UserID, @UserOrganizationID, @EntryStatusID) " + "\r\n";
            queryString = queryString + "       SELECT      LogoID, Description, LogoGenerator, EntryDate, Remarks, UserID, UserOrganizationID, EntryStatusID FROM ListLogo WHERE LogoID = SCOPE_IDENTITY() " + "\r\n";

            SQLDatabase.CreateStoredProcedure("ListLogoInsert", queryString);


            queryString = "     @LogoID int, @Description nvarchar(100), @LogoGenerator nvarchar(100), @EntryDate datetime, @Remarks nchar(10), @UserID int, @UserOrganizationID int, @EntryStatusID int " + "\r\n";
            queryString = queryString + " WITH ENCRYPTION " + "\r\n";
            queryString = queryString + " AS " + "\r\n";
            queryString = queryString + "       UPDATE      ListLogo SET [Description] = @Description, [LogoGenerator] = @LogoGenerator, [EntryDate] = @EntryDate, [Remarks] = @Remarks, [UserID] = @UserID, [UserOrganizationID] = @UserOrganizationID, [EntryStatusID] = @EntryStatusID WHERE LogoID = @LogoID " + "\r\n";
            queryString = queryString + "       SELECT      LogoID, Description, LogoGenerator, EntryDate, Remarks, UserID, UserOrganizationID, EntryStatusID FROM ListLogo WHERE LogoID = @LogoID " + "\r\n";

            SQLDatabase.CreateStoredProcedure("ListLogoUpdate", queryString);


            queryString = " @LogoID int ";
            queryString = queryString + " WITH ENCRYPTION " + "\r\n";
            queryString = queryString + " AS " + "\r\n";
            queryString = queryString + "       DELETE FROM ListLogo WHERE LogoID = @LogoID " + "\r\n";
            SQLDatabase.CreateStoredProcedure("ListLogoDelete", queryString);






            queryString = " @LogoID int " + "\r\n";
            queryString = queryString + " WITH ENCRYPTION " + "\r\n";
            queryString = queryString + " AS " + "\r\n";
            queryString = queryString + "       SELECT      LogoID, SerialID, CommonID, CommonValue, Remarks " + "\r\n";
            queryString = queryString + "       FROM        ListLogoDetail " + "\r\n";
            queryString = queryString + "       WHERE       LogoID = @LogoID " + "\r\n";

            SQLDatabase.CreateStoredProcedure("ListLogoDetailSelect", queryString);


            queryString = " @LogoID int, @SerialID int,	@CommonID int, @CommonValue float, @Remarks nvarchar(100) " + "\r\n";
            queryString = queryString + " WITH ENCRYPTION " + "\r\n";
            queryString = queryString + " AS " + "\r\n";
            queryString = queryString + "       INSERT INTO ListLogoDetail (LogoID, SerialID, CommonID, CommonValue, Remarks) VALUES (@LogoID, @SerialID, @CommonID, @CommonValue, @Remarks) " + "\r\n";
            queryString = queryString + "       SELECT      LogoID, SerialID, CommonID, CommonValue, Remarks FROM ListLogoDetail WHERE (LogoID = @LogoID) AND (SerialID = @SerialID) " + "\r\n";

            SQLDatabase.CreateStoredProcedure("ListLogoDetailInsert", queryString);



            queryString = " @LogoID int, @SerialID int,	@CommonID int, @CommonValue float, @Remarks nvarchar(100) " + "\r\n";
            queryString = queryString + " WITH ENCRYPTION " + "\r\n";
            queryString = queryString + " AS " + "\r\n";
            queryString = queryString + "       UPDATE      ListLogoDetail SET CommonID = @CommonID, CommonValue = @CommonValue, Remarks = @Remarks WHERE LogoID = @LogoID AND SerialID = @SerialID " + "\r\n";
            queryString = queryString + "       SELECT      LogoID, SerialID, CommonID, CommonValue, Remarks FROM ListLogoDetail WHERE LogoID = @LogoID AND SerialID = @SerialID " + "\r\n";

            SQLDatabase.CreateStoredProcedure("ListLogoDetailUpdate", queryString);



            queryString = " @LogoID int " + "\r\n";
            queryString = queryString + " WITH ENCRYPTION " + "\r\n";
            queryString = queryString + " AS " + "\r\n";
            queryString = queryString + "       DELETE FROM ListLogoDetail WHERE LogoID = @LogoID " + "\r\n";
            SQLDatabase.CreateStoredProcedure("ListLogoDetailDelete", queryString);



            /// <summary>
            /// Check for editable
            /// </summary>
            queryArray = new string[1];
            queryArray[0] = "   SELECT      TOP 1 LogoID FROM DataMessageMaster WHERE LogoID = @FindIdentityID ";

            SQLDatabase.CreateProcedureToCheckExisting("ListLogoEditable", queryArray);

           
        }


    }
}
