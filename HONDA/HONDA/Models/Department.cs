using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HONDA.Models
{
    public class Department
    {
        public int departmentid { get; set; }
        public string departmentname { get; set; }
        public string departmentcode { get; set; }
        public string description { get; set; }
        public int head_id { set; get; }
        public string head_name { set; get; }
        public string status { get; set; }
        public string effectivestartdate { get; set; }
        public string effectiveenddate { get; set; }
        public string creationdate { get; set; }
        public string createdby { get; set; }
        public string lastupdatedate { get; set; }
        public string lastupdatedby { get; set; }
        public string lastupdatelogin { get; set; }
        public string success_msg { set; get; }
        public string error_msg { set; get; }

    }
}