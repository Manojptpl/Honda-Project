using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HONDA.Models
{
    public class delegation
    {
        public int ID { get; set; }
        public int employee_id { get; set; }
        public string employee_name { get; set; }
        public string   fromdate { get; set; }
        public string todate { get; set; }
        public string type { get; set; }
        public string status { get; set; }
    }
}