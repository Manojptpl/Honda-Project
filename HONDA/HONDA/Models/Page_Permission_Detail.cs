using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HONDA.Models
{
    public class Page_Permission_Detail
    {
        public int emp_id { get; set; }
        public int page_id { get; set; }
        public int View { get; set; }
        public int Update { get; set; }
        public int Create { get; set; }
        public int Import { get; set; }
        public int Export { get; set; }
    }
}