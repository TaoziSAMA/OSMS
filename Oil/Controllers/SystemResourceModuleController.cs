using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Oil.AppCode;
using Oil.Models;

namespace Oil.Controllers
{
    public class SystemResourceModuleController : Controller
    {
        OSMS db = new OSMS();
        // GET: SystemResourceModule
        public ActionResult Index()
        {
            return View();
        }

        //返回列表
        [CheckResourcesFilter(ResourcesName = "SystemSettingSystemResourceModuleList")]
        public JsonResult GetList()
        {
            PageItem<SystemResourceModule> data = new PageItem<SystemResourceModule>();
            data.data = db.SystemResourceModule.Where(x => true).ToList();
            return Json(data, JsonRequestBehavior.AllowGet);
        }
    }
}