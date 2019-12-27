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

        /// <summary>
        /// 确认用户是否拥有某权限
        /// </summary>
        /// <param name="code">权限名称</param>
        /// <returns>返回bool</returns>
        public bool CheckResources(string code)
        {
            Staff user = (Staff)System.Web.HttpContext.Current.Session["userInfo"];
            List<SystemResourceModule> srsDate = GetSystemResources(user.Id);

            //总和
            if (srsDate.Any(x => x.Code.Contains(code)))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        
        /// <summary>
        /// 查找用户有什么权限
        /// </summary>
        /// <param name="id">用户id</param>
        /// <returns>权限集合</returns>
        public List<Models.SystemResourceModule> GetSystemResources(Guid id) 
        {
            StaffRole sRData = db.StaffRole.Where(x => x.StaffId == id).FirstOrDefault();//用户是什么角色
            List<Guid> ResourceModuleId = new List<Guid>();
            if (sRData != null)
            {
                List<RoleResourceModule> rrData = db.RoleResourceModule.Where(x => x.RoleId == sRData.RoleId).ToList();//这个角色有什么资源
                foreach (RoleResourceModule item in rrData)
                {
                    ResourceModuleId.Add(item.ResourceModuleId);
                }
            }

            List<SystemResourceModule> srsDate = db.SystemResourceModule.Where(x => ResourceModuleId.Contains(x.Id)).ToList();//查询系统资源库中包含ResourceModuleId的条目
            return srsDate;
        }

        //是否已经发起
        public bool IsStart(Guid RefOrderId)
        {

            List<ProcessStepRecord> data = db.ProcessStepRecord.Where(x => x.RefOrderId == RefOrderId & x.StepOrder == 0).ToList();
            if (data.Count > 0 && data.First().Result == true && data.First().WhetherToExecute == true)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}