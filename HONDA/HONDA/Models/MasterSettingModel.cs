using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HONDA.Models
{
    public class MasterSettingModel
    {        
        public int MASTER_CODE_ID { get; set; }
        public string F_DESCRIPTION { get; set; }
        public string WORK_CATEGORY { get; set; }
        public string PROJECT_FAMILY { get; set; }
        public string STATUS { get; set; }
        public string IS_ACTIVE_FORALL { get; set; }
        public string PROJECT_FLOW { get; set; }
    }
    public class CommonSettingModel
    {
        public int Sr_No { get; set; }
        public int ID { get; set; }
        public int MASTER_CODE_ID { get; set; }
        public string F_DESCRIPTION { get; set; }
        public string WORK_CATEGORY { get; set; }
        public string PROJECT_FAMILY { get; set; }
        public string STATUS { get; set; }
        public string IS_ACTIVE_FORALL { get; set; }
        public string PROJECT_FLOW { get; set; }
    }
    
}