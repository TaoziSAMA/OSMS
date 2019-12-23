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

        //添加或者修改
        public ActionResult AddOrEdit(Models.Job info)
        {
            Models.Job infoModel = new Models.Job();
            ViewBag.type = "Add";
            if (info.Id != new Guid())//判断是否存在
            {
                infoModel = db.Job.Where(x => x.Id == info.Id).First();
                ViewBag.type = "Edit";
            }
            return View(infoModel);
        }

        //保存
        public JsonResult Save(Models.Job info, string type)
        {
            var baseCtrler = DependencyResolver.Current.GetService<BaseController>();
            try
            {
                bool check = true;
                List<Models.Job> redata = db.Job.Where(x => x.Name == info.Name).ToList();
                if (redata.Count > 0)
                {
                    if (!(type == "Edit" && info.Id == redata[0].Id))
                    {
                        check = false;
                        return baseCtrler.FJson("职位名称已存在");
                    }
                }
                if (check)
                {
                    redata = db.Job.Where(x=>x.Code==info.Code).ToList();
                    if (redata.Count > 0)
                    {
                        if (!(type == "Edit" && info.Id == redata[0].Id))
                        {
                            check = false;
                            return baseCtrler.FJson("职位代码已存在");
                        }
                    }
                }
                if (check)
                {
                    if (type == "Add")
                    {
                        if (baseCtrler.CheckResources("JobManager_Add"))
                        {
                            info.Id = Guid.NewGuid();
                            info.CreateTime = DateTime.Now;
                            info.IsDel = false;
                            db.Job.Add(info);
                            db.SaveChanges();
                        }
                        else { return baseCtrler.FJson("非法操作"); }
                    }
                    else if (type == "Edit")
                    {
                        if (baseCtrler.CheckResources("JobManager_Update"))
                        {
                            Models.Job jobInfo = db.Job.Where(x => x.Id == info.Id).FirstOrDefault();
                            jobInfo.Name = info.Name;
                            jobInfo.Code = info.Code;
                            jobInfo.UpdateTime = DateTime.Now;
                            db.SaveChanges();
                        }
                    }
                }
                return baseCtrler.SJson("true");
            }
            catch (Exception e) { return baseCtrler.FJson(e.Message); }
        }


        //删除
        [CheckResourcesFilter(ResourcesName = "JobManager_Delete")]
        public JsonResult Del(Models.Job info)
        {
            var baseCtrler = DependencyResolver.Current.GetService<BaseController>();
            try
            {
                Models.Job data = db.Job.FirstOrDefault(x => x.Id == info.Id);
                data.UpdateTime = DateTime.Now;
                data.IsDel = true;
                db.SaveChanges();
                return baseCtrler.SJson("true");
            }
            catch (Exception e) { return baseCtrler.FJson(e.Message); }
        }
    }
}