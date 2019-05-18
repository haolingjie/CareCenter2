using CareCenter2.BLL;
using CareCenter2.DB;
using CareCenter2.Util.Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using static CareCenter2.Util.Common.Utility;

namespace CareCenter2.Web.Controllers
{
    public class UserController : Controller
    {
        #region 用户中心 开始
        /// <summary>
        /// 用户中心
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            //验证登录
            if (!User.Identity.IsAuthenticated)
            {               
                return RedirectToAction("Login", "Home");
            }
            //获取cookie值
            var userphone = User.Identity.Name;           
            var userInfo = HomeBLL.GetUserInfoByPhone(userphone); ;//获取用户信息

            var cookie = Request.Cookies[FormsAuthentication.FormsCookieName];
            var ticket = FormsAuthentication.Decrypt(cookie.Value);
            var userimg = ticket.UserData;
            if (userInfo.ImgUrl== null||userInfo.ImgUrl=="")
            {
                Session["HeadImgUrl"] = "/Content/img/HeadImg/login.png";
            }
            else
            {
                Session["HeadImgUrl"] = userInfo.ImgUrl;
            }
            if (userInfo.RoleStatus == 1)
            {
                if (userInfo.Role == 0)
                {
                    ViewBag.Role = "普通会员";
                }
                else if (userInfo.Role == 1)
                {
                    ViewBag.Role = "兼职保姆";
                }
                else if (userInfo.Role == 2)
                {
                    ViewBag.Role = "兼职教师";
                }
                else if (userInfo.Role == 3)
                {
                    ViewBag.Role = "兼职陪护/看护";
                }

            }
            else {
                ViewBag.Role = "普通会员";
            }
            return View(userInfo);
        }

        public ActionResult Nav()
        {
            return View();
        }

        #endregion

        #region 我的发布需求 
        /// <summary>
        /// 我的发布需求
        /// </summary>
        /// <returns></returns>
        public ActionResult OwnNeed()
        {
            //验证登录
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Login", "Home");
            }
            var userphone = User.Identity.Name;
            var userInfo = HomeBLL.GetUserInfoByPhone(userphone); ;//获取用户信息

            var cookie = Request.Cookies[FormsAuthentication.FormsCookieName];
            var ticket = FormsAuthentication.Decrypt(cookie.Value);
            var userimg = ticket.UserData;
            if (userInfo.ImgUrl == null || userInfo.ImgUrl == "")
            {
                Session["HeadImgUrl"] = "/Content/img/HeadImg/login.png";
            }
            else
            {
                Session["HeadImgUrl"] = userInfo.ImgUrl;
            }
            if (userInfo.RoleStatus == 1)
            {
                if (userInfo.Role == 0)
                {
                    ViewBag.Role = "普通会员";
                }
                else if (userInfo.Role == 1)
                {
                    ViewBag.Role = "兼职保姆";
                }
                else if (userInfo.Role == 2)
                {
                    ViewBag.Role = "兼职教师";
                }
                else if (userInfo.Role == 3)
                {
                    ViewBag.Role = "兼职陪护/看护";
                }

            }
            else
            {
                ViewBag.Role = "普通会员";
            }
            return View(userInfo);
        }
        /// <summary>
        /// 发布需求数据交互
        /// </summary>
        /// <returns></returns>
        public ActionResult OwnNeedData()
        {
            var OrderType = Request["OrderType"];//需求分类
            var userPhone = User.Identity.Name;
            //获取用户ID
            var userID = HomeBLL.GetUserInfoByPhone(userPhone).ID.ToString();

            List<OrderModel> MoneyList = UserBLL.GetOrderRecordList();
            MoneyList = MoneyList.Where(item=>item.Status>=0).ToList();
            if (!string.IsNullOrEmpty(userID))
            {
                MoneyList = MoneyList.Where(item => item.UserID == userID).ToList();
            }
            if (!string.IsNullOrEmpty(OrderType))
            {
                if (OrderType == "ClassCare")
                {
                    MoneyList = MoneyList.Where(item => item.OrderType == "共享保姆/课后托管照护").ToList();
                }
                else if (OrderType == "GravidaCare")
                {
                    MoneyList = MoneyList.Where(item => item.OrderType == "孕检照护").ToList();
                }
                else if (OrderType == "InfantCare")
                {
                    MoneyList = MoneyList.Where(item => item.OrderType == "婴幼照护").ToList();
                }
                else if (OrderType == "OldAgeCare")
                {
                    MoneyList = MoneyList.Where(item => item.OrderType == "养老照护").ToList();
                }
            }
            MoneyList = MoneyList.Distinct(new ListComparer<OrderModel>((p1, p2) => (p1.OrderID == p2.OrderID))).ToList();
            MoneyList = MoneyList.OrderByDescending(item => item.CreateDate).ToList();
            if (MoneyList != null && MoneyList.Count > 0)
            {
                DataTable NewDT = JsonHelper.ListToDataTable(MoneyList);
                var jsonString = JsonHelper.DataTableToJsonString(NewDT);
                return OperContext.PackagingAjaxMsg(AjaxStatu.ok, "", jsonString);
            }
            else
            {
                return OperContext.PackagingAjaxMsg(AjaxStatu.none, "");
            }
        }
       
        /// <summary>
        /// 我的需求详情
        /// </summary>
        /// <returns></returns>
        public ActionResult OwnNeedDetail()
        {
            //验证登录
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Login", "Home");
            }
            var userphone = User.Identity.Name;
            var userInfo = HomeBLL.GetUserInfoByPhone(userphone); ;//获取用户信息

            var cookie = Request.Cookies[FormsAuthentication.FormsCookieName];
            var ticket = FormsAuthentication.Decrypt(cookie.Value);
            var userimg = ticket.UserData;
            if (userInfo.ImgUrl == null || userInfo.ImgUrl == "")
            {
                Session["HeadImgUrl"] = "/Content/img/HeadImg/login.png";
            }
            else
            {
                Session["HeadImgUrl"] = userInfo.ImgUrl;
            }
            if (userInfo.RoleStatus == 1)
            {
                if (userInfo.Role == 0)
                {
                    ViewBag.Role = "普通会员";
                }
                else if (userInfo.Role == 1)
                {
                    ViewBag.Role = "兼职保姆";
                }
                else if (userInfo.Role == 2)
                {
                    ViewBag.Role = "兼职教师";
                }
                else if (userInfo.Role == 3)
                {
                    ViewBag.Role = "兼职陪护/看护";
                }

            }
            else
            {
                ViewBag.Role = "普通会员";
            }
            //获取详情
            var OrderID = Request["OrderID"];//订单编号
            List<OrderModel> DataList = UserBLL.GetOrderRecordList();
            //获取图片
            List<tb_Pictuer> PicList = UserBLL.GetPicList();
            //获取视频
            List<tb_Video> VideoList = UserBLL.GetVideoList();
            if (!string.IsNullOrEmpty(OrderID))
            {
                DataList = DataList.Where(item => item.OrderID == OrderID).ToList();
                ViewData["NeedDetail"] = DataList;
                //图片
                PicList = PicList.Where(item => item.OrderID == OrderID).OrderByDescending(item => item.CreateDate).ToList();
                ViewData["PicList"] = PicList;
                //视频
                VideoList = VideoList.Where(item => item.OrderID == OrderID).OrderByDescending(item => item.CreateDate).ToList();
                ViewData["VideoList"] = VideoList;

            }
            //获取评论
            List<CommentModel> CommentList = UserBLL.GetCommentData();
            if (!string.IsNullOrEmpty(OrderID))
            {
                CommentList = CommentList.Where(item => item.OrderID == OrderID).OrderByDescending(item => item.CreateDate).ToList();
                ViewData["CommentDetail"] = CommentList;
            }
            return View(userInfo);
        }
        #endregion

        #region 详情页面
        /// <summary>
        /// 详细页
        /// </summary>
        /// <returns></returns>
        public ActionResult UserDetails()
        {
            
            //验证登录
            if (!User.Identity.IsAuthenticated) {
                return RedirectToAction("Login", "Home");
            }
            var userphone = User.Identity.Name;
            var userInfo = HomeBLL.GetUserInfoByPhone(userphone); ;//获取用户信息

            var cookie = Request.Cookies[FormsAuthentication.FormsCookieName];
            var ticket = FormsAuthentication.Decrypt(cookie.Value);
            var userimg = ticket.UserData;
            if (userInfo.ImgUrl == null || userInfo.ImgUrl == "")
            {
                Session["HeadImgUrl"] = "/Content/img/HeadImg/login.png";
            }
            else
            {
                Session["HeadImgUrl"] = userInfo.ImgUrl;
            }
            if (userInfo.RoleStatus == 1)
            {
                if (userInfo.Role == 0)
                {
                    ViewBag.Role = "普通会员";
                }
                else if (userInfo.Role == 1)
                {
                    ViewBag.Role = "兼职保姆";
                }
                else if (userInfo.Role == 2)
                {
                    ViewBag.Role = "兼职教师";
                }
                else if (userInfo.Role == 3)
                {
                    ViewBag.Role = "兼职陪护/看护";
                }

            }
            else
            {
                ViewBag.Role = "普通会员";
            }
            //获取详情
            var OrderID = Request["OrderID"];//订单编号
            List<OrderModel> DataList = UserBLL.GetOrderRecordList();
            //获取图片
            List<tb_Pictuer> PicList = UserBLL.GetPicList();
            //获取视频
            List<tb_Video> VideoList = UserBLL.GetVideoList();
            if (!string.IsNullOrEmpty(OrderID))
            {
                DataList = DataList.Where(item => item.OrderID == OrderID).ToList();
                ViewData["OrderDetail"] = DataList;
                //图片
                PicList = PicList.Where(item => item.OrderID == OrderID).OrderByDescending(item => item.CreateDate).ToList();
                ViewData["PicList"] = PicList;
                //视频
                VideoList = VideoList.Where(item => item.OrderID == OrderID).OrderByDescending(item => item.CreateDate).ToList();
                ViewData["VideoList"] = VideoList;
            }
            //获取评论
            List<CommentModel> CommentList = UserBLL.GetCommentData();
            if (!string.IsNullOrEmpty(OrderID))
            {
                CommentList = CommentList.Where(item => item.OrderID == OrderID).OrderByDescending(item=>item.CreateDate).ToList();
                ViewData["CommentDetail"] = CommentList;
            }
            return View(userInfo);
        }
        /// <summary>
        /// 新增回复
        /// </summary>
        /// <returns></returns>
        public ActionResult SetCommentData()
        {
            var orderid = Request["orderid"];
            var comment = Request["comment"];
            var userphone = User.Identity.Name;
            var userInfo = HomeBLL.GetUserInfoByPhone(userphone); ;//获取用户信息
            tb_Comment com = new tb_Comment();
            com.ComIntor = comment;
            com.FaBuUserID = userInfo.ID;
            com.OrderID = orderid;
            int count = UserBLL.SetCommentData(com);
            if (count > 0)
            {
                return OperContext.PackagingAjaxMsg(AjaxStatu.ok, "新增回复成功", null, "/User/UserDetails/?OrderID=" + orderid + "");
            }
            else {
                return OperContext.PackagingAjaxMsg(AjaxStatu.err,"回复失败，请联系管理员");
            }
        }
        /// <summary>
        /// 修改订单状态
        /// </summary>
        /// <returns></returns>
        public ActionResult UpdOrderStatus()
        {
            var orderid = Request["orderid"];
            var status = Request["status"];
            var userphone = User.Identity.Name;
            var userInfo = HomeBLL.GetUserInfoByPhone(userphone); ;//获取用户信息
          
            int count = UserBLL.UpdOrderStatus(orderid,Convert.ToInt32(status));
            if (count > 0)
            {
                return OperContext.PackagingAjaxMsg(AjaxStatu.ok, "更新状态成功", null, "/User/UserDetails/?OrderID=" + orderid + "");
            }
            else
            {
                return OperContext.PackagingAjaxMsg(AjaxStatu.err, "更新状态失败，请联系管理员");
            }
        }
        #endregion

        #region 我的钱包
        /// <summary>
        ///我的钱包
        /// </summary>
        /// <returns></returns>
        public ActionResult OwnApply()
        {
            //验证登录
            if (!User.Identity.IsAuthenticated)
            {
                return View();
            }
            var userphone = User.Identity.Name;
            var userInfo = HomeBLL.GetUserInfoByPhone(userphone); ;//获取用户信息

            var cookie = Request.Cookies[FormsAuthentication.FormsCookieName];
            var ticket = FormsAuthentication.Decrypt(cookie.Value);
            var userimg = ticket.UserData;
            if (userInfo.ImgUrl == null || userInfo.ImgUrl == "")
            {
                Session["HeadImgUrl"] = "/Content/img/HeadImg/login.png";
            }
            else
            {
                Session["HeadImgUrl"] = userInfo.ImgUrl;
            }
            if (userInfo.RoleStatus == 1)
            {
                if (userInfo.Role == 0)
                {
                    ViewBag.Role = "普通会员";
                }
                else if (userInfo.Role == 1)
                {
                    ViewBag.Role = "兼职保姆";
                }
                else if (userInfo.Role == 2)
                {
                    ViewBag.Role = "兼职教师";
                }
                else if (userInfo.Role == 3)
                {
                    ViewBag.Role = "兼职陪护/看护";
                }

            }
            else
            {
                ViewBag.Role = "普通会员";
            }
            return View(userInfo);
        }
        /// <summary>
        /// 获取我的钱包数据
        /// </summary>
        /// <returns></returns>
        public ActionResult OwnApplyData()
        {
            var userPhone = User.Identity.Name;
            //获取用户ID
            var userID = HomeBLL.GetUserInfoByPhone(userPhone).ID.ToString();
            List<tb_MoneyRecord> MoneyList = UserBLL.GetMoneyRecordList();
            if (!string.IsNullOrEmpty(userID))
            {
                MoneyList = MoneyList.Where(item => item.UserID==userID).ToList();
            }
            if (MoneyList != null && MoneyList.Count > 0)
            {
                DataTable NewDT = JsonHelper.ListToDataTable(MoneyList);
                var jsonString = JsonHelper.DataTableToJsonString(NewDT);
                return OperContext.PackagingAjaxMsg(AjaxStatu.ok, "", jsonString);
            }
            else
            {
                return OperContext.PackagingAjaxMsg(AjaxStatu.none, "");
            }            
        }

        #endregion

        #region 我的订单
        /// <summary>
        /// 我的订单
        /// </summary>
        /// <returns></returns>
        public ActionResult OwnOrder()
        {
            //验证登录
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Login", "Home");
            }
            var userphone = User.Identity.Name;
            var userInfo = HomeBLL.GetUserInfoByPhone(userphone); ;//获取用户信息

            var cookie = Request.Cookies[FormsAuthentication.FormsCookieName];
            var ticket = FormsAuthentication.Decrypt(cookie.Value);
            var userimg = ticket.UserData;
            if (userInfo.ImgUrl == null || userInfo.ImgUrl == "")
            {
                Session["HeadImgUrl"] = "/Content/img/HeadImg/login.png";
            }
            else
            {
                Session["HeadImgUrl"] = userInfo.ImgUrl;
            }
            if (userInfo.RoleStatus == 1)
            {
                if (userInfo.Role == 0)
                {
                    ViewBag.Role = "普通会员";
                }
                else if (userInfo.Role == 1)
                {
                    ViewBag.Role = "兼职保姆";
                }
                else if (userInfo.Role == 2)
                {
                    ViewBag.Role = "兼职教师";
                }
                else if (userInfo.Role == 3)
                {
                    ViewBag.Role = "兼职陪护/看护";
                }

            }
            else
            {
                ViewBag.Role = "普通会员";
            }
            return View(userInfo);
        }
        /// <summary>
        /// 获取我的订单数据
        /// </summary>
        /// <returns></returns>
        public ActionResult OwnOrderData()
        {
            var OrderType = Request["OrderType"];//订单分类
            var userPhone = User.Identity.Name;
            //获取用户ID
            var userID = HomeBLL.GetUserInfoByPhone(userPhone).ID.ToString();

            List<OrderModel> MoneyList = UserBLL.GetOrderRecordList();
            if (!string.IsNullOrEmpty(userID))
            {
                MoneyList = MoneyList.Where(item => item.AdminUserID == userID).ToList();
            }
            if (!string.IsNullOrEmpty(OrderType))
            {
                if (OrderType == "ClassCare")
                {
                    MoneyList = MoneyList.Where(item => item.OrderType == "共享保姆/课后托管照护").ToList();
                }
                else if (OrderType == "GravidaCare")
                {
                    MoneyList = MoneyList.Where(item => item.OrderType == "孕检照护").ToList();
                }
                else if (OrderType == "InfantCare")
                {
                    MoneyList = MoneyList.Where(item => item.OrderType == "婴幼照护").ToList();
                }
                else if (OrderType == "OldAgeCare")
                {
                    MoneyList = MoneyList.Where(item => item.OrderType == "养老照护").ToList();
                }                
            }
            MoneyList = MoneyList.Distinct(new ListComparer<OrderModel>((p1, p2) => (p1.OrderID == p2.OrderID))).ToList();
            MoneyList = MoneyList.OrderByDescending(item => item.CreateDate).ToList();
            if (MoneyList != null && MoneyList.Count > 0)
            {
                DataTable NewDT = JsonHelper.ListToDataTable(MoneyList);
                var jsonString = JsonHelper.DataTableToJsonString(NewDT);
                return OperContext.PackagingAjaxMsg(AjaxStatu.ok, "", jsonString);
            }
            else
            {
                return OperContext.PackagingAjaxMsg(AjaxStatu.none, "");
            }
        }
        #endregion

        #region ***公共--上传主图片***
        /// <summary>
        /// 上传主图（单个图片）
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        public ActionResult UploadImgData(HttpPostedFileBase file)
        {

            var imgUrl = string.Empty;//写入数据库地址        
            HttpPostedFileBase ImgPath = file;// files["ImgPath"];//主图片
            string TempName = string.Empty;//图片新名称
            if (ImgPath.FileName != null)
            {
                //文件存放位置
                string path = ConfigHelper.GetAppSettingString("PicPath");
                //存入文件新名称
                TempName = Utility.ImgUpload(ImgPath, path);

                //写入数据库地址
                imgUrl = ConfigHelper.GetAppSettingString("ImgPath") + TempName;
                Session["imgurl"] = imgUrl;
                if (Session["imgurl"] != null)
                {
                    return OperContext.PackagingAjaxMsg(AjaxStatu.ok, "图片上传成功", imgUrl);
                }
            }

            return OperContext.PackagingAjaxMsg(AjaxStatu.err, "图片上传失败！");

        }

        /// <summary>
        /// 接收富文本上传图片数据
        /// </summary>
        /// <returns></returns>
        public ActionResult UploadImg()
        {
            string end = string.Empty;//返回数据
            //JsonResult ajaxRes = new JsonResult();//返回类型
            var imgUrl = string.Empty;
            HttpFileCollectionBase files = Request.Files;
            HttpPostedFileBase ImgPath = files[0];// files["ImgPath"];//主图片
            string TempName = string.Empty;//图片新名称
            if (ImgPath.FileName != null)
            {
                //文件存放位置
                string path = ConfigHelper.GetAppSettingString("PicPath");
                //存入文件新名称
                TempName = Utility.ImgUpload(ImgPath, path);

                //写入数据库地址
                imgUrl = ConfigHelper.GetAppSettingString("ImgPath") + TempName;
                if (imgUrl != null && imgUrl != "")
                {
                    //LogHelper.Info(imgUrl);
                    //保存成功后的json  
                    end = "{\"code\": 0,\"msg\": \"成功\",\"data\": {\"src\": \"" + imgUrl + "\"}}";
                    return Content(end);
                }
            }
            end = "{\"code\": 1,\"msg\": \"服务器故障\",\"data\": {\"src\": \"\"}}"; //返回的json  
            return Content(end);
        }


        #endregion

        #region ***上传视频***
        /// <summary>
        /// 上传视频（）
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        public ActionResult UploadVideoData(HttpPostedFileBase file)
        {

            var videoUrl = string.Empty;//写入数据库地址        
            HttpPostedFileBase VideoPath = file;// files["ImgPath"];//主图片
            string TempName = string.Empty;//图片新名称
            if (VideoPath.FileName != null)
            {
                //文件存放位置
                string path = ConfigHelper.GetAppSettingString("VideoPath");
                //存入文件新名称
                TempName = Utility.ImgUpload(VideoPath, path);

                //写入数据库地址
                videoUrl = ConfigHelper.GetAppSettingString("VideoSqlPath") + TempName;
                Session["videoUrl"] = videoUrl;
                if (Session["videoUrl"] != null)
                {
                    return OperContext.PackagingAjaxMsg(AjaxStatu.ok, "视频上传成功", videoUrl);
                }
            }

            return OperContext.PackagingAjaxMsg(AjaxStatu.err, "视频上传失败！");

        }

        #endregion
    }
    
}