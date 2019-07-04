namespace PresentationLayer
{
    partial class PublicAuthenticationPassword
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
            this.labelNewPassword = new System.Windows.Forms.Label();
            this.buttonOK = new System.Windows.Forms.Button();
            this.buttonChange = new System.Windows.Forms.Button();
            this.pictureBoxIcon = new System.Windows.Forms.PictureBox();
            this.groupBoxMainButton = new System.Windows.Forms.GroupBox();
            this.labelCurrentPassword = new System.Windows.Forms.Label();
            this.textBoxCurrentPassword = new System.Windows.Forms.TextBox();
            this.textBoxNewPassword = new System.Windows.Forms.TextBox();
            this.textBoxConfirmPassword = new System.Windows.Forms.TextBox();
            this.labelConfirmPassword = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxIcon)).BeginInit();
            this.groupBoxMainButton.SuspendLayout();
            this.SuspendLayout();
            // 
            // labelNewPassword
            // 
            this.labelNewPassword.AutoSize = true;
            this.labelNewPassword.Location = new System.Drawing.Point(75, 9);
            this.labelNewPassword.Name = "labelNewPassword";
            this.labelNewPassword.Size = new System.Drawing.Size(77, 13);
            this.labelNewPassword.TabIndex = 18;
            this.labelNewPassword.Text = "New password";
            this.labelNewPassword.Visible = false;
            // 
            // buttonOK
            // 
            this.buttonOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.buttonOK.Enabled = false;
            this.buttonOK.Location = new System.Drawing.Point(426, 19);
            this.buttonOK.Name = "buttonOK";
            this.buttonOK.Size = new System.Drawing.Size(66, 26);
            this.buttonOK.TabIndex = 1;
            this.buttonOK.Text = "OK";
            this.buttonOK.UseVisualStyleBackColor = true;
            // 
            // buttonChange
            // 
            this.buttonChange.DialogResult = System.Windows.Forms.DialogResult.Yes;
            this.buttonChange.Enabled = false;
            this.buttonChange.Location = new System.Drawing.Point(355, 19);
            this.buttonChange.Name = "buttonChange";
            this.buttonChange.Size = new System.Drawing.Size(66, 26);
            this.buttonChange.TabIndex = 0;
            this.buttonChange.Text = "Change";
            this.buttonChange.UseVisualStyleBackColor = true;
            this.buttonChange.Click += new System.EventHandler(this.buttonChange_Click);
            // 
            // pictureBoxIcon
            // 
            this.pictureBoxIcon.Image = global::PresentationLayer.Properties.Resources.Oxygen_Icons_org_Oxygen_Apps_preferences_desktop_user_password;
            this.pictureBoxIcon.Location = new System.Drawing.Point(26, 14);
            this.pictureBoxIcon.Name = "pictureBoxIcon";
            this.pictureBoxIcon.Size = new System.Drawing.Size(34, 35);
            this.pictureBoxIcon.TabIndex = 17;
            this.pictureBoxIcon.TabStop = false;
            // 
            // groupBoxMainButton
            // 
            this.groupBoxMainButton.Controls.Add(this.buttonOK);
            this.groupBoxMainButton.Controls.Add(this.buttonChange);
            this.groupBoxMainButton.Location = new System.Drawing.Point(-201, 93);
            this.groupBoxMainButton.Name = "groupBoxMainButton";
            this.groupBoxMainButton.Size = new System.Drawing.Size(644, 67);
            this.groupBoxMainButton.TabIndex = 15;
            this.groupBoxMainButton.TabStop = false;
            // 
            // labelCurrentPassword
            // 
            this.labelCurrentPassword.AutoSize = true;
            this.labelCurrentPassword.Location = new System.Drawing.Point(75, 9);
            this.labelCurrentPassword.Name = "labelCurrentPassword";
            this.labelCurrentPassword.Size = new System.Drawing.Size(79, 13);
            this.labelCurrentPassword.TabIndex = 14;
            this.labelCurrentPassword.Text = "Input password";
            // 
            // textBoxCurrentPassword
            // 
            this.textBoxCurrentPassword.Location = new System.Drawing.Point(78, 28);
            this.textBoxCurrentPassword.Name = "textBoxCurrentPassword";
            this.textBoxCurrentPassword.PasswordChar = '*';
            this.textBoxCurrentPassword.Size = new System.Drawing.Size(213, 20);
            this.textBoxCurrentPassword.TabIndex = 20;
            this.textBoxCurrentPassword.TextChanged += new System.EventHandler(this.textBoxCurrentPassword_TextChanged);
            // 
            // textBoxNewPassword
            // 
            this.textBoxNewPassword.Location = new System.Drawing.Point(78, 26);
            this.textBoxNewPassword.Name = "textBoxNewPassword";
            this.textBoxNewPassword.PasswordChar = '*';
            this.textBoxNewPassword.Size = new System.Drawing.Size(213, 20);
            this.textBoxNewPassword.TabIndex = 21;
            this.textBoxNewPassword.Visible = false;
            this.textBoxNewPassword.TextChanged += new System.EventHandler(this.textBoxNewPassword_TextChanged);
            // 
            // textBoxConfirmPassword
            // 
            this.textBoxConfirmPassword.Location = new System.Drawing.Point(78, 67);
            this.textBoxConfirmPassword.Name = "textBoxConfirmPassword";
            this.textBoxConfirmPassword.PasswordChar = '*';
            this.textBoxConfirmPassword.Size = new System.Drawing.Size(213, 20);
            this.textBoxConfirmPassword.TabIndex = 23;
            this.textBoxConfirmPassword.Visible = false;
            this.textBoxConfirmPassword.TextChanged += new System.EventHandler(this.textBoxConfirmPassword_TextChanged);
            // 
            // labelConfirmPassword
            // 
            this.labelConfirmPassword.AutoSize = true;
            this.labelConfirmPassword.Location = new System.Drawing.Point(75, 50);
            this.labelConfirmPassword.Name = "labelConfirmPassword";
            this.labelConfirmPassword.Size = new System.Drawing.Size(113, 13);
            this.labelConfirmPassword.TabIndex = 22;
            this.labelConfirmPassword.Text = "Confirm new password";
            this.labelConfirmPassword.Visible = false;
            // 
            // PublicAuthenticationPassword
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(307, 146);
            this.Controls.Add(this.textBoxConfirmPassword);
            this.Controls.Add(this.labelConfirmPassword);
            this.Controls.Add(this.textBoxNewPassword);
            this.Controls.Add(this.textBoxCurrentPassword);
            this.Controls.Add(this.labelNewPassword);
            this.Controls.Add(this.pictureBoxIcon);
            this.Controls.Add(this.groupBoxMainButton);
            this.Controls.Add(this.labelCurrentPassword);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "PublicAuthenticationPassword";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Authentication";
            this.Load += new System.EventHandler(this.PublicAuthenticationPassword_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxIcon)).EndInit();
            this.groupBoxMainButton.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label labelNewPassword;
        private System.Windows.Forms.Button buttonOK;
        private System.Windows.Forms.Button buttonChange;
        private System.Windows.Forms.PictureBox pictureBoxIcon;
        private System.Windows.Forms.GroupBox groupBoxMainButton;
        private System.Windows.Forms.Label labelCurrentPassword;
        private System.Windows.Forms.TextBox textBoxCurrentPassword;
        private System.Windows.Forms.TextBox textBoxNewPassword;
        private System.Windows.Forms.TextBox textBoxConfirmPassword;
        private System.Windows.Forms.Label labelConfirmPassword;
    }
}