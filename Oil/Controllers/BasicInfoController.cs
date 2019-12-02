using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Oil.Controllers
{
    public class BasicInfoController : Controller
    {
        // GET: BasicInfo
        public ActionResult JobIndex()
        {
            return View();
        }
    }
}