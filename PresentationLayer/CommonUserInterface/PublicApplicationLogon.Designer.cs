namespace PresentationLayer
{
    partial class PublicApplicationLogon
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PublicApplicationLogon));
            this.groupBoxMainButton = new System.Windows.Forms.GroupBox();
            this.buttonListEmployee = new System.Windows.Forms.Button();
            this.buttonCancel = new System.Windows.Forms.Button();
            this.buttonOK = new System.Windows.Forms.Button();
            this.labelFillingLineID = new System.Windows.Forms.Label();
            this.pictureBoxIcon = new System.Windows.Forms.PictureBox();
            this.comboBoxImageS8PortName = new System.Windows.Forms.ComboBox();
            this.labelPortName = new System.Windows.Forms.Label();
            this.comboBoxEmployeeID = new System.Windows.Forms.ComboBox();
            this.comboBoxAutonicsPortName = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.textBoxPassword = new System.Windows.Forms.TextBox();
            this.labelChangePassword = new System.Windows.Forms.Label();
            this.groupBoxMainButton.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxIcon)).BeginInit();
            this.SuspendLayout();
            // 
            // groupBoxMainButton
            // 
            this.groupBoxMainButton.Controls.Add(this.buttonListEmployee);
            this.groupBoxMainButton.Controls.Add(this.buttonCancel);
            this.groupBoxMainButton.Controls.Add(this.buttonOK);
            this.groupBoxMainButton.Location = new System.Drawing.Point(-313, 137);
            this.groupBoxMainButton.Name = "groupBoxMainButton";
            this.groupBoxMainButton.Size = new System.Drawing.Size(714, 67);
            this.groupBoxMainButton.TabIndex = 9;
            this.groupBoxMainButton.TabStop = false;
            // 
            // buttonListEmployee
            // 
            this.buttonListEmployee.Image = ((System.Drawing.Image)(resources.GetObject("buttonListEmployee.Image")));
            this.buttonListEmployee.Location = new System.Drawing.Point(325, 19);
            this.buttonListEmployee.Name = "buttonListEmployee";
            this.buttonListEmployee.Size = new System.Drawing.Size(25, 23);
            this.buttonListEmployee.TabIndex = 2;
            this.buttonListEmployee.UseVisualStyleBackColor = true;
            this.buttonListEmployee.Visible = false;
            this.buttonListEmployee.Click += new System.EventHandler(this.buttonOK_Click);
            // 
            // buttonCancel
            // 
            this.buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.buttonCancel.Location = new System.Drawing.Point(617, 19);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(66, 26);
            this.buttonCancel.TabIndex = 1;
            this.buttonCancel.Text = "Cancel";
            this.buttonCancel.UseVisualStyleBackColor = true;
            // 
            // buttonOK
            // 
            this.buttonOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.buttonOK.Location = new System.Drawing.Point(547, 19);
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
            this.labelFillingLineID.Location = new System.Drawing.Point(123, 16);
            this.labelFillingLineID.Name = "labelFillingLineID";
            this.labelFillingLineID.Size = new System.Drawing.Size(124, 13);
            this.labelFillingLineID.TabIndex = 5;
            this.labelFillingLineID.Text = "Họ và tên người sử dụng";
            // 
            // pictureBoxIcon
            // 
            this.pictureBoxIcon.Image = ((System.Drawing.Image)(resources.GetObject("pictureBoxIcon.Image")));
            this.pictureBoxIcon.Location = new System.Drawing.Point(26, 18);
            this.pictureBoxIcon.Margin = new System.Windows.Forms.Padding(0);
            this.pictureBoxIcon.Name = "pictureBoxIcon";
            this.pictureBoxIcon.Size = new System.Drawing.Size(67, 63);
            this.pictureBoxIcon.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBoxIcon.TabIndex = 11;
            this.pictureBoxIcon.TabStop = false;
            this.pictureBoxIcon.DoubleClick += new System.EventHandler(this.pictureBoxIcon_DoubleClick);
            // 
            // comboBoxImageS8PortName
            // 
            this.comboBoxImageS8PortName.FormattingEnabled = true;
            this.comboBoxImageS8PortName.Location = new System.Drawing.Point(126, 110);
            this.comboBoxImageS8PortName.Name = "comboBoxImageS8PortName";
            this.comboBoxImageS8PortName.Size = new System.Drawing.Size(117, 21);
            this.comboBoxImageS8PortName.TabIndex = 13;
            this.comboBoxImageS8PortName.Visible = false;
            // 
            // labelPortName
            // 
            this.labelPortName.AutoSize = true;
            this.labelPortName.Location = new System.Drawing.Point(123, 94);
            this.labelPortName.Name = "labelPortName";
            this.labelPortName.Size = new System.Drawing.Size(46, 13);
            this.labelPortName.TabIndex = 12;
            this.labelPortName.Text = "Comport";
            this.labelPortName.Visible = false;
            // 
            // comboBoxEmployeeID
            // 
            this.comboBoxEmployeeID.FormattingEnabled = true;
            this.comboBoxEmployeeID.Location = new System.Drawing.Point(126, 30);
            this.comboBoxEmployeeID.Name = "comboBoxEmployeeID";
            this.comboBoxEmployeeID.Size = new System.Drawing.Size(244, 21);
            this.comboBoxEmployeeID.TabIndex = 14;
            // 
            // comboBoxAutonicsPortName
            // 
            this.comboBoxAutonicsPortName.FormattingEnabled = true;
            this.comboBoxAutonicsPortName.Location = new System.Drawing.Point(249, 110);
            this.comboBoxAutonicsPortName.Name = "comboBoxAutonicsPortName";
            this.comboBoxAutonicsPortName.Size = new System.Drawing.Size(121, 21);
            this.comboBoxAutonicsPortName.TabIndex = 15;
            this.comboBoxAutonicsPortName.Visible = false;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(123, 54);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(52, 13);
            this.label1.TabIndex = 16;
            this.label1.Text = "Mật khẩu";
            // 
            // textBoxPassword
            // 
            this.textBoxPassword.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBoxPassword.Location = new System.Drawing.Point(127, 70);
            this.textBoxPassword.Name = "textBoxPassword";
            this.textBoxPassword.PasswordChar = '*';
            this.textBoxPassword.Size = new System.Drawing.Size(243, 22);
            this.textBoxPassword.TabIndex = 17;
            // 
            // labelChangePassword
            // 
            this.labelChangePassword.AutoSize = true;
            this.labelChangePassword.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelChangePassword.ForeColor = System.Drawing.SystemColors.Highlight;
            this.labelChangePassword.Location = new System.Drawing.Point(124, 113);
            this.labelChangePassword.Name = "labelChangePassword";
            this.labelChangePassword.Size = new System.Drawing.Size(153, 13);
            this.labelChangePassword.TabIndex = 18;
            this.labelChangePassword.Text = "Click vào đây để đổi mật khẩu";
            this.labelChangePassword.Click += new System.EventHandler(this.buttonOK_Click);
            // 
            // PublicApplicationLogon
            // 
            this.AcceptButton = this.buttonOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.buttonCancel;
            this.ClientSize = new System.Drawing.Size(385, 190);
            this.ControlBox = false;
            this.Controls.Add(this.labelChangePassword);
            this.Controls.Add(this.textBoxPassword);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.comboBoxAutonicsPortName);
            this.Controls.Add(this.comboBoxEmployeeID);
            this.Controls.Add(this.comboBoxImageS8PortName);
            this.Controls.Add(this.labelPortName);
            this.Controls.Add(this.pictureBoxIcon);
            this.Controls.Add(this.groupBoxMainButton);
            this.Controls.Add(this.labelFillingLineID);
            this.Name = "PublicApplicationLogon";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Đăng nhập";
            this.Load += new System.EventHandler(this.PublicApplicationLogon_Load);
            this.groupBoxMainButton.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxIcon)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBoxMainButton;
        private System.Windows.Forms.Button buttonCancel;
        private System.Windows.Forms.Button buttonOK;
        private System.Windows.Forms.Label labelFillingLineID;
        private System.Windows.Forms.PictureBox pictureBoxIcon;
        private System.Windows.Forms.ComboBox comboBoxImageS8PortName;
        private System.Windows.Forms.Label labelPortName;
        private System.Windows.Forms.ComboBox comboBoxEmployeeID;
        private System.Windows.Forms.ComboBox comboBoxAutonicsPortName;
        private System.Windows.Forms.Button buttonListEmployee;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox textBoxPassword;
        private System.Windows.Forms.Label labelChangePassword;
    }
}