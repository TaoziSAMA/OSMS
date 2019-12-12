using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Oil.AppCode;
using Oil.Models;

namespace Oil.Controllers
{
    public class StaffInfoManageController : Controller
    {
        OSMS db = new OSMS();
        // GET: StaffInfoManage
        public ActionResult Index()
        {
            var baseCtrler = DependencyResolver.Current.GetService<BaseController>();
            if (baseCtrler.CheckResources("StaffManager_Add"))
            {
                ViewBag.add = true;
            }
            if (baseCtrler.CheckResources("StaffManager_Update"))
            {
                ViewBag.edit = true;
            }
            return View();
        }
        //查询列表
        public JsonResult GetList(Staff info,int page,int limit)
        {
            var baseCtrler = DependencyResolver.Current.GetService<BaseController>();
            if (baseCtrler.CheckResources("StaffManager") || baseCtrler.CheckResources("OilStationLeaveOffice_Applay") || baseCtrler.CheckResources("OilStationLeaveOffice_Update"))
            {
                
                PageItem<View_StaffJ> list = Help.Page(page, limit, db.View_StaffJ.Where(x=>x.No.Contains(info.No==null?x.No:info.No) & x.Name.Contains(info.Name==null?x.Name:info.Name)& x.Status==(info.Status==null?x.Status:info.Status)).OrderBy(x=>x.Id));
                return Json(list, JsonRequestBehavior.AllowGet);
            }
            else { return baseCtrler.SJson("非法操作"); }
        }

        //添加或者删除
        public ActionResult AddOrEdit(View_StaffJ info)
        {
            Staff infoModel = new Staff();
            ViewBag.type = "Add";
            if (info.Id != new Guid())
            {
                infoModel = db.Staff.Where(x => x.Id == info.Id).First();
                DateTime BirthDay = (DateTime)infoModel.BirthDay;
                ViewBag.JobName = info.JobName;
                ViewBag.BirthDay = BirthDay.ToString("yyyy-MM-dd", DateTimeFormatInfo.InvariantInfo);
                ViewBag.OrgName = db.OrganizationStructure.Where(x => x.Id == info.OrgID).FirstOrDefault().Name;//机构名称
                ViewBag.type = "Edit";
            }
            return View(infoModel);
        }

        //保存
        public JsonResult Save(Staff info, string type)
        {
            var baseCtrler = DependencyResolver.Current.GetService<BaseController>();
            try
            {
                bool check = true;
                List<Staff> redata = db.Staff.Where(x => x.No == info.No).ToList();
                if (redata.Count > 0)
                {
                    if (!(type == "Edit" && info.Id == redata[0].Id))//判断类型是否为编辑，或者用户id相同，满足任意一项则跳过，否则提示也存在
                    {
                        check = false;
                        return baseCtrler.FJson("员工编号已存在");
                    }
                }
                if (check)
                {
                    if (type == "Add")
                    {
                        if (baseCtrler.CheckResources("StaffManager_Add"))
                        {
                            info.Id = Guid.NewGuid();
                            info.Sex = info.Sex == null ? false : true;
                            info.Password = Help.Encode("123456");//默认密码为123456 
                            info.CreateTime = DateTime.Now;
                            info.IsHSEGroup = false;//默认
                            db.Staff.Add(info);
                            db.SaveChanges();
                        }
                    }
                    else if (type == "Edit")
                    {
                        if (baseCtrler.CheckResources("StaffManager_Update"))
                        {
                            //Staffbll.Edit(info);
                            Staff data = db.Staff.FirstOrDefault(x => x.Id == info.Id);
                            data.No = info.No;
                            data.Name = info.Name;
                            data.BirthDay = info.BirthDay;
                            data.Sex = info.Sex;//性别
                            data.NativePlace = info.NativePlace;
                            data.Address = info.Address;
                            data.Tel = info.Tel;
                            data.Email = info.Email;
                            data.JobId = info.JobId;
                            data.OrgID = info.OrgID;
                            data.Status = info.Status;
                            data.UpdateTime = DateTime.Now; //创建时间
                            db.SaveChanges();//保存
                        }

                    }
                }
                return baseCtrler.SJson("true");
            }
            catch (Exception e) { return baseCtrler.FJson(e.Message); }
        }

        //打开组织页面
        public ActionResult GetOrganizationStructure()
        {
            return View();
        }

        //组织页面数据
        public JsonResult GetOrgOption()
        {
            var baseCtrler = DependencyResolver.Current.GetService<BaseController>();
            if (baseCtrler.CheckResources("StaffManager") || baseCtrler.CheckResources("OilStationDailyEntryManager_Applay") || baseCtrler.CheckResources("OilStationDailyEntryManager_Update"))
            {
                var list = (from org in db.OrganizationStructure.Where(x => true).Where(x => x.IsDel == false)
                            select new
                            {
                                Id = org.Id,
                                Name = org.Name,
                                Code = org.Code,
                                Leve = org.Leve,
                                lay_is_isChecked = true,
                                Pid = org.ParentId == null ? new Guid("{A7243447-F9AD-4A93-8C3B-464B9389EDAF}") : org.ParentId
                            }).ToList();
                return Json(new { msg = "", code = 0, data = list, count = list.ToList().Count }, JsonRequestBehavior.AllowGet);
            }
            else { return baseCtrler.SJson("非法操作"); }
        }

        //打开职位页面
        public ActionResult GetJob()
        {
            return View();
        }

        //获取职位数据
        public JsonResult GetJobList(Job info,int page,int limit)
        {
            var baseCtrler = DependencyResolver.Current.GetService<BaseController>();
            if (baseCtrler.CheckResources("StaffManager") || baseCtrler.CheckResources("OilStationDailyEntryManager_Applay") || baseCtrler.CheckResources("OilStationDailyEntryManager_Update"))
            {
                PageItem<Job> data = Help.Page(page, limit, db.Job.Where(x=>x.IsDel==false&x.Code.Contains(info.Code==null?x.Code:info.Code)&x.Name.Contains(info.Name==null?x.Name:info.Name)).OrderBy(x=>x.Id));
                return Json(data, JsonRequestBehavior.AllowGet);
            }
            else { return baseCtrler.SJson("非法操作"); }
        }
    }
}