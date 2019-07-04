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
using DataAccessLayer.MetaDataList.ListCoilDTSTableAdapters;

namespace BusinessLogicLayer.MetaDataList
{
    public class ListCoilBLL : NotifyPropertyChangeObject
    {
        public GlobalEnum.TaskID TaskID { get { return GlobalEnum.TaskID.ListCoil; } }

        private UserInformation userOrganization;

        private ListCoilMaster listCoilMaster;

        private BindingList<ListCoilDetail> listCoilDetailList;



        public ListCoilBLL()
        {
            try
            {
                if (GlobalVariables.shouldRestoreProcedure) RestoreProcedure();


                userOrganization = new UserInformation();

                listCoilMaster = new ListCoilMaster();

                this.listCoilDetailList = new BindingList<ListCoilDetail>();

                GlobalDefaultValue.Apply(this);


                this.ListCoilMaster.PropertyChanged += new PropertyChangedEventHandler(ListCoilMaster_PropertyChanged);

                this.ListCoilDetailList.ListChanged += new ListChangedEventHandler(ListCoilDetailList_ListChanged);

            }
            catch (Exception exception)
            {
                throw exception;
            }
        }

        private void ListCoilMaster_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            this.SetDirty();
        }

        private void ListCoilDetailList_ListChanged(object sender, ListChangedEventArgs e)
        {
            this.SetDirty();
        }




        #region <Adapter>

        private ListCoilListingTableAdapter listingTableAdapter;
        protected ListCoilListingTableAdapter ListingTableAdapter
        {
            get
            {
                if (listingTableAdapter == null) listingTableAdapter = new ListCoilListingTableAdapter();
                return listingTableAdapter;
            }
        }

        private ListCoilTableAdapter masterTableAdapter;
        protected ListCoilTableAdapter MasterTableAdapter
        {
            get
            {
                if (masterTableAdapter == null) masterTableAdapter = new ListCoilTableAdapter();
                return masterTableAdapter;
            }
        }

        private ListCoilDetailTableAdapter detailTableAdapter;
        protected ListCoilDetailTableAdapter DetailTableAdapter
        {
            get
            {
                if (detailTableAdapter == null) detailTableAdapter = new ListCoilDetailTableAdapter();
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

        public ListCoilDTS.ListCoilListingDataTable ListCoilListing(DateTime lowerFillterDate, DateTime upperFillterDate)
        {
            return this.ListingTableAdapter.GetData(lowerFillterDate, upperFillterDate);
        }

        public ListCoilMaster ListCoilMaster
        {
            get { return this.listCoilMaster; }
        }

        public BindingList<ListCoilDetail> ListCoilDetailList
        {
            get { return this.listCoilDetailList; }
        }


        #endregion <Storage>

        #region Properties

        #region <Primary Key>

        public int CoilID   //Primary Key
        {
            get { return this.ListCoilMaster.CoilID; }
            private set
            {
                if (this.ListCoilMaster.CoilID != value)
                {
                    this.StopTracking();

                    this.ListCoilMaster.CoilID = value;

                    this.ListCoilGetMaster();
                    this.ListCoilGetDetail();

                    this.StartTracking();
                    this.Reset();
                }

            }
        }

        #endregion <Primary Key>

        private void ListCoilGetMaster()
        {
            if (this.CoilID > 0)
            {
                ListCoilDTS.ListCoilDataTable masterDataTable = this.MasterTableAdapter.GetData(this.CoilID);

                if (masterDataTable.Count > 0)
                {
                    this.ListCoilMaster.StopTracking();

                    this.ListCoilMaster.EntryDate = masterDataTable[0].EntryDate;

                    this.ListCoilMaster.Description = masterDataTable[0].Description;
                    this.ListCoilMaster.Remarks = masterDataTable[0].Remarks;

                    this.ListCoilMaster.StartTracking();

                    this.ListCoilMaster.Reset();

                    this.UserOrganization.UserID = masterDataTable[0].UserID;
                    this.UserOrganization.UserOrganizationID = masterDataTable[0].UserOrganizationID;
                }
                else throw new System.ArgumentException("Insufficient get data");
            }
            else
            {
                GlobalDefaultValue.Apply(this.ListCoilMaster);
                this.ListCoilMaster.EntryDate = DateTime.Today;
                this.ListCoilMaster.Reset();
            }
        }


        private void ListCoilGetDetail()
        {
            this.listCoilDetailList.RaiseListChangedEvents = false;
            this.listCoilDetailList.Clear();
            if (this.CoilID >= 0)
            {

                ListCoilDTS.ListCoilDetailDataTable detailDataTable = this.DetailTableAdapter.GetData(this.CoilID);

                if (detailDataTable.Count > 0)
                {
                    foreach (ListCoilDTS.ListCoilDetailRow detailRow in detailDataTable.Rows)
                    {
                        this.listCoilDetailList.Add(new ListCoilDetail(detailRow.CommonID, detailRow.CommonValue, detailRow.Remarks));
                    }
                }
            }
            this.listCoilDetailList.RaiseListChangedEvents = true;
            this.listCoilDetailList.ResetBindings();
        }

        #endregion Properties





        #region Object Permission

        public override bool IsValid
        {
            get
            {
                List<ListCoilDetail> inValidListCoilDetail = this.ListCoilDetailList.Where(listCoilDetail => !listCoilDetail.IsValid).ToList();
                return this.ListCoilMaster.IsValid && inValidListCoilDetail.Count == 0;
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

                    return GlobalUserPermission.GetEditable("ListCoilEditable", this.CoilID);
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
                    if (!GlobalUserPermission.GetUserEditable(GlobalVariables.GlobalUserInformation.UserID, GlobalEnum.TaskID.ListCoilVerifiable, this.UserOrganization.UserOrganizationID)) return false;

                    return GlobalUserPermission.GetEditable("ListCoilEditable", this.CoilID);
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
                    if (!GlobalUserPermission.GetUserEditable(GlobalVariables.GlobalUserInformation.UserID, GlobalEnum.TaskID.ListCoilUnverifiable, this.UserOrganization.UserOrganizationID)) return false;

                    return GlobalUserPermission.GetEditable("ListCoilEditable", this.CoilID);
                }
                catch
                {
                    return false;
                }
            }
        }


        #endregion Object Permission






        public void Fill(int coilID)
        {
            if (this.CoilID == coilID) this.CoilID = -1; //Enforce to reload
            this.CoilID = coilID;
        }

        public void New()
        {
            if (this.CoilID == 0) this.CoilID = -1;
            this.CoilID = 0;
        }

        public void Edit()
        {

        }

        #region Save & Delete Method


        public bool Save()
        {
            int coilID = 0;

            try
            {

                if (!this.SaveValidate()) throw new System.ArgumentException("Insufficient save", "Save validate");

                using (TransactionScope transactionScope = new TransactionScope())
                {
                    if (this.ReadOnly) throw new System.ArgumentException("Insufficient save", "Uneditable");

                    if (!this.SaveMaster(ref coilID)) throw new System.ArgumentException("Insufficient save", "Save master");

                    if (!this.SaveDetail(coilID)) throw new System.ArgumentException("Insufficient save", "Save detail");

                    transactionScope.Complete();
                }

                this.Fill(coilID);
                return true;
            }
            catch (System.Exception exception)
            {
                throw exception;
            }
        }


        public bool Delete()
        {
            if (this.listCoilMaster.CoilID <= 0) return false;

            try
            {

                if (!this.SaveValidate()) throw new System.ArgumentException("Insufficient delete", "Delete validate");

                using (TransactionScope transactionScope = new TransactionScope())
                {
                    if (!this.Editable) throw new System.ArgumentException("Insufficient delete", "Uneditable");

                    if (!this.SaveUndo(this.listCoilMaster.CoilID)) throw new System.ArgumentException("Insufficient delete", "Delete detail");

                    if (this.MasterTableAdapter.Delete(this.listCoilMaster.CoilID) != 1) throw new System.ArgumentException("Insufficient delete", "Delete master");

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

            this.UserOrganization = GlobalUserPermission.GetUserInformation(GlobalVariables.GlobalUserInformation.UserID, this.ListCoilMaster.EntryDate);

            if (this.UserOrganization.UserID <= 0 || this.UserOrganization.UserOrganizationID <= 0) exceptionTable.AddException(new string[] { GlobalVariables.stringFieldRequired, "User information" });

            if (exceptionTable.Table.Rows.Count <= 0 && this.IsValid) return true; else throw new CustomException("Save validate", exceptionTable.Table);
        }


        private bool SaveMaster(ref int coilID)
        {
            ListCoilDTS.ListCoilDataTable masterDataTable;
            ListCoilDTS.ListCoilRow masterRow;

            if (this.listCoilMaster.CoilID <= 0) //Add
            {
                masterDataTable = new ListCoilDTS.ListCoilDataTable();
                masterRow = masterDataTable.NewListCoilRow();
            }
            else //Edit
            {
                if (!this.SaveUndo(listCoilMaster.CoilID)) throw new System.ArgumentException("Insufficient save", "Save undo");
                masterDataTable = this.MasterTableAdapter.GetData(listCoilMaster.CoilID);
                if (masterDataTable.Count > 0) masterRow = masterDataTable[0]; else throw new System.ArgumentException("Insufficient save", "Get for edit");
            }

            masterRow.EntryDate = DateTime.Now;

            masterRow.Description = this.listCoilMaster.Description;
            masterRow.Remarks = this.listCoilMaster.Remarks;

            masterRow.UserID = this.UserOrganization.UserID;
            masterRow.UserOrganizationID = this.UserOrganization.UserOrganizationID;

            masterRow.EntryStatusID = this.listCoilMaster.CoilID <= 0 || (int)masterDataTable[0]["EntryStatusID"] == (int)GlobalEnum.EntryStatusID.IsNew ? (int)GlobalEnum.EntryStatusID.IsNew : (int)GlobalEnum.EntryStatusID.IsEdited; 

            if (this.listCoilMaster.CoilID <= 0) masterDataTable.AddListCoilRow(masterRow);

            int rowsAffected = this.MasterTableAdapter.Update(masterRow);

            coilID = masterRow.CoilID;

            return rowsAffected == 1;

        }


        private bool SaveDetail(int coilID)
        {
            int serialID = 0; int rowsAffected = 0;


            #region <Save collection>

            serialID = 0;

            ListCoilDTS.ListCoilDetailDataTable detailDataTable = new ListCoilDTS.ListCoilDetailDataTable();

            foreach (ListCoilDetail listCoilDetail in this.ListCoilDetailList)
            {
                ListCoilDTS.ListCoilDetailRow detailRow = detailDataTable.NewListCoilDetailRow();

                detailRow.CoilID = coilID;
                detailRow.SerialID = ++serialID;

                detailRow.CommonID = listCoilDetail.CommonID;
                detailRow.CommonValue = listCoilDetail.CommonValue;

                detailRow.Remarks = listCoilDetail.Remarks;

                detailDataTable.AddListCoilDetailRow(detailRow);
            }

            rowsAffected = this.DetailTableAdapter.Update(detailDataTable);
            if (rowsAffected != this.listCoilDetailList.Count) throw new System.ArgumentException("Insufficient save", "Save detail");


            #endregion <Save collection>


            return true;
        }


        private bool SaveUndo(int coilID)
        {
            this.DetailTableAdapter.Delete(coilID);

            return true;
        }



        #endregion



        private void RestoreProcedure()
        {
            string queryString;
            string[] queryArray;



            queryString = "     @LowerFillterDate DateTime, @UpperFillterDate DateTime " + "\r\n";
            queryString = queryString + " WITH ENCRYPTION " + "\r\n";
            queryString = queryString + " AS " + "\r\n";
            queryString = queryString + "       SELECT      CoilID, Description, EntryDate, Remarks " + "\r\n";
            queryString = queryString + "       FROM        ListCoil " + "\r\n";

            queryString = queryString + "       WHERE       CoilID NOT IN (SELECT CoilID FROM DataMessageMaster) OR CoilID IN (SELECT CoilID FROM DataMessageMaster WHERE ProductionDate >= @LowerFillterDate AND ProductionDate <= @UpperFillterDate) " + "\r\n";

            queryString = queryString + "       ORDER BY    Description " + "\r\n";

            SQLDatabase.CreateStoredProcedure("ListCoilListing", queryString);



            queryString = "     @CoilID Int " + "\r\n";
            queryString = queryString + " WITH ENCRYPTION " + "\r\n";
            queryString = queryString + " AS " + "\r\n";
            queryString = queryString + "       SELECT      CoilID, Description, EntryDate, Remarks, UserID, UserOrganizationID, EntryStatusID " + "\r\n";
            queryString = queryString + "       FROM        ListCoil " + "\r\n";
            queryString = queryString + "       WHERE       CoilID = @CoilID " + "\r\n";

            SQLDatabase.CreateStoredProcedure("ListCoilSelect", queryString);


            queryString = "     @Description nvarchar(100),	@EntryDate datetime, @Remarks nchar(10), @UserID int, @UserOrganizationID int, @EntryStatusID int " + "\r\n";
            queryString = queryString + " WITH ENCRYPTION " + "\r\n";
            queryString = queryString + " AS " + "\r\n";
            queryString = queryString + "       INSERT INTO ListCoil ([Description], [EntryDate], [Remarks], [UserID], [UserOrganizationID], [EntryStatusID]) VALUES (@Description, @EntryDate, @Remarks, @UserID, @UserOrganizationID, @EntryStatusID) " + "\r\n";
            queryString = queryString + "       SELECT      CoilID, Description, EntryDate, Remarks, UserID, UserOrganizationID, EntryStatusID FROM ListCoil WHERE CoilID = SCOPE_IDENTITY() " + "\r\n";

            SQLDatabase.CreateStoredProcedure("ListCoilInsert", queryString);


            queryString = "     @CoilID int, @Description nvarchar(100), @EntryDate datetime, @Remarks nchar(10), @UserID int, @UserOrganizationID int, @EntryStatusID int " + "\r\n";
            queryString = queryString + " WITH ENCRYPTION " + "\r\n";
            queryString = queryString + " AS " + "\r\n";
            queryString = queryString + "       UPDATE      ListCoil SET [Description] = @Description, [EntryDate] = @EntryDate, [Remarks] = @Remarks, [UserID] = @UserID, [UserOrganizationID] = @UserOrganizationID, [EntryStatusID] = @EntryStatusID WHERE CoilID = @CoilID " + "\r\n";
            queryString = queryString + "       SELECT      CoilID, Description, EntryDate, Remarks, UserID, UserOrganizationID, EntryStatusID FROM ListCoil WHERE CoilID = @CoilID " + "\r\n";

            SQLDatabase.CreateStoredProcedure("ListCoilUpdate", queryString);


            queryString = " @CoilID int ";
            queryString = queryString + " WITH ENCRYPTION " + "\r\n";
            queryString = queryString + " AS " + "\r\n";
            queryString = queryString + "       DELETE FROM ListCoil WHERE CoilID = @CoilID " + "\r\n";
            SQLDatabase.CreateStoredProcedure("ListCoilDelete", queryString);






            queryString = " @CoilID int " + "\r\n";
            queryString = queryString + " WITH ENCRYPTION " + "\r\n";
            queryString = queryString + " AS " + "\r\n";
            queryString = queryString + "       SELECT      CoilID, SerialID, CommonID, CommonValue, Remarks " + "\r\n";
            queryString = queryString + "       FROM        ListCoilDetail " + "\r\n";
            queryString = queryString + "       WHERE       CoilID = @CoilID " + "\r\n";

            SQLDatabase.CreateStoredProcedure("ListCoilDetailSelect", queryString);


            queryString = " @CoilID int, @SerialID int,	@CommonID int, @CommonValue float, @Remarks nvarchar(100) " + "\r\n";
            queryString = queryString + " WITH ENCRYPTION " + "\r\n";
            queryString = queryString + " AS " + "\r\n";
            queryString = queryString + "       INSERT INTO ListCoilDetail (CoilID, SerialID, CommonID, CommonValue, Remarks) VALUES (@CoilID, @SerialID, @CommonID, @CommonValue, @Remarks) " + "\r\n";
            queryString = queryString + "       SELECT      CoilID, SerialID, CommonID, CommonValue, Remarks FROM ListCoilDetail WHERE (CoilID = @CoilID) AND (SerialID = @SerialID) " + "\r\n";

            SQLDatabase.CreateStoredProcedure("ListCoilDetailInsert", queryString);



            queryString = " @CoilID int, @SerialID int,	@CommonID int, @CommonValue float, @Remarks nvarchar(100) " + "\r\n";
            queryString = queryString + " WITH ENCRYPTION " + "\r\n";
            queryString = queryString + " AS " + "\r\n";
            queryString = queryString + "       UPDATE      ListCoilDetail SET CommonID = @CommonID, CommonValue = @CommonValue, Remarks = @Remarks WHERE CoilID = @CoilID AND SerialID = @SerialID " + "\r\n";
            queryString = queryString + "       SELECT      CoilID, SerialID, CommonID, CommonValue, Remarks FROM ListCoilDetail WHERE CoilID = @CoilID AND SerialID = @SerialID " + "\r\n";

            SQLDatabase.CreateStoredProcedure("ListCoilDetailUpdate", queryString);



            queryString = " @CoilID int " + "\r\n";
            queryString = queryString + " WITH ENCRYPTION " + "\r\n";
            queryString = queryString + " AS " + "\r\n";
            queryString = queryString + "       DELETE FROM ListCoilDetail WHERE CoilID = @CoilID " + "\r\n";
            SQLDatabase.CreateStoredProcedure("ListCoilDetailDelete", queryString);



            /// <summary>
            /// Check for editable
            /// </summary>
            queryArray = new string[1];
            queryArray[0] = "   SELECT      TOP 1 CoilID FROM DataMessageMaster WHERE CoilID = @FindIdentityID ";

            SQLDatabase.CreateProcedureToCheckExisting("ListCoilEditable", queryArray);

        }
    }
}
