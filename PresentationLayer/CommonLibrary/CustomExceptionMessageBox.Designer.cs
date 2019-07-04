namespace PresentationLayer
{
    partial class CustomExceptionMessageBox
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CustomExceptionMessageBox));
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.dataListViewExceptionTable = new BrightIdeasSoftware.DataListView();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.buttonOK = new System.Windows.Forms.Button();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.labelExceptionMessage = new System.Windows.Forms.Label();
            this.tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataListViewExceptionTable)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Controls.Add(this.dataListViewExceptionTable, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.groupBox1, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel2, 0, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 3;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 17.90123F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 82.09876F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 52F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(793, 512);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // dataListViewExceptionTable
            // 
            this.dataListViewExceptionTable.Cursor = System.Windows.Forms.Cursors.Default;
            this.dataListViewExceptionTable.DataSource = null;
            this.dataListViewExceptionTable.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataListViewExceptionTable.FullRowSelect = true;
            this.dataListViewExceptionTable.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.dataListViewExceptionTable.HeaderUsesThemes = false;
            this.dataListViewExceptionTable.HeaderWordWrap = true;
            this.dataListViewExceptionTable.HideSelection = false;
            this.dataListViewExceptionTable.Location = new System.Drawing.Point(3, 85);
            this.dataListViewExceptionTable.Name = "dataListViewExceptionTable";
            this.dataListViewExceptionTable.OwnerDraw = true;
            this.dataListViewExceptionTable.RowHeight = 25;
            this.dataListViewExceptionTable.ShowImagesOnSubItems = true;
            this.dataListViewExceptionTable.ShowItemCountOnGroups = true;
            this.dataListViewExceptionTable.ShowItemToolTips = true;
            this.dataListViewExceptionTable.Size = new System.Drawing.Size(787, 371);
            this.dataListViewExceptionTable.SpaceBetweenGroups = 12;
            this.dataListViewExceptionTable.TabIndex = 5;
            this.dataListViewExceptionTable.UseCompatibleStateImageBehavior = false;
            this.dataListViewExceptionTable.UseFilterIndicator = true;
            this.dataListViewExceptionTable.UseFiltering = true;
            this.dataListViewExceptionTable.UseHotItem = true;
            this.dataListViewExceptionTable.UseTranslucentHotItem = true;
            this.dataListViewExceptionTable.View = System.Windows.Forms.View.Details;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.buttonOK);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox1.Location = new System.Drawing.Point(0, 462);
            this.groupBox1.Margin = new System.Windows.Forms.Padding(0, 3, 0, 0);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Padding = new System.Windows.Forms.Padding(0);
            this.groupBox1.Size = new System.Drawing.Size(793, 50);
            this.groupBox1.TabIndex = 6;
            this.groupBox1.TabStop = false;
            // 
            // buttonOK
            // 
            this.buttonOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.buttonOK.Location = new System.Drawing.Point(715, 16);
            this.buttonOK.Name = "buttonOK";
            this.buttonOK.Size = new System.Drawing.Size(66, 26);
            this.buttonOK.TabIndex = 2;
            this.buttonOK.Text = "OK";
            this.buttonOK.UseVisualStyleBackColor = true;
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.ColumnCount = 3;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 16F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 55F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.Controls.Add(this.pictureBox1, 1, 1);
            this.tableLayoutPanel2.Controls.Add(this.labelExceptionMessage, 2, 1);
            this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel2.Location = new System.Drawing.Point(3, 3);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 2;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 13F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 91F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(787, 76);
            this.tableLayoutPanel2.TabIndex = 7;
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox1.Image")));
            this.pictureBox1.Location = new System.Drawing.Point(19, 16);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(49, 52);
            this.pictureBox1.TabIndex = 7;
            this.pictureBox1.TabStop = false;
            // 
            // labelExceptionMessage
            // 
            this.labelExceptionMessage.AutoSize = true;
            this.labelExceptionMessage.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelExceptionMessage.Location = new System.Drawing.Point(83, 13);
            this.labelExceptionMessage.Margin = new System.Windows.Forms.Padding(12, 0, 3, 0);
            this.labelExceptionMessage.Name = "labelExceptionMessage";
            this.labelExceptionMessage.Padding = new System.Windows.Forms.Padding(3);
            this.labelExceptionMessage.Size = new System.Drawing.Size(701, 91);
            this.labelExceptionMessage.TabIndex = 8;
            this.labelExceptionMessage.Text = "label1";
            // 
            // CustomExceptionMessageBox
            // 
            this.AcceptButton = this.buttonOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(793, 512);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Name = "CustomExceptionMessageBox";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Message Box";
            this.tableLayoutPanel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataListViewExceptionTable)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.tableLayoutPanel2.ResumeLayout(false);
            this.tableLayoutPanel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private BrightIdeasSoftware.DataListView dataListViewExceptionTable;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button buttonOK;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Label labelExceptionMessage;
    }
}