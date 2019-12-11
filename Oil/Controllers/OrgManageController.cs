using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Oil.Models;
using System.Web.Mvc;
using Oil.AppCode;

namespace Oil.Controllers
{
    public class OrgManageController : Controller
    {
        OSMS db = new OSMS();
        // GET: OrgManage
        public ActionResult Index()
        {
            var baseCtrler = DependencyResolver.Current.GetService<BaseController>();
            if (baseCtrler.CheckResources("OrganizationStructureManger_Add"))
            {
                ViewBag.add = true;
            }
            if (baseCtrler.CheckResources("OrganizationStructureManger_Delete"))
            {
                ViewBag.del = true;
            }
            if (baseCtrler.CheckResources("OrganizationStructureManger_Update"))
            {
                ViewBag.edit = true;
            }
            return View();
        }

        [CheckResourcesFilter(ResourcesName = "OrganizationStructureManger")]
        public JsonResult GetList(OrganizationStructure info)
        {
            PageItem<OrganizationStructure> data = new PageItem<OrganizationStructure>();
            data.data = db.OrganizationStructure.Where(x => true).Where(x => x.IsDel == false).ToList();
            data.msg = "ok";
            return Json(data, JsonRequestBehavior.AllowGet);
        }
    }
}