using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Global.Class.Library
{
    public static class GlobalEnum
    {
        public enum TaskID
        {
            EmployeeCategory = 1,
            EmployeeType = 2,
            EmployeeName = 3,



            ItemCategory = 111,
            ItemType = 112,
            ItemClass = 116,
            ItemBrand = 113,
            ItemPM_APM = 117,
            ItemGroup = 114,
            ItemCommodity = 115,


            CustomerCategory = 251,
            CustomerType = 252,
            CustomerChannel = 253,
            CustomerName = 254,
            CustomerGroup = 255,


            MarketingProgram = 109,
            MarketingProgramVerifiable = 1098888,
            MarketingProgramUnverifiable = 109888899,


            DataMessage = 121,
            DataMessageVerifiable = 1218888,
            DataMessageUnverifiable = 121888899,

            MarketingPayment = 131,
            MarketingPaymentVerifiable = 1318888,
            MarketingPaymentUnverifiable = 131888899,

            MarketingMonitoring = 151,
            MarketingMonitoringVerifiable = 1518888,
            MarketingMonitoringUnverifiable = 151888899,

            ListLogo = 888101,
            ListLogoVerifiable = 88810188,
            ListLogoUnverifiable = 88810189,

            ListFactory = 888102,
            ListFactoryVerifiable = 88810288,
            ListFactoryUnverifiable = 88810289,

            ListOwner = 888103,
            ListOwnerVerifiable = 88810388,
            ListOwnerUnverifiable = 88810389,

            ListCategory = 888104,
            ListCategoryVerifiable = 88810488,
            ListCategoryUnverifiable = 88810489,

            ListProduct = 888105,
            ListProductVerifiable = 88810588,
            ListProductUnverifiable = 88810589,

            ListCoil = 888106,
            ListCoilVerifiable = 88810688,
            ListCoilUnverifiable = 88810689,

            ListEmployee = 888107,
            ListEmployeeVerifiable = 88810788,
            ListEmployeeUnverifiable = 88810789
        }


        public enum ImageS8Command
        {
            CheckPrinterReady = 5, //05H
            CheckPrinterFault = 59, //3BH
            SendMessage = 12, //0CH
            SetupCounter = 56, //38H
            SetCounterValue = 81, //51H
            RequestCounterValue = 57 //39H
        }

        public enum DataStatusID
        {
            NotPrintedYet = 1,
            OnPrinting = 2,
            WaitForPrint = 3,
            PrintFinished = 4
        }

        public enum EntryStatusID
        {
            IsUploaded = 0,
            IsNew = 1,
            IsEdited = 9
        }
    }
}
