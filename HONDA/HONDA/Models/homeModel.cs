using System;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using HONDA.Models;

namespace HONDA.Models
{
    public class homeModel
    {
        public string User_Name { set; get; }
        public string SuccessMsg { set; get; }
        public string ErrorMsg { set; get; }
    }
}