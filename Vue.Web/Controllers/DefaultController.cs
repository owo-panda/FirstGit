using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data;
using Vue.Common;
using System.Text;
using System.Transactions;

namespace Vue.Web.Controllers
{
    [Online]
    public class DefaultController : BaseMvc
    {
        protected Model.manager manager = new BaseMvc().GetAdminInfo();
        // GET: Default
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult NavigationList()
        {
            StringBuilder strTxt = new StringBuilder();
            strTxt.Append("[");
            //获取一级栏目
            DataTable dt = new BLL.navigation().GetList(0, "parent_id=0 and nav_type='" + VueEnums.NavigationEnum.System.ToString()+"'", "id desc").Tables[0];
            DataRow[] dr = dt.Select("parent_id=0");
            for (int i = 0; i < dr.Length; i++)
            {
                bool isActionPass = true;
                if (int.Parse(dr[i]["is_lock"].ToString()) == 1)
                {
                    isActionPass = false;
                }
                if (isActionPass && manager.role_type > 1)
                {
                    string[] actionTypeArr = dr[i]["action_type"].ToString().Split(',');
                    foreach (string action_type_str in actionTypeArr)
                    {
                        //如果存在显示权限资源，则检查是否拥有该权限
                        if (action_type_str == "Show")
                        {
                            
                        }
                    }
                }
                else if(isActionPass && manager.role_type == 1)
                {
                    //超级管理员显示全部
                    strTxt.Append("{");
                    strTxt.Append("\"title\":\"" + dr[i]["title"] + "\"");
                    strTxt.Append(",\"name\":\"" + (i+1) + "\"");
                    strTxt.Append(",\"icon\":\"" + dr[i]["icon_url"] + "\"");
                    strTxt.Append(",\"menuItem\":[");
                    DataTable child_dt = new BLL.navigation().GetList(0, "parent_id="+Convert.ToInt32(dr[i]["id"])+" and nav_type='" + VueEnums.NavigationEnum.System.ToString() + "'", "id desc").Tables[0];
                    DataRow[] child_dr = child_dt.Select("is_lock=0");
                    for (int j = 0; j < child_dt.Rows.Count; j++) {
                        strTxt.Append("{");
                        strTxt.Append("\"label\":\"" + child_dr[j]["title"] + "\"");
                        strTxt.Append(",\"name\":\"" + (i + 1)+"-"+ (j + 1) + "\"");
                        strTxt.Append(",\"href\":\"" + child_dr[j]["link_url"] + "\"");
                        strTxt.Append(",\"icon\":\"" + child_dr[j]["icon_url"] + "\"");
                        strTxt.Append(",\"closable\":\"true\"");
                        strTxt.Append("}");
                        if (j < child_dt.Rows.Count - 1)
                        {
                            strTxt.Append(",");
                        }
                    }
                    strTxt.Append("]");
                    strTxt.Append("}");
                    if (i < dt.Rows.Count - 1)
                    {
                        strTxt.Append(",");
                    }
                }
            }
            strTxt.Append("]");
            return Content(strTxt.ToString());
        }
    }
}