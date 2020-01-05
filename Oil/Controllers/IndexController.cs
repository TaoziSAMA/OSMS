using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Oil.Models;

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

        //修改密码
        public ActionResult EditPwd()
        {
            //SendMailUseGmailed("1", "1", "1594698802@qq.com");
            Staff model = (Staff)Session["userinfo"];
            return View("EditPwd", model);
        }
        public JsonResult SavePwd(Staff info, string Newpassword)
        {
            var baseCtrler = DependencyResolver.Current.GetService<BaseController>();
            try
            {
                string password = info.Password;
                password = Help.Encode(password);
                Staff data= db.Staff.FirstOrDefault(x => x.Email == info.Email && x.Password == password);
                if (data != null)
                {
                    data.Password = "-1";
                }
                
                if (data != null)
                {
                    info.Password = Newpassword;
                    Staff _data = db.Staff.FirstOrDefault(x => x.Email == info.Email);
                    string _password = info.Password;
                    _password = Help.Encode(_password);
                    _data.Password = _password;
                    db.SaveChanges();
                    return baseCtrler.SJson("true");
                }
                else
                {
                    return baseCtrler.FJson("原密码错误");
                }
            }
            catch (Exception e) { return baseCtrler.FJson(e.Message); }
        }
    }
}