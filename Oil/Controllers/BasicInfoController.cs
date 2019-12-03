using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Oil.AppCode;
using System.Web.Security;

namespace Oil.Controllers
{
    public class BasicInfoController : Controller
    {
        Models.OSMS db = new Models.OSMS();
        
        // GET: BasicInfo
        public ActionResult JobIndex()
        {
            var baseCtrler = DependencyResolver.Current.GetService<BaseController>();
            if (baseCtrler.CheckResources("JobManager_Add"))
            {
                ViewBag.add = true;
            }
            if (baseCtrler.CheckResources("JobManager_Delete"))
            {
                ViewBag.del = true;
            }
            if (baseCtrler.CheckResources("JobManager_Update"))
            {
                ViewBag.edit = true;
            }
            return View();
        }

        /// <summary>
        /// 查询列表,
        /// </summary>
        /// <returns></returns>
        [CheckResourcesFilter(ResourcesName = "JobManager")]
        public JsonResult GetJobList(Models.Job info, int page, int limit)
        {
            //调用Help类的分页方法获取分页数据，支持模糊查询
            PageItem<Models.Job> jobList= Help.Page(page, limit, db.Job.Where(x => x.IsDel == false & x.Code.Contains(info.Code == null ? x.Code : info.Code)
             & x.Name.Contains(info.Name == null ? x.Name : info.Name)).OrderBy(x => x.Id));
            return Json(jobList, JsonRequestBehavior.AllowGet);
        }
    }
}