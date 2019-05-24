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
    public class HomeController : BaseMvc
    {
        protected BLL.users bll = new BLL.users();
        // GET: Home

        public ActionResult Index()
        {
            return View();
        }
        public ActionResult UserList(int page_index, int page_size)
        {
            int recordCount = 0;
            int selectId = Convert.ToInt32(HttpContext.Request.Form["select_id"]);
            string strTxt = HttpContext.Request.Form["value"];
            string strWhere = "1=1";
            if (!string.IsNullOrWhiteSpace(strTxt))
            {
                strWhere += " and NickName like '%" + strTxt + "%'";
            }
            if (selectId != 0)
            {
                strWhere += " and BloodTypeId=" + selectId;
            }
            //获取所有users数据
            DataTable dt = new BLL.users().GetList(page_size,page_index, strWhere, "Id asc",out recordCount).Tables[0];
            var JsonData = new
            {
                count = recordCount,
                page_index = page_index,
                row = dt,
            };
            return Content(JsonHelper.ToJson(JsonData));
        }
        public ActionResult Add(FormCollection collection)
        {
            Model.users model = new Model.users();
            model.LoginPwd = HttpContext.Request.Form["LoginPwd"].ToString();
            model.FriendshipPolicyId = 1;
            model.NickName = HttpContext.Request.Form["NickName"].ToString();
            model.FaceId = 8;
            model.Sex= HttpContext.Request.Form["Sex"].ToString();
            model.Age = Convert.ToInt32(HttpContext.Request.Form["Age"]);
            model.Name = "张珊";
            model.StarId = 8;
            model.BloodTypeId = 3;
            string strJson = "";
            if (bll.Add(model) > 0)
            {
                strJson = "{\"type\":\"1\",\"msg\":\"新增成功!\"}";
            }else
            {
                strJson = "{\"type\":\"0\",\"msg\":\"新增失败!\"}";
            }
            return Json(strJson, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Edit(FormCollection collection)
        {
            string strJson = "";
            int id = Convert.ToInt32(HttpContext.Request.Form["Id"]);
            Model.users model = bll.GetModel(id);
            if (model==null)
            {
                strJson = "{\"type\":\"0\",\"msg\":\"暂无数据!\"}";
                return Json(strJson, JsonRequestBehavior.AllowGet);
            }
            model.LoginPwd = HttpContext.Request.Form["LoginPwd"].ToString();
            model.NickName = HttpContext.Request.Form["NickName"].ToString();
            model.Sex = HttpContext.Request.Form["Sex"].ToString();
            model.Age = Convert.ToInt32(HttpContext.Request.Form["Age"]);
            if (bll.Update(model))
            {
                strJson = "{\"type\":\"1\",\"msg\":\"修改成功!\"}";
            }
            else
            {
                strJson = "{\"type\":\"0\",\"msg\":\"修改失败!\"}";
            }
            return Json(strJson, JsonRequestBehavior.AllowGet);
        }
        public ActionResult getModel(FormCollection collection)
        {
            string strJson = "";
            int id = Convert.ToInt32(HttpContext.Request.Form["Id"]);
            Model.users model = bll.GetModel(id);
            if (model == null)
            {
                strJson = "{\"type\":\"0\",\"msg\":\"暂无数据!\"}";
                return Json(strJson, JsonRequestBehavior.AllowGet);
            }

            StringBuilder strTxt = new StringBuilder();
            strTxt.Append("{");
            strTxt.Append("\"Id\":\"" + model.Id + "\"");
            strTxt.Append(",\"LoginPwd\":\"" + model.LoginPwd + "\"");
            strTxt.Append(",\"NickName\":\"" + model.NickName + "\"");
            strTxt.Append(",\"Sex\":\"" + model.Sex + "\"");
            strTxt.Append(",\"Age\":\"" + model.Age + "\"");
            strTxt.Append("}");

            strJson = "{\"type\":\"1\",\"data\":" + strTxt + ",\"msg\":\"获取成功!\"}";
            return Json(strJson, JsonRequestBehavior.AllowGet);
        }
        public ActionResult Delete(FormCollection collection)
        {
            string strJson = "";
            Model.users model = new Model.users();
            string ids = HttpContext.Request.Form["Id"];
            try
            {
                //事务保证同时更新状态
                using (TransactionScope ts = new TransactionScope())
                {

                    int count = 0;//删除标识记录
                    string[] Ids = ids.Split(',');
                    for (int i = 0; i < Ids.Length; i++)
                    {
                        model = bll.GetModel(Convert.ToInt32(Ids[i]));
                        if (model != null)
                        {
                            if (bll.Delete(Convert.ToInt32(Ids[i])))
                            {
                                count++;
                            }
                        }
                    }

                    if (Ids.Length == count)
                    {
                        ts.Complete();
                        strJson = "{\"type\":\"1\",\"msg\":\"删除成功!\"}";
                    }
                    else
                    {
                        strJson = "{\"type\":\"0\",\"msg\":\"删除失败!\"}";
                    }
                }
            }
            catch (Exception ex)
            {
                strJson = "{\"type\":\"0\",\"msg\":\""+ ex.Message + "\"}";
            }
            return Content(strJson);
        }
        public ActionResult Search(int page_index, int page_size)
        {
            int recordCount = 0;
            int selectId = Convert.ToInt32(HttpContext.Request.Form["select_id"]);
            string strTxt = HttpContext.Request.Form["strTxt"];
            string strWhere = "1=1";
            if (!string.IsNullOrWhiteSpace(strTxt))
            {
                strWhere += " and NickName like '%" + strTxt + "%'";
            }
            if (selectId!=0)
            {
                strWhere += " and BloodTypeId="+selectId;
            }
            //获取所有users数据
            DataTable dt = new BLL.users().GetList(page_size, page_index, strWhere, "Id asc", out recordCount).Tables[0];
            var JsonData = new
            {
                count = recordCount,
                page_index = page_index,
                row = dt,
            };
            return Content(JsonHelper.ToJson(JsonData));
        }
        public ActionResult BloodList()
        {
            //获取所有血型数据
            DataTable dt = new BLL.bloodtype().GetList(0, "1=1", "Id asc").Tables[0];
            StringBuilder strTxt = new StringBuilder();
            strTxt.Append("[");
            strTxt.Append("{\"value\":\"0\",\"label\":\"请选择\"}");
            if (dt.Rows.Count > 0)
            {
                strTxt.Append(",");
            }
            for (int i = 0;i < dt.Rows.Count;i++)
            {
                DataRow dr = dt.Rows[i];
                strTxt.Append("{");
                strTxt.Append("\"value\":\"" + dr["Id"] + "\"");
                strTxt.Append(",\"label\":\"" + dr["BloodType"] + "\"");
                strTxt.Append("}");
                //是否加逗号
                if (i < dt.Rows.Count - 1)
                {
                    strTxt.Append(",");
                }
            }
            strTxt.Append("]");
            string strJson = "{\"type\":\"0\",\"data\":"+strTxt+",\"msg\":\"获取成功!\"}";
            return Content(strJson);
        }
    }
}