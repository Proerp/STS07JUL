namespace PresentationLayer
{
    partial class PublicShowDataMessage
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
            this.label3 = new System.Windows.Forms.Label();
            this.textBoxCoilCodeAndExtention = new System.Windows.Forms.TextBox();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.toolStripMDIMain = new System.Windows.Forms.ToolStrip();
            this.toolStripButtonEscape = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparatorPrint = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripButtonOK = new System.Windows.Forms.ToolStripButton();
            this.label1 = new System.Windows.Forms.Label();
            this.textBoxCounterValue = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.textBoxCounterAutonics = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.textBoxDataMessageDisplay = new System.Windows.Forms.TextBox();
            this.labelWarningMessage = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.comboBoxDataStatusID = new System.Windows.Forms.ComboBox();
            this.labelUserInputCounterValue = new System.Windows.Forms.Label();
            this.textBoxUserInputCounterValue = new System.Windows.Forms.TextBox();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.toolStripMDIMain.SuspendLayout();
            this.SuspendLayout();
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.label3.Location = new System.Drawing.Point(90, 131);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(54, 15);
            this.label3.TabIndex = 27;
            this.label3.Text = "Mã cuộn";
            // 
            // textBoxCoilCodeAndExtention
            // 
            this.textBoxCoilCodeAndExtention.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.textBoxCoilCodeAndExtention.Location = new System.Drawing.Point(93, 147);
            this.textBoxCoilCodeAndExtention.Name = "textBoxCoilCodeAndExtention";
            this.textBoxCoilCodeAndExtention.ReadOnly = true;
            this.textBoxCoilCodeAndExtention.Size = new System.Drawing.Size(725, 23);
            this.textBoxCoilCodeAndExtention.TabIndex = 26;
            this.textBoxCoilCodeAndExtention.Text = "991819";
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = global::PresentationLayer.Properties.Resources.Get_Info_B;
            this.pictureBox1.Location = new System.Drawing.Point(35, 61);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(32, 32);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.pictureBox1.TabIndex = 28;
            this.pictureBox1.TabStop = false;
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
            this.toolStripMDIMain.Size = new System.Drawing.Size(834, 39);
            this.toolStripMDIMain.TabIndex = 29;
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
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.label1.Location = new System.Drawing.Point(90, 176);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(73, 15);
            this.label1.TabIndex = 31;
            this.label1.Text = "Số mét đã in";
            // 
            // textBoxCounterValue
            // 
            this.textBoxCounterValue.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.textBoxCounterValue.Location = new System.Drawing.Point(93, 193);
            this.textBoxCounterValue.Name = "textBoxCounterValue";
            this.textBoxCounterValue.ReadOnly = true;
            this.textBoxCounterValue.Size = new System.Drawing.Size(725, 23);
            this.textBoxCounterValue.TabIndex = 30;
            this.textBoxCounterValue.Text = "991819";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.label2.Location = new System.Drawing.Point(90, 221);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(116, 15);
            this.label2.TabIndex = 33;
            this.label2.Text = "Số mét đồng hồ phụ";
            // 
            // textBoxCounterAutonics
            // 
            this.textBoxCounterAutonics.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.textBoxCounterAutonics.Location = new System.Drawing.Point(93, 239);
            this.textBoxCounterAutonics.Name = "textBoxCounterAutonics";
            this.textBoxCounterAutonics.ReadOnly = true;
            this.textBoxCounterAutonics.Size = new System.Drawing.Size(725, 23);
            this.textBoxCounterAutonics.TabIndex = 32;
            this.textBoxCounterAutonics.Text = "991819";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.label4.Location = new System.Drawing.Point(90, 266);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(93, 15);
            this.label4.TabIndex = 35;
            this.label4.Text = "Nội dung bản in";
            // 
            // textBoxDataMessageDisplay
            // 
            this.textBoxDataMessageDisplay.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.textBoxDataMessageDisplay.Location = new System.Drawing.Point(93, 285);
            this.textBoxDataMessageDisplay.Name = "textBoxDataMessageDisplay";
            this.textBoxDataMessageDisplay.ReadOnly = true;
            this.textBoxDataMessageDisplay.Size = new System.Drawing.Size(725, 23);
            this.textBoxDataMessageDisplay.TabIndex = 34;
            this.textBoxDataMessageDisplay.Text = "991819";
            // 
            // labelWarningMessage
            // 
            this.labelWarningMessage.AutoSize = true;
            this.labelWarningMessage.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.labelWarningMessage.ForeColor = System.Drawing.SystemColors.Highlight;
            this.labelWarningMessage.Location = new System.Drawing.Point(90, 61);
            this.labelWarningMessage.Name = "labelWarningMessage";
            this.labelWarningMessage.Size = new System.Drawing.Size(541, 15);
            this.labelWarningMessage.TabIndex = 37;
            this.labelWarningMessage.Text = "Vui lòng xác nhận: IN TIẾP TỤC CUỘN TÔN này. Nhấn OK để tiếp tục in, nhấn Cancel " +
                "để chọn lại sau.";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.label5.ForeColor = System.Drawing.SystemColors.Highlight;
            this.label5.Location = new System.Drawing.Point(90, 313);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(62, 15);
            this.label5.TabIndex = 38;
            this.label5.Text = "Tình trạng";
            // 
            // comboBoxDataStatusID
            // 
            this.comboBoxDataStatusID.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.comboBoxDataStatusID.ForeColor = System.Drawing.SystemColors.Highlight;
            this.comboBoxDataStatusID.FormattingEnabled = true;
            this.comboBoxDataStatusID.Location = new System.Drawing.Point(93, 331);
            this.comboBoxDataStatusID.Name = "comboBoxDataStatusID";
            this.comboBoxDataStatusID.Size = new System.Drawing.Size(725, 23);
            this.comboBoxDataStatusID.TabIndex = 39;
            // 
            // labelUserInputCounterValue
            // 
            this.labelUserInputCounterValue.AutoSize = true;
            this.labelUserInputCounterValue.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.labelUserInputCounterValue.ForeColor = System.Drawing.SystemColors.Highlight;
            this.labelUserInputCounterValue.Location = new System.Drawing.Point(90, 355);
            this.labelUserInputCounterValue.Name = "labelUserInputCounterValue";
            this.labelUserInputCounterValue.Size = new System.Drawing.Size(138, 15);
            this.labelUserInputCounterValue.TabIndex = 41;
            this.labelUserInputCounterValue.Text = "Nhập số mét in tiếp theo";
            // 
            // textBoxUserInputCounterValue
            // 
            this.textBoxUserInputCounterValue.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.textBoxUserInputCounterValue.ForeColor = System.Drawing.SystemColors.Highlight;
            this.textBoxUserInputCounterValue.Location = new System.Drawing.Point(93, 373);
            this.textBoxUserInputCounterValue.Name = "textBoxUserInputCounterValue";
            this.textBoxUserInputCounterValue.Size = new System.Drawing.Size(725, 23);
            this.textBoxUserInputCounterValue.TabIndex = 40;
            this.textBoxUserInputCounterValue.Text = "991819";
            // 
            // PublicShowDataMessage
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(834, 411);
            this.Controls.Add(this.labelUserInputCounterValue);
            this.Controls.Add(this.textBoxUserInputCounterValue);
            this.Controls.Add(this.comboBoxDataStatusID);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.labelWarningMessage);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.textBoxDataMessageDisplay);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.textBoxCounterAutonics);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.textBoxCounterValue);
            this.Controls.Add(this.toolStripMDIMain);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.textBoxCoilCodeAndExtention);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "PublicShowDataMessage";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Thông tin bản in";
            this.Load += new System.EventHandler(this.PublicShowDataMessage_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.toolStripMDIMain.ResumeLayout(false);
            this.toolStripMDIMain.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox textBoxCoilCodeAndExtention;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.ToolStrip toolStripMDIMain;
        private System.Windows.Forms.ToolStripButton toolStripButtonEscape;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparatorPrint;
        private System.Windows.Forms.ToolStripButton toolStripButtonOK;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox textBoxCounterValue;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox textBoxCounterAutonics;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox textBoxDataMessageDisplay;
        private System.Windows.Forms.Label labelWarningMessage;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.ComboBox comboBoxDataStatusID;
        private System.Windows.Forms.Label labelUserInputCounterValue;
        private System.Windows.Forms.TextBox textBoxUserInputCounterValue;
    }
}