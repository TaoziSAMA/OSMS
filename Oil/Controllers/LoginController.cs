using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using Oil.AppCode;

namespace Oil.Controllers
{
    public class LoginController : Controller
    {
        Models.OSMS db = new Models.OSMS();

        // GET: Login
        [LoginFilter(IsCheck = false)]
        public ActionResult Index()
        {
            //判断是否存在cookie
            if (Request.Cookies.Count > 0)
            {
                //获取指定 cookie
                HttpCookie authCookieEmail = Request.Cookies["email"];
                HttpCookie authCookiePwd = Request.Cookies["pwd"];
                if (authCookieEmail != null && authCookiePwd!=null)
                {
                    //票证解密
                    FormsAuthenticationTicket authTicketEmail = FormsAuthentication.Decrypt(authCookieEmail.Value);
                    FormsAuthenticationTicket authTicketPwd = FormsAuthentication.Decrypt(authCookiePwd.Value);
                    //获取cookie存入的值
                    string userEmail = authTicketEmail.Name;
                    string userPwd = authTicketPwd.Name;
                    var list = db.Staff.Where(u => u.Email == userEmail && u.Password==userPwd).FirstOrDefault();
                    Session["userInfo"] = list;
                    ViewBag.userEmail = userEmail;
                    ViewBag.userPwd = userPwd;
                }
            }
            return View();
            
        }

        //登录验证
        [HttpPost]
        [LoginFilter(IsCheck = false)]
        public ActionResult IndexLG(FormCollection f)
        {
            
            var email = f["username"];
            var pwd = f["password"];
            var remember = f["remember"];

            var token = new
            {
                access_token = "c262e61cd13ad99fc650e6908c7e5e65b63d2f32185ecfed6b801ee3fbdd5c0a",
            };
            var result1 = new
            {
                code = 0,
                msg = "登入成功",
                data = token
            };
            var result2 = new
            {
                code = 0,
                msg = "账号密码错误",
                data = token
            };

            var list = db.Staff.Where(u => u.Email == email && u.Password == pwd && u.Status=="1").FirstOrDefault();
            if (list != null)
            {
                if (remember != null)
                {
                    HttpCookie authCookieEmail = Request.Cookies["email"];
                    HttpCookie authCookiePwd = Request.Cookies["pwd"];
                    if (authCookieEmail != null && authCookiePwd != null)
                    {
                        authCookieEmail.Expires = DateTime.Now.Add(new TimeSpan(-1, 0, 0, 0));
                        authCookiePwd.Expires = DateTime.Now.Add(new TimeSpan(-1, 0, 0, 0));
                    }
                    //创建票证
                    FormsAuthenticationTicket ticketEmail = new FormsAuthenticationTicket(
                        1,
                        email,//存储的用户信息
                        DateTime.Now,
                        DateTime.Now.AddMinutes(60),
                        true,
                        Request.UserHostAddress
                        );
                    FormsAuthenticationTicket ticketPwd = new FormsAuthenticationTicket(
                        1,
                        pwd,//存储的用户信息
                        DateTime.Now,
                        DateTime.Now.AddMinutes(60),
                        true,
                        Request.UserHostAddress
                        );
                    // 加密票证
                    string encTicketEmail = FormsAuthentication.Encrypt(ticketEmail);
                    string encTicketPwd = FormsAuthentication.Encrypt(ticketPwd);
                    // 创建cookie
                    HttpCookie cookie1 = new HttpCookie("email", encTicketEmail);
                    HttpCookie cookie2 = new HttpCookie("pwd", encTicketPwd);
                    Response.Cookies.Add(cookie1);
                    Response.Cookies.Add(cookie2);
                }
                Session["userInfo"] = list;

                //存入登录信息
                Models.LoginInfo lf = new Models.LoginInfo();

                lf.StaffId = list.Id;//用户id
                lf.LoginTime = DateTime.Now;//登录时间
                lf.Id = Guid.NewGuid();
                db.LoginInfo.Add(lf);
                db.SaveChanges();


                return Json(result1, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(result2, JsonRequestBehavior.AllowGet);
            }

        }
    }
}