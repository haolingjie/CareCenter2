using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace CareCenter2.Util.Common
{
    /// <summary>
    /// 操作类的上下文
    /// </summary>
    public class OperContext
    {
        #region 把Ajax返回值封装成json格式的字符串
        public static ActionResult PackagingAjaxMsg(AjaxStatu statu, string msg, object data = null, string bakurl = "")
        {
            AjaxMsgParameter amp = new AjaxMsgParameter
            {
                Statu = statu,
                Msg = msg,
                Data = data,
                BackUrl = bakurl,
            };
            JsonResult ajaxRes = new JsonResult();
            ajaxRes.Data = amp;
            return ajaxRes;
        }
        #endregion

        #region 返回错误页面
        //public static ActionResult ErrPage()
        //{
        //    return view
        //}
        #endregion
    }
}
