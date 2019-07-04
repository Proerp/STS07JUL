using System;
using System.IO;
using System.Reflection;
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
    public partial class frmMDIMain : Form
    {


        #region Delegate test for call Save Action

        public delegate void SaveDelegate(object sender, EventArgs e);
        public event SaveDelegate Save;

        #endregion Delegate test for call Save Action

        #region Contractor
        public frmMDIMain()
        {
            InitializeComponent();
        }
        #endregion Contractor

        #region Form Events
        private void frmMDIMain_Load(object sender, EventArgs e)
        {
            //MainControllingInterface childForm = new MainControllingInterface();

            ControllingInterface childForm = new ControllingInterface();
            childForm.MdiParent = this;
            childForm.WindowState = FormWindowState.Maximized;
            childForm.Show();

            DateTime buildDate = new FileInfo(Assembly.GetExecutingAssembly().Location).LastWriteTime;
            this.toolStripStatusUserDescription.Text = GlobalVariables.GlobalUserInformation.UserDescription;
            this.toolStripStatusLabel3.Text = "Version 1.0." + ", Date: " + buildDate.ToString("dd/MM/yyyy HH:mm:ss") + "         "; //GlobalVariables.ConfigVersionID(GlobalVariables.ConfigID).ToString() + 
             
        }

        private void frmMDIMain_MdiChildActivate(object sender, EventArgs e)
        {
            ToolStripManager.RevertMerge(this.toolStripMDIMain);
            IMergeToolStrip mergeToolStrip = ActiveMdiChild as IMergeToolStrip;
            if (mergeToolStrip != null)
            {
                ToolStripManager.Merge(mergeToolStrip.ChildToolStrip, toolStripMDIMain);
            }
        }

        #endregion Form Events

        #region     Toolbar Action
        private void ShowNewForm(object sender, EventArgs e)
        {
            //Form4 childForm = new Form4();
            //childForm.MdiParent = this;
            //childForm.Text = "Window " + childFormNumber++;
            //childForm.WindowState = FormWindowState.Maximized;
            //childForm.Show();


            MessageBox.Show("ABC");

            //EmployeeCategory childForm = new EmployeeCategory();
            //childForm.MdiParent = this;
            //childForm.Text = "Window " + childFormNumber++;
            //childForm.WindowState = FormWindowState.Maximized;
            //childForm.Show();


        }

        private void OpenFile(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            openFileDialog.Filter = "Text Files (*.txt)|*.txt|All Files (*.*)|*.*";
            if (openFileDialog.ShowDialog(this) == DialogResult.OK)
            {
                string FileName = openFileDialog.FileName;
            }
        }

        private void SaveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            saveFileDialog.Filter = "Text Files (*.txt)|*.txt|All Files (*.*)|*.*";
            if (saveFileDialog.ShowDialog(this) == DialogResult.OK)
            {
                string FileName = saveFileDialog.FileName;
            }
        }

        private void ExitToolsStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void CutToolStripMenuItem_Click(object sender, EventArgs e)
        {
        }

        private void CopyToolStripMenuItem_Click(object sender, EventArgs e)
        {
        }

        private void PasteToolStripMenuItem_Click(object sender, EventArgs e)
        {
        }

        private void ToolBarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            toolStripMDIMain.Visible = toolBarToolStripMenuItem.Checked;
        }

        private void StatusBarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            statusStrip.Visible = statusBarToolStripMenuItem.Checked;
        }

        private void CascadeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LayoutMdi(MdiLayout.Cascade);
        }

        private void TileVerticalToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LayoutMdi(MdiLayout.TileVertical);
        }

        private void TileHorizontalToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LayoutMdi(MdiLayout.TileHorizontal);
        }

        private void ArrangeIconsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LayoutMdi(MdiLayout.ArrangeIcons);
        }

        private void CloseAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            foreach (Form childForm in MdiChildren)
            {
                childForm.Close();
            }
        }

        private void saveToolStripButton_Click(object sender, EventArgs e)
        {
            if (this.Save != null) this.Save(sender, e);
        }

        #endregion     Toolbar Action



    }
}
