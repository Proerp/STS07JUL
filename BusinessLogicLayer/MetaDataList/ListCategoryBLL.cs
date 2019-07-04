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
using DataAccessLayer.MetaDataList.ListCategoryDTSTableAdapters;

namespace BusinessLogicLayer.MetaDataList
{
    public class ListCategoryBLL : NotifyPropertyChangeObject
    {
        public GlobalEnum.TaskID TaskID { get { return GlobalEnum.TaskID.ListCategory; } }

        private UserInformation userOrganization;

        private ListCategoryMaster listCategoryMaster;

        private BindingList<ListCategoryDetail> listCategoryDetailList;



        public ListCategoryBLL()
        {
            try
            {
                if (GlobalVariables.shouldRestoreProcedure) RestoreProcedure();


                userOrganization = new UserInformation();

                listCategoryMaster = new ListCategoryMaster();

                this.listCategoryDetailList = new BindingList<ListCategoryDetail>();

                GlobalDefaultValue.Apply(this);


                this.ListCategoryMaster.PropertyChanged += new PropertyChangedEventHandler(ListCategoryMaster_PropertyChanged);

                this.ListCategoryDetailList.ListChanged += new ListChangedEventHandler(ListCategoryDetailList_ListChanged);

            }
            catch (Exception exception)
            {
                throw exception;
            }
        }

        private void ListCategoryMaster_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            this.SetDirty();
        }

        private void ListCategoryDetailList_ListChanged(object sender, ListChangedEventArgs e)
        {
            this.SetDirty();
        }




        #region <Adapter>

        private ListCategoryListingTableAdapter listingTableAdapter;
        protected ListCategoryListingTableAdapter ListingTableAdapter
        {
            get
            {
                if (listingTableAdapter == null) listingTableAdapter = new ListCategoryListingTableAdapter();
                return listingTableAdapter;
            }
        }

        private ListCategoryTableAdapter masterTableAdapter;
        protected ListCategoryTableAdapter MasterTableAdapter
        {
            get
            {
                if (masterTableAdapter == null) masterTableAdapter = new ListCategoryTableAdapter();
                return masterTableAdapter;
            }
        }

        private ListCategoryDetailTableAdapter detailTableAdapter;
        protected ListCategoryDetailTableAdapter DetailTableAdapter
        {
            get
            {
                if (detailTableAdapter == null) detailTableAdapter = new ListCategoryDetailTableAdapter();
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

        public ListCategoryDTS.ListCategoryListingDataTable ListCategoryListing()
        {
            return this.ListingTableAdapter.GetData();
        }

        public ListCategoryMaster ListCategoryMaster
        {
            get { return this.listCategoryMaster; }
        }

        public BindingList<ListCategoryDetail> ListCategoryDetailList
        {
            get { return this.listCategoryDetailList; }
        }


        #endregion <Storage>

        #region Properties

        #region <Primary Key>

        public int CategoryID   //Primary Key
        {
            get { return this.ListCategoryMaster.CategoryID; }
            private set
            {
                if (this.ListCategoryMaster.CategoryID != value)
                {
                    this.StopTracking();

                    this.ListCategoryMaster.CategoryID = value;

                    this.ListCategoryGetMaster();
                    this.ListCategoryGetDetail();

                    this.StartTracking();
                    this.Reset();
                }

            }
        }

        #endregion <Primary Key>

        private void ListCategoryGetMaster()
        {
            if (this.CategoryID > 0)
            {
                ListCategoryDTS.ListCategoryDataTable masterDataTable = this.MasterTableAdapter.GetData(this.CategoryID);

                if (masterDataTable.Count > 0)
                {
                    this.ListCategoryMaster.StopTracking();

                    this.ListCategoryMaster.EntryDate = masterDataTable[0].EntryDate;

                    this.ListCategoryMaster.Description = masterDataTable[0].Description;
                    this.ListCategoryMaster.LogoGenerator = masterDataTable[0].LogoGenerator;
                    this.ListCategoryMaster.Remarks = masterDataTable[0].Remarks;

                    this.ListCategoryMaster.StartTracking();

                    this.ListCategoryMaster.Reset();

                    this.UserOrganization.UserID = masterDataTable[0].UserID;
                    this.UserOrganization.UserOrganizationID = masterDataTable[0].UserOrganizationID;
                }
                else throw new System.ArgumentException("Insufficient get data");
            }
            else
            {
                GlobalDefaultValue.Apply(this.ListCategoryMaster);
                this.ListCategoryMaster.EntryDate = DateTime.Today;
                this.ListCategoryMaster.Reset();
            }
        }


        private void ListCategoryGetDetail()
        {
            this.listCategoryDetailList.RaiseListChangedEvents = false;
            this.listCategoryDetailList.Clear();
            if (this.CategoryID >= 0)
            {

                ListCategoryDTS.ListCategoryDetailDataTable detailDataTable = this.DetailTableAdapter.GetData(this.CategoryID);

                if (detailDataTable.Count > 0)
                {
                    foreach (ListCategoryDTS.ListCategoryDetailRow detailRow in detailDataTable.Rows)
                    {
                        this.listCategoryDetailList.Add(new ListCategoryDetail(detailRow.CommonID, detailRow.CommonValue, detailRow.Remarks));
                    }
                }
            }
            this.listCategoryDetailList.RaiseListChangedEvents = true;
            this.listCategoryDetailList.ResetBindings();
        }

        #endregion Properties





        #region Object Permission

        public override bool IsValid
        {
            get
            {
                List<ListCategoryDetail> inValidListCategoryDetail = this.ListCategoryDetailList.Where(listCategoryDetail => !listCategoryDetail.IsValid).ToList();
                return this.ListCategoryMaster.IsValid && inValidListCategoryDetail.Count == 0;
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

                    return GlobalUserPermission.GetEditable("ListCategoryEditable", this.CategoryID);
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
                    if (!GlobalUserPermission.GetUserEditable(GlobalVariables.GlobalUserInformation.UserID, GlobalEnum.TaskID.ListCategoryVerifiable, this.UserOrganization.UserOrganizationID)) return false;

                    return GlobalUserPermission.GetEditable("ListCategoryEditable", this.CategoryID);
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
                    if (!GlobalUserPermission.GetUserEditable(GlobalVariables.GlobalUserInformation.UserID, GlobalEnum.TaskID.ListCategoryUnverifiable, this.UserOrganization.UserOrganizationID)) return false;

                    return GlobalUserPermission.GetEditable("ListCategoryEditable", this.CategoryID);
                }
                catch
                {
                    return false;
                }
            }
        }


        #endregion Object Permission






        public void Fill(int categoryID)
        {
            if (this.CategoryID == categoryID) this.CategoryID = -1; //Enforce to reload
            this.CategoryID = categoryID;
        }

        public void New()
        {
            if (this.CategoryID == 0) this.CategoryID = -1;
            this.CategoryID = 0;
        }

        public void Edit()
        {

        }

        #region Save & Delete Method


        public bool Save()
        {
            int categoryID = 0;

            try
            {

                if (!this.SaveValidate()) throw new System.ArgumentException("Insufficient save", "Save validate");

                using (TransactionScope transactionScope = new TransactionScope())
                {
                    if (this.ReadOnly) throw new System.ArgumentException("Insufficient save", "Uneditable");

                    if (!this.SaveMaster(ref categoryID)) throw new System.ArgumentException("Insufficient save", "Save master");

                    if (!this.SaveDetail(categoryID)) throw new System.ArgumentException("Insufficient save", "Save detail");

                    transactionScope.Complete();
                }

                this.Fill(categoryID);
                return true;
            }
            catch (System.Exception exception)
            {
                throw exception;
            }
        }


        public bool Delete()
        {
            if (this.listCategoryMaster.CategoryID <= 0) return false;

            try
            {

                if (!this.SaveValidate()) throw new System.ArgumentException("Insufficient delete", "Delete validate");

                using (TransactionScope transactionScope = new TransactionScope())
                {
                    if (!this.Editable) throw new System.ArgumentException("Insufficient delete", "Uneditable");

                    if (!this.SaveUndo(this.listCategoryMaster.CategoryID)) throw new System.ArgumentException("Insufficient delete", "Delete detail");

                    if (this.MasterTableAdapter.Delete(this.listCategoryMaster.CategoryID) != 1) throw new System.ArgumentException("Insufficient delete", "Delete master");

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

            this.UserOrganization = GlobalUserPermission.GetUserInformation(GlobalVariables.GlobalUserInformation.UserID, this.ListCategoryMaster.EntryDate);

            if (this.UserOrganization.UserID <= 0 || this.UserOrganization.UserOrganizationID <= 0) exceptionTable.AddException(new string[] { GlobalVariables.stringFieldRequired, "User information" });

            if (exceptionTable.Table.Rows.Count <= 0 && this.IsValid) return true; else throw new CustomException("Save validate", exceptionTable.Table);
        }


        private bool SaveMaster(ref int categoryID)
        {
            ListCategoryDTS.ListCategoryDataTable masterDataTable;
            ListCategoryDTS.ListCategoryRow masterRow;

            if (this.listCategoryMaster.CategoryID <= 0) //Add
            {
                masterDataTable = new ListCategoryDTS.ListCategoryDataTable();
                masterRow = masterDataTable.NewListCategoryRow();
            }
            else //Edit
            {
                if (!this.SaveUndo(listCategoryMaster.CategoryID)) throw new System.ArgumentException("Insufficient save", "Save undo");
                masterDataTable = this.MasterTableAdapter.GetData(listCategoryMaster.CategoryID);
                if (masterDataTable.Count > 0) masterRow = masterDataTable[0]; else throw new System.ArgumentException("Insufficient save", "Get for edit");
            }

            masterRow.EntryDate = DateTime.Now;

            masterRow.Description = this.listCategoryMaster.Description;
            masterRow.LogoGenerator = this.listCategoryMaster.LogoGenerator;
            masterRow.Remarks = this.listCategoryMaster.Remarks;

            masterRow.UserID = this.UserOrganization.UserID;
            masterRow.UserOrganizationID = this.UserOrganization.UserOrganizationID;

            masterRow.EntryStatusID = this.listCategoryMaster.CategoryID <= 0 || (int)masterDataTable[0]["EntryStatusID"] == (int)GlobalEnum.EntryStatusID.IsNew ? (int)GlobalEnum.EntryStatusID.IsNew : (int)GlobalEnum.EntryStatusID.IsEdited; 

            if (this.listCategoryMaster.CategoryID <= 0) masterDataTable.AddListCategoryRow(masterRow);

            int rowsAffected = this.MasterTableAdapter.Update(masterRow);

            categoryID = masterRow.CategoryID;

            return rowsAffected == 1;

        }


        private bool SaveDetail(int categoryID)
        {
            int serialID = 0; int rowsAffected = 0;


            #region <Save collection>

            serialID = 0;

            ListCategoryDTS.ListCategoryDetailDataTable detailDataTable = new ListCategoryDTS.ListCategoryDetailDataTable();

            foreach (ListCategoryDetail listCategoryDetail in this.ListCategoryDetailList)
            {
                ListCategoryDTS.ListCategoryDetailRow detailRow = detailDataTable.NewListCategoryDetailRow();

                detailRow.CategoryID = categoryID;
                detailRow.SerialID = ++serialID;

                detailRow.CommonID = listCategoryDetail.CommonID;
                detailRow.CommonValue = listCategoryDetail.CommonValue;

                detailRow.Remarks = listCategoryDetail.Remarks;

                detailDataTable.AddListCategoryDetailRow(detailRow);
            }

            rowsAffected = this.DetailTableAdapter.Update(detailDataTable);
            if (rowsAffected != this.listCategoryDetailList.Count) throw new System.ArgumentException("Insufficient save", "Save detail");


            #endregion <Save collection>


            return true;
        }


        private bool SaveUndo(int categoryID)
        {
            this.DetailTableAdapter.Delete(categoryID);

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
            queryString = queryString + "       SELECT      CategoryID, Description, LogoGenerator, EntryDate, Remarks " + "\r\n";
            queryString = queryString + "       FROM        ListCategory " + "\r\n";
            queryString = queryString + "       ORDER BY    Description " + "\r\n";

            SQLDatabase.CreateStoredProcedure("ListCategoryListing", queryString);



            queryString = "     @CategoryID Int " + "\r\n";
            queryString = queryString + " WITH ENCRYPTION " + "\r\n";
            queryString = queryString + " AS " + "\r\n";
            queryString = queryString + "       SELECT      CategoryID, Description, LogoGenerator, EntryDate, Remarks, UserID, UserOrganizationID, EntryStatusID " + "\r\n";
            queryString = queryString + "       FROM        ListCategory " + "\r\n";
            queryString = queryString + "       WHERE       CategoryID = @CategoryID " + "\r\n";

            SQLDatabase.CreateStoredProcedure("ListCategorySelect", queryString);


            queryString = "     @Description nvarchar(100),	@LogoGenerator nvarchar(100), @EntryDate datetime, @Remarks nchar(10), @UserID int, @UserOrganizationID int, @EntryStatusID int " + "\r\n";
            queryString = queryString + " WITH ENCRYPTION " + "\r\n";
            queryString = queryString + " AS " + "\r\n";
            queryString = queryString + "       INSERT INTO ListCategory ([Description], [LogoGenerator], [EntryDate], [Remarks], [UserID], [UserOrganizationID], [EntryStatusID]) VALUES (@Description, @LogoGenerator, @EntryDate, @Remarks, @UserID, @UserOrganizationID, @EntryStatusID) " + "\r\n";
            queryString = queryString + "       SELECT      CategoryID, Description, LogoGenerator, EntryDate, Remarks, UserID, UserOrganizationID, EntryStatusID FROM ListCategory WHERE CategoryID = SCOPE_IDENTITY() " + "\r\n";

            SQLDatabase.CreateStoredProcedure("ListCategoryInsert", queryString);


            queryString = "     @CategoryID int, @Description nvarchar(100), @LogoGenerator nvarchar(100), @EntryDate datetime, @Remarks nchar(10), @UserID int, @UserOrganizationID int, @EntryStatusID int " + "\r\n";
            queryString = queryString + " WITH ENCRYPTION " + "\r\n";
            queryString = queryString + " AS " + "\r\n";
            queryString = queryString + "       UPDATE      ListCategory SET [Description] = @Description, [LogoGenerator] = @LogoGenerator, [EntryDate] = @EntryDate, [Remarks] = @Remarks, [UserID] = @UserID, [UserOrganizationID] = @UserOrganizationID, [EntryStatusID] = @EntryStatusID WHERE CategoryID = @CategoryID " + "\r\n";
            queryString = queryString + "       SELECT      CategoryID, Description, LogoGenerator, EntryDate, Remarks, UserID, UserOrganizationID, EntryStatusID FROM ListCategory WHERE CategoryID = @CategoryID " + "\r\n";

            SQLDatabase.CreateStoredProcedure("ListCategoryUpdate", queryString);


            queryString = " @CategoryID int ";
            queryString = queryString + " WITH ENCRYPTION " + "\r\n";
            queryString = queryString + " AS " + "\r\n";
            queryString = queryString + "       DELETE FROM ListCategory WHERE CategoryID = @CategoryID " + "\r\n";
            SQLDatabase.CreateStoredProcedure("ListCategoryDelete", queryString);






            queryString = " @CategoryID int " + "\r\n";
            queryString = queryString + " WITH ENCRYPTION " + "\r\n";
            queryString = queryString + " AS " + "\r\n";
            queryString = queryString + "       SELECT      CategoryID, SerialID, CommonID, CommonValue, Remarks " + "\r\n";
            queryString = queryString + "       FROM        ListCategoryDetail " + "\r\n";
            queryString = queryString + "       WHERE       CategoryID = @CategoryID " + "\r\n";

            SQLDatabase.CreateStoredProcedure("ListCategoryDetailSelect", queryString);


            queryString = " @CategoryID int, @SerialID int,	@CommonID int, @CommonValue float, @Remarks nvarchar(100) " + "\r\n";
            queryString = queryString + " WITH ENCRYPTION " + "\r\n";
            queryString = queryString + " AS " + "\r\n";
            queryString = queryString + "       INSERT INTO ListCategoryDetail (CategoryID, SerialID, CommonID, CommonValue, Remarks) VALUES (@CategoryID, @SerialID, @CommonID, @CommonValue, @Remarks) " + "\r\n";
            queryString = queryString + "       SELECT      CategoryID, SerialID, CommonID, CommonValue, Remarks FROM ListCategoryDetail WHERE (CategoryID = @CategoryID) AND (SerialID = @SerialID) " + "\r\n";

            SQLDatabase.CreateStoredProcedure("ListCategoryDetailInsert", queryString);



            queryString = " @CategoryID int, @SerialID int,	@CommonID int, @CommonValue float, @Remarks nvarchar(100) " + "\r\n";
            queryString = queryString + " WITH ENCRYPTION " + "\r\n";
            queryString = queryString + " AS " + "\r\n";
            queryString = queryString + "       UPDATE      ListCategoryDetail SET CommonID = @CommonID, CommonValue = @CommonValue, Remarks = @Remarks WHERE CategoryID = @CategoryID AND SerialID = @SerialID " + "\r\n";
            queryString = queryString + "       SELECT      CategoryID, SerialID, CommonID, CommonValue, Remarks FROM ListCategoryDetail WHERE CategoryID = @CategoryID AND SerialID = @SerialID " + "\r\n";

            SQLDatabase.CreateStoredProcedure("ListCategoryDetailUpdate", queryString);



            queryString = " @CategoryID int " + "\r\n";
            queryString = queryString + " WITH ENCRYPTION " + "\r\n";
            queryString = queryString + " AS " + "\r\n";
            queryString = queryString + "       DELETE FROM ListCategoryDetail WHERE CategoryID = @CategoryID " + "\r\n";
            SQLDatabase.CreateStoredProcedure("ListCategoryDetailDelete", queryString);



            /// <summary>
            /// Check for editable
            /// </summary>
            queryArray = new string[1];
            queryArray[0] = "   SELECT      TOP 1 CategoryID FROM DataMessageMaster WHERE CategoryID = @FindIdentityID ";

            SQLDatabase.CreateProcedureToCheckExisting("ListCategoryEditable", queryArray);

        }
    }
}
