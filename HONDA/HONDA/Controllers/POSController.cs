using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using HONDA.CommanFilter;
namespace HONDA.Controllers
{
    [SessionFilter]
    public class POSController : Controller
    {
        // GET: POS
        public ActionResult SaleOrderApproval()
        {
            return View();
        }

        public ActionResult SalesOrderCreation()
        {
            return View();
        }

        public ActionResult SalesOrderReturn()
        {
            return View();
        }

        public ActionResult SalesOrderList()
        {
            return View();
        }
        public ActionResult SalesOrderReturnList()
        {
            return View();
        }



    }
}