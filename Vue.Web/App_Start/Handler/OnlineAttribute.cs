using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Vue.Common;
using Vue.Model;
using Vue.BLL;
using Newtonsoft.Json.Linq;
using System.Data;
using System.Text;
using System.Web.Script.Serialization;
using Newtonsoft.Json;
using System.Collections;

namespace Vue.Web
{
    public class OnlineAttribute: AuthorizeAttribute
    {
        protected Model.manager manager;
        /// <summary>
        /// 单点登录
        /// </summary>
        /// <param name="filterContext"></param>
        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            manager = new BaseMvc().GetAdminInfo();
            if (manager == null)
            {
                filterContext.Result = new RedirectResult("/Login/Index");
                return;
            }

            Hashtable hOnline = (Hashtable)HttpContext.Current.Application["Online"];
            if (hOnline != null)
            {
                IDictionaryEnumerator idE = hOnline.GetEnumerator();
                while (idE.MoveNext())
                {
                    if (idE.Key != null && idE.Key.ToString().Equals(HttpContext.Current.Session.SessionID))
                    {
                        //already login  
                        if (idE.Value != null && "XXXXXX".Equals(idE.Value.ToString()))
                        {
                            hOnline.Remove(HttpContext.Current.Session.SessionID);
                            HttpContext.Current.Application.Lock();
                            HttpContext.Current.Application["Online"] = hOnline;
                            HttpContext.Current.Application.UnLock();
                            filterContext.Result = new RedirectResult("~/Login/Index");
                            return;
                        }
                        break;
                    }
                }
            }
        }
    }
}