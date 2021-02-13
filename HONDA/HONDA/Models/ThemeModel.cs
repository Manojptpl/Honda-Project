using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HONDA.Models
{
    public class ThemeModel
    {
        public int THEME_ID { get; set; }
        public string COMPANY_NAME { get; set; }
        public string LOGO { get; set; }
        public string FAVICON { get; set; }
        public string SuccessMsg { set; get; }
        public string ErrorMsg { set; get; }
    }
}