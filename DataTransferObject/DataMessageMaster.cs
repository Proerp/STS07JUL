using System;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel;
using System.Text;
using System.Drawing;
using System.Reflection;
using System.IO;

using Global.Class.Library;
using DataTransferObject;


namespace DataTransferObject
{

    public class DataMessageMaster : NotifyPropertyChangeObject
    {
        private byte[] dataStatusImage;

        private int dataMessageID;
        private int foreignMessageID;

        private int requestedEmployeeID;
        private int dataStatusID;

        private DateTime entryDate;

        private DateTime productionDate;
        private bool productionDatePrintable;

        private DateTime beginingDate;
        private DateTime endingDate;

        private bool verified;
        private DateTime verifiedDate;

        private int logoID;
        private string logoName;
        private string logoLogo;
        private bool logoPrintable;

        private int factoryID;
        private string factoryName;
        private string factoryLogo;
        private bool factoryPrintable;

        private int ownerID;
        private string ownerName;
        private string ownerLogo;
        private bool ownerPrintable;

        private int categoryID;
        private string categoryName;
        private string categoryLogo;
        private bool categoryPrintable;

        private int productID;
        private string productName;
        private string productLogo;
        private bool productPrintable;

        private int coilID;
        private string coilCode;
        private int coilExtension;
        private bool coilPrintable;

        private double counterValue;
        private double counterAutonics;
        private bool counterPrintable;

        private string remarks;


        public DataMessageMaster()
            : this(0, 0, 0, 0, false, DateTime.Now, DateTime.Now, false, DateTime.Now, DateTime.Now, DateTime.Now, 0, "", "", false, 0, "", "", false, 0, "", "", false, 0, "", "", false, 0, "", "", false, 0, "", 0, false, 0, 0, false, "")
        {
        }

        public DataMessageMaster(int dataMessageID, int foreignMessageID, int requestedEmployeeID, int dataStatusID, bool verified, DateTime verifiedDate, DateTime productionDate, bool productionDatePrintable, DateTime entryDate, DateTime beginingDate, DateTime endingDate, int logoID, string logoName, string logoLogo, bool logoPrintable, int factoryID, string factoryName, string factoryLogo, bool factoryPrintable, int ownerID, string ownerName, string ownerLogo, bool ownerPrintable, int categoryID, string categoryName, string categoryLogo, bool categoryPrintable, int productID, string productName, string productLogo, bool productPrintable, int coilID, string coilCode, int coilExtension, bool coilPrintable, double counterValue, double counterAutonics, bool counterPrintable, string remarks)
        {
            GlobalDefaultValue.Apply(this);

            this.StopTracking();

            this.DataMessageID = dataMessageID;
            this.ForeignMessageID = foreignMessageID;

            this.RequestedEmployeeID = requestedEmployeeID;
            this.DataStatusID = dataStatusID;

            this.ProductionDate = productionDate;
            this.ProductionDatePrintable = productionDatePrintable;
            this.EntryDate = entryDate;

            this.BeginingDate = beginingDate;
            this.EndingDate = endingDate;

            this.Verified = verified;
            this.VerifiedDate = verifiedDate;

            this.LogoID = logoID;
            this.LogoName = logoName;
            this.LogoLogo = logoLogo;
            this.LogoPrintable = logoPrintable;

            this.FactoryID = factoryID;
            this.FactoryName = factoryName;
            this.FactoryLogo = factoryLogo;
            this.FactoryPrintable = factoryPrintable;

            this.OwnerID = ownerID;
            this.OwnerName = ownerName;
            this.OwnerLogo = ownerLogo;
            this.OwnerPrintable = ownerPrintable;

            this.CategoryID = categoryID;
            this.CategoryName = categoryName;
            this.CategoryLogo = categoryLogo;
            this.CategoryPrintable = categoryPrintable;

            this.ProductID = productID;
            this.ProductName = productName;
            this.ProductLogo = productLogo;
            this.ProductPrintable = productPrintable;

            this.CoilID = coilID;
            this.CoilCode = coilCode;
            this.CoilExtension = coilExtension;
            this.CoilPrintable = coilPrintable;

            this.CounterValue = counterValue;
            this.CounterAutonics = counterAutonics;
            this.CounterPrintable = counterPrintable;

            this.Remarks = remarks;

            this.StartTracking();
            this.Reset();

        }



        #region Properties


        [DefaultValue(-1)]
        public int DataMessageID
        {
            get { return this.dataMessageID; }
            set { ApplyPropertyChange<DataMessageMaster, int>(ref this.dataMessageID, o => o.DataMessageID, value); }
        }

        [DefaultValue(-1)]
        public int ForeignMessageID
        {
            get { return this.foreignMessageID; }
            set { ApplyPropertyChange<DataMessageMaster, int>(ref this.foreignMessageID, o => o.ForeignMessageID, value); }
        }

        [DefaultValue(1)]
        public int RequestedEmployeeID
        {
            get { return this.requestedEmployeeID; }
            set { ApplyPropertyChange<DataMessageMaster, int>(ref this.requestedEmployeeID, o => o.RequestedEmployeeID, value); }
        }


        [DefaultValue(1)]
        public int DataStatusID
        {
            get { return this.dataStatusID; }
            set
            {
                if (this.dataStatusID != value)
                {
                    ApplyPropertyChange<DataMessageMaster, int>(ref this.dataStatusID, o => o.DataStatusID, value);
                    this.dataStatusImage = this.GetBytes(this.DataStatusID == (int)GlobalEnum.DataStatusID.OnPrinting ? ResourceIcon.OnPrinting : this.DataStatusID == (int)GlobalEnum.DataStatusID.WaitForPrint ? ResourceIcon.WaitForPrint : this.DataStatusID == (int)GlobalEnum.DataStatusID.PrintFinished ? ResourceIcon.PrintFinished : ResourceIcon.NullICon);
                }
                else
                    this.dataStatusImage = this.GetBytes(ResourceIcon.NullICon);
            }
        }


        [DefaultValue(false)]
        public bool Verified
        {
            get { return this.verified; }
            set { ApplyPropertyChange<DataMessageMaster, bool>(ref this.verified, o => o.Verified, value); }
        }

        [DefaultValue(typeof(DateTime), "01/01/1900")]
        public DateTime VerifiedDate
        {
            get { return this.verifiedDate; }
            set { ApplyPropertyChange<DataMessageMaster, DateTime>(ref this.verifiedDate, o => o.VerifiedDate, value); }
        }

        //[DefaultValue(-1)]
        public int LogoID
        {
            get { return this.logoID; }
            set { ApplyPropertyChange<DataMessageMaster, int>(ref this.logoID, o => o.LogoID, value); }
        }

        //[DefaultValue("")]
        public string LogoName
        {
            get { return this.logoName; }
            set { ApplyPropertyChange<DataMessageMaster, string>(ref this.logoName, o => o.LogoName, value.Trim()); }
        }

        //[DefaultValue("")]
        public string LogoLogo
        {
            get { return this.logoLogo; }
            set { ApplyPropertyChange<DataMessageMaster, string>(ref this.logoLogo, o => o.LogoLogo, value.Trim()); }
        }

        [DefaultValue(true)]
        public bool LogoPrintable
        {
            get { return this.logoPrintable; }
            set { ApplyPropertyChange<DataMessageMaster, bool>(ref this.logoPrintable, o => o.LogoPrintable, value); }
        }

        //[DefaultValue(-1)]
        public int FactoryID
        {
            get { return this.factoryID; }
            set { ApplyPropertyChange<DataMessageMaster, int>(ref this.factoryID, o => o.FactoryID, value); }
        }

        //[DefaultValue("")]
        public string FactoryName
        {
            get { return this.factoryName; }
            set { ApplyPropertyChange<DataMessageMaster, string>(ref this.factoryName, o => o.FactoryName, value.Trim()); }
        }

        //[DefaultValue("")]
        public string FactoryLogo
        {
            get { return this.factoryLogo; }
            set { ApplyPropertyChange<DataMessageMaster, string>(ref this.factoryLogo, o => o.FactoryLogo, value.Trim()); }
        }

        [DefaultValue(true)]
        public bool FactoryPrintable
        {
            get { return this.factoryPrintable; }
            set { ApplyPropertyChange<DataMessageMaster, bool>(ref this.factoryPrintable, o => o.FactoryPrintable, value); }
        }

        //[DefaultValue(-1)]
        public int OwnerID
        {
            get { return this.ownerID; }
            set { ApplyPropertyChange<DataMessageMaster, int>(ref this.ownerID, o => o.OwnerID, value); }
        }

        //[DefaultValue("")]
        public string OwnerName
        {
            get { return this.ownerName; }
            set { ApplyPropertyChange<DataMessageMaster, string>(ref this.ownerName, o => o.OwnerName, value); }
        }

        //[DefaultValue("")]
        public string OwnerLogo
        {
            get { return this.ownerLogo; }
            set { ApplyPropertyChange<DataMessageMaster, string>(ref this.ownerLogo, o => o.OwnerLogo, value); }
        }

        [DefaultValue(true)]
        public bool OwnerPrintable
        {
            get { return this.ownerPrintable; }
            set { ApplyPropertyChange<DataMessageMaster, bool>(ref this.ownerPrintable, o => o.OwnerPrintable, value); }
        }


        //[DefaultValue(-1)]
        public int CategoryID
        {
            get { return this.categoryID; }
            set { ApplyPropertyChange<DataMessageMaster, int>(ref this.categoryID, o => o.CategoryID, value); }
        }

        //[DefaultValue("")]
        public string CategoryName
        {
            get { return this.categoryName; }
            set { ApplyPropertyChange<DataMessageMaster, string>(ref this.categoryName, o => o.CategoryName, value); }
        }

        //[DefaultValue("")]
        public string CategoryLogo
        {
            get { return this.categoryLogo; }
            set { ApplyPropertyChange<DataMessageMaster, string>(ref this.categoryLogo, o => o.CategoryLogo, value); }
        }

        [DefaultValue(true)]
        public bool CategoryPrintable
        {
            get { return this.categoryPrintable; }
            set { ApplyPropertyChange<DataMessageMaster, bool>(ref this.categoryPrintable, o => o.CategoryPrintable, value); }
        }


        //[DefaultValue(-1)]
        public int ProductID
        {
            get { return this.productID; }
            set { ApplyPropertyChange<DataMessageMaster, int>(ref this.productID, o => o.ProductID, value); }
        }

        //[DefaultValue("")]
        public string ProductName
        {
            get { return this.productName; }
            set { ApplyPropertyChange<DataMessageMaster, string>(ref this.productName, o => o.ProductName, value); }
        }

        //[DefaultValue("")]
        public string ProductLogo
        {
            get { return this.productLogo; }
            set { ApplyPropertyChange<DataMessageMaster, string>(ref this.productLogo, o => o.ProductLogo, value); }
        }

        [DefaultValue(true)]
        public bool ProductPrintable
        {
            get { return this.productPrintable; }
            set { ApplyPropertyChange<DataMessageMaster, bool>(ref this.productPrintable, o => o.ProductPrintable, value); }
        }


        [DefaultValue(-1)]
        public int CoilID
        {
            get { return this.coilID; }
            set { ApplyPropertyChange<DataMessageMaster, int>(ref this.coilID, o => o.CoilID, value); }
        }

        [DefaultValue("")]
        public string CoilCode
        {
            get { return this.coilCode; }
            set { ApplyPropertyChange<DataMessageMaster, string>(ref this.coilCode, o => o.CoilCode, value); }
        }


        [DefaultValue(0)]
        public int CoilExtension
        {
            get { return this.coilExtension; }
            set { ApplyPropertyChange<DataMessageMaster, int>(ref this.coilExtension, o => o.CoilExtension, value); }
        }

        [DefaultValue(true)]
        public bool CoilPrintable
        {
            get { return this.coilPrintable; }
            set { ApplyPropertyChange<DataMessageMaster, bool>(ref this.coilPrintable, o => o.CoilPrintable, value); }
        }


        [DefaultValue(0)]
        public double CounterValue
        {
            get { return this.counterValue; }
            set { ApplyPropertyChange<DataMessageMaster, double>(ref this.counterValue, o => o.CounterValue, value); }
        }

        [DefaultValue(0)]
        public double CounterAutonics
        {
            get { return this.counterAutonics; }
            set { ApplyPropertyChange<DataMessageMaster, double>(ref this.counterAutonics, o => o.CounterAutonics, value); }
        }

        [DefaultValue(true)]
        public bool CounterPrintable
        {
            get { return this.counterPrintable; }
            set { ApplyPropertyChange<DataMessageMaster, bool>(ref this.counterPrintable, o => o.CounterPrintable, value); }
        }


        [DefaultValue(typeof(DateTime), "01/01/1900")]
        public DateTime ProductionDate
        {
            get { return this.productionDate; }
            set { ApplyPropertyChange<DataMessageMaster, DateTime>(ref this.productionDate, o => o.ProductionDate, value); }
        }

        [DefaultValue(true)]
        public bool ProductionDatePrintable
        {
            get { return this.productionDatePrintable; }
            set { ApplyPropertyChange<DataMessageMaster, bool>(ref this.productionDatePrintable, o => o.ProductionDatePrintable, value); }
        }

        [DefaultValue(typeof(DateTime), "01/01/1900")]
        public DateTime EntryDate
        {
            get { return this.entryDate; }
            set { ApplyPropertyChange<DataMessageMaster, DateTime>(ref this.entryDate, o => o.EntryDate, value); }
        }

        [DefaultValue(typeof(DateTime), "01/01/1900")]
        public DateTime BeginingDate
        {
            get { return this.beginingDate; }
            set { ApplyPropertyChange<DataMessageMaster, DateTime>(ref this.beginingDate, o => o.BeginingDate, value); }
        }

        [DefaultValue(typeof(DateTime), "01/01/1900")]
        public DateTime EndingDate
        {
            get { return this.endingDate; }
            set { ApplyPropertyChange<DataMessageMaster, DateTime>(ref this.endingDate, o => o.EndingDate, value); }
        }


        [DefaultValue("")]
        public string Remarks
        {
            get { return this.remarks; }
            set { ApplyPropertyChange<DataMessageMaster, string>(ref this.remarks, o => o.Remarks, value); }
        }


        public byte[] DataStatusImage
        { get { return this.dataStatusImage; } }


        private byte[] GetBytes(Icon icon)
        {
            if (icon != null)
            {
                MemoryStream ms = new MemoryStream();
                icon.Save(ms);
                return ms.ToArray();
            }
            else return null;
        }

        #endregion




        public string Display1
        { get { return this.DisplayImageS81(false); } }

        public string DisplayImageS81(bool imageS8)
        {
            if (!imageS8)
                return (this.LogoPrintable ? (this.LogoName != "" ? "[" + this.LogoName + "]" : "") : "");
            else// Bold: 2, Font: 38 (56)
                return (this.LogoPrintable ? this.ImageS8BlockTextCounterLogo("01", "38", this.LogoName, this.LogoLogo) : "");
        }

        public string Display2
        { get { return this.DisplayImageS82(false); } }

        public string DisplayImageS82(bool imageS8)
        {
            if (!imageS8)
                return (this.FactoryPrintable ? this.FactoryName + (this.FactoryName != "" ? " " : "") : "") + (this.CategoryPrintable ? this.CategoryName + (this.CategoryName != "" ? " " : "") : "") + (this.OwnerPrintable ? this.OwnerName + (this.OwnerName != "" ? " " : "") : "") + (this.ProductPrintable ? this.ProductName : "");
            else // Bold: 2, Font: 38 (56)
                return (this.FactoryPrintable ? this.ImageS8BlockTextCounterLogo("01", "38", this.FactoryName, this.FactoryLogo) + this.ImageS8BlockTextCounterLogo("01", "38", "-", "") : "") + (this.CategoryPrintable ? this.ImageS8BlockTextCounterLogo("01", "38", this.CategoryName, this.CategoryLogo) : "") + (this.OwnerPrintable ? this.ImageS8BlockTextCounterLogo("01", "38", "        " + this.OwnerName, this.OwnerLogo) : "") + (this.ProductPrintable ? this.ImageS8BlockTextCounterLogo("01", "38", this.ProductName, this.ProductLogo) : "");
        }

        public string Display3
        { get { return this.DisplayImageS83(false); } }

        public string DisplayImageS83(bool imageS8)
        {
            if (!imageS8)
                return (this.ProductionDatePrintable ? (this.ProductionDate > new DateTime(1900, 1, 1) ? DateTime.Now.ToString("dd/MM/yy") : "") : "");//this.ProductionDate.ToString("dd/MM/yy")
            else// Bold: 2, Font: 34 (52)
                return (this.ProductionDatePrintable ? this.ImageS8BlockTextCounterLogo("01", (this.ProductionDatePrintable && this.CoilPrintable ? "38" : "38"), DateTime.Now.ToString("dd/MM/yy"), "") : ""); //this.ProductionDate.ToString("dd/MM/yy")
            //REMARKS 21-APR-2016: CAU LENH SAU DAY LA IN 2 ROWS. CAU LENH NAY HOAN TOAN DUNG
            //return (this.ProductionDatePrintable ? this.ImageS8BlockTextCounterLogo("01", (this.ProductionDatePrintable && this.CoilPrintable ? "34" : "38"), this.ProductionDate.ToString("dd/MM/yy"), "") : "");
        }

        public string Display4
        { get { return this.DisplayImageS84(false); } }

        public string DisplayImageS84(bool imageS8)
        {
            if (!imageS8)
                return (this.CoilPrintable ? this.CoilCode : ""); //+ (this.CoilCode != "" ? "-" + this.CoilExtension.ToString("00") : "")
            else// Bold: 2, Font: 35 (53) ///// Newline: 0A
                return (this.CoilPrintable ? this.ImageS8BlockTextCounterLogo("01", (this.ProductionDatePrintable && this.CoilPrintable ? "38" : "38"), this.CoilCode, "") : "");//+ "-" + this.CoilExtension.ToString("00")
            //REMARKS 21-APR-2016: CAU LENH SAU DAY LA IN 2 ROWS. CAU LENH NAY HOAN TOAN DUNG
            //return (this.ProductionDatePrintable && this.CoilPrintable ? "0A," : "") + (this.CoilPrintable ? this.ImageS8BlockTextCounterLogo("01", (this.ProductionDatePrintable && this.CoilPrintable ? "35" : "38"), this.CoilCode + "-" + this.CoilExtension.ToString("00"), "") : "");
        }

        public string Display5
        { get { return this.DisplayImageS85(false); } }

        public string DisplayImageS85(bool imageS8)
        {
            if (!imageS8)
                return (this.CounterPrintable ? "[Counter]" : "");
            else// Bold: 2, Font: 38 (56), COUNTER: 1C
                return (this.CounterPrintable ? "02,38,1C," : "");
        }

        public string Display6
        { get { return this.DisplayImageS86(false); } }

        public string DisplayImageS86(bool imageS8)
        {
            if (!imageS8)
                return (this.CounterPrintable ? "MÉT" : "");
            else// Bold: 2, Font: 38 (56)
                return (this.CounterPrintable ? this.ImageS8BlockTextCounterLogo("01", "38", " MET", "") : "");
        }


        //Mr THẢO: LOGO: FF; THÉP: FE; SÓNG THẦN: FD; TÔN: FC; MẠ KẼM: FB; MẠ MÀU: FA; LẠNH: F9 
        //BLOCK TEXT/COUNTER/LOGO: [BOLD/SYMBOL/CONTENT]
        //BOLD: 01|02
        //SYMBOL: TEXT | COUNTER: 38: (LARGE - LINE 1); 34 (SMALL LINE 1); 35 (SMALL LINE 2)
        //SYMBOL: LOGO: FF, FE, FD, FC, ..
        //CONTENT: TEXT: ASCCI TEXT, COUNTER: 1C, LOGO: 21


        private string ImageS8BlockTextCounterLogo(string hexBold, string hexSymbol, string textDescription, string listLogoGenerator)
        {
            if (listLogoGenerator != "")
            {
                string[] arrayLogoGenerator = listLogoGenerator.Split(";".ToCharArray());
                string returnHexString = ""; int logoGeneratorNumber = 0; string logoHexContent = "21";

                string symbolGenerator = ""; string symbolNumber = "";

                foreach (string logoGenerator in arrayLogoGenerator)
                {

                    // LEMINHHIEP04NOV2017: IMAJES8: CLASSIC PRINTER. symbolNumber IS ALWAY 21H
                    // IMAJE 9040: MASTER, IP65 PRINTER => symbolNumber: WILL BE: SYMBOL 1: 21H, SYMBOL 2: 22H, ... ETC
                    // HERE: WE SET LOGO FOR IMAGE 9040 LIKE THIS: 250-1, 250-2, 250-3 ===> THE SYMBOL IS 1, 2, 3 => WE NEED TO MAKE LIKE THIS: '2' + SYMBOL => '21', '22', '23' (HEX)
                    if (logoGenerator.IndexOf("-") > 0)
                    {
                        symbolGenerator = logoGenerator.Substring(0, logoGenerator.IndexOf("-"));
                        symbolNumber = "2" + logoGenerator.Substring(logoGenerator.IndexOf("-") + 1); //'2' + SYMBOL => '21', '22', '23' (HEX)
                    }
                    else
                    {
                        symbolGenerator = logoGenerator;
                        symbolNumber = logoHexContent;
                    }


                    if (int.TryParse(symbolGenerator, out logoGeneratorNumber))
                        returnHexString = returnHexString + hexBold + "," + logoGeneratorNumber.ToString("X2") + "," + symbolNumber + "," + this.ImageS8BlockTextCounterLogo(hexBold, hexSymbol, " ", ""); //Add BLANK after logo
                }
                return returnHexString;
            }
            else
                if (textDescription != "")
                    return hexBold + "," + hexSymbol + "," + GlobalStaticFunction.TextToHEX(textDescription + (textDescription.Substring(textDescription.Length - 1, 1) == " " ? "" : " "));
                else
                    return "";
        }



        public DataMessageMaster ShallowClone()
        {
            return (DataMessageMaster)this.MemberwiseClone();
        }


        public bool CopyFrom(DataMessageMaster source)
        {
            PropertyInfo[] destinationProperties = this.GetType().GetProperties();
            foreach (PropertyInfo destinationPropertyInfo in destinationProperties)
            {
                if (destinationPropertyInfo.CanWrite)
                {
                    PropertyInfo sourcePropertyInfo = source.GetType().GetProperty(destinationPropertyInfo.Name);

                    destinationPropertyInfo.SetValue(this, sourcePropertyInfo.GetValue(source, null), null);
                }
            }

            return true;
        }


        protected override System.Collections.Generic.List<ValidationRule> CreateRules()
        {
            List<ValidationRule> validationRules = base.CreateRules();

            validationRules.Add(new SimpleValidationRule("RequestedEmployeeID", "Vui lòng chọn người lập.", delegate { return (this.RequestedEmployeeID > 0); }));
            validationRules.Add(new SimpleValidationRule("DataStatusID", "Vui lòng nhập tình trạng bản in.", delegate { return (this.DataStatusID > 0); }));
            validationRules.Add(new SimpleValidationRule("LogoID", "Vui lòng chọn logo.", delegate { return (this.LogoID > 0); }));
            validationRules.Add(new SimpleValidationRule("FactoryID", "Vui lòng chọn nhà máy.", delegate { return (this.FactoryID > 0); }));
            validationRules.Add(new SimpleValidationRule("OwnerID", "Vui lòng chọn chủ hàng.", delegate { return (this.OwnerID > 0); }));
            validationRules.Add(new SimpleValidationRule("CategoryID", "Vui lòng chọn loại sản phẩm.", delegate { return (this.CategoryID > 0); }));
            validationRules.Add(new SimpleValidationRule("ProductID", "Vui lòng chọn mã sản phẩm.", delegate { return (this.ProductID > 0); }));
            validationRules.Add(new SimpleValidationRule("CoilID", "Vui lòng chọn mã cuộn.", delegate { return (this.CoilID > 0); }));

            return validationRules;
        }
    }
}
