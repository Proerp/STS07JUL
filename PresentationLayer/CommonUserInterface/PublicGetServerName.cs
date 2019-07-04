using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Global.Class.Library;

namespace PresentationLayer
{
    public partial class PublicGetServerName : Form
    {
        public PublicGetServerName()
        {
            InitializeComponent();
            this.buttonOK.DialogResult = DialogResult.OK;
            this.buttonCancel.DialogResult = DialogResult.Cancel;
            this.AcceptButton = this.buttonOK;
            this.CancelButton = this.buttonCancel;
        }

        private void PublicGetServerName_Load(object sender, EventArgs e)
        {
            try
            {
                this.textBoxServerName.Text = GlobalRegistry.Read("ServerName");
                this.textBoxDatabaseName.Text = GlobalRegistry.Read("DatabaseName");
            }
            catch (Exception ex)
            {
                GlobalExceptionHandler.ShowExceptionMessageBox(this, ex);
            }
        }

        private void buttonOK_Click(object sender, EventArgs e)
        {
            try
            {
                GlobalRegistry.Write("ServerName", this.textBoxServerName.Text);
                GlobalRegistry.Write("DatabaseName", this.textBoxDatabaseName.Text);

                GlobalMsADO.ServerName = this.textBoxServerName.Text;
                GlobalMsADO.DatabaseName = this.textBoxDatabaseName.Text;

                return;

                //http://www.codeproject.com/Articles/11967/Adding-custom-dialogs-to-your-applications


            }
            catch (Exception ex)
            {
                GlobalExceptionHandler.ShowExceptionMessageBox(this, ex);
            }
        }
    }
}
