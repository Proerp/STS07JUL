using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.Data;
using System.Data.SqlClient;
using Microsoft.SqlServer.MessageBox;
using Global.Class.Library;

using DataAccessLayer;

using System.Text;



namespace PresentationLayer
{
    static class Startup
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {

            bool isLoginOK = false;



            ////KHONG LOAD DATABASE
            //GlobalVariables.LocationID = 1;
            //isLoginOK = true;



            try
            {

                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);

                GlobalRegistry.ShowError = true;

                //GlobalMsADO.ServerName = "SERVER\\SQLEXPRESS";

                GlobalMsADO.ServerName = GlobalRegistry.Read("ServerName");
                GlobalMsADO.DatabaseName = GlobalRegistry.Read("DatabaseName");

                //GlobalMsADO.ServerName = "WH\\SQLEXPRESS";
                //GlobalMsADO.DatabaseName = "E:\\20.02.2013\\Database\\BPFillingSystem.mdf";


                //GlobalMsADO.ServerName = "HOME-PC\\SQLEXPRESS";
                //GlobalMsADO.DatabaseName = "BPFillingSystem";

                //GlobalMsADO.ServerName = "SONY-VIO\\SQLEXPRESS";
                //GlobalMsADO.DatabaseName = "ImageS8SongThan";

                GlobalMsADO.ServerName = "192.168.1.18";

                while (!isLoginOK && GlobalVariables.LocationID >= -1)//Try to open startup database, Exit if user cancel to input ServerName and DatabaseName
                {


                    //MessageBox.Show (GlobalStaticFunction.TextToASCII("AB"));
                    //MessageBox.Show(GlobalStaticFunction.TextToHEX("AB"));



                    ////byte[] a = GlobalStaticFunction.XORBytes(GlobalStaticFunction.StringToByteArrayFastest("01"), GlobalStaticFunction.StringToByteArrayFastest("02"));

                    //////////string hex = BitConverter.ToString(data);

                    //////////string hex = BitConverter.ToString(data).Replace("-", string.Empty); ;

                    ////string hex = BitConverter.ToString(a);

                    ////MessageBox.Show (hex);


                    //////////1111111111---------MessageBox.Show(GlobalStaticFunction.TextToHEX("BOURG LES VALENCE" ));

                    ////////////byte[] a = GlobalStaticFunction.CheckSumHEXString("0A,00,13,01,0A,02,38,49,4D,41,4A,45,20,02,54,46,52,41,4E,43,45,0D");

                    ////////////byte[] a = GlobalStaticFunction.CheckSumHEXString("0A,00,2A,01,0A,02,38,49,4D,41,4A,45,20,01,53,42,4F,55,52,47,20,4C,45,53,20,56,41,4C,45,4E,43,45,0A,02,54,46,52,41,4E,43,45,1E,1E,1E,0D");
                    //////////1111111111---------byte[] a = GlobalStaticFunction.CheckSumHEXString("0A,00,2A,01,0A,02,38,49,4D,41,4A,45,20,1C,01,53,42,4F,55,52,47,20,4C,45,53,20,56,41,4C,45,4E,43,45,0A,02,54,46,52,41,4E,43,45,1E,1E,1E,0D");

                    //////////1111111111---------MessageBox.Show(BitConverter.ToString(a));


                    if (GlobalMsADO.ServerName == null || GlobalMsADO.DatabaseName == null || GlobalMsADO.ServerName == "" || GlobalMsADO.DatabaseName == "")//Show Get Server Name Dialog
                    {
                        PublicGetServerName publicGetServerName = new PublicGetServerName();
                        if (publicGetServerName.ShowDialog() != DialogResult.OK) { GlobalVariables.LocationID = -2; }
                        if (publicGetServerName.DialogResult == DialogResult.OK)

                            publicGetServerName.Dispose();
                    }

                    if (GlobalVariables.LocationID >= -1)//Do not execute if user cancel to input ServerName and DatabaseName
                    {
                        try
                        {
                            if (GlobalMsADO.MainDataAccessConnection(true).State == ConnectionState.Open)//Try to open new connection
                            {
                                isLoginOK = true;
                                GlobalVariables.LocationID = 1;
                            }
                            else
                            {
                                GlobalMsADO.ServerName = "";
                                GlobalMsADO.DatabaseName = "";
                            }
                        }
                        catch
                        {
                            GlobalMsADO.ServerName = "";
                            GlobalMsADO.DatabaseName = "";
                        }

                    }
                }


                if (GlobalVariables.LocationID > 0)
                {

                    GlobalVariables.IgnoreEmptyCarton = GlobalRegistry.Read("IgnoreEmptyCarton") == "0" ? false : true;


                    PublicApplicationLogon publicApplicationLogon = new PublicApplicationLogon();

                    if (publicApplicationLogon.ShowDialog() == DialogResult.OK) Application.Run(new frmMDIMain());

                    publicApplicationLogon.Dispose();

                }
            }

            catch (Exception ex)
            {
                GlobalExceptionHandler.ShowExceptionMessageBox(new Form(), ex);
            }
            finally
            {
                Console.WriteLine("Executing finally block.");
            }

        }


    }
}
