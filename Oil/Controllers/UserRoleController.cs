using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Oil.Models;
using Oil.AppCode;
using System.Web.Script.Serialization;

namespace Oil.Controllers
{
    public class UserRoleController : Controller
    {
        OSMS db = new OSMS();

        // GET: UserRole
        public ActionResult Index()
        {
            var baseCtrler = DependencyResolver.Current.GetService<BaseController>();
            if (baseCtrler.CheckResources("SystemSettingStaffRoleManager_Setting"))
            {
                ViewBag.edit = true;
            }
            return View();
        }

        //获取用户表格数据
        [CheckResourcesFilter(ResourcesName = "SystemSettingStaffRoleManager")]
        public JsonResult GetList(Staff info,int page,int limit)
        {
            PageItem<View_StaffJ> list = Help.Page(page, limit, db.View_StaffJ.Where(x => x.No.Contains(info.No == null ? x.No : info.No)
                    & x.Name.Contains(info.Name == null ? x.Name : info.Name)
                    & x.Status == (info.Status == null ? x.Status : info.Status)).OrderBy(x => x.Id));
            return Json(list, JsonRequestBehavior.AllowGet);
        }

        //获取编辑页面
        public ActionResult Edit(Staff info)
        {
            ViewBag.Id = info.Id;
            return View();
        }

        //返回所有角色
        [CheckResourcesFilter(ResourcesName = "SystemSettingStaffRoleManager_Setting")]
        public JsonResult GetStaffRole(Role info)
        {
            PageItem<Role> data = new PageItem<Role>();
            data.data = db.Role.Where(x => x.Code.Contains(info.Code == null ? x.Code : info.Code) & x.Name.Contains(info.Name == null ? x.Name : info.Name)).ToList();
            return Json(data, JsonRequestBehavior.AllowGet);
        }


        //获取用户的角色
        [CheckResourcesFilter(ResourcesName = "SystemSettingStaffRoleManager_Setting")]
        public JsonResult GetRoleMbll(StaffRole info)
        {
            List<StaffRole> data = db.StaffRole.Where(x => x.StaffId == info.StaffId).ToList();
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        [CheckResourcesFilter(ResourcesName = "SystemSettingStaffRoleManager_Setting")]
        public JsonResult Save(string data,StaffRole info)
        {
            var baseCtrler = DependencyResolver.Current.GetService<BaseController>();
            try
            {
                JavaScriptSerializer js = new JavaScriptSerializer();
                List<Role> dataR = js.Deserialize<List<Role>>(data);
                List<StaffRole> dataSR = new List<StaffRole>();
                foreach (Role item in dataR)
                {
                    StaffRole sta = new StaffRole();
                    sta.StaffId = info.StaffId;
                    sta.RoleId = item.Id;
                    dataSR.Add(sta);
                }
                if (info != null)
                {   
                    db.Database.ExecuteSqlCommand(@"delete from StaffRole where StaffId='"+info.StaffId+"'");//sql语句前加@让C#识别为一条sql语句，防止sql在编译器中换行导致c#报错
                }//先删除

                if (dataSR.Count > 0)
                {
                    string sql = "";
                    foreach (StaffRole item_add in dataSR)
                    {
                        sql += string.Format("insert into StaffRole values(newid(),'{0}','{1}')\r", item_add.StaffId, item_add.RoleId);
                    }
                    db.Database.ExecuteSqlCommand(sql);
                }//再添加
                
                return baseCtrler.SJson("true");
            }
            catch (Exception e)
            {
                return baseCtrler.FJson(e.Message);
            }
        }
    }
}