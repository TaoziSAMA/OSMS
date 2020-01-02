using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Oil.Models;

namespace Oil.Controllers
{
    public class MainPageController : Controller
    {
        Models.OSMS db = new Models.OSMS();
        Help help = new Help();
        // GET: MainPage
        public ActionResult Main()
        {
            var baseCtrler = DependencyResolver.Current.GetService<BaseController>();
            if (baseCtrler.CheckResources("ManagerDaily"))
            {
                ViewBag.isManage = true;
            }


            Staff user = Session["userInfo"] as Models.Staff;

            ViewBag.EntryCount = db.ProcessStepRecord.Where(x => x.WaitForExecutionStaffId == user.Id & x.Type == "Entry" & x.WhetherToExecute==false).Count();
            ViewBag.QuitCount = db.ProcessStepRecord.Where(x => x.WaitForExecutionStaffId == user.Id & x.Type == "LeaveOffice").Count();
            ViewBag.username = user.Name;
            ViewBag.ornname = db.OrganizationStructure.Where(x => x.Id == user.OrgID).Where(x => x.IsDel == false).FirstOrDefault().Name;
            ViewBag.Jobname = db.Job.Where(x => x.Id == user.JobId).Where(x => x.IsDel == false).FirstOrDefault().Name;
            var LoginList = db.LoginInfo.Where(x => x.StaffId == user.Id).ToList();
            ViewBag.count = LoginList.Count();
            if (LoginList.Count() > 1)
            {
                ViewBag.time = LoginList.OrderByDescending(x => x.LoginTime).ToList()[1].LoginTime;
            }
            else
            {
                ViewBag.time = LoginList.OrderByDescending(x => x.LoginTime).ToList()[0].LoginTime + "(本次时间)";
            }

            //统计图数据
            
            string[] xarray = help.GetX().Split(',');
            ViewBag.x1 = xarray[0];
            ViewBag.x2 = xarray[1];
            ViewBag.x3 = xarray[2];
            ViewBag.x4 = xarray[3];
            ViewBag.x5 = xarray[4];
            ViewBag.x6 = xarray[5];
            ViewBag.x7 = xarray[6];
            ViewBag.y = help.GetY(user.Id);
            return View();
        }
    }
}