using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

using Global.Class.Library;
using DataAccessLayer;
using DataTransferObject;

namespace BusinessLogicLayer
{
    public class FillingLineData : NotifyPropertyChangeObject
    {

        private int productID;
        private string productCode;
        private string productCodeOriginal;

        private string batchNo;
        private DateTime settingDate;
        private int settingMonthID;


        private string batchSerialNumber;
        private string monthSerialNumber;
        private string batchCartonNumber;
        private string monthCartonNumber;
        private string remarks;


        #region Contructor

        public FillingLineData()
        {

            DataTable defaultFillingLineData = ADODatabase.GetDataTable("SELECT FillingLineData.ProductID, ListProductName.ProductCode, ListProductName.ProductCodeOriginal, ListProductName.NoItemPerCarton, FillingLineData.BatchNo, FillingLineData.SettingDate, FillingLineData.SettingMonthID, FillingLineData.BatchSerialNumber, FillingLineData.MonthSerialNumber, FillingLineData.BatchCartonNumber, FillingLineData.MonthCartonNumber FROM FillingLineData INNER JOIN ListProductName ON FillingLineData.ProductID = ListProductName.ProductID WHERE FillingLineData.FillingLineID = " + (int)this.FillingLineID + " AND FillingLineData.IsDefault = 1");

            if (defaultFillingLineData.Rows.Count > 0)
            {
                this.StartTracking();

                this.ProductID = int.Parse(defaultFillingLineData.Rows[0]["ProductID"].ToString());
                this.ProductCode = defaultFillingLineData.Rows[0]["ProductCode"].ToString();
                this.ProductCodeOriginal = defaultFillingLineData.Rows[0]["ProductCodeOriginal"].ToString();

                GlobalVariables.noItemPerCartonSetByProductID = int.Parse(defaultFillingLineData.Rows[0]["NoItemPerCarton"].ToString());




                int noItem = 0;
                GlobalVariables.noItemPerCartonSetByProductID = int.TryParse(GlobalRegistry.Read("NoItemPerCartonSetByProductID"), out noItem) ? noItem : 0;






                this.BatchNo = defaultFillingLineData.Rows[0]["BatchNo"].ToString();

                this.SettingDate = DateTime.Parse(defaultFillingLineData.Rows[0]["SettingDate"].ToString());
                this.SettingMonthID = int.Parse(defaultFillingLineData.Rows[0]["SettingMonthID"].ToString());

                this.BatchSerialNumber = defaultFillingLineData.Rows[0]["BatchSerialNumber"].ToString();
                this.MonthSerialNumber = defaultFillingLineData.Rows[0]["MonthSerialNumber"].ToString();

                this.BatchCartonNumber = defaultFillingLineData.Rows[0]["BatchCartonNumber"].ToString();
                this.MonthCartonNumber = defaultFillingLineData.Rows[0]["MonthCartonNumber"].ToString();

                this.StartTracking();
            }


        }

        #endregion Contructor


        #region Public Properties


        public GlobalVariables.FillingLine FillingLineID
        {
            get { return GlobalVariables.FillingLineID; }

        }

        public string FillingLineCode
        {
            get { return GlobalVariables.FillingLineCode; }
        }

        public string FillingLineName
        {
            get { return GlobalVariables.FillingLineName; }
        }


        public int ProductID    //ResetSerialNumber
        {
            get { return this.productID; }
            set
            {
                if (this.productID != value)
                {
                    ApplyPropertyChange<FillingLineData, int>(ref this.productID, o => o.ProductID, value);

                    DataTable dataTableFillingLineData = SQLDatabase.GetDataTable("SELECT BatchNo, BatchSerialNumber, MonthSerialNumber, BatchCartonNumber, MonthCartonNumber FROM FillingLineData WHERE FillingLineID = " + (int)this.FillingLineID + " AND ProductID = " + this.ProductID + " AND SettingMonthID = " + this.SettingMonthID);
                    if (dataTableFillingLineData.Rows.Count > 0)
                        this.ResetSerialNumber(dataTableFillingLineData.Rows[0]["BatchNo"].ToString() == this.BatchNo ? dataTableFillingLineData.Rows[0]["BatchSerialNumber"].ToString() : "000001", dataTableFillingLineData.Rows[0]["MonthSerialNumber"].ToString(), dataTableFillingLineData.Rows[0]["BatchNo"].ToString() == this.BatchNo ? dataTableFillingLineData.Rows[0]["BatchCartonNumber"].ToString() : "900001", dataTableFillingLineData.Rows[0]["MonthCartonNumber"].ToString());
                    else
                        this.ResetSerialNumber("000001", "000001", "900001", "900001");
                }
            }
        }


        public string ProductCode
        {
            get { return this.productCode; }
            set { ApplyPropertyChange<FillingLineData, string>(ref this.productCode, o => o.ProductCode, value); }
        }

        public string ProductCodeOriginal
        {
            get { return this.productCodeOriginal; }
            set { ApplyPropertyChange<FillingLineData, string>(ref this.productCodeOriginal, o => o.ProductCodeOriginal, value); }
        }


        //-------------------------


        public string BatchNo   //ResetSerialNumber
        {
            get { return this.batchNo; }
            set
            {
                if (this.batchNo != value)
                {
                    if (value.Length == 8)
                    {
                        ApplyPropertyChange<FillingLineData, string>(ref this.batchNo, o => o.BatchNo, value);

                        DataTable dataTableFillingLineData = SQLDatabase.GetDataTable("SELECT BatchSerialNumber, BatchCartonNumber FROM FillingLineData WHERE FillingLineID = " + (int)this.FillingLineID + " AND ProductID = " + this.ProductID + " AND SettingMonthID = " + this.SettingMonthID + " AND BatchNo = " + this.BatchNo);
                        if (dataTableFillingLineData.Rows.Count > 0)
                            this.ResetSerialNumber(dataTableFillingLineData.Rows[0]["BatchSerialNumber"].ToString(), this.MonthSerialNumber, dataTableFillingLineData.Rows[0]["BatchCartonNumber"].ToString() , this.MonthCartonNumber);
                        else
                            this.ResetSerialNumber("000001", this.MonthSerialNumber, "900001", this.MonthCartonNumber);
                    }
                    else
                    {
                        throw new System.InvalidOperationException("NMVN: Invalid batch number format.");
                    }
                }
            }
        }


        public DateTime SettingDate
        {
            get { return this.settingDate; }
            set
            {
                ApplyPropertyChange<FillingLineData, DateTime>(ref this.settingDate, o => o.SettingDate, value);
                this.SettingMonthID = GlobalStaticFunction.DateToContinuosMonth(this.SettingDate);
            }
        }

        public int SettingMonthID   //ResetSerialNumber
        {
            get { return this.settingMonthID; }
            set
            {
                if (this.settingMonthID != value)
                {
                    ApplyPropertyChange<FillingLineData, int>(ref this.settingMonthID, o => o.SettingMonthID, value);
                    this.ResetSerialNumber(this.BatchSerialNumber, "000001", this.BatchCartonNumber, "900001");
                }
            }
        }

        //-------------------------

        public string BatchSerialNumber
        {
            get { return this.batchSerialNumber; }

            set
            {
                if (value != this.batchSerialNumber)
                {
                    int intValue = 0;
                    if (int.TryParse(value, out intValue) && value.Length == 6)
                    {
                        ApplyPropertyChange<FillingLineData, string>(ref this.batchSerialNumber, o => o.BatchSerialNumber, value);
                    }
                    else
                    {
                        throw new System.InvalidOperationException("NMVN: Invalid serial number format.");
                    }
                }
            }
        }

        public string MonthSerialNumber
        {
            get { return this.monthSerialNumber; }

            set
            {
                if (value != this.monthSerialNumber)
                {
                    int intValue = 0;
                    if (int.TryParse(value, out intValue) && value.Length == 6)
                    {
                        ApplyPropertyChange<FillingLineData, string>(ref this.monthSerialNumber, o => o.MonthSerialNumber, value);
                    }
                    else
                    {
                        throw new System.InvalidOperationException("NMVN: Invalid serial number format.");
                    }
                }
            }
        }

        public string BatchCartonNumber
        {
            get { return this.batchCartonNumber; }

            set
            {
                if (value != this.batchCartonNumber)
                {
                    int intValue = 0;
                    if (int.TryParse(value, out intValue) && value.Length == 6 && value.Substring(0, 1) == "9")
                    {
                        ApplyPropertyChange<FillingLineData, string>(ref this.batchCartonNumber, o => o.BatchCartonNumber, value);
                    }
                    else
                    {
                        throw new System.InvalidOperationException("NMVN: Invalid serial number format.");
                    }
                }
            }
        }

        public string MonthCartonNumber
        {
            get { return this.monthCartonNumber; }

            set
            {
                if (value != this.monthCartonNumber)
                {
                    int intValue = 0;
                    if (int.TryParse(value, out intValue) && value.Length == 6 && value.Substring(0, 1) == "9")
                    {
                        ApplyPropertyChange<FillingLineData, string>(ref this.monthCartonNumber, o => o.MonthCartonNumber, value);
                    }
                    else
                    {
                        throw new System.InvalidOperationException("NMVN: Invalid serial number format.");
                    }
                }
            }
        }


        //-------------------------



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
                    this.NotifyPropertyChanged("Remarks");
                }
            }
        }

        #endregion Public Properties

        #region Method

        public FillingLineData ShallowClone()
        {
            return (FillingLineData)this.MemberwiseClone();
        }

        private void ResetSerialNumber(string batchSerialNumber, string monthSerialNumber, string batchCartonNumber, string monthCartonNumber)
        {
            if (this.BatchSerialNumber != batchSerialNumber) this.BatchSerialNumber = batchSerialNumber;
            if (this.MonthSerialNumber != monthSerialNumber) this.MonthSerialNumber = monthSerialNumber;
            if (this.BatchCartonNumber != batchCartonNumber) this.BatchCartonNumber = batchCartonNumber;
            if (this.MonthCartonNumber != monthCartonNumber) this.MonthCartonNumber = monthCartonNumber;
        }

        public bool DataValidated()
        {
            return this.FillingLineID != 0 && this.ProductID != 0 && this.BatchNo != "" & this.BatchSerialNumber != "" & this.MonthSerialNumber != "" & this.BatchCartonNumber != "" & this.MonthCartonNumber != "";
        }

        public bool Update()
        {
            try
            {
                int rowsAffected = ADODatabase.ExecuteTransaction("UPDATE FillingLineData SET IsDefault = 0 WHERE FillingLineID = " + (int)this.FillingLineID + "; " +
                                                                  "UPDATE FillingLineData SET BatchNo = N'" + this.BatchNo.ToString() + "', " +
                                                                                            " SettingDate = CONVERT(smalldatetime, '" + this.SettingDate.ToString("dd/MM/yyyy") + "', 103), " +
                                                                                            " SettingMonthID = " + this.SettingMonthID.ToString() + ", " +
                                                                                            " BatchSerialNumber = N'" + this.BatchSerialNumber.ToString() + "', " +
                                                                                            " MonthSerialNumber = N'" + this.MonthSerialNumber.ToString() + "', " +
                                                                                            " BatchCartonNumber = N'" + this.BatchCartonNumber.ToString() + "', " +
                                                                                            " MonthCartonNumber = N'" + this.MonthCartonNumber.ToString() + "', " +
                                                                                            " LastSerialDate = GetDate(), IsDefault = 1 " +
                                                                  "WHERE FillingLineID   =  " + (int)this.FillingLineID + " AND ProductID = " + this.ProductID);
                return rowsAffected > 0;
            }
            catch (Exception exception)
            {
                throw exception;
            }
        }

        public bool Save()
        {
            try
            {
                int rowsAffected = ADODatabase.ExecuteNonQuery("UPDATE FillingLineData SET IsDefault = 1 WHERE FillingLineID = " + (int)this.FillingLineID + " AND ProductID = " + this.ProductID);


                if (rowsAffected <= 0) //Add New
                {
                    rowsAffected = ADODatabase.ExecuteTransaction("UPDATE FillingLineData SET IsDefault = 0 WHERE FillingLineID = " + (int)this.FillingLineID + "; " +
                                                                  "INSERT INTO FillingLineData (FillingLineID, ProductID, BatchNo, SettingDate, SettingMonthID, BatchSerialNumber, MonthSerialNumber, BatchCartonNumber, MonthCartonNumber, Remarks, LastSettingDate, LastSerialDate, IsDefault) " +
                                                                  "VALUES (" + (int)this.FillingLineID + ", " + this.ProductID + ", N'" + this.BatchNo + "', CONVERT(smalldatetime, '" + this.SettingDate.ToString("dd/MM/yyyy") + "',103), " + this.SettingMonthID.ToString() + ", N'" + this.BatchSerialNumber + "', N'" + this.MonthSerialNumber + "', N'" + this.BatchCartonNumber + "', N'" + this.MonthCartonNumber + "', N'" + this.Remarks + "', GetDate(), GetDate(), 1) ");

                    return rowsAffected > 0;
                }

                else //Update Only
                {
                    return Update();
                }
            }
            catch (Exception exception)
            {
                throw exception;
            }
        }

        #endregion Method
    }
}
