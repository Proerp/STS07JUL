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
using DataAccessLayer.MetaDataList.ListFactoryDTSTableAdapters;

namespace BusinessLogicLayer.MetaDataList
{
    public class ListFactoryBLL : NotifyPropertyChangeObject
    {
        public GlobalEnum.TaskID TaskID { get { return GlobalEnum.TaskID.ListFactory; } }

        private UserInformation userOrganization;

        private ListFactoryMaster listFactoryMaster;

        private BindingList<ListFactoryDetail> listFactoryDetailList;



        public ListFactoryBLL()
        {
            try
            {
                if (GlobalVariables.shouldRestoreProcedure) RestoreProcedure();


                userOrganization = new UserInformation();

                listFactoryMaster = new ListFactoryMaster();

                this.listFactoryDetailList = new BindingList<ListFactoryDetail>();

                GlobalDefaultValue.Apply(this);


                this.ListFactoryMaster.PropertyChanged += new PropertyChangedEventHandler(ListFactoryMaster_PropertyChanged);

                this.ListFactoryDetailList.ListChanged += new ListChangedEventHandler(ListFactoryDetailList_ListChanged);

            }
            catch (Exception exception)
            {
                throw exception;
            }
        }

        private void ListFactoryMaster_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            this.SetDirty();
        }

        private void ListFactoryDetailList_ListChanged(object sender, ListChangedEventArgs e)
        {
            this.SetDirty();
        }




        #region <Adapter>

        private ListFactoryListingTableAdapter listingTableAdapter;
        protected ListFactoryListingTableAdapter ListingTableAdapter
        {
            get
            {
                if (listingTableAdapter == null) listingTableAdapter = new ListFactoryListingTableAdapter();
                return listingTableAdapter;
            }
        }

        private ListFactoryTableAdapter masterTableAdapter;
        protected ListFactoryTableAdapter MasterTableAdapter
        {
            get
            {
                if (masterTableAdapter == null) masterTableAdapter = new ListFactoryTableAdapter();
                return masterTableAdapter;
            }
        }

        private ListFactoryDetailTableAdapter detailTableAdapter;
        protected ListFactoryDetailTableAdapter DetailTableAdapter
        {
            get
            {
                if (detailTableAdapter == null) detailTableAdapter = new ListFactoryDetailTableAdapter();
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

        public ListFactoryDTS.ListFactoryListingDataTable ListFactoryListing()
        {
            return this.ListingTableAdapter.GetData();
        }

        public ListFactoryMaster ListFactoryMaster
        {
            get { return this.listFactoryMaster; }
        }

        public BindingList<ListFactoryDetail> ListFactoryDetailList
        {
            get { return this.listFactoryDetailList; }
        }


        #endregion <Storage>

        #region Properties

        #region <Primary Key>

        public int FactoryID   //Primary Key
        {
            get { return this.ListFactoryMaster.FactoryID; }
            private set
            {
                if (this.ListFactoryMaster.FactoryID != value)
                {
                    this.StopTracking();

                    this.ListFactoryMaster.FactoryID = value;

                    this.ListFactoryGetMaster();
                    this.ListFactoryGetDetail();

                    this.StartTracking();
                    this.Reset();
                }

            }
        }

        #endregion <Primary Key>

        private void ListFactoryGetMaster()
        {
            if (this.FactoryID > 0)
            {
                ListFactoryDTS.ListFactoryDataTable masterDataTable = this.MasterTableAdapter.GetData(this.FactoryID);

                if (masterDataTable.Count > 0)
                {
                    this.ListFactoryMaster.StopTracking();

                    this.ListFactoryMaster.EntryDate = masterDataTable[0].EntryDate;

                    this.ListFactoryMaster.Description = masterDataTable[0].Description;
                    this.ListFactoryMaster.LogoGenerator = masterDataTable[0].LogoGenerator;
                    this.ListFactoryMaster.Remarks = masterDataTable[0].Remarks;

                    this.ListFactoryMaster.StartTracking();

                    this.ListFactoryMaster.Reset();

                    this.UserOrganization.UserID = masterDataTable[0].UserID;
                    this.UserOrganization.UserOrganizationID = masterDataTable[0].UserOrganizationID;
                }
                else throw new System.ArgumentException("Insufficient get data");
            }
            else
            {
                GlobalDefaultValue.Apply(this.ListFactoryMaster);
                this.ListFactoryMaster.EntryDate = DateTime.Today;
                this.ListFactoryMaster.Reset();
            }
        }


        private void ListFactoryGetDetail()
        {
            this.listFactoryDetailList.RaiseListChangedEvents = false;
            this.listFactoryDetailList.Clear();
            if (this.FactoryID >= 0)
            {

                ListFactoryDTS.ListFactoryDetailDataTable detailDataTable = this.DetailTableAdapter.GetData(this.FactoryID);

                if (detailDataTable.Count > 0)
                {
                    foreach (ListFactoryDTS.ListFactoryDetailRow detailRow in detailDataTable.Rows)
                    {
                        this.listFactoryDetailList.Add(new ListFactoryDetail(detailRow.CommonID, detailRow.CommonValue, detailRow.Remarks));
                    }
                }
            }
            this.listFactoryDetailList.RaiseListChangedEvents = true;
            this.listFactoryDetailList.ResetBindings();
        }

        #endregion Properties





        #region Object Permission

        public override bool IsValid
        {
            get
            {
                List<ListFactoryDetail> inValidListFactoryDetail = this.ListFactoryDetailList.Where(listFactoryDetail => !listFactoryDetail.IsValid).ToList();
                return this.ListFactoryMaster.IsValid && inValidListFactoryDetail.Count == 0;
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

                    return GlobalUserPermission.GetEditable("ListFactoryEditable", this.FactoryID);
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
                    if (!GlobalUserPermission.GetUserEditable(GlobalVariables.GlobalUserInformation.UserID, GlobalEnum.TaskID.ListFactoryVerifiable, this.UserOrganization.UserOrganizationID)) return false;

                    return GlobalUserPermission.GetEditable("ListFactoryEditable", this.FactoryID);
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
                    if (!GlobalUserPermission.GetUserEditable(GlobalVariables.GlobalUserInformation.UserID, GlobalEnum.TaskID.ListFactoryUnverifiable, this.UserOrganization.UserOrganizationID)) return false;

                    return GlobalUserPermission.GetEditable("ListFactoryEditable", this.FactoryID);
                }
                catch
                {
                    return false;
                }
            }
        }


        #endregion Object Permission






        public void Fill(int factoryID)
        {
            if (this.FactoryID == factoryID) this.FactoryID = -1; //Enforce to reload
            this.FactoryID = factoryID;
        }

        public void New()
        {
            if (this.FactoryID == 0) this.FactoryID = -1;
            this.FactoryID = 0;
        }

        public void Edit()
        {

        }

        #region Save & Delete Method


        public bool Save()
        {
            int factoryID = 0;

            try
            {

                if (!this.SaveValidate()) throw new System.ArgumentException("Insufficient save", "Save validate");

                using (TransactionScope transactionScope = new TransactionScope())
                {
                    if (this.ReadOnly) throw new System.ArgumentException("Insufficient save", "Uneditable");

                    if (!this.SaveMaster(ref factoryID)) throw new System.ArgumentException("Insufficient save", "Save master");

                    if (!this.SaveDetail(factoryID)) throw new System.ArgumentException("Insufficient save", "Save detail");

                    transactionScope.Complete();
                }

                this.Fill(factoryID);
                return true;
            }
            catch (System.Exception exception)
            {
                throw exception;
            }
        }


        public bool Delete()
        {
            if (this.listFactoryMaster.FactoryID <= 0) return false;

            try
            {

                if (!this.SaveValidate()) throw new System.ArgumentException("Insufficient delete", "Delete validate");

                using (TransactionScope transactionScope = new TransactionScope())
                {
                    if (!this.Editable) throw new System.ArgumentException("Insufficient delete", "Uneditable");

                    if (!this.SaveUndo(this.listFactoryMaster.FactoryID)) throw new System.ArgumentException("Insufficient delete", "Delete detail");

                    if (this.MasterTableAdapter.Delete(this.listFactoryMaster.FactoryID) != 1) throw new System.ArgumentException("Insufficient delete", "Delete master");

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

            this.UserOrganization = GlobalUserPermission.GetUserInformation(GlobalVariables.GlobalUserInformation.UserID, this.ListFactoryMaster.EntryDate);

            if (this.UserOrganization.UserID <= 0 || this.UserOrganization.UserOrganizationID <= 0) exceptionTable.AddException(new string[] { GlobalVariables.stringFieldRequired, "User information" });

            if (exceptionTable.Table.Rows.Count <= 0 && this.IsValid) return true; else throw new CustomException("Save validate", exceptionTable.Table);
        }


        private bool SaveMaster(ref int factoryID)
        {
            ListFactoryDTS.ListFactoryDataTable masterDataTable;
            ListFactoryDTS.ListFactoryRow masterRow;

            if (this.listFactoryMaster.FactoryID <= 0) //Add
            {
                masterDataTable = new ListFactoryDTS.ListFactoryDataTable();
                masterRow = masterDataTable.NewListFactoryRow();
            }
            else //Edit
            {
                if (!this.SaveUndo(listFactoryMaster.FactoryID)) throw new System.ArgumentException("Insufficient save", "Save undo");
                masterDataTable = this.MasterTableAdapter.GetData(listFactoryMaster.FactoryID);
                if (masterDataTable.Count > 0) masterRow = masterDataTable[0]; else throw new System.ArgumentException("Insufficient save", "Get for edit");
            }

            masterRow.EntryDate = DateTime.Now;

            masterRow.Description = this.listFactoryMaster.Description;
            masterRow.LogoGenerator = this.listFactoryMaster.LogoGenerator;
            masterRow.Remarks = this.listFactoryMaster.Remarks;

            masterRow.UserID = this.UserOrganization.UserID;
            masterRow.UserOrganizationID = this.UserOrganization.UserOrganizationID;

            masterRow.EntryStatusID = this.listFactoryMaster.FactoryID <= 0 || (int)masterDataTable[0]["EntryStatusID"] == (int)GlobalEnum.EntryStatusID.IsNew ? (int)GlobalEnum.EntryStatusID.IsNew : (int)GlobalEnum.EntryStatusID.IsEdited; 

            if (this.listFactoryMaster.FactoryID <= 0) masterDataTable.AddListFactoryRow(masterRow);

            int rowsAffected = this.MasterTableAdapter.Update(masterRow);

            factoryID = masterRow.FactoryID;

            return rowsAffected == 1;

        }


        private bool SaveDetail(int factoryID)
        {
            int serialID = 0; int rowsAffected = 0;


            #region <Save collection>

            serialID = 0;

            ListFactoryDTS.ListFactoryDetailDataTable detailDataTable = new ListFactoryDTS.ListFactoryDetailDataTable();

            foreach (ListFactoryDetail listFactoryDetail in this.ListFactoryDetailList)
            {
                ListFactoryDTS.ListFactoryDetailRow detailRow = detailDataTable.NewListFactoryDetailRow();

                detailRow.FactoryID = factoryID;
                detailRow.SerialID = ++serialID;

                detailRow.CommonID = listFactoryDetail.CommonID;
                detailRow.CommonValue = listFactoryDetail.CommonValue;

                detailRow.Remarks = listFactoryDetail.Remarks;

                detailDataTable.AddListFactoryDetailRow(detailRow);
            }

            rowsAffected = this.DetailTableAdapter.Update(detailDataTable);
            if (rowsAffected != this.listFactoryDetailList.Count) throw new System.ArgumentException("Insufficient save", "Save detail");


            #endregion <Save collection>


            return true;
        }


        private bool SaveUndo(int factoryID)
        {
            this.DetailTableAdapter.Delete(factoryID);

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
            queryString = queryString + "       SELECT      FactoryID, Description, LogoGenerator, EntryDate, Remarks " + "\r\n";
            queryString = queryString + "       FROM        ListFactory " + "\r\n";
            queryString = queryString + "       ORDER BY    Description " + "\r\n";

            SQLDatabase.CreateStoredProcedure("ListFactoryListing", queryString);



            queryString = "     @FactoryID Int " + "\r\n";
            queryString = queryString + " WITH ENCRYPTION " + "\r\n";
            queryString = queryString + " AS " + "\r\n";
            queryString = queryString + "       SELECT      FactoryID, Description, LogoGenerator, EntryDate, Remarks, UserID, UserOrganizationID, EntryStatusID " + "\r\n";
            queryString = queryString + "       FROM        ListFactory " + "\r\n";
            queryString = queryString + "       WHERE       FactoryID = @FactoryID " + "\r\n";

            SQLDatabase.CreateStoredProcedure("ListFactorySelect", queryString);


            queryString = "     @Description nvarchar(100),	@LogoGenerator nvarchar(100), @EntryDate datetime, @Remarks nchar(10), @UserID int, @UserOrganizationID int, @EntryStatusID int " + "\r\n";
            queryString = queryString + " WITH ENCRYPTION " + "\r\n";
            queryString = queryString + " AS " + "\r\n";
            queryString = queryString + "       INSERT INTO ListFactory ([Description], [LogoGenerator], [EntryDate], [Remarks], [UserID], [UserOrganizationID], [EntryStatusID]) VALUES (@Description, @LogoGenerator, @EntryDate, @Remarks, @UserID, @UserOrganizationID, @EntryStatusID) " + "\r\n";
            queryString = queryString + "       SELECT      FactoryID, Description, LogoGenerator, EntryDate, Remarks, UserID, UserOrganizationID, EntryStatusID FROM ListFactory WHERE FactoryID = SCOPE_IDENTITY() " + "\r\n";

            SQLDatabase.CreateStoredProcedure("ListFactoryInsert", queryString);


            queryString = "     @FactoryID int, @Description nvarchar(100), @LogoGenerator nvarchar(100), @EntryDate datetime, @Remarks nchar(10), @UserID int, @UserOrganizationID int, @EntryStatusID int " + "\r\n";
            queryString = queryString + " WITH ENCRYPTION " + "\r\n";
            queryString = queryString + " AS " + "\r\n";
            queryString = queryString + "       UPDATE      ListFactory SET [Description] = @Description, [LogoGenerator] = @LogoGenerator, [EntryDate] = @EntryDate, [Remarks] = @Remarks, [UserID] = @UserID, [UserOrganizationID] = @UserOrganizationID, [EntryStatusID] = @EntryStatusID WHERE FactoryID = @FactoryID " + "\r\n";
            queryString = queryString + "       SELECT      FactoryID, Description, LogoGenerator, EntryDate, Remarks, UserID, UserOrganizationID, EntryStatusID FROM ListFactory WHERE FactoryID = @FactoryID " + "\r\n";

            SQLDatabase.CreateStoredProcedure("ListFactoryUpdate", queryString);


            queryString = " @FactoryID int ";
            queryString = queryString + " WITH ENCRYPTION " + "\r\n";
            queryString = queryString + " AS " + "\r\n";
            queryString = queryString + "       DELETE FROM ListFactory WHERE FactoryID = @FactoryID " + "\r\n";
            SQLDatabase.CreateStoredProcedure("ListFactoryDelete", queryString);






            queryString = " @FactoryID int " + "\r\n";
            queryString = queryString + " WITH ENCRYPTION " + "\r\n";
            queryString = queryString + " AS " + "\r\n";
            queryString = queryString + "       SELECT      FactoryID, SerialID, CommonID, CommonValue, Remarks " + "\r\n";
            queryString = queryString + "       FROM        ListFactoryDetail " + "\r\n";
            queryString = queryString + "       WHERE       FactoryID = @FactoryID " + "\r\n";

            SQLDatabase.CreateStoredProcedure("ListFactoryDetailSelect", queryString);


            queryString = " @FactoryID int, @SerialID int,	@CommonID int, @CommonValue float, @Remarks nvarchar(100) " + "\r\n";
            queryString = queryString + " WITH ENCRYPTION " + "\r\n";
            queryString = queryString + " AS " + "\r\n";
            queryString = queryString + "       INSERT INTO ListFactoryDetail (FactoryID, SerialID, CommonID, CommonValue, Remarks) VALUES (@FactoryID, @SerialID, @CommonID, @CommonValue, @Remarks) " + "\r\n";
            queryString = queryString + "       SELECT      FactoryID, SerialID, CommonID, CommonValue, Remarks FROM ListFactoryDetail WHERE (FactoryID = @FactoryID) AND (SerialID = @SerialID) " + "\r\n";

            SQLDatabase.CreateStoredProcedure("ListFactoryDetailInsert", queryString);



            queryString = " @FactoryID int, @SerialID int,	@CommonID int, @CommonValue float, @Remarks nvarchar(100) " + "\r\n";
            queryString = queryString + " WITH ENCRYPTION " + "\r\n";
            queryString = queryString + " AS " + "\r\n";
            queryString = queryString + "       UPDATE      ListFactoryDetail SET CommonID = @CommonID, CommonValue = @CommonValue, Remarks = @Remarks WHERE FactoryID = @FactoryID AND SerialID = @SerialID " + "\r\n";
            queryString = queryString + "       SELECT      FactoryID, SerialID, CommonID, CommonValue, Remarks FROM ListFactoryDetail WHERE FactoryID = @FactoryID AND SerialID = @SerialID " + "\r\n";

            SQLDatabase.CreateStoredProcedure("ListFactoryDetailUpdate", queryString);



            queryString = " @FactoryID int " + "\r\n";
            queryString = queryString + " WITH ENCRYPTION " + "\r\n";
            queryString = queryString + " AS " + "\r\n";
            queryString = queryString + "       DELETE FROM ListFactoryDetail WHERE FactoryID = @FactoryID " + "\r\n";
            SQLDatabase.CreateStoredProcedure("ListFactoryDetailDelete", queryString);



            /// <summary>
            /// Check for editable
            /// </summary>
            queryArray = new string[1];
            queryArray[0] = "   SELECT      TOP 1 FactoryID FROM DataMessageMaster WHERE FactoryID = @FindIdentityID ";

            SQLDatabase.CreateProcedureToCheckExisting("ListFactoryEditable", queryArray);

        }
    }
}
