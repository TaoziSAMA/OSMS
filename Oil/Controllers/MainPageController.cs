using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Oil.Controllers
{
    public class MainPageController : Controller
    {
        // GET: MainPage
        public ActionResult Main()
        {
            return View();
        }
    }
}