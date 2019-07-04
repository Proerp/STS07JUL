using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;
using DataAccessLayer;

namespace PresentationLayer
{
    public partial class ManageProductName : Form
    {
        private BindingSource bindingSource = new BindingSource();
        private SqlDataAdapter sqlDataAdapter = new SqlDataAdapter();

        public ManageProductName()
        {
            InitializeComponent();

            dataGridViewProductName.AutoGenerateColumns = false;
            dataGridViewProductName.DataSource = bindingSource;

            this.LoadProductList();

            bindingSource.PositionChanged += new EventHandler(bindingSource_PositionChanged);
        }

        private void LoadProductList()
        {
            try
            {
                sqlDataAdapter = new SqlDataAdapter("SELECT * FROM ListProductName", ADODatabase.Connection());

                // Create a command builder to generate SQL update, insert, and delete commands based on selectCommand. These are used to update the database.
                SqlCommandBuilder commandBuilder = new SqlCommandBuilder(sqlDataAdapter);

                // Populate a new data table and bind it to the BindingSource.
                DataTable dataTable = new DataTable();
                dataTable.Locale = System.Globalization.CultureInfo.InvariantCulture;
                sqlDataAdapter.Fill(dataTable);
                bindingSource.DataSource = dataTable;


            }
            catch (Exception exception)
            {
                GlobalExceptionHandler.ShowExceptionMessageBox(this, exception);
            }
        }

        void bindingSource_PositionChanged(object sender, EventArgs e)
        {
            try
            {
                this.UpdateProductName();
            }
            catch (Exception exception)
            {
                GlobalExceptionHandler.ShowExceptionMessageBox(this, exception);
            }
        }

        private void ManageProductName_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                this.UpdateProductName();
            }
            catch (Exception exception)
            {
                e.Cancel = true;
                GlobalExceptionHandler.ShowExceptionMessageBox(this, exception);
            }
        }

        private void UpdateProductName()
        {
            DataTable dataTable = ((DataTable)bindingSource.DataSource).GetChanges(DataRowState.Added); int rowsAdded = 0;
            if (dataTable != null) rowsAdded = dataTable.Rows.Count;
            int rowsAffected = sqlDataAdapter.Update((DataTable)bindingSource.DataSource);
            if (rowsAffected != 0)
            {
                ADODatabase.ExecuteNonQuery("UPDATE ListFillingLineName SET ListProductNameRowsAffected = ListProductNameRowsAffected + " + rowsAffected);
                if (rowsAdded > 0) this.LoadProductList();
            }
        }

        private void dataGridViewProductName_CellValidating(object sender, DataGridViewCellValidatingEventArgs e)
        {
            try
            {
                string headerText = dataGridViewProductName.Columns[e.ColumnIndex].Name;

                if (headerText.Equals("ProductCode") && e.FormattedValue.ToString().Length != 3) e.Cancel = true;
                if (headerText.Equals("ProductCodeOriginal") && e.FormattedValue.ToString().Length != 7) e.Cancel = true;

            }
            catch (Exception exception)
            {
                e.Cancel = true;
                GlobalExceptionHandler.ShowExceptionMessageBox(this, exception);
            }
        }
    }
}
