using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;


using Guifreaks.Navisuite;

using Global.Class.Library;
using DataAccessLayer;
using DataTransferObject;

namespace PresentationLayer
{
    public partial class CommonMDI : Form
    {

        [DllImport("uxtheme.dll", CharSet = CharSet.Unicode)]
        public static extern int SetWindowTheme(IntPtr hWnd, String pszSubAppName, String pszSubIdList);


        #region Contractor

        private GlobalEnum.TaskID taskID;

        public CommonMDI(GlobalEnum.TaskID taskID)
        {
            InitializeComponent();

            this.taskID = taskID;

            if (this.taskID == GlobalEnum.TaskID.DataMessage)
                this.Size = new Size(1250, 810);
        }

        private void CommonMDI_Load(object sender, EventArgs e)
        {
            try
            {
                Form childForm;

                switch (this.taskID)
                {
                    case GlobalEnum.TaskID.DataMessage:
                        childForm = new DataMessage();
                        break;
                    case GlobalEnum.TaskID.ListLogo:
                        childForm = new ListLogo();
                        break;
                    case GlobalEnum.TaskID.ListFactory:
                        childForm = new ListFactory();
                        break;
                    case GlobalEnum.TaskID.ListOwner:
                        childForm = new ListOwner();
                        break;
                    case GlobalEnum.TaskID.ListCategory:
                        childForm = new ListCategory();
                        break;
                    case GlobalEnum.TaskID.ListProduct:
                        childForm = new ListProduct();
                        break;
                    case GlobalEnum.TaskID.ListCoil:
                        childForm = new ListCoil();
                        break;
                    case GlobalEnum.TaskID.ListEmployee:
                        childForm = new ListEmployee();
                        break;
                    default:
                        childForm = new DataMessage();
                        break;
                }

                if (childForm != null)
                {
                    childForm.MdiParent = this;
                    childForm.WindowState = FormWindowState.Maximized;
                    childForm.Show();
                }
            }
            catch (Exception exception)
            {
                GlobalExceptionHandler.ShowExceptionMessageBox(this, exception);
            }
        }

        #endregion Contractor



        #region Form Events: Merge toolstrip & Set toolbar context

        private void frmMDIMain_MdiChildActivate(object sender, EventArgs e)
        {
            try
            {
                ToolStripManager.RevertMerge(this.toolStripMDIMain);
                IMergeToolStrip mdiChildMergeToolStrip = ActiveMdiChild as IMergeToolStrip;
                if (mdiChildMergeToolStrip != null)
                {
                    ToolStripManager.Merge(mdiChildMergeToolStrip.ChildToolStrip, toolStripMDIMain);
                }

                ICallToolStrip mdiChildCallToolStrip = ActiveMdiChild as ICallToolStrip;
                if (mdiChildCallToolStrip != null)
                {
                    mdiChildCallToolStrip.PropertyChanged -= new PropertyChangedEventHandler(mdiChildCallToolStrip_PropertyChanged);
                    mdiChildCallToolStrip.PropertyChanged += new PropertyChangedEventHandler(mdiChildCallToolStrip_PropertyChanged);

                    mdiChildCallToolStrip_PropertyChanged(mdiChildCallToolStrip, new PropertyChangedEventArgs("IsDirty"));
                }
            }
            catch (Exception exception)
            {
                GlobalExceptionHandler.ShowExceptionMessageBox(this, exception);
            }
        }

        void mdiChildCallToolStrip_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            try
            {
                ICallToolStrip mdiChildCallToolStrip = sender as ICallToolStrip;
                if (mdiChildCallToolStrip != null)
                {

                    bool closable = mdiChildCallToolStrip.Closable;
                    bool loadable = mdiChildCallToolStrip.Loadable;
                    bool newable = mdiChildCallToolStrip.Newable;
                    bool editable = mdiChildCallToolStrip.Editable;
                    bool isDirty = mdiChildCallToolStrip.IsDirty;
                    bool deletable = mdiChildCallToolStrip.Deletable;
                    bool importable = mdiChildCallToolStrip.Importable;
                    bool exportable = mdiChildCallToolStrip.Exportable;
                    bool verifiable = mdiChildCallToolStrip.Verifiable;
                    bool unverifiable = mdiChildCallToolStrip.Unverifiable;
                    bool printable = mdiChildCallToolStrip.Printable;
                    bool searchable = mdiChildCallToolStrip.Searchable;

                    bool readonlyMode = mdiChildCallToolStrip.ReadonlyMode;
                    bool editableMode = mdiChildCallToolStrip.EditableMode;
                    bool isValid = mdiChildCallToolStrip.IsValid;


                    this.toolStripButtonEscape.Enabled = closable;
                    this.toolStripButtonLoad.Enabled = loadable && readonlyMode;

                    this.toolStripButtonNew.Enabled = newable && readonlyMode;
                    this.toolStripButtonEdit.Enabled = editable && readonlyMode;
                    this.toolStripButtonSave.Enabled = isDirty && isValid && editableMode;
                    this.toolStripButtonDelete.Enabled = deletable && readonlyMode;

                    this.toolStripButtonImport.Visible = importable;
                    this.toolStripButtonImport.Enabled = importable && newable && readonlyMode;
                    this.toolStripButtonExport.Visible = exportable;
                    this.toolStripButtonExport.Enabled = exportable;//&& !isDirty && readonlyMode;
                    this.toolStripSeparatorImport.Visible = importable || exportable;

                    this.toolStripButtonVerify.Visible = verifiable || unverifiable;
                    this.toolStripButtonVerify.Enabled = verifiable || unverifiable;
                    this.toolStripButtonVerify.Text = verifiable ? "Verify" : "Unverify";
                    this.toolStripSeparatorVerify.Visible = verifiable || unverifiable;

                    this.toolStripButtonPrint.Visible = printable;
                    this.toolStripButtonPrint.Enabled = printable;
                    this.toolStripButtonPrintPreview.Visible = printable;
                    this.toolStripButtonPrintPreview.Enabled = printable;
                    this.toolStripSeparatorPrint.Visible = printable;

                    this.toolStripComboBoxSearchText.Visible = searchable;
                    this.toolStripButtonSearch.Visible = searchable;
                }
            }
            catch (Exception exception)
            {
                GlobalExceptionHandler.ShowExceptionMessageBox(this, exception);
            }
        }

        #endregion Form Events

        #region Load and Open Module, Task



        #endregion Load and Open Module, Task

        #region    Automaticaly Toolbar Action


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


        #endregion     Toolbar Action


        #region <Call Tool Strip>
        private void toolStripButtonEscape_Click(object sender, EventArgs e)
        {
            try
            {
                ICallToolStrip callToolStrip = ActiveMdiChild as ICallToolStrip;
                if (callToolStrip != null) callToolStrip.Escape();

                if (this.MdiChildren.Length == 0) { this.DialogResult = System.Windows.Forms.DialogResult.OK; }
            }
            catch (Exception exception)
            {
                GlobalExceptionHandler.ShowExceptionMessageBox(this, exception);
            }
        }

        private void toolStripButtonLoad_Click(object sender, EventArgs e)
        {
            try
            {
                ICallToolStrip callToolStrip = ActiveMdiChild as ICallToolStrip;
                if (callToolStrip != null) callToolStrip.Loading();
            }
            catch (Exception exception)
            {
                GlobalExceptionHandler.ShowExceptionMessageBox(this, exception);
            }
        }

        private void toolStripButtonSearch_Click(object sender, EventArgs e)
        {
            try
            {
                ICallToolStrip callToolStrip = ActiveMdiChild as ICallToolStrip;
                if (callToolStrip != null) callToolStrip.SearchText(this.toolStripComboBoxSearchText.Text);
            }
            catch (Exception exception)
            {
                GlobalExceptionHandler.ShowExceptionMessageBox(this, exception);
            }
        }


        private void toolStripButtonNew_Click(object sender, EventArgs e)
        {
            try
            {
                ICallToolStrip callToolStrip = ActiveMdiChild as ICallToolStrip;
                if (callToolStrip != null) callToolStrip.New();
            }
            catch (Exception exception)
            {
                GlobalExceptionHandler.ShowExceptionMessageBox(this, exception);
            }
        }


        private void toolStripButtonEdit_Click(object sender, EventArgs e)
        {
            try
            {
                ICallToolStrip callToolStrip = ActiveMdiChild as ICallToolStrip;
                if (callToolStrip != null) callToolStrip.Edit();
            }
            catch (Exception exception)
            {
                GlobalExceptionHandler.ShowExceptionMessageBox(this, exception);
            }
        }

        private void toolStripButtonSave_Click(object sender, EventArgs e)
        {
            try
            {
                ICallToolStrip callToolStrip = ActiveMdiChild as ICallToolStrip;
                if (callToolStrip != null) callToolStrip.Save();
            }
            catch (Exception exception)
            {
                GlobalExceptionHandler.ShowExceptionMessageBox(this, exception);
            }
        }

        private void toolStripButtonDelete_Click(object sender, EventArgs e)
        {
            try
            {
                ICallToolStrip callToolStrip = ActiveMdiChild as ICallToolStrip;
                if (callToolStrip != null) callToolStrip.Delete();
            }
            catch (Exception exception)
            {
                GlobalExceptionHandler.ShowExceptionMessageBox(this, exception);
            }
        }


        private void toolStripButtonImport_Click(object sender, EventArgs e)
        {
            try
            {
                ICallToolStrip callToolStrip = ActiveMdiChild as ICallToolStrip;
                if (callToolStrip != null) callToolStrip.Import();
            }
            catch (Exception exception)
            {
                GlobalExceptionHandler.ShowExceptionMessageBox(this, exception);
            }
        }

        private void toolStripButtonExport_Click(object sender, EventArgs e)
        {
            try
            {
                ICallToolStrip callToolStrip = ActiveMdiChild as ICallToolStrip;
                if (callToolStrip != null) callToolStrip.Export();
            }
            catch (Exception exception)
            {
                GlobalExceptionHandler.ShowExceptionMessageBox(this, exception);
            }
        }

        private void toolStripButtonVerify_Click(object sender, EventArgs e)
        {
            try
            {
                ICallToolStrip callToolStrip = ActiveMdiChild as ICallToolStrip;
                if (callToolStrip != null) callToolStrip.Verify();
            }
            catch (Exception exception)
            {
                GlobalExceptionHandler.ShowExceptionMessageBox(this, exception);
            }
        }


        private void toolStripButtonPrint_Click(object sender, EventArgs e)
        {
            try
            {
                ICallToolStrip callToolStrip = ActiveMdiChild as ICallToolStrip;
                if (callToolStrip != null) callToolStrip.Print(PrintDestination.Print);
            }
            catch (Exception exception)
            {
                GlobalExceptionHandler.ShowExceptionMessageBox(this, exception);
            }
        }

        private void toolStripButtonPrintPreview_Click(object sender, EventArgs e)
        {
            try
            {
                ICallToolStrip callToolStrip = ActiveMdiChild as ICallToolStrip;
                if (callToolStrip != null) callToolStrip.Print(PrintDestination.PrintPreview);
            }
            catch (Exception exception)
            {
                GlobalExceptionHandler.ShowExceptionMessageBox(this, exception);
            }
        }

        #endregion <Call Tool Strip>

        private void CommonMDI_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                toolStripButtonEscape_Click(this, new EventArgs());

                if (this.MdiChildren.Length > 0) { e.Cancel = true; return; }
            }
            catch (Exception exception)
            {
                GlobalExceptionHandler.ShowExceptionMessageBox(this, exception);
            }

        }






















    }
}
