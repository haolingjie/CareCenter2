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

namespace CareCenter2.Web.Controllers
{
    public class HomeController : Controller
    {
        /// <summary>
        /// 首页
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            //验证登录
            if (User.Identity.IsAuthenticated)
            {
                var cookie = Request.Cookies[FormsAuthentication.FormsCookieName];
                var ticket = FormsAuthentication.Decrypt(cookie.Value);
                var userimg = ticket.UserData;
                if (userimg == null || userimg == "")
                {
                    Session["HeadImgUrl"] = "/Content/img/HeadImg/login.png";
                }
                else
                {
                    Session["HeadImgUrl"] = userimg;
                }
            }
            var dateList = DemandBLL.GetPicList().Where(item => item.OrderID == "主轮播图").ToList();
            dateList = dateList.OrderByDescending(item => item.CreateDate).ToList();
            var zhutu = DemandBLL.GetPicList().Where(item => item.OrderID == "需求主图").ToList();
            zhutu = zhutu.OrderByDescending(item => item.CreateDate).ToList();
            var ClassCare = zhutu.Where(t => t.PicTitle == "课后托管").ToList();//共享保姆/课后托管照护
            ViewData["ClassCare"] = ClassCare;
            var InfantCare = zhutu.Where(t => t.PicTitle == "婴幼照护").ToList();//婴幼照护
            ViewData["InfantCare"] = InfantCare;
            var OldAgeCare = zhutu.Where(t => t.PicTitle == "养老照护").ToList();//养老照护
            ViewData["OldAgeCare"] = OldAgeCare;
            var GravidaCare = zhutu.Where(t => t.PicTitle == "孕检照护").ToList();//孕检照护
            ViewData["GravidaCare"] = GravidaCare;
            //var GravidaCare = DemandBLL.GetPicList().Where(item => item.OrderID == "婴幼轮播").ToList();
            //ViewData["lunbo"] = lunbo;

            return View(dateList);
        }
        /// <summary>
        /// 登录
        /// </summary>
        /// <returns></returns>
        public ActionResult Login(string id)
        {
            //点击注册或登录按钮
            var status = id == null ? "" : id;
            ViewBag.msg = status;
            return View();
        }
        /// <summary>
        /// 登录数据交互
        /// </summary>
        /// <returns></returns>
        public ActionResult LoginIn()
        {
            //获取页面重定向地址
            var param = string.Empty;//重定向地址
            var ReturnUrl = HttpContext.Request.UrlReferrer.Query;//获取重定向URL
            if (!string.IsNullOrEmpty(ReturnUrl))
            {
                var split = ReturnUrl.Split('=');
                if (split.Length > 2)
                {
                    param = split[1] + "=" + split[2];//截取重定向URL
                }
                else
                {
                    param = ReturnUrl.Split('=')[1];//截取重定向URL
                }
            }
            else
            {
                param = "/User/Index/";
            }


            var txtAdmin = Request["AdminID"];//用户名
            var txtPwd = Request["AdminPwd"];//密码
            txtPwd = Utility.DesEncrypt(txtPwd);//加密
            //判断登录是否成功
            bool loginFalg = HomeBLL.GetUserInfo(txtAdmin, txtPwd);
            if (loginFalg)//登录成功
            {
                var ID = string.Empty;//用户ID
                var userphone = string.Empty;//手机号码  
                var HeaderImg = string.Empty;//头像
                DateTime LastLoginTime;//上次登录时间                    

                //查询用户信息
                var userInfo = HomeBLL.GetLoginInfo(txtAdmin, txtPwd);
                if (userInfo != null)
                {

                    ID = userInfo.ID.ToString();
                    userphone = userInfo.UserPhone;
                    HeaderImg = userInfo.ImgUrl == null ? "/Content/img/HeadImg/login.png" : userInfo.ImgUrl;
                    LastLoginTime = System.DateTime.Now;// SqlHelper.GetDateTime(dt, 0, "LastLoginTime");
                    //更新登录时间和登录次数
                    var AdminsUpd = HomeBLL.UpdateLastLoginTime(ID, LastLoginTime);

                }
                //登录信息写入cookie
                //FormsAuthentication.SetAuthCookie(userphone, false);
                FormsAuthenticationTicket ticket = new FormsAuthenticationTicket(1, userphone, DateTime.Now, DateTime.Now.AddDays(7), true, HeaderImg);
                FormsIdentity identity = new FormsIdentity(ticket);
                HttpCookie cookie = new HttpCookie(FormsAuthentication.FormsCookieName, FormsAuthentication.Encrypt(ticket));
                if (ticket.IsPersistent)
                {
                    cookie.Expires = ticket.Expiration;
                }
                Response.Cookies.Add(cookie);
                Session["HeadImgUrl"] = userInfo.ImgUrl == null ? "/Content/img/HeadImg/login.png" : userInfo.ImgUrl;
                return OperContext.PackagingAjaxMsg(AjaxStatu.ok, "登录成功", null, param);
            }
            else
            {
                return OperContext.PackagingAjaxMsg(AjaxStatu.err, "用户名或密码不正确");
            }
        }
        /// <summary>
        /// 注册
        /// </summary>
        /// <returns></returns>
        public ActionResult Register()
        {
            return View();
        }
        /// <summary>
        /// 注册数据交互
        /// </summary>
        /// <returns></returns>
        public ActionResult RegisterIn()
        {
            tb_User UserInfo = new tb_User();
            UserInfo.LoginPwd = Request["txtUserPwd"].ToString();
            UserInfo.UserPhone = Request["txtPhone"].ToString();
            UserInfo.NickName = Request["txtNick"].ToString();
            UserInfo.LoginPwd = Utility.DesEncrypt(UserInfo.LoginPwd);//加密
            var ValidCode = Request["txtValidCode"].ToString();//短信验证码
            if (System.Web.HttpContext.Current.Session["RegCode"] == null)
            {
                return OperContext.PackagingAjaxMsg(AjaxStatu.err, "验证码过期，请重新发送！");
            }
            if (System.Web.HttpContext.Current.Session["RegCode"].ToString() != ValidCode)
            {
                return OperContext.PackagingAjaxMsg(AjaxStatu.err, "验证码错误");
            }
            if (Convert.ToDateTime(Session["Vdate"]) < DateTime.Now.AddMinutes(-2))
            {
                return OperContext.PackagingAjaxMsg(AjaxStatu.err, "验证码过期，请重新发送");
            }
            //验证注册的手机号码唯一性
            var Vphone = HomeBLL.GetUserInfoByPhone(UserInfo.UserPhone);
            if (Vphone != null)
            {
                return OperContext.PackagingAjaxMsg(AjaxStatu.err, "当前手机号已存在，请换更换");
            }

            int count = HomeBLL.SetUserInfo(UserInfo);//主表
            if (count > 0)
            {
                //登录信息写入cookie
                //FormsAuthentication.SetAuthCookie(userphone, false);
                FormsAuthenticationTicket ticket = new FormsAuthenticationTicket(1, UserInfo.UserPhone, DateTime.Now, DateTime.Now.AddDays(7), true, UserInfo.ImgUrl);
                FormsIdentity identity = new FormsIdentity(ticket);
                HttpCookie cookie = new HttpCookie(FormsAuthentication.FormsCookieName, FormsAuthentication.Encrypt(ticket));
                if (ticket.IsPersistent)
                {
                    cookie.Expires = ticket.Expiration;
                }
                Response.Cookies.Add(cookie);
                return OperContext.PackagingAjaxMsg(AjaxStatu.ok, "注册成功", null, "/User/Index/");
            }
            else
            {
                return OperContext.PackagingAjaxMsg(AjaxStatu.err, "注册失败");
            }
        }
        /// <summary>
        /// 发送手机验证码(注册)
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult PhoneCode()
        {
            string phone = Request["phone"].ToString();//取现接受验证码手机号码            
            //验证注册的手机号码唯一性
            var Vphone = HomeBLL.GetUserInfoByPhone(phone);
            if (Vphone != null && Vphone.UserPhone != "")
            {
                return OperContext.PackagingAjaxMsg(AjaxStatu.err, "当前手机号已存在，请换更换");
            }
            DateTime Vdate;
            //Session["RegCode"] = "123456";//验证码
            //Session["Vdate"] = DateTime.Now;//验证码发送时间
            //return OperContext.PackagingAjaxMsg(AjaxStatu.ok, "验证码已发送您的手机,请获取");

            string count = SendPhoneCode.SendVerifyCode(phone, out Vdate);//发送验证码
            if (count != "")
            {
                Session["RegCode"] = count;//验证码
                Session["Vdate"] = Vdate;//验证码发送时间
                return OperContext.PackagingAjaxMsg(AjaxStatu.ok, "验证码已发送您的手机,请获取");

            }
            else
            {
                return OperContext.PackagingAjaxMsg(AjaxStatu.err, "验证码发送失败");
            }

        }
        /// <summary>
        /// 发送手机验证码(重置密码)
        /// </summary>
        /// <returns></returns>
        public ActionResult ResetPhoneCode()
        {
            string phone = Request["phone"].ToString();//取现接受验证码手机号码            
            //验证注册的手机号码唯一性
            var Vphone = HomeBLL.GetUserInfoByPhone(phone);
            if (Vphone == null)
            {
                return OperContext.PackagingAjaxMsg(AjaxStatu.err, "当前手机号不存在，请确认");
            }
            DateTime Vdate;
            //Session["RegCode"] = "123456";//验证码
            //Session["Vdate"] = DateTime.Now;//验证码发送时间
            //return OperContext.PackagingAjaxMsg(AjaxStatu.ok, "验证码已发送您的手机,请获取");

            string count = SendPhoneCode.SendVerifyCode(phone, out Vdate);//发送验证码
            if (count != "")
            {
                Session["RegCode"] = count;//验证码
                Session["Vdate"] = Vdate;//验证码发送时间
                return OperContext.PackagingAjaxMsg(AjaxStatu.ok, "验证码已发送您的手机,请获取");

            }
            else
            {
                return OperContext.PackagingAjaxMsg(AjaxStatu.err, "验证码发送失败");
            }
        }
        /// <summary>
        /// 密码重置
        /// </summary>
        /// <returns></returns>
        public ActionResult ResetPwd()
        {
            var LoginPwd = Request["txtUserPwd"].ToString();
            var UserPhone = Request["txtPhone"].ToString();
            LoginPwd = Utility.DesEncrypt(LoginPwd);//加密
            var ValidCode = Request["txtValidCode"].ToString();//短信验证码
            if (System.Web.HttpContext.Current.Session["RegCode"] == null)
            {
                return OperContext.PackagingAjaxMsg(AjaxStatu.err, "验证码过期，请重新发送！");
            }
            if (System.Web.HttpContext.Current.Session["RegCode"].ToString() != ValidCode)
            {
                return OperContext.PackagingAjaxMsg(AjaxStatu.err, "验证码错误");
            }
            if (Convert.ToDateTime(Session["Vdate"]) < DateTime.Now.AddMinutes(-2))
            {
                return OperContext.PackagingAjaxMsg(AjaxStatu.err, "验证码过期，请重新发送");
            }

            int count = HomeBLL.UpdUserPwd(UserPhone, LoginPwd);
            if (count > 0)
            {

                return OperContext.PackagingAjaxMsg(AjaxStatu.ok, "密码重置成功", null, "/Home/Login/");
            }
            else
            {
                return OperContext.PackagingAjaxMsg(AjaxStatu.err, "密码重置失败");
            }
        }


        /// <summary>
        /// 用户隐私条款
        /// </summary>
        /// <returns></returns>
        public ActionResult Terms()
        {
            return View();
        }
        /// <summary>
        /// 表单
        /// </summary>
        /// <returns></returns>
        public ActionResult UploadOrder()
        {
            return View();
        }
        /// <summary>
        /// 广告业务
        /// </summary>
        /// <returns></returns>
        public ActionResult Introduce()
        {
            return View();
        }

        #region 修改密码 开始
        /// <summary>
        /// 修改密码
        /// </summary>
        /// <returns></returns>
        public ActionResult UpdUserWpd()
        {
            return View();
        }
        /// <summary>
        /// 修改密码数据交互
        /// </summary>
        /// <returns></returns>
        public ActionResult UpdUserWpdData()
        {
            var phone = User.Identity.Name;
            var UserPhone = Request["UserPhone"];
            var LoginPwd = Request["LoginPwd"];
            var newLoginPwd = Request["newLoginPwd"];
            LoginPwd = Utility.DesEncrypt(LoginPwd);
            newLoginPwd = Utility.DesEncrypt(newLoginPwd);
            if (UserPhone != phone)
            {
                return OperContext.PackagingAjaxMsg(AjaxStatu.err, "您要修改的账号与手机号码不匹配");
            }
            var user = HomeBLL.GetLoginInfo(UserPhone, LoginPwd);
            if (user != null)
            {
                int count = HomeBLL.UpdUserPwd(UserPhone, newLoginPwd);
                if (count > 0)
                {
                    return OperContext.PackagingAjaxMsg(AjaxStatu.ok, "密码修改成功", null, "/User/Index/");
                }
                else
                {
                    return OperContext.PackagingAjaxMsg(AjaxStatu.err, "密码修改失败");
                }
            }
            else
            {
                return OperContext.PackagingAjaxMsg(AjaxStatu.err, "原始密码不正确");
            }

        }
        #endregion



        #region ***退出登录 ***
        public ActionResult LoginOut()
        {
            Session.Abandon();//清除全部Session
            //删除Forms验证票证
            FormsAuthentication.SignOut();
            //清除cookie
            Response.Cookies["cookie"].Expires = DateTime.Now.AddDays(-1);
            return RedirectToAction("Index", "Home");
        }
        #endregion

        #region ***公共方法***
        /// <summary>
        /// 验证当前手机号码是否存在
        /// </summary>
        /// <returns></returns>
        public ActionResult ChkPhoneExist()
        {
            string phone = Request["phone"].ToString();//取现接受验证码手机号码
            var aa = "";
            if (aa != null)
            {
                return OperContext.PackagingAjaxMsg(AjaxStatu.err, "当前手机号已存在，请换更换");
            }
            else
            {
                return OperContext.PackagingAjaxMsg(AjaxStatu.ok, "当前手机号不存在，可以注册");
            }
        }

        /// <summary>
        /// 验证当前等会账号是否存在
        /// </summary>
        /// <returns></returns>
        public ActionResult ChkAccountExist()
        {
            string usernmae = Request["usernmae"].ToString();//接受验证码手机号码
            var aa = "";
            if (aa != null)
            {
                return OperContext.PackagingAjaxMsg(AjaxStatu.err, "当前登录账号已存在，请换更换");
            }
            else
            {
                return OperContext.PackagingAjaxMsg(AjaxStatu.ok, "当前登录账号不存在，可以注册");
            }
        }
        /// <summary>
        /// 发送手机验证码()
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult SendCode()
        {
            string phone = Request["phone"].ToString();//取现接受验证码手机号码            
            //验证注册的手机号码唯一性
            //DataTable Vphone = UserBLL.Vphone(phone);
            //if (Vphone != null && Vphone.Rows.Count > 0)
            //{
            //    return OperContext.PackagingAjaxMsg(AjaxStatu.err, "当前手机号已存在，请换更换");
            //}
            DateTime Vdate = DateTime.Now;
            string count = SendPhoneCode.SendVerifyCode(phone, out Vdate);//发送验证码
            if (count != "")
            {
                Session["RegCode"] = count;//验证码
                Session["Vdate"] = Vdate;//验证码发送时间
                return OperContext.PackagingAjaxMsg(AjaxStatu.ok, "验证码已发送您的手机,请获取");

            }
            else
            {
                return OperContext.PackagingAjaxMsg(AjaxStatu.err, "验证码发送失败");
            }

        }
        /// <summary>
        /// 设置密码是验证码
        /// </summary>
        /// <returns></returns>
        public ActionResult SendCodePwd()
        {
            var phone = User.Identity.Name;//登陆者手机号码            
                                           //验证注册的手机号码唯一性

            if (phone == null && phone == "")
            {
                return OperContext.PackagingAjaxMsg(AjaxStatu.err, "当前用户未登录，请先登录");
            }
            DateTime Vdate = DateTime.Now;
            string count = SendPhoneCode.SendVerifyCode(phone, out Vdate);//发送验证码
            if (count != "")
            {
                Session["RegCode"] = count;//验证码
                Session["Vdate"] = Vdate;//验证码发送时间
                return OperContext.PackagingAjaxMsg(AjaxStatu.ok, "验证码已发送您的手机,请获取");

            }
            else
            {
                return OperContext.PackagingAjaxMsg(AjaxStatu.err, "验证码发送失败");
            }
        }
        #endregion ***公共方法***

        #region ---刷新信息 开始
        /// <summary>
        /// 刷新用户信息
        /// </summary>
        public static void RefUser()
        {
            if (System.Web.HttpContext.Current.User.Identity.IsAuthenticated)
            {
                var cookies = System.Web.HttpContext.Current.Request.Cookies[FormsAuthentication.FormsCookieName];
                var tickets = FormsAuthentication.Decrypt(cookies.Value);
                string role = tickets.UserData;
                if (!role.Contains("/"))
                {
                    System.Web.HttpContext.Current.Session.Abandon();//清除全部Session
                                                                     //删除Forms验证票证
                    FormsAuthentication.SignOut();
                    //清除cookie
                    System.Web.HttpContext.Current.Response.Cookies["cookie"].Expires = DateTime.Now.AddDays(-1);

                }
                else
                {
                    System.Web.HttpContext.Current.Session["HeadImgUrl"] = role;//头像
                }


            }
        }
        #endregion

    }
}