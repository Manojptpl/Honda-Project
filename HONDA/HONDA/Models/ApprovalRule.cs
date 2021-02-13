using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HONDA.Models
{
    public class ApprovalRule
    {
        public int ID { get; set; }
        public string For { get; set; }
        public string  Type { get; set; }
        public string ApprovalTypeFor { get; set; }
        public string Type_Id { get; set; }
        public string RuleName { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public string Status { get; set; }
        public List<Condition> condition { get; set; }

    }
    public class Condition
    { 
        public int ID { get; set; }
        public string type { get; set; }
        public string formula { get; set; }
        public string formula_Value { get; set; }
        public int chk_hierarch { get; set; }
        public string hierarchyname { get; set; }
        public string hierarchymaxlevel { get; set; }
        public int chk_singleuser { get; set; }
        public string singleuser_code { get; set; }
        public int chk_approval { get; set; }
        public int chk_fyi { get; set; }
        public string fyi_frequency { get; set; }
        public string fyi_email { get; set; }
        public string fyi_employee { get; set; }

    }
    public class ApprovalRule_edit
    {
        public int ID { get; set; }
        public string For { get; set; }
        public string Type { get; set; }
        public string Type_Id { get; set; }
        public string ApprovalTypeFor { get; set; }
        public string RuleName { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public string Status { get; set; }
        public List<Condition_edit> condition { get; set; }

    }
    public class Condition_edit
    {
        public int ID { get; set; }
        public string type { get; set; }
        public string type_id { get; set; }
        public string formula { get; set; }
        public string formula_Value { get; set; }
        public int chk_hierarch { get; set; }
        public string hierarchyname { get; set; } 
        public string hierarchymaxlevel { get; set; }
        public int chk_singleuser { get; set; }
        public string singleuser_code { get; set; }
        public string singleuser_name { get; set; }
        public int chk_approval { get; set; }
        public int chk_fyi { get; set; }
        public string fyi_frequency { get; set; }
        public string fyi_email { get; set; }
        public string fyi_employee { get; set; }
        public string fyi_employee_id { get; set; }

    }

}