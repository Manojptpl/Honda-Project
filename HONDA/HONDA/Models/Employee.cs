using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HONDA.Models
{
    public class EModel
    {
        public int Sr_No { set; get; }
        public int ID { get; set; }
        public int Person_id { get; set; }
        public string Empcode { get; set; }
        public string Name { get; set; }
        public string FirstName { get; set; }
        public string Dob { get; set; }
        public string Doj { get; set; }
        public string DepartmentID { get; set; }
        public string Department_Name { get; set; }
        public string DesignationID { get; set; }
        public string Designation_Name { get; set; }
        public string SectionID { get; set; }
        public string Section_Name { get; set; }
        public string Location { get; set; }
        public string Email { get; set; }
        public string Status { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
        public string Gender { get; set; }
        public string ImgUrl { get; set; }
        public int ManagerID { get; set; }
        public string Manager_Name { get; set; }
        public string Grade { set; get; }
        public int Grade_id { set; get; }
        public string Employment_Type { get; set; }
        public string SuccessMsg { set; get; }
        public string ErrorMsg { set; get; }
        public string basicpay { set; get; }
        public string User_Code { set; get; }
        public string Profile_Img { get; set; }
        public string ROLE_NAME { set; get; }
    }
    public class SystemUser
    {
        public int user_id { set; get; }
        public int Sr_No { set; get; }
        public string Letter { set; get; }
        public string created_date { set; get; }
        public int emp_id { set; get; }
        public string emp_code { set; get; }
        public string Showemp_code { set; get; }
        public string first_name { set; get; }
        public string middle_name { set; get; }
        public string last_name { set; get; }
        public string full_name { set; get; }
        public string emp_name { set; get; }
        public string sex { set; get; }
        public string email { set; get; }
        public string status { set; get; }
        public int store_id { set; get; }
        public string store_name { set; get; }
        public string roles { set; get; }
        public int role_id { set; get; }
        public string role_name { set; get; }
        public string password { set; get; }
        public string confirm_password { set; get; }
        public int is_navone { set; get; }
        public DateTime? start_date { set; get; }
        public DateTime? end_date { set; get; }
        public string Showstart_date { set; get; }
        public string Showend_date { set; get; }
        public int chkmanagement { set; get; }
        public int chkhr { set; get; }
        public int chkfh { set; get; }
        public string success_msg { set; get; }
        public string error_msg { set; get; }
        public List<UserPermissionsStatus> userPermissions { set; get; }
    }

}