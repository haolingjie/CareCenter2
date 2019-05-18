using CareCenter2.BLL;
using CareCenter2.DB;
using CareCenter2.Util.Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using static CareCenter2.Util.Common.Utility;

namespace CareCenter2.Admin.Controllers
{
    public class UploadFileController : Controller
    {
        #region ***建设家园 开始***
        // GET: UploadFile
        /// <summary>
        /// 关于我们
        /// </summary>
        /// <returns></returns>
        public ActionResult AboutUs()
        {
            //验证登录
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("SignIn", "Home");
            }
            return View();
        }
        /// <summary>
        /// 获取数据
        /// </summary>
        /// <returns></returns>
        public ActionResult AboutUsData()
        {
            //验证登录
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("SignIn", "Home");
            }
            var search = Request["search"] == null ? "" : Request["search"].ToString();//搜索条件
            var type = Request["type"] == null ? "" : Request["type"].ToString();//类型
            List<tb_Context> RList = AdminsBLL.GetContextList();
            if (search != "")
            {

                RList = RList.Where(item => item.Title.Contains(search) ||
                                           item.Explain.Contains(search)).ToList();
            }
            if (type != "")
            {
                RList = RList.Where(item => item.Type == type).ToList();
            }
            RList = RList.OrderByDescending(item => item.CreateDate).ToList();
            //RList = RList.Distinct(new ListComparer<tb_Context>((p1, p2) => (p1.OrderID == p2.OrderID))).ToList();//去除重复数据
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
        /// 新建图文
        /// </summary>
        /// <returns></returns>
        public ActionResult AddContext()
        {
            //验证登录
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("SignIn", "Home");
            }
            return View();
        }

        /// <summary>
        /// 添加信息
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult AddContextData()
        {

            var Title = Request["Title"];
            var Explain = Request["Explain"];
            //var Memo = Request["Memo"];
            var Type = Request["Type"];
            var Intro = Request["Intro"];
            tb_Context con = new tb_Context();
            con.Title = Title;
            con.Explain = Explain;
            con.Type = Type;
            con.Intro = Intro;
            int count = AdminsBLL.AddContextData(con);
            if (count > 0)
            {
                return OperContext.PackagingAjaxMsg(AjaxStatu.ok, "图文信息提交成功!", null, "/UploadFile/AboutUs/");
               
            }
            else
            {
                return OperContext.PackagingAjaxMsg(AjaxStatu.err, "图文信息提交失败,请联系管理员！");
            }          
        }
        /// <summary>
        /// 修改信息
        /// </summary>
        /// <returns></returns>
        public ActionResult EidtContext()
        {
            //验证登录
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("SignIn", "Home");
            }
            //获取详情
            var ContextID = Request["ContextID"];//订单编号
            List<tb_Context> DataList = AdminsBLL.GetContextList();
           
            if (!string.IsNullOrEmpty(ContextID))
            {
                DataList = DataList.Where(item => item.ContextID == ContextID).ToList();

            }
            return View(DataList);          
        }

        /// <summary>
        /// 修改信息数据交互
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult EidtContextData()
        {
            //验证登录
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("SignIn", "Home");
            }
            var Title = Request["Title"];
            var Explain = Request["Explain"];
            var ContextID = Request["ContextID"];
            var Type = Request["Type"];
            var Intro = Request["Intro"];
            tb_Context con = new tb_Context();
            con.Title = Title;
            con.Explain = Explain;
            con.Type = Type;
            con.Intro = Intro;
            con.ContextID = ContextID;
            int count = AdminsBLL.UpdContextData(con);
            if (count > 0)
            {
                return OperContext.PackagingAjaxMsg(AjaxStatu.ok, "图文信息修改成功!", null, "/UploadFile/AboutUs/");

            }
            else
            {
                return OperContext.PackagingAjaxMsg(AjaxStatu.err, "图文信息修改失败");
            }
        }
        /// <summary>
        /// 删除文本
        /// </summary>
        /// <returns></returns>
        public ActionResult UpdateStatus()
        {
            //验证登录
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("SignIn", "Home");
            }
            var ContextID = Request["ContextID"];
            int count = AdminsBLL.DelContext(ContextID);
            if (count > 0)            {
                return OperContext.PackagingAjaxMsg(AjaxStatu.ok, "删除成功!");
            }
            else
            {
                return OperContext.PackagingAjaxMsg(AjaxStatu.err, "删除失败");
            }
        }
        #endregion

        #region ***图片上传 开始***
        /// <summary>
        /// 专有图片
        /// </summary>
        /// <returns></returns>
        public ActionResult UploadPicture()
        {
            //验证登录
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("SignIn", "Home");
            }
            return View();
        }
        /// <summary>
        /// 专有图片上传
        /// </summary>
        /// <returns></returns>
        public ActionResult UploadPictureData()
        {
            //验证登录
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("SignIn", "Home");
            }
            //var search = Request["search"] == null ? "" : Request["search"].ToString();//搜索条件
            var type = Request["type"] == null ? "" : Request["type"].ToString();//类型
            List<tb_Pictuer> RList = AdminsBLL.GetPicList().Where(item=>Utility.CheckStringChineseReg(item.OrderID)==true).ToList();
            //if (search != "")
            //{
            //    RList = RList.Where(item => item.PicTitle.Contains(search) ||
            //                               item.Memo.Contains(search)).ToList();
            //}
            if (type != "")
            {
                RList = RList.Where(item => item.OrderID == type).ToList();
            }
            RList = RList.OrderByDescending(item => item.CreateDate).ToList();
            //RList = RList.Distinct(new ListComparer<tb_Context>((p1, p2) => (p1.OrderID == p2.OrderID))).ToList();//去除重复数据
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

        #region ***精彩瞬间 开始***
        /// <summary>
        /// 精彩瞬间
        /// </summary>
        /// <returns></returns>
        public ActionResult AddMoment()
        {
            //验证登录
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("SignIn", "Home");
            }
            return View();
        }

        /// <summary>
        /// 上传多图片并发布新房源
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        public ActionResult UploadManyImgData(HttpPostedFileBase file)
        {
            //var TotalData = Request["TotalData"];//总数据
            var imgUrl = string.Empty;//写入数据库地址     
            HttpPostedFileBase ImgPath = file;// files["ImgPath"];//主图片
            string TempName = string.Empty;//图片新名称
           
            //文件存放位置
            string path = ConfigHelper.GetAppSettingString("PicPath");
            //存入文件新名称
            TempName = Utility.ImgUpload(ImgPath, path);

            //写入数据库地址
            imgUrl = ConfigHelper.GetAppSettingString("ImgPath") + TempName;
            //Session[""+ sessiondate + ""] = imgUrl;//存储上传图片地址

            var PicTitle = Request["PicTitle"];
            var Memo = Request["Memo"];//简介           
            //多图片插入
            tb_Pictuer MentPic = new tb_Pictuer();
            MentPic.PicTitle = PicTitle;
            MentPic.PicUrl = imgUrl;
            MentPic.OrderID = "精彩瞬间";           
            MentPic.Memo = Memo;           
            int count = DemandBLL.PictureIns(MentPic);
          
            if (count > 0)
            {
                return OperContext.PackagingAjaxMsg(AjaxStatu.ok, "精彩瞬间发布成功", null, "/UploadFile/UploadPicture/");
            }
            else
            {
                return OperContext.PackagingAjaxMsg(AjaxStatu.err, "精彩瞬间发布失败，请联系管理员！");
            }
            
        }
        #endregion

        #region ***轮播图 开始***
        /// <summary>
        /// 轮播图
        /// </summary>
        /// <returns></returns>
        public ActionResult AddLunBoPic()
        {
            //验证登录
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("SignIn", "Home");
            }           
            return View();
        }
        /// <summary>
        /// 轮播图数据
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult AddLunBoPicData()
        {       
            var Title = Request["Title"];
            var Memo = Request["Memo"];            
            var ImgUrl = Request["ImgUrl"];
            var OrderID = Request["OrderID"];
            int picCnt = 0;//图片           
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
            if (!string.IsNullOrEmpty(ImgUrl) && picCnt <= 0)
            {
                return OperContext.PackagingAjaxMsg(AjaxStatu.err, "轮播图提交失败,请联系管理员！");
            }           
            return OperContext.PackagingAjaxMsg(AjaxStatu.ok, "轮播图提交成功!", null, "/UploadFile/UploadPicture/");

        }
        #endregion

        #region ***需求主图 开始***
        /// <summary>
        /// 需求主图
        /// </summary>
        /// <returns></returns>
        public ActionResult AddXuQiuPic()
        {
            //验证登录
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("SignIn", "Home");
            }           
            return View();
        }

        /// <summary>
        /// 需求主图数据
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult AddXuQiuPicData()
        {
            var Title = Request["Title"];
            var Memo = Request["Memo"];           
            var ImgUrl = Request["ImgUrl"];           
            int picCnt = 0;//图片
           


            //图片
            if (!string.IsNullOrEmpty(ImgUrl))
            {
                tb_Pictuer pic = new tb_Pictuer();
                pic.PicTitle = Title;
                pic.OrderID = "需求主图";
                pic.PicUrl = ImgUrl;
                pic.Memo = Memo;
                picCnt = DemandBLL.PictureIns(pic);
            }

            
            if (!string.IsNullOrEmpty(ImgUrl) && picCnt <= 0)
            {
                return OperContext.PackagingAjaxMsg(AjaxStatu.err, "需求图片提交失败,请联系管理员！");
            }
            
            return OperContext.PackagingAjaxMsg(AjaxStatu.ok, "需求图片提交成功!", null, "/UploadFile/UploadPicture/");

        }
        #endregion


        /// <summary>
        /// 删除图片
        /// </summary>
        /// <returns></returns>
        public ActionResult DelPicTure()
        {
            //验证登录
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("SignIn", "Home");
            }
            var PicID = Request["PicID"];
            int count = AdminsBLL.DelPicData(PicID);
            if (count > 0)
            {
                return OperContext.PackagingAjaxMsg(AjaxStatu.ok, "删除成功!");
            }
            else
            {
                return OperContext.PackagingAjaxMsg(AjaxStatu.err, "删除失败");
            }
        }

        /// <summary>
        /// 修改图片标题/简介
        /// </summary>
        /// <returns></returns>
        public ActionResult EditPicTure()
        {
            //验证登录
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("SignIn", "Home");
            }
            var PicID = Request["PicID"];
            var PicTitle = Request["PicTitle"];
            var Memo = Request["Memo"];
            int count = AdminsBLL.EditPicData(PicID,PicTitle,Memo);
            if (count > 0)
            {
                return OperContext.PackagingAjaxMsg(AjaxStatu.ok, "修改成功!");
            }
            else
            {
                return OperContext.PackagingAjaxMsg(AjaxStatu.err, "修改失败");
            }
        }

        #endregion
    }
}