using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Oil.Models;
using Oil.AppCode;
using System.Globalization;

namespace Oil.Controllers
{
    public class OilMaterialApplyController : Controller
    {
        OSMS db = new OSMS();
        Staff user = System.Web.HttpContext.Current.Session["userinfo"] as Staff;
        // GET: OilMaterialApply
        public ActionResult Index()
        {
            var baseCtrler = DependencyResolver.Current.GetService<BaseController>();
            if(baseCtrler.CheckResources("OilStationLeaveOffice_Applay"))
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

        //查看列表
        [CheckResourcesFilter(ResourcesName = "OilMaterialOrder")]
        public JsonResult GetList(OilMaterialOrder info, string SelApplyDate, int page = 1, int limit = 10)
        {
            info.ApplyPersonId = user.Id;
            if (!string.IsNullOrEmpty(SelApplyDate))
            {
                string[] date = SelApplyDate.Replace(" ", "").Split('~');
                info.CreateTime = Convert.ToDateTime(date[0]);
                info.UpdateTime = Convert.ToDateTime(date[1]);
            }
            PageItem<View_OilMaterialOrderSP> data = Help.Page(page, limit, db.View_OilMaterialOrderSP.Where(x =>
            x.IsDel == false &
            x.No.Contains(info.No == null ? x.No : info.No) &
            x.ApplyPersonId == (info.ApplyPersonId == null ? x.ApplyPersonId : info.ApplyPersonId) &
            info.CreateTime == null ? true : (x.CreateTime >= info.CreateTime &
            x.CreateTime <= info.UpdateTime)).OrderBy(x => x.Id));
            foreach (View_OilMaterialOrderSP item in data.data)
            {
                item.Discrible = Help.GetDiscrible(item.Id, db);
            }

            return Json(data, JsonRequestBehavior.AllowGet);
        }

        //添加或者修改
        public ActionResult AddOrEdit(View_OilMaterialOrderSP info)
        {
            OilMaterialOrder infoModel = new OilMaterialOrder();
            ViewBag.type = "Add";
            ViewBag.StaffName = user.Name;
            infoModel.ApplyPersonId = user.Id;
            if (info.Id != new Guid())
            {
                infoModel = db.OilMaterialOrder.Where(x => x.Id == info.Id & x.IsDel==false).First();
                DateTime ApplyDate = new DateTime();
                ApplyDate = (DateTime)infoModel.ApplyDate;
                ViewBag.ApplyDate = ApplyDate.ToString("yyyy-MM-dd", DateTimeFormatInfo.InvariantInfo);
                ViewBag.type = "Edit";
            }
            return View(infoModel);
        }

        //获取订单有哪些油料明细
        [CheckResourcesFilter(ResourcesName = "OilMaterialOrder_Update")]
        public JsonResult GetDetail(OilMaterialOrder info, string type)
        {
            List<OilMaterialOrderDetail> data = db.OilMaterialOrderDetail.Where(x => x.OrderId == info.Id& x.IsDel==false).OrderBy(x => x.Id).ToList();
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        //保存
        public JsonResult Save(OilMaterialOrder info, string type)
        {
            var baseCtrler = DependencyResolver.Current.GetService<BaseController>();
            try
            {
                if (type == "Add")
                {
                    if (baseCtrler.CheckResources("OilMaterialOrder_Applay"))
                    {
                        info.Id = Guid.NewGuid();//设置ID
                        //先添加油料明细表
                        List<OilMaterialOrderDetail> data = new List<OilMaterialOrderDetail>();
                        
                        foreach (OilMaterialOrderDetail item in info.OilMaterialOrderDetail)
                        {
                            item.OrderId = info.Id;
                            data.Add(item);
                        }
                        info.No = Help.GetOrderNumber<OilMaterialOrder>(db, "YLSQ", "OilMaterialOrder");//获取单号
                        info.CreateTime = DateTime.Now; //创建时间
                        info.IsDel = false;//是否已删除
                        
                        db.OilMaterialOrder.Add(info);//添加油料申请
                        //最后添加 防止外键冲突
                        foreach (OilMaterialOrderDetail item in data)
                        {
                            item.Id = Guid.NewGuid();//设置ID
                            item.CreateTime = DateTime.Now; //创建时间
                            item.UpdateTime = DateTime.Now;//修改时间
                            item.IsDel = false;//是否已删除
                            db.OilMaterialOrderDetail.Add(item);
                        }
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
                        if (baseCtrler.CheckResources("OilMaterialOrder_Update"))
                        {
                            //修改明细单
                            List<OilMaterialOrderDetail> dataadd = new List<OilMaterialOrderDetail>();
                            foreach (OilMaterialOrderDetail item in info.OilMaterialOrderDetail)
                            {
                                item.OrderId = info.Id;
                                dataadd.Add(item);
                            }
                            Guid OrderId = dataadd.First().OrderId;
                            List<OilMaterialOrderDetail> data = db.OilMaterialOrderDetail.Where(x => x.IsDel == false & x.OrderId == OrderId).ToList();
                            //比较提交的明细条目与原条目的数量
                            if (dataadd.Count <= data.Count)
                            {
                                //比原条目少则把新数据覆盖旧数据，并把原条目多出的条数设为null
                                for (int i = 0; i < data.Count; i++)
                                {
                                    if (i < dataadd.Count)
                                    {
                                        data[i].OilSpec = dataadd[i].OilSpec;
                                        data[i].Volume = dataadd[i].Volume;
                                        data[i].Surplus = dataadd[i].Surplus;
                                        data[i].DayAvg = dataadd[i].DayAvg;
                                        data[i].NeedAmount = dataadd[i].NeedAmount;
                                        data[i].UpdateTime = DateTime.Now; //创建时间
                                    }
                                    else
                                    {
                                        data[i] = null;
                                    }
                                }
                            }
                            else
                            {
                                //比原条目多则把新数据覆盖旧数据，并把提交条目多出的条数进行添加操作
                                for (int i = 0; i < dataadd.Count; i++)
                                {
                                    if (i < data.Count)
                                    {
                                        data[i].OilSpec = dataadd[i].OilSpec;
                                        data[i].Volume = dataadd[i].Volume;
                                        data[i].Surplus = dataadd[i].Surplus;
                                        data[i].DayAvg = dataadd[i].DayAvg;
                                        data[i].NeedAmount = dataadd[i].NeedAmount;
                                        data[i].UpdateTime = DateTime.Now; //创建时间
                                    }
                                    else
                                    {
                                        dataadd[i].Id = Guid.NewGuid();//设置ID
                                        dataadd[i].CreateTime = DateTime.Now; //创建时间
                                        dataadd[i].UpdateTime = DateTime.Now;//修改时间
                                        dataadd[i].IsDel = false;//是否已删除
                                        db.OilMaterialOrderDetail.Add(dataadd[i]);
                                    }
                                }
                            }
                            db.SaveChanges();//保存
                            OilMaterialOrder _data = db.OilMaterialOrder.FirstOrDefault(x => x.Id == info.Id);
                            _data.UpdateTime = DateTime.Now; //创建时间
                            _data.Remark = info.Remark;
                            _data.ApplyDate = info.ApplyDate;
                            db.SaveChanges();//保存
                        }

                    }
                }
                return baseCtrler.SJson("true");
            }
            catch (Exception e) { return baseCtrler.FJson(e.Message); }
        }

        //删除
        [CheckResourcesFilter(ResourcesName = "OilMaterialOrder_Delete")]
        public JsonResult Del(OilMaterialOrder info)
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
                    //删除明细表
                    Guid OrderId = info.Id;
                    List<OilMaterialOrderDetail> detailData = db.OilMaterialOrderDetail.Where(x => x.IsDel == false & x.OrderId == OrderId).ToList();
                    foreach (OilMaterialOrderDetail item in detailData)
                    {
                        item.UpdateTime = DateTime.Now; //创建时间
                        item.IsDel = true;
                    }
                    //删除申请表
                    OilMaterialOrder orderData = db.OilMaterialOrder.FirstOrDefault(x => x.Id == info.Id);
                    orderData.UpdateTime = DateTime.Now; //创建时间
                    orderData.IsDel = true;
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
        [CheckResourcesFilter(ResourcesName = "OilMaterialOrder_Launch")]
        public JsonResult Start(OilMaterialOrder info)
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
                    //Oilbll.Start(info);
                    ProcessStepRecord currentInfo = db.ProcessStepRecord.FirstOrDefault(x => x.RefOrderId == info.Id & x.StepOrder == 0);

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
                        Help.SetProcess("OilMaterialOrder", user.Id, info.No, info.Id, db);//添加到流程
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
        [CheckResourcesFilter(ResourcesName = "OilMaterialOrder_ViewProcess")]
        public JsonResult GetProcessInfo(Entry info)
        {
            PageItem<ProcessStepRecord> data = new PageItem<ProcessStepRecord>();
            data.data = db.ProcessStepRecord.Where(x => x.RefOrderId == info.Id).OrderBy(x => x.StepOrder).ToList();
            return Json(data, JsonRequestBehavior.AllowGet);
        }
    }
}