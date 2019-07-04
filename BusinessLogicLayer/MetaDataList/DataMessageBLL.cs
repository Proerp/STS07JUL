using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Data;
using System.Transactions;


using Global.Class.Library;
using DataTransferObject;
using DataAccessLayer;
using DataAccessLayer.DataMessageDTSTableAdapters;

namespace BusinessLogicLayer
{
    public class DataMessageBLL : NotifyPropertyChangeObject
    {
        public GlobalEnum.TaskID TaskID { get { return GlobalEnum.TaskID.DataMessage; } }

        private UserInformation userOrganization;

        private DataMessageMaster dataMessageMaster;

        private BindingList<DataMessageMaster> dataMessageMasterList;
        private BindingList<DataMessageDetail> dataMessageDetailList;



        public DataMessageBLL()
        {
            try
            {
                if (GlobalVariables.shouldRestoreProcedure) RestoreProcedure();

                userOrganization = new UserInformation();

                this.dataMessageMaster = new DataMessageMaster();


                this.dataMessageMasterList = new BindingList<DataMessageMaster>();
                this.dataMessageDetailList = new BindingList<DataMessageDetail>();

                GlobalDefaultValue.Apply(this);


                this.DataMessageMaster.PropertyChanged += new PropertyChangedEventHandler(DataMessageMaster_PropertyChanged);

                this.dataMessageMasterList.ListChanged += new ListChangedEventHandler(DataMessageDetail_ListChanged);
                this.DataMessageDetailList.ListChanged += new ListChangedEventHandler(DataMessageDetail_ListChanged);

            }
            catch (Exception exception)
            {
                throw exception;
            }
        }

        private void DataMessageMaster_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            this.SetDirty();
        }

        private void DataMessageDetail_ListChanged(object sender, ListChangedEventArgs e)
        {
            this.SetDirty();
        }




        #region <Adapter>


        private DataMessageMasterTableAdapter masterTableAdapter;
        protected DataMessageMasterTableAdapter MasterTableAdapter
        {
            get
            {
                if (masterTableAdapter == null) masterTableAdapter = new DataMessageMasterTableAdapter();
                return masterTableAdapter;
            }
        }

        private DataMessageDetailTableAdapter detailTableAdapter;
        protected DataMessageDetailTableAdapter DetailTableAdapter
        {
            get
            {
                if (detailTableAdapter == null) detailTableAdapter = new DataMessageDetailTableAdapter();
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

        public void DataMessageMasterListing(DateTime lowerFillterDate, DateTime upperFillterDate)
        {
            this.dataMessageMasterList.RaiseListChangedEvents = false;
            this.dataMessageMasterList.Clear();

            DataMessageDTS.DataMessageMasterDataTable dataMessageMasterDataTable = this.MasterTableAdapter.GetDataByDate(lowerFillterDate, upperFillterDate);

            if (dataMessageMasterDataTable.Count > 0)
            {
                foreach (DataMessageDTS.DataMessageMasterRow dataMessageMasterRow in dataMessageMasterDataTable.Rows)
                {
                    this.dataMessageMasterList.Add(new DataMessageMaster(dataMessageMasterRow.DataMessageID, dataMessageMasterRow.ForeignMessageID, dataMessageMasterRow.RequestedEmployeeID, dataMessageMasterRow.DataStatusID, dataMessageMasterRow.Verified, dataMessageMasterRow.VerifiedDate, dataMessageMasterRow.ProductionDate, dataMessageMasterRow.ProductionDatePrintable, dataMessageMasterRow.EntryDate, dataMessageMasterRow.BeginingDate, dataMessageMasterRow.EndingDate, dataMessageMasterRow.LogoID, dataMessageMasterRow.LogoName, dataMessageMasterRow.LogoLogo, dataMessageMasterRow.LogoPrintable, dataMessageMasterRow.FactoryID, dataMessageMasterRow.FactoryName, dataMessageMasterRow.FactoryLogo, dataMessageMasterRow.FactoryPrintable, dataMessageMasterRow.OwnerID, dataMessageMasterRow.OwnerName, dataMessageMasterRow.OwnerLogo, dataMessageMasterRow.OwnerPrintable, dataMessageMasterRow.CategoryID, dataMessageMasterRow.CategoryName, dataMessageMasterRow.CategoryLogo, dataMessageMasterRow.CategoryPrintable, dataMessageMasterRow.ProductID, dataMessageMasterRow.ProductName, dataMessageMasterRow.ProductLogo, dataMessageMasterRow.ProductPrintable, dataMessageMasterRow.CoilID, dataMessageMasterRow.CoilCode, dataMessageMasterRow.CoilExtension, dataMessageMasterRow.CoilPrintable, dataMessageMasterRow.CounterValue, dataMessageMasterRow.CounterAutonics, dataMessageMasterRow.CounterPrintable, dataMessageMasterRow.Remarks));
                }
            }

            this.dataMessageMasterList.RaiseListChangedEvents = true;
            this.dataMessageMasterList.ResetBindings();
        }

        public DataMessageMaster DataMessageMaster
        {
            get { return this.dataMessageMaster; }
            //set
            //{
            //    this.StopTracking();

            //    if (this.DataMessageMaster != null)
            //        this.DataMessageMaster.PropertyChanged -= new PropertyChangedEventHandler(DataMessageMaster_PropertyChanged);

            //    this.dataMessageMaster = value;
            //    this.DataMessageMaster.PropertyChanged += new PropertyChangedEventHandler(DataMessageMaster_PropertyChanged);



            //    this.DataMessageGetDetail();

            //    this.StartTracking();
            //    this.Reset();
            //}
        }

        public BindingList<DataMessageMaster> DataMessageMasterList
        {
            get { return this.dataMessageMasterList; }
        }

        public BindingList<DataMessageDetail> DataMessageDetailList
        {
            get { return this.dataMessageDetailList; }
        }


        #endregion <Storage>

        #region Properties

        #region <Primary Key>

        public int DataMessageID   //Primary Key
        {
            get { return this.DataMessageMaster.DataMessageID; }
            private set
            {
                if (this.DataMessageMaster.DataMessageID != value)
                {
                    this.StopTracking();

                    this.DataMessageMaster.DataMessageID = value;

                    this.DataMessageGetMaster();
                    this.DataMessageGetDetail();

                    this.StartTracking();
                    this.NotifyPropertyChanged("DataMessageID"); //--This NotifyPropertyChanged: Only at SONG THAN Project. See dataMessageBLL_PropertyChanged for more detail 
                    this.Reset();
                }

            }
        }

        #endregion <Primary Key>

        private void DataMessageGetMaster()
        {
            if (this.DataMessageID > 0)
            {
                DataMessageDTS.DataMessageMasterDataTable masterDataTable = this.MasterTableAdapter.GetData(this.DataMessageID);

                if (masterDataTable.Count > 0)
                {
                    this.DataMessageMaster.StopTracking();

                    this.DataMessageMaster.ForeignMessageID = masterDataTable[0].ForeignMessageID;
                    this.DataMessageMaster.RequestedEmployeeID = masterDataTable[0].RequestedEmployeeID;
                    this.DataMessageMaster.DataStatusID = masterDataTable[0].DataStatusID;

                    this.DataMessageMaster.ProductionDatePrintable = masterDataTable[0].ProductionDatePrintable;
                    this.DataMessageMaster.ProductionDate = masterDataTable[0].ProductionDate;
                    this.DataMessageMaster.EntryDate = masterDataTable[0].EntryDate;

                    this.DataMessageMaster.BeginingDate = masterDataTable[0].BeginingDate;
                    this.DataMessageMaster.EndingDate = masterDataTable[0].EndingDate;

                    this.DataMessageMaster.Verified = masterDataTable[0].Verified;
                    this.DataMessageMaster.VerifiedDate = masterDataTable[0].VerifiedDate;

                    this.DataMessageMaster.LogoID = masterDataTable[0].LogoID;
                    this.DataMessageMaster.LogoName = masterDataTable[0].LogoName;
                    this.DataMessageMaster.LogoLogo = masterDataTable[0].LogoLogo;
                    this.DataMessageMaster.LogoPrintable = masterDataTable[0].LogoPrintable;

                    this.DataMessageMaster.FactoryID = masterDataTable[0].FactoryID;
                    this.DataMessageMaster.FactoryName = masterDataTable[0].FactoryName;
                    this.DataMessageMaster.FactoryLogo = masterDataTable[0].FactoryLogo;
                    this.DataMessageMaster.FactoryPrintable = masterDataTable[0].FactoryPrintable;

                    this.DataMessageMaster.OwnerID = masterDataTable[0].OwnerID;
                    this.DataMessageMaster.OwnerName = masterDataTable[0].OwnerName;
                    this.DataMessageMaster.OwnerLogo = masterDataTable[0].OwnerLogo;
                    this.DataMessageMaster.OwnerPrintable = masterDataTable[0].OwnerPrintable;

                    this.DataMessageMaster.CategoryID = masterDataTable[0].CategoryID;
                    this.DataMessageMaster.CategoryName = masterDataTable[0].CategoryName;
                    this.DataMessageMaster.CategoryLogo = masterDataTable[0].CategoryLogo;
                    this.DataMessageMaster.CategoryPrintable = masterDataTable[0].CategoryPrintable;

                    this.DataMessageMaster.ProductID = masterDataTable[0].ProductID;
                    this.DataMessageMaster.ProductName = masterDataTable[0].ProductName;
                    this.DataMessageMaster.ProductLogo = masterDataTable[0].ProductLogo;
                    this.DataMessageMaster.ProductPrintable = masterDataTable[0].ProductPrintable;

                    this.DataMessageMaster.CoilID = masterDataTable[0].CoilID;
                    this.DataMessageMaster.CoilCode = masterDataTable[0].CoilCode;
                    this.DataMessageMaster.CoilExtension = masterDataTable[0].CoilExtension;
                    this.DataMessageMaster.CoilPrintable = masterDataTable[0].CoilPrintable;

                    this.DataMessageMaster.CounterValue = masterDataTable[0].CounterValue;
                    this.DataMessageMaster.CounterAutonics = masterDataTable[0].CounterAutonics;
                    this.DataMessageMaster.CounterPrintable = masterDataTable[0].CounterPrintable;

                    this.DataMessageMaster.Remarks = masterDataTable[0].Remarks;

                    this.DataMessageMaster.StartTracking();

                    this.DataMessageMaster.Reset();

                    this.UserOrganization.UserID = masterDataTable[0].UserID;
                    this.UserOrganization.UserOrganizationID = masterDataTable[0].UserOrganizationID;
                }
                else throw new System.ArgumentException("Insufficient get data");
            }
            else
            {
                GlobalDefaultValue.Apply(this.DataMessageMaster);
                this.DataMessageMaster.ProductionDate = DateTime.Today;
                this.DataMessageMaster.EntryDate = DateTime.Today;
                this.DataMessageMaster.Reset();
            }
        }


        private void DataMessageGetDetail()
        {
            this.dataMessageDetailList.RaiseListChangedEvents = false;
            this.dataMessageDetailList.Clear();
            if (this.DataMessageID > 0)
            {
                DataMessageDTS.DataMessageDetailDataTable dataMessageDetailDataTable = this.DetailTableAdapter.GetData(this.DataMessageID);

                if (dataMessageDetailDataTable.Count > 0)
                {
                    foreach (DataMessageDTS.DataMessageDetailRow dataMessageDetailRow in dataMessageDetailDataTable.Rows)
                    {
                        this.dataMessageDetailList.Add(new DataMessageDetail(dataMessageDetailRow.SendDate, dataMessageDetailRow.SendType, dataMessageDetailRow.SendMessage, dataMessageDetailRow.CounterValueBefore, dataMessageDetailRow.CounterValueAfter, dataMessageDetailRow.CounterAutonicsBefore, dataMessageDetailRow.CounterAutonicsAfter, dataMessageDetailRow.Description, dataMessageDetailRow.Remarks));
                    }
                }
            }
            this.dataMessageDetailList.RaiseListChangedEvents = true;
            this.dataMessageDetailList.ResetBindings();
        }



        #endregion Properties





        #region Object Permission

        public override bool IsValid
        {
            get
            {
                List<DataMessageDetail> inValidDataMessageDetail = this.DataMessageDetailList.Where(dataMessageDetail => !dataMessageDetail.IsValid).ToList();
                return this.DataMessageMaster.IsValid && inValidDataMessageDetail.Count == 0;
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
                    if (this.dataMessageMaster.ProductionDate <= GlobalUserPermission.GlobalLockedDate()) return false;

                    if (!GlobalUserPermission.GetUserEditable(GlobalVariables.GlobalUserInformation.UserID, this.TaskID, this.UserOrganization.UserOrganizationID)) return false;

                    if (!GlobalUserPermission.GetEditable("DataMessageMasterApproved", this.DataMessageID)) return false;

                    return GlobalUserPermission.GetEditable("DataMessageMasterEditable", this.DataMessageID);
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
                    if (this.dataMessageMaster.ProductionDate <= GlobalUserPermission.GlobalLockedDate()) return false;

                    if (!GlobalUserPermission.GetUserEditable(GlobalVariables.GlobalUserInformation.UserID, GlobalEnum.TaskID.DataMessageVerifiable, this.UserOrganization.UserOrganizationID)) return false;

                    return GlobalUserPermission.GetEditable("ApplyPropertyChange<DataMessageMasterEditable", this.DataMessageID);
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
                    if (this.dataMessageMaster.ProductionDate <= GlobalUserPermission.GlobalLockedDate()) return false;

                    if (!GlobalUserPermission.GetUserEditable(GlobalVariables.GlobalUserInformation.UserID, GlobalEnum.TaskID.DataMessageUnverifiable, this.UserOrganization.UserOrganizationID)) return false;

                    return GlobalUserPermission.GetEditable("ApplyPropertyChange<DataMessageMasterEditable", this.DataMessageID);
                }
                catch
                {
                    return false;
                }
            }
        }


        #endregion Object Permission






        public void Fill(int dataMessageID)
        {
            if (this.DataMessageID == dataMessageID) this.DataMessageID = -1; //Enforce to reload
            this.DataMessageID = dataMessageID;
        }

        //public void Fill(DataMessageMaster source)
        //{
        //    this.StopTracking();

        //    this.DataMessageMaster.CopyFrom(source);
        //    this.DataMessageGetDetail();

        //    this.StartTracking();
        //    this.Reset();
        //}

        public void New()
        {
            if (this.DataMessageID == 0) this.DataMessageID = -1;
            this.DataMessageID = 0;
        }

        public void Edit()
        {

        }

        #region Save & Delete Method

        public bool SplitCoil(int coilExtension)
        {
            try
            {
                if (coilExtension <= 1) throw new Exception("Số cuộn chia phải lớn hơn 1.");
                if (this.IsDirty || this.DataMessageMaster.DataMessageID <= 0) throw new Exception("Vui lòng lưu dữ liệu trước khi chia cuộn.");

                using (TransactionScope transactionScope = new TransactionScope())
                {
                    int coilExtensionMax = 0; string coilCodeMax = ""; string coilCodeRoot = ""; int coilCodeSuffixAscii = 0;

                    DataTable dataTable = SQLDatabase.GetDataTable("SELECT MAX(CoilCode) AS CoilCodeMax, MAX(CoilExtension) AS CoilExtensionMax FROM DataMessageMaster WHERE CoilID = " + this.DataMessageMaster.CoilID);
                    if (dataTable.Rows.Count > 0)
                    {
                        coilExtensionMax = (int)dataTable.Rows[0]["CoilExtensionMax"]; coilCodeMax = dataTable.Rows[0]["CoilCodeMax"].ToString();
                        if (coilExtensionMax == 0) { coilCodeRoot = coilCodeMax; coilCodeSuffixAscii = 65; } else { coilCodeRoot = coilCodeMax.Substring(0, coilCodeMax.Length - 1); coilCodeSuffixAscii = (int)(coilCodeMax.Substring(coilCodeMax.Length - 1))[0]; }


                        for (int i = 1; i < coilExtension; i++)
                        {
                            DataMessageBLL splitDataMessageBLL = new DataMessageBLL();

                            splitDataMessageBLL.DataMessageMaster.CopyFrom(this.DataMessageMaster);
                            splitDataMessageBLL.DataMessageMaster.DataMessageID = -1;
                            splitDataMessageBLL.DataMessageMaster.CounterValue = 0;
                            splitDataMessageBLL.DataMessageMaster.CounterAutonics = 0;
                            splitDataMessageBLL.DataMessageMaster.DataStatusID = (int)GlobalEnum.DataStatusID.NotPrintedYet;

                            splitDataMessageBLL.DataMessageMaster.CoilExtension = (coilExtensionMax == 0 ? 1 : coilExtensionMax) + i; //CUỘN GỐC: 00->01
                            splitDataMessageBLL.DataMessageMaster.CoilCode = coilCodeRoot + ((char)(coilCodeSuffixAscii + i)).ToString(); //CUỘN GỐC: []->[A]                        

                            if (!splitDataMessageBLL.Save()) throw new Exception("Lỗi chia cuộn.");

                            #region According to SONG THAN USER: This following code is used to update these NULLABLE column(s) of table DataMessageMaster (These NULLABLE column(s) are created by SONG THAN USER)
                            SQLDatabase.UpdateNullableColumns(this.DataMessageMaster.DataMessageID, splitDataMessageBLL.DataMessageID);
                            #endregion

                        }
                        if (coilExtensionMax == 0 && SQLDatabase.ExecuteNonQuery("UPDATE DataMessageMaster SET CoilExtension = 1, CoilCode = CoilCode + 'A' WHERE DataMessageID = " + this.DataMessageMaster.DataMessageID) != 1) throw new Exception("Lỗi cập nhật cuộn 00 -> 01.");
                    }

                    transactionScope.Complete();
                }

                return true;
            }
            catch (System.Exception exception)
            {
                throw exception;
            }
        }





        public bool UpdateDataStatus(GlobalEnum.DataStatusID dataStatusID)
        {
            try
            {
                if (this.DataMessageMaster.DataStatusID != (int)dataStatusID)
                {
                    if (this.MasterTableAdapter.DataMessageMasterUpdateDataStatus(this.DataMessageMaster.DataMessageID, GlobalVariables.GlobalUserInformation.UserID, (int)dataStatusID) >= 1)
                    {
                        this.DataMessageMaster.DataStatusID = (int)dataStatusID;

                        if (dataStatusID == GlobalEnum.DataStatusID.NotPrintedYet || dataStatusID == GlobalEnum.DataStatusID.OnPrinting || dataStatusID == GlobalEnum.DataStatusID.WaitForPrint)
                            this.DataMessageMasterListing(GlobalVariables.GlobalOptionSetting.LowerFillterDate, GlobalVariables.GlobalOptionSetting.UpperFillterDate);

                        return true;
                    }
                    else
                        return false;
                }
                else return true;
            }
            catch (System.Exception exception)
            {
                throw exception;
            }
        }


        public bool Save()
        {
            int dataMessageID = 0;

            try
            {

                if (!this.SaveValidate()) throw new System.ArgumentException("Insufficient save", "Save validate");

                using (TransactionScope transactionScope = new TransactionScope())
                {
                    if (!this.Editable) throw new System.ArgumentException("Insufficient save", "Uneditable");

                    if (!this.SaveMaster(ref dataMessageID)) throw new System.ArgumentException("Insufficient save", "Save master");

                    if (!this.SaveDetail(dataMessageID)) throw new System.ArgumentException("Insufficient save", "Save detail");

                    if (this.SaveConflict()) throw new System.ArgumentException("Insufficient save", "Save conflict");

                    transactionScope.Complete();
                }

                this.Fill(dataMessageID);
                return true;
            }
            catch (System.Exception exception)
            {
                throw exception;
            }
        }


        public bool Delete()
        {
            if (this.dataMessageMaster.DataMessageID <= 0) return false;

            try
            {

                if (!this.SaveValidate()) throw new System.ArgumentException("Insufficient delete", "Delete validate");

                using (TransactionScope transactionScope = new TransactionScope())
                {
                    if (!this.Editable) throw new System.ArgumentException("Insufficient delete", "Uneditable");

                    if (!this.SaveUndo(this.dataMessageMaster.DataMessageID)) throw new System.ArgumentException("Insufficient delete", "Delete detail");

                    if (this.MasterTableAdapter.Delete(this.dataMessageMaster.DataMessageID) != 1) throw new System.ArgumentException("Insufficient delete", "Delete master");

                    if (this.SaveConflict()) throw new System.ArgumentException("Insufficient delete", "Delete conflict");

                    transactionScope.Complete();                    //northwindDataSet.AcceptChanges();
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

            this.UserOrganization = GlobalUserPermission.GetUserInformation(GlobalVariables.GlobalUserInformation.UserID, this.DataMessageMaster.ProductionDate);

            if (this.UserOrganization.UserID <= 0 || this.UserOrganization.UserOrganizationID <= 0) exceptionTable.AddException(new string[] { GlobalVariables.stringFieldRequired, "User information" });

            if (exceptionTable.Table.Rows.Count <= 0 && this.IsValid) return true; else throw new CustomException("Save validate", exceptionTable.Table);
        }


        private bool SaveMaster(ref int dataMessageID)
        {
            DataMessageDTS.DataMessageMasterDataTable masterDataTable;
            DataMessageDTS.DataMessageMasterRow masterRow;

            if (this.dataMessageMaster.DataMessageID <= 0) //Add
            {
                masterDataTable = new DataMessageDTS.DataMessageMasterDataTable();
                masterRow = masterDataTable.NewDataMessageMasterRow();

                masterRow.PrintedUserID = this.UserOrganization.UserID;
                masterRow.OracleStatusID = (int)GlobalEnum.EntryStatusID.IsNew;
            }
            else //Edit
            {
                if (!this.SaveUndo(dataMessageMaster.DataMessageID)) throw new System.ArgumentException("Insufficient save", "Save undo");
                masterDataTable = this.MasterTableAdapter.GetData(dataMessageMaster.DataMessageID);
                if (masterDataTable.Count > 0) masterRow = masterDataTable[0]; else throw new System.ArgumentException("Insufficient save", "Get for edit");
            }

            masterRow.ForeignMessageID = this.dataMessageMaster.ForeignMessageID;
            masterRow.RequestedEmployeeID = this.dataMessageMaster.RequestedEmployeeID;
            masterRow.DataStatusID = this.dataMessageMaster.DataStatusID;

            masterRow.ProductionDatePrintable = this.dataMessageMaster.ProductionDatePrintable;
            masterRow.ProductionDate = this.dataMessageMaster.ProductionDate;
            masterRow.EntryDate = this.dataMessageMaster.EntryDate;

            masterRow.BeginingDate = this.dataMessageMaster.BeginingDate;
            masterRow.EndingDate = this.dataMessageMaster.EndingDate;

            masterRow.Verified = this.dataMessageMaster.Verified;
            masterRow.VerifiedDate = this.dataMessageMaster.VerifiedDate;

            masterRow.LogoID = this.dataMessageMaster.LogoID;
            masterRow.LogoName = this.dataMessageMaster.LogoName;
            masterRow.LogoLogo = this.dataMessageMaster.LogoLogo;
            masterRow.LogoPrintable = this.dataMessageMaster.LogoPrintable;

            masterRow.FactoryID = this.dataMessageMaster.FactoryID;
            masterRow.FactoryName = this.dataMessageMaster.FactoryName;
            masterRow.FactoryLogo = this.dataMessageMaster.FactoryLogo;
            masterRow.FactoryPrintable = this.dataMessageMaster.FactoryPrintable;

            masterRow.OwnerID = this.dataMessageMaster.OwnerID;
            masterRow.OwnerName = this.dataMessageMaster.OwnerName;
            masterRow.OwnerLogo = this.dataMessageMaster.OwnerLogo;
            masterRow.OwnerPrintable = this.dataMessageMaster.OwnerPrintable;

            masterRow.CategoryID = this.dataMessageMaster.CategoryID;
            masterRow.CategoryName = this.dataMessageMaster.CategoryName;
            masterRow.CategoryLogo = this.dataMessageMaster.CategoryLogo;
            masterRow.CategoryPrintable = this.dataMessageMaster.CategoryPrintable;

            masterRow.ProductID = this.dataMessageMaster.ProductID;
            masterRow.ProductName = this.dataMessageMaster.ProductName;
            masterRow.ProductLogo = this.dataMessageMaster.ProductLogo;
            masterRow.ProductPrintable = this.dataMessageMaster.ProductPrintable;

            masterRow.CoilID = this.dataMessageMaster.CoilID;
            masterRow.CoilCode = this.dataMessageMaster.CoilCode;
            masterRow.CoilExtension = this.dataMessageMaster.CoilExtension;
            masterRow.CoilPrintable = this.dataMessageMaster.CoilPrintable;

            masterRow.CounterValue = this.dataMessageMaster.CounterValue;
            masterRow.CounterAutonics = this.dataMessageMaster.CounterAutonics;
            masterRow.CounterPrintable = this.dataMessageMaster.CounterPrintable;

            masterRow.Remarks = this.dataMessageMaster.Remarks;

            masterRow.UserID = this.UserOrganization.UserID;
            masterRow.UserOrganizationID = this.UserOrganization.UserOrganizationID;

            masterRow.EntryStatusID = this.dataMessageMaster.DataMessageID <= 0 || (int)masterDataTable[0]["EntryStatusID"] == (int)GlobalEnum.EntryStatusID.IsNew ? (int)GlobalEnum.EntryStatusID.IsNew : (int)GlobalEnum.EntryStatusID.IsEdited;

            if (this.dataMessageMaster.DataMessageID <= 0) masterDataTable.AddDataMessageMasterRow(masterRow);

            int rowsAffected = this.MasterTableAdapter.Update(masterRow);

            dataMessageID = masterRow.DataMessageID;

            return rowsAffected == 1;

        }


        private bool SaveDetail(int dataMessageID)
        {
            int serialID = 0; int rowsAffected = 0;


            #region <Item Category>

            serialID = 0;

            DataMessageDTS.DataMessageDetailDataTable dataMessageDetailDataTable = new DataMessageDTS.DataMessageDetailDataTable();

            foreach (DataMessageDetail dataMessageDetail in this.dataMessageDetailList)
            {
                DataMessageDTS.DataMessageDetailRow dataMessageDetailRow = dataMessageDetailDataTable.NewDataMessageDetailRow();

                dataMessageDetailRow.DataMessageID = dataMessageID;
                dataMessageDetailRow.SerialID = ++serialID;

                dataMessageDetailRow.SendDate = dataMessageDetail.SendDate;
                dataMessageDetailRow.SendType = dataMessageDetail.SendType;
                dataMessageDetailRow.SendMessage = dataMessageDetail.SendMessage;
                dataMessageDetailRow.CounterValueBefore = dataMessageDetail.CounterValueBefore;
                dataMessageDetailRow.CounterValueAfter = dataMessageDetail.CounterValueAfter;
                dataMessageDetailRow.CounterAutonicsBefore = dataMessageDetail.CounterAutonicsBefore;
                dataMessageDetailRow.CounterAutonicsAfter = dataMessageDetail.CounterAutonicsAfter;
                dataMessageDetailRow.Description = dataMessageDetail.Description;
                dataMessageDetailRow.Remarks = dataMessageDetail.Remarks;

                dataMessageDetailDataTable.AddDataMessageDetailRow(dataMessageDetailRow);
            }

            rowsAffected = this.DetailTableAdapter.Update(dataMessageDetailDataTable);
            if (rowsAffected != this.dataMessageDetailList.Count) throw new System.ArgumentException("Insufficient save", "Save detail: Item Category");


            #endregion <Item Category>


            return true;
        }


        private bool SaveUndo(int dataMessageID)
        {
            this.DetailTableAdapter.Delete(dataMessageID);

            return true;
        }


        private bool SaveConflict()
        {
            return false;
        }



        #endregion



        #region Import Excel

        public bool ImportExcel(string fileName)
        {
            return true;

        }

        #endregion Import Excel



        private void RestoreProcedure()
        {
            string queryString;
            string[] queryArray;


            queryString = "     @LowerFillterDate DateTime, @UpperFillterDate DateTime " + "\r\n";
            queryString = queryString + " WITH ENCRYPTION " + "\r\n";
            queryString = queryString + " AS " + "\r\n";
            queryString = queryString + "       SELECT      DataMessageMaster.DataMessageID, DataMessageMaster.ForeignMessageID, DataMessageMaster.ProductionDate, DataMessageMaster.ProductionDatePrintable, DataMessageMaster.EntryDate, DataMessageMaster.BeginingDate, DataMessageMaster.EndingDate, DataMessageMaster.Verified, DataMessageMaster.VerifiedDate, DataMessageMaster.LogoID, DataMessageMaster.LogoName, DataMessageMaster.LogoLogo, DataMessageMaster.LogoPrintable, DataMessageMaster.FactoryID, DataMessageMaster.FactoryName, DataMessageMaster.FactoryLogo, DataMessageMaster.FactoryPrintable, DataMessageMaster.OwnerID, DataMessageMaster.OwnerName, DataMessageMaster.OwnerLogo, DataMessageMaster.OwnerPrintable, DataMessageMaster.CategoryID, DataMessageMaster.CategoryName, DataMessageMaster.CategoryLogo, DataMessageMaster.CategoryPrintable, DataMessageMaster.ProductID, DataMessageMaster.ProductName, DataMessageMaster.ProductLogo, DataMessageMaster.ProductPrintable, DataMessageMaster.CoilID, DataMessageMaster.CoilCode, DataMessageMaster.CoilExtension, DataMessageMaster.CoilPrintable, DataMessageMaster.CounterValue, DataMessageMaster.CounterAutonics, DataMessageMaster.CounterPrintable, DataMessageMaster.Remarks, DataMessageMaster.DataStatusID, DataMessageMaster.OracleStatusID, DataMessageMaster.RequestedEmployeeID, DataMessageMaster.UserID, DataMessageMaster.UserOrganizationID, DataMessageMaster.PrintedUserID, DataMessageMaster.EntryStatusID " + "\r\n";
            queryString = queryString + "       FROM        DataMessageMaster INNER JOIN " + "\r\n";
            queryString = queryString + "                   ListDataStatus ON DataMessageMaster.DataStatusID = ListDataStatus.DataStatusID " + "\r\n";
            queryString = queryString + "       WHERE       DataMessageMaster.DataStatusID <> " + (int)GlobalEnum.DataStatusID.PrintFinished + " OR (DataMessageMaster.ProductionDate >= @LowerFillterDate AND DataMessageMaster.ProductionDate <= @UpperFillterDate) " + "\r\n";
            queryString = queryString + "       ORDER BY    ListDataStatus.SortOrderNo, DataMessageMaster.ProductionDate DESC, DataMessageMaster.CoilCode, DataMessageMaster.CoilExtension " + "\r\n"; //DataMessageMaster.LogoName, DataMessageMaster.FactoryName, DataMessageMaster.OwnerName, DataMessageMaster.CategoryName, DataMessageMaster.ProductName, 

            SQLDatabase.CreateStoredProcedure("DataMessageMasterListing", queryString);


            queryString = "     @DataMessageID Int " + "\r\n";
            queryString = queryString + " WITH ENCRYPTION " + "\r\n";
            queryString = queryString + " AS " + "\r\n";
            queryString = queryString + "       SELECT      DataMessageID, ForeignMessageID, ProductionDate, ProductionDatePrintable, EntryDate, BeginingDate, EndingDate, Verified, VerifiedDate, LogoID, LogoName, LogoLogo, LogoPrintable, FactoryID, FactoryName, FactoryLogo, FactoryPrintable, OwnerID, OwnerName, OwnerLogo, OwnerPrintable, CategoryID, CategoryName, CategoryLogo, CategoryPrintable, ProductID, ProductName, ProductLogo, ProductPrintable, CoilID, CoilCode, CoilExtension, CoilPrintable, CounterValue, CounterAutonics, CounterPrintable, Remarks, DataStatusID, RequestedEmployeeID, UserID, UserOrganizationID, PrintedUserID, EntryStatusID, OracleStatusID " + "\r\n";
            queryString = queryString + "       FROM        DataMessageMaster " + "\r\n";
            queryString = queryString + "       WHERE       DataMessageID = @DataMessageID " + "\r\n";

            SQLDatabase.CreateStoredProcedure("DataMessageMasterSelect", queryString);


            queryString = "     @ForeignMessageID int, @ProductionDate datetime, @ProductionDatePrintable bit, @EntryDate datetime, @BeginingDate datetime, @EndingDate datetime, @Verified bit, @VerifiedDate datetime, @LogoID int, @LogoName nvarchar(100), @LogoLogo nvarchar(100), @LogoPrintable bit, @FactoryID int, @FactoryName nvarchar(100), @FactoryLogo nvarchar(100), @FactoryPrintable bit, @OwnerID int, @OwnerName nvarchar(100), @OwnerLogo nvarchar(100), @OwnerPrintable bit, @CategoryID int, @CategoryName nvarchar(100), @CategoryLogo nvarchar(100), @CategoryPrintable bit, @ProductID int, @ProductName nvarchar(100), @ProductLogo nvarchar(100), @ProductPrintable bit, @CoilID int, @CoilCode nvarchar(100), @CoilExtension int, @CoilPrintable bit, @CounterValue float, @CounterAutonics float, @CounterPrintable bit, @Remarks nvarchar(200), @DataStatusID int, @RequestedEmployeeID int, @UserID int, @UserOrganizationID int, @PrintedUserID int, @EntryStatusID int, @OracleStatusID int " + "\r\n";
            queryString = queryString + " WITH ENCRYPTION " + "\r\n";
            queryString = queryString + " AS " + "\r\n";
            queryString = queryString + "       INSERT INTO     DataMessageMaster (ForeignMessageID, ProductionDate, ProductionDatePrintable, EntryDate, BeginingDate, EndingDate, Verified, VerifiedDate, LogoID, LogoName, LogoLogo, LogoPrintable, FactoryID, FactoryName, FactoryLogo, FactoryPrintable, OwnerID, OwnerName, OwnerLogo, OwnerPrintable, CategoryID, CategoryName, CategoryLogo, CategoryPrintable, ProductID, ProductName, ProductLogo, ProductPrintable, CoilID, CoilCode, CoilExtension, CoilPrintable, CounterValue, CounterAutonics, CounterPrintable, Remarks, DataStatusID, RequestedEmployeeID, UserID, UserOrganizationID, PrintedUserID, EntryStatusID, OracleStatusID) VALUES (@ForeignMessageID, @ProductionDate, @ProductionDatePrintable, @EntryDate, @BeginingDate, @EndingDate, @Verified, @VerifiedDate, @LogoID, @LogoName, @LogoLogo, @LogoPrintable, @FactoryID, @FactoryName, @FactoryLogo, @FactoryPrintable, @OwnerID, @OwnerName, @OwnerLogo, @OwnerPrintable, @CategoryID, @CategoryName, @CategoryLogo, @CategoryPrintable, @ProductID, @ProductName, @ProductLogo, @ProductPrintable, @CoilID, @CoilCode, @CoilExtension, @CoilPrintable, @CounterValue, @CounterAutonics, @CounterPrintable, @Remarks, @DataStatusID, @RequestedEmployeeID, @UserID, @UserOrganizationID, @PrintedUserID, @EntryStatusID, @OracleStatusID) " + "\r\n";
            queryString = queryString + "       SELECT          DataMessageID, ForeignMessageID, ProductionDate, ProductionDatePrintable, EntryDate, BeginingDate, EndingDate, Verified, VerifiedDate, LogoID, LogoName, LogoLogo, LogoPrintable, FactoryID, FactoryName, FactoryLogo, FactoryPrintable, OwnerID, OwnerName, OwnerLogo, OwnerPrintable, CategoryID, CategoryName, CategoryLogo, CategoryPrintable, ProductID, ProductName, ProductLogo, ProductPrintable, CoilID, CoilCode, CoilExtension, CoilPrintable, CounterValue, CounterAutonics, CounterPrintable, Remarks, DataStatusID, RequestedEmployeeID, UserID, UserOrganizationID, PrintedUserID, EntryStatusID, OracleStatusID FROM DataMessageMaster WHERE DataMessageID = SCOPE_IDENTITY() " + "\r\n";

            SQLDatabase.CreateStoredProcedure("DataMessageMasterInsert", queryString);


            queryString = "     @DataMessageID int,	@ForeignMessageID int, @ProductionDate datetime, @ProductionDatePrintable bit, @EntryDate datetime, @BeginingDate datetime, @EndingDate datetime, @Verified bit, @VerifiedDate datetime, @LogoID int, @LogoName nvarchar(100), @LogoLogo nvarchar(100), @LogoPrintable bit, @FactoryID int, @FactoryName nvarchar(100), @FactoryLogo nvarchar(100), @FactoryPrintable bit, @OwnerID int, @OwnerName nvarchar(100), @OwnerLogo nvarchar(100), @OwnerPrintable bit, @CategoryID int, @CategoryName nvarchar(100), @CategoryLogo nvarchar(100), @CategoryPrintable bit, @ProductID int, @ProductName nvarchar(100), @ProductLogo nvarchar(100), @ProductPrintable bit, @CoilID int, @CoilCode nvarchar(100), @CoilExtension int, @CoilPrintable bit, @CounterValue float, @CounterAutonics float, @CounterPrintable bit, @Remarks nvarchar(200), @DataStatusID int, @RequestedEmployeeID int, @UserID int, @UserOrganizationID int, @PrintedUserID int, @EntryStatusID int, @OracleStatusID int " + "\r\n";
            queryString = queryString + " WITH ENCRYPTION " + "\r\n";
            queryString = queryString + " AS " + "\r\n";
            queryString = queryString + "       UPDATE      DataMessageMaster SET ForeignMessageID = @ForeignMessageID, ProductionDate = @ProductionDate, ProductionDatePrintable = @ProductionDatePrintable, EntryDate = @EntryDate, BeginingDate = @BeginingDate, EndingDate = @EndingDate, Verified = @Verified, VerifiedDate = @VerifiedDate, LogoID = @LogoID, LogoName = @LogoName, LogoLogo = @LogoLogo, LogoPrintable = @LogoPrintable, FactoryID = @FactoryID, FactoryName = @FactoryName, FactoryLogo = @FactoryLogo, FactoryPrintable = @FactoryPrintable, OwnerID = @OwnerID, OwnerName = @OwnerName, OwnerLogo = @OwnerLogo, OwnerPrintable = @OwnerPrintable, CategoryID = @CategoryID, CategoryName = @CategoryName, CategoryLogo = @CategoryLogo, CategoryPrintable = @CategoryPrintable, ProductID = @ProductID, ProductName = @ProductName, ProductLogo = @ProductLogo, ProductPrintable = @ProductPrintable, CoilID = @CoilID, CoilCode = @CoilCode, CoilExtension = @CoilExtension, CoilPrintable = @CoilPrintable, CounterValue = @CounterValue, CounterAutonics = @CounterAutonics, CounterPrintable = @CounterPrintable, Remarks = @Remarks, DataStatusID = @DataStatusID, RequestedEmployeeID = @RequestedEmployeeID, UserID = @UserID, UserOrganizationID = @UserOrganizationID, PrintedUserID = @PrintedUserID, EntryStatusID = @EntryStatusID, OracleStatusID = @OracleStatusID WHERE DataMessageID = @DataMessageID " + "\r\n";
            queryString = queryString + "       SELECT      DataMessageID, ForeignMessageID, ProductionDate, ProductionDatePrintable, EntryDate, BeginingDate, EndingDate, Verified, VerifiedDate, LogoID, LogoName, LogoLogo, LogoPrintable, FactoryID, FactoryName, FactoryLogo, FactoryPrintable, OwnerID, OwnerName, OwnerLogo, OwnerPrintable, CategoryID, CategoryName, CategoryLogo, CategoryPrintable, ProductID, ProductName, ProductLogo, ProductPrintable, CoilID, CoilCode, CoilExtension, CoilPrintable, CounterValue, CounterAutonics, CounterPrintable, Remarks, DataStatusID, RequestedEmployeeID, UserID, UserOrganizationID, PrintedUserID, EntryStatusID, OracleStatusID FROM DataMessageMaster WHERE DataMessageID = @DataMessageID " + "\r\n";

            SQLDatabase.CreateStoredProcedure("DataMessageMasterUpdate", queryString);


            queryString = "     @DataMessageID int ";
            queryString = queryString + " WITH ENCRYPTION " + "\r\n";
            queryString = queryString + " AS " + "\r\n";
            queryString = queryString + "       DELETE FROM DataMessageMaster WHERE DataMessageID = @DataMessageID " + "\r\n";
            SQLDatabase.CreateStoredProcedure("DataMessageMasterDelete", queryString);


            queryString = "     @DataMessageID int, @PrintedUserID int, @CounterValue float, @CounterAutonics float ";
            queryString = queryString + " WITH ENCRYPTION " + "\r\n";
            queryString = queryString + " AS " + "\r\n";
            queryString = queryString + "       UPDATE      DataMessageMaster SET PrintedUserID = @PrintedUserID, CounterValue = CASE @CounterValue WHEN -1 THEN CounterValue ELSE @CounterValue END, CounterAutonics = CASE @CounterAutonics WHEN -1 THEN CounterAutonics ELSE @CounterAutonics END, EntryDate = GetDate(), EntryStatusID = " + (int)GlobalEnum.EntryStatusID.IsEdited + " WHERE DataMessageID = @DataMessageID " + "\r\n";
            SQLDatabase.CreateStoredProcedure("DataMessageMasterUpdateCounter", queryString);


            queryString = "     @DataMessageID int, @PrintedUserID int, @DataStatusID int ";
            queryString = queryString + " WITH ENCRYPTION " + "\r\n";
            queryString = queryString + " AS " + "\r\n";
            queryString = queryString + "       IF (@DataStatusID = " + (int)GlobalEnum.DataStatusID.OnPrinting + " OR @DataStatusID = " + (int)GlobalEnum.DataStatusID.WaitForPrint + ") \r\n";
            queryString = queryString + "                   UPDATE      DataMessageMaster SET PrintedUserID = @PrintedUserID, DataStatusID = (CASE          WHEN DataMessageID = @DataMessageID THEN @DataStatusID            WHEN @DataStatusID = " + (int)GlobalEnum.DataStatusID.OnPrinting + " THEN " + (int)GlobalEnum.DataStatusID.PrintFinished + "      ELSE " + (int)GlobalEnum.DataStatusID.NotPrintedYet + " END)       ,        CounterValue = 0, CounterAutonics = 0   , EntryStatusID = " + (int)GlobalEnum.EntryStatusID.IsEdited + "     WHERE DataMessageID = @DataMessageID OR DataStatusID = @DataStatusID " + "\r\n";
            queryString = queryString + "       ELSE " + "\r\n";
            queryString = queryString + "                   UPDATE      DataMessageMaster SET PrintedUserID = @PrintedUserID, DataStatusID = @DataStatusID, EntryStatusID = " + (int)GlobalEnum.EntryStatusID.IsEdited + " WHERE DataMessageID = @DataMessageID " + "\r\n";
            SQLDatabase.CreateStoredProcedure("DataMessageMasterUpdateDataStatus", queryString);





            queryString = " @DataMessageID int " + "\r\n";
            queryString = queryString + " WITH ENCRYPTION " + "\r\n";
            queryString = queryString + " AS " + "\r\n";
            queryString = queryString + "       SELECT      DataMessageID, SerialID, SendDate, SendType, SendMessage, CounterValueBefore, CounterValueAfter, CounterAutonicsBefore, CounterAutonicsAfter, Description, Remarks " + "\r\n";
            queryString = queryString + "       FROM        DataMessageDetail " + "\r\n";
            queryString = queryString + "       WHERE       DataMessageID = @DataMessageID " + "\r\n";
            queryString = queryString + "       ORDER BY    SendDate DESC, SerialID DESC " + "\r\n";

            SQLDatabase.CreateStoredProcedure("DataMessageDetailSelect", queryString);


            queryString = " @DataMessageID int, @SendDate datetime, @SendType nvarchar(300), @SendMessage nvarchar(300), @CounterValueBefore float, @CounterValueAfter float, @CounterAutonicsBefore float, @CounterAutonicsAfter float, @Description nvarchar(100), @Remarks nvarchar(100) " + "\r\n";
            queryString = queryString + " WITH ENCRYPTION " + "\r\n";
            queryString = queryString + " AS " + "\r\n";
            queryString = queryString + "       INSERT INTO DataMessageDetail (DataMessageID, SendDate, SendType, SendMessage, CounterValueBefore, CounterValueAfter, CounterAutonicsBefore, CounterAutonicsAfter, Description, Remarks, EntryStatusID) VALUES (@DataMessageID, @SendDate, @SendType, @SendMessage, @CounterValueBefore, @CounterValueAfter, @CounterAutonicsBefore, @CounterAutonicsAfter, @Description, @Remarks, " + (int)GlobalEnum.EntryStatusID.IsNew + " ) " + "\r\n";
            queryString = queryString + "       SELECT      DataMessageID, SerialID, SendDate, SendType, SendMessage, CounterValueBefore, CounterValueAfter, CounterAutonicsBefore, CounterAutonicsAfter, Description, Remarks FROM DataMessageDetail WHERE DataMessageID = @DataMessageID AND SerialID = SCOPE_IDENTITY() " + "\r\n";

            SQLDatabase.CreateStoredProcedure("DataMessageDetailInsert", queryString);



            queryString = " @DataMessageID int, @SerialID int, @SendDate datetime, @SendType nvarchar(300), @SendMessage nvarchar(300), @CounterValueBefore float, @CounterValueAfter float, @CounterAutonicsBefore float, @CounterAutonicsAfter float, @Description nvarchar(100), @Remarks nvarchar(100) " + "\r\n";
            queryString = queryString + " WITH ENCRYPTION " + "\r\n";
            queryString = queryString + " AS " + "\r\n";
            queryString = queryString + "       UPDATE      DataMessageDetail SET SendDate = @SendDate, SendType = @SendType, SendMessage = @SendMessage, CounterValueBefore = @CounterValueBefore, CounterValueAfter = @CounterValueAfter, CounterAutonicsBefore = @CounterAutonicsBefore, CounterAutonicsAfter = @CounterAutonicsAfter, Description = @Description, Remarks = @Remarks, EntryStatusID = " + (int)GlobalEnum.EntryStatusID.IsEdited + "  WHERE DataMessageID = @DataMessageID AND SerialID = @SerialID " + "\r\n";
            queryString = queryString + "       SELECT      DataMessageID, SerialID, SendDate, SendType, SendMessage, CounterValueBefore, CounterValueAfter, CounterAutonicsBefore, CounterAutonicsAfter, Description, Remarks FROM DataMessageDetail WHERE DataMessageID = @DataMessageID AND SerialID = @SerialID " + "\r\n";

            SQLDatabase.CreateStoredProcedure("DataMessageDetailUpdate", queryString);



            queryString = " @DataMessageID int " + "\r\n";
            queryString = queryString + " WITH ENCRYPTION " + "\r\n";
            queryString = queryString + " AS " + "\r\n";
            queryString = queryString + "       DELETE FROM DataMessageDetail WHERE DataMessageID = @DataMessageID " + "\r\n";
            SQLDatabase.CreateStoredProcedure("DataMessageDetailDelete", queryString);





            queryString = " @DataMessageID int " + "\r\n";
            queryString = queryString + " WITH ENCRYPTION " + "\r\n";
            queryString = queryString + " AS " + "\r\n";
            queryString = queryString + "       SELECT DataMessageID, EntryDate, CounterValue, CounterAutonics FROM DataMessageCounterLog WHERE DataMessageID = @DataMessageID " + "\r\n";
            SQLDatabase.CreateStoredProcedure("DataMessageCounterLogSelect", queryString);



            queryString = " @DataMessageID int, @EntryDate datetime, @CounterValue float, @CounterAutonics float " + "\r\n";
            queryString = queryString + " WITH ENCRYPTION " + "\r\n";
            queryString = queryString + " AS " + "\r\n";
            queryString = queryString + "       INSERT INTO DataMessageCounterLog (DataMessageID, EntryDate, CounterValue, CounterAutonics, EntryStatusID) VALUES (@DataMessageID, @EntryDate, @CounterValue, @CounterAutonics, " + (int)GlobalEnum.EntryStatusID.IsNew + " ) " + "\r\n";
            queryString = queryString + "       SELECT DataMessageID, EntryDate, CounterValue, CounterAutonics FROM DataMessageCounterLog WHERE DataMessageID = @DataMessageID AND EntryDate = @EntryDate " + "\r\n";
            SQLDatabase.CreateStoredProcedure("DataMessageCounterLogInsert", queryString);



            queryString = " @DataMessageID int, @EntryDate datetime, @CounterValue float, @CounterAutonics float " + "\r\n";
            queryString = queryString + " WITH ENCRYPTION " + "\r\n";
            queryString = queryString + " AS " + "\r\n";
            queryString = queryString + "       UPDATE DataMessageCounterLog SET CounterValue = @CounterValue, CounterAutonics = @CounterAutonics, EntryStatusID = " + (int)GlobalEnum.EntryStatusID.IsEdited + " WHERE DataMessageID = @DataMessageID AND EntryDate = @EntryDate  " + "\r\n";
            queryString = queryString + "       SELECT DataMessageID, EntryDate, CounterValue, CounterAutonics FROM DataMessageCounterLog WHERE DataMessageID = @DataMessageID AND EntryDate = @EntryDate " + "\r\n";
            SQLDatabase.CreateStoredProcedure("DataMessageCounterLogUpdate", queryString);



            queryString = " @DataMessageID int " + "\r\n";
            queryString = queryString + " WITH ENCRYPTION " + "\r\n";
            queryString = queryString + " AS " + "\r\n";
            queryString = queryString + "       DELETE FROM DataMessageCounterLog WHERE DataMessageID = @DataMessageID " + "\r\n";
            SQLDatabase.CreateStoredProcedure("DataMessageCounterLogDelete", queryString);


            /// <summary>
            /// Check for editable
            /// </summary>
            queryArray = new string[0];
            //queryArray[0] = " SELECT TOP 1 MarketingIncentiveID FROM MarketingIncentiveMaster WHERE MarketingIncentiveID = @FindIdentityID AND Approved = 1 ";

            SQLDatabase.CreateProcedureToCheckExisting("DataMessageMasterApproved", queryArray);



            /// <summary>
            /// Check for editable
            /// </summary>
            queryArray = new string[1];
            queryArray[0] = " SELECT TOP 1 DataMessageID FROM DataMessageDetail WHERE DataMessageID = @FindIdentityID ";

            SQLDatabase.CreateProcedureToCheckExisting("DataMessageMasterEditable", queryArray);


        }


        private void a()
        {
            string queryString = "";

            queryString = "     @DataMessageID Int " + "\r\n";
            queryString = queryString + " WITH ENCRYPTION " + "\r\n";
            queryString = queryString + " AS " + "\r\n";
            queryString = queryString + "       SELECT      DataMessageID, ForeignMessageID, ProductionDate, ProductionDatePrintable, EntryDate, BeginingDate, EndingDate, Verified, VerifiedDate, LogoID, LogoName, LogoLogo, LogoPrintable, FactoryID, FactoryName, FactoryLogo, FactoryPrintable, OwnerID, OwnerName, OwnerLogo, OwnerPrintable, CategoryID, CategoryName, CategoryLogo, CategoryPrintable, ProductID, ProductName, ProductLogo, ProductPrintable, CoilID, CoilCode, CoilExtension, CoilPrintable, CounterValue, CounterAutonics, Remarks, DataStatusID, RequestedEmployeeID, UserID, UserOrganizationID, EntryStatusID " + "\r\n";
            queryString = queryString + "       FROM        DataMessageMaster " + "\r\n";
            queryString = queryString + "       WHERE       DataMessageID = @DataMessageID " + "\r\n";

            SQLDatabase.CreateStoredProcedure("DataMessageMasterSelect", queryString);


            queryString = "     @ForeignMessageID int, @ProductionDate datetime, @ProductionDatePrintable bit, @EntryDate datetime, @BeginingDate datetime, @EndingDate datetime, @Verified bit, @VerifiedDate datetime, @LogoID int, @LogoName nvarchar(100), @LogoLogo nvarchar(100), @LogoPrintable bit, @FactoryID int, @FactoryName nvarchar(100), @FactoryLogo nvarchar(100), @FactoryPrintable bit, @OwnerID int, @OwnerName nvarchar(100), @OwnerLogo nvarchar(100), @OwnerPrintable bit, @CategoryID int, @CategoryName nvarchar(100), @CategoryLogo nvarchar(100), @CategoryPrintable bit, @ProductID int, @ProductName nvarchar(100), @ProductLogo nvarchar(100), @ProductPrintable bit, @CoilID int, @CoilCode nvarchar(100), @CoilExtension int, @CoilPrintable bit, @CounterValue float, @CounterAutonics float, @Remarks nvarchar(200), @DataStatusID int, @RequestedEmployeeID int, @UserID int, @UserOrganizationID int, @EntryStatusID int " + "\r\n";
            queryString = queryString + " WITH ENCRYPTION " + "\r\n";
            queryString = queryString + " AS " + "\r\n";
            queryString = queryString + "       INSERT INTO     DataMessageMaster (ForeignMessageID, ProductionDate, ProductionDatePrintable, EntryDate, BeginingDate, EndingDate, Verified, VerifiedDate, LogoID, LogoName, LogoLogo, LogoPrintable, FactoryID, FactoryName, FactoryLogo, FactoryPrintable, OwnerID, OwnerName, OwnerLogo, OwnerPrintable, CategoryID, CategoryName, CategoryLogo, CategoryPrintable, ProductID, ProductName, ProductLogo, ProductPrintable, CoilID, CoilCode, CoilExtension, CoilPrintable, CounterValue, CounterAutonics, Remarks, DataStatusID, RequestedEmployeeID, UserID, UserOrganizationID, EntryStatusID) VALUES (@ForeignMessageID, @ProductionDate, @ProductionDatePrintable, @EntryDate, @BeginingDate, @EndingDate, @Verified, @VerifiedDate, @LogoID, @LogoName, @LogoLogo, @LogoPrintable, @FactoryID, @FactoryName, @FactoryLogo, @FactoryPrintable, @OwnerID, @OwnerName, @OwnerLogo, @OwnerPrintable, @CategoryID, @CategoryName, @CategoryLogo, @CategoryPrintable, @ProductID, @ProductName, @ProductLogo, @ProductPrintable, @CoilID, @CoilCode, @CoilExtension, @CoilPrintable, @CounterValue, @CounterAutonics, @Remarks, @DataStatusID, @RequestedEmployeeID, @UserID, @UserOrganizationID, @EntryStatusID) " + "\r\n";
            queryString = queryString + "       SELECT          DataMessageID, ForeignMessageID, ProductionDate, ProductionDatePrintable, EntryDate, BeginingDate, EndingDate, Verified, VerifiedDate, LogoID, LogoName, LogoLogo, LogoPrintable, FactoryID, FactoryName, FactoryLogo, FactoryPrintable, OwnerID, OwnerName, OwnerLogo, OwnerPrintable, CategoryID, CategoryName, CategoryLogo, CategoryPrintable, ProductID, ProductName, ProductLogo, ProductPrintable, CoilID, CoilCode, CoilExtension, CoilPrintable, CounterValue, CounterAutonics, Remarks, DataStatusID, RequestedEmployeeID, UserID, UserOrganizationID, EntryStatusID FROM DataMessageMaster WHERE DataMessageID = SCOPE_IDENTITY() " + "\r\n";

            SQLDatabase.CreateStoredProcedure("DataMessageMasterInsert", queryString);


            queryString = "     @DataMessageID int,	@ForeignMessageID int, @ProductionDate datetime, @ProductionDatePrintable bit, @EntryDate datetime, @BeginingDate datetime, @EndingDate datetime, @Verified bit, @VerifiedDate datetime, @LogoID int, @LogoName nvarchar(100), @LogoLogo nvarchar(100), @LogoPrintable bit, @FactoryID int, @FactoryName nvarchar(100), @FactoryLogo nvarchar(100), @FactoryPrintable bit, @OwnerID int, @OwnerName nvarchar(100), @OwnerLogo nvarchar(100), @OwnerPrintable bit, @CategoryID int, @CategoryName nvarchar(100), @CategoryLogo nvarchar(100), @CategoryPrintable bit, @ProductID int, @ProductName nvarchar(100), @ProductLogo nvarchar(100), @ProductPrintable bit, @CoilID int, @CoilCode nvarchar(100), @CoilExtension int, @CoilPrintable bit, @CounterValue float, @CounterAutonics float, @Remarks nvarchar(200), @DataStatusID int, @RequestedEmployeeID int, @UserID int, @UserOrganizationID int, @EntryStatusID int " + "\r\n";
            queryString = queryString + " WITH ENCRYPTION " + "\r\n";
            queryString = queryString + " AS " + "\r\n";
            queryString = queryString + "       UPDATE      DataMessageMaster SET ForeignMessageID = @ForeignMessageID, ProductionDate = @ProductionDate, ProductionDatePrintable = @ProductionDatePrintable, EntryDate = @EntryDate, BeginingDate = @BeginingDate, EndingDate = @EndingDate, Verified = @Verified, VerifiedDate = @VerifiedDate, LogoID = @LogoID, LogoName = @LogoName, LogoLogo = @LogoLogo, LogoPrintable = @LogoPrintable, FactoryID = @FactoryID, FactoryName = @FactoryName, FactoryLogo = @FactoryLogo, FactoryPrintable = @FactoryPrintable, OwnerID = @OwnerID, OwnerName = @OwnerName, OwnerLogo = @OwnerLogo, OwnerPrintable = @OwnerPrintable, CategoryID = @CategoryID, CategoryName = @CategoryName, CategoryLogo = @CategoryLogo, CategoryPrintable = @CategoryPrintable, ProductID = @ProductID, ProductName = @ProductName, ProductLogo = @ProductLogo, ProductPrintable = @ProductPrintable, CoilID = @CoilID, CoilCode = @CoilCode, CoilExtension = @CoilExtension, CoilPrintable = @CoilPrintable, CounterValue = @CounterValue, CounterAutonics = @CounterAutonics, Remarks = @Remarks, DataStatusID = @DataStatusID, RequestedEmployeeID = @RequestedEmployeeID, UserID = @UserID, UserOrganizationID = @UserOrganizationID, EntryStatusID = @EntryStatusID WHERE DataMessageID = @DataMessageID " + "\r\n";
            queryString = queryString + "       SELECT      DataMessageID, ForeignMessageID, ProductionDate, ProductionDatePrintable, EntryDate, BeginingDate, EndingDate, Verified, VerifiedDate, LogoID, LogoName, LogoLogo, LogoPrintable, FactoryID, FactoryName, FactoryLogo, FactoryPrintable, OwnerID, OwnerName, OwnerLogo, OwnerPrintable, CategoryID, CategoryName, CategoryLogo, CategoryPrintable, ProductID, ProductName, ProductLogo, ProductPrintable, CoilID, CoilCode, CoilExtension, CoilPrintable, CounterValue, CounterAutonics, Remarks, DataStatusID, RequestedEmployeeID, UserID, UserOrganizationID, EntryStatusID FROM DataMessageMaster WHERE DataMessageID = @DataMessageID " + "\r\n";

            SQLDatabase.CreateStoredProcedure("DataMessageMasterUpdate", queryString);


            queryString = "     @DataMessageID int ";
            queryString = queryString + " WITH ENCRYPTION " + "\r\n";
            queryString = queryString + " AS " + "\r\n";
            queryString = queryString + "       DELETE FROM DataMessageMaster WHERE DataMessageID = @DataMessageID " + "\r\n";
            SQLDatabase.CreateStoredProcedure("DataMessageMasterDelete", queryString);

        }
    }
}
