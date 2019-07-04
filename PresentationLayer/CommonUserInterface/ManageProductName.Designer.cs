namespace PresentationLayer
{
    partial class ManageProductName
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            this.dataGridViewProductName = new System.Windows.Forms.DataGridView();
            this.ProductCode = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ProductCodeOriginal = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ProductName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.NoItemPerCarton = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewProductName)).BeginInit();
            this.SuspendLayout();
            // 
            // dataGridViewProductName
            // 
            this.dataGridViewProductName.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dataGridViewProductName.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.dataGridViewProductName.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewProductName.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.ProductCode,
            this.ProductCodeOriginal,
            this.ProductName,
            this.NoItemPerCarton});
            this.dataGridViewProductName.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridViewProductName.Location = new System.Drawing.Point(0, 0);
            this.dataGridViewProductName.Name = "dataGridViewProductName";
            this.dataGridViewProductName.Size = new System.Drawing.Size(620, 684);
            this.dataGridViewProductName.TabIndex = 0;
            this.dataGridViewProductName.CellValidating += new System.Windows.Forms.DataGridViewCellValidatingEventHandler(this.dataGridViewProductName_CellValidating);
            // 
            // ProductCode
            // 
            this.ProductCode.DataPropertyName = "ProductCode";
            this.ProductCode.HeaderText = "Product Code";
            this.ProductCode.Name = "ProductCode";
            this.ProductCode.Width = 97;
            // 
            // ProductCodeOriginal
            // 
            this.ProductCodeOriginal.DataPropertyName = "ProductCodeOriginal";
            this.ProductCodeOriginal.HeaderText = "Original Code";
            this.ProductCodeOriginal.Name = "ProductCodeOriginal";
            this.ProductCodeOriginal.Width = 95;
            // 
            // ProductName
            // 
            this.ProductName.DataPropertyName = "ProductName";
            this.ProductName.HeaderText = "Product Name";
            this.ProductName.Name = "ProductName";
            // 
            // NoItemPerCarton
            // 
            this.NoItemPerCarton.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.NoItemPerCarton.DataPropertyName = "NoItemPerCarton";
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle2.Format = "N0";
            dataGridViewCellStyle2.NullValue = null;
            this.NoItemPerCarton.DefaultCellStyle = dataGridViewCellStyle2;
            this.NoItemPerCarton.HeaderText = "No Item Per Carton";
            this.NoItemPerCarton.Name = "NoItemPerCarton";
            // 
            // ManageProductName
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(620, 684);
            this.Controls.Add(this.dataGridViewProductName);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ManageProductName";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Product List";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.ManageProductName_FormClosing);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewProductName)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView dataGridViewProductName;
        private System.Windows.Forms.DataGridViewTextBoxColumn ProductCode;
        private System.Windows.Forms.DataGridViewTextBoxColumn ProductCodeOriginal;
        private System.Windows.Forms.DataGridViewTextBoxColumn ProductName;
        private System.Windows.Forms.DataGridViewTextBoxColumn NoItemPerCarton;
    }
}