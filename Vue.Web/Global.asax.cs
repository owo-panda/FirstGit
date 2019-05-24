using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Collections;

namespace Vue.Web
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            RouteConfig.RegisterRoutes(RouteTable.Routes);
        }
        protected void Session_End()
        {
            Hashtable hOnline = (Hashtable)Application["Online"];
            try
            {
                if (hOnline[Session.SessionID] != null)
                {
                    hOnline.Remove(Session.SessionID);
                    Application.Lock();
                    Application["Online"] = hOnline;
                    Application.UnLock();
                }
            }
            catch (Exception e)
            {
                Application["Online"] = null;
            }
        }
    }
}
