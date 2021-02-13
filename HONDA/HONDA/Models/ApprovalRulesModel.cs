using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HONDA.Models
{
    public class ApprovalRulesModel
    {
        public int leave_type_id { set; get; }
        public string leave_type_name { set; get; }
        public string leave_type_code { set; get; }
        public string leave_type_status { set; get; }
        public int emp_id { set; get; }
        public string emp_name { set; get; }
        public string emp_status { set; get; }
        public int role_id { set; get; }
        public string role_name { set; get; }
        public string role_status { set; get; }
        //public List<LevelModel> levelmodal { set; get; }
    }
    public class LevelModel
    {
        public int level_id { set; get; }
        public string level_name { set; get; }
        public int sr_no { set; get; }
        public int hierarchy_id { set; get; }
        public string hierarchy_name { set; get; }
        public string hierarchy_level { set; get; }
        public string hierarchy_level_attr { set; get; }
        public string hierarchy_value { set; get; }
        public string hierarchy_value_attr { set; get; }
        public string startdate { set; get; }
        public string enddate { set; get; }
        public string emp_name { set; get; }
        public string emp_id { set; get; }
        public string success_msg { set; get; }
        public string error_msg { set; get; }
        public string status { set; get; }
    }
    public class HierarchyModel
    {
        public int hierarchy_id { set; get; }
        public string hierarchy_name { set; get; }
        public string hierarchy_level { set; get; }
        public string hierarchy_value { set; get; }
        public string startdate { set; get; }
        public string enddate { set; get; }     
        public int employee_id { set; get; }
        public string status { set; get; }
    }
}