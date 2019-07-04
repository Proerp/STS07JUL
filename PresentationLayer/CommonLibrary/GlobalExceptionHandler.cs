using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using DataTransferObject;
using Microsoft.SqlServer.MessageBox;

namespace PresentationLayer
{
    class GlobalExceptionHandler
    {

        public static ExceptionMessageBoxDialogResult ShowExceptionMessageBox(IWin32Window owner, string exceptionMessage)
        {
            return GlobalExceptionHandler.ShowExceptionMessageBox(owner, new Exception(exceptionMessage));
        }

        public static ExceptionMessageBoxDialogResult ShowExceptionMessageBox(IWin32Window owner, Exception ex)
        {
            //try
            //{

            ExceptionMessageBoxDialogResult exceptionMessageBoxDialogResult;

            CustomException customEx = ex as CustomException;
            if (customEx != null)
            {
                CustomExceptionMessageBox customExceptionMessageBox = new CustomExceptionMessageBox(customEx);
                exceptionMessageBoxDialogResult = (ExceptionMessageBoxDialogResult)customExceptionMessageBox.ShowDialog(owner);
            }
            else
            {
                System.Data.Common.DbException dbException = ex as System.Data.Common.DbException;
                if (dbException != null && dbException.ErrorCode == -2146232060)
                    ex = new Exception("Không thể lưu do trùng dữ liệu. Vui lòng kiểm tra lại.");


                ExceptionMessageBox exceptionMessageBox = new ExceptionMessageBox(ex); //exceptionMessageBox.ShowToolBar = false;
                exceptionMessageBoxDialogResult = (ExceptionMessageBoxDialogResult)exceptionMessageBox.Show(owner);
            }
            return exceptionMessageBoxDialogResult;
            //}
            //catch {}// Environment.Exit(0); return ExceptionMessageBoxDialogResult.None; }
        }

    }



}
