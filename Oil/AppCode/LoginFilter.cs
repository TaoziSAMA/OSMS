using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Oil.AppCode
{
    public class LoginFilter : ActionFilterAttribute
    {
        public bool IsCheck { get; set; }//是否启用

        //调用后执行
        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            //throw new NotImplementedException();
        }
        //调用前执行
        //登陆过滤器，检查session，非登陆状态重定向至登陆页面
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (IsCheck && (filterContext.HttpContext.Session["userInfo"]) == null)
            {
                //filterContext.Cancel = true;
                //controller.HttpContext.Response.Redirect("./Login");　　//重定向到要跳转的页面
                //filterContext.Controller.HttpContext.Redirect("/Home/Login", false);
                //filterContext.RequestContext.HttpContext.RedirectLocal("/Home/Login");
                //RedirectToAction("Login", "Home");
                filterContext.Result =new RedirectResult("/Login/Index");
                //filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary(new { controller = "Login", action = "Index", area = string.Empty }));
                //filterContext.Result = new EmptyResult();

            }
        }
    }
}