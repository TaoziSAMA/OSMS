using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace Oil.Controllers
{
    public class BaseController : Controller
    {
        // GET: Base
        JavaScriptSerializer js = new JavaScriptSerializer();
        //返回正常Json
        public JsonResult SJson(string Info)
        {
            return Json(new { Result = true, info = Info }, JsonRequestBehavior.AllowGet);
        }
        //返回异常Json
        public JsonResult FJson(string errorMsg)
        {
            return Json(new { Result = false, ErrorMsg = errorMsg }, JsonRequestBehavior.AllowGet);
        }
    }
}