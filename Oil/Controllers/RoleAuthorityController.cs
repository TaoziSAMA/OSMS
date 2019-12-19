using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using Oil.AppCode;
using Oil.Models;

namespace Oil.Controllers
{
    public class RoleAuthorityController : Controller
    {
        OSMS db = new OSMS();
        // GET: RoleAuthority
        public ActionResult Index()
        {
            var baseCtrler = DependencyResolver.Current.GetService<BaseController>();
            if (baseCtrler.CheckResources("SystemSettingRoleManage_Add"))
            {
                ViewBag.add = true;
            }
            if (baseCtrler.CheckResources("SystemSettingRoleManage_Delete"))
            {
                ViewBag.del = true;
            }
            if (baseCtrler.CheckResources("SystemSettingRoleManage_Update"))
            {
                ViewBag.edit = true;
            }
            if (baseCtrler.CheckResources("SystemSettingRoleManage_SettingResources"))
            {
                ViewBag.setAuthority = true;
            }

            return View();
        }

        //获取列表
        [CheckResourcesFilter(ResourcesName = "SystemSettingRoleManage")]
        public JsonResult GetList(Role info)
        {
            PageItem<Role> data = new PageItem<Role>();
            data.data = db.Role.Where(x => x.Code.Contains(info.Code == null ? x.Code : info.Code) & x.Name.Contains(info.Name == null ? x.Name : info.Name)).ToList();
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        //添加修改
        public ActionResult AddOrEdit(Staff info)
        {
            Role infoModel = new Role();
            ViewBag.type = "Add";
            if (info.Id!=new Guid())
            {
                infoModel = db.Role.Where(x => x.Id == info.Id).ToList().FirstOrDefault();
                ViewBag.type = "Edit";
            }

            return View(infoModel);
        }

        //保存
        public JsonResult Save(Role info,string type)
        {
            var baseCtrler = DependencyResolver.Current.GetService<BaseController>();
            try
            {
                bool check = true;
                List<Role> redata = db.Role.Where(x => x.Name == info.Name).ToList();
                if (redata.Count > 0)
                {
                    if (!(type == "Edit" && info.Id == redata[0].Id))
                    {
                        check = false;
                        return baseCtrler.FJson("角色名称已存在");
                    }
                }
                if (check)
                {
                    redata = db.Role.Where(x => x.Code == info.Code).ToList();
                    if (redata.Count > 0)
                    {
                        if (!(type == "Edit" && info.Id == redata[0].Id))
                        {
                            check = false;
                            return baseCtrler.FJson("角色代码已存在");
                        }
                    }
                }
                if (check)
                {
                    if (type == "Add")
                    {
                        if (baseCtrler.CheckResources("SystemSettingRoleManage_Add"))
                        {
                            info.Id = Guid.NewGuid();
                            db.Role.Add(info);
                            db.SaveChanges();
                        }
                    }
                    else if (type == "Edit")
                    {
                        if (baseCtrler.CheckResources("SystemSettingRoleManage_Update"))
                        {
                            Role data = db.Role.Where(x => x.Id == info.Id).FirstOrDefault();
                            data.Name = info.Name;
                            data.Code = info.Code;
                            db.SaveChanges();
                        }
                    }
                }
                return baseCtrler.SJson("true");
            }
            catch (Exception e) { return baseCtrler.FJson(e.Message); }
        }

        //删除
        [CheckResourcesFilter(ResourcesName = "SystemSettingRoleManage_Delete")]
        public ActionResult Del(Role info)
        {
            var baseCtrler = DependencyResolver.Current.GetService<BaseController>();
            try
            {
                db.Role.Attach(info);
                db.Role.Remove(info);
                return baseCtrler.SJson("true");
            }
            catch (Exception e) { return baseCtrler.FJson(e.Message); }
        }

        //打开权限设置页面
        public ActionResult SetAuthority(Role info)
        {
            ViewBag.Id = info.Id;
            return View();
        }


        //获取角色拥有那些权限
        [CheckResourcesFilter(ResourcesName = "SystemSettingRoleManage_SettingResources")]
        public JsonResult GetRoleMbll(RoleResourceModule info)
        {
            List<RoleResourceModule> data = db.RoleResourceModule.Where(x => x.RoleId == info.RoleId).ToList();
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        //保存权限
        [CheckResourcesFilter(ResourcesName = "SystemSettingRoleManage_SettingResources")]
        public JsonResult SaveAuthorityList(string data,RoleResourceModule info)
        {
            var baseCtrler = DependencyResolver.Current.GetService<BaseController>();
            try
            {
                JavaScriptSerializer js = new JavaScriptSerializer();
                List<SystemResourceModule> dataSR = js.Deserialize<List<SystemResourceModule>>(data);
                List<RoleResourceModule> dataRR = new List<RoleResourceModule>();
                foreach(SystemResourceModule item in dataSR)
                {
                    RoleResourceModule rol = new RoleResourceModule();
                    rol.RoleId = info.RoleId;
                    rol.ResourceModuleId = item.Id;
                    dataRR.Add(rol);
                }
                if (info != null)
                {
                    db.Database.ExecuteSqlCommand(@"delete from RoleResourceModule where RoleId='" + info.RoleId + "'");//sql语句前加@让C#识别为一条sql语句，防止sql在编译器中换行导致c#报错
                }
                if (dataRR.Count > 0)
                {
                    string sql = "";
                    foreach (RoleResourceModule item_add in dataRR)
                    {
                        sql += string.Format("insert into RoleResourceModule values(newid(),'{0}','{1}')\r", item_add.RoleId, item_add.ResourceModuleId);
                    }
                    db.Database.ExecuteSqlCommand(sql);
                }
                return baseCtrler.SJson("true");
            }
            catch(Exception e) { return baseCtrler.FJson(e.Message); }
        }
    }
}