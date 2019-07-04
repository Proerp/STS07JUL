using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Reflection;

//using System.ComponentModel;

using BrightIdeasSoftware;
using OfficeExcel = Microsoft.Office.Interop.Excel;
using System.Globalization;

namespace PresentationLayer
{
    public static class CommonFormAction
    {
        public static void OLVFilter(ObjectListView olv, string txt)
        {
            CommonFormAction.OLVFilter(olv, txt, 0);
        }

        public static void OLVFilter(ObjectListView olv, string txt, int matchKind)
        {
            TextMatchFilter filter = null;
            if (!String.IsNullOrEmpty(txt))
            {
                switch (matchKind)
                {
                    case 0:
                    default:
                        filter = TextMatchFilter.Contains(olv, txt);
                        break;
                    case 1:
                        filter = TextMatchFilter.Prefix(olv, txt);
                        break;
                    case 2:
                        filter = TextMatchFilter.Regex(olv, txt);
                        break;
                }
            }
            // Setup a default renderer to draw the filter matches

            if (filter == null)
                olv.DefaultRenderer = null;
            else
            {
                olv.DefaultRenderer = new HighlightTextRenderer(filter);

                // Uncomment this line to see how the GDI+ rendering looks
                //olv.DefaultRenderer = new HighlightTextRenderer { Filter = filter, UseGdiTextRendering = false };
            }

            // Some lists have renderers already installed
            HighlightTextRenderer highlightingRenderer = olv.GetColumn(0).Renderer as HighlightTextRenderer;
            if (highlightingRenderer != null)
                highlightingRenderer.Filter = filter;


            olv.AdditionalFilter = filter;
            //olv.Invalidate();

        }

        public static void Export<T>(List<T> list)
        {
            DataTable dataTable = ListToDataTable(list);
            CommonFormAction.Export(dataTable);
        }

        public static void Export(DataTable dataTableExport)
        {
            if (dataTableExport != null)
            {
                //SaveFileDialog saveFileDialogMain = new SaveFileDialog();
                //saveFileDialogMain.Filter = "Excel files (*.xls)|*.xls|All files (*.*)|*.*";
                //saveFileDialogMain.FilterIndex = 1;
                //saveFileDialogMain.RestoreDirectory = true;
                //saveFileDialogMain.OverwritePrompt = true;

                //if (saveFileDialogMain.ShowDialog() == DialogResult.OK)
                //{
                ////The following run very fast, but does not smoothly, sometime it raise error with some case of data (Ex. x0020 ....)
                ////RKLib.ExportData.Export objExport = new RKLib.ExportData.Export("Win");
                ////objExport.ExportDetails(dataTableExport, RKLib.ExportData.Export.ExportFormat.Excel, saveFileDialogMain.FileName);

                CommonFormAction.ExportToExcel(dataTableExport, "ExportData");
                //}
            }
        }



        #region Convert ListToDataTable


        //public static DataTable ListToDataTable<T>(this IList<T> data)
        //{
        //    PropertyDescriptorCollection props =
        //        TypeDescriptor.GetProperties(typeof(T));
        //    DataTable table = new DataTable();
        //    for (int i = 0; i < props.Count; i++)
        //    {
        //        PropertyDescriptor prop = props[i];
        //        //if (prop.GetType().GetGenericTypeDefinition() != typeof(Dictionary<,>))
        //        if (!typeof(IDictionary<,>).IsAssignableFrom(prop.GetType()))
        //            table.Columns.Add(prop.Name, prop.PropertyType);
        //    }
        //    object[] values = new object[props.Count];
        //    foreach (T item in data)
        //    {
        //        for (int i = 0; i < values.Length; i++)
        //        {
        //            //if (props[i].GetType().GetGenericTypeDefinition() != typeof(Dictionary<,>))
        //            if (!typeof(IDictionary<,>).IsAssignableFrom(props[i].GetType()))
        //                values[i] = props[i].GetValue(item);
        //        }
        //        table.Rows.Add(values);
        //    }
        //    return table;
        //}

        //public static DataTable ListToDataTable<T>(List<T> list)
        //{
        //    DataTable dt = new DataTable();

        //    foreach (PropertyInfo info in typeof(T).GetProperties())
        //    {
        //        dt.Columns.Add(new DataColumn(info.Name, System.Type.GetType(info.PropertyType.FullName.ToString() )));
        //    }
        //    foreach (T t in list)
        //    {
        //        DataRow row = dt.NewRow();
        //        foreach (PropertyInfo info in typeof(T).GetProperties())
        //        {
        //            row[info.Name] = info.GetValue(t, null);
        //        }
        //        row["CustomerID"] = 1;
        //        row["EffectedDate"] = DateTime.Today;
        //        row["SalesTarget"] = 1000;
        //        row["Remarks"] = "A";



        //        dt.Rows.Add(row);
        //    }
        //    return dt;
        //}




        //  This function is adapated from: http://www.codeguru.com/forum/showthread.php?t=450171
        //  My thanks to Carl Quirion, for making it "nullable-friendly".
        public static DataTable ListToDataTable<T>(List<T> list)
        {
            DataTable dt = new DataTable();

            foreach (PropertyInfo info in typeof(T).GetProperties())
            {
                //if (!typeof(IDictionary<,>).IsAssignableFrom(info.GetType()))
                if (info.Name != "Changes" && info.Name != "IsDirty")
                    dt.Columns.Add(new DataColumn(info.Name, GetNullableType(info.PropertyType)));
            }
            foreach (T t in list)
            {
                DataRow row = dt.NewRow();
                foreach (PropertyInfo info in typeof(T).GetProperties())
                {
                    //if (!typeof(IDictionary<,>).IsAssignableFrom(info.GetType()))
                    if (info.Name != "Changes" && info.Name != "IsDirty")
                        if (!IsNullableType(info.PropertyType))
                            row[info.Name] = info.GetValue(t, null);
                        else
                            row[info.Name] = (info.GetValue(t, null) ?? DBNull.Value);
                }
                dt.Rows.Add(row);
            }
            return dt;
        }

        private static Type GetNullableType(Type t)
        {
            Type returnType = t;
            if (t.IsGenericType && t.GetGenericTypeDefinition().Equals(typeof(Nullable<>)))
            {
                returnType = Nullable.GetUnderlyingType(t);
            }
            return returnType;
        }

        private static bool IsNullableType(Type type)
        {
            return (type == typeof(string) ||
                    type.IsArray ||
                    (type.IsGenericType &&
                     type.GetGenericTypeDefinition().Equals(typeof(Nullable<>))));
        }

        #endregion Convert ListToDataTable




        private static void ExportToExcel(DataTable dataTableExport, string workSheetName)
        {
            OfficeExcel.Application excelApplication = null;// Declare variables that hold references to excel objects
            OfficeExcel.Workbook excelWorkBook = null;
            OfficeExcel.Worksheet targetSheet = null;

            try
            {
                if (dataTableExport.Rows.Count <= 0) throw new Exception("There is no data to show.");

                object[,] arr = new object[dataTableExport.Rows.Count, dataTableExport.Columns.Count]; bool boolValue;
                for (int rowIndex = 0; rowIndex < dataTableExport.Rows.Count; rowIndex++)
                {
                    for (int columnIndex = 0; columnIndex < dataTableExport.Columns.Count; columnIndex++)
                    {
                        if (dataTableExport.Columns[columnIndex].DataType == typeof(System.Boolean))
                            if (bool.TryParse(dataTableExport.Rows[rowIndex][columnIndex].ToString(), out boolValue))
                                arr[rowIndex, columnIndex] = (boolValue == true ? 1 : 0);
                            else //NULL
                                arr[rowIndex, columnIndex] = 0;
                        else
                            arr[rowIndex, columnIndex] = dataTableExport.Rows[rowIndex][columnIndex];
                    }
                }

                excelApplication = new OfficeExcel.Application();// Create an instance of Excel
                excelWorkBook = excelApplication.Workbooks.Add(OfficeExcel.XlWBATemplate.xlWBATWorksheet);//Create a workbook and add a worksheet.
                targetSheet = (OfficeExcel.Worksheet)(excelWorkBook.Worksheets[1]);
                targetSheet.Name = workSheetName;

                for (int columnIndex = 0; columnIndex < dataTableExport.Columns.Count; columnIndex++)
                {
                    CommonFormAction.SetCellValue(targetSheet, CommonFormAction.GetExcelColumnName(columnIndex + 1) + "1", "'" + dataTableExport.Columns[columnIndex].ColumnName);
                }

                var startCell = (OfficeExcel.Range)targetSheet.Cells[2, 1];
                var endCell = (OfficeExcel.Range)targetSheet.Cells[dataTableExport.Rows.Count + 1, dataTableExport.Columns.Count];
                var writeRange = targetSheet.Range[startCell, endCell];

                writeRange.Value2 = arr;
                string shortDatePattern = CultureInfo.CurrentCulture.DateTimeFormat.ShortDatePattern;

                for (int columnIndex = 0; columnIndex < dataTableExport.Columns.Count; columnIndex++)
                {
                    if (dataTableExport.Columns[columnIndex].DataType == typeof(System.DateTime) || dataTableExport.Columns[columnIndex].DataType == typeof(System.Double) || dataTableExport.Columns[columnIndex].DataType == typeof(System.Boolean))
                        ((OfficeExcel.Range)targetSheet.Cells[2, columnIndex + 1]).EntireColumn.NumberFormat = dataTableExport.Columns[columnIndex].DataType == typeof(System.DateTime) ? shortDatePattern : "#,###";
                }

                targetSheet.get_Range("A1", CommonFormAction.GetExcelColumnName(dataTableExport.Columns.Count) + (dataTableExport.Rows.Count + 1).ToString()).Columns.AutoFit();
                targetSheet.get_Range("A1", CommonFormAction.GetExcelColumnName(dataTableExport.Columns.Count) + "1").RowHeight = targetSheet.get_Range("A1", CommonFormAction.GetExcelColumnName(dataTableExport.Columns.Count) + "1").RowHeight + 8;
                targetSheet.get_Range("A1", CommonFormAction.GetExcelColumnName(dataTableExport.Columns.Count) + "1").VerticalAlignment = OfficeExcel.XlVAlign.xlVAlignCenter;

                excelApplication.Visible = true;
            }
            catch (Exception exception)
            {
                throw exception;
            }
            finally
            {
                targetSheet = null;// Release the references to the Excel objects.
                if (excelWorkBook != null) excelWorkBook = null;// Release the Workbook object.

                //// Release the ApplicationClass object.
                //if (excelApplication != null)
                //{
                //    excelApplication.Quit();
                //    excelApplication = null;
                //}//Le Minh Hiep

                GC.Collect();
                GC.WaitForPendingFinalizers();
                GC.Collect();
                GC.WaitForPendingFinalizers();
            }
        }

        private static string GetExcelColumnName(int columnNumber)
        {
            int dividend = columnNumber;
            string columnName = String.Empty;
            int modulo;

            while (dividend > 0)
            {
                modulo = (dividend - 1) % 26;
                columnName = Convert.ToChar(65 + modulo).ToString() + columnName;
                dividend = (int)((dividend - modulo) / 26);
            }

            return columnName;
        }

        /// <summary>
        /// Helper method to set a value on a single cell.
        /// </summary>
        private static void SetCellValue(OfficeExcel.Worksheet targetSheet, string cell, object value)
        {
            targetSheet.get_Range(cell, Type.Missing).set_Value(OfficeExcel.XlRangeValueDataType.xlRangeValueDefault, value);
        }



    }
}
