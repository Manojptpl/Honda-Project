using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HONDA.Models
{
    public class Holiday
    {
        public int ID { get; set; }
        public string  holiday_name { get; set; }
        public string holiday_sdate { get; set; }
        public string holiday_edate { get; set; }
        public int holiday_type { get; set; }
        public string holiday_typename { get; set; }
        public string  holiday_date { get; set; }
        public string holiday_region { get; set; }
        public string holiday_region_name { get; set; }
        public string holiday_region_name2 { get; set; }
        public string holiday_day { get; set; }
      //  public string holiday_noofday { get; set; }
        public string holiday_desc { get; set; }
        public string createdby { get; set; }
        public string updatedby { get; set; }
        public string status { get; set; }
        public string currentdate { get; set; }

        public string holiday_year { get; set; }

        public string startdatehalfday { get; set; }
        public string enddatehalfday { get; set; }
        public double noofdays { get; set; }


    }
}