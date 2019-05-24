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
    public class BaseMvc : Controller
    {
        public BaseMvc()
        {
            
        }

        #region 管理员============================================
        /// <summary>
        /// 判断管理员是否已经登录(解决Session超时问题)
        /// </summary>
        public bool IsAdminLogin()
        {
            //如果Session为Null
            if (System.Web.HttpContext.Current.Session[VueKeys.SESSION_ADMIN_INFO] != null)
            {
                return true;
            }
            else
            {
                //检查Cookies
                string adminname = Utils.GetCookie("AdminName", "Vue");
                string adminpwd = Utils.GetCookie("AdminPwd", "Vue");
                if (adminname != "" && adminpwd != "")
                {
                    BLL.manager bll = new BLL.manager();
                    Model.manager model = bll.GetModel(adminname, adminpwd);
                    if (model != null)
                    {
                        System.Web.HttpContext.Current.Session[VueKeys.SESSION_ADMIN_INFO] = model;
                        return true;
                    }
                }
            }
            return false;
        }

        /// <summary>
        /// 取得管理员信息
        /// </summary>
        public Model.manager GetAdminInfo()
        {
            if (IsAdminLogin())
            {
                Model.manager model = System.Web.HttpContext.Current.Session[VueKeys.SESSION_ADMIN_INFO] as Model.manager;
                if (model != null)
                {
                    return model;
                }
            }
            return null;
        }

        ///// <summary>
        ///// 检查管理员权限
        ///// </summary>
        ///// <param name="nav_name">菜单名称</param>
        ///// <param name="action_type">操作类型</param>
        //public void ChkAdminLevel(string nav_name, string action_type)
        //{
        //    Model.manager model = GetAdminInfo();
        //    BLL.manager_role bll = new BLL.manager_role();
        //    bool result = bll.Exists(model.role_id, nav_name, action_type);

        //    if (!result)
        //    {
        //        string msgbox = "parent.jsdialog(\"错误提示\", \"您没有管理该页面的权限，请勿非法进入！\", \"back\")";
        //        Response.Write("<script type=\"text/javascript\">" + msgbox + "</script>");
        //        Response.End();
        //    }
        //}

        ///// <summary>
        ///// 写入管理日志
        ///// </summary>
        ///// <param name="action_type"></param>
        ///// <param name="remark"></param>
        ///// <returns></returns>
        //public bool AddAdminLog(string action_type, string remark)
        //{
        //    if (sysConfig.logstatus > 0)
        //    {
        //        Model.manager model = GetAdminInfo();
        //        int newId = new BLL.manager_log().Add(model.id, model.user_name, action_type, remark);
        //        if (newId > 0)
        //        {
        //            return true;
        //        }
        //    }
        //    return false;
        //}

        #endregion
    }
}