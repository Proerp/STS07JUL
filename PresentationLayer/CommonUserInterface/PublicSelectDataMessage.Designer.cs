namespace PresentationLayer
{
    partial class PublicSelectDataMessage
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            this.toolStripMDIMain = new System.Windows.Forms.ToolStrip();
            this.toolStripButtonEscape = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparatorPrint = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripButtonOK = new System.Windows.Forms.ToolStripButton();
            this.dataGridViewDataMessageMaster = new System.Windows.Forms.DataGridView();
            this.ProductionDate = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.LogoName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.FactoryName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.CategoryName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.OwnerName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ProductName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.CoilCode = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.CoilExtension = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.toolStripMDIMain.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewDataMessageMaster)).BeginInit();
            this.SuspendLayout();
            // 
            // toolStripMDIMain
            // 
            this.toolStripMDIMain.BackColor = System.Drawing.SystemColors.Control;
            this.toolStripMDIMain.BackgroundImage = global::PresentationLayer.Properties.Resources.Toolbar_Image;
            this.toolStripMDIMain.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripButtonEscape,
            this.toolStripSeparatorPrint,
            this.toolStripButtonOK});
            this.toolStripMDIMain.LayoutStyle = System.Windows.Forms.ToolStripLayoutStyle.HorizontalStackWithOverflow;
            this.toolStripMDIMain.Location = new System.Drawing.Point(0, 0);
            this.toolStripMDIMain.Name = "toolStripMDIMain";
            this.toolStripMDIMain.Size = new System.Drawing.Size(1139, 39);
            this.toolStripMDIMain.TabIndex = 30;
            this.toolStripMDIMain.Text = "ToolStrip";
            // 
            // toolStripButtonEscape
            // 
            this.toolStripButtonEscape.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonEscape.Image = global::PresentationLayer.Properties.Resources.esc;
            this.toolStripButtonEscape.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.toolStripButtonEscape.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonEscape.Name = "toolStripButtonEscape";
            this.toolStripButtonEscape.Size = new System.Drawing.Size(36, 36);
            this.toolStripButtonEscape.Text = "ESC";
            this.toolStripButtonEscape.Click += new System.EventHandler(this.toolStripButtonEscapeAndOK_Click);
            // 
            // toolStripSeparatorPrint
            // 
            this.toolStripSeparatorPrint.Name = "toolStripSeparatorPrint";
            this.toolStripSeparatorPrint.Size = new System.Drawing.Size(6, 39);
            // 
            // toolStripButtonOK
            // 
            this.toolStripButtonOK.Image = global::PresentationLayer.Properties.Resources.Saki_NuoveXT_Actions_ok;
            this.toolStripButtonOK.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.toolStripButtonOK.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonOK.Name = "toolStripButtonOK";
            this.toolStripButtonOK.Size = new System.Drawing.Size(59, 36);
            this.toolStripButtonOK.Text = "OK";
            this.toolStripButtonOK.Click += new System.EventHandler(this.toolStripButtonEscapeAndOK_Click);
            // 
            // dataGridViewDataMessageMaster
            // 
            this.dataGridViewDataMessageMaster.AllowUserToAddRows = false;
            this.dataGridViewDataMessageMaster.AllowUserToDeleteRows = false;
            this.dataGridViewDataMessageMaster.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dataGridViewDataMessageMaster.BackgroundColor = System.Drawing.SystemColors.Control;
            this.dataGridViewDataMessageMaster.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.dataGridViewDataMessageMaster.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            this.dataGridViewDataMessageMaster.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewDataMessageMaster.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.ProductionDate,
            this.LogoName,
            this.FactoryName,
            this.CategoryName,
            this.OwnerName,
            this.ProductName,
            this.CoilCode,
            this.CoilExtension,
            this.dataGridViewTextBoxColumn1,
            this.dataGridViewTextBoxColumn3});
            this.dataGridViewDataMessageMaster.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridViewDataMessageMaster.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
            this.dataGridViewDataMessageMaster.GridColor = System.Drawing.SystemColors.Control;
            this.dataGridViewDataMessageMaster.Location = new System.Drawing.Point(0, 39);
            this.dataGridViewDataMessageMaster.Name = "dataGridViewDataMessageMaster";
            this.dataGridViewDataMessageMaster.RowHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            dataGridViewCellStyle4.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dataGridViewDataMessageMaster.RowsDefaultCellStyle = dataGridViewCellStyle4;
            this.dataGridViewDataMessageMaster.RowTemplate.Height = 27;
            this.dataGridViewDataMessageMaster.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridViewDataMessageMaster.Size = new System.Drawing.Size(1139, 601);
            this.dataGridViewDataMessageMaster.TabIndex = 68;
            this.dataGridViewDataMessageMaster.RowPostPaint += new System.Windows.Forms.DataGridViewRowPostPaintEventHandler(this.dataGridViewDataMessageMaster_RowPostPaint);
            // 
            // ProductionDate
            // 
            this.ProductionDate.DataPropertyName = "ProductionDate";
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.ProductionDate.DefaultCellStyle = dataGridViewCellStyle1;
            this.ProductionDate.FillWeight = 10F;
            this.ProductionDate.HeaderText = "Ngày SX";
            this.ProductionDate.Name = "ProductionDate";
            // 
            // LogoName
            // 
            this.LogoName.DataPropertyName = "LogoName";
            this.LogoName.FillWeight = 7F;
            this.LogoName.HeaderText = "Logo";
            this.LogoName.Name = "LogoName";
            this.LogoName.ReadOnly = true;
            // 
            // FactoryName
            // 
            this.FactoryName.DataPropertyName = "FactoryName";
            this.FactoryName.FillWeight = 16F;
            this.FactoryName.HeaderText = "Đơn vị sản xuất";
            this.FactoryName.Name = "FactoryName";
            // 
            // CategoryName
            // 
            this.CategoryName.DataPropertyName = "CategoryName";
            this.CategoryName.FillWeight = 14F;
            this.CategoryName.HeaderText = "Loại sản phẩm";
            this.CategoryName.Name = "CategoryName";
            // 
            // OwnerName
            // 
            this.OwnerName.DataPropertyName = "OwnerName";
            this.OwnerName.FillWeight = 10F;
            this.OwnerName.HeaderText = "Chủ hàng";
            this.OwnerName.Name = "OwnerName";
            // 
            // ProductName
            // 
            this.ProductName.DataPropertyName = "ProductName";
            this.ProductName.FillWeight = 21F;
            this.ProductName.HeaderText = "Mã sản phẫm";
            this.ProductName.Name = "ProductName";
            // 
            // CoilCode
            // 
            this.CoilCode.DataPropertyName = "CoilCode";
            this.CoilCode.FillWeight = 10F;
            this.CoilCode.HeaderText = "Mã cuộn";
            this.CoilCode.Name = "CoilCode";
            // 
            // CoilExtension
            // 
            this.CoilExtension.DataPropertyName = "CoilExtension";
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.CoilExtension.DefaultCellStyle = dataGridViewCellStyle2;
            this.CoilExtension.FillWeight = 9F;
            this.CoilExtension.HeaderText = "Chia cuộn";
            this.CoilExtension.Name = "CoilExtension";
            // 
            // dataGridViewTextBoxColumn1
            // 
            this.dataGridViewTextBoxColumn1.DataPropertyName = "CounterValue";
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle3.Format = "N0";
            dataGridViewCellStyle3.NullValue = null;
            this.dataGridViewTextBoxColumn1.DefaultCellStyle = dataGridViewCellStyle3;
            this.dataGridViewTextBoxColumn1.FillWeight = 8F;
            this.dataGridViewTextBoxColumn1.HeaderText = "Số mét";
            this.dataGridViewTextBoxColumn1.Name = "dataGridViewTextBoxColumn1";
            // 
            // dataGridViewTextBoxColumn3
            // 
            this.dataGridViewTextBoxColumn3.DataPropertyName = "Remarks";
            this.dataGridViewTextBoxColumn3.FillWeight = 15F;
            this.dataGridViewTextBoxColumn3.HeaderText = "Ghi chú";
            this.dataGridViewTextBoxColumn3.Name = "dataGridViewTextBoxColumn3";
            // 
            // PublicSelectDataMessage
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1139, 640);
            this.Controls.Add(this.dataGridViewDataMessageMaster);
            this.Controls.Add(this.toolStripMDIMain);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "PublicSelectDataMessage";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Select DataMessageMaster";
            this.toolStripMDIMain.ResumeLayout(false);
            this.toolStripMDIMain.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewDataMessageMaster)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolStrip toolStripMDIMain;
        private System.Windows.Forms.ToolStripButton toolStripButtonEscape;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparatorPrint;
        private System.Windows.Forms.ToolStripButton toolStripButtonOK;
        private System.Windows.Forms.DataGridView dataGridViewDataMessageMaster;
        private System.Windows.Forms.DataGridViewTextBoxColumn ProductionDate;
        private System.Windows.Forms.DataGridViewTextBoxColumn LogoName;
        private System.Windows.Forms.DataGridViewTextBoxColumn FactoryName;
        private System.Windows.Forms.DataGridViewTextBoxColumn CategoryName;
        private System.Windows.Forms.DataGridViewTextBoxColumn OwnerName;
        private System.Windows.Forms.DataGridViewTextBoxColumn ProductName;
        private System.Windows.Forms.DataGridViewTextBoxColumn CoilCode;
        private System.Windows.Forms.DataGridViewTextBoxColumn CoilExtension;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn3;
    }
}