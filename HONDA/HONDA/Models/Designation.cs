using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HONDA.Models
{
    public class Designation
    {
        public int ID { get; set; }
        public string designationname { get; set; }
        public string designationcode { get; set; }
        public string status { get; set; }
        public string effectivestartdate { get; set; }
        public string effectiveenddate { get; set; }
        public string creationdate { get; set; }
        public string createdby { get; set; }
        public string lastupdatedate { get; set; }
        public string lastupdatedby { get; set; }
        public string lastupdatelogin { get; set; }
    }
}