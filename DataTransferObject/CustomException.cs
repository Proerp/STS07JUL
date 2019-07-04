using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

using System.Runtime.Serialization;

namespace DataTransferObject
{
    [Serializable]
    public class CustomException : Exception
    {
        private DataTable exceptionTable;
        public DataTable ExceptionTable { get { return this.exceptionTable; } }

        private bool showGroups = true;
        public bool ShowGroups
        {
            get { return this.showGroups; }
            set { this.showGroups = value; }
        }

        public CustomException()
            : base() { }

        public CustomException(string message)
            : base(message) { }

        public CustomException(string format, params object[] args)
            : base(string.Format(format, args)) { }

        public CustomException(string message, Exception innerException)
            : base(message, innerException) { }

        public CustomException(string format, Exception innerException, params object[] args)
            : base(string.Format(format, args), innerException) { }

        protected CustomException(SerializationInfo info, StreamingContext context)
            : base(info, context) { }





        public CustomException(string message, DataTable exceptionTable)
            : base(message) { this.exceptionTable = exceptionTable; }

        public CustomException(string format, DataTable exceptionTable, params object[] args)
            : base(string.Format(format, args)) { this.exceptionTable = exceptionTable; }

        public CustomException(string message, Exception innerException, DataTable exceptionTable)
            : base(message, innerException) { this.exceptionTable = exceptionTable; }

        public CustomException(string format, Exception innerException, DataTable exceptionTable, params object[] args)
            : base(string.Format(format, args), innerException) { this.exceptionTable = exceptionTable; }


    }

    public class ExceptionTable
    {
        private bool isDirty;

        public bool IsDirty { get { return this.isDirty; } }
        public void ClearDirty() { this.isDirty = false; }

        private DataTable table;
        public DataTable Table { get { return this.table; } }

        public ExceptionTable(string[,] arrayColumn)
        {
            this.table = new DataTable("ExceptionTable");

            for (int i = 0; i < arrayColumn.GetLength(0); i++)
            {
                this.table.Columns.Add(arrayColumn[i, 0], System.Type.GetType(arrayColumn[i, 1]));
            }
        }

        public void AddException(string[] arrayColumn)
        {
            if (arrayColumn.GetLength(0) != this.table.Columns.Count) throw new Exception("In valid exception rows.");

            if (this.table.Columns.Count >= 1)
            {
                //DataRow[] foundDataRows = this.table.Select(this.table.Columns[0].ColumnName + " = '" + arrayColumn[0] + "' " + (this.table.Columns.Count >= 2 ? " AND " + this.table.Columns[1].ColumnName + " = '" + arrayColumn[1] + "' " : "") + (this.table.Columns.Count >= 3 ? " AND " + this.table.Columns[2].ColumnName + " = '" + arrayColumn[2] + "' " : ""));
                //if (foundDataRows == null || foundDataRows.Length == 0)
                //{
                    DataRow dataRow = this.table.NewRow();
                    for (int i = 0; i < this.table.Columns.Count; i++)
                    {
                        dataRow[i] = arrayColumn[i];
                    }

                    this.table.Rows.Add(dataRow);
                    this.isDirty = true;
                //}
            }
        }

    }
}
