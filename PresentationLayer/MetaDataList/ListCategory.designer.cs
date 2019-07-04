namespace PresentationLayer
{
    partial class ListCategory
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ListCategory));
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            this.imageListTabControl = new System.Windows.Forms.ImageList(this.components);
            this.toolStripChildForm = new System.Windows.Forms.ToolStrip();
            this.toolStripButton2 = new System.Windows.Forms.ToolStripButton();
            this.errorProviderMaster = new System.Windows.Forms.ErrorProvider(this.components);
            this.naviGroupDetails = new Guifreaks.Navisuite.NaviGroup(this.components);
            this.numericUpDownSizingDetail = new System.Windows.Forms.NumericUpDown();
            this.checkBoxIsDirtyBLL = new System.Windows.Forms.CheckBox();
            this.checkBoxIsDirty = new System.Windows.Forms.CheckBox();
            this.tableLayoutPanelMaster = new System.Windows.Forms.TableLayoutPanel();
            this.textBoxLogoGenerator = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.textBoxDescription = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label14 = new System.Windows.Forms.Label();
            this.textBoxRemarks = new System.Windows.Forms.TextBox();
            this.MaterialSetID = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.Quantity = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Remarks = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataListViewMaster = new BrightIdeasSoftware.DataListView();
            this.olvColumn4 = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.olvColumn1 = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.olvColumn2 = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.toolStripChildForm.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.errorProviderMaster)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.naviGroupDetails)).BeginInit();
            this.naviGroupDetails.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownSizingDetail)).BeginInit();
            this.tableLayoutPanelMaster.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataListViewMaster)).BeginInit();
            this.SuspendLayout();
            // 
            // imageListTabControl
            // 
            this.imageListTabControl.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageListTabControl.ImageStream")));
            this.imageListTabControl.TransparentColor = System.Drawing.Color.Transparent;
            this.imageListTabControl.Images.SetKeyName(0, "Actions-user-group-new.ico");
            this.imageListTabControl.Images.SetKeyName(1, "folder-invoices.ico");
            this.imageListTabControl.Images.SetKeyName(2, "User-Files.ico");
            this.imageListTabControl.Images.SetKeyName(3, "Map.ico");
            this.imageListTabControl.Images.SetKeyName(4, "Box-Zelda.ico");
            this.imageListTabControl.Images.SetKeyName(5, "app-map.ico");
            this.imageListTabControl.Images.SetKeyName(6, "hospital (1).ico");
            this.imageListTabControl.Images.SetKeyName(7, "capsule.ico");
            this.imageListTabControl.Images.SetKeyName(8, "Capsule (1).ico");
            this.imageListTabControl.Images.SetKeyName(9, "Capsule (2).ico");
            // 
            // toolStripChildForm
            // 
            this.toolStripChildForm.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.toolStripChildForm.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripButton2});
            this.toolStripChildForm.Location = new System.Drawing.Point(0, 0);
            this.toolStripChildForm.Name = "toolStripChildForm";
            this.toolStripChildForm.Size = new System.Drawing.Size(794, 39);
            this.toolStripChildForm.TabIndex = 15;
            this.toolStripChildForm.Text = "toolStrip1";
            this.toolStripChildForm.Visible = false;
            // 
            // toolStripButton2
            // 
            this.toolStripButton2.Image = global::PresentationLayer.Properties.Resources.output_disconnect_icone_6892__2_;
            this.toolStripButton2.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.toolStripButton2.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton2.Name = "toolStripButton2";
            this.toolStripButton2.Size = new System.Drawing.Size(102, 36);
            this.toolStripButton2.Text = "Disconnect";
            this.toolStripButton2.Visible = false;
            // 
            // errorProviderMaster
            // 
            this.errorProviderMaster.ContainerControl = this;
            // 
            // naviGroupDetails
            // 
            this.naviGroupDetails.Caption = "   Chi tiết";
            this.naviGroupDetails.Controls.Add(this.numericUpDownSizingDetail);
            this.naviGroupDetails.Controls.Add(this.checkBoxIsDirtyBLL);
            this.naviGroupDetails.Controls.Add(this.checkBoxIsDirty);
            this.naviGroupDetails.Controls.Add(this.tableLayoutPanelMaster);
            this.naviGroupDetails.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.naviGroupDetails.Expanded = false;
            this.naviGroupDetails.ExpandedHeight = 104;
            this.naviGroupDetails.HeaderContextMenuStrip = null;
            this.naviGroupDetails.LayoutStyle = Guifreaks.Navisuite.NaviLayoutStyle.Office2010Blue;
            this.naviGroupDetails.Location = new System.Drawing.Point(0, 722);
            this.naviGroupDetails.Name = "naviGroupDetails";
            this.naviGroupDetails.Padding = new System.Windows.Forms.Padding(0, 22, 0, 0);
            this.naviGroupDetails.Size = new System.Drawing.Size(794, 20);
            this.naviGroupDetails.TabIndex = 24;
            this.naviGroupDetails.Text = "naviGroup1";
            this.naviGroupDetails.HeaderMouseClick += new System.Windows.Forms.MouseEventHandler(this.naviGroupDetails_HeaderMouseClick);
            // 
            // numericUpDownSizingDetail
            // 
            this.numericUpDownSizingDetail.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.numericUpDownSizingDetail.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.numericUpDownSizingDetail.Increment = new decimal(new int[] {
            5,
            0,
            0,
            0});
            this.numericUpDownSizingDetail.Location = new System.Drawing.Point(777, 0);
            this.numericUpDownSizingDetail.Maximum = new decimal(new int[] {
            788,
            0,
            0,
            0});
            this.numericUpDownSizingDetail.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numericUpDownSizingDetail.Name = "numericUpDownSizingDetail";
            this.numericUpDownSizingDetail.Size = new System.Drawing.Size(17, 22);
            this.numericUpDownSizingDetail.TabIndex = 6;
            this.numericUpDownSizingDetail.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.numericUpDownSizingDetail.Value = new decimal(new int[] {
            106,
            0,
            0,
            0});
            this.numericUpDownSizingDetail.Visible = false;
            this.numericUpDownSizingDetail.ValueChanged += new System.EventHandler(this.numericUpDownSizingDetail_ValueChanged);
            // 
            // checkBoxIsDirtyBLL
            // 
            this.checkBoxIsDirtyBLL.AutoSize = true;
            this.checkBoxIsDirtyBLL.Location = new System.Drawing.Point(-300, 0);
            this.checkBoxIsDirtyBLL.Name = "checkBoxIsDirtyBLL";
            this.checkBoxIsDirtyBLL.Size = new System.Drawing.Size(49, 17);
            this.checkBoxIsDirtyBLL.TabIndex = 63;
            this.checkBoxIsDirtyBLL.Text = "Dirty";
            this.checkBoxIsDirtyBLL.UseVisualStyleBackColor = true;
            // 
            // checkBoxIsDirty
            // 
            this.checkBoxIsDirty.AutoSize = true;
            this.checkBoxIsDirty.Location = new System.Drawing.Point(-300, 0);
            this.checkBoxIsDirty.Name = "checkBoxIsDirty";
            this.checkBoxIsDirty.Size = new System.Drawing.Size(49, 17);
            this.checkBoxIsDirty.TabIndex = 24;
            this.checkBoxIsDirty.Text = "Dirty";
            this.checkBoxIsDirty.UseVisualStyleBackColor = true;
            // 
            // tableLayoutPanelMaster
            // 
            this.tableLayoutPanelMaster.AutoSize = true;
            this.tableLayoutPanelMaster.BackColor = System.Drawing.Color.Transparent;
            this.tableLayoutPanelMaster.ColumnCount = 9;
            this.tableLayoutPanelMaster.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25.02005F));
            this.tableLayoutPanelMaster.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 23.31998F));
            this.tableLayoutPanelMaster.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 10F));
            this.tableLayoutPanelMaster.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 0F));
            this.tableLayoutPanelMaster.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 23.31998F));
            this.tableLayoutPanelMaster.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 10F));
            this.tableLayoutPanelMaster.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 5.010011F));
            this.tableLayoutPanelMaster.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 23.32999F));
            this.tableLayoutPanelMaster.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 50F));
            this.tableLayoutPanelMaster.Controls.Add(this.textBoxLogoGenerator, 1, 1);
            this.tableLayoutPanelMaster.Controls.Add(this.label6, 0, 0);
            this.tableLayoutPanelMaster.Controls.Add(this.textBoxDescription, 1, 0);
            this.tableLayoutPanelMaster.Controls.Add(this.label4, 0, 2);
            this.tableLayoutPanelMaster.Controls.Add(this.label14, 0, 1);
            this.tableLayoutPanelMaster.Controls.Add(this.textBoxRemarks, 1, 2);
            this.tableLayoutPanelMaster.Dock = System.Windows.Forms.DockStyle.Top;
            this.tableLayoutPanelMaster.Location = new System.Drawing.Point(0, 22);
            this.tableLayoutPanelMaster.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanelMaster.Name = "tableLayoutPanelMaster";
            this.tableLayoutPanelMaster.Padding = new System.Windows.Forms.Padding(0, 2, 0, 2);
            this.tableLayoutPanelMaster.RowCount = 3;
            this.tableLayoutPanelMaster.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelMaster.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelMaster.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelMaster.Size = new System.Drawing.Size(794, 73);
            this.tableLayoutPanelMaster.TabIndex = 62;
            // 
            // textBoxLogoGenerator
            // 
            this.tableLayoutPanelMaster.SetColumnSpan(this.textBoxLogoGenerator, 7);
            this.textBoxLogoGenerator.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBoxLogoGenerator.Location = new System.Drawing.Point(182, 26);
            this.textBoxLogoGenerator.Margin = new System.Windows.Forms.Padding(1);
            this.textBoxLogoGenerator.Name = "textBoxLogoGenerator";
            this.textBoxLogoGenerator.Size = new System.Drawing.Size(558, 21);
            this.textBoxLogoGenerator.TabIndex = 64;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.BackColor = System.Drawing.Color.Transparent;
            this.label6.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label6.Location = new System.Drawing.Point(1, 3);
            this.label6.Margin = new System.Windows.Forms.Padding(1);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(179, 21);
            this.label6.TabIndex = 30;
            this.label6.Text = "Loại sản phẩm";
            this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // textBoxDescription
            // 
            this.tableLayoutPanelMaster.SetColumnSpan(this.textBoxDescription, 7);
            this.textBoxDescription.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBoxDescription.Location = new System.Drawing.Point(182, 3);
            this.textBoxDescription.Margin = new System.Windows.Forms.Padding(1);
            this.textBoxDescription.Name = "textBoxDescription";
            this.textBoxDescription.Size = new System.Drawing.Size(558, 21);
            this.textBoxDescription.TabIndex = 60;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.BackColor = System.Drawing.Color.Transparent;
            this.label4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label4.Location = new System.Drawing.Point(1, 49);
            this.label4.Margin = new System.Windows.Forms.Padding(1);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(179, 21);
            this.label4.TabIndex = 28;
            this.label4.Text = "Ghi chú";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.BackColor = System.Drawing.Color.Transparent;
            this.label14.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label14.Location = new System.Drawing.Point(1, 26);
            this.label14.Margin = new System.Windows.Forms.Padding(1);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(179, 21);
            this.label14.TabIndex = 51;
            this.label14.Text = "Mã số logo";
            this.label14.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // textBoxRemarks
            // 
            this.tableLayoutPanelMaster.SetColumnSpan(this.textBoxRemarks, 7);
            this.textBoxRemarks.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBoxRemarks.Location = new System.Drawing.Point(182, 49);
            this.textBoxRemarks.Margin = new System.Windows.Forms.Padding(1);
            this.textBoxRemarks.Name = "textBoxRemarks";
            this.textBoxRemarks.Size = new System.Drawing.Size(558, 21);
            this.textBoxRemarks.TabIndex = 14;
            // 
            // MaterialSetID
            // 
            this.MaterialSetID.DataPropertyName = "CommonID";
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.Transparent;
            this.MaterialSetID.DefaultCellStyle = dataGridViewCellStyle1;
            this.MaterialSetID.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.MaterialSetID.HeaderText = "CommonID";
            this.MaterialSetID.Name = "MaterialSetID";
            // 
            // Quantity
            // 
            this.Quantity.DataPropertyName = "CommonValue";
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle2.BackColor = System.Drawing.Color.Transparent;
            dataGridViewCellStyle2.Format = "N0";
            this.Quantity.DefaultCellStyle = dataGridViewCellStyle2;
            this.Quantity.HeaderText = "CommonValue";
            this.Quantity.Name = "Quantity";
            // 
            // Remarks
            // 
            this.Remarks.DataPropertyName = "Remarks";
            dataGridViewCellStyle3.BackColor = System.Drawing.Color.Transparent;
            this.Remarks.DefaultCellStyle = dataGridViewCellStyle3;
            this.Remarks.FillWeight = 112.1169F;
            this.Remarks.HeaderText = "Remarks";
            this.Remarks.Name = "Remarks";
            // 
            // dataListViewMaster
            // 
            this.dataListViewMaster.AllColumns.Add(this.olvColumn4);
            this.dataListViewMaster.AllColumns.Add(this.olvColumn1);
            this.dataListViewMaster.AllColumns.Add(this.olvColumn2);
            this.dataListViewMaster.AllowColumnReorder = true;
            this.dataListViewMaster.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.olvColumn4,
            this.olvColumn1,
            this.olvColumn2});
            this.dataListViewMaster.Cursor = System.Windows.Forms.Cursors.Default;
            this.dataListViewMaster.DataSource = null;
            this.dataListViewMaster.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataListViewMaster.FullRowSelect = true;
            this.dataListViewMaster.HeaderUsesThemes = false;
            this.dataListViewMaster.HeaderWordWrap = true;
            this.dataListViewMaster.HideSelection = false;
            this.dataListViewMaster.Location = new System.Drawing.Point(0, 0);
            this.dataListViewMaster.Name = "dataListViewMaster";
            this.dataListViewMaster.OwnerDraw = true;
            this.dataListViewMaster.RowHeight = 27;
            this.dataListViewMaster.ShowGroups = false;
            this.dataListViewMaster.ShowImagesOnSubItems = true;
            this.dataListViewMaster.ShowItemCountOnGroups = true;
            this.dataListViewMaster.ShowItemToolTips = true;
            this.dataListViewMaster.Size = new System.Drawing.Size(794, 722);
            this.dataListViewMaster.SpaceBetweenGroups = 12;
            this.dataListViewMaster.TabIndex = 25;
            this.dataListViewMaster.UseCompatibleStateImageBehavior = false;
            this.dataListViewMaster.UseFilterIndicator = true;
            this.dataListViewMaster.UseFiltering = true;
            this.dataListViewMaster.UseHotItem = true;
            this.dataListViewMaster.UseTranslucentHotItem = true;
            this.dataListViewMaster.View = System.Windows.Forms.View.Details;
            this.dataListViewMaster.SelectedIndexChanged += new System.EventHandler(this.dataListViewMaster_SelectedIndexChanged);
            this.dataListViewMaster.DoubleClick += new System.EventHandler(this.dataListViewMaster_DoubleClick);
            // 
            // olvColumn4
            // 
            this.olvColumn4.AspectName = "EntryDate";
            this.olvColumn4.AspectToStringFormat = "{0:dd/MM/yy}";
            this.olvColumn4.CellPadding = null;
            this.olvColumn4.HeaderTextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.olvColumn4.Text = "Ngày Tạo";
            this.olvColumn4.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.olvColumn4.Width = 80;
            // 
            // olvColumn1
            // 
            this.olvColumn1.AspectName = "Description";
            this.olvColumn1.CellPadding = null;
            this.olvColumn1.Text = "Tên loại sản phẩm";
            this.olvColumn1.Width = 200;
            // 
            // olvColumn2
            // 
            this.olvColumn2.AspectName = "LogoGenerator";
            this.olvColumn2.CellPadding = null;
            this.olvColumn2.Text = "Mã số logo";
            this.olvColumn2.Width = 150;
            // 
            // ListCategory
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(794, 742);
            this.Controls.Add(this.dataListViewMaster);
            this.Controls.Add(this.naviGroupDetails);
            this.Controls.Add(this.toolStripChildForm);
            this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "ListCategory";
            this.Text = "Danh Mục Loại Sản Phẩm";
            this.Load += new System.EventHandler(this.ListCategory_Load);
            this.toolStripChildForm.ResumeLayout(false);
            this.toolStripChildForm.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.errorProviderMaster)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.naviGroupDetails)).EndInit();
            this.naviGroupDetails.ResumeLayout(false);
            this.naviGroupDetails.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownSizingDetail)).EndInit();
            this.tableLayoutPanelMaster.ResumeLayout(false);
            this.tableLayoutPanelMaster.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataListViewMaster)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ImageList imageListTabControl;
        private System.Windows.Forms.ToolStrip toolStripChildForm;
        private System.Windows.Forms.ToolStripButton toolStripButton2;
        private Guifreaks.Navisuite.NaviGroup naviGroupDetails;
        private System.Windows.Forms.NumericUpDown numericUpDownSizingDetail;
        private System.Windows.Forms.CheckBox checkBoxIsDirtyBLL;
        private System.Windows.Forms.CheckBox checkBoxIsDirty;
        private System.Windows.Forms.TextBox textBoxRemarks;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelMaster;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.ErrorProvider errorProviderMaster;
        private System.Windows.Forms.TextBox textBoxDescription;
        private System.Windows.Forms.DataGridViewComboBoxColumn MaterialSetID;
        private System.Windows.Forms.DataGridViewTextBoxColumn Quantity;
        private System.Windows.Forms.DataGridViewTextBoxColumn Remarks;
        private System.Windows.Forms.TextBox textBoxLogoGenerator;
        private BrightIdeasSoftware.DataListView dataListViewMaster;
        private BrightIdeasSoftware.OLVColumn olvColumn1;
        private BrightIdeasSoftware.OLVColumn olvColumn4;
        private BrightIdeasSoftware.OLVColumn olvColumn2;


    }
}