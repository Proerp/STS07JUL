namespace PresentationLayer
{
    partial class PublicFind
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
            this.toolStripMDIMain = new System.Windows.Forms.ToolStrip();
            this.toolStripButtonEscape = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparatorPrint = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripButtonOK = new System.Windows.Forms.ToolStripButton();
            this.label02 = new System.Windows.Forms.Label();
            this.label01 = new System.Windows.Forms.Label();
            this.textBoxFilterText = new System.Windows.Forms.TextBox();
            this.pictureBoxIcon = new System.Windows.Forms.PictureBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.checkBoxMatchWholeWord = new System.Windows.Forms.CheckBox();
            this.checkBoxMatchCase = new System.Windows.Forms.CheckBox();
            this.comboBoxFilterColumnID = new System.Windows.Forms.ComboBox();
            this.toolStripMDIMain.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxIcon)).BeginInit();
            this.groupBox1.SuspendLayout();
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
            this.toolStripMDIMain.Size = new System.Drawing.Size(445, 39);
            this.toolStripMDIMain.TabIndex = 31;
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
            this.toolStripButtonOK.Image = global::PresentationLayer.Properties.Resources.Oxygen_Icons_org_Oxygen_Actions_edit_find;
            this.toolStripButtonOK.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.toolStripButtonOK.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonOK.Name = "toolStripButtonOK";
            this.toolStripButtonOK.Size = new System.Drawing.Size(93, 36);
            this.toolStripButtonOK.Text = "Tìm kiếm";
            this.toolStripButtonOK.Click += new System.EventHandler(this.toolStripButtonEscapeAndOK_Click);
            // 
            // label02
            // 
            this.label02.AutoSize = true;
            this.label02.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.label02.ForeColor = System.Drawing.SystemColors.ControlText;
            this.label02.Location = new System.Drawing.Point(120, 107);
            this.label02.Name = "label02";
            this.label02.Size = new System.Drawing.Size(114, 15);
            this.label02.TabIndex = 40;
            this.label02.Text = "Tìm theo cột dữ liệu";
            // 
            // label01
            // 
            this.label01.AutoSize = true;
            this.label01.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.label01.ForeColor = System.Drawing.SystemColors.ControlText;
            this.label01.Location = new System.Drawing.Point(120, 53);
            this.label01.Name = "label01";
            this.label01.Size = new System.Drawing.Size(130, 15);
            this.label01.TabIndex = 43;
            this.label01.Text = "Nhập nội dung cần tìm";
            // 
            // textBoxFilterText
            // 
            this.textBoxFilterText.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.textBoxFilterText.Location = new System.Drawing.Point(123, 71);
            this.textBoxFilterText.Name = "textBoxFilterText";
            this.textBoxFilterText.Size = new System.Drawing.Size(308, 23);
            this.textBoxFilterText.TabIndex = 42;
            this.textBoxFilterText.TextChanged += new System.EventHandler(this.textBoxFilterText_TextChanged);
            // 
            // pictureBoxIcon
            // 
            this.pictureBoxIcon.Image = global::PresentationLayer.Properties.Resources.Oxygen_Icons_org_Oxygen_Actions_system_search;
            this.pictureBoxIcon.Location = new System.Drawing.Point(53, 53);
            this.pictureBoxIcon.Name = "pictureBoxIcon";
            this.pictureBoxIcon.Size = new System.Drawing.Size(35, 41);
            this.pictureBoxIcon.TabIndex = 44;
            this.pictureBoxIcon.TabStop = false;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.checkBoxMatchWholeWord);
            this.groupBox1.Controls.Add(this.checkBoxMatchCase);
            this.groupBox1.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox1.Location = new System.Drawing.Point(123, 158);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(308, 73);
            this.groupBox1.TabIndex = 45;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Tùy chọn";
            // 
            // checkBoxMatchWholeWord
            // 
            this.checkBoxMatchWholeWord.AutoSize = true;
            this.checkBoxMatchWholeWord.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.checkBoxMatchWholeWord.Location = new System.Drawing.Point(31, 22);
            this.checkBoxMatchWholeWord.Name = "checkBoxMatchWholeWord";
            this.checkBoxMatchWholeWord.Size = new System.Drawing.Size(151, 19);
            this.checkBoxMatchWholeWord.TabIndex = 1;
            this.checkBoxMatchWholeWord.Text = "Tìm chính xác nội dung";
            this.checkBoxMatchWholeWord.UseVisualStyleBackColor = true;
            // 
            // checkBoxMatchCase
            // 
            this.checkBoxMatchCase.AutoSize = true;
            this.checkBoxMatchCase.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.checkBoxMatchCase.Location = new System.Drawing.Point(31, 45);
            this.checkBoxMatchCase.Name = "checkBoxMatchCase";
            this.checkBoxMatchCase.Size = new System.Drawing.Size(202, 19);
            this.checkBoxMatchCase.TabIndex = 0;
            this.checkBoxMatchCase.Text = "Phân biệt chữ thường và chữ hoa";
            this.checkBoxMatchCase.UseVisualStyleBackColor = true;
            // 
            // comboBoxFilterColumnID
            // 
            this.comboBoxFilterColumnID.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxFilterColumnID.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.comboBoxFilterColumnID.FormattingEnabled = true;
            this.comboBoxFilterColumnID.Location = new System.Drawing.Point(123, 125);
            this.comboBoxFilterColumnID.Name = "comboBoxFilterColumnID";
            this.comboBoxFilterColumnID.Size = new System.Drawing.Size(308, 23);
            this.comboBoxFilterColumnID.TabIndex = 46;
            // 
            // PublicFind
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(445, 245);
            this.Controls.Add(this.comboBoxFilterColumnID);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.pictureBoxIcon);
            this.Controls.Add(this.label01);
            this.Controls.Add(this.textBoxFilterText);
            this.Controls.Add(this.label02);
            this.Controls.Add(this.toolStripMDIMain);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "PublicFind";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Tìm kiếm";
            this.TopMost = true;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.PublicFind_FormClosing);
            this.toolStripMDIMain.ResumeLayout(false);
            this.toolStripMDIMain.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxIcon)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolStrip toolStripMDIMain;
        private System.Windows.Forms.ToolStripButton toolStripButtonEscape;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparatorPrint;
        private System.Windows.Forms.ToolStripButton toolStripButtonOK;
        private System.Windows.Forms.Label label02;
        private System.Windows.Forms.Label label01;
        private System.Windows.Forms.TextBox textBoxFilterText;
        private System.Windows.Forms.PictureBox pictureBoxIcon;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.CheckBox checkBoxMatchWholeWord;
        private System.Windows.Forms.CheckBox checkBoxMatchCase;
        private System.Windows.Forms.ComboBox comboBoxFilterColumnID;
    }
}