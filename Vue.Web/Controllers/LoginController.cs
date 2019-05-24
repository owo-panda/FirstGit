using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Vue.Common;
using System.Collections;

namespace Vue.Web.Controllers
{
    public class LoginController : BaseMvc
    {
        // GET: Login
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Login()
        {
            string username = HttpContext.Request.Form["username"].ToString();
            string password = HttpContext.Request.Form["password"].ToString();
            string remember = HttpContext.Request.Form["status"].ToString();
            Model.manager model = new BLL.manager().GetModel(username);
            if (model==null)
            {
                return Content("{\"type\":\"0\",\"msg\":\"用户名或密码错误!\"}");
            }
            string newpwd = DESEncrypt.Encrypt(password, model.salt);
            if (model.password != newpwd)
            {
                return Content("{\"type\":\"0\",\"msg\":\"密码错误!\"}");
            }

            Session[VueKeys.SESSION_ADMIN_INFO] = model;
            Session.Timeout = 45;
            //记住登录状态下次自动登录
            //写入Cookies
            Utils.WriteCookie("VueRememberName", model.user_name, 14400);
            if (remember.ToLower() == "true")
            {
                Utils.WriteCookie("AdminName", "Vue", model.user_name, 43200);
                Utils.WriteCookie("AdminPwd", "Vue", model.password, 43200);
            }
            else
            {
                //防止Session提前过期
                Utils.WriteCookie("AdminName", "Vue", model.user_name);
                Utils.WriteCookie("AdminPwd", "Vue", model.password);
            }
            //验证成功
            Hashtable hOnline = (Hashtable)System.Web.HttpContext.Current.Application["Online"];
            if (hOnline != null)
            {
                int i = 0;
                while (i < hOnline.Count) //因小BUG所以增加此判断，强制查询到底 
                {
                    IDictionaryEnumerator idE = hOnline.GetEnumerator();
                    string strKey = "";
                    while (idE.MoveNext())
                    {
                        if (idE.Value != null && idE.Value.ToString().Equals(username))
                        {
                            //already login              
                            strKey = idE.Key.ToString();
                            hOnline[strKey] = "XXXXXX";
                            break;
                        }
                    }
                    i = i + 1;
                }
            }
            else
            {
                hOnline = new Hashtable();
            }
            hOnline[Session.SessionID] = username;
            System.Web.HttpContext.Current.Application.Lock();
            System.Web.HttpContext.Current.Application["Online"] = hOnline;
            System.Web.HttpContext.Current.Application.UnLock();

            return Content("{\"type\":\"1\",\"msg\":\"登录成功!\"}");
        }
        public ActionResult LoginOut()
        {
            System.Web.HttpContext.Current.Session[VueKeys.SESSION_ADMIN_INFO] = null;
            Utils.WriteCookie("AdminName", "Vue", -14400);
            Utils.WriteCookie("AdminPwd", "Vue", -14400);
            System.Web.HttpContext.Current.Session.Abandon();
            return Content("{\"type\":\"1\",\"msg\":\"注销成功!\"}");
        }
    }
}