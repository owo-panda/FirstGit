using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Vue.Web.Controllers
{
    [Online]
    public class NavigationController : BaseMvc
    {
        // GET: Navigation
        public ActionResult Index()
        {
            return View();
        }

    }
}