using CareCenter2.BLL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CareCenter2.Web.Controllers
{
    public class AboutUsController : Controller
    {
        // GET: AboutUs
        public ActionResult About()
        {
            var dateList = DemandBLL.GetAllContext().Where(item => item.Type == "关于我们").ToList();
            dateList = dateList.OrderByDescending(item => item.CreateDate).ToList();
            return View(dateList);
        }
    }
}