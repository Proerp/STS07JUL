using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;

using Global.Class.Library;

namespace DataAccessLayer
{
    public static class GlobalUserPermission
    {
        public enum ACLLevel : int
        {
            ACLNoAccess = 0,
            ACLReadable = 1,
            ACLEditable = 2
        }

        static GlobalUserPermission()
        {
            try
            {
                if (GlobalVariables.shouldRestoreProcedure) RestoreProcedure();
            }
            catch (Exception exception)
            {
                throw exception;
            }
        }

        public static DateTime GlobalLockedDate()
        {
            return DateTime.MinValue;
        }








        /// <summary>
        /// Get User Information of UserID at ChangeDate
        /// </summary>
        /// <param name="userID"></param>
        /// <param name="changeDate"></param>
        /// <returns></returns>
        public static UserInformation GetUserInformation(int userID, DateTime changeDate)
        {
            UserInformation userInformation = new UserInformation();

            SqlParameter[] sqlParameter = new SqlParameter[2];
            sqlParameter[0] = new SqlParameter("UserID", userID); sqlParameter[0].SqlDbType = SqlDbType.Int; sqlParameter[0].Direction = ParameterDirection.Input;
            sqlParameter[1] = new SqlParameter("ChangeDate", changeDate); sqlParameter[1].SqlDbType = SqlDbType.DateTime; sqlParameter[1].Direction = ParameterDirection.Input;

            DataTable userInformationTable = SQLDatabase.GetDataTable("SPGetUserInformation", CommandType.StoredProcedure, sqlParameter);

            if (userInformationTable.Rows.Count > 0)
            {
                userInformation.UserID = (int)userInformationTable.Rows[0]["UserID"];
                userInformation.UserOrganizationID = (int)userInformationTable.Rows[0]["UserOrganizationID"];
            }

            return userInformation;
        }







        /// <summary>
        /// Get the top level permission for a specific UserID on a specific TaskID
        /// </summary>
        /// <param name="userID"></param>
        /// <param name="taskID"></param>
        /// <returns></returns>
        public static bool GetUserReadOnly(int userID, GlobalEnum.TaskID taskID)
        {
            
            return false;//For SONG THAN: All USERS ARE EDITABLE

            //Need to check the lasted version
            //Set lRec = ADORECNEW(): Call lRec.Open("SELECT MAX(VersionID) AS VersionID FROM VersionMaster ", ClassConn)
            //If Not lRec.BOF And Not lRec.EOF Then
            //    If lNMVNVersionID <> lRec.Fields("VersionID") Then
            //        MsgBox "The program ERmgr on this computer is not the lasted version." & Chr(13) & "Please contact your admin for more information. Thank you!", vbExclamation + vbDefaultButton1, "ERmgr"
            //        End
            //    End If
            //End If

            SqlParameter[] sqlParameter = new SqlParameter[2];
            sqlParameter[0] = new SqlParameter("UserID", userID); sqlParameter[0].SqlDbType = SqlDbType.Int; sqlParameter[0].Direction = ParameterDirection.Input;
            sqlParameter[1] = new SqlParameter("TaskID", (int)taskID); sqlParameter[1].SqlDbType = SqlDbType.Int; sqlParameter[1].Direction = ParameterDirection.Input;

            int maxACLEditable = SQLDatabase.GetScalarValue("SPMaxACLEditable", CommandType.StoredProcedure, sqlParameter);
            return (ACLLevel)maxACLEditable != ACLLevel.ACLEditable;
        }







        /// <summary>
        /// Get the permission for a specific UserID on a specific TaskID ON A SPECIFIC UserOrganizationID
        /// </summary>
        /// <param name="userID"></param>
        /// <param name="taskID"></param>
        /// <param name="userOrganizationID"></param>
        /// <returns></returns>
        public static bool GetUserEditable(int userID, GlobalEnum.TaskID taskID, int userOrganizationID)
        {
            //Need to check the lasted version
            //Set lRec = ADORECNEW(): Call lRec.Open("SELECT MAX(VersionID) AS VersionID FROM VersionMaster ", ClassConn)
            //If Not lRec.BOF And Not lRec.EOF Then
            //    If lNMVNVersionID <> lRec.Fields("VersionID") Then
            //        MsgBox "The program ERmgr on this computer is not the lasted version." & Chr(13) & "Please contact your admin for more information. Thank you!", vbExclamation + vbDefaultButton1, "ERmgr"
            //        End
            //    End If
            //End If

            SqlParameter[] sqlParameter = new SqlParameter[3];
            sqlParameter[0] = new SqlParameter("UserID", userID); sqlParameter[0].SqlDbType = SqlDbType.Int; sqlParameter[0].Direction = ParameterDirection.Input;
            sqlParameter[1] = new SqlParameter("TaskID", (int)taskID); sqlParameter[1].SqlDbType = SqlDbType.Int; sqlParameter[1].Direction = ParameterDirection.Input;
            sqlParameter[2] = new SqlParameter("UserOrganizationID", userOrganizationID); sqlParameter[0].SqlDbType = SqlDbType.Int; sqlParameter[0].Direction = ParameterDirection.Input;

            int aclEditable = SQLDatabase.GetScalarValue("SPUserACLEditable", CommandType.StoredProcedure, sqlParameter);
            return (ACLLevel)aclEditable == ACLLevel.ACLEditable; 
        }









        /// <summary>
        /// Get Editable of a Data Entry (Data Transaction Object)
        /// </summary>
        /// <param name="storedProcedureName"></param>
        /// <param name="findIdentityID"></param>
        /// <returns></returns>
        public static bool GetEditable(string storedProcedureName, int findIdentityID)
        {
            SqlParameter[] sqlParameter = new SqlParameter[1];
            sqlParameter[0] = new SqlParameter("FindIdentityID", findIdentityID); sqlParameter[0].SqlDbType = SqlDbType.Int; sqlParameter[0].Direction = ParameterDirection.Input;

            int existIdentityID = SQLDatabase.GetReturnValue(storedProcedureName, CommandType.StoredProcedure, sqlParameter);
            return existIdentityID == -1;
        }











        private static void RestoreProcedure()
        {
            string queryString = "";



            /// <summary>
            /// Get UserID Information of UserID at ChangeDate
            /// </summary>    
            queryString = "  @UserID Int, @ChangeDate DateTime " + "\r\n";
            queryString = queryString + "  WITH ENCRYPTION " + "\r\n";
            queryString = queryString + "  AS " + "\r\n";
            queryString = queryString + "    BEGIN " + "\r\n";

            queryString = queryString + "       IF @UserID = 0 " + "\r\n";
            queryString = queryString + "           SELECT  ListEmployee.EmployeeID AS UserID, 1 AS UserOrganizationID " + "\r\n";
            queryString = queryString + "           FROM ListEmployee " + "\r\n";
            queryString = queryString + "       ELSE " + "\r\n";
            queryString = queryString + "           SELECT  TOP 1 ListEmployee.EmployeeID AS UserID, 1 AS UserOrganizationID " + "\r\n";
            queryString = queryString + "           FROM ListEmployee " + "\r\n";
            queryString = queryString + "           WHERE   ListEmployee.EmployeeID = @UserID " + "\r\n";

            queryString = queryString + "    END " + "\r\n";

            SQLDatabase.CreateStoredProcedure("SPGetUserInformation", queryString);






            /// <summary>
            /// Get the top level permission for a specific UserID on a specific TaskID
            /// </summary>    
            queryString = " @UserID Int, @TaskID Int " + "\r\n";
            queryString = queryString + " WITH ENCRYPTION " + "\r\n";
            queryString = queryString + " AS " + "\r\n";
            queryString = queryString + "   SELECT      ISNULL(MAX(ACLEditable), 0) AS MaxACLEditable FROM PublicAccessControl " + "\r\n";
            queryString = queryString + "   WHERE       UserID = @UserID AND TaskID = @TaskID " + "\r\n";

            SQLDatabase.CreateStoredProcedure("SPMaxACLEditable", queryString);







            /// <summary>
            /// Get the permission for a specific UserID on a specific TaskID ON A SPECIFIC UserOrganizationID
            /// </summary>    

            //VIEC GET EDITABLE: CO 2 TRUONG HOP
            //    '//A. Maintenance List (DANH MUC THAM KHAO): UserOrganizationID = 0 => CO QUYEN HAY KHONG MA THOI, BOI VI MAU TIN TRONG DANH SACH THAM KHAO LA CUA CHUNG => DO DO KHONG CAN DEN UserOrganizationID
            //    '//B. Transaction Data (CAC MAU TIN GIAO DICH - CAC MAU TIN CO CHU SO HUU), KHI DO CO 2 TINH HUONG:
            //        '//B.1 TINH HUONG 1: MAU TIN DA SAVE: (TUC LA DA CO CHU SO HUU - CO UserOrganizationID): NGUOI DANG TRUY CAP CO QUYEN EDIT TREN MAU TIN CO CHU SO HUU HAY KHONG?
            //        '//B2. TINH HUONG 2:
            //                '//B.2: NGUOI DUNG CO QUYEN EDITABLE TREN TASKID, NHUNG KHONG BIET CO QUYEN TREN MOT DON VI CU THE UserOrganizationID HAY KHONG?
            //                '//B.2: THEM VAO DO, TAI THOI DIEM EDIT, CHUA XAC DINH MAU TIN THUOC VE AI, KHI DO UserOrganizationID = 0,
            //                '//B.2: DO DO CUNG CHI XAC DINH DUA TREN QUYEN EDITABLE CUA TASKID CUA UserID HIEN HANH MA THOI
            //                '//B.2: KHI SAVE (HOAC HAY HON LA KHI CHON CHU SO HUU - VI DU: CHON UserID TRONG QUOTATION) TA MOI XAC DINH DUOC UserOrganizationID
            //                '//B.2: DEN LUC NAY VIEC XAC DINH QUYEN EDITABLE LA CU THE ROI, DO DO NEU KHONG CO QUYEN EDITABLE CHO MOT DON VI CU THE => THI SE KHONG CHO SAVE

            queryString = " @UserID Int, @TaskID Int, @UserOrganizationID Int " + "\r\n";
            queryString = queryString + " WITH ENCRYPTION " + "\r\n";
            queryString = queryString + " AS " + "\r\n";

            queryString = queryString + "   IF @UserOrganizationID > 0  " + "\r\n"; // CHI CO T/H B.1

            queryString = queryString + "       SELECT      ISNULL(MAX(ACLEditable), 0) AS MaxACLEditable FROM PublicAccessControl " + "\r\n";
            queryString = queryString + "       WHERE       UserID = @UserID AND TaskID = @TaskID AND UserOrganizationID = @UserOrganizationID " + "\r\n";

            queryString = queryString + "   ELSE " + "\r\n"; //AP DUNG CHO TRUONG T/H A + T/H B.2

            queryString = queryString + "       SELECT      ISNULL(MAX(ACLEditable), 0) AS MaxACLEditable FROM PublicAccessControl " + "\r\n";
            queryString = queryString + "       WHERE       UserID = @UserID AND TaskID = @TaskID " + "\r\n";

            SQLDatabase.CreateStoredProcedure("SPUserACLEditable", queryString);
        }

    }


}
