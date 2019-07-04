namespace PresentationLayer
{
    partial class SettingFillingLineData
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SettingFillingLineData));
            this.comboBoxProductID = new System.Windows.Forms.ComboBox();
            this.groupBoxMainButton = new System.Windows.Forms.GroupBox();
            this.labelWarningNewMonth = new System.Windows.Forms.Label();
            this.pictureBoxWarningNewMonth = new System.Windows.Forms.PictureBox();
            this.buttonCancel = new System.Windows.Forms.Button();
            this.buttonOK = new System.Windows.Forms.Button();
            this.labelFillingLineID = new System.Windows.Forms.Label();
            this.textBoxFillingLineName = new System.Windows.Forms.TextBox();
            this.dateTimePickerSettingDate = new System.Windows.Forms.DateTimePicker();
            this.textBoxBatchNo = new System.Windows.Forms.TextBox();
            this.textBoxBatchSerialNumber = new System.Windows.Forms.TextBox();
            this.textBoxMonthSerialNumber = new System.Windows.Forms.TextBox();
            this.textBoxBatchCartonNumber = new System.Windows.Forms.TextBox();
            this.textBoxMonthCartonNumber = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.timerEverySecond = new System.Windows.Forms.Timer(this.components);
            this.checkBoxSaveAgreement = new System.Windows.Forms.CheckBox();
            this.groupBoxMainButton.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxWarningNewMonth)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // comboBoxProductID
            // 
            this.comboBoxProductID.FormattingEnabled = true;
            this.comboBoxProductID.Location = new System.Drawing.Point(105, 42);
            this.comboBoxProductID.Name = "comboBoxProductID";
            this.comboBoxProductID.Size = new System.Drawing.Size(362, 21);
            this.comboBoxProductID.TabIndex = 14;
            // 
            // groupBoxMainButton
            // 
            this.groupBoxMainButton.Controls.Add(this.labelWarningNewMonth);
            this.groupBoxMainButton.Controls.Add(this.pictureBoxWarningNewMonth);
            this.groupBoxMainButton.Controls.Add(this.buttonCancel);
            this.groupBoxMainButton.Controls.Add(this.buttonOK);
            this.groupBoxMainButton.Location = new System.Drawing.Point(-54, 209);
            this.groupBoxMainButton.Name = "groupBoxMainButton";
            this.groupBoxMainButton.Size = new System.Drawing.Size(749, 67);
            this.groupBoxMainButton.TabIndex = 13;
            this.groupBoxMainButton.TabStop = false;
            // 
            // labelWarningNewMonth
            // 
            this.labelWarningNewMonth.AutoSize = true;
            this.labelWarningNewMonth.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelWarningNewMonth.ForeColor = System.Drawing.Color.Green;
            this.labelWarningNewMonth.Location = new System.Drawing.Point(84, 30);
            this.labelWarningNewMonth.Name = "labelWarningNewMonth";
            this.labelWarningNewMonth.Size = new System.Drawing.Size(140, 13);
            this.labelWarningNewMonth.TabIndex = 25;
            this.labelWarningNewMonth.Text = "Please Check Printing Date.";
            this.labelWarningNewMonth.Visible = false;
            // 
            // pictureBoxWarningNewMonth
            // 
            this.pictureBoxWarningNewMonth.Image = ((System.Drawing.Image)(resources.GetObject("pictureBoxWarningNewMonth.Image")));
            this.pictureBoxWarningNewMonth.Location = new System.Drawing.Point(56, 19);
            this.pictureBoxWarningNewMonth.Name = "pictureBoxWarningNewMonth";
            this.pictureBoxWarningNewMonth.Size = new System.Drawing.Size(31, 31);
            this.pictureBoxWarningNewMonth.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBoxWarningNewMonth.TabIndex = 24;
            this.pictureBoxWarningNewMonth.TabStop = false;
            this.pictureBoxWarningNewMonth.Visible = false;
            // 
            // buttonCancel
            // 
            this.buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.buttonCancel.Location = new System.Drawing.Point(630, 23);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(66, 26);
            this.buttonCancel.TabIndex = 1;
            this.buttonCancel.Text = "Cancel";
            this.buttonCancel.UseVisualStyleBackColor = true;
            // 
            // buttonOK
            // 
            this.buttonOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.buttonOK.Location = new System.Drawing.Point(558, 23);
            this.buttonOK.Name = "buttonOK";
            this.buttonOK.Size = new System.Drawing.Size(66, 26);
            this.buttonOK.TabIndex = 0;
            this.buttonOK.Text = "OK";
            this.buttonOK.UseVisualStyleBackColor = true;
            this.buttonOK.Click += new System.EventHandler(this.buttonOK_Click);
            // 
            // labelFillingLineID
            // 
            this.labelFillingLineID.AutoSize = true;
            this.labelFillingLineID.Location = new System.Drawing.Point(102, 26);
            this.labelFillingLineID.Name = "labelFillingLineID";
            this.labelFillingLineID.Size = new System.Drawing.Size(44, 13);
            this.labelFillingLineID.TabIndex = 12;
            this.labelFillingLineID.Text = "Product";
            // 
            // textBoxFillingLineName
            // 
            this.textBoxFillingLineName.Location = new System.Drawing.Point(-132, 140);
            this.textBoxFillingLineName.Name = "textBoxFillingLineName";
            this.textBoxFillingLineName.ReadOnly = true;
            this.textBoxFillingLineName.Size = new System.Drawing.Size(202, 20);
            this.textBoxFillingLineName.TabIndex = 16;
            this.textBoxFillingLineName.Visible = false;
            // 
            // dateTimePickerSettingDate
            // 
            this.dateTimePickerSettingDate.Location = new System.Drawing.Point(105, 140);
            this.dateTimePickerSettingDate.Name = "dateTimePickerSettingDate";
            this.dateTimePickerSettingDate.Size = new System.Drawing.Size(362, 20);
            this.dateTimePickerSettingDate.TabIndex = 17;
            // 
            // textBoxBatchNo
            // 
            this.textBoxBatchNo.Location = new System.Drawing.Point(105, 91);
            this.textBoxBatchNo.Name = "textBoxBatchNo";
            this.textBoxBatchNo.Size = new System.Drawing.Size(362, 20);
            this.textBoxBatchNo.TabIndex = 18;
            this.textBoxBatchNo.Text = "991819";
            // 
            // textBoxBatchSerialNumber
            // 
            this.textBoxBatchSerialNumber.Location = new System.Drawing.Point(492, 90);
            this.textBoxBatchSerialNumber.Name = "textBoxBatchSerialNumber";
            this.textBoxBatchSerialNumber.Size = new System.Drawing.Size(150, 20);
            this.textBoxBatchSerialNumber.TabIndex = 19;
            this.textBoxBatchSerialNumber.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // textBoxMonthSerialNumber
            // 
            this.textBoxMonthSerialNumber.Location = new System.Drawing.Point(491, 43);
            this.textBoxMonthSerialNumber.Name = "textBoxMonthSerialNumber";
            this.textBoxMonthSerialNumber.Size = new System.Drawing.Size(150, 20);
            this.textBoxMonthSerialNumber.TabIndex = 20;
            this.textBoxMonthSerialNumber.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // textBoxBatchCartonNumber
            // 
            this.textBoxBatchCartonNumber.Location = new System.Drawing.Point(492, 188);
            this.textBoxBatchCartonNumber.Name = "textBoxBatchCartonNumber";
            this.textBoxBatchCartonNumber.Size = new System.Drawing.Size(150, 20);
            this.textBoxBatchCartonNumber.TabIndex = 21;
            this.textBoxBatchCartonNumber.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // textBoxMonthCartonNumber
            // 
            this.textBoxMonthCartonNumber.Location = new System.Drawing.Point(492, 140);
            this.textBoxMonthCartonNumber.Name = "textBoxMonthCartonNumber";
            this.textBoxMonthCartonNumber.Size = new System.Drawing.Size(150, 20);
            this.textBoxMonthCartonNumber.TabIndex = 22;
            this.textBoxMonthCartonNumber.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(14, 123);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(56, 13);
            this.label1.TabIndex = 23;
            this.label1.Text = "Filling Line";
            this.label1.Visible = false;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(102, 123);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(68, 13);
            this.label2.TabIndex = 24;
            this.label2.Text = "Printing Date";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(102, 74);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(75, 13);
            this.label3.TabIndex = 25;
            this.label3.Text = "Batch Number";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(489, 74);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(103, 13);
            this.label4.TabIndex = 26;
            this.label4.Text = "Batch Pack Number";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(489, 27);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(112, 13);
            this.label5.TabIndex = 27;
            this.label5.Text = "Monthly Pack Number";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(489, 171);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(109, 13);
            this.label6.TabIndex = 28;
            this.label6.Text = "Batch Carton Number";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(489, 123);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(118, 13);
            this.label7.TabIndex = 29;
            this.label7.Text = "Monthly Carton Number";
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox1.Image")));
            this.pictureBox1.Location = new System.Drawing.Point(22, 15);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(48, 48);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.pictureBox1.TabIndex = 15;
            this.pictureBox1.TabStop = false;
            // 
            // timerEverySecond
            // 
            this.timerEverySecond.Enabled = true;
            this.timerEverySecond.Interval = 1000;
            this.timerEverySecond.Tick += new System.EventHandler(this.timerEverySecond_Tick);
            // 
            // checkBoxSaveAgreement
            // 
            this.checkBoxSaveAgreement.AutoSize = true;
            this.checkBoxSaveAgreement.ForeColor = System.Drawing.SystemColors.HotTrack;
            this.checkBoxSaveAgreement.Location = new System.Drawing.Point(105, 183);
            this.checkBoxSaveAgreement.Name = "checkBoxSaveAgreement";
            this.checkBoxSaveAgreement.Size = new System.Drawing.Size(109, 17);
            this.checkBoxSaveAgreement.TabIndex = 30;
            this.checkBoxSaveAgreement.Text = "I want to change.";
            this.checkBoxSaveAgreement.UseVisualStyleBackColor = true;
            // 
            // SettingFillingLineData
            // 
            this.AcceptButton = this.buttonOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.buttonCancel;
            this.ClientSize = new System.Drawing.Size(657, 270);
            this.ControlBox = false;
            this.Controls.Add(this.checkBoxSaveAgreement);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.textBoxMonthCartonNumber);
            this.Controls.Add(this.textBoxBatchCartonNumber);
            this.Controls.Add(this.textBoxMonthSerialNumber);
            this.Controls.Add(this.textBoxBatchSerialNumber);
            this.Controls.Add(this.textBoxBatchNo);
            this.Controls.Add(this.dateTimePickerSettingDate);
            this.Controls.Add(this.textBoxFillingLineName);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.comboBoxProductID);
            this.Controls.Add(this.groupBoxMainButton);
            this.Controls.Add(this.labelFillingLineID);
            this.Name = "SettingFillingLineData";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Setting Data";
            this.groupBoxMainButton.ResumeLayout(false);
            this.groupBoxMainButton.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxWarningNewMonth)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.ComboBox comboBoxProductID;
        private System.Windows.Forms.GroupBox groupBoxMainButton;
        private System.Windows.Forms.Button buttonCancel;
        private System.Windows.Forms.Button buttonOK;
        private System.Windows.Forms.Label labelFillingLineID;
        private System.Windows.Forms.TextBox textBoxFillingLineName;
        private System.Windows.Forms.DateTimePicker dateTimePickerSettingDate;
        private System.Windows.Forms.TextBox textBoxBatchNo;
        private System.Windows.Forms.TextBox textBoxBatchSerialNumber;
        private System.Windows.Forms.TextBox textBoxMonthSerialNumber;
        private System.Windows.Forms.TextBox textBoxBatchCartonNumber;
        private System.Windows.Forms.TextBox textBoxMonthCartonNumber;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label labelWarningNewMonth;
        private System.Windows.Forms.PictureBox pictureBoxWarningNewMonth;
        private System.Windows.Forms.Timer timerEverySecond;
        private System.Windows.Forms.CheckBox checkBoxSaveAgreement;
    }
}