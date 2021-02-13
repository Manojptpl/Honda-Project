using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data;
using HONDA.CommanFilter;
using System.Net.Mail;
using System.Net;
using System.Web.Script.Serialization;
using System.IO;
using System.Text;
using System.Configuration;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Threading.Tasks;
using HONDA.Database_Access_Layer;
using HONDA.Models;
namespace HONDA.Controllers
{
    public class homeController : Controller
    {
        DALEmployee dllemployee = new DALEmployee();
        db mydb = new db();
        #region DashBoard        [SessionFilter]
        public ActionResult Index()
        {
            //ViewBag.mgrshow = Convert.ToInt32(Session["role_id"].ToString()) == 2 || Convert.ToInt32(Session["role_id"].ToString()) == 4 ? "display:block" : "display:none";

            string CurrentBaseUrl = "";
            string Scheme = Request.Url.Scheme;
            if (string.IsNullOrEmpty(Scheme))
            {
                Scheme = "http";
            }
            if (Request.Url.Port > 0 && Request.Url.Host.Contains("localhost"))
            {
                CurrentBaseUrl = Scheme + "://" + Request.Url.Host + ":" + Request.Url.Port;
            }
            else
            {
                CurrentBaseUrl = Scheme + "://" + Request.Url.Host + ":" + Request.Url.Port;
            }
            Int32 empId = Convert.ToInt32(Session["emp_id"]);
            DataSet menuByPermission = mydb.getMenuByPermission(empId);
            string menuString = "";// "<li  id=\"myli\"><a href=\"" + CurrentBaseUrl + "/home/index\">Home</a></li>";
            int rowcount = 1;

            if (menuByPermission.Tables[0].Rows.Count > 0)
            {
                string moduleName = "";
                DataRow[] rows = menuByPermission.Tables[0].Select("MODULE_NAME <> 'Settings'");
                foreach (DataRow dr in rows.OrderBy(p => p["MORDER_NO"]).ThenBy(p => p["PORDER_NO"]))
                {
                    //If module is different then insert new row in table.
                    //if (moduleName == "" || moduleName != dr["MODULE_NAME"].ToString())
                    //{
                    //    if (moduleName != "")
                    //    {
                    //        menuString += "</ul></li>";
                    //    }
                    //    menuString += "<li class=\"submenu\">";
                    //    menuString += "<a href=\"javascript: void(0); \"><span>" + dr["MODULE_NAME"].ToString() + "</span> <span class=\"menu-arrow\"></span></a>";
                    //    //menuString += "<ul class=\"list-unstyled\" style=\"display:none;\">";
                    //    //moduleName = dr["MODULE_NAME"].ToString();
                    //}

                    menuString += "<li><a href=\"" + CurrentBaseUrl + "" + dr["PAGE_URL"].ToString() + "\">" + dr["PAGE_NAME"].ToString() + "</a></li>";

                    //if (rowcount == rows.Count())
                    //{
                    //    menuString += "</ul></li>";
                    //}

                    rowcount++;
                }

                //Add submenu for settings
                //string settingMenu = "";
                //DataRow[] dsSubMenu = menuByPermission.Tables[0].Select("MODULE_NAME = 'Settings'");
                //if (dsSubMenu.Count() > 0)
                //{
                //    //Add setting menu in main page
                //    menuString += "<li><a href=\"" + CurrentBaseUrl + "" + dsSubMenu[0]["PAGE_URL"] + "\">Settings</a></li>";
                //}

                //foreach (DataRow dr in dsSubMenu.OrderBy(p => p["MORDER_NO"]).ThenBy(p => p["PORDER_NO"]))
                //{
                //    string activeStatus = settingMenu == "" ? " class=\"active\"" : "";
                //    settingMenu += "<li><a href=\"" + CurrentBaseUrl + "" + dr["PAGE_URL"].ToString() + "\">" + dr["PAGE_NAME"].ToString() + "</a></li>";
                //    //settingMenu +="<li id=\"myli\"><a href=\"../settings/themesettings\">Theme Settings</a></li>";
                //}

               // Session["SettingMenus"] = settingMenu;
                Session["Menus"] = menuString;
                //DataSet ds = mydb.BindList();
                //if (ds.Tables[25].Rows.Count > 0)
                //{

                //    Session["favicon"] = "" + ds.Tables[25].Rows[0]["FAVICON"].ToString();
                //    Session["logo"] = ds.Tables[25].Rows[0]["LOGO"].ToString();
                //    Session["comp_name"] = ds.Tables[25].Rows[0]["COMPANY_NAME"].ToString();
                //}
                //else
                //{
                //    Session["favicon"] = "";
                //    Session["logo"] = "";
                //    Session["comp_name"] = "";
                //}
                return View();
            }
            else
            {
                return RedirectToAction("Logout");
            }
            Session["SettingMenus"] = "";
            Session["Menus"] = "";
            return View();
        }
        [SessionFilter]
        [HttpPost]
        public JsonResult GetEmployeeDashBoard(int Month, int Year)
        {
            
            var holiday = ""; var balance = ""; var approve_leave = ""; var Inprogress_leave = "";
            var employee = ""; var notification = ""; var leave_all_type = ""; var Attendence_detail = "";
            var Employment_type = ""; ;
            try
            {

          
               int Emp_id = Convert.ToInt32(Session["emp_id"].ToString());
                var ds =  mydb.GetEmpDashBoard(Emp_id, Month, Year);
                
                holiday = DataTableToJSONWithJSONNet(ds.Tables[0]) ;               
             
                balance = DataTableToJSONWithJSONNet(ds.Tables[1]);
                approve_leave = DataTableToJSONWithJSONNet(ds.Tables[2]);
                Inprogress_leave = DataTableToJSONWithJSONNet(ds.Tables[3]);
                employee = DataTableToJSONWithJSONNet(ds.Tables[4]);           
                notification = DataTableToJSONWithJSONNet(ds.Tables[6]);
                leave_all_type = DataTableToJSONWithJSONNet(ds.Tables[7]);
                Attendence_detail = DataTableToJSONWithJSONNet(ds.Tables[8]);
                Employment_type = DataTableToJSONWithJSONNet(ds.Tables[9]);


            }
            catch
            {
                RedirectToAction("Login", "Home");
            }
            var result = new { holiday, balance, approve_leave, Inprogress_leave, employee, notification, leave_all_type, Attendence_detail, Employment_type };
            return Json(result, JsonRequestBehavior.AllowGet);
        }

       
        [SessionFilter]
        [HttpPost]
        public JsonResult get_employee_leave_bymgr(int emp_id)
        {
            try
            {
                var ds = mydb.get_empleave_detail_mgrby(emp_id);
                var leave_detail = DataTableToJSONWithJSONNet(ds.Tables[0]);
                return Json(leave_detail, JsonRequestBehavior.AllowGet);
            }
            catch 
            {
                return Json("error", JsonRequestBehavior.AllowGet);

            }

        }
        [SessionFilter]
        [HttpPost]
        public JsonResult get_employee_attendetail_bymgr(int emp_id)
        {
            try
            {
                var ds = mydb.get_empatten_detail_mgrby(emp_id);
                var atten_detail = DataTableToJSONWithJSONNet(ds.Tables[0]);
                return Json(atten_detail, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json("error", JsonRequestBehavior.AllowGet);

            }

        }

        [HttpPost]
        public JsonResult get_employee_bymgr(int emp_id)
        {
            try
            {
                var ds = mydb.get_emp_detail(emp_id);
                var emp_detail = DataTableToJSONWithJSONNet(ds.Tables[0]);
                return Json(emp_detail, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json("error", JsonRequestBehavior.AllowGet);

            }

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

        #endregion

        //#region Change Password
        //public ActionResult changepassword()
        //{
        //    return View();
        //}
    
        //[HttpPost]
        //public JsonResult changepassword(string old_pwd,string new_pwd,int emp_id)
        //{
        //    string ErrorMsg = "";
        //    try
        //    {           
        //      ErrorMsg = mydb.changepassword(old_pwd, new_pwd, emp_id);
            
        //    return Json(new { ErrorMsg }, JsonRequestBehavior.AllowGet);

        //    }
        //    catch (Exception ex)
        //    {
        //        ErrorMsg = "Error," + ex.ToString();
        //        return Json(new {  ErrorMsg }, JsonRequestBehavior.AllowGet);
        //    }

        //}
        //#endregion
        //#region Forgot Password
        //public ActionResult forgetpassword()
        //{
        //    return View();
        //}
        //[HttpPost]
        //public JsonResult forgetpassword(string email_id)
        //{
        //    string SuccessMsg = "", ErrorMsg = "";
        //    string msg = mydb.forgotpassword(email_id);
        //    string[] response = msg.Split(',');
        //    if (response[0] == "success")
        //    {
        //        SuccessMsg = response[1];
        //      SendMail_ForgotPassword(email_id, response[1],response[2].ToString());
        //    }
        //    else {
        //        ErrorMsg = msg;
        //    }
        //    return Json(new { SuccessMsg, ErrorMsg }, JsonRequestBehavior.AllowGet);
        //}

        //public string SendMail_ForgotPassword(string email, string emp_name, string emp_id)
        //{
        //    using (MailMessage mm = new MailMessage("ots.support@prudencesoftech.net", email))
        //    {
        //        mm.Subject = "Reset Password";
        //        string mail_body = "<html><body>Hi " + emp_name + "," +
        //                          "<p>You recently asked to reset your ots account password.</p>" +
        //                          "<p><a href = 'http://otstest.prudencesoftech.in/ResetPassword/ResetPassword.html?id=" + emp_id + " title = 'Online Time Sheet system' target = _blank> OTS system </a> to view the details.<br/></p>" +
        //                          "<p>Thank You,<br/>Human Resources<br/>Prudence Technology Pvt. Ltd.</p></body></html>";
        //        mm.Body = mail_body;
        //        mm.IsBodyHtml = true;
        //        using (SmtpClient smtp = new SmtpClient())
        //        {
        //            smtp.Host = "mail.prudencesoftech.net";
        //            smtp.EnableSsl = false;
        //            NetworkCredential NetworkCred = new NetworkCredential("ots.support@prudencesoftech.net", "Tc#wo6s3eyYutbdaC5PP.BxC");
        //            smtp.UseDefaultCredentials = true;
        //            smtp.Credentials = NetworkCred;
        //            smtp.Port = 587;
        //            try
        //            {
        //                smtp.Send(mm);
        //                ViewBag.Message = "Email Sent.";
        //            }
        //            catch (Exception ex)
        //            {
        //                ViewBag.Message = ex.Message;
        //            }

        //        }
        //    }
        //    return ViewBag.Message;
        //}
        //#endregion

        #region Profile

        [SessionFilter]
        public ActionResult profile()
        {
            EModel data = new EModel();
            if (Session["emp_id"]!=null)
            {
             
                data = dllemployee.GetEmployeeDetails(Session["emp_id"].ToString());
            }
          

             
            return View(data);
        }
    

        [HttpPost]
        public ActionResult UploadFiles()
        {
            List<string> FileName = new List<string>();
            var file_name = "";
            string randomstr = RandomString(8);
            Session["RandomNumber"] = randomstr;
            HttpFileCollectionBase files = Request.Files;
            for (int i = 0; i < files.Count; i++)
            {
                HttpPostedFileBase file = files[i];
                string fname;
                string[] extn;
                string extension = "";

                string BrowserDetail = Request.UserAgent;
                string[] newstr = BrowserDetail.Split(new char[] { '/' });
                BrowserDetail = newstr[newstr.Length - 2];
                newstr = BrowserDetail.Split(' ');
                // Checking for Microsoft Edge  
                if (newstr[1] == "Edge")
                {
                    string[] testfiles = file.FileName.Split(new char[] { '\\' });
                    fname = testfiles[testfiles.Length - 1];
                    extn = fname.Split('.');
                    fname = extn[0];
                    extension = extn[1];
                }
                // Checking for Internet Explorer  
                else if (Request.Browser.Browser.ToUpper() == "IE" || Request.Browser.Browser.ToUpper() == "INTERNETEXPLORER")
                {
                    string[] testfiles = file.FileName.Split(new char[] { '\\' });
                    fname = testfiles[testfiles.Length - 1];
                    extn = fname.Split('.');
                    fname = extn[0];
                    extension = extn[1];
                }
                else
                {
                    fname = file.FileName;
                    extn = fname.Split('.');
                    fname = extn[0];
                    extension = extn[1];
                }
                file_name = fname.Replace(',',' ') + "_" + randomstr + "." + extension;

                FileName.Add(file_name);
                // Get the complete folder path and store the file inside it.    
                fname = Path.Combine(Server.MapPath("~/ThemeImg/"), fname.Replace(',', ' ') + "_" + randomstr + "." + extension);
                file.SaveAs(fname);
                dllemployee.addprofile_image(Session["emp_id"].ToString(), file_name);

                if (Session["profile_image"].ToString() != null)
                {
                    string oldimg = Session["profile_image"].ToString();
                    if (Session["profile_image"].ToString() != "/assets/img/user.jpg")
                    {
                        DeleteFile(oldimg.Substring(10));
                    }
                }
                Session["profile_image"] = null;
                Session["profile_image"] = "/ThemeImg/" + FileName[0];


            }
            return Json(FileName, JsonRequestBehavior.AllowGet);
        }

        private static string RandomString(int length)
        {
            Random rng = new Random();
            const string pool = "abcdefghijklmnopqrstuvwxyz0123456789";
            var builder = new StringBuilder();

            for (var i = 0; i < length; i++)
            {
                var c = pool[rng.Next(0, pool.Length)];
                builder.Append(c);
            }

            return builder.ToString();
        }


        private string DeleteFile(string Filename)
        {
            string FileName = Filename;
            if (FileName != "")
            {
                try
                {
                    //HttpFileCollectionBase files = Request.Files;
                    //for (int i = 0; i < files.Count; i++)
                    //{
                    //    HttpPostedFileBase file = files[i];
                    //    string fname;
                    //    string[] extn;
                    //    string extension = "";
                    //    // Checking for Internet Explorer  
                    //    if (Request.Browser.Browser.ToUpper() == "IE" || Request.Browser.Browser.ToUpper() == "INTERNETEXPLORER")
                    //    {
                    //        string[] testfiles = file.FileName.Split(new char[] { '\\' });
                    //        fname = testfiles[testfiles.Length - 1];
                    //    }
                    //    else
                    //    {
                    //        fname = file.FileName;
                    //        extn = fname.Split('.');
                    //        fname = extn[0];
                    //        extension = extn[1];
                    //    }
                    //    FileName = fname + "_" + Session["RandomNumber"].ToString() + "." + extension;
                    //    // Get the complete folder path and store the file inside it.  
                    //    fname = Path.Combine(Server.MapPath("~/UploadFile/"), fname + "_" + Session["RandomNumber"].ToString() + "." + extension);

                    //    if (System.IO.File.Exists(fname))
                    //    {
                    //        System.IO.File.Delete(fname);
                    //    }
                    //}
                    //FileName = fname + "_" + Session["RandomNumber"].ToString() + "." + extension;
                    // Get the complete folder path and store the file inside it.  
                    FileName = Path.Combine(Server.MapPath("~/ThemeImg/"), FileName);

                    if (System.IO.File.Exists(FileName))
                    {
                        System.IO.File.Delete(FileName);
                    }
                    return ""; //Json(Filename + " Deleted Successfully.", JsonRequestBehavior.AllowGet);
                }
                catch (Exception ex)
                {
                    return "";// Json(ex.Message, JsonRequestBehavior.AllowGet);
                }
            }
            return "";// Json("", JsonRequestBehavior.AllowGet);
        }

        #endregion
        #region Login/Logout
        public ActionResult login()
        {
          
            return View();
        }
        [HttpPost]
        public JsonResult Login(string user_name, string password)
        {

            ////string hostName = Dns.GetHostName(); // Retrive the Name of HOST  
            ////Console.WriteLine(hostName);
            ////// Get the IP  
            ////string myIP = Dns.GetHostByName(hostName).AddressList[0].ToString();

            //string VisitorsIPAddr = string.Empty;
            //if (HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"] != null)
            //{
            //    VisitorsIPAddr = HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"].ToString();
            //}
            //else if (HttpContext.Current.Request.UserHostAddress.Length != 0)
            //{
            //    VisitorsIPAddr = HttpContext.Current.Request.UserHostAddress;
            //}


            string msg = "";
             string SuccessMsg = "", ErrorMsg = "";
            try
            {


                //var s1 = new WebClient();
                //var byteArray = Encoding.ASCII.GetBytes("TASHI:TASHI@15");
                //string base64 = Convert.ToBase64String(byteArray);
                //string authorization = String.Concat("Basic ", base64);
                //s1.Headers.Add("authorization", authorization);
                //s1.Headers.Add("X-ApiKey", "MyRandomApiKeyValue");
                //var a = s1.DownloadString("http://103.205.66.149:81/api/master/OTS_CHECK");
                //var R = Newtonsoft.Json.JsonConvert.DeserializeObject(a);
                //if (R.ToString() == "Active")
                if (true)
                {


                    msg = mydb.checklogin(user_name, password);
                    string[] response = msg.Split(',');
                    if (response[0] != "error")
                    {
                        Session["emp_name"] = response[0];
                        Session["emp_id"] = response[1];
                        Session["role_id"] = response[2];
                        Session["manager_id"] = response[3];
                        Session["role_name"] = response[4];
                        Session["profile_image"] = response[5];
                        Session["Attendance_Submit_right"] = response[6];
                        msg = "";

                    }
                    else
                    {
                        msg = response[1];
                    }

                }
                else
                {

                    msg = "Error connecting to server, Please contact Admin.";
                }


                //if (user_name == "Admin" && password == "Admin")
                //{

                //    Session["emp_name"] = "Admin";
                //    Session["emp_id"] = 1;
                //    Session["role_id"] = 2;
                //    Session["manager_id"] = 1;
                //    Session["role_name"] = 1;
                //    Session["profile_image"] = "";

                //}
                //else
                //{

                //    msg = "";
                //}

            }
            catch (Exception ex)
            {
                msg = "Invalid Credentials";

                   msg = ex.Message;
            }
            return Json(msg, JsonRequestBehavior.AllowGet);

        }
        public ActionResult logout()
        {
            Session.Abandon();
            Session.Clear();
            Session.RemoveAll();
            return RedirectToAction("Login", "home", new { area = "" });
        }
        #endregion

        ////#region Reset Password
        ////[SessionFilter]
        ////public ActionResult ResetPassword()
        ////{
        ////    return View();
        ////}

        ////#endregion

        #region Reset Password
        //[SessionFilter]
        //public ActionResult ResetPassword()
        //{
        //    return View();
        //}

        public ActionResult ResetPassword(string id)
        {
            try
            {
             //   Guid hexaId = new Guid(id);
                int expireTime = Convert.ToInt32(ConfigurationManager.AppSettings["FORGOT_LINK_EXPIRE"].ToString());
                string msg = mydb.validateresetpassword(id, expireTime);
                ViewBag.InvalidError = "";
                string[] response = msg.Split(',');

                if (response[0] == "success")
                {
                    ViewBag.userId = response[1];
                }
                else
                {
                    ViewBag.InvalidError = response[0];
                }
            }
            catch (Exception ex)
            {
                ViewBag.InvalidError = "Something wrong, please try again.";
            }
            return View();
        }

        [HttpPost]
        public JsonResult resetpassword(string new_pwd, int emp_id)
        {
            string SuccessMsg = "", ErrorMsg = "";
            int expireTime = Convert.ToInt32(ConfigurationManager.AppSettings["FORGOT_LINK_EXPIRE"].ToString());
            string msg = mydb.resetpassword(new_pwd, emp_id, expireTime);
            if (msg == "Success") { SuccessMsg = msg; }
            else { ErrorMsg = msg; }
            return Json(new { SuccessMsg, ErrorMsg }, JsonRequestBehavior.AllowGet);

        }
        #endregion

        #region Change Password
        [SessionFilter]
        public ActionResult changepassword()
        {
            return View();
        }
        [SessionFilter]
        [HttpPost]
        public JsonResult changepassword(string old_pwd, string new_pwd)
        {
            string SuccessMsg = "", ErrorMsg = "";
            try
            {
                string msg = mydb.changepassword(old_pwd, new_pwd, Convert.ToInt32(@Session["emp_id"].ToString()));
                string[] response = msg.Split(',');
                if (response[0] == "Success")
                {
                    string res = "Sent";// EmailService.Send_Email_ChangePassword(response[1], response[2]);
                    if (res == "Sent")
                    {
                        SuccessMsg = "Successfully Changed Password.";
                        //db.GenrateEmailLogs(response[1], "Changed Password", Convert.ToInt32(Session["emp_id"].ToString()), "Success", res,0);
                    }
                    else
                    {
                        ErrorMsg = "Successfully Changed Password.";
                        //db.GenrateEmailLogs(response[1], "Changed Password", Convert.ToInt32(Session["emp_id"].ToString()), "Fail", res,0);
                    }
                }
                else
                {
                    ErrorMsg = msg;
                }
                return Json(new { SuccessMsg, ErrorMsg }, JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
            {
                ErrorMsg = "Error," + ex.ToString();
                return Json(new { SuccessMsg, ErrorMsg }, JsonRequestBehavior.AllowGet);
            }

        }
        #endregion

        #region Forgot Password
        public ActionResult forgetpassword()
        {
            return View();
        }

        [HttpPost]
        public JsonResult forgetpassword(string email_id)
        {
            string SuccessMsg = "", ErrorMsg = "";
            string msg = mydb.forgotpassword(email_id);
            string[] response = msg.Split(',');
            if (response[0] == "Success")
            {
                //response[1]; //User Name
                //response[2]; //Link url
                string res = EmailService.Send_Email_ForgetPassword(email_id, response[1], response[2].ToString());
                if (res == "Sent")
                {
                    SuccessMsg = "Reset link has been sent to your email.";

                }
                else
                {
                    ErrorMsg = res;
                }
            }
            else
            {
                ErrorMsg = response[1];
            }
            return Json(new { SuccessMsg, ErrorMsg }, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Time Sheet

        public ActionResult TimesheetCategory()
        {
            return View();
        }


        // GET: Admin
        public ActionResult Settings()
        {
            return View();
        }
        #endregion








    }
}


