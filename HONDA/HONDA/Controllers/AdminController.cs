using System;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using HONDA.Models;
using HONDA.CommanFilter;
using HONDA.Database_Access_Layer;
using System.Web.Script.Serialization;


namespace HONDA.Controllers
{
    [SessionFilter]
    public class AdminController : Controller
    {
        // GET: Admin
        #region General Information               
        homeModel hm = new homeModel();        
        DataSet ds = new DataSet();
        DataTable dt = new DataTable();
        AdminDB admin_layer = new AdminDB();
        db dblayer = new db();
        MasterSettingModel mm = new MasterSettingModel();
        private List<EmpModel> emp;
        #endregion
        public ActionResult TemplateSettings()
        {
            return View();
        }
        public ActionResult TempQuery()
        {            
            return View();
        }
        public ActionResult DotnetObject()
        {
            var jsonString = "[{\"FN\":\"Manoj\",\"LN\":\"Kumar\",\"Gndr\":\"Male\",\"Salary\":4000},{\"FN\":\"Suraj\",\"LN\":\"Sharma\",\"Gndr\":\"Male\",\"Salary\":3000}]";
            //emp = new List<EmpModel>() {
            //    new EmpModel()
            //    { FN = "Manoj",
            //    LN = "Kumar",
            //    Gndr = "Male",
            //    Salary = 4000},
            //    new EmpModel()
            //    { FN = "Suraj",
            //    LN = "Sharma",
            //    Gndr = "Male",
            //    Salary = 3000}
            //};
            //EmpModel employee1 = new EmpModel
            //{
            //    FN = "Manoj",
            //    LN = "Kumar",
            //    Gndr = "Male",
            //    Salary = 4000
            //};
            //EmpModel employee2 = new EmpModel
            //{
            //    FN = "Suraj",
            //    LN = "Sharma",
            //    Gndr = "Male",
            //    Salary = 3000
            //};
            //List<EmpModel> employeelist = new List<EmpModel>();
            //employeelist.Add(employee1);
            //employeelist.Add(employee2);
            //JavaScriptSerializer javascriptserializer = new JavaScriptSerializer();
            //var jsonString = javascriptserializer.Serialize(employeelist);
            //ViewBag.Data = jsonString;

            //JavaScriptSerializer javascriptserializer = new JavaScriptSerializer();
            //var jsonString = javascriptserializer.Serialize(emp);
            //ViewBag.Data = jsonString;

            JavaScriptSerializer javascriptserializer = new JavaScriptSerializer();            
            List<EmpModel> emplist =(List<EmpModel>) javascriptserializer.Deserialize(jsonString, typeof(List<EmpModel>));
            foreach (EmpModel employee in emplist)
            {
                Response.Write("FN=" + employee.FN + "<br/>");
                Response.Write("LN=" + employee.LN + "<br/>");
                Response.Write("Gender=" + employee.Gndr + "<br/>");
                Response.Write("Salary=" + employee.Salary + "<br/><br/>");
            }
            return View();

        }
               
        public ActionResult MasterSetting()
        {
           
            return View();
        }        

        public JsonResult bindMaterSetting(int type)
        {            
            List<CommonSettingModel> MasterSettingList = new List<CommonSettingModel>();
            try
            {
                dt = admin_layer.GetMasterSetting_History(type);
                foreach (DataRow dr in dt.Rows)
                {
                    CommonSettingModel ms = new CommonSettingModel();
                    ms.Sr_No = Convert.ToInt32(dr["Sr_No"]);
                    ms.ID = Convert.ToInt32(dr["ID"]);
                    ms.F_DESCRIPTION = dr["F_DESCRIPTION"].ToString();
                    ms.PROJECT_FAMILY = dr["PROJECT_FAMILY"].ToString();
                    ms.PROJECT_FLOW = dr["PROJECT_FLOW"].ToString();
                    ms.WORK_CATEGORY = dr["WORK_CATEGORY"].ToString();
                    ms.STATUS = dr["STATUS"].ToString();
                    MasterSettingList.Add(ms);
                }
            }
            catch(Exception ex)
            {
                throw ex;
            }

            return Json(MasterSettingList, JsonRequestBehavior.AllowGet);
        }
        public JsonResult CreateMasterSetting(MasterSettingModel objSettingModel)
        {
            List<MasterSettingModel> MasterSettingModellist = new List<MasterSettingModel>();
            MasterSettingModellist.Add(objSettingModel);
            ListtoDataTableConverter converter = new ListtoDataTableConverter();
            DataTable dt = new DataTable();
            dt = converter.ToDataTable(MasterSettingModellist);

            homeModel hm = new homeModel();
            string res = "";
            try
            {
                res = admin_layer.CreateMasterSetting(dt);

                string[] response = res.Split(',');
                if (response[0] == "Success")
                {
                    hm.SuccessMsg = response[1];
                }
                else if (response[0] == "Error")
                {
                    hm.ErrorMsg = response[1];
                }
            }
            catch (Exception ex)
            {
                hm.ErrorMsg = ex.Message;
            }
            return Json(hm, JsonRequestBehavior.AllowGet);
        }
        
        [HttpPost]
        public JsonResult mastersettingview(int id)
        {
            CommonSettingModel data = new CommonSettingModel();
            data = admin_layer.GetMasterSettingDetails(id);
            return Json(data);
        }
        [HttpPost]
        public JsonResult mastersettingedit(int id)
        {
            CommonSettingModel data = new CommonSettingModel();
            data = admin_layer.GetMasterSettingDetails(id);
            return Json(data);
        }

        public JsonResult UpdateMasterSetting(CommonSettingModel settingModel)
        {
            homeModel hm = new homeModel();
            string res = "";
            try
            {
                res = admin_layer.Update_MasterSetting(settingModel.ID, settingModel.F_DESCRIPTION, settingModel.WORK_CATEGORY, settingModel.PROJECT_FAMILY, settingModel.PROJECT_FLOW, settingModel.STATUS);
                                
                string[] response = res.Split(',');
                if (response[0] == "Success")
                {
                    hm.SuccessMsg = response[1];

                }
                else if (response[0] == "Error")
                {
                    hm.ErrorMsg = response[1];
                }                
            }
            catch (Exception ex)
            {
                hm.ErrorMsg = ex.Message;
            }
            return Json(hm, JsonRequestBehavior.AllowGet);
        }

        public ActionResult AssignTemplate()
        {
            return View();
        }
        DALEmployee dllemployee = new DALEmployee();
        db mydb = new db();
        public ActionResult TimeSheetCalendar()
        {


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
            string menuString = "<li id=\"myli\"><a href=\"" + CurrentBaseUrl + "/admin/TimeSheetCalendar\">TimeSheetCalendar</a></li>";
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
                    // if (moduleName != "")
                    // {
                    // menuString += "</ul></li>";
                    // }
                    // menuString += "<li class=\"submenu\">";
                    // menuString += "<a href=\"javascript: void(0); \"><span>" + dr["MODULE_NAME"].ToString() + "</span> <span class=\"menu-arrow\"></span></a>";
                    // //menuString += "<ul class=\"list-unstyled\" style=\"display:none;\">";
                    // //moduleName = dr["MODULE_NAME"].ToString();
                    //}
                    if (Session["role_id"].ToString() == "3")
                    {

                    }
                    else
                    {
                        menuString += "<li><a href=\"" + CurrentBaseUrl + "" + dr["PAGE_URL"].ToString() + "\">" + dr["PAGE_NAME"].ToString() + "</a></li>";

                    }


                    //if (rowcount == rows.Count())
                    //{
                    // menuString += "</ul></li>";
                    //}

                    rowcount++;
                }

                //Add submenu for settings
                //string settingMenu = "";
                //DataRow[] dsSubMenu = menuByPermission.Tables[0].Select("MODULE_NAME = 'Settings'");
                //if (dsSubMenu.Count() > 0)
                //{
                // //Add setting menu in main page
                // menuString += "<li><a href=\"" + CurrentBaseUrl + "" + dsSubMenu[0]["PAGE_URL"] + "\">Settings</a></li>";
                //}

                //foreach (DataRow dr in dsSubMenu.OrderBy(p => p["MORDER_NO"]).ThenBy(p => p["PORDER_NO"]))
                //{
                // string activeStatus = settingMenu == "" ? " class=\"active\"" : "";
                // settingMenu += "<li><a href=\"" + CurrentBaseUrl + "" + dr["PAGE_URL"].ToString() + "\">" + dr["PAGE_NAME"].ToString() + "</a></li>";
                // //settingMenu +="<li id=\"myli\"><a href=\"../settings/themesettings\">Theme Settings</a></li>";
                //}

                // Session["SettingMenus"] = settingMenu;
                Session["Menus"] = menuString;
                //DataSet ds = mydb.BindList();
                //if (ds.Tables[25].Rows.Count > 0)
                //{

                // Session["favicon"] = "" + ds.Tables[25].Rows[0]["FAVICON"].ToString();
                // Session["logo"] = ds.Tables[25].Rows[0]["LOGO"].ToString();
                // Session["comp_name"] = ds.Tables[25].Rows[0]["COMPANY_NAME"].ToString();
                //}
                //else
                //{
                // Session["favicon"] = "";
                // Session["logo"] = "";
                // Session["comp_name"] = "";
                //}
                return View();
            }
            return View(new EventViewModel());
        }
        public ActionResult TimeSheetReport()
        {
            return View();
        }
        [HttpPost]
        public JsonResult GetEvents(DateTime start, DateTime end)
        {
            var viewModel = new EventViewModel();
            var events = new List<EventViewModel>();


            //start = DateTime.Today.AddDays(-14);
            //end = DateTime.Today.AddDays(-11);

            //for (var i = 1; i <= 5; i++)
            //{
            //    events.Add(new EventViewModel()
            //    {
            //        id = i,
            //        title = "Event " + i,
            //        start = start.ToString(),
            //        end = end.ToString(),
            //        allDay = false
            //    });

            //    start = start.AddDays(7);
            //    end = end.AddDays(7);


            DataTable dt = new DataTable();
            db dblayer = new db();
            dt = dblayer.Get_Event(Convert.ToInt32(Session["emp_id"].ToString()), DateTime.Now, DateTime.Now);
            if (dt.Rows.Count > 0)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    EventViewModel ev = new EventViewModel();
                    ev.id = Convert.ToInt64(dr["ID"].ToString());
                    ev.start = dr["START_DATE"].ToString();
                    ev.end = dr["END_DATE"].ToString();
                    ev.title = dr["TITLE"].ToString();
                    events.Add(ev);
                }
            }
            //events.Add(new EventViewModel()
            //{
            //    id = 6,
            //    title = "Event " + 6,
            //    start = DateTime.Now.ToString("yyyy-MM-dd"),
            //    end = DateTime.Now.ToString("yyyy-MM-dd"),
            //    allDay = false

            //});
            //events.Add(new EventViewModel()
            //{
            //    id = 6,
            //    title = "Event " + 2,
            //    start = DateTime.Now.AddDays(-1).ToString("yyyy-MM-dd"),
            //    end = DateTime.Now.AddDays(-1).ToString("yyyy-MM-dd"),
            //    allDay = false

            //});
            //events.Add(new EventViewModel()
            //{
            //    id = 6,
            //    title = "Event " + 2,
            //    start = DateTime.Now.AddDays(-1).ToString("yyyy-MM-dd"),
            //    end = DateTime.Now.AddDays(-1).ToString("yyyy-MM-dd"),
            //    allDay = false

            //});


            return Json(events.ToArray(), JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult ChangeEvents(int id, string event_name)
        {
            string response = "";
            try
            {
                DataTable dt = new DataTable();
                //db dblayer = new db();
                //response = dblayer.Chagen_Event(id, event_name, Convert.ToInt32(Session["emp_id"].ToString()));
            }
            catch
            {
                response = "error";
            }

            return Json(response, JsonRequestBehavior.AllowGet);
        }
    }
    public class EventViewModel
    {
        public Int64 id { get; set; }

        public String title { get; set; }

        public String start { get; set; }

        public String end { get; set; }

        public bool allDay { get; set; }
    }
}