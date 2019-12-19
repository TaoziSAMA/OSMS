using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Oil.Models;
using Oil.AppCode;

namespace Oil.Controllers
{
    public class ProcessSettingController : Controller
    {
        OSMS db = new OSMS();
        
        // GET: ProcessSetting
        public ActionResult Index()
        {
            var baseCtrler = DependencyResolver.Current.GetService<BaseController>();
            if (baseCtrler.CheckResources("SystemSettingProcessSetting_Add"))
            {
                ViewBag.add = true;
            }
            if (baseCtrler.CheckResources("SystemSettingProcessSetting_Delete"))
            {
                ViewBag.del = true;
            }
            if (baseCtrler.CheckResources("SystemSettingProcessSetting_Update"))
            {
                ViewBag.edit = true;
            }
            if (baseCtrler.CheckResources("SystemSettingProcessSetting_MoveUp"))
            {
                ViewBag.up = true;
            }
            if (baseCtrler.CheckResources("SystemSettingProcessSetting_MoveDown"))
            {
                ViewBag.down = true;
            }


            List<ProcessItem> info = new List<ProcessItem>();
            ViewBag.ProitemList = db.ProcessItem.Where(x => true).ToList();
            return View();
        }


        //查询列表
        [CheckResourcesFilter(ResourcesName = "SystemSettingProcessSetting")]
        public JsonResult GetList(Approver info)
        {
            PageItem<Approver> data = new PageItem<Approver>();
            List<Approver> aaa = db.Approver.Where(x => x.JobCode.Contains(info.JobCode == null ? x.JobCode : info.JobCode)
             & x.AreaLeve == (info.AreaLeve == -1 ? x.AreaLeve : info.AreaLeve) & x.ProcessItemId == (info.ProcessItemId == null ? x.ProcessItemId : info.ProcessItemId)).OrderBy(x => x.Order).ToList();
            data.data = aaa;
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        //添加或者修改
        public ActionResult AddOrEdit(Approver info)
        {
            Approver infoModel = new Approver();
            ViewBag.type = "Add";
            if (info.Id != new Guid())
            {
                infoModel = db.Approver.First(x => x.Id == info.Id);
                ViewBag.type = "Edit";
            }
            infoModel.ProcessItemId = info.ProcessItemId;//如果是添加则取的下拉框选中的值，传入修改添加页面以供保存补充字段
            return View(infoModel);
        }
    }
}