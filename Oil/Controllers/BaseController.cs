using Oil.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace Oil.Controllers
{
    public class BaseController : Controller
    {
        Models.OSMS db = new OSMS();
        // GET: Base
        JavaScriptSerializer js = new JavaScriptSerializer();
        //返回正常Json
        public JsonResult SJson(string Info)
        {
            return Json(new { Result = true, info = Info }, JsonRequestBehavior.AllowGet);
        }
        //返回异常Json
        public JsonResult FJson(string errorMsg)
        {
            return Json(new { Result = false, ErrorMsg = errorMsg }, JsonRequestBehavior.AllowGet);
        }

        //获取用户具体权限
        public bool CheckResources(string code)
        {
            Staff user = Session["userInfo"] as Staff;
            StaffRole sRData = db.StaffRole.Where(x => x.StaffId == user.Id).FirstOrDefault();//用户是什么角色
            List<RoleResourceModule> rrData = db.RoleResourceModule.Where(x => x.RoleId == sRData.RoleId).ToList();//这个角色有什么资源
            //将角色的资源id循环填入资源id的List
            List<Guid> ResourceModuleId = new List<Guid>();
            foreach (RoleResourceModule item in rrData)
            {
                ResourceModuleId.Add(item.ResourceModuleId);
            }
            List<SystemResourceModule> srsDate = db.SystemResourceModule.Where(x => ResourceModuleId.Contains(x.Id)).ToList();//查询系统资源库中包含ResourceModuleId的条目


            //总和
            if (Sredata.Any(x => x.Code.Contains(code)))
            {
                return true;
            }
            else
            {
                return false;
            }=
        }
    }
}