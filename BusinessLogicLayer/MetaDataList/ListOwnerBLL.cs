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
using DataAccessLayer.MetaDataList.ListOwnerDTSTableAdapters;

namespace BusinessLogicLayer.MetaDataList
{
    public class ListOwnerBLL : NotifyPropertyChangeObject
    {
        public GlobalEnum.TaskID TaskID { get { return GlobalEnum.TaskID.ListOwner; } }

        private UserInformation userOrganization;

        private ListOwnerMaster listOwnerMaster;

        private BindingList<ListOwnerDetail> listOwnerDetailList;



        public ListOwnerBLL()
        {
            try
            {
                if (GlobalVariables.shouldRestoreProcedure) RestoreProcedure();


                userOrganization = new UserInformation();

                listOwnerMaster = new ListOwnerMaster();

                this.listOwnerDetailList = new BindingList<ListOwnerDetail>();

                GlobalDefaultValue.Apply(this);


                this.ListOwnerMaster.PropertyChanged += new PropertyChangedEventHandler(ListOwnerMaster_PropertyChanged);

                this.ListOwnerDetailList.ListChanged += new ListChangedEventHandler(ListOwnerDetailList_ListChanged);

            }
            catch (Exception exception)
            {
                throw exception;
            }
        }

        private void ListOwnerMaster_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            this.SetDirty();
        }

        private void ListOwnerDetailList_ListChanged(object sender, ListChangedEventArgs e)
        {
            this.SetDirty();
        }




        #region <Adapter>

        private ListOwnerListingTableAdapter listingTableAdapter;
        protected ListOwnerListingTableAdapter ListingTableAdapter
        {
            get
            {
                if (listingTableAdapter == null) listingTableAdapter = new ListOwnerListingTableAdapter();
                return listingTableAdapter;
            }
        }

        private ListOwnerTableAdapter masterTableAdapter;
        protected ListOwnerTableAdapter MasterTableAdapter
        {
            get
            {
                if (masterTableAdapter == null) masterTableAdapter = new ListOwnerTableAdapter();
                return masterTableAdapter;
            }
        }

        private ListOwnerDetailTableAdapter detailTableAdapter;
        protected ListOwnerDetailTableAdapter DetailTableAdapter
        {
            get
            {
                if (detailTableAdapter == null) detailTableAdapter = new ListOwnerDetailTableAdapter();
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

        public ListOwnerDTS.ListOwnerListingDataTable ListOwnerListing()
        {
            return this.ListingTableAdapter.GetData();
        }

        public ListOwnerMaster ListOwnerMaster
        {
            get { return this.listOwnerMaster; }
        }

        public BindingList<ListOwnerDetail> ListOwnerDetailList
        {
            get { return this.listOwnerDetailList; }
        }


        #endregion <Storage>

        #region Properties

        #region <Primary Key>

        public int OwnerID   //Primary Key
        {
            get { return this.ListOwnerMaster.OwnerID; }
            private set
            {
                if (this.ListOwnerMaster.OwnerID != value)
                {
                    this.StopTracking();

                    this.ListOwnerMaster.OwnerID = value;

                    this.ListOwnerGetMaster();
                    this.ListOwnerGetDetail();

                    this.StartTracking();
                    this.Reset();
                }

            }
        }

        #endregion <Primary Key>

        private void ListOwnerGetMaster()
        {
            if (this.OwnerID > 0)
            {
                ListOwnerDTS.ListOwnerDataTable masterDataTable = this.MasterTableAdapter.GetData(this.OwnerID);

                if (masterDataTable.Count > 0)
                {
                    this.ListOwnerMaster.StopTracking();

                    this.ListOwnerMaster.EntryDate = masterDataTable[0].EntryDate;

                    this.ListOwnerMaster.Description = masterDataTable[0].Description;
                    this.ListOwnerMaster.LogoGenerator = masterDataTable[0].LogoGenerator;
                    this.ListOwnerMaster.Remarks = masterDataTable[0].Remarks;

                    this.ListOwnerMaster.StartTracking();

                    this.ListOwnerMaster.Reset();

                    this.UserOrganization.UserID = masterDataTable[0].UserID;
                    this.UserOrganization.UserOrganizationID = masterDataTable[0].UserOrganizationID;
                }
                else throw new System.ArgumentException("Insufficient get data");
            }
            else
            {
                GlobalDefaultValue.Apply(this.ListOwnerMaster);
                this.ListOwnerMaster.EntryDate = DateTime.Today;
                this.ListOwnerMaster.Reset();
            }
        }


        private void ListOwnerGetDetail()
        {
            this.listOwnerDetailList.RaiseListChangedEvents = false;
            this.listOwnerDetailList.Clear();
            if (this.OwnerID >= 0)
            {

                ListOwnerDTS.ListOwnerDetailDataTable detailDataTable = this.DetailTableAdapter.GetData(this.OwnerID);

                if (detailDataTable.Count > 0)
                {
                    foreach (ListOwnerDTS.ListOwnerDetailRow detailRow in detailDataTable.Rows)
                    {
                        this.listOwnerDetailList.Add(new ListOwnerDetail(detailRow.CommonID, detailRow.CommonValue, detailRow.Remarks));
                    }
                }
            }
            this.listOwnerDetailList.RaiseListChangedEvents = true;
            this.listOwnerDetailList.ResetBindings();
        }

        #endregion Properties





        #region Object Permission

        public override bool IsValid
        {
            get
            {
                List<ListOwnerDetail> inValidListOwnerDetail = this.ListOwnerDetailList.Where(listOwnerDetail => !listOwnerDetail.IsValid).ToList();
                return this.ListOwnerMaster.IsValid && inValidListOwnerDetail.Count == 0;
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

                    return GlobalUserPermission.GetEditable("ListOwnerEditable", this.OwnerID);
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
                    if (!GlobalUserPermission.GetUserEditable(GlobalVariables.GlobalUserInformation.UserID, GlobalEnum.TaskID.ListOwnerVerifiable, this.UserOrganization.UserOrganizationID)) return false;

                    return GlobalUserPermission.GetEditable("ListOwnerEditable", this.OwnerID);
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
                    if (!GlobalUserPermission.GetUserEditable(GlobalVariables.GlobalUserInformation.UserID, GlobalEnum.TaskID.ListOwnerUnverifiable, this.UserOrganization.UserOrganizationID)) return false;

                    return GlobalUserPermission.GetEditable("ListOwnerEditable", this.OwnerID);
                }
                catch
                {
                    return false;
                }
            }
        }


        #endregion Object Permission






        public void Fill(int ownerID)
        {
            if (this.OwnerID == ownerID) this.OwnerID = -1; //Enforce to reload
            this.OwnerID = ownerID;
        }

        public void New()
        {
            if (this.OwnerID == 0) this.OwnerID = -1;
            this.OwnerID = 0;
        }

        public void Edit()
        {

        }

        #region Save & Delete Method


        public bool Save()
        {
            int ownerID = 0;

            try
            {

                if (!this.SaveValidate()) throw new System.ArgumentException("Insufficient save", "Save validate");

                using (TransactionScope transactionScope = new TransactionScope())
                {
                    if (this.ReadOnly) throw new System.ArgumentException("Insufficient save", "Uneditable");

                    if (!this.SaveMaster(ref ownerID)) throw new System.ArgumentException("Insufficient save", "Save master");

                    if (!this.SaveDetail(ownerID)) throw new System.ArgumentException("Insufficient save", "Save detail");

                    transactionScope.Complete();
                }

                this.Fill(ownerID);
                return true;
            }
            catch (System.Exception exception)
            {
                throw exception;
            }
        }


        public bool Delete()
        {
            if (this.listOwnerMaster.OwnerID <= 0) return false;

            try
            {

                if (!this.SaveValidate()) throw new System.ArgumentException("Insufficient delete", "Delete validate");

                using (TransactionScope transactionScope = new TransactionScope())
                {
                    if (!this.Editable) throw new System.ArgumentException("Insufficient delete", "Uneditable");

                    if (!this.SaveUndo(this.listOwnerMaster.OwnerID)) throw new System.ArgumentException("Insufficient delete", "Delete detail");

                    if (this.MasterTableAdapter.Delete(this.listOwnerMaster.OwnerID) != 1) throw new System.ArgumentException("Insufficient delete", "Delete master");

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

            this.UserOrganization = GlobalUserPermission.GetUserInformation(GlobalVariables.GlobalUserInformation.UserID, this.ListOwnerMaster.EntryDate);

            if (this.UserOrganization.UserID <= 0 || this.UserOrganization.UserOrganizationID <= 0) exceptionTable.AddException(new string[] { GlobalVariables.stringFieldRequired, "User information" });

            if (exceptionTable.Table.Rows.Count <= 0 && this.IsValid) return true; else throw new CustomException("Save validate", exceptionTable.Table);
        }


        private bool SaveMaster(ref int ownerID)
        {
            ListOwnerDTS.ListOwnerDataTable masterDataTable;
            ListOwnerDTS.ListOwnerRow masterRow;

            if (this.listOwnerMaster.OwnerID <= 0) //Add
            {
                masterDataTable = new ListOwnerDTS.ListOwnerDataTable();
                masterRow = masterDataTable.NewListOwnerRow();
            }
            else //Edit
            {
                if (!this.SaveUndo(listOwnerMaster.OwnerID)) throw new System.ArgumentException("Insufficient save", "Save undo");
                masterDataTable = this.MasterTableAdapter.GetData(listOwnerMaster.OwnerID);
                if (masterDataTable.Count > 0) masterRow = masterDataTable[0]; else throw new System.ArgumentException("Insufficient save", "Get for edit");
            }

            masterRow.EntryDate = DateTime.Now;

            masterRow.Description = this.listOwnerMaster.Description;
            masterRow.LogoGenerator = this.listOwnerMaster.LogoGenerator;
            masterRow.Remarks = this.listOwnerMaster.Remarks;

            masterRow.UserID = this.UserOrganization.UserID;
            masterRow.UserOrganizationID = this.UserOrganization.UserOrganizationID;

            masterRow.EntryStatusID = this.listOwnerMaster.OwnerID <= 0 || (int)masterDataTable[0]["EntryStatusID"] == (int)GlobalEnum.EntryStatusID.IsNew ? (int)GlobalEnum.EntryStatusID.IsNew : (int)GlobalEnum.EntryStatusID.IsEdited; 

            if (this.listOwnerMaster.OwnerID <= 0) masterDataTable.AddListOwnerRow(masterRow);

            int rowsAffected = this.MasterTableAdapter.Update(masterRow);

            ownerID = masterRow.OwnerID;

            return rowsAffected == 1;

        }


        private bool SaveDetail(int ownerID)
        {
            int serialID = 0; int rowsAffected = 0;


            #region <Save collection>

            serialID = 0;

            ListOwnerDTS.ListOwnerDetailDataTable detailDataTable = new ListOwnerDTS.ListOwnerDetailDataTable();

            foreach (ListOwnerDetail listOwnerDetail in this.ListOwnerDetailList)
            {
                ListOwnerDTS.ListOwnerDetailRow detailRow = detailDataTable.NewListOwnerDetailRow();

                detailRow.OwnerID = ownerID;
                detailRow.SerialID = ++serialID;

                detailRow.CommonID = listOwnerDetail.CommonID;
                detailRow.CommonValue = listOwnerDetail.CommonValue;

                detailRow.Remarks = listOwnerDetail.Remarks;

                detailDataTable.AddListOwnerDetailRow(detailRow);
            }

            rowsAffected = this.DetailTableAdapter.Update(detailDataTable);
            if (rowsAffected != this.listOwnerDetailList.Count) throw new System.ArgumentException("Insufficient save", "Save detail");


            #endregion <Save collection>


            return true;
        }


        private bool SaveUndo(int ownerID)
        {
            this.DetailTableAdapter.Delete(ownerID);

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
            queryString = queryString + "       SELECT      OwnerID, Description, LogoGenerator, EntryDate, Remarks " + "\r\n";
            queryString = queryString + "       FROM        ListOwner " + "\r\n";
            queryString = queryString + "       ORDER BY    Description " + "\r\n";

            SQLDatabase.CreateStoredProcedure("ListOwnerListing", queryString);



            queryString = "     @OwnerID Int " + "\r\n";
            queryString = queryString + " WITH ENCRYPTION " + "\r\n";
            queryString = queryString + " AS " + "\r\n";
            queryString = queryString + "       SELECT      OwnerID, Description, LogoGenerator, EntryDate, Remarks, UserID, UserOrganizationID, EntryStatusID " + "\r\n";
            queryString = queryString + "       FROM        ListOwner " + "\r\n";
            queryString = queryString + "       WHERE       OwnerID = @OwnerID " + "\r\n";

            SQLDatabase.CreateStoredProcedure("ListOwnerSelect", queryString);


            queryString = "     @Description nvarchar(100),	@LogoGenerator nvarchar(100), @EntryDate datetime, @Remarks nchar(10), @UserID int, @UserOrganizationID int, @EntryStatusID int " + "\r\n";
            queryString = queryString + " WITH ENCRYPTION " + "\r\n";
            queryString = queryString + " AS " + "\r\n";
            queryString = queryString + "       INSERT INTO ListOwner ([Description], [LogoGenerator], [EntryDate], [Remarks], [UserID], [UserOrganizationID], [EntryStatusID]) VALUES (@Description, @LogoGenerator, @EntryDate, @Remarks, @UserID, @UserOrganizationID, @EntryStatusID) " + "\r\n";
            queryString = queryString + "       SELECT      OwnerID, Description, LogoGenerator, EntryDate, Remarks, UserID, UserOrganizationID, EntryStatusID FROM ListOwner WHERE OwnerID = SCOPE_IDENTITY() " + "\r\n";

            SQLDatabase.CreateStoredProcedure("ListOwnerInsert", queryString);


            queryString = "     @OwnerID int, @Description nvarchar(100), @LogoGenerator nvarchar(100), @EntryDate datetime, @Remarks nchar(10), @UserID int, @UserOrganizationID int, @EntryStatusID int " + "\r\n";
            queryString = queryString + " WITH ENCRYPTION " + "\r\n";
            queryString = queryString + " AS " + "\r\n";
            queryString = queryString + "       UPDATE      ListOwner SET [Description] = @Description, [LogoGenerator] = @LogoGenerator, [EntryDate] = @EntryDate, [Remarks] = @Remarks, [UserID] = @UserID, [UserOrganizationID] = @UserOrganizationID, [EntryStatusID] = @EntryStatusID WHERE OwnerID = @OwnerID " + "\r\n";
            queryString = queryString + "       SELECT      OwnerID, Description, LogoGenerator, EntryDate, Remarks, UserID, UserOrganizationID, EntryStatusID FROM ListOwner WHERE OwnerID = @OwnerID " + "\r\n";

            SQLDatabase.CreateStoredProcedure("ListOwnerUpdate", queryString);


            queryString = " @OwnerID int ";
            queryString = queryString + " WITH ENCRYPTION " + "\r\n";
            queryString = queryString + " AS " + "\r\n";
            queryString = queryString + "       DELETE FROM ListOwner WHERE OwnerID = @OwnerID " + "\r\n";
            SQLDatabase.CreateStoredProcedure("ListOwnerDelete", queryString);






            queryString = " @OwnerID int " + "\r\n";
            queryString = queryString + " WITH ENCRYPTION " + "\r\n";
            queryString = queryString + " AS " + "\r\n";
            queryString = queryString + "       SELECT      OwnerID, SerialID, CommonID, CommonValue, Remarks " + "\r\n";
            queryString = queryString + "       FROM        ListOwnerDetail " + "\r\n";
            queryString = queryString + "       WHERE       OwnerID = @OwnerID " + "\r\n";

            SQLDatabase.CreateStoredProcedure("ListOwnerDetailSelect", queryString);


            queryString = " @OwnerID int, @SerialID int,	@CommonID int, @CommonValue float, @Remarks nvarchar(100) " + "\r\n";
            queryString = queryString + " WITH ENCRYPTION " + "\r\n";
            queryString = queryString + " AS " + "\r\n";
            queryString = queryString + "       INSERT INTO ListOwnerDetail (OwnerID, SerialID, CommonID, CommonValue, Remarks) VALUES (@OwnerID, @SerialID, @CommonID, @CommonValue, @Remarks) " + "\r\n";
            queryString = queryString + "       SELECT      OwnerID, SerialID, CommonID, CommonValue, Remarks FROM ListOwnerDetail WHERE (OwnerID = @OwnerID) AND (SerialID = @SerialID) " + "\r\n";

            SQLDatabase.CreateStoredProcedure("ListOwnerDetailInsert", queryString);



            queryString = " @OwnerID int, @SerialID int,	@CommonID int, @CommonValue float, @Remarks nvarchar(100) " + "\r\n";
            queryString = queryString + " WITH ENCRYPTION " + "\r\n";
            queryString = queryString + " AS " + "\r\n";
            queryString = queryString + "       UPDATE      ListOwnerDetail SET CommonID = @CommonID, CommonValue = @CommonValue, Remarks = @Remarks WHERE OwnerID = @OwnerID AND SerialID = @SerialID " + "\r\n";
            queryString = queryString + "       SELECT      OwnerID, SerialID, CommonID, CommonValue, Remarks FROM ListOwnerDetail WHERE OwnerID = @OwnerID AND SerialID = @SerialID " + "\r\n";

            SQLDatabase.CreateStoredProcedure("ListOwnerDetailUpdate", queryString);



            queryString = " @OwnerID int " + "\r\n";
            queryString = queryString + " WITH ENCRYPTION " + "\r\n";
            queryString = queryString + " AS " + "\r\n";
            queryString = queryString + "       DELETE FROM ListOwnerDetail WHERE OwnerID = @OwnerID " + "\r\n";
            SQLDatabase.CreateStoredProcedure("ListOwnerDetailDelete", queryString);



            /// <summary>
            /// Check for editable
            /// </summary>
            queryArray = new string[1];
            queryArray[0] = "   SELECT      TOP 1 OwnerID FROM DataMessageMaster WHERE OwnerID = @FindIdentityID ";

            SQLDatabase.CreateProcedureToCheckExisting("ListOwnerEditable", queryArray);

        }
    }
}
