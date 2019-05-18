using CareCenter2.BLL;
using CareCenter2.DB;
using CareCenter2.Util.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CareCenter2.Web.Controllers
{
    public class PartTimeController : Controller
    {
        // GET: PartTime  兼职
        /// <summary>
        /// 兼职保姆
        /// </summary>
        /// <returns></returns>
        public ActionResult Nancy()
        {
            //验证登录
            //if (!User.Identity.IsAuthenticated)
            //{
            //    return RedirectToAction("Login", "Home");
            //}
            return View();
        }
        /// <summary>
        /// 兼职保姆数据交互
        /// </summary>
        /// <returns></returns>
        public ActionResult NancyData()
        {
            var userPhone = User.Identity.Name;
            //获取用户ID
            var userID = HomeBLL.GetUserInfoByPhone(userPhone).ID.ToString();
            var listdata = PartTimeBLL.GetPartTimeData(userID);           
            if (listdata.Count > 0)
            {
                return OperContext.PackagingAjaxMsg(AjaxStatu.none, "您已申请了" + listdata[0].Post + "职位，请注意接收来电！");
            }
            var username = Request["username"];
            var sex = Request["sex"];
            var work = Request["work"];
            var date_range = Request["date_range"];
            var serve1 = Request["serve1"];
            var serve2 = Request["serve2"];
            var salary = Request["salary"];
            var memo = Request["memo"];//职位性质
            //var tel = Request["tel"];
            tb_PartTime pt = new tb_PartTime();
            pt.UserID = userID;
            pt.UserName = username;
            pt.Education = null;
            pt.YearWork = work;
            pt.DateRange = date_range;
            pt.RoomService = serve1;
            pt.AgencyService = serve2;
            pt.Salary = Convert.ToDecimal(salary);
            pt.Phone = userPhone;
            pt.Post = "保姆";
            pt.SchoolName = null;
            pt.GraduationYear = null;
            pt.Sex = sex;
            pt.Memo = memo;
            //获取用户ID
            int count = PartTimeBLL.SetPartTimeTeacher(pt);
            if (count > 0)
            {
                int cnt = PartTimeBLL.UpdUserRole(userID, 1);//0：普通会员；1：保姆；2：教师；3：陪护/看护
                if (cnt > 0)
                {
                    return OperContext.PackagingAjaxMsg(AjaxStatu.ok, "保姆申请成功,请注意接收来电", null, "/User/Index/");
                }
                else
                {
                    return OperContext.PackagingAjaxMsg(AjaxStatu.err, "保姆申请失败");
                }
              
            }
            else
            {
                return OperContext.PackagingAjaxMsg(AjaxStatu.none, "保姆申请失败");
            }
        }
        /// <summary>
        /// 老师
        /// </summary>
        /// <returns></returns>
        public ActionResult Teacher()
        {
            return View();
        }
        /// <summary>
        /// 兼职教师数据交互
        /// </summary>
        /// <returns></returns>
        public ActionResult TeacherData()
        {
            var userPhone = User.Identity.Name;
            //获取用户ID
            var userID = HomeBLL.GetUserInfoByPhone(userPhone).ID.ToString();
            var listdata = PartTimeBLL.GetPartTimeData(userID);          
            if (listdata.Count > 0)
            {
                return OperContext.PackagingAjaxMsg(AjaxStatu.none, "您已申请了" + listdata[0].Post + "职位，请注意接收来电！");
            }
            var username = Request["username"];
            var education = Request["education"];
            var work = Request["work"];
            var date_range = Request["date_range"];
            var serve1 = Request["serve1"];
            var serve2 = Request["serve2"];
            var salary = Request["salary"];
            var sex = Request["sex"];
            var memo = Request["memo"];//职位性质
            tb_PartTime pt = new tb_PartTime();
            pt.UserID = userID;
            pt.UserName = username;
            pt.Education = education;
            pt.YearWork = work;
            pt.DateRange = date_range;
            pt.RoomService = serve1;
            pt.AgencyService = serve2;
            pt.Salary =Convert.ToDecimal(salary);
            pt.Phone = userPhone;
            pt.Post = "教师";
            pt.SchoolName = null;
            pt.GraduationYear = null;
            pt.Sex = sex;
            pt.Memo = memo;
            //获取用户ID
            int count = PartTimeBLL.SetPartTimeTeacher(pt);
            if (count > 0)
            {
                int cnt = PartTimeBLL.UpdUserRole(userID, 2);//0：普通会员；1：保姆；2：教师；3：陪护/看护
                if (cnt > 0)
                {
                    return OperContext.PackagingAjaxMsg(AjaxStatu.ok, "教师申请成功,请注意接收来电", null, "/User/Index/");
                }
                else {
                    return OperContext.PackagingAjaxMsg(AjaxStatu.err, "教师申请失败");
                }
               
            }
            else
            {
                return OperContext.PackagingAjaxMsg(AjaxStatu.none, "教师申请失败");
            }
        }
        /// <summary>
        /// 看护
        /// </summary>
        /// <returns></returns>
        public ActionResult Chaperonage()
        {
            return View();
        }

        /// <summary>
        /// 兼职看护数据交互
        /// </summary>
        /// <returns></returns>
        public ActionResult ChaperonageData()
        {
            var userPhone = User.Identity.Name;
            //获取用户ID
            var userID = HomeBLL.GetUserInfoByPhone(userPhone).ID.ToString();
            var listdata = PartTimeBLL.GetPartTimeData(userID);          
            if (listdata.Count > 0)
            {
                return OperContext.PackagingAjaxMsg(AjaxStatu.none, "您已申请了"+ listdata[0].Post + "职位，请注意接收来电！");
            }
            var username = Request["username"];
            var education = Request["education"];
            var work = Request["work"];
            var date_range = Request["date_range"];
            var serve1 = Request["serve1"];
            var serve2 = Request["serve2"];
            var salary = Request["salary"];
            var sex = Request["sex"];
            var memo = Request["memo"];//职位性质
            tb_PartTime pt = new tb_PartTime();
            pt.UserID = userID;
            pt.UserName = username;
            pt.Education = education;
            pt.YearWork = work;
            pt.DateRange = date_range;
            pt.RoomService = serve1;
            pt.AgencyService = serve2;
            pt.Salary = Convert.ToDecimal(salary);
            pt.Phone = userPhone;
            pt.Post = "看护/陪护";
            pt.SchoolName = null;
            pt.GraduationYear = null;
            pt.Sex = sex;
            pt.Memo = memo;
            //获取用户ID
            int count = PartTimeBLL.SetPartTimeTeacher(pt);
            if (count > 0)
            {
                int cnt = PartTimeBLL.UpdUserRole(userID, 3);//0：普通会员；1：保姆；2：教师；3：陪护/看护
                if (cnt > 0)
                {
                    return OperContext.PackagingAjaxMsg(AjaxStatu.ok, "看护/陪护申请成功,请注意接收来电", null, "/User/Index/");
                }
                else
                {
                    return OperContext.PackagingAjaxMsg(AjaxStatu.err, "看护/陪护申请失败");
                }
              
            }
            else
            {
                return OperContext.PackagingAjaxMsg(AjaxStatu.none, "看护/陪护申请失败");
            }
        }
    }
}