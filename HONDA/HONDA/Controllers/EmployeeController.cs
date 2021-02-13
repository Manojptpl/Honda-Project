using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HONDA.Controllers
{
    public class EmployeeController : Controller
    {
        // GET: Employee
        public ActionResult TimeSheetCalendar()
        {
            return View(new EventViewModel());
        }
    }
}