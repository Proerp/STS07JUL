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
using DataAccessLayer.MetaDataList.ListProductDTSTableAdapters;

namespace BusinessLogicLayer.MetaDataList
{
    public class ListProductBLL : NotifyPropertyChangeObject
    {
        public GlobalEnum.TaskID TaskID { get { return GlobalEnum.TaskID.ListProduct; } }

        private UserInformation userOrganization;

        private ListProductMaster listProductMaster;

        private BindingList<ListProductDetail> listProductDetailList;



        public ListProductBLL()
        {
            try
            {
                if (GlobalVariables.shouldRestoreProcedure) RestoreProcedure();


                userOrganization = new UserInformation();

                listProductMaster = new ListProductMaster();

                this.listProductDetailList = new BindingList<ListProductDetail>();

                GlobalDefaultValue.Apply(this);


                this.ListProductMaster.PropertyChanged += new PropertyChangedEventHandler(ListProductMaster_PropertyChanged);

                this.ListProductDetailList.ListChanged += new ListChangedEventHandler(ListProductDetailList_ListChanged);

            }
            catch (Exception exception)
            {
                throw exception;
            }
        }

        private void ListProductMaster_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            this.SetDirty();
        }

        private void ListProductDetailList_ListChanged(object sender, ListChangedEventArgs e)
        {
            this.SetDirty();
        }




        #region <Adapter>

        private ListProductListingTableAdapter listingTableAdapter;
        protected ListProductListingTableAdapter ListingTableAdapter
        {
            get
            {
                if (listingTableAdapter == null) listingTableAdapter = new ListProductListingTableAdapter();
                return listingTableAdapter;
            }
        }

        private ListProductTableAdapter masterTableAdapter;
        protected ListProductTableAdapter MasterTableAdapter
        {
            get
            {
                if (masterTableAdapter == null) masterTableAdapter = new ListProductTableAdapter();
                return masterTableAdapter;
            }
        }

        private ListProductDetailTableAdapter detailTableAdapter;
        protected ListProductDetailTableAdapter DetailTableAdapter
        {
            get
            {
                if (detailTableAdapter == null) detailTableAdapter = new ListProductDetailTableAdapter();
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

        public ListProductDTS.ListProductListingDataTable ListProductListing()
        {
            return this.ListingTableAdapter.GetData();
        }

        public ListProductMaster ListProductMaster
        {
            get { return this.listProductMaster; }
        }

        public BindingList<ListProductDetail> ListProductDetailList
        {
            get { return this.listProductDetailList; }
        }


        #endregion <Storage>

        #region Properties

        #region <Primary Key>

        public int ProductID   //Primary Key
        {
            get { return this.ListProductMaster.ProductID; }
            private set
            {
                if (this.ListProductMaster.ProductID != value)
                {
                    this.StopTracking();

                    this.ListProductMaster.ProductID = value;

                    this.ListProductGetMaster();
                    this.ListProductGetDetail();

                    this.StartTracking();
                    this.Reset();
                }

            }
        }

        #endregion <Primary Key>

        private void ListProductGetMaster()
        {
            if (this.ProductID > 0)
            {
                ListProductDTS.ListProductDataTable masterDataTable = this.MasterTableAdapter.GetData(this.ProductID);

                if (masterDataTable.Count > 0)
                {
                    this.ListProductMaster.StopTracking();

                    this.ListProductMaster.EntryDate = masterDataTable[0].EntryDate;

                    this.ListProductMaster.Description = masterDataTable[0].Description;
                    this.ListProductMaster.LogoGenerator = masterDataTable[0].LogoGenerator;
                    this.ListProductMaster.Remarks = masterDataTable[0].Remarks;

                    this.ListProductMaster.StartTracking();

                    this.ListProductMaster.Reset();

                    this.UserOrganization.UserID = masterDataTable[0].UserID;
                    this.UserOrganization.UserOrganizationID = masterDataTable[0].UserOrganizationID;
                }
                else throw new System.ArgumentException("Insufficient get data");
            }
            else
            {
                GlobalDefaultValue.Apply(this.ListProductMaster);
                this.ListProductMaster.EntryDate = DateTime.Today;
                this.ListProductMaster.Reset();
            }
        }


        private void ListProductGetDetail()
        {
            this.listProductDetailList.RaiseListChangedEvents = false;
            this.listProductDetailList.Clear();
            if (this.ProductID >= 0)
            {

                ListProductDTS.ListProductDetailDataTable detailDataTable = this.DetailTableAdapter.GetData(this.ProductID);

                if (detailDataTable.Count > 0)
                {
                    foreach (ListProductDTS.ListProductDetailRow detailRow in detailDataTable.Rows)
                    {
                        this.listProductDetailList.Add(new ListProductDetail(detailRow.CommonID, detailRow.CommonValue, detailRow.Remarks));
                    }
                }
            }
            this.listProductDetailList.RaiseListChangedEvents = true;
            this.listProductDetailList.ResetBindings();
        }

        #endregion Properties





        #region Object Permission

        public override bool IsValid
        {
            get
            {
                List<ListProductDetail> inValidListProductDetail = this.ListProductDetailList.Where(listProductDetail => !listProductDetail.IsValid).ToList();
                return this.ListProductMaster.IsValid && inValidListProductDetail.Count == 0;
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

                    return GlobalUserPermission.GetEditable("ListProductEditable", this.ProductID);
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
                    if (!GlobalUserPermission.GetUserEditable(GlobalVariables.GlobalUserInformation.UserID, GlobalEnum.TaskID.ListProductVerifiable, this.UserOrganization.UserOrganizationID)) return false;

                    return GlobalUserPermission.GetEditable("ListProductEditable", this.ProductID);
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
                    if (!GlobalUserPermission.GetUserEditable(GlobalVariables.GlobalUserInformation.UserID, GlobalEnum.TaskID.ListProductUnverifiable, this.UserOrganization.UserOrganizationID)) return false;

                    return GlobalUserPermission.GetEditable("ListProductEditable", this.ProductID);
                }
                catch
                {
                    return false;
                }
            }
        }


        #endregion Object Permission






        public void Fill(int productID)
        {
            if (this.ProductID == productID) this.ProductID = -1; //Enforce to reload
            this.ProductID = productID;
        }

        public void New()
        {
            if (this.ProductID == 0) this.ProductID = -1;
            this.ProductID = 0;
        }

        public void Edit()
        {

        }

        #region Save & Delete Method


        public bool Save()
        {
            int productID = 0;

            try
            {

                if (!this.SaveValidate()) throw new System.ArgumentException("Insufficient save", "Save validate");

                using (TransactionScope transactionScope = new TransactionScope())
                {
                    if (this.ReadOnly) throw new System.ArgumentException("Insufficient save", "Uneditable");

                    if (!this.SaveMaster(ref productID)) throw new System.ArgumentException("Insufficient save", "Save master");

                    if (!this.SaveDetail(productID)) throw new System.ArgumentException("Insufficient save", "Save detail");

                    transactionScope.Complete();
                }

                this.Fill(productID);
                return true;
            }
            catch (System.Exception exception)
            {
                throw exception;
            }
        }


        public bool Delete()
        {
            if (this.listProductMaster.ProductID <= 0) return false;

            try
            {

                if (!this.SaveValidate()) throw new System.ArgumentException("Insufficient delete", "Delete validate");

                using (TransactionScope transactionScope = new TransactionScope())
                {
                    if (!this.Editable) throw new System.ArgumentException("Insufficient delete", "Uneditable");

                    if (!this.SaveUndo(this.listProductMaster.ProductID)) throw new System.ArgumentException("Insufficient delete", "Delete detail");

                    if (this.MasterTableAdapter.Delete(this.listProductMaster.ProductID) != 1) throw new System.ArgumentException("Insufficient delete", "Delete master");

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

            this.UserOrganization = GlobalUserPermission.GetUserInformation(GlobalVariables.GlobalUserInformation.UserID, this.ListProductMaster.EntryDate);

            if (this.UserOrganization.UserID <= 0 || this.UserOrganization.UserOrganizationID <= 0) exceptionTable.AddException(new string[] { GlobalVariables.stringFieldRequired, "User information" });

            if (exceptionTable.Table.Rows.Count <= 0 && this.IsValid) return true; else throw new CustomException("Save validate", exceptionTable.Table);
        }


        private bool SaveMaster(ref int productID)
        {
            ListProductDTS.ListProductDataTable masterDataTable;
            ListProductDTS.ListProductRow masterRow;

            if (this.listProductMaster.ProductID <= 0) //Add
            {
                masterDataTable = new ListProductDTS.ListProductDataTable();
                masterRow = masterDataTable.NewListProductRow();
            }
            else //Edit
            {
                if (!this.SaveUndo(listProductMaster.ProductID)) throw new System.ArgumentException("Insufficient save", "Save undo");
                masterDataTable = this.MasterTableAdapter.GetData(listProductMaster.ProductID);
                if (masterDataTable.Count > 0) masterRow = masterDataTable[0]; else throw new System.ArgumentException("Insufficient save", "Get for edit");
            }

            masterRow.EntryDate = DateTime.Now;

            masterRow.Description = this.listProductMaster.Description;
            masterRow.LogoGenerator = this.listProductMaster.LogoGenerator;
            masterRow.Remarks = this.listProductMaster.Remarks;

            masterRow.UserID = this.UserOrganization.UserID;
            masterRow.UserOrganizationID = this.UserOrganization.UserOrganizationID;

            masterRow.EntryStatusID = this.listProductMaster.ProductID <= 0 || (int)masterDataTable[0]["EntryStatusID"] == (int)GlobalEnum.EntryStatusID.IsNew ? (int)GlobalEnum.EntryStatusID.IsNew : (int)GlobalEnum.EntryStatusID.IsEdited; 

            if (this.listProductMaster.ProductID <= 0) masterDataTable.AddListProductRow(masterRow);

            int rowsAffected = this.MasterTableAdapter.Update(masterRow);

            productID = masterRow.ProductID;

            return rowsAffected == 1;

        }


        private bool SaveDetail(int productID)
        {
            int serialID = 0; int rowsAffected = 0;


            #region <Save collection>

            serialID = 0;

            ListProductDTS.ListProductDetailDataTable detailDataTable = new ListProductDTS.ListProductDetailDataTable();

            foreach (ListProductDetail listProductDetail in this.ListProductDetailList)
            {
                ListProductDTS.ListProductDetailRow detailRow = detailDataTable.NewListProductDetailRow();

                detailRow.ProductID = productID;
                detailRow.SerialID = ++serialID;

                detailRow.CommonID = listProductDetail.CommonID;
                detailRow.CommonValue = listProductDetail.CommonValue;

                detailRow.Remarks = listProductDetail.Remarks;

                detailDataTable.AddListProductDetailRow(detailRow);
            }

            rowsAffected = this.DetailTableAdapter.Update(detailDataTable);
            if (rowsAffected != this.listProductDetailList.Count) throw new System.ArgumentException("Insufficient save", "Save detail");


            #endregion <Save collection>


            return true;
        }


        private bool SaveUndo(int productID)
        {
            this.DetailTableAdapter.Delete(productID);

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
            queryString = queryString + "       SELECT      ProductID, Description, LogoGenerator, EntryDate, Remarks " + "\r\n";
            queryString = queryString + "       FROM        ListProduct " + "\r\n";
            queryString = queryString + "       ORDER BY    Description " + "\r\n";

            SQLDatabase.CreateStoredProcedure("ListProductListing", queryString);



            queryString = "     @ProductID Int " + "\r\n";
            queryString = queryString + " WITH ENCRYPTION " + "\r\n";
            queryString = queryString + " AS " + "\r\n";
            queryString = queryString + "       SELECT      ProductID, Description, LogoGenerator, EntryDate, Remarks, UserID, UserOrganizationID, EntryStatusID " + "\r\n";
            queryString = queryString + "       FROM        ListProduct " + "\r\n";
            queryString = queryString + "       WHERE       ProductID = @ProductID " + "\r\n";

            SQLDatabase.CreateStoredProcedure("ListProductSelect", queryString);


            queryString = "     @Description nvarchar(100),	@LogoGenerator nvarchar(100), @EntryDate datetime, @Remarks nchar(10), @UserID int, @UserOrganizationID int, @EntryStatusID int " + "\r\n";
            queryString = queryString + " WITH ENCRYPTION " + "\r\n";
            queryString = queryString + " AS " + "\r\n";
            queryString = queryString + "       INSERT INTO ListProduct ([Description], [LogoGenerator], [EntryDate], [Remarks], [UserID], [UserOrganizationID], [EntryStatusID]) VALUES (@Description, @LogoGenerator, @EntryDate, @Remarks, @UserID, @UserOrganizationID, @EntryStatusID) " + "\r\n";
            queryString = queryString + "       SELECT      ProductID, Description, LogoGenerator, EntryDate, Remarks, UserID, UserOrganizationID, EntryStatusID FROM ListProduct WHERE ProductID = SCOPE_IDENTITY() " + "\r\n";

            SQLDatabase.CreateStoredProcedure("ListProductInsert", queryString);


            queryString = "     @ProductID int, @Description nvarchar(100), @LogoGenerator nvarchar(100), @EntryDate datetime, @Remarks nchar(10), @UserID int, @UserOrganizationID int, @EntryStatusID int " + "\r\n";
            queryString = queryString + " WITH ENCRYPTION " + "\r\n";
            queryString = queryString + " AS " + "\r\n";
            queryString = queryString + "       UPDATE      ListProduct SET [Description] = @Description, [LogoGenerator] = @LogoGenerator, [EntryDate] = @EntryDate, [Remarks] = @Remarks, [UserID] = @UserID, [UserOrganizationID] = @UserOrganizationID, [EntryStatusID] = @EntryStatusID WHERE ProductID = @ProductID " + "\r\n";
            queryString = queryString + "       SELECT      ProductID, Description, LogoGenerator, EntryDate, Remarks, UserID, UserOrganizationID, EntryStatusID FROM ListProduct WHERE ProductID = @ProductID " + "\r\n";

            SQLDatabase.CreateStoredProcedure("ListProductUpdate", queryString);


            queryString = " @ProductID int ";
            queryString = queryString + " WITH ENCRYPTION " + "\r\n";
            queryString = queryString + " AS " + "\r\n";
            queryString = queryString + "       DELETE FROM ListProduct WHERE ProductID = @ProductID " + "\r\n";
            SQLDatabase.CreateStoredProcedure("ListProductDelete", queryString);






            queryString = " @ProductID int " + "\r\n";
            queryString = queryString + " WITH ENCRYPTION " + "\r\n";
            queryString = queryString + " AS " + "\r\n";
            queryString = queryString + "       SELECT      ProductID, SerialID, CommonID, CommonValue, Remarks " + "\r\n";
            queryString = queryString + "       FROM        ListProductDetail " + "\r\n";
            queryString = queryString + "       WHERE       ProductID = @ProductID " + "\r\n";

            SQLDatabase.CreateStoredProcedure("ListProductDetailSelect", queryString);


            queryString = " @ProductID int, @SerialID int,	@CommonID int, @CommonValue float, @Remarks nvarchar(100) " + "\r\n";
            queryString = queryString + " WITH ENCRYPTION " + "\r\n";
            queryString = queryString + " AS " + "\r\n";
            queryString = queryString + "       INSERT INTO ListProductDetail (ProductID, SerialID, CommonID, CommonValue, Remarks) VALUES (@ProductID, @SerialID, @CommonID, @CommonValue, @Remarks) " + "\r\n";
            queryString = queryString + "       SELECT      ProductID, SerialID, CommonID, CommonValue, Remarks FROM ListProductDetail WHERE (ProductID = @ProductID) AND (SerialID = @SerialID) " + "\r\n";

            SQLDatabase.CreateStoredProcedure("ListProductDetailInsert", queryString);



            queryString = " @ProductID int, @SerialID int,	@CommonID int, @CommonValue float, @Remarks nvarchar(100) " + "\r\n";
            queryString = queryString + " WITH ENCRYPTION " + "\r\n";
            queryString = queryString + " AS " + "\r\n";
            queryString = queryString + "       UPDATE      ListProductDetail SET CommonID = @CommonID, CommonValue = @CommonValue, Remarks = @Remarks WHERE ProductID = @ProductID AND SerialID = @SerialID " + "\r\n";
            queryString = queryString + "       SELECT      ProductID, SerialID, CommonID, CommonValue, Remarks FROM ListProductDetail WHERE ProductID = @ProductID AND SerialID = @SerialID " + "\r\n";

            SQLDatabase.CreateStoredProcedure("ListProductDetailUpdate", queryString);



            queryString = " @ProductID int " + "\r\n";
            queryString = queryString + " WITH ENCRYPTION " + "\r\n";
            queryString = queryString + " AS " + "\r\n";
            queryString = queryString + "       DELETE FROM ListProductDetail WHERE ProductID = @ProductID " + "\r\n";
            SQLDatabase.CreateStoredProcedure("ListProductDetailDelete", queryString);



            /// <summary>
            /// Check for editable
            /// </summary>
            queryArray = new string[1];
            queryArray[0] = "   SELECT      TOP 1 ProductID FROM DataMessageMaster WHERE ProductID = @FindIdentityID ";

            SQLDatabase.CreateProcedureToCheckExisting("ListProductEditable", queryArray);

        }
    }
}
