using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using HONDA.CommanFilter;
using HONDA.Models;
using System.Data;
using Newtonsoft.Json;
using HONDA.Database_Access_Layer;
using HONDA.Models;
using System.Text;
using System.Web.Script.Serialization;
using System.IO;

namespace HONDA.Controllers
{
    [SessionFilter]
    public class settingsController : Controller
    {
        // GET: settings
        db dblayer = new db();
        commonModel cm = new commonModel();
        successFailureModel successStatus = new successFailureModel();
        DALWorkStructure dllworkstructure = new DALWorkStructure();
       // DALLeave dllleave = new DALLeave();
        public ActionResult companysettings()
        {
            Page_Permission_Detail info = new Page_Permission_Detail();
            info = dblayer.Get_Page_UserPermission(Convert.ToInt32(Session["emp_id"].ToString()), 28);
            ViewBag.event_permission = info;
            List<SelectListItem> _Months = new List<SelectListItem>()
            {
                new SelectListItem{ Text="Select",Value="0"},
                new SelectListItem{ Text="Jan",Value="01"},
                new SelectListItem{ Text="Feb",Value="02"},
                new SelectListItem{ Text="Mar",Value="03"},
                new SelectListItem{ Text="Apr",Value="04"},
                new SelectListItem{ Text="May",Value="05"},
                new SelectListItem{ Text="Jun",Value="06"},
               new SelectListItem{ Text="Jul",Value="07"},
               new SelectListItem{ Text="Aug",Value="08"},
               new SelectListItem{ Text="Sep",Value="09"},
               new SelectListItem{ Text="Oct",Value="10"},
              new SelectListItem{ Text="Nov",Value="11"},
              new SelectListItem{ Text="Dec",Value="12"},

            };

            ViewBag.Months = _Months;
            ViewBag.Companylist = dllworkstructure.GetCompany();
            ViewBag.countrylist = dllworkstructure.GetCountry();
            ViewBag.zonelist = dllworkstructure.GetZone();
            ViewBag.Statelist = dllworkstructure.GetState();
            ViewBag.Citylist = dllworkstructure.GetStore();
            BusinessUnit businessunit = new BusinessUnit();
            businessunit = dllworkstructure.GetBusinessUnit();
            return View(businessunit);
        }

        public ActionResult localization()
        {
            Page_Permission_Detail info = new Page_Permission_Detail();
            info = dblayer.Get_Page_UserPermission(Convert.ToInt32(Session["emp_id"].ToString()), 29);
            ViewBag.event_permission = info;


            return View();
        }
       
        public ActionResult rolesandpermissions(int Id = 0)
        {
            Page_Permission_Detail info = new Page_Permission_Detail();
            info = dblayer.Get_Page_UserPermission(Convert.ToInt32(Session["emp_id"].ToString()), 32);
            ViewBag.event_permission = info;

            DataTable dtroles = dblayer.getroles();
            //If id is not passed, then added first roles to active
            if (Id == 0 && dtroles.Rows.Count > 0)
            {
                Id = Convert.ToInt32(dtroles.Rows[0]["ROLE_ID"]);
            }
            ViewBag.roleId = Id;

            var roles_list = (from DataRow dr in dtroles.Rows
                              select new roleModel()
                              {
                                  ROLE_ID = Convert.ToInt32(dr["ROLE_ID"].ToString()),
                                  ROLE_NAME = dr["ROLE_NAME"].ToString(),
                                  ROLEACTIVEINDEX = (Id == Convert.ToInt32(dr["ROLE_ID"].ToString())) ? 1 : 0
                              }).ToList();
            ViewBag.roles = roles_list;

            DataTable dtmodules = dblayer.getmodules(Id);
            var modules_list = (from DataRow dr in dtmodules.Rows
                                select new moduleModel()
                                {
                                    MODULE_ID = Convert.ToInt32(dr["MODULE_ID"].ToString()),
                                    MODULE_NAME = dr["MODULE_NAME"].ToString(),
                                    ROLEACTIVEINDEX = Convert.ToInt32(dr["ISACTIVE"].ToString())
                                }).ToList();
            ViewBag.modules = modules_list;

            DataTable dtPagePermission = dblayer.getPagePermission(Id);

            #region Create table with columns

            DataTable dtfilterPagePermission = new DataTable();

            dtfilterPagePermission.Columns.Add("ROLE_PERMISSION_ID");
            dtfilterPagePermission.Columns.Add("ROLE_ID");
            dtfilterPagePermission.Columns.Add("PAGE_ID");

            dtfilterPagePermission.Columns.Add("PERMISSION_ID");
            dtfilterPagePermission.Columns.Add("MODULE_ID");
            dtfilterPagePermission.Columns.Add("STATUS");
            dtfilterPagePermission.Columns.Add("PAGE_NAME");
            dtfilterPagePermission.Columns.Add("MODULE_NAME");

            int noOfColumns = dtPagePermission.Columns.Count - 1;
            int noOfColumnsDynamic = 0;

            DataView view = new DataView(dtPagePermission);
            DataTable distinctValues = view.ToTable(true, "PERMISSION_NAME");
            foreach (DataRow pname in distinctValues.Rows)
            {
                dtfilterPagePermission.Columns.Add(pname["PERMISSION_NAME"].ToString());
                noOfColumnsDynamic += 1;
            }

            #endregion

            var pageName = "";
            int fillColumns = 0;
            DataRow row = dtfilterPagePermission.NewRow();

            foreach (DataRow pp in dtPagePermission.Rows)
            {

                if (pageName == "" || (pageName != pp["PAGE_NAME"].ToString()))
                {
                    pageName = pp["PAGE_NAME"].ToString();
                    fillColumns = 1;
                    row = dtfilterPagePermission.NewRow();

                    row["ROLE_PERMISSION_ID"] = pp["ROLE_PERMISSION_ID"].ToString();
                    row["ROLE_ID"] = pp["ROLE_ID"].ToString();
                    row["PAGE_ID"] = pp["PAGE_ID"].ToString();
                    row["PERMISSION_ID"] = pp["PERMISSION_ID"].ToString();
                    row["MODULE_ID"] = pp["MODULE_ID"].ToString();
                    row["STATUS"] = pp["STATUS"].ToString();
                    row["PAGE_NAME"] = pp["PAGE_NAME"].ToString();
                    row["MODULE_NAME"] = pp["MODULE_NAME"].ToString();
                    row[noOfColumns] = pp["ROLE_PERMISSION_ID"] + "~" + pp["STATUS"].ToString();
                }
                else if (pageName == pp["PAGE_NAME"].ToString())
                {
                    row[noOfColumns + fillColumns] = pp["ROLE_PERMISSION_ID"] + "~" + pp["STATUS"].ToString();
                    fillColumns += 1;
                }
                //If original value including dynamic column 
                if (noOfColumnsDynamic == fillColumns)
                {
                    dtfilterPagePermission.Rows.Add(row);
                }
            }


            ViewBag.pagePermission = dtfilterPagePermission;
            ViewBag.fixedColumns = noOfColumns;

            return View();
        }
        public ActionResult emailsettings()
        {
            return View();
        }

        public ActionResult notificationssettings()
        { return View(); }

        public ActionResult systemuser()
        {
            Page_Permission_Detail info = new Page_Permission_Detail();
            info = dblayer.Get_Page_UserPermission(Convert.ToInt32(Session["emp_id"].ToString()), 31);
            ViewBag.event_permission = info;
            List<SystemUser> li_users = new List<SystemUser>();
            try
            {
                DataSet ds = dblayer.BindList();

                ViewBag.role = ds.Tables[2];
                ViewBag.emp = ds.Tables[7];
                ViewBag.SYSTEM_USER = ds.Tables[8];
                Session["Employee"] = ViewBag.emp;

                DALWorkStructure dalworkstructure = new DALWorkStructure();
                ViewBag.storenameli = dalworkstructure.GetStore();

                List<SelectListItem> li_role = new List<SelectListItem>();

                foreach (DataRow dr in ViewBag.role.Rows)
                {
                    li_role.Add(new SelectListItem { Text = @dr["ROLE_NAME"].ToString(), Value = @dr["ROLE_ID"].ToString() });
                }
                ViewBag.ROLE_List = li_role;

                List<SelectListItem> li_employee = new List<SelectListItem>();

                //foreach (DataRow dr in ViewBag.emp.Rows)
                //{
                //    li_employee.Add(new SelectListItem { Text = @dr["TEMP_NAME"].ToString(), Value = @dr["EMP_ID"].ToString() });
                //}
                //ViewBag.EMP_List = li_employee;

                DataTable dtEmployeeCodeNotSystemUser = dblayer.getEmployeeNotSystemUser();
                var employeesCodeNotSystemUser_list = (from DataRow dr in dtEmployeeCodeNotSystemUser.Rows
                                                       select new employeeModel()
                                                       {
                                                           EMPLOYEE_ID = Convert.ToInt32(dr["PERSON_ID"].ToString()),
                                                           EMPLOYEE_CODE = dr["EMPLOYEE_CODE"].ToString(),
                                                           EMP_NAME = dr["EMPLOYEE_CODE"].ToString() + " (" + dr["EMP_NAME"].ToString() + ")"
                                                       }).ToList();

                ViewBag.employeesCodeNotSystemUser = new SelectList(employeesCodeNotSystemUser_list, "EMPLOYEE_CODE", "EMP_NAME");

                foreach (DataRow dr in ViewBag.SYSTEM_USER.Rows)
                {
                    SystemUser su = new SystemUser();
                    su.Sr_No = Convert.ToInt32(dr["SR_NO"].ToString());
                    su.emp_id = Convert.ToInt32(dr["EMP_ID"].ToString());
                    su.emp_name = dr["EMP_NAME"].ToString();
                    su.Letter = dr["Letter"].ToString();
                    su.email = dr["Email"].ToString();
                    su.created_date = dr["Created_date"].ToString();
                    su.role_name = dr["role_name"].ToString();
                    su.role_id = Convert.ToInt32(dr["role_id"].ToString());
                    li_users.Add(su);
                }
            }
            catch (Exception ex)
            {

            }
            return View(li_users);
        }
        [HttpPost]
        public JsonResult GetEmployee(int EMP_CODE)
        {
            List<SystemUser> li_system = new List<SystemUser>();
            try
            {
                DataTable dt = new DataTable();
                dt = (DataTable)Session["Employee"];
                DataRow[] dr = dt.Select("EMPLOYEE_CODE='" + EMP_CODE + "'");
                if (dr.Length > 0)
                {
                    SystemUser su = new SystemUser();
                    su.full_name = dr[0]["EMP_NAME"].ToString();
                    su.emp_id = Convert.ToInt32(dr[0]["EMP_ID"].ToString());
                    //su.first_name = dr[0]["FIRST_NAME"].ToString();
                    //su.middle_name = dr[0]["MIDDLE_NAME"].ToString();
                    //su.last_name = dr[0]["LAST_NAME"].ToString();
                    su.email = dr[0]["EMAIL"].ToString();
                    li_system.Add(su);
                }
            }
            catch (Exception ex)
            {

            }
            return Json(li_system, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult GetRoleById(int role_id)
        {
            List<roleModel> rm_list = new List<roleModel>();
            try
            {
                DataTable dt = dblayer.getRoleById(role_id);
                if(dt.Rows.Count>0)
                {
                    foreach(DataRow dr in dt.Rows)
                    {
                        roleModel rm = new roleModel();
                        rm.ROLE_ID = Convert.ToInt32(dr["ROLE_ID"].ToString());
                        rm.ROLE_NAME = dr["ROLE_NAME"].ToString();
                        rm.status = Convert.ToInt32(dr["STATUS"].ToString());
                        rm_list.Add(rm);
                    }
                }
            }
            catch(Exception ex)
            {
                
            }
            return Json(rm_list,JsonRequestBehavior.AllowGet);
        }
        public ActionResult securitypolicy()
        {
            return View();
        }

        public ActionResult emailtemplates()
        {
            return View();
        }
        public ActionResult approval_rules()
        { 
            Page_Permission_Detail info = new Page_Permission_Detail();
            info = dblayer.Get_Page_UserPermission(Convert.ToInt32(Session["emp_id"].ToString()), 37);
            ViewBag.event_permission = info;

            List<SelectListItem> li_level = new List<SelectListItem>();
            ViewBag.level = li_level;
           
            return View();
        }
        [HttpPost]
        public ActionResult get_approval_rules(string approval_for ,string id)
        {
           
            List<ApprovalRule> arlist = new List<ApprovalRule>();
            List<Condition> clist = new List<Condition>();
            DataSet ds = new DataSet();
            ds = dblayer.Get_all_approval_rules(approval_for, id);
            if (ds.Tables[0].Rows.Count>0)
            {
                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    ApprovalRule ar = new ApprovalRule();
                    ar.For = row["APPROVAL_FOR"].ToString();
                    ar.ID = Convert.ToInt32(row["AHEAD_ID"].ToString());
                    ar.Type = row["TYPENAME"].ToString();
                    ar.Type_Id = row["APPROVAL_TYPE"].ToString();
                   // ar.ApprovalTypeFor = row["approvaltypefor_name"].ToString();
                    ar.StartDate = row["START_DATE"].ToString();
                    ar.EndDate = row["END_DATE"].ToString();
                    ar.Status = row["APPROVAL_STATUS"].ToString();
                    ar.RuleName = row["RULE_NAME"].ToString();

                    if (ds.Tables[1].Rows.Count > 0)
                    {
                        foreach (DataRow row1 in ds.Tables[1].Rows)
                        {
                            Condition cnd = new Condition();
                            cnd.formula = row1["CONDITION_FOR_DISPLAY"].ToString();
                            cnd.chk_hierarch= Convert.ToInt32(row1["IS_HIERARCHY"].ToString());
                            cnd.hierarchyname = row1["H_MAX_LEVEL"].ToString();
                            cnd.chk_singleuser = Convert.ToInt32(row1["IS_Single_User"].ToString());
                            cnd.singleuser_code =  row1["emp_name"].ToString();
                            cnd.chk_approval = Convert.ToInt32(row1["IS_Auto_Approval"].ToString());
                             
                            clist.Add(cnd);

                        }

                        ar.condition = clist;
                    }
                    
                    arlist.Add(ar);



                }
             
            }
         
            return Json(arlist, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult get_approval_rules_edit(  string id)
        {
            List<ApprovalRule_edit> arlist = new List<ApprovalRule_edit>();
            List<Condition_edit> clist = new List<Condition_edit>();
            DataSet ds = new DataSet();
            ds = dblayer.Get_all_approval_rules_edit(id);
            if (ds.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    ApprovalRule_edit ar = new ApprovalRule_edit();
                    ar.For = row["APPROVAL_FOR"].ToString();
                    ar.ID = Convert.ToInt32(row["AHEAD_ID"].ToString());
                    ar.Type = row["TYPENAME"].ToString();
                    ar.Type_Id = row["APPROVAL_TYPE"].ToString();
                 //   ar.ApprovalTypeFor = row["APPROVAL_TYPE_FOR"].ToString();
                    ar.StartDate = row["START_DATE"].ToString();
                    ar.EndDate = row["END_DATE"].ToString();
                    ar.Status = row["APPROVAL_STATUS"].ToString();
                    ar.RuleName = row["RULE_NAME"].ToString();

                    if (ds.Tables[1].Rows.Count > 0)
                    {
                        foreach (DataRow row1 in ds.Tables[1].Rows)
                        {
                            Condition_edit cnd = new Condition_edit();
                          //  cnd.ID =Convert.ToInt32( row1["APPROVAL_HIERARCHY_ID"].ToString());
                            cnd.type = row["TYPENAME"].ToString();
                            cnd.type_id = row["APPROVAL_TYPE"].ToString();
                            cnd.formula = row1["CONDITION_FOR_DISPLAY"].ToString();
                            cnd.formula_Value = row1["CONDITION_FOR_VALUE"].ToString();
                            cnd.chk_hierarch = Convert.ToInt32(row1["IS_HIERARCHY"].ToString());
                            cnd.hierarchyname = row1["HIERARCHY_ID"].ToString();
                            cnd.hierarchymaxlevel = row1["H_MAX_LEVEL"].ToString();
                            cnd.chk_singleuser = Convert.ToInt32(row1["IS_Single_User"].ToString());
                            cnd.singleuser_code = row1["EMPLOYEE_ID"].ToString();
                            cnd.singleuser_name = row1["emp_name"].ToString();
                            cnd.chk_approval = Convert.ToInt32(row1["IS_Auto_Approval"].ToString()); 
                            cnd.chk_fyi = Convert.ToInt32(row1["chkFYI"].ToString());
                            cnd.fyi_frequency =row1["FYI_LEVEL"].ToString();
                            cnd.fyi_email = row1["FYI_EMAILID"].ToString();
                            //cnd.fyi_employee = row1["IS_Auto_Approval"].ToString();
                           // cnd.fyi_employee_id = row1["IS_Auto_Approval"].ToString();

                            clist.Add(cnd);

                        }

                        ar.condition = clist;
                    }

                    arlist.Add(ar);



                }

            }

            return Json(arlist, JsonRequestBehavior.AllowGet);
        }
        public ActionResult approval_rules_setting()
        {
            List<SelectListItem> li_value = new List<SelectListItem>();
            ViewBag.value = li_value;

            List<SelectListItem> li_hierarchy = new List<SelectListItem>();
            ViewBag.hierarchy = li_hierarchy;

            List<SelectListItem> li_emp = new List<SelectListItem>();
            ViewBag.Employee = li_emp;
            return View();
        }
        public ActionResult edit_approval_rules_setting(string id) {
            if (id == null)
            {
                return RedirectToAction("approval_rules", "settings");
            }
            else
            {

                List<SelectListItem> li_value = new List<SelectListItem>();
                ViewBag.value = li_value;

                List<SelectListItem> li_hierarchy = new List<SelectListItem>();
                ViewBag.hierarchy = li_hierarchy;

                List<SelectListItem> li_emp = new List<SelectListItem>();
                ViewBag.Employee = li_emp;
                return View();
            }
        
        }
        public JsonResult BindLeaveType()
        {
            DataSet ds = dblayer.BindList();

            ViewBag.leavetype = ds.Tables[0];

            List<ApprovalRulesModel> lmlist = new List<ApprovalRulesModel>();

            foreach (DataRow dr in ViewBag.leavetype.Rows)
            {
                lmlist.Add(new ApprovalRulesModel { leave_type_name = @dr["LEAVE_TYPE_NAME"].ToString(), leave_type_id = Convert.ToInt32(@dr["LEAVE_TYPE_ID"].ToString()) });
            }
            return Json(lmlist,JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult BindLoadType()
        {
            DataSet ds = dblayer.BindList();

            ViewBag.leavetype = ds.Tables[20];

            List<ApprovalRulesModel> lmlist = new List<ApprovalRulesModel>();

            foreach (DataRow dr in ViewBag.leavetype.Rows)
            {
                lmlist.Add(new ApprovalRulesModel { leave_type_name = @dr["ADVANCE_LOAN_TYPE"].ToString(), leave_type_id = Convert.ToInt32(@dr["ADVANCE_LOAN_TYPE_ID"].ToString()) });
            }
            return Json(lmlist, JsonRequestBehavior.AllowGet);
        }
        public JsonResult BindExpenceType()
        {
            DataSet ds = dblayer.BindList();

            ViewBag.expencetype = ds.Tables[21];

            List<ApprovalRulesModel> lmlist = new List<ApprovalRulesModel>();

            foreach (DataRow dr in ViewBag.expencetype.Rows)
            {
                lmlist.Add(new ApprovalRulesModel { leave_type_name = @dr["EXPENSE_TYPE_NAME"].ToString(), leave_type_id = Convert.ToInt32(@dr["EXPENSE_TYPE_ID"].ToString()) });
            }
            return Json(lmlist, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult Bind_Hierarchy_Name()
        {
            String msg= dblayer.bind_hierarchy_name();
 
            return Json(msg, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult saveapprovalrule(ApprovalRule approvalrule)
        {
            String msg = dblayer.Add_Approval_Rule(approvalrule, Session["emp_id"].ToString());
            return Json(msg, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult Bind_Hierarchy_Level(String Name)
        {
            String msg = dblayer.bind_hierarchy_level(Name);

            return Json(msg, JsonRequestBehavior.AllowGet);
        }
        public JsonResult BindUser()
        {
            DataSet ds = dblayer.BindList();

            ViewBag.user = ds.Tables[1];

            List<ApprovalRulesModel> lmlist = new List<ApprovalRulesModel>();

            foreach (DataRow dr in ViewBag.user.Rows)
            {
                lmlist.Add(new ApprovalRulesModel { emp_name = @dr["EMP_NAME"].ToString(), emp_id = Convert.ToInt32(@dr["PERSON_ID"].ToString()) });
            }
            return Json(lmlist, JsonRequestBehavior.AllowGet);
        }
        public JsonResult BindRole()
        {
            DataSet ds = dblayer.BindList();

            ViewBag.ROLE = ds.Tables[2];

            List<ApprovalRulesModel> lmlist = new List<ApprovalRulesModel>();

            foreach (DataRow dr in ViewBag.ROLE.Rows)
            {
                lmlist.Add(new ApprovalRulesModel { role_name = @dr["ROLE_NAME"].ToString(), role_id = Convert.ToInt32(@dr["ROLE_ID"].ToString()) });
            }
            return Json(lmlist, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetLevelValue()
        {
            List<LevelModel> li_level = new List<LevelModel>();
            DataSet ds = dblayer.BindList();
            DataTable dt = ds.Tables[4];
            try
            {
                foreach (DataRow dr in dt.Rows)
                {
                    li_level.Add(new LevelModel { level_name = @dr["LEVEL_NAME"].ToString(), level_id = Convert.ToInt32(@dr["ID"].ToString()) });
                }
            }
            catch(Exception ex)
            {

            }
            return Json(li_level,JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult save_hierarchy(string TableData)
        {
            LevelModel lm = new LevelModel();
            
            try
            {
                var serializeData = JsonConvert.DeserializeObject<List<HierarchyModel>>(TableData);
                
                List<HierarchyModel> li_hierarchy = new List<HierarchyModel>();
                //li_hierarchy.Add(TableData);
                ListtoDataTableConverter converter = new ListtoDataTableConverter();

                DataTable dt = converter.ToDataTable(serializeData);
               
                string res = dblayer.save_hierarchy(dt);
                string[] response = res.Split(',');
                if(response[0]=="Success")
                {
                    lm.success_msg = response[1];
                }
                else
                {
                    lm.error_msg = response[1];
                }
            }
            catch(Exception ex)
            {
                lm.error_msg = ex.Message;
            }
           return Json(lm,JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult GetHierarchyById(int hierarchy_id)
        {
            List<LevelModel> li_hier = new List<LevelModel>();
            try
            {
                DataTable dt = dblayer.GetHierarchy(hierarchy_id);
                
                foreach(DataRow dr in dt.Rows)
                {
                    LevelModel lm = new LevelModel();
                    lm.sr_no = Convert.ToInt32(dr["SR_NO"].ToString());
                    lm.hierarchy_id = Convert.ToInt32(dr["HIERARCHY_ID"].ToString());
                    lm.hierarchy_name = dr["HIERARCHY_NAME"].ToString();
                    lm.hierarchy_level = dr["HIERARCHY_LEVEL"].ToString();
                    lm.hierarchy_value = dr["HIERARCHY_VALUE"].ToString();
                    lm.status = dr["STATUS"].ToString();
                    li_hier.Add(lm);
                }
            }
            catch(Exception ex)
            {

            }
            return Json(li_hier, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult UpdateHierarchyById(int hierarchy_id,string hierarchy_level,string hierarchy_value,string status)
        {
            LevelModel lm = new LevelModel();
            try
            {
                string res = dblayer.update_hierarchy(hierarchy_id,hierarchy_level,hierarchy_value,status);
                if(res=="update")
                {
                    lm.success_msg = "Successfully Updated.";
                }
            }
            catch(Exception ex)
            {
                lm.error_msg = ex.Message;
            }
            return Json(lm, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult GetHierarchyByName(string hierarchy_name)
        {
            List<LevelModel> li_hier = new List<LevelModel>();
            try
            {
                DataTable dt = dblayer.GetHierarchyByName(hierarchy_name);

                foreach (DataRow dr in dt.Rows)
                {
                    LevelModel lm = new LevelModel();
                    lm.sr_no = Convert.ToInt32(dr["SR_NO"].ToString());
                    lm.hierarchy_id = Convert.ToInt32(dr["HIERARCHY_ID"].ToString());
                    lm.hierarchy_name = dr["HIERARCHY_NAME"].ToString();
                    lm.hierarchy_level_attr = dr["HIERARCHY_LEVEL_ATTR"].ToString();
                    lm.hierarchy_level = dr["HIERARCHY_LEVEL"].ToString();
                    lm.hierarchy_value = dr["HIERARCHY_VALUE"].ToString();
                    lm.hierarchy_value_attr = dr["HIERARCHY_VALUE_ATTR"].ToString();
                    lm.emp_name= dr["EMP_NAME"].ToString();
                    lm.emp_id= dr["EMP_ID"].ToString();
                    lm.startdate = dr["START_DATE"].ToString();
                    lm.enddate = dr["END_DATE"].ToString();

                    lm.status = dr["STATUS"].ToString();
                    li_hier.Add(lm);
                }
            }
            catch(Exception ex)
            {

            }
            return Json(li_hier,JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetHierarchy()
        {
            List<LevelModel> li_hier = new List<LevelModel>();
            try
            {
                DataSet ds = dblayer.BindList();
                DataTable dt = ds.Tables[5];

                foreach (DataRow dr in dt.Rows)
                {
                    LevelModel lm = new LevelModel();
                    lm.sr_no = Convert.ToInt32(dr["SR_NO"].ToString());
                    lm.hierarchy_name = dr["HIERARCHY_NAME"].ToString();
                    li_hier.Add(lm);
                }
            }
            catch (Exception ex)
            {

            }
            return Json(li_hier, JsonRequestBehavior.AllowGet);
        }
        public ActionResult Hierarchy()
        {
            Page_Permission_Detail info = new Page_Permission_Detail();
            info = dblayer.Get_Page_UserPermission(Convert.ToInt32(Session["emp_id"].ToString()), 43);
            ViewBag.event_permission = info;
            List<SelectListItem> li_emp = new List<SelectListItem>();
            ViewBag.employee = li_emp;
            return View();
        }
        [HttpPost]
        public JsonResult BindEmployee()
        {
            List<ApprovalRulesModel> lmlist = new List<ApprovalRulesModel>();
            try
            {
                DataSet ds = dblayer.BindList();

                ViewBag.employee = ds.Tables[1];

                foreach (DataRow dr in ViewBag.employee.Rows)
                {
                    lmlist.Add(new ApprovalRulesModel { emp_name = @dr["EMP_NAME"].ToString(), emp_id = Convert.ToInt32(@dr["PERSON_ID"].ToString()) });
                }
            }
            catch (Exception ex)
            {

            }
            return Json(lmlist, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public JsonResult CreateUser(SystemUser su)
        {
            SystemUser system = new SystemUser();
            try
            {
                string res = dblayer.CreateSystemUser(su.emp_id, su.password, su.is_navone, su.status, su.role_id, su.emp_code, su.start_date, su.end_date, su.store_id,su.chkmanagement,su.chkhr,su.chkfh);
                string[] response = res.Split(',');
                if (response[0] == "Success")
                {
                    string resPermission = "Not updated";
                    if (su.userPermissions != null)
                    {
                        su.userPermissions.ToList().ForEach(a => a.EMPLOYEE_ID = su.emp_id);
                        resPermission = dblayer.updateUserRolePermission(su.userPermissions.AsEnumerable(), Session["emp_id"].ToString());
                    }
                    system.success_msg = response[1];
                }
                else
                {
                    system.error_msg = response[1];
                }
            }
            catch (Exception ex)
            {
                system.error_msg = ex.Message;
            }
            return Json(system, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult GetEmployeeById(int EMP_ID)
        {
            List<SystemUser> li_system = new List<SystemUser>();
            //List<SystemUser> li_roles = new List<SystemUser>();
            try
            {
                DataTable emp_dt = dblayer.GetEmployeeById(EMP_ID);
                foreach (DataRow dr in emp_dt.Rows)
                {
                    SystemUser su = new SystemUser();
                    su.user_id = Convert.ToInt32(dr["USER_ID"].ToString());
                    su.emp_id = Convert.ToInt32(dr["EMP_ID"].ToString());
                    su.Showemp_code = dr["TEMP_NAME"].ToString();
                    su.emp_code = dr["EMP_CODE"].ToString();
                    su.full_name = dr["FULL_NAME"].ToString();
                    //su.first_name = dr["FIRST_NAME"].ToString();
                    //su.middle_name = dr["MIDDLE_NAME"].ToString();
                    //su.last_name = dr["LAST_NAME"].ToString();
                    su.email = dr["EMAIL"].ToString();
                    su.role_id = Convert.ToInt32(dr["ROLE_ID"].ToString());
                    su.password = dr["USER_PASSWORD"].ToString();
                    su.confirm_password = dr["USER_PASSWORD"].ToString();
                    su.status = dr["STATUS"].ToString();
                    su.is_navone = Convert.ToInt32(dr["IS_NAVONE_USER"].ToString());
                    su.Showstart_date = dr["EFFECTIVE_START_DATE"].ToString();
                    su.Showend_date = dr["EFFECTIVE_END_DATE"].ToString();
                    su.store_id = Convert.ToInt32(dr["STORE_ID"].ToString());
                    su.chkfh = Convert.ToInt32(dr["IS_FH_USER"].ToString());
                    su.chkhr = Convert.ToInt32(dr["IS_HR_USER"].ToString());
                    su.chkmanagement = Convert.ToInt32(dr["Is_Management_USER"].ToString());
                    li_system.Add(su);
                }
                //foreach (DataRow dr in roles_dt.Rows)
                //{
                //    SystemUser su = new SystemUser();
                //    su.role_id = Convert.ToInt32(dr["ROLE_ID"].ToString());
                //    li_roles.Add(su);
                //}
            }
            catch (Exception ex)
            {

            }
            return Json(li_system, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult ViewEmployeeById(int EMP_ID)
        {
            List<SystemUser> li_system = new List<SystemUser>();
            try
            {
                DataTable emp_dt = dblayer.GetEmployeeById(EMP_ID);
                foreach (DataRow dr in emp_dt.Rows)
                {
                    SystemUser su = new SystemUser();

                    su.Showemp_code = dr["TEMP_NAME"].ToString();
                    su.emp_code = dr["EMP_CODE"].ToString();
                    su.full_name = dr["FULL_NAME"].ToString();
                    su.email = dr["EMAIL"].ToString();
                    su.roles = dr["ROLE_NAME"].ToString();
                    su.password = dr["USER_PASSWORD"].ToString();
                    su.confirm_password = dr["USER_PASSWORD"].ToString();
                    su.status = dr["STATUS"].ToString();
                    su.is_navone = Convert.ToInt32(dr["IS_NAVONE_USER"].ToString());
                    su.Showstart_date = dr["EFFECTIVE_START_DATE"].ToString();
                    su.Showend_date = dr["EFFECTIVE_END_DATE"].ToString();
                    su.role_id = Convert.ToInt32(dr["ROLE_ID"].ToString());
                    su.emp_id = Convert.ToInt32(dr["EMP_ID"].ToString());
                    su.store_name = dr["STORE_NAME"].ToString();
                    su.chkfh = Convert.ToInt32(dr["IS_FH_USER"].ToString());
                    su.chkhr = Convert.ToInt32(dr["IS_HR_USER"].ToString());
                    su.chkmanagement = Convert.ToInt32(dr["Is_Management_USER"].ToString());
                    li_system.Add(su);
                }
            }
            catch (Exception ex)
            {

            }
            return Json(li_system, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult UpdateUser(SystemUser su)
        {
            SystemUser system = new SystemUser();
            try
            {
                int emp_id = Convert.ToInt32(Session["emp_id"].ToString());
                string res = dblayer.UpdateSystemUser(emp_id, su.user_id, su.password, su.is_navone, su.status, su.role_id, su.start_date, su.end_date, su.store_id, su.chkmanagement, su.chkhr, su.chkfh);
                string[] response = res.Split(',');
                if (response[0] == "Success")
                {
                    string resPermission = "Not updated";
                    if (su.userPermissions != null)
                    {
                        resPermission = dblayer.updateUserRolePermission(su.userPermissions.AsEnumerable(), Session["emp_id"].ToString());
                    }
                    system.success_msg = response[1];
                }
                else
                {
                    system.error_msg = response[1];
                }
            }
            catch (Exception ex)
            {
                system.error_msg = ex.Message;
            }
            return Json(system, JsonRequestBehavior.AllowGet);
        }
        //Ajax call
        public JsonResult CreateRole(string roleName,int role_status)
        {
            try
            {
                string error = dblayer.addroles(roleName, cm.getCurrentDate, Session["emp_id"].ToString(),role_status);
                if (error == "")
                {
                    successStatus.SuccessMsg = "success";
                }
                successStatus.ErrorMsg = error;
            }
            catch (Exception ex)
            {
                successStatus.ErrorMsg = "Exception while creating role";
            }


            return Json(successStatus, JsonRequestBehavior.AllowGet);
        }
        //Ajax call
        public JsonResult UpdateRole(int role_id, int role_status,string role_name)
        {
            try
            {
                string res = dblayer.Updateroles(role_id, role_status, role_name);
                string[] response = res.Split(',');
                if (response[0] == "Success")
                {
                    successStatus.SuccessMsg = response[1];
                }
                else
                {
                    successStatus.ErrorMsg = response[1];
                }
                
            }
            catch (Exception ex)
            {
                successStatus.ErrorMsg = ex.Message ;
            }


            return Json(successStatus, JsonRequestBehavior.AllowGet);
        }
        public JsonResult UpdateRolePermissionModule(List<RolePermissionsStatus> rolePermissions)
        {
            string error = "";
            try
            {
                if (rolePermissions.Count > 0)
                {
                    error = dblayer.updateRolePermission(rolePermissions.AsEnumerable());

                    if (error == "")
                    {
                        successStatus.SuccessMsg = "success";
                    }
                }
                else
                {
                    error = "No records to update";
                }
                successStatus.ErrorMsg = error;
            }
            catch (Exception ex)
            {
                successStatus.ErrorMsg = "Exception while creating role";
            }


            return Json(successStatus, JsonRequestBehavior.AllowGet);
        }
        public PartialViewResult GetUserPermissionByRoleId(int roleId, int empId)
        {

            #region Create user permission

            DataTable dtPagePermission = dblayer.getPagePermissionForUser(roleId, empId);

            #region Create table with columns

            DataTable dtfilterPagePermission = new DataTable();

            dtfilterPagePermission.Columns.Add("ROLE_PERMISSION_ID");
            dtfilterPagePermission.Columns.Add("ROLE_ID");
            dtfilterPagePermission.Columns.Add("PAGE_ID");

            dtfilterPagePermission.Columns.Add("PERMISSION_ID");
            dtfilterPagePermission.Columns.Add("MODULE_ID");
            dtfilterPagePermission.Columns.Add("STATUS");
            dtfilterPagePermission.Columns.Add("UPSTATUS");
            dtfilterPagePermission.Columns.Add("PAGE_NAME");
            dtfilterPagePermission.Columns.Add("MODULE_NAME");

            int noOfColumns = dtPagePermission.Columns.Count - 1;
            int noOfColumnsDynamic = 0;

            DataView view = new DataView(dtPagePermission);
            DataTable distinctValues = view.ToTable(true, "PERMISSION_NAME");
            foreach (DataRow pname in distinctValues.Rows)
            {
                dtfilterPagePermission.Columns.Add(pname["PERMISSION_NAME"].ToString());
                noOfColumnsDynamic += 1;
            }

            #endregion

            var pageName = "";
            int fillColumns = 0;
            DataRow row = dtfilterPagePermission.NewRow();

            foreach (DataRow pp in dtPagePermission.Rows)
            {

                if (pageName == "" || (pageName != pp["PAGE_NAME"].ToString()))
                {
                    pageName = pp["PAGE_NAME"].ToString();
                    fillColumns = 1;
                    row = dtfilterPagePermission.NewRow();

                    row["ROLE_PERMISSION_ID"] = pp["ROLE_PERMISSION_ID"].ToString();
                    row["ROLE_ID"] = pp["ROLE_ID"].ToString();
                    row["PAGE_ID"] = pp["PAGE_ID"].ToString();
                    row["PERMISSION_ID"] = pp["PERMISSION_ID"].ToString();
                    row["MODULE_ID"] = pp["MODULE_ID"].ToString();
                    row["STATUS"] = pp["STATUS"].ToString();
                    row["PAGE_NAME"] = pp["PAGE_NAME"].ToString();
                    row["MODULE_NAME"] = pp["MODULE_NAME"].ToString();
                    row[noOfColumns] = pp["ROLE_PERMISSION_ID"] + "~" + pp["STATUS"].ToString() + "~" + pp["UPSTATUS"].ToString();
                }
                else if (pageName == pp["PAGE_NAME"].ToString())
                {
                    row[noOfColumns + fillColumns] = pp["ROLE_PERMISSION_ID"] + "~" + pp["STATUS"].ToString() + "~" + pp["UPSTATUS"].ToString();
                    fillColumns += 1;
                }
                //If original value including dynamic column 
                if (noOfColumnsDynamic == fillColumns)
                {
                    dtfilterPagePermission.Rows.Add(row);
                }
            }
            ViewBag.pagePermission = dtfilterPagePermission;
            ViewBag.fixedColumns = noOfColumns;

            UserPermissions up = new UserPermissions();
            up.DATATABLE_VALUE = dtfilterPagePermission;
            up.NO_FIXED_COLUMN = noOfColumns;
            up.EMPLOYEE_ID = empId;
            up.ROLE_ID = roleId;

            #endregion

            if (empId == 0)
            {
                return PartialView("_UserPermissionAdd", up);
            }
            else
            {
                return PartialView("_UserPermission", up);
            }

        }

        [NonAction]
        public string UpdateUserRolePermission(List<UserPermissionsStatus> userPermissions, string login_user)
        {
            string error = "";
            try
            {
                if (userPermissions.Count > 0)
                {
                    error = dblayer.updateUserRolePermission(userPermissions.AsEnumerable(), login_user);


                    if (error == "")
                    {
                        successStatus.SuccessMsg = "success";
                    }
                }
                else
                {
                    error = "No records to update";
                }
                successStatus.ErrorMsg = error;
            }
            catch (Exception ex)
            {
                successStatus.ErrorMsg = "Exception while creating role";
            }


            return error;
        }

         
        public ActionResult Delegation()
        {
            List<SelectListItem> Employee = new List<SelectListItem>()
            {
                new SelectListItem{ Text="Select",Value="0"},
              
            };
            List<SelectListItem> Employeeedit = new List<SelectListItem>()
            {
                new SelectListItem{ Text="Select",Value="0"},

            };
            try
            {
                //DataSet ds; //= dblayer.BindList();

                DataSet ds = dblayer.delegation_emp_list(Convert.ToInt32(Session["emp_id"].ToString()));
               // ViewBag.employee_edit = ds.Tables[1];

                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    Employee.Add(new SelectListItem { Text  = @dr["EMP_NAME"].ToString(), Value =  @dr["PERSON_ID"].ToString() });
                }
                ViewBag.employeelist = Employee;

                //foreach (DataRow dr in ViewBag.employee.Rows)
                //{
                //    Employeeedit.Add(new SelectListItem { Text = @dr["EMP_NAME"].ToString(), Value = @dr["PERSON_ID"].ToString() });
                //}
                ViewBag.employeelist_edit = Employee;

            }
            catch (Exception ex)
            {

            }
            return View();
        }
        [HttpPost]
        public JsonResult adddelegation(delegation delegation)
        {

            string msg = dblayer.Add_delegation(delegation, Session["emp_id"].ToString());
            return Json(msg, JsonRequestBehavior.AllowGet);
        }


        public JsonResult binddelegationintable()
        {
            DataTable dt = new DataTable();
            dt = dblayer.Binddelegation( Session["emp_id"].ToString());
            var jsonobj = DataTableToJSONWithJSONNet(dt);
            return Json(jsonobj, JsonRequestBehavior.AllowGet);
        }


        public JsonResult get_delegationbyid(int id)
        {
            DataTable dt = new DataTable();
            dt = dblayer.Get_delegation(id);
            var jsonobj = DataTableToJSONWithJSONNet(dt);
            return Json(jsonobj, JsonRequestBehavior.AllowGet);
        }
        public JsonResult binduserontype(string TYPE)
        {
            ViewBag.user = dblayer.Get_delegation_employee_on_type(TYPE, Session["emp_id"].ToString());
            List<ApprovalRulesModel> lmlist = new List<ApprovalRulesModel>();
            foreach (DataRow dr in ViewBag.user.Rows)
            {
                lmlist.Add(new ApprovalRulesModel { emp_name = @dr["EMP_NAME"].ToString(), emp_id = Convert.ToInt32(@dr["PERSON_ID"].ToString()) });
            }
            return Json(lmlist, JsonRequestBehavior.AllowGet);
        }
        public JsonResult binduserontype_edit(string TYPE,int delegation_apply_emp_id )
        {
            ViewBag.user = dblayer.Get_delegation_employee_on_type_edit(TYPE, delegation_apply_emp_id, Session["emp_id"].ToString());
            List<ApprovalRulesModel> lmlist = new List<ApprovalRulesModel>();
            foreach (DataRow dr in ViewBag.user.Rows)
            {
                lmlist.Add(new ApprovalRulesModel { emp_name = @dr["EMP_NAME"].ToString(), emp_id = Convert.ToInt32(@dr["PERSON_ID"].ToString()) });
            }
            return Json(lmlist, JsonRequestBehavior.AllowGet);
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


        #region theme settings
        // GET: ThemeModel
        DALTheme objDalTheme = new DALTheme();
        public ActionResult themesettings()
        {
            Page_Permission_Detail info = new Page_Permission_Detail();
            info = dblayer.Get_Page_UserPermission(Convert.ToInt32(Session["emp_id"].ToString()), 30);
            ViewBag.event_permission = info;

            ThemeModel tm = new ThemeModel();
            tm = objDalTheme.GetThemeSettings();
            return View(tm);
        }
        [HttpPost]
        public JsonResult AddInsertUpdateCreateTheme(ThemeModel objTheme)
        {
            ThemeModel sm = new ThemeModel();
            string res = "";
            try
            {
                res = objDalTheme.ProcessDataInsertUpdate(objTheme);
                string[] response = res.Split(',');
                if (response[0] == "Success")
                {
                    sm.SuccessMsg = response[1];
                }
                else
                {
                    sm.ErrorMsg = response[1];
                }
            }
            catch (Exception ex)
            {
                sm.ErrorMsg = ex.Message;
            }
            return Json(sm, JsonRequestBehavior.AllowGet);
        }

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
                file_name = fname + "_" + randomstr + "." + extension;

                FileName.Add(file_name);
                // Get the complete folder path and store the file inside it.    
                fname = Path.Combine(Server.MapPath("~/ThemeImg/"), fname + "_" + randomstr + "." + extension);
                file.SaveAs(fname);
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
    }
    #endregion


}
