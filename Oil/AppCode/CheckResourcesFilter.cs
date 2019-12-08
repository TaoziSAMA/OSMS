using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json;
using Oil.Controllers;

namespace Oil.AppCode
{
    public class CheckResourcesFilter : ActionFilterAttribute
    {
        public string ResourcesName { get; set; }//权限名称
        //调用后执行
        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            //throw new NotImplementedException();
        }
        //调用前执行
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            BaseController Base = new BaseController();
            if (!Base.CheckResources(ResourcesName))
            {
                // filterContext.Result = new EmptyResult();
                //没有权限,验证不通过
                ContentResult Content = new ContentResult();
                Content.Content = "莫得权限，操作非法'";
                //执行结果为权限不通过
                filterContext.Result = Content;
            }
        }
    }
}