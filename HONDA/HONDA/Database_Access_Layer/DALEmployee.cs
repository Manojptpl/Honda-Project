using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using HONDA.Models;
using System.Web.Mvc;

namespace HONDA.Database_Access_Layer
{
    public class DALEmployee
    {
        DataTable dt = new DataTable();
        private string connection()
        {
            return ConfigurationManager.ConnectionStrings["conn"].ToString();
        }
       
        //public string AddEmployeeLeaveMapping(EmployeeLeaveMapping elm, string Createdby)
        //{
        //    string res = "";
        //    try
        //    {

        //        using (SqlConnection con = new SqlConnection(connection()))
        //        {
        //            using (SqlCommand cmd = new SqlCommand("Prc_Employee_LeaveMapping1", con))
        //            {
        //                cmd.CommandType = CommandType.StoredProcedure;
        //                cmd.Parameters.AddWithValue("@applicablefor", elm.applicablefor);
        //                cmd.Parameters.AddWithValue("@person_id", elm.employeeid);
        //                cmd.Parameters.AddWithValue("@gender", elm.gender);
        //                cmd.Parameters.AddWithValue("@leave_type_id", elm.allleavetype);
        //                cmd.Parameters.AddWithValue("@start_date", elm.startdate);
        //                cmd.Parameters.AddWithValue("@doj", elm.doj);
        //                cmd.Parameters.AddWithValue("@specific_date", elm.specificdate);
        //                cmd.Parameters.AddWithValue("@end_date", elm.enddate);
        //                cmd.Parameters.AddWithValue("@status", elm.status);
        //                cmd.Parameters.AddWithValue("@created_by", Createdby);
        //                SqlParameter oblogin = new SqlParameter();
        //                oblogin.ParameterName = "@message";
        //                oblogin.SqlDbType = SqlDbType.NVarChar;
        //                oblogin.Size = 50;
        //                oblogin.Direction = ParameterDirection.Output;
        //                cmd.Parameters.Add(oblogin);
        //                con.Open();
        //                cmd.ExecuteNonQuery();
        //                res = Convert.ToString(oblogin.Value);
        //                con.Close();
        //            }
        //        }
        //        return res;
        //    }
        //    catch (Exception ex)
        //    {
        //        return ex.Message;

        //    }

        //}

        //public string UpdateEmployeeLeaveMapping(EmployeeLeaveMapping elm, string Createdby)
        //{
        //    string res = "";
        //    try
        //    {

        //        using (SqlConnection con = new SqlConnection(connection()))
        //        {
        //            using (SqlCommand cmd = new SqlCommand("Prc_Employee_LeaveMapping", con))
        //            {
        //                cmd.CommandType = CommandType.StoredProcedure;
        //                cmd.Parameters.AddWithValue("@id", elm.ID);
        //                cmd.Parameters.AddWithValue("@applicablefor", elm.applicablefor);
        //                cmd.Parameters.AddWithValue("@person_id", elm.employeeid);
        //                cmd.Parameters.AddWithValue("@gender", elm.gender);
        //                cmd.Parameters.AddWithValue("@leave_type_id", elm.allleavetype);
        //                cmd.Parameters.AddWithValue("@start_date", elm.startdate);
        //                cmd.Parameters.AddWithValue("@doj", elm.doj);
        //                cmd.Parameters.AddWithValue("@specific_date", elm.specificdate);
        //                cmd.Parameters.AddWithValue("@end_date", elm.enddate);
        //                cmd.Parameters.AddWithValue("@status", elm.status);
        //                cmd.Parameters.AddWithValue("@created_by", Createdby);
        //                SqlParameter oblogin = new SqlParameter();
        //                oblogin.ParameterName = "@message";
        //                oblogin.SqlDbType = SqlDbType.NVarChar;
        //                oblogin.Size = 50;
        //                oblogin.Direction = ParameterDirection.Output;
        //                cmd.Parameters.Add(oblogin);
        //                con.Open();
        //                cmd.ExecuteNonQuery();
        //                res = Convert.ToString(oblogin.Value);
        //                con.Close();
        //            }
        //        }
        //        return res;
        //    }
        //    catch (Exception ex)
        //    {
        //        return ex.Message;

        //    }

        //}

        //public EmployeeLeaveMapping EditEmployeeLeaveMapping(string id)
        //{


        //    EmployeeLeaveMapping employeeleavemapping = new EmployeeLeaveMapping();
        //    using (SqlConnection con = new SqlConnection(connection()))
        //    {
        //        using (SqlCommand cmd = new SqlCommand("Prc_EditEmployeeLeaveMapping", con))
        //        {
        //            cmd.CommandType = CommandType.StoredProcedure;
        //            cmd.Parameters.AddWithValue("@id", id);
        //            con.Open();
        //            SqlDataReader dr = cmd.ExecuteReader();
        //            if (dr.Read())
        //            {
                      
        //                employeeleavemapping.ID = Convert.ToInt32(dr["LEAVEMAPPING_ID"].ToString());
        //                employeeleavemapping.applicablefor = dr["APPLICABLEFOR"].ToString();
        //                employeeleavemapping.employeeid = dr["PERSON_ID"].ToString();
                       
        //                employeeleavemapping.gender = dr["GENDER"].ToString();
        //                employeeleavemapping.allleavetype = dr["LEAVE_TYPE_ID"].ToString();
        //                employeeleavemapping.doj = Convert.ToInt32  ( dr["DOJ"].ToString());
        //                employeeleavemapping.specificdate = Convert.ToInt32( dr["SPECIFIC_DATE"].ToString());
        //                employeeleavemapping.startdate = dr["START_DATE"].ToString();
        //                employeeleavemapping.enddate = dr["END_DATE"].ToString();
        //                employeeleavemapping.status = dr["STATUS"].ToString();
        //                employeeleavemapping.data_of_joining= dr["DATE_OF_JOINING"].ToString();


        //            }

        //            con.Close();
        //        }
        //    }
        //    return employeeleavemapping;
        //}

        //public List<StoreLocationMapping> GetStoreLocationMapping()
        //{
        //    List<StoreLocationMapping> StoreLocationMappingli = new List<StoreLocationMapping>();

        //    using (SqlConnection con = new SqlConnection(connection()))
        //    {
        //        using (SqlCommand cmd = new SqlCommand("Prc_StoreLocationMappingList", con))
        //        {
        //            cmd.CommandType = CommandType.StoredProcedure;
        //            con.Open();
        //            SqlDataReader dr = cmd.ExecuteReader();
        //            while (dr.Read())
        //            {
        //                StoreLocationMapping storelocationmapping = new StoreLocationMapping();
        //                storelocationmapping.ID = Convert.ToInt32(dr["STORE_LOCATION_ID"].ToString());
        //                storelocationmapping.storename = dr["storename"].ToString();
        //                storelocationmapping.StoreLocation = dr["StoreLocation"].ToString();
        //                storelocationmapping.BUName = dr["BUName"].ToString();
        //                storelocationmapping.radius = Convert.ToDouble(dr["RADIUS"].ToString());
        //                storelocationmapping.storelatitude = Convert.ToDouble(dr["LATITUDE"].ToString());
        //                storelocationmapping.storelongitude = Convert.ToDouble(dr["LONGITUDE"].ToString());
        //                storelocationmapping.status = dr["STATUS"].ToString();
        //                StoreLocationMappingli.Add(storelocationmapping);
        //            }

        //            con.Close();
        //        }
        //    }
        //    return StoreLocationMappingli;



        //}
        public List<EModel> GetEmployeeList()
        {
            List<EModel> employeeli = new List<EModel>();
            using (SqlConnection con = new SqlConnection(connection()))
            {
                using (SqlCommand cmd = new SqlCommand("Prc_GetEmployeeList", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    con.Open();
                    SqlDataReader dr = cmd.ExecuteReader();
                    while (dr.Read())
                    {
                        EModel employee = new EModel();
                        employee.Empcode = dr["EMPLOYEE_CODE"].ToString();
                        employee.Name = dr["FULL_NAME"].ToString();
                        employee.FirstName = dr["FIRST_NAME"].ToString();
                        employee.Dob = dr["DOB"].ToString();
                        employee.Doj = dr["DOJ"].ToString();
                        //employee.DepartmentID = dr["DEPARTMENTID"].ToString();
                        //employee.DesignationID = dr["DESIGNATIONID"].ToString();

                        employee.Department_Name = dr["DEPARTMENT_NAME"].ToString();
                        employee.Designation_Name = dr["DESIGNATION_NAME"].ToString();
                        employee.Section_Name = dr["SECTION_NAME"].ToString();
                        employee.Location = dr["LOCATION"].ToString();
                        employee.Email = dr["EMAIL"].ToString();
                        employee.Status = dr["STATUS"].ToString();
                        employee.Person_id = Convert.ToInt32(dr["PERSON_ID"].ToString());
                        employee.Sr_No = Convert.ToInt32(dr["SR_NO"].ToString());

                        employee.Profile_Img = dr["PROFILE_IMG"].ToString();
                        employeeli.Add(employee);
                    }
                    con.Close();
                }
            }
            return employeeli;
        }
        public List<EModel> GetMyTeamEmployee(int emp_id)
        {
            List<EModel> employeeli = new List<EModel>();
            using (SqlConnection con = new SqlConnection(connection()))
            {
                using (SqlCommand cmd = new SqlCommand("Prc_GetMyTeamEmployee", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@emp_id", emp_id);
                    con.Open();
                    SqlDataReader dr = cmd.ExecuteReader();
                    while (dr.Read())
                    {
                        EModel employee = new EModel();
                        employee.Empcode = dr["EMPLOYEE_CODE"].ToString();
                        employee.Name = dr["FULL_NAME"].ToString();
                        employee.FirstName = dr["FIRST_NAME"].ToString();
                        employee.Dob = dr["DOB"].ToString();
                        employee.Doj = dr["DOJ"].ToString();
                        employee.DepartmentID = dr["DEPARTMENTID"].ToString();
                        employee.DesignationID = dr["DESIGNATIONID"].ToString();
                        employee.Location = dr["LOCATION"].ToString();
                        employee.Email = dr["EMAIL"].ToString();
                        employee.Status = dr["STATUS"].ToString();
                        employee.Person_id = Convert.ToInt32(dr["PERSON_ID"].ToString());
                        employee.Sr_No = Convert.ToInt32(dr["SR_NO"].ToString());
                        employee.Profile_Img = dr["PROFILE_IMG"].ToString();
                        employeeli.Add(employee);
                    }
                    con.Close();
                }
            }
            return employeeli;
        }

        public EModel GetEmployeeDetails(string id)
        {
            EModel employee = new EModel();

            using (SqlConnection con = new SqlConnection(connection()))
            {
                using (SqlCommand cmd = new SqlCommand("Prc_GetEmployeeDetail", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@id", id);
                    con.Open();
                    SqlDataReader dr = cmd.ExecuteReader();
                    while (dr.Read())
                    {   employee.ROLE_NAME = dr["ROLE_NAME"].ToString();
                        employee.Empcode = dr["EMPLOYEE_CODE"].ToString();
                        employee.Name = dr["FULL_NAME"].ToString();
                        employee.Dob = dr["DOB"].ToString();
                        employee.Doj = dr["DOJ"].ToString();
                        //employee.DepartmentID = dr["DEPARTMENT_NAME"].ToString();
                        //employee.DesignationID = dr["DESIGNATION_NAME"].ToString();
                        employee.Department_Name = dr["DEPARTMENT_NAME"].ToString();
                        employee.Designation_Name = dr["DESIGNATION_NAME"].ToString();
                        employee.Section_Name = dr["SECTION_NAME"].ToString();
                        employee.Location = dr["LOCATION"].ToString();
                        employee.Email = dr["EMAIL"].ToString();
                        employee.Gender= dr["GENDER"].ToString();
                        employee.Person_id = Convert.ToInt32(dr["PERSON_ID"].ToString());
                        employee.Grade = dr["GRADE_NAME"].ToString();
                        employee.Grade_id =Convert.ToInt32( dr["GRADE_ID"].ToString());
                        employee.Manager_Name= dr["MANAGER_NAME"].ToString();
                        employee.Employment_Type= dr["EMPLOYMENT_TYPE"].ToString();
                        employee.Employment_Type = dr["EMPLOYMENT_TYPE"].ToString();
                        employee.basicpay = dr["BASIC_PAY"].ToString();
                        employee.User_Code = dr["USER_CODE"].ToString();
                        employee.Profile_Img = dr["PROFILE_IMG"].ToString();
                        

                    }
                    con.Close();
                }
            }
            return employee;
        }

        public List<SelectListItem> DropdownEmployee() {
            List<SelectListItem> employee = new List<SelectListItem>();
            using (SqlConnection con = new SqlConnection(connection()))
            {
                using (SqlCommand cmd = new SqlCommand("Prc_GetEmployeeList", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    con.Open();
                    SqlDataReader dr = cmd.ExecuteReader();
                    while (dr.Read())
                    {
                        employee.Add(new SelectListItem
                        {
                            Text = dr["FULL_NAME"].ToString(),
                            Value = dr["PERSON_ID"].ToString()
                        });
                    }
                }
                employee.Insert(0, new SelectListItem { Value = "0", Text = "Select" });
            }
            return employee;
        }
        public DataSet GetEmployeeLeaveMapping( string applicationfor,string employeeid, string genderid)
        {
            DataSet ds = new DataSet();
            using (SqlConnection con = new SqlConnection(connection()))
            {
                using (SqlCommand cmd = new SqlCommand("Prc_GetEmployee_LeaveMappingList", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@applicationfor", applicationfor);
                    cmd.Parameters.AddWithValue("@employeeid", employeeid);
                    cmd.Parameters.AddWithValue("@gender", genderid);
                    con.Open();
                    using (SqlDataAdapter adp = new SqlDataAdapter(cmd))
                    {
                        adp.Fill(ds);
                    }
                    con.Close();
                }
            }
            return ds;
        }

        public DataTable SearchDirectory(string Search_Letter)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(connection()))
                {
                    using (SqlCommand cmd = new SqlCommand("Prc_SearchDirectory", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@search_letter", Search_Letter);
                        con.Open();
                        using (SqlDataAdapter adp = new SqlDataAdapter(cmd))
                        {
                            adp.Fill(dt);
                        }
                        con.Close();
                    }
                }
            }
            catch (Exception ex)
            {

            }
            return dt;
        }


        public void addprofile_image(string emp_id, string img_name)
        {
            using (SqlConnection con = new SqlConnection(connection()))
            {
                using (SqlCommand cmd = new SqlCommand("PRC_ADD_PROFILE_IMG", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@emp_id", emp_id);
                    cmd.Parameters.AddWithValue("@img_name", img_name);
                    con.Open();
                    cmd.ExecuteNonQuery();
                    con.Close();

                }
            }
        }
    }
}
 