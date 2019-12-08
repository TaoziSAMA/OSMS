using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Oil.Controllers
{
    public class IndexController : Controller
    {
        Models.OSMS db = new Models.OSMS();
        // GET: Index
        public ActionResult Index()
        {
            var baseCtrler = DependencyResolver.Current.GetService<BaseController>();
            Models.Staff user = Session["userInfo"] as Models.Staff;
            ViewBag.username = user.Name;
            List<Models.SystemResourceModule> Sredata = baseCtrler.GetSystemResources(user.Id);

            ViewBag.SystemResourceModuleParent = Sredata.Where(x => x.ParentId == new Guid()).ToList(); //一级菜单
            ViewBag.SystemResourceModule = Sredata.Where(x => x.ParentId != new Guid()).ToList();//二级菜单
            return View();
        }



        //退出登录
        public JsonResult OutLogin()
        {
            var baseCtrler = DependencyResolver.Current.GetService<BaseController>();
            Session.Remove("userInfo");
            return baseCtrler.SJson("1");
        }
    }
}