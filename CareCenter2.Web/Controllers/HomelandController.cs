using CareCenter2.BLL;
using CareCenter2.Util.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CareCenter2.Web.Controllers
{
    /// <summary>
    /// 家园
    /// </summary>
    public class HomelandController : Controller
    {
        // GET: Homeland
        /// <summary>
        /// 公司/团队/服务基本说明
        /// </summary>
        /// <returns></returns>
        public ActionResult Explain()
        {
            var dateList = DemandBLL.GetAllContext().Where(item => item.Type == "公司团队").ToList();
            dateList = dateList.OrderByDescending(item => item.CreateDate).ToList();
            return View(dateList);
        }
        /// <summary>
        /// 工作意义
        /// </summary>
        /// <returns></returns>
        public ActionResult WorkMean()
        {
            var dateList = DemandBLL.GetAllContext().Where(item => item.Type == "工作意义").ToList();
            dateList = dateList.OrderByDescending(item => item.CreateDate).ToList();
            return View(dateList);
        }
        /// <summary>
        /// 申请应用
        /// </summary>
        /// <returns></returns>
        public ActionResult Apply()
        {
            var dateList = DemandBLL.GetAllContext().Where(item => item.Type == "申请应用").ToList();
            dateList = dateList.OrderByDescending(item => item.CreateDate).ToList();
            return View(dateList);
        }
        /// <summary>
        /// 你想知道的
        /// </summary>
        /// <returns></returns>
        public ActionResult YouKnow()
        {
            var dateList = DemandBLL.GetAllContext().Where(item => item.Type == "你想知道").ToList();
            dateList = dateList.OrderByDescending(item => item.CreateDate).ToList();
            return View(dateList);
        }
        /// <summary>
        /// 工作注意事项
        /// </summary>
        /// <returns></returns>
        public ActionResult MatterNeed()
        {
            var dateList = DemandBLL.GetAllContext().Where(item => item.Type == "注意事项").ToList();
            dateList = dateList.OrderByDescending(item => item.CreateDate).ToList();
            return View(dateList);
        }
        /// <summary>
        /// 精彩瞬间
        /// </summary>
        /// <returns></returns>
        public ActionResult Moment()
        {
            var dateList = DemandBLL.GetPicList().Where(item => item.OrderID == "精彩瞬间").ToList();
            dateList = dateList.OrderByDescending(item => item.CreateDate).ToList();
            return View(dateList);
        }



        
    }
}