using CareCenter2.BLL;
using CareCenter2.DB;
using CareCenter2.Util.Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using static CareCenter2.Util.Common.Utility;

namespace CareCenter2.Admin.Controllers
{
    public class MemberMController : Controller
    {
        // GET: MemberM

        #region ***看护方审核***
        /// <summary>
        /// 看护方审批
        /// </summary>
        /// <returns></returns>
        public ActionResult CheckedCare()
        {
            //验证登录
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("SignIn", "Home");
            }
            return View();
        }

        /// <summary>
        /// 获取看护方审批数据
        /// </summary>
        /// <returns></returns>
        public ActionResult CheckedCareData()
        {
            var search = Request["search"] == null ? "" : Request["search"].ToString();//搜索条件
            var type = Request["type"] == null ? "" : Request["type"].ToString();//类型
            List<OrderModel> RList = AdminsBLL.GetOrderList();
            if (search != "")
            {
               
                RList = RList.Where(item => item.NickName.Contains(search) ||
                                           item.UserPhone.Contains(search)).ToList();
            }
            if (type != "")
            {
                RList = RList.Where(item => item.OrderType==type).ToList();
            }
            RList = RList.OrderByDescending(item => item.CreateDate).ToList();
            RList = RList.Distinct(new ListComparer<OrderModel>((p1, p2) => (p1.OrderID == p2.OrderID))).ToList();//去除重复数据
            DataTable newOrder = Utility.ListToDataTable(RList);
            if (newOrder != null && newOrder.Rows.Count > 0)
            {
                var jsonString = Utility.DataTableToJsonString(newOrder);
                return OperContext.PackagingAjaxMsg(AjaxStatu.ok, "", jsonString);
            }
            else
            {
                return OperContext.PackagingAjaxMsg(AjaxStatu.none, "");
            }
        }
        /// <summary>
        /// 通过/驳回需求
        /// </summary>
        /// <returns></returns>
        public ActionResult UpdateStatus()
        {
            //验证登录
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("SignIn", "Home");
            }
            var orderid = Request["ID"];
            var flag = Request["Status"];
            if (flag == "1")
            {
                int count = AdminsBLL.UpdOrderStatus(orderid, Convert.ToInt32(flag));
                if (count > 0)
                {
                    return OperContext.PackagingAjaxMsg(AjaxStatu.ok, "审核成功");
                }
                else
                {
                    return OperContext.PackagingAjaxMsg(AjaxStatu.err, "审核失败");
                }
            }
            else if (flag == "-1")
            {
                int count = AdminsBLL.UpdOrderStatus(orderid, Convert.ToInt32(flag));
                if (count > 0)
                {
                    return OperContext.PackagingAjaxMsg(AjaxStatu.ok, "驳回成功");
                }
                else
                {
                    return OperContext.PackagingAjaxMsg(AjaxStatu.err, "驳回失败");
                }
            }
            else  {
                int count = AdminsBLL.UpdOrderStatus(orderid, Convert.ToInt32(flag));
                if (count > 0)
                {
                    return OperContext.PackagingAjaxMsg(AjaxStatu.ok, "回访成功");
                }
                else
                {
                    return OperContext.PackagingAjaxMsg(AjaxStatu.err, "回访失败");
                }
            }
        }
        #endregion

        #region ***课后照护管理***

        #region --课后照护--
        /// <summary>
        /// 课后照护
        /// </summary>
        /// <returns></returns>
        public ActionResult AdminClassCare()
        {
            //验证登录
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("SignIn", "Home");
            }
            return View();
        }
        /// <summary>
        /// 课后照护数据
        /// </summary>
        /// <returns></returns>
        public ActionResult AdminClassCareData()
        {
            //验证登录
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("SignIn", "Home");
            }
            var phone = User.Identity.Name;//手机号码
            var search = Request["search"] == null ? "" : Request["search"].ToString();//搜索条件
            var type = Request["type"] == null ? "" : Request["type"].ToString();//类型
            List<OrderModel> RList = AdminsBLL.GetOrderList();
            RList = RList.Where(item => !string.IsNullOrEmpty(item.AdminUserID) && item.Status >= 1).ToList();
            //获取后台管理员权限
            var admin = AdminsBLL.GetAdminInfoByAdmin(phone);
            //获取前台用户编号
            var adminUser = HomeBLL.GetUserInfoByPhone(phone);

            if (admin != null )
            {
                if (admin.Memo != "超级管理员")
                {
                    if (adminUser != null)
                    {
                        RList = RList.Where(item => item.AdminUserID == adminUser.ID).ToList();
                    }
                    else
                    {
                        return OperContext.PackagingAjaxMsg(AjaxStatu.none, "");
                    }
                }
                           
            }

            if (search != "")
            {
                RList = RList.Where(item => item.NickName.Contains(search) ||
                                           item.UserPhone.Contains(search)).ToList();
            }
            if (type != "")
            {
                RList = RList.Where(item => item.OrderType == type).ToList();
            }
            RList = RList.OrderByDescending(item => item.CreateDate).ToList();
            RList = RList.Distinct(new ListComparer<OrderModel>((p1, p2) => (p1.OrderID == p2.OrderID))).ToList();//去除重复数据
            DataTable newOrder = Utility.ListToDataTable(RList);
            if (newOrder != null && newOrder.Rows.Count > 0)
            {
                var jsonString = Utility.DataTableToJsonString(newOrder);
                return OperContext.PackagingAjaxMsg(AjaxStatu.ok, "", jsonString);
            }
            else
            {
                return OperContext.PackagingAjaxMsg(AjaxStatu.none, "");
            }
        }
        /// <summary>
        /// 课后照护详情
        /// </summary>
        /// <returns></returns>
        public ActionResult AdminClassCareDetail()
        {
            //验证登录
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("SignIn", "Home");
            }
            //获取详情
            var OrderID = Request["OrderID"];//订单编号
            List<OrderModel> DataList = AdminsBLL.GetOrderList();
            if (!string.IsNullOrEmpty(OrderID))
            {
                DataList = DataList.Where(item => item.OrderID == OrderID).ToList();               
            }
            //获取图片
            List<tb_Pictuer> PicList = AdminsBLL.GetPicList();
            PicList = PicList.Where(item => item.OrderID == OrderID).OrderByDescending(item=>item.CreateDate).ToList();
            ViewData["PicList"] = PicList;
            //获取视频
            List<tb_Video> VideoList = AdminsBLL.GetVideoList();
            VideoList = VideoList.Where(item => item.OrderID == OrderID).OrderByDescending(item => item.CreateDate).ToList();
            ViewData["VideoList"] = VideoList;

            return View(DataList);
        }
        #endregion

        #region --添加信息--
        /// <summary>
        /// 添加信息
        /// </summary>
        /// <returns></returns>
        public ActionResult AddClassCare()
        {
            //验证登录
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("SignIn", "Home");
            }
            var OrderID = Request["OrderID"];
            ViewBag.orderid = OrderID;
            return View();
        }

        /// <summary>
        /// 添加信息
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult AddClassCareData()
        {

            var OrderID = Request["OrderID"];
            var Title = Request["Title"];
            var Memo = Request["Memo"];
            var VideoUrl = Request["VideoUrl"];
            var ImgUrl = Request["ImgUrl"];
            //var OrderClassify = Request["OrderClassify"];
            //var OrderType = Request["OrderType"];
            //var OrderIntro = Request["OrderIntro"];
            //var OrderID = Utility.GetNewGUID();
            int picCnt = 0;//图片
            int videoCnt = 0;//视频
          

            //图片
            if (!string.IsNullOrEmpty(ImgUrl))
            {
                tb_Pictuer pic = new tb_Pictuer();
                pic.PicTitle = Title;
                pic.OrderID = OrderID;
                pic.PicUrl = ImgUrl;
                pic.Memo = Memo;
                picCnt = DemandBLL.PictureIns(pic);
            }

            //视频
            if (!string.IsNullOrEmpty(VideoUrl))
            {
                tb_Video video = new tb_Video();
                video.VideoTitle = Title;
                video.OrderID = OrderID;
                video.VideoUrl = VideoUrl;
                video.Memo = Memo;
                videoCnt = DemandBLL.VideoIns(video);
            }
            if (!string.IsNullOrEmpty(ImgUrl) && picCnt <= 0)
            {
                return OperContext.PackagingAjaxMsg(AjaxStatu.err, "图片提交失败,请联系管理员！");
            }
            if (!string.IsNullOrEmpty(VideoUrl) && videoCnt <= 0)
            {
                return OperContext.PackagingAjaxMsg(AjaxStatu.err, "视频提交失败,请联系管理员！");
            }

            return OperContext.PackagingAjaxMsg(AjaxStatu.ok, "信息提交成功请查看详情!", null, "/MemberM/AdminClassCare/");
           
        }

        #endregion

        #region--编辑信息--
        /// <summary>
        /// 编辑
        /// </summary>
        /// <returns></returns>
        public ActionResult EditClassCare()
        {
            //验证登录
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("SignIn", "Home");
            }
            //获取详情
            var OrderID = Request["OrderID"];//订单编号
            List<OrderModel> DataList = AdminsBLL.GetOrderList();
            DataList = DataList.Distinct(new ListComparer<OrderModel>((p1, p2) => (p1.OrderID == p2.OrderID))).ToList();//去除重复数据
            if (!string.IsNullOrEmpty(OrderID))
            {
                DataList = DataList.Where(item => item.OrderID == OrderID).ToList();

            }
            return View(DataList);
        }
        /// <summary>
        /// 修改信息
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult EditClassCareData()
        {
            //验证登录
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("SignIn", "Home");
            }
            var OrderID = Request["OrderID"];
            var Title = Request["Title"];
            var Memo = Request["Memo"];
            //var VideoUrl = Request["VideoUrl"];
            //var ImgUrl = Request["ImgUrl"];
            var OrderClassify = Request["OrderClassify"];
            var OrderType = Request["OrderType"];
            var OrderIntro = Request["OrderIntro"];

            //需求数据
            tb_Order order = new tb_Order();                       
            order.OrderID = OrderID;
            order.OrderTitle = Title;
            order.OrderType = OrderType;
            order.OrderClassify = Convert.ToInt32(OrderClassify);
            order.Memo = Memo;
            order.OrderIntor = OrderIntro;

            int count = DemandBLL.EditOrderData(order);
            if (count > 0)
            {
                return OperContext.PackagingAjaxMsg(AjaxStatu.ok, "信息提交成功请查看详情!", null, "/MemberM/AdminClassCare/");
            }
            else
            {
                return OperContext.PackagingAjaxMsg(AjaxStatu.err, "信息提交失败!");
            }

        }

        #endregion

        #region--删除信息--

        /// <summary>
        /// 删除
        /// </summary>
        /// <returns></returns>
        public ActionResult DelClassCare()
        {
            //验证登录
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("SignIn", "Home");
            }
           
            var OrderID = Request["OrderID"];//订单编号
            ViewBag.orderid = OrderID;
            return View();
        }
        /// <summary>
        /// 获取删除数据
        /// </summary>
        /// <returns></returns>
        public ActionResult DelClassCareData()
        {
            //验证登录
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("SignIn", "Home");
            }
            var OrderID = Request["orderid"];
            //var search = Request["search"] == null ? "" : Request["search"].ToString();//搜索条件
            //图片
            List<tb_Pictuer> picList = AdminsBLL.GetPicList();
            picList = picList.Where(item => item.OrderID== OrderID && item.Status == 1).ToList();
            //视频
            List<tb_Video> videoList = AdminsBLL.GetVideoList();
            videoList = videoList.Where(item => item.OrderID == OrderID && item.Status == 1).ToList();


            //if (search != "")
            //{
            //    picList = picList.Where(item => item.PicTitle.Contains(search) ||
            //                               item.Memo.Contains(search)).ToList();
            //    videoList = videoList.Where(item => item.VideoTitle.Contains(search) ||
            //                              item.Memo.Contains(search)).ToList();
            //}
            picList = picList.OrderByDescending(item => item.CreateDate).ToList();
            videoList = videoList.OrderByDescending(item => item.CreateDate).ToList();
            DataTable newOrder1 = Utility.ListToDataTable(picList);
            DataTable newOrder2 = Utility.ListToDataTable(videoList);
            if (newOrder1 != null || newOrder2!=null)
            {
                var jsonString1 = Utility.DataTableToJsonString(newOrder1);
                var jsonString2 = Utility.DataTableToJsonString(newOrder2);

                jsonlist jsonString = new jsonlist();
                jsonString.pic = jsonString1;
                jsonString.video = jsonString2;
                return OperContext.PackagingAjaxMsg(AjaxStatu.ok, "", jsonString);
            }
            else
            {
                return OperContext.PackagingAjaxMsg(AjaxStatu.none, "");
            }
        }
        /// <summary>
        /// 删除图片
        /// </summary>
        /// <returns></returns>
        public ActionResult DelPic()
        {
            var picid = Request["PicID"];
            int count = AdminsBLL.DelPicData(picid);
            if (count > 0)
            {
                return OperContext.PackagingAjaxMsg(AjaxStatu.ok, "删除成功");
            }
            else
            {
                return OperContext.PackagingAjaxMsg(AjaxStatu.err, "删除失败");
            }
        }
        /// <summary>
        /// 删除视频
        /// </summary>
        /// <returns></returns>
        public ActionResult DelVideo()
        {
            var videoid = Request["VideoID"];
            int count = AdminsBLL.DelVideoData(videoid);
            if (count > 0)
            {
                return OperContext.PackagingAjaxMsg(AjaxStatu.ok, "删除成功");
            }
            else
            {
                return OperContext.PackagingAjaxMsg(AjaxStatu.err, "删除失败");
            }
        }
        #endregion

        #endregion

        #region ***服务方管理***
        /// <summary>
        /// 服务方人员
        /// </summary>
        /// <returns></returns>
        public ActionResult CareParson()
        {
            //验证登录
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("SignIn", "Home");
            }
            return View();
        }
        /// <summary>
        /// 服务方人员数据
        /// </summary>
        /// <returns></returns>
        public ActionResult CareParsonData()
        {
            //验证登录
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("SignIn", "Home");
            }
            var search = Request["search"] == null ? "" : Request["search"].ToString();//搜索条件
            List<tb_User> RList = AdminsBLL.GetCareParsonList();
            if (search != "")
            {

                RList = RList.Where(item => item.NickName.Contains(search) ||
                                           item.UserPhone.Contains(search)).ToList();
            }
            RList = RList.OrderByDescending(item => item.CreateDate).ToList();
            DataTable newOrder = Utility.ListToDataTable(RList);
            if (newOrder != null && newOrder.Rows.Count > 0)
            {
                var jsonString = Utility.DataTableToJsonString(newOrder);
                return OperContext.PackagingAjaxMsg(AjaxStatu.ok, "", jsonString);
            }
            else
            {
                return OperContext.PackagingAjaxMsg(AjaxStatu.none, "");
            }
        }

        /// <summary>
        /// 通过/驳回申请职位
        /// </summary>
        /// <returns></returns>
        public ActionResult UpdateCareRole()
        {
            //验证登录
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("SignIn", "Home");
            }
            var userid = Request["ID"];
            var flag = Request["flag"];
            if (flag == "1")
            {
                int count = AdminsBLL.UpdRoleStatus(userid, Convert.ToInt32(flag));
                if (count > 0)
                {
                    return OperContext.PackagingAjaxMsg(AjaxStatu.ok, "申请成功");
                }
                else
                {
                    return OperContext.PackagingAjaxMsg(AjaxStatu.err, "申请失败");
                }
            }
            else 
            {
                int count = AdminsBLL.DelPart(userid);
                if (count > 0)
                {
                    int cnt = AdminsBLL.UpdOldRoleStatus(userid);
                    if (cnt > 0)
                    {
                        return OperContext.PackagingAjaxMsg(AjaxStatu.ok, "职位驳回请重新申请");
                    }
                    else
                    {
                        return OperContext.PackagingAjaxMsg(AjaxStatu.err, "驳回失败");
                    }
                    
                }
                else
                {
                    return OperContext.PackagingAjaxMsg(AjaxStatu.err, "驳回失败");
                }
            }
           
        }
        /// <summary>
        /// 服务方详情
        /// </summary>
        /// <returns></returns>
        public ActionResult CareParsonDetail()
        {
            //验证登录
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("SignIn", "Home");
            }
            var userid = Request["userid"];
            var partdata = AdminsBLL.GetPartTimeData(userid);
            return View(partdata);
        }
        /// <summary>
        /// 订单列表
        /// </summary>
        /// <returns></returns>
        public ActionResult NeedOrderList()
        {
            //验证登录
            if (!User.Identity.IsAuthenticated)
            {                
                return RedirectToAction("SignIn", "Home");
            }
            var userid = Request["userid"];
            ViewBag.userid = userid;
            return View();
        }
        /// <summary>
        /// 订单列表数据
        /// </summary>
        /// <returns></returns>
        public ActionResult NeedOrderListData()
        {
            //验证登录
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("SignIn", "Home");
            }
            var userid = Request["userid"];
            List<OrderModel> MoneyList = AdminsBLL.GetOrderList();
            if (!string.IsNullOrEmpty(userid))
            {
                MoneyList = MoneyList.Where(item => item.AdminUserID == userid).ToList();
            }
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
        /// 添加服务人员
        /// </summary>
        /// <returns></returns>
        public ActionResult AddCareParson()
        {
            //验证登录
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("SignIn", "Home");
            }
            return View();
        }

        /// <summary>
        /// 添加服务人员数据交互
        /// </summary>
        /// <returns></returns>
        public ActionResult AddCareParsonData()
        {
            //验证登录
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("SignIn", "Home");
            }
            tb_User UserInfo = new tb_User();//前台会员
           
            UserInfo.LoginPwd = Request["LoginPwd"].ToString();
            UserInfo.UserPhone = Request["UserPhone"].ToString();
            UserInfo.RealName = Request["RealName"].ToString();
            UserInfo.Role =Convert.ToInt32(Request["Role"].ToString());
            UserInfo.NickName = Request["NickName"].ToString();
            UserInfo.LoginPwd = Utility.DesEncrypt(UserInfo.LoginPwd);//加密
            tb_PartTime pt = new tb_PartTime();//兼职实体
            var Post = Request["Post"];
            var education = Request["education"];
            var sex = Request["sex"];
            var work = Request["work"];
            var date_range = Request["date_range"];
            var serve1 = Request["serve1"];
            var serve2 = Request["serve2"];
            var salary = Request["salary"];
            var Vphone = HomeBLL.GetUserInfoByPhone(UserInfo.UserPhone);  //验证注册的手机号码唯一性  
            if (Vphone != null)
            {
                return OperContext.PackagingAjaxMsg(AjaxStatu.err, "当前手机号已注册，请联系该会员自行申请");
            }
            int count = AdminsBLL.AddUserInfoByAdmin(UserInfo);//注册前台会员
            if (count > 0)
            {
                //获取用户ID
                var userID = HomeBLL.GetUserInfoByPhone(UserInfo.UserPhone).ID.ToString();
                //var listdata = PartTimeBLL.GetPartTimeData(userID);

                pt.UserID = userID;
                pt.UserName = UserInfo.RealName;
                pt.Education = education;
                pt.YearWork = work;
                pt.DateRange = date_range;
                pt.RoomService = serve1;
                pt.AgencyService = serve2;
                pt.Salary = Convert.ToDecimal(salary);
                pt.Phone = UserInfo.UserPhone;
                pt.Post = Post;
                pt.SchoolName = null;
                pt.GraduationYear = null;
                pt.Sex = sex;
                int cnt = AdminsBLL.AddPartTimeInfoByAdmin(pt);
                if (cnt > 0)
                {
                    return OperContext.PackagingAjaxMsg(AjaxStatu.ok, "添加成功", null, "/MemberM/CareParson/");
                }
                else {
                    return OperContext.PackagingAjaxMsg(AjaxStatu.err, "添加失败,请注意前台用户表");
                }
               
            }
            else
            {
                return OperContext.PackagingAjaxMsg(AjaxStatu.err, "添加失败");
            }

           




           
        }
        #endregion

        #region ***派发需求给服务方***

        /// <summary>
        /// 派发需求给服务方
        /// </summary>
        /// <returns></returns>
        public ActionResult NeedToCareParson()
        {
            //验证登录
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("SignIn", "Home");
            }            
            var OrderID = Request["OrderID"];
            //获取需求数据
            List<OrderModel> OrderDetail = AdminsBLL.GetOrderList();
            if (!string.IsNullOrEmpty(OrderID))
            {
                OrderDetail = OrderDetail.Where(item => item.OrderID == OrderID).ToList();
            }
            //获取服务方人员数据
            List<tb_User> ParsonList = AdminsBLL.GetCareParsonList();
            if (OrderDetail[0].OrderType == "课后看护") {
                ParsonList = ParsonList.Where(item => item.RoleStatus == 1 && item.ID != OrderDetail[0].UserID && item.Role == 2).ToList();//0：普通会员；1：保姆；2：教师；3：陪护/看护
            }
            else if (OrderDetail[0].OrderType == "孕检陪护") {
                ParsonList = ParsonList.Where(item => item.RoleStatus == 1 && item.ID != OrderDetail[0].UserID && item.Role == 1).ToList();//0：普通会员；1：保姆；2：教师；3：陪护/看护
            }
            else if (OrderDetail[0].OrderType == "幼儿看护") {
                ParsonList = ParsonList.Where(item => item.RoleStatus == 1 && item.ID != OrderDetail[0].UserID && item.Role == 1).ToList();//0：普通会员；1：保姆；2：教师；3：陪护/看护
            }
            else if (OrderDetail[0].OrderType == "敬老看护") {
                ParsonList = ParsonList.Where(item => item.RoleStatus == 1 && item.ID != OrderDetail[0].UserID && item.Role == 3).ToList();//0：普通会员；1：保姆；2：教师；3：陪护/看护
            }
               
            ViewData["ParsonList"] = ParsonList;
            return View(OrderDetail);
        }
        /// <summary>
        /// 派发需求给服务方数据交互
        /// </summary>
        /// <returns></returns>
        public ActionResult NeedToCareParsonData()
        {
            //验证登录
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("SignIn", "Home");
            }
            var OrderID = Request["OrderID"];
            var AdminUserID = Request["AdminUserID"];
           
            int count = AdminsBLL.UpdOrderAdminUserID(OrderID, AdminUserID);
            if (count > 0)
            {
                int cnt = AdminsBLL.UpdOrderStatus(OrderID, 2);
                if (cnt > 0)
                {
                    return OperContext.PackagingAjaxMsg(AjaxStatu.ok, "服务人员派发成功", null, "/MemberM/CheckedCare/");
                }
                else
                {
                    return OperContext.PackagingAjaxMsg(AjaxStatu.err, "需求状态更新失败", null, "/MemberM/CheckedCare/");
                }   

            }
            else
            {
                return OperContext.PackagingAjaxMsg(AjaxStatu.err, "服务人员派发失败");
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
    public class jsonlist
    {
        public object pic { get; set; }
        public object video { get; set; }
    }
}