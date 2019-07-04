using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

using Global.Class.Library;
using DataAccessLayer;
using DataAccessLayer.ERmgrUIPTableAdapters;

using DataAccessLayer.ListMaintenanceTableAdapters;

namespace BusinessLogicLayer
{
    public class CommonMetaList
    {
        public ListMaintenance.ListEmployeeDataTable GetListEmployee()
        {
            return GetListEmployee(false);
        }

        public ListMaintenance.ListEmployeeDataTable GetListEmployee(bool withNewRow)
        {
            ListEmployeeTableAdapter listEmployeeTableAdapter = new ListEmployeeTableAdapter();
            ListMaintenance.ListEmployeeDataTable listEmployeeDataTable = listEmployeeTableAdapter.GetData();

            if (withNewRow) listEmployeeDataTable.AddListEmployeeRow("", DateTime.Now, "", "", -1, -1, (int)GlobalEnum.EntryStatusID.IsNew);
            return listEmployeeDataTable;
        }

        public bool CheckPasswordSuccessful(int employeeID, string password)
        {
            ListEmployeeTableAdapter listEmployeeTableAdapter = new ListEmployeeTableAdapter();
            ListMaintenance.ListEmployeeDataTable listEmployeeDataTable = listEmployeeTableAdapter.GetDataByEmployeeID(employeeID);
            if (listEmployeeDataTable.Rows.Count > 0)
                return listEmployeeDataTable.Rows[0]["Password"].ToString() == password;
            else
                return false;
        }

        public ListMaintenance.ListDataStatusDataTable GetListDataStatus()
        {
            return GetListDataStatus(false);
        }

        public ListMaintenance.ListDataStatusDataTable GetListDataStatus(bool withNewRow)
        {
            ListDataStatusTableAdapter listDataStatusTableAdapter = new ListDataStatusTableAdapter();
            ListMaintenance.ListDataStatusDataTable listDataStatusDataTable = listDataStatusTableAdapter.GetData();

            if (withNewRow) listDataStatusDataTable.AddListDataStatusRow("", DateTime.Now);
            return listDataStatusDataTable;
        }

        public ListMaintenance.ListDataStatusDataTable GetDataStatusByID(int dataStatusID)
        {
            ListDataStatusTableAdapter listDataStatusTableAdapter = new ListDataStatusTableAdapter();
            ListMaintenance.ListDataStatusDataTable listDataStatusDataTable = listDataStatusTableAdapter.GetDataByDataStatusID(dataStatusID);

            return listDataStatusDataTable;
        }

        public string GetDataStatusName(int dataStatusID)
        {
            ListMaintenance.ListDataStatusDataTable listDataStatusDataTable = this.GetDataStatusByID(dataStatusID);
            if (listDataStatusDataTable.Rows.Count > 0)
                return listDataStatusDataTable.Rows[0]["Description"].ToString();
            else
                return "";
        }

        public ListMaintenance.ListLogoDataTable GetListLogo()
        {
            return GetListLogo(false);
        }

        public ListMaintenance.ListLogoDataTable GetListLogo(bool withNewRow)
        {
            ListLogoTableAdapter listLogoTableAdapter = new ListLogoTableAdapter();
            ListMaintenance.ListLogoDataTable listLogoDataTable = listLogoTableAdapter.GetData();

            if (withNewRow) listLogoDataTable.AddListLogoRow("", "", DateTime.Now);
            return listLogoDataTable;
        }

        public ListMaintenance.ListFactoryDataTable GetListFactory()
        {
            return GetListFactory(false);
        }

        public ListMaintenance.ListFactoryDataTable GetListFactory(bool withNewRow)
        {
            ListFactoryTableAdapter listFactoryTableAdapter = new ListFactoryTableAdapter();
            ListMaintenance.ListFactoryDataTable listFactoryDataTable = listFactoryTableAdapter.GetData();

            if (withNewRow) listFactoryDataTable.AddListFactoryRow("", "", DateTime.Now);
            return listFactoryDataTable;
        }


        public ListMaintenance.ListOwnerDataTable GetListOwner()
        {
            return GetListOwner(false);
        }

        public ListMaintenance.ListOwnerDataTable GetListOwner(bool withNewRow)
        {
            ListOwnerTableAdapter listOwnerTableAdapter = new ListOwnerTableAdapter();
            ListMaintenance.ListOwnerDataTable listOwnerDataTable = listOwnerTableAdapter.GetData();

            if (withNewRow) listOwnerDataTable.AddListOwnerRow("", "", DateTime.Now);
            return listOwnerDataTable;
        }


        public ListMaintenance.ListCategoryDataTable GetListCategory()
        {
            return GetListCategory(false);
        }

        public ListMaintenance.ListCategoryDataTable GetListCategory(bool withNewRow)
        {
            ListCategoryTableAdapter listCategoryTableAdapter = new ListCategoryTableAdapter();
            ListMaintenance.ListCategoryDataTable listCategoryDataTable = listCategoryTableAdapter.GetData();

            if (withNewRow) listCategoryDataTable.AddListCategoryRow("", "", DateTime.Now);
            return listCategoryDataTable;
        }


        public ListMaintenance.ListProductDataTable GetListProduct()
        {
            return GetListProduct(false);
        }

        public ListMaintenance.ListProductDataTable GetListProduct(bool withNewRow)
        {
            ListProductTableAdapter listProductTableAdapter = new ListProductTableAdapter();
            ListMaintenance.ListProductDataTable listProductDataTable = listProductTableAdapter.GetData();

            if (withNewRow) listProductDataTable.AddListProductRow("", "", DateTime.Now);
            return listProductDataTable;
        }


        public ListMaintenance.ListCoilDataTable GetListCoil()
        {
            return GetListCoil(false);
        }

        public ListMaintenance.ListCoilDataTable GetListCoil(bool withNewRow)
        {
            ListCoilTableAdapter listCoilTableAdapter = new ListCoilTableAdapter();
            ListMaintenance.ListCoilDataTable listCoilDataTable = listCoilTableAdapter.GetData(GlobalVariables.GlobalOptionSetting.LowerFillterDate, GlobalVariables.GlobalOptionSetting.UpperFillterDate);

            if (withNewRow) listCoilDataTable.AddListCoilRow("", DateTime.Now, "");
            return listCoilDataTable;
        }








        public ListMaintenance.ListStaffNameDataTable GetStaffName()
        {
            return GetStaffName(false);
        }

        public ListMaintenance.ListStaffNameDataTable GetStaffName(bool withNewRow)
        {
            ListStaffNameTableAdapter listStaffNameTableAdapter = new ListStaffNameTableAdapter();
            ListMaintenance.ListStaffNameDataTable listStaffNameDataTable = listStaffNameTableAdapter.GetData();

            if (withNewRow) listStaffNameDataTable.AddListStaffNameRow("", "");
            return listStaffNameDataTable;
        }



        public ERmgrUIP.ListAddressAreaDataTable GetAddressArea()
        {
            return GetAddressArea(false);
        }

        public ERmgrUIP.ListAddressAreaDataTable GetAddressArea(bool withNewRow)
        {
            ListAddressAreaTableAdapter listAddressAreaTableAdapter = new ListAddressAreaTableAdapter();
            ERmgrUIP.ListAddressAreaDataTable listAddressAreaDataTable = listAddressAreaTableAdapter.GetData();

            if (withNewRow) listAddressAreaDataTable.AddListAddressAreaRow(0, "");
            return listAddressAreaDataTable;
        }


        public ERmgrUIP.ListItemCategoryDataTable GetItemCategory()
        {
            return GetItemCategory(false);
        }

        public ERmgrUIP.ListItemCategoryDataTable GetItemCategory(bool withNewRow)
        {
            ListItemCategoryTableAdapter listItemCategoryTableAdapter = new ListItemCategoryTableAdapter();
            ERmgrUIP.ListItemCategoryDataTable listItemCategoryDataTable = listItemCategoryTableAdapter.GetData();

            if (withNewRow) listItemCategoryDataTable.AddListItemCategoryRow(0, "");
            return listItemCategoryDataTable;
        }


        public ERmgrUIP.ListItemCommodityDataTable GetItemCommodity()
        {
            return GetItemCommodity(false);
        }

        public ERmgrUIP.ListItemCommodityDataTable GetItemCommodity(bool withNewRow)
        {
            ListItemCommodityTableAdapter listItemCommodityTableAdapter = new ListItemCommodityTableAdapter();
            ERmgrUIP.ListItemCommodityDataTable listItemCommodityDataTable = listItemCommodityTableAdapter.GetData();

            if (withNewRow) listItemCommodityDataTable.AddListItemCommodityRow(0, "", "");
            return listItemCommodityDataTable;
        }



        public ERmgrUIP.ListCustomerChannelDataTable GetCustomerChannel()
        {
            return GetCustomerChannel(false, false);
        }

        public ERmgrUIP.ListCustomerChannelDataTable GetCustomerChannel(bool withNewRow)
        {
            return GetCustomerChannel(withNewRow, false);
        }

        public ERmgrUIP.ListCustomerChannelDataTable GetCustomerChannel(bool withNewRow, bool primaryChannelOnly)
        {
            ListCustomerChannelTableAdapter listCustomerChannelTableAdapter = new ListCustomerChannelTableAdapter();
            ERmgrUIP.ListCustomerChannelDataTable listCustomerChannelDataTable;
            if (primaryChannelOnly)
                listCustomerChannelDataTable = listCustomerChannelTableAdapter.GetPrimaryChannel();
            else
                listCustomerChannelDataTable = listCustomerChannelTableAdapter.GetData();

            if (withNewRow) listCustomerChannelDataTable.AddListCustomerChannelRow(0, " ");
            return listCustomerChannelDataTable;
        }



        public ERmgrUIP.ListCustomerNameDataTable GetCustomerName()
        {
            return GetCustomerName(false);
        }

        public ERmgrUIP.ListCustomerNameDataTable GetCustomerName(bool withNewRow)
        {
            ListCustomerNameTableAdapter listCustomerNameTableAdapter = new ListCustomerNameTableAdapter();
            ERmgrUIP.ListCustomerNameDataTable listCustomerNameDataTable = listCustomerNameTableAdapter.GetData();

            if (withNewRow) listCustomerNameDataTable.AddListCustomerNameRow(0, " ", " ");
            return listCustomerNameDataTable;
        }


        public ERmgrUIP.ListMarketingProgramGroupDataTable GetMarketingProgramGroup()
        {
            return GetMarketingProgramGroup(false);
        }

        public ERmgrUIP.ListMarketingProgramGroupDataTable GetMarketingProgramGroup(bool withNewRow)
        {
            ListMarketingProgramGroupTableAdapter listMarketingProgramGroupTableAdapter = new ListMarketingProgramGroupTableAdapter();
            ERmgrUIP.ListMarketingProgramGroupDataTable listMarketingProgramGroupDataTable = listMarketingProgramGroupTableAdapter.GetData();

            if (withNewRow) listMarketingProgramGroupDataTable.AddListMarketingProgramGroupRow(0, "");
            return listMarketingProgramGroupDataTable;
        }


        public ERmgrUIP.ListMarketingProgramTypeDataTable GetMarketingProgramType()
        {
            return GetMarketingProgramType(false);
        }

        public ERmgrUIP.ListMarketingProgramTypeDataTable GetMarketingProgramType(bool withNewRow)
        {
            ListMarketingProgramTypeTableAdapter listMarketingProgramTypeTableAdapter = new ListMarketingProgramTypeTableAdapter();
            ERmgrUIP.ListMarketingProgramTypeDataTable listMarketingProgramTypeDataTable = listMarketingProgramTypeTableAdapter.GetData();

            if (withNewRow) listMarketingProgramTypeDataTable.AddListMarketingProgramTypeRow(0, "");
            return listMarketingProgramTypeDataTable;
        }


        public ERmgrUIP.ListMarketingPaymentTypeDataTable GetMarketingPaymentType()
        {
            return GetMarketingPaymentType(false);
        }

        public ERmgrUIP.ListMarketingPaymentTypeDataTable GetMarketingPaymentType(bool withNewRow)
        {
            ListMarketingPaymentTypeTableAdapter listMarketingPaymentTypeTableAdapter = new ListMarketingPaymentTypeTableAdapter();
            ERmgrUIP.ListMarketingPaymentTypeDataTable listMarketingPaymentTypeDataTable = listMarketingPaymentTypeTableAdapter.GetData();

            if (withNewRow) listMarketingPaymentTypeDataTable.AddListMarketingPaymentTypeRow(0, "");
            return listMarketingPaymentTypeDataTable;
        }

        public ERmgrUIP.ListMarketingPaymentTermDataTable GetMarketingPaymentTerm()
        {
            return GetMarketingPaymentTerm(false);
        }

        public ERmgrUIP.ListMarketingPaymentTermDataTable GetMarketingPaymentTerm(bool withNewRow)
        {
            ListMarketingPaymentTermTableAdapter listMarketingPaymentTermTableAdapter = new ListMarketingPaymentTermTableAdapter();
            ERmgrUIP.ListMarketingPaymentTermDataTable listMarketingPaymentTermDataTable = listMarketingPaymentTermTableAdapter.GetData();

            if (withNewRow) listMarketingPaymentTermDataTable.AddListMarketingPaymentTermRow(0, "");
            return listMarketingPaymentTermDataTable;
        }


        public ERmgrUIP.ListItemTypeDataTable GetItemType()
        {
            return GetItemType(false);
        }

        public ERmgrUIP.ListItemTypeDataTable GetItemType(bool withNewRow)
        {
            ListItemTypeTableAdapter listItemTypeTableAdapter = new ListItemTypeTableAdapter();
            ERmgrUIP.ListItemTypeDataTable listItemTypeDataTable = listItemTypeTableAdapter.GetData();

            if (withNewRow) listItemTypeDataTable.AddListItemTypeRow(0, "");
            return listItemTypeDataTable;
        }

        public ERmgrUIP.ListTimeBaseDataTable GetTimeBase()
        {
            return GetTimeBase(false);
        }

        public ERmgrUIP.ListTimeBaseDataTable GetTimeBase(bool withNewRow)
        {
            ListTimeBaseTableAdapter listTimeBaseTableAdapter = new ListTimeBaseTableAdapter();
            ERmgrUIP.ListTimeBaseDataTable listTimeBaseDataTable = listTimeBaseTableAdapter.GetData();

            if (withNewRow) listTimeBaseDataTable.AddListTimeBaseRow(0, "");
            return listTimeBaseDataTable;
        }


        public ERmgrUIP.ListMechanicSchemeDataTable GetMechanicScheme()
        {
            return GetMechanicScheme(false);
        }

        public ERmgrUIP.ListMechanicSchemeDataTable GetMechanicScheme(bool withNewRow)
        {
            ListMechanicSchemeTableAdapter listMechanicSchemeTableAdapter = new ListMechanicSchemeTableAdapter();
            ERmgrUIP.ListMechanicSchemeDataTable listMechanicSchemeDataTable = listMechanicSchemeTableAdapter.GetData();

            if (withNewRow) listMechanicSchemeDataTable.AddListMechanicSchemeRow(0, "", "");
            return listMechanicSchemeDataTable;
        }


        public ERmgrUIP.MarketingProgramListDataTable GetMarketingProgramList()
        {
            return GetMarketingProgramList(false);
        }

        public ERmgrUIP.MarketingProgramListDataTable GetMarketingProgramList(bool withNewRow)
        {
            MarketingProgramListTableAdapter marketingProgramListTableAdapter = new MarketingProgramListTableAdapter();
            ERmgrUIP.MarketingProgramListDataTable marketingProgramListDataTable = marketingProgramListTableAdapter.GetData();

            if (withNewRow) marketingProgramListDataTable.AddMarketingProgramListRow(" ", " ", " ");
            return marketingProgramListDataTable;
        }


        public ERmgrUIP.MarketingIncentiveMasterListDataTable GetMarketingIncentiveMasterList()
        {
            return GetMarketingIncentiveMasterList(false);
        }

        public ERmgrUIP.MarketingIncentiveMasterListDataTable GetMarketingIncentiveMasterList(bool withNewRow)
        {
            MarketingIncentiveMasterListTableAdapter MarketingIncentiveMasterListTableAdapter = new MarketingIncentiveMasterListTableAdapter();
            ERmgrUIP.MarketingIncentiveMasterListDataTable MarketingIncentiveMasterListDataTable = MarketingIncentiveMasterListTableAdapter.GetData();

            if (withNewRow) MarketingIncentiveMasterListDataTable.AddMarketingIncentiveMasterListRow(0, DateTime.Today, " ");
            return MarketingIncentiveMasterListDataTable;
        }

    }
}
