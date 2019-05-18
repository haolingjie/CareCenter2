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

namespace CareCenter2.Web.Controllers
{
    public class DemandController : Controller
    {
        #region 课后看护 开始
        // GET: Demand
        /// <summary>
        /// 课后看护
        /// </summary>
        /// <returns></returns>
        public ActionResult ClassCare()
        {
            return View();
        }
        /// <summary>
        /// 课后看护数据查询
        /// </summary>
        public ActionResult ClassCareData()
        {
            List<OrderModel> DataList = UserBLL.GetOrderRecordList();
            DataList = DataList.Where(item => item.OrderType == "共享保姆/课后托管照护" && item.Status >= 3).OrderByDescending(item => item.CreateDate).ToList();
            DataList = DataList.Distinct(new ListComparer<OrderModel>((p1, p2) => (p1.OrderID == p2.OrderID))).ToList();
            if (DataList != null && DataList.Count > 0)
            {
                DataTable NewDT = JsonHelper.ListToDataTable(DataList);
                var jsonString = JsonHelper.DataTableToJsonString(NewDT);
                return OperContext.PackagingAjaxMsg(AjaxStatu.ok, "", jsonString);
            }
            else
            {
                return OperContext.PackagingAjaxMsg(AjaxStatu.none, "");
            }
        }
       
        #endregion

        #region 幼儿看护 开始
        /// <summary>
        /// 幼儿看护
        /// </summary>
        /// <returns></returns>
        public ActionResult InfantCare()
        {
            return View();
        }
        /// <summary>
        /// 幼儿看护数据查询
        /// </summary>
        /// <returns></returns>
        public ActionResult InfantCareData()
        {
            List<OrderModel> DataList = UserBLL.GetOrderRecordList();
            DataList = DataList.Where(item => item.OrderType == "婴幼照护" && item.Status >= 3).OrderByDescending(item => item.CreateDate).ToList();
            DataList = DataList.Distinct(new ListComparer<OrderModel>((p1, p2) => (p1.OrderID == p2.OrderID))).ToList();
            if (DataList != null && DataList.Count > 0)
            {
                DataTable NewDT = JsonHelper.ListToDataTable(DataList);
                var jsonString = JsonHelper.DataTableToJsonString(NewDT);
                return OperContext.PackagingAjaxMsg(AjaxStatu.ok, "", jsonString);
            }
            else
            {
                return OperContext.PackagingAjaxMsg(AjaxStatu.none, "");
            }
        }
        #endregion

        #region 24小时智能照护平台(幼儿) 开始
        /// <summary>
        /// 幼儿看护
        /// </summary>
        /// <returns></returns>
        public ActionResult InfantCare_hour()
        {
            return View();
        }
        /// <summary>
        /// 幼儿看护数据查询
        /// </summary>
        /// <returns></returns>
        public ActionResult InfantCare_hourData()
        {
            List<OrderModel> DataList = UserBLL.GetOrderRecordList();
            DataList = DataList.Where(item => item.OrderType == "幼儿看护").OrderByDescending(item => item.CreateDate).ToList();
            DataList = DataList.Distinct(new ListComparer<OrderModel>((p1, p2) => (p1.OrderID == p2.OrderID))).ToList();
            if (DataList != null && DataList.Count > 0)
            {
                DataTable NewDT = JsonHelper.ListToDataTable(DataList);
                var jsonString = JsonHelper.DataTableToJsonString(NewDT);
                return OperContext.PackagingAjaxMsg(AjaxStatu.ok, "", jsonString);
            }
            else
            {
                return OperContext.PackagingAjaxMsg(AjaxStatu.none, "");
            }
        }
        #endregion

        #region 全方位早教服务 开始
        /// <summary>
        /// 幼儿看护
        /// </summary>
        /// <returns></returns>
        public ActionResult InfantCare_all()
        {
            return View();
        }
        /// <summary>
        /// 幼儿看护数据查询
        /// </summary>
        /// <returns></returns>
        public ActionResult InfantCare_allData()
        {
            List<OrderModel> DataList = UserBLL.GetOrderRecordList();
            DataList = DataList.Where(item => item.OrderType == "幼儿看护").OrderByDescending(item => item.CreateDate).ToList();
            DataList = DataList.Distinct(new ListComparer<OrderModel>((p1, p2) => (p1.OrderID == p2.OrderID))).ToList();
            if (DataList != null && DataList.Count > 0)
            {
                DataTable NewDT = JsonHelper.ListToDataTable(DataList);
                var jsonString = JsonHelper.DataTableToJsonString(NewDT);
                return OperContext.PackagingAjaxMsg(AjaxStatu.ok, "", jsonString);
            }
            else
            {
                return OperContext.PackagingAjaxMsg(AjaxStatu.none, "");
            }
        }
        #endregion

        #region 敬老看护 开始
        /// <summary>
        /// 敬老看护
        /// </summary>
        /// <returns></returns>
        public ActionResult OldAgeCare()
        {
            return View();
        }
        /// <summary>
        /// 敬老看护数据查询
        /// </summary>
        /// <returns></returns>
        public ActionResult OldAgeCareData()
        {
            List<OrderModel> DataList = UserBLL.GetOrderRecordList();
            DataList = DataList.Where(item => item.OrderType == "养老照护" && item.Status >= 3).OrderByDescending(item => item.CreateDate).ToList();
            DataList = DataList.Distinct(new ListComparer<OrderModel>((p1, p2) => (p1.OrderID == p2.OrderID))).ToList();
            if (DataList != null && DataList.Count > 0)
            {
                DataTable NewDT = JsonHelper.ListToDataTable(DataList);
                var jsonString = JsonHelper.DataTableToJsonString(NewDT);
                return OperContext.PackagingAjaxMsg(AjaxStatu.ok, "", jsonString);
            }
            else
            {
                return OperContext.PackagingAjaxMsg(AjaxStatu.none, "");
            }
        }
        #endregion

        #region 360°养老养护住区 开始
        /// <summary>
        /// 敬老看护
        /// </summary>
        /// <returns></returns>
        public ActionResult OldAgeCare_year()
        {
            return View();
        }
        /// <summary>
        /// 敬老看护数据查询
        /// </summary>
        /// <returns></returns>
        public ActionResult OldAgeCare_yearData()
        {
            List<OrderModel> DataList = UserBLL.GetOrderRecordList();
            DataList = DataList.Where(item => item.OrderType == "360度颐养养老照护").OrderByDescending(item => item.CreateDate).ToList();
            DataList = DataList.Distinct(new ListComparer<OrderModel>((p1, p2) => (p1.OrderID == p2.OrderID))).ToList();
            if (DataList != null && DataList.Count > 0)
            {
                DataTable NewDT = JsonHelper.ListToDataTable(DataList);
                var jsonString = JsonHelper.DataTableToJsonString(NewDT);
                return OperContext.PackagingAjaxMsg(AjaxStatu.ok, "", jsonString);
            }
            else
            {
                return OperContext.PackagingAjaxMsg(AjaxStatu.none, "");
            }
        }
        #endregion

        #region 全方位健康颐养服务 开始
        /// <summary>
        /// 敬老看护
        /// </summary>
        /// <returns></returns>
        public ActionResult OldAgeCare_all()
        {
            return View();
        }
        /// <summary>
        /// 敬老看护数据查询
        /// </summary>
        /// <returns></returns>
        public ActionResult OldAgeCare_allData()
        {
            List<OrderModel> DataList = UserBLL.GetOrderRecordList();
            DataList = DataList.Where(item => item.OrderType == "全方位健康颐养服务").OrderByDescending(item => item.CreateDate).ToList();
            DataList = DataList.Distinct(new ListComparer<OrderModel>((p1, p2) => (p1.OrderID == p2.OrderID))).ToList();
            if (DataList != null && DataList.Count > 0)
            {
                DataTable NewDT = JsonHelper.ListToDataTable(DataList);
                var jsonString = JsonHelper.DataTableToJsonString(NewDT);
                return OperContext.PackagingAjaxMsg(AjaxStatu.ok, "", jsonString);
            }
            else
            {
                return OperContext.PackagingAjaxMsg(AjaxStatu.none, "");
            }
        }
        #endregion

        #region 24小时智能照护平台 开始
        /// <summary>
        /// 敬老看护
        /// </summary>
        /// <returns></returns>
        public ActionResult OldAgeCare_hour()
        {
            return View();
        }
        /// <summary>
        /// 敬老看护数据查询
        /// </summary>
        /// <returns></returns>
        public ActionResult OldAgeCare_hourData()
        {
            List<OrderModel> DataList = UserBLL.GetOrderRecordList();
            DataList = DataList.Where(item => item.OrderType == "24小时智能照护平台").OrderByDescending(item => item.CreateDate).ToList();
            DataList = DataList.Distinct(new ListComparer<OrderModel>((p1, p2) => (p1.OrderID == p2.OrderID))).ToList();
            if (DataList != null && DataList.Count > 0)
            {
                DataTable NewDT = JsonHelper.ListToDataTable(DataList);
                var jsonString = JsonHelper.DataTableToJsonString(NewDT);
                return OperContext.PackagingAjaxMsg(AjaxStatu.ok, "", jsonString);
            }
            else
            {
                return OperContext.PackagingAjaxMsg(AjaxStatu.none, "");
            }
        }
        #endregion

        #region 双向营养餐饮服务 开始
        /// <summary>
        /// 敬老看护
        /// </summary>
        /// <returns></returns>
        public ActionResult OldAgeCare_food()
        {
            return View();
        }
        /// <summary>
        /// 敬老看护数据查询
        /// </summary>
        /// <returns></returns>
        public ActionResult OldAgeCare_foodData()
        {
            List<OrderModel> DataList = UserBLL.GetOrderRecordList();
            DataList = DataList.Where(item => item.OrderType == "双向营养餐饮服务").OrderByDescending(item => item.CreateDate).ToList();
            DataList = DataList.Distinct(new ListComparer<OrderModel>((p1, p2) => (p1.OrderID == p2.OrderID))).ToList();
            if (DataList != null && DataList.Count > 0)
            {
                DataTable NewDT = JsonHelper.ListToDataTable(DataList);
                var jsonString = JsonHelper.DataTableToJsonString(NewDT);
                return OperContext.PackagingAjaxMsg(AjaxStatu.ok, "", jsonString);
            }
            else
            {
                return OperContext.PackagingAjaxMsg(AjaxStatu.none, "");
            }
        }
        #endregion

        #region 孕检陪护 开始
        /// <summary>
        /// 孕检陪护
        /// </summary>
        /// <returns></returns>
        public ActionResult GravidaCare()
        {
            return View();
        }
        /// <summary>
        /// 孕检陪护数据查询
        /// </summary>
        /// <returns></returns>
        public ActionResult GravidaCareData()
        {
            List<OrderModel> DataList = UserBLL.GetOrderRecordList();
            DataList = DataList.Where(item => item.OrderType == "孕产照护" && item.Status >= 3).OrderByDescending(item => item.CreateDate).ToList();
            DataList = DataList.Distinct(new ListComparer<OrderModel>((p1, p2) => (p1.OrderID == p2.OrderID))).ToList();
            if (DataList != null && DataList.Count > 0)
            {
                DataTable NewDT = JsonHelper.ListToDataTable(DataList);
                var jsonString = JsonHelper.DataTableToJsonString(NewDT);
                return OperContext.PackagingAjaxMsg(AjaxStatu.ok, "", jsonString);
            }
            else
            {
                return OperContext.PackagingAjaxMsg(AjaxStatu.none, "");
            }
        }
        #endregion

        #region 需求详情 开始
        /// <summary>
        /// 需求详情
        /// </summary>
        /// <returns></returns>
        public ActionResult NeedDetail()
        {
            //获取详情
            var OrderID = Request["OrderID"];//订单编号
            var type = Request["type"];//1：共享保姆/课后托管照护；2：婴幼照护；3：养老照护；4：孕检照护
            List<OrderModel> DataList = new List<OrderModel>();
            //获取图片
            List<tb_Pictuer> PicList = UserBLL.GetPicList();
            //获取视频
            List<tb_Video> VideoList = UserBLL.GetVideoList();

            if (!string.IsNullOrEmpty(OrderID))
            {
                DataList = UserBLL.GetOrderRecordList();
                DataList = DataList.Where(item => item.OrderID == OrderID).ToList();
                //图片
                PicList = PicList.Where(item => item.OrderID == OrderID).OrderByDescending(item => item.CreateDate).ToList();
                ViewData["PicList"] = PicList;
                //视频
                VideoList = VideoList.Where(item => item.OrderID == OrderID).OrderByDescending(item => item.CreateDate).ToList();
                ViewData["VideoList"] = VideoList;
            }
            ViewBag.type = type;
            return View(DataList);
        }
        #endregion

        #region 新需求提交
        /// <summary>
        /// 新需求
        /// </summary>
        /// <returns></returns>
        public ActionResult NewNeed()
        {
            var type = Request["type"];//1：共享保姆/课后托管照护；2：婴幼照护；3：养老照护；4：孕检照护
            ViewBag.typenew = type;
            return View();
        }

        /// <summary>
        /// 提交需求
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult SetOwnNeedData()
        {

            var Title = Request["Title"];
            var Memo = Request["Memo"];
            var VideoUrl = Request["VideoUrl"];
            var ImgUrl = Request["ImgUrl"];
            var OrderClassify = Request["OrderClassify"];
            var OrderType = Request["OrderType"];
            var OrderIntro = Request["OrderIntro"];
            var OrderID = Utility.GetNewGUID();
            var PersonInro = Request["PersonInro"];//被照护人情况
            var Address = Request["Address"];
            var CeraRange = Request["CeraRange"];
            var DateType = Request["DateType"];// 照护方式  如：4H（半天）、10h(白天)、24h（全天）
            var CarePerson = Request["CarePerson"];
            var CareAddress = Request["CareAddress"];
            int picCnt = 0;//图片
            int videoCnt = 0;//视频
            var userphone = User.Identity.Name;
            var userInfo = HomeBLL.GetUserInfoByPhone(userphone); ;//获取用户信息

            //需求数据
            tb_Order order = new tb_Order();
            order.UserID = userInfo.ID;
            order.OrderID = OrderID;
            order.OrderTitle = Title;
            order.OrderType = OrderType;
            order.OrderClassify = Convert.ToInt32(OrderClassify);
            order.Memo = Memo;
            order.OrderIntor = OrderIntro;
            order.PersonInro = PersonInro;
            order.Address = Address;
            order.CeraRange = CeraRange;
            order.DateType = DateType;
            order.CarePerson = CarePerson;
            order.CareAddress = CareAddress;




            int count = DemandBLL.OwnNeedDataIns(order);
            var ordertemp = UserBLL.GetOrderDataBYOrderID(OrderID);
            if (count > 0 && ordertemp != null)
            {
                //图片
                if (!string.IsNullOrEmpty(ImgUrl))
                {
                    tb_Pictuer pic = new tb_Pictuer();
                    pic.OrderID = OrderID;
                    pic.PicUrl = ImgUrl;
                    picCnt = DemandBLL.PictureIns(pic);
                }

                //视频
                if (!string.IsNullOrEmpty(VideoUrl))
                {
                    tb_Video video = new tb_Video();
                    video.OrderID = OrderID;
                    video.VideoUrl = VideoUrl;
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

                return OperContext.PackagingAjaxMsg(AjaxStatu.ok, "需求提交成功请至个人中心查看!", null, "/User/OwnNeed/");
            }
            else
            {
                return OperContext.PackagingAjaxMsg(AjaxStatu.err, "需求提交失败,请联系管理员！");
            }
        }
        #endregion
    }
}