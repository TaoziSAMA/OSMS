using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Oil.Models;
using Oil.AppCode;

namespace Oil.Controllers
{
    public class QuitApprovalController : Controller
    {
        OSMS db = new OSMS();
        Staff user = System.Web.HttpContext.Current.Session["userinfo"] as Staff;
        // GET: QuitApproval
        public ActionResult Index()
        {
            var baseCtrler = DependencyResolver.Current.GetService<BaseController>();
            if (baseCtrler.CheckResources("ManagerDailyLeaveOffice_PassThrough"))
            {
                ViewBag.adopt = true;
            }
            if (baseCtrler.CheckResources("ManagerDailyLeaveOffice_TurnDown"))
            {
                ViewBag.reject = true;
            }
            if (baseCtrler.CheckResources("ManagerDailyLeaveOffice_ViewProcess"))
            {
                ViewBag.processView = true;
            }
            return View();
        }

        //查询列表
        [CheckResourcesFilter(ResourcesName = "ManagerDailyLeaveOffice")]
        public JsonResult GetList(LeaveOffice info, string SelApplyDate, string isExecute, int page, int limit)
        {
            if (!string.IsNullOrEmpty(SelApplyDate))
            {
                string[] date = SelApplyDate.Replace(" ", "").Split('~');
                info.CreateTime = Convert.ToDateTime(date[0]);
                info.UpdateTime = Convert.ToDateTime(date[1]);
            }
            //查找等待本用户审核的数据
            List<ProcessStepRecord> _data = db.ProcessStepRecord.Where(x => x.WaitForExecutionStaffId == user.Id).ToList();
            List<Guid> infoid = new List<Guid>();
            if (_data.Count > 0)
            {
                foreach (var item in _data)
                {
                    infoid.Add(item.RefOrderId);
                }
            }

            PageItem<View_LeaveOfficeJ> viewdata = Help.Page(page, limit, db.View_LeaveOfficeJ.Where(x =>
              x.IsDel == false &
              x.No.Contains(info.No == null ? x.No : info.No) &
              infoid.Contains(x.Id) & info.CreateTime == null ? true : (x.CreateTime >= info.CreateTime &
              x.CreateTime <= info.UpdateTime)).OrderBy(x => x.Id));//当前审批人的全部信息

            foreach (View_LeaveOfficeJ item in viewdata.data)
            {
                item.NeedExec = Help.GetExec(user.Id, item.Id, db);
                item.Discrible = Help.GetDiscrible(item.Id, db);
            }
            if (isExecute == "1")
            {
                viewdata.data = viewdata.data.Where(x => x.NeedExec == "false").ToList();
            }
            else if (isExecute == "0")
            {
                viewdata.data = viewdata.data.Where(x => x.NeedExec == "true").ToList();
            }
            PageItem<View_LeaveOfficeJ> data= viewdata;
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        //通过或者驳回
        public ActionResult AdoptOrReject(LeaveOffice info, string type)
        {
            ProcessStepRecord data = db.ProcessStepRecord.Where(x => x.RefOrderId == info.Id && x.WaitForExecutionStaffId == user.Id).First();
            ViewBag.type = type;
            return View(data);
        }

        //保存
        public JsonResult Save(ProcessStepRecord info, string type)
        {
            var baseCtrler = DependencyResolver.Current.GetService<BaseController>();
            try
            {
                ProcessStepRecord currentInfo = db.ProcessStepRecord.Where(x => x.Id == info.Id).First();
                currentInfo.WhetherToExecute = true;
                if (type == "adopt")
                {
                    if (baseCtrler.CheckResources("ManagerDailyLeaveOffice_PassThrough"))
                    {
                        if (currentInfo.ExecuteMethod == "UpdateStaffStatus")
                        {
                            new Execution().UpdateStaffStatus(currentInfo);
                        }

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
                        baseCtrler.SJson("false");
                    }
                }
                else if (type == "reject")
                {
                    if (baseCtrler.CheckResources("ManagerDailyEntryManager_TurnDown"))
                    {
                        int StepOrder=0;
                        if (currentInfo.StepOrder != 0)
                        {
                            StepOrder = currentInfo.StepOrder - 1;
                        }
                        
                        ProcessStepRecord infoUp = db.ProcessStepRecord.Where(x => x.RefOrderId == currentInfo.RefOrderId & x.StepOrder == StepOrder).First();

                        //List<ProcessStepRecord> infoDown = db.ProcessStepRecord.Where(x => x.RefOrderId == currentInfo.RefOrderId & x.StepOrder > currentInfo.StepOrder).ToList();
                        //foreach (ProcessStepRecord item in infoDown)
                        //{
                        //    item.WhetherToExecute = true;
                        //    item.Result = false;
                        //}
                        // Staff staInfo = Stabll.Get(x => x.Id == infoUp.WaitForExecutionStaffId).First();
                        // SendMailUseGmail(infoUp.No, info.RecordRemarks, staInfo.Email);//邮件通知

                        //infoUp.WhetherToExecute = false;
                        //infoUp.RecordRemarks = info.RecordRemarks;

                        ProcessStepRecord data = db.ProcessStepRecord.Where(x => x.Id == currentInfo.Id).OrderByDescending(x => x.StepOrder).First();
                        data.UpdateTime = DateTime.Now; //创建时间
                        data.WhetherToExecute = currentInfo.WhetherToExecute;
                        data.RecordRemarks = info.RecordRemarks;
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
                        else if (data.WhetherToExecute == true)
                        {
                            data.Discrible = data.Discrible.Replace("=>", "+").Split('+').First();
                            data.Discrible = data.Discrible + "=>审批驳回";
                        }
                        db.SaveChanges();//保存
                    }
                    else
                    {
                        baseCtrler.SJson("false");
                    }
                }
                return baseCtrler.SJson("true");
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
        [CheckResourcesFilter(ResourcesName = "ManagerDailyEntryManager_ViewProcess")]
        public JsonResult GetProcessInfo(Entry info)
        {
            PageItem<ProcessStepRecord> data = new PageItem<ProcessStepRecord>();
            data.data = db.ProcessStepRecord.Where(x => x.RefOrderId == info.Id).OrderBy(x => x.StepOrder).ToList();
            return Json(data, JsonRequestBehavior.AllowGet);
        }
    }
}