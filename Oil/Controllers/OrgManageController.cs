using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Oil.Models;
using System.Web.Mvc;
using Oil.AppCode;

namespace Oil.Controllers
{
    public class OrgManageController : Controller
    {
        OSMS db = new OSMS();
        // GET: OrgManage
        public ActionResult Index()
        {
            var baseCtrler = DependencyResolver.Current.GetService<BaseController>();
            if (baseCtrler.CheckResources("OrganizationStructureManger_Add"))
            {
                ViewBag.add = true;
            }
            if (baseCtrler.CheckResources("OrganizationStructureManger_Delete"))
            {
                ViewBag.del = true;
            }
            if (baseCtrler.CheckResources("OrganizationStructureManger_Update"))
            {
                ViewBag.edit = true;
            }
            return View();
        }

        //获取列表
        [CheckResourcesFilter(ResourcesName = "OrganizationStructureManger")]
        public JsonResult GetList(OrganizationStructure info)
        {
            var list = (from org in db.OrganizationStructure.Where(x => true).Where(x => x.IsDel == false)
                        select new
                        {
                            Id = org.Id,
                            Name = org.Name,
                            Code = org.Code,
                            Leve = org.Leve,
                            lay_is_isChecked = true,
                            ParentId = org.ParentId == null ? new Guid("{A7243447-F9AD-4A93-8C3B-464B9389EDAF}") : org.ParentId
                        }).ToList();
            return Json(new { msg = "", code = 0, data = list, count = list.ToList().Count }, JsonRequestBehavior.AllowGet);
        }

        //添加修改
        public ActionResult AddOrEdit(OrganizationStructure info)
        {
            ViewBag.type = "Add";
            info.Leve = info.Leve + 1;
            if (info.ParentId != null)
            {
                ViewBag.type = "Edit";
            }
            return View(info);
        }

        //保存
        public JsonResult Save(OrganizationStructure info, string type,int Cleve)
        {
            var baseCtrler = DependencyResolver.Current.GetService<BaseController>();
            try
            {
                bool check = true;//辅助变量
                List<OrganizationStructure> redata = db.OrganizationStructure.Where(x => x.Name == info.Name).ToList();
                if (redata.Count > 0)//检查名称是否相同，存在同名进入if
                {
                    if (!(type == "Edit") && info.Id == redata[0].Id)//判断状态是否为添加，状态type为edit并且id存在判定为修改则跳出if，否则为添加进入if
                    {
                        check = false;
                        return baseCtrler.FJson("机构名称已存在");
                    }
                }
                if (check)
                {
                    redata = db.OrganizationStructure.Where(x => x.Code == info.Code).ToList();
                    if (redata.Count > 0)//检查代码是否相同，存在相同进入if
                    {
                        if (!(type == "Edit") && info.Id == redata[0].Id)//同上
                        {
                            check = false;
                            return baseCtrler.FJson("机构代码已存在");
                        }
                    }
                }
                if (check)
                {
                    if (type == "Add")
                    {
                        if (baseCtrler.CheckResources("OrganizationStructureManger_Add"))
                        {
                            info.Id = Guid.NewGuid();//设置ID
                            info.CreateTime = DateTime.Now; //创建时间
                            info.IsDel = false;//是否已删除
                            info.Leve = Cleve;
                            db.OrganizationStructure.Add(info);
                            db.SaveChanges();
                        }
                    }
                    else if (type == "Edit")
                    {
                        if (baseCtrler.CheckResources("OrganizationStructureManger_Update"))
                        {
                            OrganizationStructure data = db.OrganizationStructure.FirstOrDefault(x => x.Id == info.Id);
                            data.Name = info.Name;
                            data.Code = info.Code;
                            data.UpdateTime = DateTime.Now;
                            db.SaveChanges();
                        }
                    }
                }
                return baseCtrler.SJson("true");
            }
            catch (Exception e) { return baseCtrler.FJson(e.Message); }
        }

        //删除
        [CheckResourcesFilter(ResourcesName = "OrganizationStructureManger_Delete")]
        public JsonResult Del(OrganizationStructure info)
        {
            var baseCtrler = DependencyResolver.Current.GetService<BaseController>();
            try
            {
                if (info.Leve != 3)//判断是否为最低级机构
                {
                    //查询子机构，并删除本机构
                    List<OrganizationStructure> dataList = db.OrganizationStructure.Where(x => x.ParentId == info.Id).ToList();
                    OrganizationStructure data = db.OrganizationStructure.FirstOrDefault(x => x.Id == info.Id);
                    data.UpdateTime = DateTime.Now;
                    data.IsDel = true;
                    db.SaveChanges();
                    if (dataList.Count > 0)//判断是否有子机构
                    {
                        foreach (OrganizationStructure item in dataList)//循环删除子机构
                        {
                            data = db.OrganizationStructure.FirstOrDefault(x => x.Id == item.Id);
                            data.UpdateTime = DateTime.Now;
                            data.IsDel = true;
                            db.SaveChanges();
                        }

                        if (dataList[0].Leve != 3)//判断子机构是否为最低级机构
                        {
                        GO:
                            List<OrganizationStructure> dataLists = null;
                            foreach (OrganizationStructure itemC in dataList)
                            {
                                dataLists = db.OrganizationStructure.Where(x => x.ParentId == itemC.Id).ToList();//查询子机构中的子机构
                                //if (dataList.Count > 0)
                                //{
                                    foreach (OrganizationStructure item in dataLists)
                                    {
                                        data = db.OrganizationStructure.FirstOrDefault(x => x.Id == item.Id);
                                        data.UpdateTime = DateTime.Now;
                                        data.IsDel = true;
                                        db.SaveChanges();
                                    }
                                //}
                            }
                            if (dataLists.Count > 0)
                            {
                                if (dataLists[0].Leve != 3)             
                                {
                                    dataList = dataLists;
                                    goto GO;
                                }
                            }
                        }
                    }
                }
                else
                {
                    OrganizationStructure data = db.OrganizationStructure.FirstOrDefault(x => x.Id == info.Id);
                    data.UpdateTime = DateTime.Now;
                    data.IsDel = true;
                    db.SaveChanges();
                }
                return baseCtrler.SJson("true");
            }
            catch (Exception e) { return baseCtrler.FJson(e.Message); }
        }
    }
}