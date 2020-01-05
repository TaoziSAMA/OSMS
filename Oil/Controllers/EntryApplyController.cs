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
    public class EntryApplyController : Controller
    {
        OSMS db = new OSMS();
        // GET: EntryApply
        public ActionResult Index()
        {
            var baseCtrler = DependencyResolver.Current.GetService<BaseController>();
            if (baseCtrler.CheckResources("OilStationDailyEntryManager_Applay"))
            {
                ViewBag.add = true;
            }
            if (baseCtrler.CheckResources("OilStationDailyEntryManager_Delete"))
            {
                ViewBag.del = true;
            }
            if (baseCtrler.CheckResources("OilStationDailyEntryManager_Update"))
            {
                ViewBag.edit = true;
            }
            if (baseCtrler.CheckResources("OilStationDailyEntryManager_Launch"))
            {
                ViewBag.start = true;
            }
            if (baseCtrler.CheckResources("OilStationDailyEntryManager_ViewProcess"))
            {
                ViewBag.processView = true;
            }

            return View();
        }


        //获取列表
        [CheckResourcesFilter(ResourcesName = "OilStationDailyEntryManager")]
        public JsonResult GetList(Entry info, string SelApplyDate, int page, int limit)
        {
            //范围时间字符串裁切
            if (!string.IsNullOrEmpty(SelApplyDate))
            {
                string[] date = SelApplyDate.Replace(" ", "").Split('~');
                info.CreateTime = Convert.ToDateTime(date[0]);
                info.UpdateTime = Convert.ToDateTime(date[1]);
            }
            //根据no和createtime来模糊查询
            PageItem<View_Entry> view_data = Help.Page(page, limit, db.View_Entry.Where(x =>
                x.IsDel == false &
                x.No.Contains(info.No == null ? x.No : info.No) &
                info.CreateTime == null ? true : (x.CreateTime >= info.CreateTime & x.CreateTime <= info.UpdateTime)
            ).OrderBy(x => x.Id));
            foreach(View_Entry item in view_data.data)
            {
                item.Discrible = Help.GetDiscrible(item.Id, db);
            }
            PageItem<View_Entry> data = view_data;
            return Json(data, JsonRequestBehavior.AllowGet);
        }
        //选择职位
        public ActionResult GetJob()
        {
            return View();
        }
        //选择机构
        public ActionResult GetOrganizationStructure()
        {
            return View();
        }

        //添加和修改
        public ActionResult AddOrEdit(Entry info)
        {
            Staff user = Session["userInfo"] as Staff;
            Entry infoModel = new Entry();
            ViewBag.type = "Add";
            ViewBag.StaffName = user.Name;
            if (info.Id != new Guid())
            {
                infoModel = db.Entry.Where(x => x.IsDel == false).FirstOrDefault(x => x.Id == info.Id);
                if (infoModel.BirthDay != null)
                    ViewBag.BirthDay = ((DateTime)infoModel.BirthDay).ToString("yyyy-MM-dd", DateTimeFormatInfo.InvariantInfo);//转换出生日期格式
                if (infoModel.EntryDate != null)
                    ViewBag.EntryDate = ((DateTime)infoModel.EntryDate).ToString("yyyy-MM-dd", DateTimeFormatInfo.InvariantInfo);//转换入职日期
                if(infoModel.EducationalExperience1StartDate!=null)
                    ViewBag.EducationalExperience1Date = ((DateTime)infoModel.EducationalExperience1StartDate).ToString("yyyy-MM-dd", DateTimeFormatInfo.InvariantInfo) + " ~ " + ((DateTime)infoModel.EducationalExperience1EndDate).ToString("yyyy-MM-dd", DateTimeFormatInfo.InvariantInfo);
                if (infoModel.EducationalExperience2StartDate != null)
                    ViewBag.EducationalExperience2Date = ((DateTime)infoModel.EducationalExperience2StartDate).ToString("yyyy-MM-dd", DateTimeFormatInfo.InvariantInfo) + " ~ " + ((DateTime)infoModel.EducationalExperience2EndDate).ToString("yyyy-MM-dd", DateTimeFormatInfo.InvariantInfo);
                if (infoModel.EducationalExperience3StartDate != null)
                    ViewBag.EducationalExperience3Date = ((DateTime)infoModel.EducationalExperience3StartDate).ToString("yyyy-MM-dd", DateTimeFormatInfo.InvariantInfo) + " ~ " + ((DateTime)infoModel.EducationalExperience3EndDate).ToString("yyyy-MM-dd", DateTimeFormatInfo.InvariantInfo);
                if (infoModel.EducationalExperience4StartDate != null)
                    ViewBag.EducationalExperience4Date = ((DateTime)infoModel.EducationalExperience4StartDate).ToString("yyyy-MM-dd", DateTimeFormatInfo.InvariantInfo) + " ~ " + ((DateTime)infoModel.EducationalExperience4EndDate).ToString("yyyy-MM-dd", DateTimeFormatInfo.InvariantInfo);
                List<OrganizationStructure> orgdata = db.OrganizationStructure.Where(x => x.Id == infoModel.Organization_Id & x.IsDel==false).ToList();
                if (orgdata.Count > 0)
                {
                    ViewBag.Organization_Name = orgdata.FirstOrDefault().Name;
                }
                List<Job> jdata = db.Job.Where(x => x.Id == infoModel.JobId & x.IsDel==false).ToList();
                if (jdata.Count > 0)
                {
                    ViewBag.JobName = jdata.FirstOrDefault().Name;
                }
                ViewBag.type = "Edit";
            }
            return View(infoModel);
        }

        //保存
        public JsonResult Save(Entry info,string type, string EducationalExperience1Date, string EducationalExperience2Date, string EducationalExperience3Date, string EducationalExperience4Date)
        {
            Staff user = Session["userInfo"] as Staff;
            var baseCtrler = DependencyResolver.Current.GetService<BaseController>();
            try
            {
                if (!string.IsNullOrEmpty(EducationalExperience1Date))
                {
                    string[] date = EducationalExperience1Date.Replace(" ", "").Split('~');
                    info.EducationalExperience1StartDate = Convert.ToDateTime(date[0]);
                    info.EducationalExperience1EndDate = Convert.ToDateTime(date[1]);
                }
                if (!string.IsNullOrEmpty(EducationalExperience2Date))
                {
                    string[] date = EducationalExperience2Date.Replace(" ", "").Split('~');
                    info.EducationalExperience2StartDate = Convert.ToDateTime(date[0]);
                    info.EducationalExperience2EndDate = Convert.ToDateTime(date[1]);
                }
                if (!string.IsNullOrEmpty(EducationalExperience3Date))
                {
                    string[] date = EducationalExperience3Date.Replace(" ", "").Split('~');
                    info.EducationalExperience3StartDate = Convert.ToDateTime(date[0]);
                    info.EducationalExperience3EndDate = Convert.ToDateTime(date[1]);
                }
                if (!string.IsNullOrEmpty(EducationalExperience4Date))
                {
                    string[] date = EducationalExperience4Date.Replace(" ", "").Split('~');
                    info.EducationalExperience4StartDate = Convert.ToDateTime(date[0]);
                    info.EducationalExperience4EndDate = Convert.ToDateTime(date[1]);
                }
                if (type == "Add")
                {
                    if (baseCtrler.CheckResources("OilStationDailyEntryManager_Applay"))
                    {
                        info.Id = Guid.NewGuid();
                        info.No=Help.GetOrderNumber<Entry>(db, "RZSQ", "Entry");//获取单号
                        info.CreateTime = DateTime.Now;
                        info.IsDel = false;
                        info.CreateStaffeId = user.Id;
                        db.Entry.Add(info);
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
                        if (baseCtrler.CheckResources("OilStationDailyEntryManager_Update"))
                        {
                            Entry data = db.Entry.FirstOrDefault(x => x.Id == info.Id);
                            data.StaffName = info.StaffName;  //员工姓名
                            data.Sex = info.Sex;  //性别
                            data.BirthDay = info.BirthDay;  //出生日期
                            data.MaritalStatus = info.MaritalStatus;  //婚姻状况
                            data.Height = info.Height;  //身高
                            data.HighestEducation = info.HighestEducation;  //最高学历
                            data.Major = info.Major;  //专业
                            data.ForeginLanguageAptitude = info.ForeginLanguageAptitude;  //外语能力
                            data.IDNumber = info.IDNumber;  //生份证号
                            data.NativePlace = info.NativePlace;  //籍贯
                            data.Address = info.Address;  //详细地址
                            data.Email = info.Email;  //邮件
                            data.Tel = info.Tel;  //联系电话
                            data.HaseMedicalHistory = info.HaseMedicalHistory;  //是否有传染病史
                            data.MedicalHistoryComment = info.MedicalHistoryComment;  //传染病史备注
                            data.HobbiesAndSpeciality = info.HobbiesAndSpeciality;  //兴趣爱好及特长
                            data.EducationalExperience1StartDate = info.EducationalExperience1StartDate;  //教育?经历（高中/中专）开始日期
                            data.EducationalExperience1EndDate = info.EducationalExperience1EndDate;  //教育?经历（高中/中专）结束日期
                            data.EducationalExperience1SchoolName = info.EducationalExperience1SchoolName;  //教育?经历（高中/中专）学校名
                            data.EducationalExperience1Major = info.EducationalExperience1Major;  //教育?经历（高中/中专）学历
                            data.EducationalExperience1Certificate = info.EducationalExperience1Certificate;  //教育?经历（高中/中专）证书
                            data.EducationalExperience2StartDate = info.EducationalExperience2StartDate;  //教育?经历（大专/本科）开始日期
                            data.EducationalExperience2EndDate = info.EducationalExperience2EndDate;  //教育?经历（大专/本科）结束日期
                            data.EducationalExperience2SchoolName = info.EducationalExperience2SchoolName;  //教育?经历（大专/本科）学校名
                            data.EducationalExperience2Major = info.EducationalExperience2Major;  //教育?经历（大专/本科）专业
                            data.EducationalExperience2Certificate = info.EducationalExperience2Certificate;  //教育?经历（大专/本科）证书
                            data.EducationalExperience3StartDate = info.EducationalExperience3StartDate;  //教育?经历（研究生）开始日期
                            data.EducationalExperience3EndDate = info.EducationalExperience3EndDate;  //教育?经历（研究生）开始结束日期
                            data.EducationalExperience3SchoolName = info.EducationalExperience3SchoolName;  //教育?经历（研究生）学校名
                            data.EducationalExperience3Major = info.EducationalExperience3Major;  //教育?经历（研究生）专业
                            data.EducationalExperience3Certificate = info.EducationalExperience3Certificate;  //教育?经历（研究生）证书
                            data.EducationalExperience4StartDate = info.EducationalExperience4StartDate;  //教育?经历（其它）开始日期
                            data.EducationalExperience4EndDate = info.EducationalExperience4EndDate;  //教育?经历（其它）结束日期
                            data.EducationalExperience4SchoolName = info.EducationalExperience4SchoolName;  //教育?经历（其它）结束学校名
                            data.EducationalExperience4Major = info.EducationalExperience4Major;  //教育?经历（其它）专业
                            data.EducationalExperience4Certificate = info.EducationalExperience4Certificate;  //教育?经历（其它）证书
                            data.FamilyStatus1Name = info.FamilyStatus1Name;  //家庭成员1 名称
                            data.FamilyStatus1Appellation = info.FamilyStatus1Appellation;  //家庭成员1 称谓
                            data.FamilyStatus1Company = info.FamilyStatus1Company;  //家庭成员1 工作单位
                            data.FamilyStatus1Tel = info.FamilyStatus1Tel;  //家庭成员1 联系电话
                            data.FamilyStatus2Name = info.FamilyStatus2Name;  //家庭成员2 名称
                            data.FamilyStatus2Appellation = info.FamilyStatus2Appellation;  //家庭成员2 称谓
                            data.FamilyStatus2Company = info.FamilyStatus2Company;  //家庭成员2 工作单位
                            data.FamilyStatus2Tel = info.FamilyStatus2Tel;  //家庭成员2 电话
                            data.EmergencyContactName = info.EmergencyContactName;  //紧急联系人名字
                            data.EmergencyContactTel = info.EmergencyContactTel;  //紧急联系人电话
                            data.EmergencyContactEelationShipWithMe = info.EmergencyContactEelationShipWithMe;  //紧急联系人关系
                            data.JobId = info.JobId;  //职位Id 与职位表对应
                            data.Title = info.Title;  //职称
                            data.Organization_Id = info.Organization_Id;  //机构部门 与组织机构表对应
                            data.SupervisorComments = info.SupervisorComments;  //主管评语
                            data.ProbationarySalary = info.ProbationarySalary;  //试用期薪水
                            data.CorrectSalary = info.CorrectSalary;  //转正薪水
                            data.EntryDate = info.EntryDate;  //入职日期
                            data.BirthCertificatePhoto = info.BirthCertificatePhoto;  //身份证拍照
                            data.RegistrationPhoto = info.RegistrationPhoto;  //登记照
                            data.BankCardNumber = info.BankCardNumber;  //银行卡号
                            data.WorkNumber = info.WorkNumber;  //创建员工工号
                            data.CreateStaffeId = info.CreateStaffeId;  //创建员工Id
                            data.UpdateTime = DateTime.Now; //创建时间
                            db.SaveChanges();
                        }
                    }
                }
                return baseCtrler.SJson("true");
            }
            catch (Exception e) { return baseCtrler.FJson(e.Message); }
        }

        //删除
        [CheckResourcesFilter(ResourcesName = "OilStationDailyEntryManager_Delete")]
        public JsonResult Del(Entry info)
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
                    Entry data = db.Entry.FirstOrDefault(x => x.Id == info.Id);
                    data.UpdateTime = DateTime.Now; //更新时间
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
        [CheckResourcesFilter(ResourcesName = "OilStationDailyEntryManager_Launch")]
        public JsonResult Start(Entry info)
        {
            Staff user = Session["userInfo"] as Staff;
            var baseCtrler = DependencyResolver.Current.GetService<BaseController>();
            try
            {
                if (baseCtrler.IsStart(info.Id))
                {
                    return baseCtrler.FJson("已经发起无法再次发起");

                }
                else
                {
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
                        Help.SetProcess("Entry", user.Id, info.No, info.Id, db);//添加到流程
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
        [CheckResourcesFilter(ResourcesName = "OilStationDailyEntryManager_ViewProcess")]
        public JsonResult GetProcessInfo(Entry info)
        {
            PageItem<ProcessStepRecord> data = new PageItem<ProcessStepRecord>();
            data.data = db.ProcessStepRecord.Where(x => x.RefOrderId == info.Id).OrderBy(x => x.StepOrder).ToList();
            return Json(data, JsonRequestBehavior.AllowGet);

        }
    }
}