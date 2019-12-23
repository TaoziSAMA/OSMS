using Oil.AppCode;
using Oil.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

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

        //保存
        public JsonResult Save(Approver info,string type)
        {
            var baseCtrler = DependencyResolver.Current.GetService<BaseController>();
            try
            {
                bool check = true;
                List<Approver> redata = db.Approver.Where(x => x.JobCode == info.JobCode).ToList();
                if (check & info.ExecuteMethod != null)
                {
                    redata = db.Approver.Where(x => x.ExecuteMethod == info.ExecuteMethod).ToList();
                    if (redata.Count > 0)
                    {
                        if (!(type == "Edit" && info.Id == redata[0].Id))
                        {
                            check = false;
                            return baseCtrler.FJson("执行方法Code已存在");
                        }
                    }
                }
                if (check)
                { 
                    if(type=="Add")
                    {
                        if (baseCtrler.CheckResources("SystemSettingProcessSetting_Add"))
                        {
                            info.Id = Guid.NewGuid();
                            Approver data = db.Approver.Where(x => x.ProcessItemId == info.ProcessItemId).OrderByDescending(x => x.Order).First();
                            info.Order = data.Order+1;
                            db.Approver.Add(info);
                            db.SaveChanges();
                        }
                    }
                    if (type == "Edit")
                    {
                        if (baseCtrler.CheckResources("SystemSettingProcessSetting_Update"))
                        {
                            Approver data = db.Approver.Where(x => x.Id == info.Id).FirstOrDefault();
                            data.JobCode = info.JobCode;
                            data.AreaLeve = info.AreaLeve;
                            data.Discrible = info.Discrible;
                            data.ExecuteMethod = info.ExecuteMethod;
                            db.SaveChanges();
                        }
                    }
                }
                return baseCtrler.SJson("true");
            }
            catch (Exception e) { return baseCtrler.FJson(e.Message); }
        }

        [CheckResourcesFilter(ResourcesName = "SystemSettingProcessSetting_Delete")]
        public JsonResult Del(Approver info)
        {
            var baseCtrler = DependencyResolver.Current.GetService<BaseController>();
            try
            {
                List<Approver> data = db.Approver.Where(x => x.ProcessItemId == info.ProcessItemId & x.Order > info.Order).ToList();//找出发起顺序比删除项高的数据
                if (data.Count > 0)
                {
                    foreach (Approver item in data)
                    {
                        Approver update = db.Approver.Where(x => x.Id == item.Id).FirstOrDefault();
                        update.Order--;//依次给删除项补位
                    }
                }
                db.Approver.Attach(info);//删除选中项
                db.Approver.Remove(info);
                db.SaveChanges();
                return baseCtrler.SJson("true");
            }
            catch (Exception e) { return baseCtrler.FJson(e.Message); }
        }

        public JsonResult Move(Approver info,string type)
        {
            var baseCtrler = DependencyResolver.Current.GetService<BaseController>();
            try
            {
                if (type == "up")
                {
                    if (baseCtrler.CheckResources("SystemSettingProcessSetting_MoveUp"))
                    {
                        Approver data = db.Approver.Where(x => x.Id == info.Id).FirstOrDefault();
                        data.Order -= 1;
                        Approver datas = db.Approver.FirstOrDefault(x => x.ProcessItemId == info.ProcessItemId & x.Order == data.Order);
                        datas.Order += 1;
                        db.SaveChanges();

                    }
                }
                if (type == "down")
                {
                    if (baseCtrler.CheckResources("SystemSettingProcessSetting_MoveDown"))
                    {
                        Approver data = db.Approver.Where(x => x.Id == info.Id).FirstOrDefault();
                        data.Order += 1;
                        Approver datas = db.Approver.FirstOrDefault(x => x.ProcessItemId == info.ProcessItemId & x.Order == data.Order);
                        datas.Order -= 1;
                        db.SaveChanges();

                    }
                }
                return baseCtrler.SJson("true");
            }
            catch (Exception e) { return baseCtrler.FJson(e.Message); }
        }
    }
}