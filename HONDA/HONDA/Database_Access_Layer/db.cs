using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Net.Mail;
using System.Net;
using System.Web.Script.Serialization;
using HONDA.Models;
namespace HONDA.Database_Access_Layer
{

    public class db
    {


        private SqlConnection con;
        DataTable dt = new DataTable();
        DataSet ds = new DataSet();

        public string Uname { get; set; }
        public string Password { get; set; }

        private void connection()
        {
            string constr = ConfigurationManager.ConnectionStrings["conn"].ToString();
            con = new SqlConnection(constr);
        }

        public void Set_Credential()
        {
            Uname = ConfigurationManager.AppSettings["Service_UID"].ToString();
            Password = ConfigurationManager.AppSettings["Service_PWD"].ToString();
        }

        public string DataTableToJSONWithJSONNet(DataTable table)
        {
            JavaScriptSerializer jsSerializer = new JavaScriptSerializer();
            List<Dictionary<string, object>> parentRow = new List<Dictionary<string, object>>();
            Dictionary<string, object> childRow;
            foreach (DataRow row in table.Rows)
            {
                childRow = new Dictionary<string, object>();
                foreach (DataColumn col in table.Columns)
                {
                    childRow.Add(col.ColumnName, row[col]);
                }
                parentRow.Add(childRow);
            }
            return jsSerializer.Serialize(parentRow);
        }
        public Page_Permission_Detail Get_Page_UserPermission(int emp_id, int page_id)
        {
            Page_Permission_Detail info = new Page_Permission_Detail();
            info.View = 0;
            info.Update = 0;
            info.Create = 0;
            info.Import = 0;
            info.Export = 0;
            connection();
            SqlCommand cmd = new SqlCommand("PRC_GetUserPermission", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@Emp_ID", emp_id);
            cmd.Parameters.AddWithValue("@PageID", page_id);
            con.Open();
            SqlDataReader dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                info.emp_id = Convert.ToInt32(dr["EMP_ID"].ToString());
                info.page_id = Convert.ToInt32(dr["PAGE_ID"].ToString());
                if (dr["PERMISSION_NAME"].ToString().Trim() == "View")
                {
                    info.View = 1;
                }
                if (dr["PERMISSION_NAME"].ToString().Trim() == "Update")
                {
                    info.Update = 1;
                }
                if (dr["PERMISSION_NAME"].ToString().Trim() == "Create")
                {
                    info.Create = 1;
                }
                if (dr["PERMISSION_NAME"].ToString().Trim() == "Import")
                {
                    info.Import = 1;
                }
                if (dr["PERMISSION_NAME"].ToString().Trim() == "Export")
                {
                    info.Export = 1;
                }

            }
            con.Close();
            return info;
        }
        public string checklogin(string user_name, string password)
        {
            string res = "";
            connection();
            SqlCommand cmd = new SqlCommand("Prc_validateUser", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@user_name", user_name);
            cmd.Parameters.AddWithValue("@password", password);
            SqlParameter oblogin = new SqlParameter();
            oblogin.ParameterName = "@message";
            oblogin.SqlDbType = SqlDbType.NVarChar;
            oblogin.Size = 150;
            oblogin.Direction = ParameterDirection.Output;
            cmd.Parameters.Add(oblogin);
            con.Open();
            cmd.ExecuteNonQuery();
            res = Convert.ToString(oblogin.Value);

            con.Close();
            return res;
        }
        public string forgotpassword(string email_id)
        {
            string res = "";
            connection();
            SqlCommand cmd = new SqlCommand("Prc_ForgotPassword", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@email", email_id);
            SqlParameter oblogin = new SqlParameter();
            oblogin.ParameterName = "@message";
            oblogin.SqlDbType = SqlDbType.NVarChar;
            oblogin.Size = 500;
            oblogin.Direction = ParameterDirection.Output;
            cmd.Parameters.Add(oblogin);
            con.Open();
            cmd.ExecuteNonQuery();
            res = Convert.ToString(oblogin.Value);

            con.Close();
            return res;
        }
        public string changepassword(string old_password, string new_password, int emp_id)
        {
            string res = "";
            connection();
            SqlCommand cmd = new SqlCommand("Prc_ChangePassword", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@Oldpassword", old_password);
            cmd.Parameters.AddWithValue("@Newpassword", new_password);
            cmd.Parameters.AddWithValue("@emp_id", emp_id);
            SqlParameter oblogin = new SqlParameter();
            oblogin.ParameterName = "@message";
            oblogin.SqlDbType = SqlDbType.NVarChar;
            oblogin.Size = 500;
            oblogin.Direction = ParameterDirection.Output;
            cmd.Parameters.Add(oblogin);
            con.Open();
            cmd.ExecuteNonQuery();
            res = Convert.ToString(oblogin.Value);

            con.Close();
            return res;
        }


        public DataTable getroles()
        {
            DataTable dt = new DataTable();
            string res = "";
            try
            {

                connection();
                //string query = "SELECT ROLE_ID,ROLE_NAME FROM MST_ROLE WHERE STATUS='Active' ORDER BY ROLE_ID ASC";
                string query = "SELECT ROLE_ID,ROLE_NAME FROM MST_ROLE   ORDER BY ROLE_ID ASC";
                con.Open();
                using (SqlDataAdapter da = new SqlDataAdapter(query, con))
                {
                    da.Fill(dt);
                    con.Close();
                }
            }
            catch (Exception ex)
            {
                res = ex.Message;
            }
            return dt;
        }

        public string addroles(string roleName, DateTime currentDate, string createdBy, int status)
        {
            string res = "";
            try
            {
                var role_status = status == 1 ? "Active" : "In-Active";
                connection();
                using (SqlCommand cmd = new SqlCommand("Prc_AddRoles", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@ROLE_NAME", SqlDbType.VarChar, 50).Value = roleName;
                    cmd.Parameters.Add("@STATUS", SqlDbType.VarChar, 50).Value = role_status;
                    cmd.Parameters.Add("@EFFECTIVE_START_DATE", SqlDbType.DateTime).Value = currentDate;
                    cmd.Parameters.Add("@CREATION_DATE", SqlDbType.DateTime).Value = currentDate;
                    cmd.Parameters.Add("@CREATED_BY", SqlDbType.VarChar, 50).Value = createdBy;

                    SqlParameter outData = new SqlParameter();
                    outData.ParameterName = "@message";
                    outData.SqlDbType = SqlDbType.NVarChar;
                    outData.Size = 100;
                    outData.Direction = ParameterDirection.Output;
                    cmd.Parameters.Add(outData);
                    con.Open();
                    cmd.ExecuteNonQuery();
                    res = Convert.ToString(outData.Value);
                    con.Close();
                }
            }
            catch (Exception ex)
            {
                res = ex.Message;
            }
            return res;
        }
        public string Updateroles(int role_id, int status, string role_name)
        {
            string res = "";
            try
            {
                var role_status = status == 1 ? "Active" : "In-Active";
                connection();
                using (SqlCommand cmd = new SqlCommand("Prc_UpdateRoles", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@ROLE_ID", SqlDbType.Int).Value = role_id;
                    cmd.Parameters.Add("@STATUS", SqlDbType.VarChar, 50).Value = role_status;
                    cmd.Parameters.Add("@ROLE_NAME", SqlDbType.VarChar, 100).Value = role_name;

                    SqlParameter outData = new SqlParameter();
                    outData.ParameterName = "@message";
                    outData.SqlDbType = SqlDbType.NVarChar;
                    outData.Size = 100;
                    outData.Direction = ParameterDirection.Output;
                    cmd.Parameters.Add(outData);
                    con.Open();
                    cmd.ExecuteNonQuery();
                    res = Convert.ToString(outData.Value);
                    con.Close();
                }
            }
            catch (Exception ex)
            {
                res = ex.Message;
            }
            return res;
        }

        public DataTable getRoleById(int role_id)
        {
            try
            {
                connection();
                SqlCommand cmd = new SqlCommand("Prc_GetRoleById", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@role_id", role_id);
                con.Open();
                using (SqlDataAdapter da = new SqlDataAdapter())
                {
                    da.SelectCommand = cmd;
                    da.Fill(dt);
                }
                con.Close();
            }
            catch (Exception ex)
            {

            }
            return dt;
        }
        public DataTable getmodules(int roleId)
        {
            DataTable dt = new DataTable();
            string res = "";
            try
            {
                connection();
                using (SqlCommand cmd = new SqlCommand("Prc_GetModule", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@ROLE_ID", SqlDbType.Int).Value = roleId;

                    con.Open();
                    using (SqlDataAdapter da = new SqlDataAdapter())
                    {
                        da.SelectCommand = cmd;
                        da.Fill(dt);
                    }
                    con.Close();
                }
            }
            catch (Exception ex)
            {
                res = ex.Message;
            }
            return dt;
        }

        //All the permissions of roles
        public DataTable getPagePermission(int roleId)
        {
            DataTable dt = new DataTable();
            string res = "";
            try
            {
                connection();
                using (SqlCommand cmd = new SqlCommand("Prc_GetPagePermissions", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@ROLE_ID", SqlDbType.Int).Value = roleId;

                    con.Open();
                    using (SqlDataAdapter da = new SqlDataAdapter())
                    {
                        da.SelectCommand = cmd;
                        da.Fill(dt);
                    }
                    con.Close();
                }
            }
            catch (Exception ex)
            {
                res = ex.Message;
            }
            return dt;
        }

        //All the permissions assigned to roles for user
        public DataTable getPagePermissionForUser(int roleId, int empId)
        {
            DataTable dt = new DataTable();
            string res = "";
            try
            {
                connection();
                using (SqlCommand cmd = new SqlCommand("Prc_GetPagePermissionsForUser", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@ROLE_ID", SqlDbType.Int).Value = roleId;
                    cmd.Parameters.Add("@EMPLOYEE_ID", SqlDbType.Int).Value = empId;

                    con.Open();
                    using (SqlDataAdapter da = new SqlDataAdapter())
                    {
                        da.SelectCommand = cmd;
                        da.Fill(dt);
                    }
                    con.Close();
                }
            }
            catch (Exception ex)
            {
                res = ex.Message;
            }
            return dt;
        }

        public string updateRolePermission(IEnumerable<RolePermissionsStatus> rollPermission)
        {
            string res = "";
            try
            {
                DataTable dtups = new DataTable();
                dtups.Columns.Add("ROLE_ID");
                dtups.Columns.Add("ROLE_PERMISSION_ID");
                dtups.Columns.Add("CHECKED_STATUS");

                foreach (RolePermissionsStatus ps in rollPermission)
                {
                    DataRow dr = dtups.NewRow();
                    dr["ROLE_ID"] = ps.ROLE_ID;
                    dr["ROLE_PERMISSION_ID"] = ps.ROLE_PERMISSION_ID;
                    dr["CHECKED_STATUS"] = ps.CHECKED_STATUS;
                    dtups.Rows.Add(dr);
                }

                connection();
                //string query = "UPDATE MST_ROLE_PERMISSION SET STATUS=CASE WHEN @STATUS='TRUE' THEN 'Active' ELSE 'In-Active' END WHERE ROLE_PERMISSION_ID=@ROLEPERMISSIONID";
                using (SqlCommand cmd = new SqlCommand("prc_UpdateRolePermission", con))
                {
                    //cmd.CommandType = CommandType.StoredProcedure;
                    //cmd.Parameters.Add("@ROLEPERMISSIONID", SqlDbType.Int).Value = rollPermissionId;
                    //cmd.Parameters.Add("@STATUS", SqlDbType.VarChar).Value = checkedStatus;

                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@ROLE_PERMISSION_STATUS", SqlDbType.Structured).Value = dtups;

                    con.Open();
                    cmd.ExecuteNonQuery();
                    con.Close();
                }
            }
            catch (Exception ex)
            {
                res = ex.Message;
            }
            return res;
        }
        public DataSet getMenuByPermission(int user_id)
        {
            DataSet ds = new DataSet();
            try
            {
                connection();
                using (SqlCommand cmd = new SqlCommand("Prc_GetMenuByEmpId", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@user_id", user_id);
                    con.Open();

                    using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                    {
                        da.Fill(ds);
                        con.Close();
                    }
                }
            }
            catch (Exception ex)
            {

            }
            return ds;
        }
        public string updateUserRolePermission(IEnumerable<UserPermissionsStatus> ups, string login_user) //int empId, int rollPermissionId, string checkedStatus)
        {
            string res = "";
            try
            {


                DataTable dtups = new DataTable();
                dtups.Columns.Add("EMPLOYEE_ID");
                dtups.Columns.Add("ROLE_PERMISSION_ID");
                dtups.Columns.Add("ROLE_ID");
                dtups.Columns.Add("CHECKED_STATUS");

                foreach (UserPermissionsStatus ps in ups)
                {
                    DataRow dr = dtups.NewRow();
                    dr["EMPLOYEE_ID"] = ps.EMPLOYEE_ID;
                    dr["ROLE_PERMISSION_ID"] = ps.ROLE_PERMISSION_ID;
                    dr["ROLE_ID"] = ps.ROLE_ID;
                    dr["CHECKED_STATUS"] = ps.CHECKED_STATUS;
                    dtups.Rows.Add(dr);
                }

                connection();
                using (SqlCommand cmd = new SqlCommand("prc_SaveUpdateUserPermission", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@USER_PERMISSION_STATUS", SqlDbType.Structured).Value = dtups;
                    cmd.Parameters.AddWithValue("@login_user", login_user);

                    con.Open();
                    cmd.ExecuteNonQuery();
                    con.Close();
                }
            }
            catch (Exception ex)
            {
                res = ex.Message;
            }
            return res;
        }

        public DataSet BindList()
        {
            DataSet ds = new DataSet();

            try
            {
                connection();
                SqlCommand cmd = new SqlCommand("Prc_BindList", con);
                cmd.CommandType = CommandType.StoredProcedure;
                con.Open();
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(ds);
                con.Close();
            }
            catch (Exception ex)
            {

            }
            finally
            {


            }
            return ds;
        }
        public string save_hierarchy(DataTable tabledata)
        {
            string res = "";
            try
            {
                connection();
                SqlCommand cmd = new SqlCommand("Prc_InsertHierarchy", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@hierarcy_data", tabledata);
                SqlParameter obj_hier = new SqlParameter();
                obj_hier.ParameterName = "@message";
                obj_hier.SqlDbType = SqlDbType.NVarChar;
                obj_hier.Size = 70;
                obj_hier.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(obj_hier);
                con.Open();
                cmd.ExecuteNonQuery();
                res = Convert.ToString(obj_hier.Value);
                con.Close();
            }
            catch (Exception ex)
            {
                res = ex.Message;
            }
            return res;
        }
        //public DataTable GetHierarchy()
        //{
        //    try
        //    {
        //        connection();
        //        SqlCommand cmd = new SqlCommand("Prc_GetHierarchy", con);
        //        cmd.CommandType = CommandType.StoredProcedure;
        //        con.Open();
        //        SqlDataAdapter da = new SqlDataAdapter(cmd);
        //        da.Fill(dt);
        //        con.Close();
        //    }
        //    catch(Exception ex)
        //    {

        //    }
        //    return dt;
        //}
        public DataTable GetHierarchy(int hierarchy_id)
        {
            try
            {
                connection();
                SqlCommand cmd = new SqlCommand("Prc_GetHierarchy", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("hierarchy_id", hierarchy_id);
                con.Open();
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                con.Close();
            }
            catch (Exception ex)
            {

            }
            return dt;
        }
        public DataTable GetHierarchyByName(string hierarchy_name)
        {
            try
            {
                connection();
                SqlCommand cmd = new SqlCommand("PRC_HierarchyByName", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@hierarchy_name", hierarchy_name);
                con.Open();
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                con.Close();
            }
            catch (Exception ex)
            {

            }
            return dt;
        }
        public string update_hierarchy(int hierarchy_id, string hierarchy_level, string hierarchy_value, string status)
        {
            string res = "";
            try
            {
                connection();
                SqlCommand cmd = new SqlCommand("Prc_UpdateHierarchy", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@hierarchy_id", hierarchy_id);
                cmd.Parameters.AddWithValue("@hierarchy_level", hierarchy_level);
                cmd.Parameters.AddWithValue("@hierarchy_value", hierarchy_value);
                cmd.Parameters.AddWithValue("@status", status);
                SqlParameter obj_hier = new SqlParameter();
                obj_hier.ParameterName = "@message";
                obj_hier.SqlDbType = SqlDbType.NVarChar;
                obj_hier.Size = 50;
                obj_hier.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(obj_hier);
                con.Open();
                cmd.ExecuteNonQuery();
                res = Convert.ToString(obj_hier.Value);
                con.Close();
            }
            catch (Exception ex)
            {
                res = ex.Message;
            }
            return res;
        }

        public string CreateSystemUser(int emp_id, string password, int is_navone, string status, int role_id, string emp_code, DateTime? start_date, DateTime? end_date, int store_id, int chkmanagement, int chkhr, int chkfh)
        {
            string res = "";
            try
            {
                connection();
                SqlCommand cmd = new SqlCommand("Prc_InsertSystemUser", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@emp_id", emp_id);
                cmd.Parameters.AddWithValue("@password", password);
                cmd.Parameters.AddWithValue("@store_id", store_id);
                cmd.Parameters.AddWithValue("@is_navone", is_navone);
                cmd.Parameters.AddWithValue("@status", status);
                cmd.Parameters.AddWithValue("@role_id", role_id);
                cmd.Parameters.AddWithValue("@emp_code", emp_code);
                cmd.Parameters.AddWithValue("@start_date", start_date);
                cmd.Parameters.AddWithValue("@chk_management", chkmanagement);
                cmd.Parameters.AddWithValue("@chk_hr", chkhr);
                cmd.Parameters.AddWithValue("@chk_fd", chkfh);
                SqlParameter obj_hier = new SqlParameter();
                obj_hier.ParameterName = "@message";
                obj_hier.SqlDbType = SqlDbType.NVarChar;
                obj_hier.Size = 100;
                obj_hier.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(obj_hier);
                con.Open();
                cmd.ExecuteNonQuery();
                res = Convert.ToString(obj_hier.Value);
                con.Close();
            }
            catch (Exception ex)
            {
                res = ex.Message;
            }
            return res;
        }
        public string UpdateSystemUser(int emp_id, int user_id, string password, int is_navone, string status, int role_id, DateTime? start_date, DateTime? end_date, int store_id, int chkmanagement, int chkhr, int chkfh)
        {
            string res = "";
            try
            {
                connection();
                SqlCommand cmd = new SqlCommand("Prc_UpdateSystemUser", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@emp_id", emp_id);
                cmd.Parameters.AddWithValue("@user_id", user_id);
                cmd.Parameters.AddWithValue("@password", password);
                cmd.Parameters.AddWithValue("@is_navone", is_navone);
                cmd.Parameters.AddWithValue("@status", status);
                cmd.Parameters.AddWithValue("@store_id", store_id);
                cmd.Parameters.AddWithValue("@role_id", role_id);
                cmd.Parameters.AddWithValue("@start_date", start_date);
                cmd.Parameters.AddWithValue("@end_date", end_date);
                cmd.Parameters.AddWithValue("@chk_management", chkmanagement);
                cmd.Parameters.AddWithValue("@chk_hr", chkhr);
                cmd.Parameters.AddWithValue("@chk_fd", chkfh);
                SqlParameter obj_hier = new SqlParameter();
                obj_hier.ParameterName = "@message";
                obj_hier.SqlDbType = SqlDbType.NVarChar;
                obj_hier.Size = 100;
                obj_hier.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(obj_hier);
                con.Open();
                cmd.ExecuteNonQuery();
                res = Convert.ToString(obj_hier.Value);
                con.Close();
            }
            catch (Exception ex)
            {
                res = ex.Message;
            }
            return res;
        }
        public DataTable GetEmployeeById(int EMP_ID)
        {
            try
            {
                connection();
                SqlCommand cmd = new SqlCommand("Prc_GetEmployeeById", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@emp_id", EMP_ID);
                con.Open();
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                con.Close();
            }
            catch (Exception ex)
            {

            }
            return dt;
        }

        public DataSet GetAttendanceEntry(int week_no, int year, int month, int region, int dept, int loc, string ecode, string ename, string emp_login_id)
        {
            try
            {
                connection();
                SqlCommand cmd = new SqlCommand("PRC_AttendanceEntry", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@week_number", week_no);
                cmd.Parameters.AddWithValue("@year", year);
                cmd.Parameters.AddWithValue("@month", month);
                cmd.Parameters.AddWithValue("@EmployeeLoginID", emp_login_id);
                cmd.Parameters.AddWithValue("@PersonID", ecode);
                cmd.Parameters.AddWithValue("@EmpName", ename);
                cmd.Parameters.AddWithValue("@Regin", region);
                cmd.Parameters.AddWithValue("@Depart", dept);
                cmd.Parameters.AddWithValue("@Location", loc);
                con.Open();
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.SelectCommand.CommandTimeout = 120;
                da.Fill(ds);
                con.Close();
            }
            catch (Exception ex)

            {

            }
            return ds;
        }

        public string Update_AttendanceAll(string attendance_date, string emp_id, string attendance_type)
        {
            string res = "";
            try
            {
                connection();
                SqlCommand cmd = new SqlCommand("Prc_UpdateAttendance_EntryAll", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@attendance_date", attendance_date);
                cmd.Parameters.AddWithValue("@emp_id", emp_id);
                cmd.Parameters.AddWithValue("@attendance_type", attendance_type);
                SqlParameter attobj = new SqlParameter();
                attobj.ParameterName = "@message";
                attobj.SqlDbType = SqlDbType.NVarChar;
                attobj.Size = 100;
                attobj.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(attobj);
                con.Open();
                cmd.ExecuteNonQuery();
                res = Convert.ToString(attobj.Value);
                con.Close();
            }
            catch (Exception ex)
            {

            }
            finally
            {
                con.Close();
            }
            return res;
        }
        public DataSet GetAttendanceEntrySearch(int week_no, int year, int month, int region, int dept, int loc, string ecode, string ename, string emp_login_id)
        {
            try
            {
                connection();
                SqlCommand cmd = new SqlCommand("PRC_AttendanceEntrySearch", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@week_number", week_no);
                cmd.Parameters.AddWithValue("@year", year);
                cmd.Parameters.AddWithValue("@month", month);
                cmd.Parameters.AddWithValue("@EmployeeLoginID", emp_login_id);
                cmd.Parameters.AddWithValue("@PersonID", ecode);
                cmd.Parameters.AddWithValue("@EmpName", ename);
                cmd.Parameters.AddWithValue("@Regin", region);
                cmd.Parameters.AddWithValue("@Depart", dept);
                cmd.Parameters.AddWithValue("@Location", loc);
                con.Open();
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.SelectCommand.CommandTimeout = 120;
                da.Fill(ds);
                con.Close();
            }
            catch (Exception ex)
            {

            }
            return ds;
        }
        public DataSet GetAttendanceSummarySearch(string manager_id, int year, int month, int region, int dept, int loc, string ecode, string ename, string emp_login_id)
        {
            DataSet dss = new DataSet();
            try
            {

                connection();
                SqlCommand cmd = new SqlCommand("PRC_AttendanceSummaryReport", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@year", year);
                cmd.Parameters.AddWithValue("@month", month);
                cmd.Parameters.AddWithValue("@EmployeeLoginID", emp_login_id);
                cmd.Parameters.AddWithValue("@PersonID", ecode);
                cmd.Parameters.AddWithValue("@EmpName", ename);
                cmd.Parameters.AddWithValue("@Regin", region);
                cmd.Parameters.AddWithValue("@Depart", dept);
                cmd.Parameters.AddWithValue("@Location", loc);
                cmd.Parameters.AddWithValue("@ManagerID", manager_id == "0" ? "" : manager_id);
                con.Open();
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.SelectCommand.CommandTimeout = 120;
                da.Fill(dss);
                con.Close();
            }
            catch (Exception ex)
            {

            }
            return dss;
        }
        public DataTable GetAttendanceRegister(string Month, string year, string emp_login_id, string empcode, string empname, string region, string depart, string loc, string mgrid)
        {
            try
            {
                connection();
                SqlCommand cmd = new SqlCommand("Prc_Attendance_Register", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@month", Month);
                cmd.Parameters.AddWithValue("@year", year);
                cmd.Parameters.AddWithValue("@EmployeeLoginID", emp_login_id);
                cmd.Parameters.AddWithValue("@PersonID", empcode);
                cmd.Parameters.AddWithValue("@EmpName", empname);
                cmd.Parameters.AddWithValue("@Regin", region);
                cmd.Parameters.AddWithValue("@Depart", depart);
                cmd.Parameters.AddWithValue("@Location", loc);
                cmd.Parameters.AddWithValue("@ManagerID", mgrid);

                con.Open();
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.SelectCommand.CommandTimeout = 120;
                da.Fill(dt);
                con.Close();
            }
            catch (Exception ex)
            {

            }
            return dt;
        }
        public DataTable GetAttendanceRegister_Approval(string Month, string year, string emp_login_id, string empcode, string empname, string region, string depart, string loc, string mgrid)
        {
            try
            {
                connection();
                SqlCommand cmd = new SqlCommand("Prc_Attendance_Register_Approval", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@month", Month);
                cmd.Parameters.AddWithValue("@year", year);
                cmd.Parameters.AddWithValue("@EmployeeLoginID", emp_login_id);
                cmd.Parameters.AddWithValue("@PersonID", empcode);
                cmd.Parameters.AddWithValue("@EmpName", empname);
                cmd.Parameters.AddWithValue("@Regin", region);
                cmd.Parameters.AddWithValue("@Depart", depart);
                cmd.Parameters.AddWithValue("@Location", loc);
                cmd.Parameters.AddWithValue("@ManagerID", mgrid);

                con.Open();
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.SelectCommand.CommandTimeout = 120;
                da.Fill(dt);
                con.Close();
            }
            catch (Exception ex)
            {

            }
            return dt;
        }
        public string Update_Attendance(DateTime attendance_date, int emp_id, string attendance_type)
        {
            string res = "";
            try
            {
                connection();
                SqlCommand cmd = new SqlCommand("Prc_UpdateAttendance_Entry", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@attendance_date", attendance_date);
                cmd.Parameters.AddWithValue("@emp_id", emp_id);
                cmd.Parameters.AddWithValue("@attendance_type", attendance_type);
                SqlParameter attobj = new SqlParameter();
                attobj.ParameterName = "@message";
                attobj.SqlDbType = SqlDbType.NVarChar;
                attobj.Size = 100;
                attobj.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(attobj);
                con.Open();
                cmd.ExecuteNonQuery();
                res = Convert.ToString(attobj.Value);
                con.Close();
            }
            catch (Exception ex)
            {

            }
            finally
            {
                con.Close();
            }
            return res;
        }
        public DataTable getEmployeeNotSystemUser()
        {
            DataTable dt = new DataTable();
            string res = "";
            try
            {
                connection();
                con.Open();
                using (SqlDataAdapter da = new SqlDataAdapter("Proc_GetEmpDetail_NotSystemUser", con))
                {
                    da.Fill(dt);
                    con.Close();
                }
            }
            catch (Exception ex)
            {
                res = ex.Message;
            }
            return dt;
        }

        public string Approve_Attendance(int emp_id, DateTime? first_date, DateTime? second_date, DateTime? third_date, DateTime? fourth_date, DateTime? five_date, DateTime? six_date, DateTime? seven_date)
        {
            string res = "";
            try
            {
                connection();
                SqlCommand cmd = new SqlCommand("Prc_UAttendance_Entry", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@first_date", first_date);
                cmd.Parameters.AddWithValue("@second_date", second_date);
                cmd.Parameters.AddWithValue("@third_date", third_date);
                cmd.Parameters.AddWithValue("@fourth_date", fourth_date);
                cmd.Parameters.AddWithValue("@five_date", five_date);
                cmd.Parameters.AddWithValue("@six_date", six_date);
                cmd.Parameters.AddWithValue("@seven_date", seven_date);
                cmd.Parameters.AddWithValue("@emp_id", emp_id);
                SqlParameter attobj = new SqlParameter();
                attobj.ParameterName = "@message";
                attobj.SqlDbType = SqlDbType.NVarChar;
                attobj.Size = 100;
                attobj.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(attobj);
                con.Open();
                cmd.ExecuteNonQuery();
                res = Convert.ToString(attobj.Value);
                con.Close();
            }
            catch (Exception ex)
            {

            }
            return res;
        }
        public string CheckAttendanceStatus(DateTime attendance_date, int emp_id)
        {
            string res = "";
            try
            {
                connection();
                SqlCommand cmd = new SqlCommand("Prc_CheckAttendanceStatus", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@previous_month", attendance_date);
                cmd.Parameters.AddWithValue("@emp_id", emp_id);
                SqlParameter attobj = new SqlParameter();
                attobj.ParameterName = "@message";
                attobj.SqlDbType = SqlDbType.NVarChar;
                attobj.Size = 50;
                attobj.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(attobj);
                con.Open();
                cmd.ExecuteNonQuery();
                res = Convert.ToString(attobj.Value);
                con.Close();
            }
            catch (Exception ex)
            {

            }
            return res;
        }

        public string bind_hierarchy_name()
        {

            string res = "";
            connection();
            SqlCommand cmd = new SqlCommand("Prc_Get_Hierarchy_Name", con);
            cmd.CommandType = CommandType.StoredProcedure;
            con.Open();
            SqlDataReader dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                if (res == "")
                {
                    res = dr["HIERARCHY_NAME"].ToString();
                }
                else
                {
                    res += "," + dr["HIERARCHY_NAME"].ToString();
                }

            }
            con.Close();

            return res;
        }
        public string bind_hierarchy_level(String Name)
        {

            string res = "";
            connection();
            SqlCommand cmd = new SqlCommand("Prc_Get_Hierarchy_Level", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@name", Name);
            con.Open();
            SqlDataReader dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                if (res == "")
                {
                    res = dr["HIERARCHY_LEVEL"].ToString();
                }
                else
                {
                    res += "," + dr["HIERARCHY_LEVEL"].ToString();
                }

            }
            con.Close();

            return res;
        }


        public string Add_Approval_Rule(ApprovalRule ar, string createdby)
        {
            string res = "";
            try
            {
                ListtoDataTableConverter converter = new ListtoDataTableConverter();
                DataTable dt = new DataTable();
                if (ar.condition != null)
                {
                    dt = converter.ToDataTable(ar.condition);
                }
                else
                { dt = null; }


                connection();
                SqlCommand cmd = new SqlCommand("Prc_Add_Approval_Rule", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@ID", ar.ID);
                cmd.Parameters.AddWithValue("@for", ar.For);
                cmd.Parameters.AddWithValue("@type", ar.Type);
                cmd.Parameters.AddWithValue("@approvaltypefor", ar.ApprovalTypeFor);
                cmd.Parameters.AddWithValue("@rulename", ar.RuleName);
                cmd.Parameters.AddWithValue("@startdate", ar.StartDate);
                cmd.Parameters.AddWithValue("@enddate", ar.EndDate);
                cmd.Parameters.AddWithValue("@status", ar.Status);
                cmd.Parameters.AddWithValue("@tblCondition", dt);
                cmd.Parameters.AddWithValue("@createdby", createdby);
                SqlParameter attobj = new SqlParameter();
                attobj.ParameterName = "@message";
                attobj.SqlDbType = SqlDbType.NVarChar;
                attobj.Size = 50;
                attobj.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(attobj);
                con.Open();
                cmd.ExecuteNonQuery();
                res = Convert.ToString(attobj.Value);
                con.Close();
            }
            catch (Exception ex)
            {

            }
            return res;
        }
        public DataSet Get_all_approval_rules(string approval_for, string id)
        {
            DataSet ds = new DataSet();
            string res = "";
            try
            {
                connection();
                using (SqlCommand cmd = new SqlCommand("Prc_Get_Approval_Rules", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@approvalfor", approval_for);
                    cmd.Parameters.AddWithValue("@id", id);
                    con.Open();
                    using (SqlDataAdapter da = new SqlDataAdapter())
                    {
                        da.SelectCommand = cmd;
                        da.Fill(ds);
                    }
                    con.Close();
                }
            }
            catch (Exception ex)
            {
                res = ex.Message;
            }
            return ds;
        }

        public DataSet Get_all_approval_rules_edit(string id)
        {
            DataSet ds = new DataSet();
            string res = "";
            try
            {
                connection();
                // using (SqlCommand cmd = new SqlCommand("Prc_Edit_Get_Approval_Rules", con))
                using (SqlCommand cmd = new SqlCommand("Prc_Edit_Approval_Rules", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    //   cmd.Parameters.AddWithValue("@approvalfor", approval_for);
                    cmd.Parameters.AddWithValue("@id", id);
                    con.Open();
                    using (SqlDataAdapter da = new SqlDataAdapter())
                    {
                        da.SelectCommand = cmd;
                        da.Fill(ds);
                    }
                    con.Close();
                }
            }
            catch (Exception ex)
            {
                res = ex.Message;
            }
            return ds;
        }

        public string Update_AttendanceAll_Approval(string status, string emp_id, string month, string year, string createdby)
        {
            string res = "";
            try
            {
                connection();
                SqlCommand cmd = new SqlCommand("Prc_UpdateAttendance_EntryAll_Approval", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@emp_id", emp_id);
                cmd.Parameters.AddWithValue("@month", month);
                cmd.Parameters.AddWithValue("@year", year);
                cmd.Parameters.AddWithValue("@Status", status);
                cmd.Parameters.AddWithValue("@Createdby", createdby);
                SqlParameter attobj = new SqlParameter();
                attobj.ParameterName = "@message";
                attobj.SqlDbType = SqlDbType.NVarChar;
                attobj.Size = 100;
                attobj.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(attobj);
                con.Open();
                cmd.ExecuteNonQuery();
                res = Convert.ToString(attobj.Value);
                con.Close();
            }
            catch (Exception ex)
            {

            }
            finally
            {
                con.Close();
            }
            return res;
        }

        public string Update_AttendanceAll_Submit(string status, string emp_id, string month, string year, string createdby)
        {
            string res = "";
            try
            {
                connection();
                SqlCommand cmd = new SqlCommand("Prc_UpdateAttendance_EntryAll_Submit", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@emp_id", emp_id);
                cmd.Parameters.AddWithValue("@month", month);
                cmd.Parameters.AddWithValue("@year", year);
                cmd.Parameters.AddWithValue("@Status", status);
                cmd.Parameters.AddWithValue("@Createdby", createdby);
                SqlParameter attobj = new SqlParameter();
                attobj.ParameterName = "@message";
                attobj.SqlDbType = SqlDbType.NVarChar;
                attobj.Size = 100;
                attobj.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(attobj);
                con.Open();
                cmd.ExecuteNonQuery();
                res = Convert.ToString(attobj.Value);
                con.Close();
            }
            catch (Exception ex)
            {

            }
            finally
            {
                con.Close();
            }
            return res;
        }

        //deligaiton
        public string Add_delegation(delegation dl, string empl_id)
        {
            string res = "";
            try
            {
                connection();
                SqlCommand cmd = new SqlCommand("Prc_AddDelegation", con);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@id", dl.ID);
                cmd.Parameters.AddWithValue("@emp_id", dl.employee_id);
                cmd.Parameters.AddWithValue("@fromdate", dl.fromdate);
                cmd.Parameters.AddWithValue("@todate", dl.todate);
                cmd.Parameters.AddWithValue("@status", dl.status);
                cmd.Parameters.AddWithValue("@delgiationtype", dl.type);
                cmd.Parameters.AddWithValue("@createdby", empl_id);
                SqlParameter attobj = new SqlParameter();
                attobj.ParameterName = "@message";
                attobj.SqlDbType = SqlDbType.NVarChar;
                attobj.Size = 150;
                attobj.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(attobj);
                con.Open();
                cmd.ExecuteNonQuery();
                res = Convert.ToString(attobj.Value);
                con.Close();
            }
            catch (Exception ex)
            {

            }
            finally
            {
                con.Close();
            }
            return res;

        }
        public DataSet delegation_emp_list(int empl_id)
        {
            DataSet ds = new DataSet();
            string res = "";
            try
            {
                connection();
                SqlCommand cmd = new SqlCommand("PRC_DelegationEmployeeList", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@EmployeeID", empl_id);
                con.Open();
                using (SqlDataAdapter adp = new SqlDataAdapter(cmd))
                {
                    adp.Fill(ds);
                }
                con.Close();
            }
            catch (Exception ex)
            {

            }
            finally
            {
                con.Close();
            }
            return ds;

        }
        public DataTable Binddelegation(string empl_id)
        {
            string res = "";
            DataTable dt = new DataTable();
            try
            {
                connection();
                SqlCommand cmd = new SqlCommand("Prc_GetDelegation_List", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@createdby", empl_id);
                con.Open();
                using (SqlDataAdapter adp = new SqlDataAdapter(cmd))
                {
                    adp.Fill(dt);
                }
                con.Close();
            }
            catch (Exception ex)
            {

            }
            finally
            {
                con.Close();
            }
            return dt;

        }

        public DataTable Get_delegation(int id)
        {
            string res = "";
            DataTable dt = new DataTable();
            try
            {
                connection();
                SqlCommand cmd = new SqlCommand("Prc_GetDelegationbyid", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@id", id);
                con.Open();
                using (SqlDataAdapter adp = new SqlDataAdapter(cmd))
                {
                    adp.Fill(dt);
                }
                con.Close();
            }
            catch (Exception ex)
            {

            }
            finally
            {
                con.Close();
            }
            return dt;

        }
        public DataTable Get_delegation_employee_on_type(string type, string id)
        {
            string res = "";
            DataTable dt = new DataTable();
            try
            {
                connection();
                SqlCommand cmd = new SqlCommand("Prc_GetDelegationEmployeeOnType", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Type", type);
                cmd.Parameters.AddWithValue("@emp_id", id);
                con.Open();
                using (SqlDataAdapter adp = new SqlDataAdapter(cmd))
                {
                    adp.Fill(dt);
                }
                con.Close();
            }
            catch (Exception ex)
            {

            }
            finally
            {
                con.Close();
            }
            return dt;

        }

        public DataTable Get_delegation_employee_on_type_edit(string type, int delegationempid, string id)
        {
            string res = "";
            DataTable dt = new DataTable();
            try
            {
                connection();
                SqlCommand cmd = new SqlCommand("Prc_GetDelegationEmployeeOnType_edit", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Type", type);
                cmd.Parameters.AddWithValue("@delegationapplyempid", delegationempid);
                cmd.Parameters.AddWithValue("@emp_id", id);
                con.Open();
                using (SqlDataAdapter adp = new SqlDataAdapter(cmd))
                {
                    adp.Fill(dt);
                }
                con.Close();
            }
            catch (Exception ex)
            {

            }
            finally
            {
                con.Close();
            }
            return dt;

        }

        public string SendMailBySMTPNew(string FromID, string ToID, string msgSub, string msgBody)
        {

            MailMessage mm = new MailMessage();
            mm.From = new MailAddress("ots.support@prudencesoftech.net");
            mm.Subject = msgSub;
            mm.Body = msgBody;
            mm.IsBodyHtml = true;
            using (SmtpClient smtp = new SmtpClient())
            {
                string[] multil = ToID.Split(',');
                foreach (string mutimail in multil)
                {
                    mm.To.Add(mutimail);
                }
                smtp.Host = "mail.prudencesoftech.net";
                smtp.EnableSsl = false;
                NetworkCredential NetworkCred = new NetworkCredential("ots.support@prudencesoftech.net", "Tc#wo6s3eyYutbdaC5PP.BxC");
                smtp.UseDefaultCredentials = true;
                smtp.Credentials = NetworkCred;
                smtp.Port = 587;
                try
                {
                    smtp.Send(mm);
                    return "Mail sent successfully !";
                }
                catch (Exception ex)
                {
                    return ex.Message.ToString();
                }
            }


        }



        public DataSet Get_Employee_Mail_info(int id)
        {
            string res = "";
            DataSet dt = new DataSet();
            try
            {
                connection();
                SqlCommand cmd = new SqlCommand("PRC_Notification_EMAILAddress", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@EmployeeID", id);
                con.Open();
                using (SqlDataAdapter adp = new SqlDataAdapter(cmd))
                {
                    adp.Fill(dt);
                }
                con.Close();
            }
            catch (Exception ex)
            {

            }
            finally
            {
                con.Close();
            }
            return dt;

        }
        public DataSet Get_Employee_Mail_info_Expense_Fuel(int id)
        {
            string res = "";
            DataSet dt = new DataSet();
            try
            {
                connection();
                SqlCommand cmd = new SqlCommand("PRC_Notification_Expense_Fuel", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@EmployeeID", id);
                con.Open();
                using (SqlDataAdapter adp = new SqlDataAdapter(cmd))
                {
                    adp.Fill(dt);
                }
                con.Close();
            }
            catch (Exception ex)
            {

            }
            finally
            {
                con.Close();
            }
            return dt;

        }

        public DataSet Get_Employee_Mail_info_Transfer_Claim(int id, int transfer_claim_id)
        {
            string res = "";
            DataSet dt = new DataSet();
            try
            {
                connection();
                SqlCommand cmd = new SqlCommand("PRC_Notification_TransferClaim", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@EmployeeID", id);
                cmd.Parameters.AddWithValue("@transfer_claim_id", transfer_claim_id);
                con.Open();
                using (SqlDataAdapter adp = new SqlDataAdapter(cmd))
                {
                    adp.Fill(dt);
                }
                con.Close();
            }
            catch (Exception ex)
            {

            }
            finally
            {
                con.Close();
            }
            return dt;

        }


        public DataSet Get_Employee_Mail_info_on_approve(int id)
        {
            string res = "";
            DataSet dt = new DataSet();
            try
            {
                connection();
                SqlCommand cmd = new SqlCommand("PRC_Notification_EMAILAddress_on_approve", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@applyleave_id", id);
                con.Open();
                using (SqlDataAdapter adp = new SqlDataAdapter(cmd))
                {
                    adp.Fill(dt);
                }
                con.Close();
            }
            catch (Exception ex)
            {

            }
            finally
            {
                con.Close();
            }
            return dt;

        }

        public DataSet Get_Expense_Employee_Mail_info_on_approve(int id, string expense_name)
        {
            string res = "";
            DataSet dt = new DataSet();
            try
            {
                connection();
                SqlCommand cmd = new SqlCommand("PRC_Notification_EMAILAddress_Expense_on_approve", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@id", id);
                cmd.Parameters.AddWithValue("@expense_name", expense_name);
                con.Open();
                using (SqlDataAdapter adp = new SqlDataAdapter(cmd))
                {
                    adp.Fill(dt);
                }
                con.Close();
            }
            catch (Exception ex)
            {

            }
            finally
            {
                con.Close();
            }
            return dt;

        }


        public DataSet Get_Employee_Mail_info_on_attendence(int id, int mgr_id)
        {
            string res = "";
            DataSet dt = new DataSet();
            try
            {
                connection();
                SqlCommand cmd = new SqlCommand("PRC_Notification_EMAILAddress_on_attendence", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@EmployeeID", id);
                cmd.Parameters.AddWithValue("@emp_mnger_id", mgr_id);
                con.Open();
                using (SqlDataAdapter adp = new SqlDataAdapter(cmd))
                {
                    adp.Fill(dt);
                }
                con.Close();
            }
            catch (Exception ex)
            {

            }
            finally
            {
                con.Close();
            }
            return dt;

        }
        public DataSet GetEmpDashBoard(int emp_id, int month, int year)
        {
            try
            {
                connection();
                using (SqlCommand cmd = new SqlCommand("Prc_EmployeeDashBoard", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@emp_id", emp_id);
                    cmd.Parameters.AddWithValue("@month", month);
                    cmd.Parameters.AddWithValue("@year", year);

                    con.Open();
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(ds);
                    con.Close();
                    return ds;
                }
            }
            catch (Exception ex)
            {

            }
            return ds;
        }
        public DataSet get_empleave_detail_mgrby(int emp_id)
        {
            DataSet ds = new DataSet();
            try
            {
                connection();
                using (SqlCommand cmd = new SqlCommand("PRC_Dashboard_LeaveDetails", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Emp_id", emp_id);
                    con.Open();
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(ds);
                    con.Close();
                    return ds;
                }
            }
            catch (Exception ex)
            {

            }
            return ds;
        }

        public DataSet get_empatten_detail_mgrby(int emp_id)
        {
            DataSet ds = new DataSet();
            try
            {
                connection();
                using (SqlCommand cmd = new SqlCommand("PRC_EmployeeAttendanceDetailHeadDetails", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Emp_id", emp_id);
                    con.Open();
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(ds);
                    con.Close();
                    return ds;
                }
            }
            catch (Exception ex)
            {

            }
            return ds;
        }

        public DataSet get_emp_detail_mgrby(int emp_id)
        {
            DataSet ds = new DataSet();
            try
            {
                connection();
                using (SqlCommand cmd = new SqlCommand("PRC_EmployeeAttendanceDetailHeadDetails", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Emp_id", emp_id);
                    con.Open();
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(ds);
                    con.Close();
                    return ds;
                }
            }
            catch (Exception ex)
            {

            }
            return ds;
        }

        public DataSet get_emp_detail(int emp_id)
        {
            DataSet ds = new DataSet();
            try
            {
                connection();
                using (SqlCommand cmd = new SqlCommand("PRC_Dashboard_Employment_details", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Emp_id", emp_id);
                    con.Open();
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(ds);
                    con.Close();
                    return ds;
                }
            }
            catch (Exception ex)
            {

            }
            return ds;
        }


        //Ankit Rajput For Genrate Email Logs
        public static string GenrateEmailLogs(string email_id, string email_type, int emp_id, string message, string status, int tran_no)
        {
            string res = "";
            try
            {
                using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["conn"].ToString()))
                {
                    using (SqlCommand cmd = new SqlCommand("Prc_UspEmailLog", con))
                    {

                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@EMAIL_ID", email_id);
                        cmd.Parameters.AddWithValue("@EMAIL_TYPE", email_type);
                        cmd.Parameters.AddWithValue("@TRAN_NO", tran_no);
                        //cmd.Parameters.AddWithValue("@DOC_NO", doc_no);
                        //cmd.Parameters.AddWithValue("@STORE_NAME", store_name);
                        cmd.Parameters.AddWithValue("@EMP_ID", emp_id);
                        cmd.Parameters.AddWithValue("@MESSAGE", message);
                        cmd.Parameters.AddWithValue("@STATUS", status);
                        SqlParameter objdept = new SqlParameter();
                        objdept.ParameterName = "@out_message";
                        objdept.SqlDbType = SqlDbType.NVarChar;
                        objdept.Size = 50;
                        objdept.Direction = ParameterDirection.Output;
                        cmd.Parameters.Add(objdept);
                        con.Open();
                        cmd.ExecuteNonQuery();
                        res = Convert.ToString(objdept.Value);
                        con.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                res = ex.Message;
            }
            return res;
        }

        #region Reset Password
        public string validateresetpassword(string guid, int expireTime)
        {
            string res = "";
            connection();
            SqlCommand cmd = new SqlCommand("prc_validateResetPassword", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@guid", guid);
            cmd.Parameters.AddWithValue("@expireTime", expireTime);
            SqlParameter oblogin = new SqlParameter();
            oblogin.ParameterName = "@message";
            oblogin.SqlDbType = SqlDbType.NVarChar;
            oblogin.Size = 50;
            oblogin.Direction = ParameterDirection.Output;
            cmd.Parameters.Add(oblogin);
            con.Open();
            cmd.ExecuteNonQuery();
            res = Convert.ToString(oblogin.Value);

            con.Close();
            return res;
        }

        public string resetpassword(string new_password, int emp_id, int expireTime)
        {
            string res = "";
            connection();
            SqlCommand cmd = new SqlCommand("Prc_ResetPassword", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@Newpassword", new_password);
            cmd.Parameters.AddWithValue("@UserId", emp_id);
            cmd.Parameters.AddWithValue("@expireTime", expireTime);
            SqlParameter oblogin = new SqlParameter();
            oblogin.ParameterName = "@message";
            oblogin.SqlDbType = SqlDbType.NVarChar;
            oblogin.Size = 50;
            oblogin.Direction = ParameterDirection.Output;
            cmd.Parameters.Add(oblogin);
            con.Open();
            cmd.ExecuteNonQuery();
            res = Convert.ToString(oblogin.Value);

            con.Close();
            return res;
        }
        #endregion

        public DataSet Get_Delegation_Employee(int emp_id)
        {
            string res = "";
            DataSet dt = new DataSet();
            try
            {
                connection();
                SqlCommand cmd = new SqlCommand("PRC_GET_DELEGATION_DEVICE_TOKEN", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@emp_id", emp_id);
                con.Open();
                using (SqlDataAdapter adp = new SqlDataAdapter(cmd))
                {
                    adp.Fill(dt);
                }
                con.Close();
            }
            catch (Exception ex)
            {

            }
            finally
            {
                con.Close();
            }
            return dt;

        }
        public string save_Notification(string TITLE, string MESSAGE, string SCREEN_NAME = "", string month = "0", int year = 0, int SENDER_EMP_ID = 0, int RECIEVER_EMP_ID = 0)
        {
            try
            {
                string res = "";
                connection();
                using (SqlCommand cmd = new SqlCommand("PRC_INSERT_GET_NOTIFICATION", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@TITLE", TITLE);
                    cmd.Parameters.AddWithValue("@BODY", MESSAGE);
                    cmd.Parameters.AddWithValue("@SCREEN_NAME", SCREEN_NAME);
                    cmd.Parameters.AddWithValue("@MONTH", month);
                    cmd.Parameters.AddWithValue("@YEAR", year);
                    cmd.Parameters.AddWithValue("@RECEIVER_EMP_ID", RECIEVER_EMP_ID);
                    cmd.Parameters.AddWithValue("@SENDER_EMP_ID", SENDER_EMP_ID);
                    SqlParameter oparm = new SqlParameter();
                    oparm.ParameterName = "@message";
                    oparm.Size = 100;
                    oparm.Direction = ParameterDirection.Output;
                    cmd.Parameters.Add(oparm);

                    con.Open();
                    cmd.ExecuteNonQuery();
                    res = Convert.ToString(oparm.Value);
                    con.Close();
                }
                return "save";
            }
            catch (Exception ex)
            {
                return "error";

            }
        }


        public string Chagen_Event(int id, string event_name, int emp_id)
        {
            string res = "";
            try
            {
                connection();
                SqlCommand cmd = new SqlCommand("Prc_Change_Calendar_Event", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@emp_id", emp_id);
                cmd.Parameters.AddWithValue("@id", id);
                cmd.Parameters.AddWithValue("@event_name", event_name);

                SqlParameter outparam = new SqlParameter();
                outparam.ParameterName = "@message";
                outparam.SqlDbType = SqlDbType.NVarChar;
                outparam.Size = 150;
                outparam.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(outparam);
                con.Open();
                cmd.ExecuteNonQuery();

                res = outparam.Value.ToString();


                con.Close();

            }
            catch
            {
                res = "error";

            }

            return res;

        }
        public DataTable Get_Event(int emp_id, DateTime from_date, DateTime end_date)
        {

            try
            {
                connection();
                SqlCommand cmd = new SqlCommand("Prc_GetEventFullCalendar", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@emp_id", emp_id);
                cmd.Parameters.AddWithValue("@startdate", from_date);
                cmd.Parameters.AddWithValue("@enddate", end_date);
                con.Open();
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                con.Close();
            }
            catch (Exception ex)
            {

            }
            return dt;

        }
    }
}