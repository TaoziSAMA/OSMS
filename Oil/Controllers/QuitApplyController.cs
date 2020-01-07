using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Oil.Models;
using Oil.AppCode;
using System.Globalization;

namespace Oil.Controllers
{
    public class QuitApplyController : Controller
    {
        OSMS db = new OSMS();
        Staff user = System.Web.HttpContext.Current.Session["userinfo"] as Staff;
        // GET: QuitApply
        public ActionResult Index()
        {
            var baseCtrler = DependencyResolver.Current.GetService<BaseController>();
            if (baseCtrler.CheckResources("OilStationLeaveOffice_Applay"))
            {
                ViewBag.add = true;
            }
            if (baseCtrler.CheckResources("OilStationLeaveOffice_Delete"))
            {
                ViewBag.del = true;
            }
            if (baseCtrler.CheckResources("OilStationLeaveOffice_Update"))
            {
                ViewBag.edit = true;
            }
            if (baseCtrler.CheckResources("OilStationLeaveOffice_Launch"))
            {
                ViewBag.start = true;
            }
            if (baseCtrler.CheckResources("OilStationLeaveOffice_ViewProcess"))
            {
                ViewBag.processView = true;
            }
            return View();
        }

        //获取列表
        [CheckResourcesFilter(ResourcesName = "OilStationLeaveOffice")]
        public JsonResult GetList(LeaveOffice info,string SelApplyDate,int page,int limit)
        {
            info.ApplyPersonId = user.Id;
            if (!string.IsNullOrEmpty(SelApplyDate))
            {
                string[] date = SelApplyDate.Replace(" ", "").Split('~');
                info.CreateTime = Convert.ToDateTime(date[0]);
                info.UpdateTime = Convert.ToDateTime(date[1]);
            }
            PageItem<View_LeaveOfficeJ> data =Help.Page(page, limit, db.View_LeaveOfficeJ.Where(x =>
            x.IsDel == false &
            x.No.Contains(info.No == null ? x.No : info.No) &
            x.ApplyPersonId == (info.ApplyPersonId == null ? x.ApplyPersonId : info.ApplyPersonId) &
            info.CreateTime == null ? true : (x.CreateTime >= info.CreateTime &
            x.CreateTime <= info.UpdateTime)).OrderBy(x => x.Id));
            foreach (View_LeaveOfficeJ item in data.data)
            {
                item.Discrible = Help.GetDiscrible(item.Id, db);
            }
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        //添加或者修改
        public ActionResult AddOrEdit(LeaveOffice info)
        {
            LeaveOffice infoModel = new LeaveOffice();
            ViewBag.type = "Add";
            ViewBag.Staff_Name = user.Name;
            ViewBag.Job_Name= db.Job.Where(x => x.Id == user.JobId).Where(x => x.IsDel == false).FirstOrDefault().Name;
            infoModel.ApplyPersonId = user.Id;
            if (info.Id != new Guid())
            {
                infoModel = db.LeaveOffice.Where(x => x.Id == info.Id& x.IsDel==false).First();
                DateTime ApplyDate = new DateTime();
                ApplyDate = (DateTime)infoModel.ApplyDate;
                ViewBag.ApplyDate = ApplyDate.ToString("yyyy-MM-dd", DateTimeFormatInfo.InvariantInfo);
                ViewBag.HandoverSatffName = db.Staff.Where(x => x.Id == info.HandoverSatffId).First().Name;
                ViewBag.type = "Edit";
            }
            return View(infoModel);
        }


        //保存
        public JsonResult Save(LeaveOffice info, string type)
        {
            var baseCtrler = DependencyResolver.Current.GetService<BaseController>();
            try
            {
                if (type == "Add")
                {
                    if (baseCtrler.CheckResources("OilStationLeaveOffice_Applay"))
                    {
                        info.StaffName = user.Name;
                        info.JobId = (Guid)user.JobId;
                        info.Id = Guid.NewGuid();//设置ID
                        info.No = Help.GetOrderNumber<LeaveOffice>(db, "LZSQ", "LeaveOffice");//获取单号
                        info.CreateTime = DateTime.Now; //创建时间
                        info.IsDel = false;//是否已删除
                        db.LeaveOffice.Add(info);
                        db.SaveChanges();
                    }
                }
                else if (type == "Edit")
                {
                    if (baseCtrler.IsStart(info.Id))
                    {
                        return baseCtrler.FJson("已经发起无法修改");
                    }
                    else
                    {
                        if (baseCtrler.CheckResources("OilStationLeaveOffice_Update"))
                        {
                            LeaveOffice data = db.LeaveOffice.FirstOrDefault(x => x.Id == info.Id);
                            data.LeaveType = info.LeaveType;
                            data.ApplyDate = info.ApplyDate;
                            data.Reason = info.Reason;
                            data.ExplanationHandover = info.ExplanationHandover;
                            data.HandoverSatffId = info.HandoverSatffId;
                            data.UpdateTime = DateTime.Now; //创建时间
                            db.SaveChanges();//保存
                        }

                    }
                }
                return baseCtrler.SJson("true");
            }
            catch (Exception e) { return baseCtrler.FJson(e.Message); }
        }

        //打开交接人
        public ActionResult GetHandoverSatff()
        {
            return View();
        }

        //删除
        [CheckResourcesFilter(ResourcesName = "OilStationLeaveOffice_Delete")]
        public JsonResult Del(LeaveOffice info)
        {
            var baseCtrler = DependencyResolver.Current.GetService<BaseController>();
            try
            {
                if (baseCtrler.IsStart(info.Id))
                {
                    return baseCtrler.FJson("已经发起无法删除");
                }
                else
                {
                    //删除申请表
                    LeaveOffice data = db.LeaveOffice.FirstOrDefault(x => x.Id == info.Id);
                    data.UpdateTime = DateTime.Now; //创建时间
                    data.IsDel = true;
                    db.SaveChanges();//保存

                    //删除流程表
                    List<ProcessStepRecord> delInfo = db.ProcessStepRecord.Where(x => x.RefOrderId == info.Id).ToList();
                    foreach (var item in delInfo)
                    {
                        db.ProcessStepRecord.Attach(item);//将对象添加到EF管理容器中 ObjectStateManager
                        db.ProcessStepRecord.Remove(item);//将对象包装类状态标识为删除
                    }
                    db.SaveChanges();//保存
                    return baseCtrler.SJson("true");
                }
            }
            catch (Exception e) { return baseCtrler.FJson(e.Message); }
        }

        //发起
        [CheckResourcesFilter(ResourcesName = "OilStationLeaveOffice_Launch")]
        public JsonResult Start(LeaveOffice info)
        {
            var baseCtrler = DependencyResolver.Current.GetService<BaseController>();
            try
            {
                if (baseCtrler.IsStart(info.Id))
                {
                    return baseCtrler.FJson("已经发起无法再次发起");

                }
                else
                {
                    ProcessStepRecord currentInfo = db.ProcessStepRecord.Where(x => x.RefOrderId == info.Id & x.StepOrder == 0).FirstOrDefault();
                    if (currentInfo != null)
                    {
                        currentInfo.WhetherToExecute = true;
                        currentInfo.Result = true;
                        ProcessStepRecord data = db.ProcessStepRecord.Where(x => x.Id == info.Id).OrderByDescending(x => x.StepOrder).First();
                        data.UpdateTime = DateTime.Now; //创建时间
                        data.WhetherToExecute = currentInfo.WhetherToExecute;
                        data.RecordRemarks = currentInfo.RecordRemarks;
                        data.Result = currentInfo.Result;
                        if (data.WhetherToExecute == true & data.Result == true)
                        {
                            if (data.StepOrder == 0)
                            {
                                data.Discrible = data.Discrible + "=>已发起";
                            }
                            else
                            {
                                data.Discrible = data.Discrible + "=>已审批通过";
                            }
                        }
                        else if (data.WhetherToExecute == false)
                        {
                            data.Discrible = data.Discrible.Replace("=>", "+").Split('+').First();
                        }
                        db.SaveChanges();//保存
                    }
                    else
                    {
                        Help.SetProcess("LeaveOffice", info.ApplyPersonId, info.No, info.Id, db);//添加到流程
                    }
                    return baseCtrler.SJson("true");
                }

            }
            catch (Exception e) { return baseCtrler.FJson(e.Message); }
        }


        //流程视图
        public ActionResult ProcessInfo(Entry info)
        {
            ViewBag.No = null;
            if (db.ProcessStepRecord.Where(x => x.RefOrderId == info.Id).ToList().Count > 0)
            {
                ViewBag.No = info.No;
            }
            else
            {
                ViewBag.No = "未发起";
            }
            ViewBag.Id = info.Id;
            return View();
        }
        //流程视图返回数据
        [CheckResourcesFilter(ResourcesName = "ManagerDailyLeaveOffice_ViewProcess")]
        public JsonResult GetProcessInfo(Entry info)
        {
            PageItem<ProcessStepRecord> data = new PageItem<ProcessStepRecord>();
            data.data = db.ProcessStepRecord.Where(x => x.RefOrderId == info.Id).OrderBy(x => x.StepOrder).ToList();
            return Json(data, JsonRequestBehavior.AllowGet);
        }
    }
}