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

namespace CareCenter2.Admin.Controllers
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
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("SignIn", "Home");
            }
            //var admin = User.Identity.Name;
            //var adminData = AdminsBLL.GetAdminInfoByAdmin(admin);
            return View();
        }
        #region 登录  开始
        /// <summary>
        /// 登录
        /// </summary>
        /// <returns></returns>
        public ActionResult SignIn()
        {
           

            return View();
        }
        /// <summary>
        /// 登录数据验证
        /// </summary>
        /// <returns></returns>
        public ActionResult SignInData()
        {
            var AdminID = Request["AdminID"].ToString();
            var AdminPwd = Request["AdminPwd"].ToString();
            AdminPwd = Utility.DesEncrypt(AdminPwd);//加密

            bool adminCHK = AdminsBLL.ChkAdminInfo(AdminID, AdminPwd);
            if (adminCHK)
            {//登录成功
                int count = AdminsBLL.UpdateLastDateTime(AdminID);
                if (count < 0)
                {
                    return OperContext.PackagingAjaxMsg(AjaxStatu.err, "登录时间更新失败");
                }
                //获取管理员菜单
                //Session["AdminsMenu"] = AdminsBLL.GetAdminMenu(AdminID);
                //获取信息
                var adminData = AdminsBLL.GetAdminInfoByAdmin(AdminID);
                //管理员ID、
                Session["NickName"] = adminData.NickName;
                Session["Mobile"] = adminData.Mobile;
                Session.Timeout = 1000;
                //登录信息写入cookie
                //FormsAuthentication.SetAuthCookie(Phone, false);
                FormsAuthenticationTicket ticket = new FormsAuthenticationTicket(1, adminData.Mobile, DateTime.Now, DateTime.Now.AddDays(2), true, adminData.NickName);
                FormsIdentity identity = new FormsIdentity(ticket);
                HttpCookie cookie = new HttpCookie(FormsAuthentication.FormsCookieName, FormsAuthentication.Encrypt(ticket));
                if (ticket.IsPersistent) cookie.Expires = ticket.Expiration;
                Response.Cookies.Add(cookie);
                return OperContext.PackagingAjaxMsg(AjaxStatu.ok, "登录成功", null, "/Home/Index/");
            }
            else//登录失败
            {
                return OperContext.PackagingAjaxMsg(AjaxStatu.err, "用户名或密码错误！");

            }
        }
        #endregion

        #region 注册 开始
        public ActionResult SignUp()
        {
            //验证登录
            //if (!User.Identity.IsAuthenticated)
            //{
            //    return RedirectToAction("SignIn", "Home");
            //}
            var adminid = Utility.GetNewGUID_AdminID();
            ViewBag.adminid = adminid;
            return View();
        }
        /// <summary>
        /// 注册数据写入
        /// </summary>
        /// <returns></returns>
        public ActionResult RegisterIn()
        {
            //验证登录
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("SignIn", "Home");
            }
            var AdminID = Request["AdminID"];
            var AdminPwd = Request["AdminPwd"];
            var RealName = Request["RealName"];
            var Mobile = Request["Mobile"];
            var Memo = Request["Memo"];
            var NickName = Request["NickName"];
            AdminPwd = Utility.DesEncrypt(AdminPwd);
            var admin = AdminsBLL.GetAdminInfoByAdmin(Mobile);
            if (admin != null)
            {
                return OperContext.PackagingAjaxMsg(AjaxStatu.err, "该号码已注册,请更换手机");
            }
            tb_Admins am = new tb_Admins();
            am.AdminID = AdminID;
            am.AdminPwd = AdminPwd;
            am.RealName = RealName;
            am.Mobile = Mobile;
            am.NickName = NickName;           
            int count = AdminsBLL.Register(am);
            if (count > 0)
            {
                return OperContext.PackagingAjaxMsg(AjaxStatu.ok, "注册成功", null, "/Home/AdminList/");
            }
            else
            {
                return OperContext.PackagingAjaxMsg(AjaxStatu.err, "注册失败");
            }


        }
        #endregion

        #region 后台管理员 开始
        /// <summary>
        /// 获取管理员信息
        /// </summary>
        /// <returns></returns>
        public ActionResult AdminList()
        {
            //验证登录
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("SignIn", "Home");
            }
            return View();
        }

        /// <summary>
        /// 获取管理员信息数据
        /// </summary>
        /// <returns></returns>
        public ActionResult AdminListData()
        {
            //验证登录
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("SignIn", "Home");
            }
            var search = Request["search"] == null ? "" : Request["search"].ToString();//搜索条件
            List<tb_Admins> RList = AdminsBLL.GetAlladminInfo();
            if (search != "")
            {
                //RList = RList.Where(item => item.HouseName.Contains(search)).ToList();
                RList = RList.Where(item => item.NickName.Contains(search) ||
                                           item.Mobile.Contains(search) ||
                                           item.AdminID.Contains(search) ||
                                           item.RealName.Contains(search)).ToList();
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
        /// 启用/禁用管理员
        /// </summary>
        /// <returns></returns>
        public ActionResult UpdateMemberStatus()
        {
            //验证登录
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("SignIn", "Home");
            }
            var MemberID = Request["ID"];
            var flag = Request["Status"];
            if (flag == "1")
            {
                int count = AdminsBLL.UpdAdminStatus(MemberID, Convert.ToInt32(flag));
                if (count > 0)
                {
                    return OperContext.PackagingAjaxMsg(AjaxStatu.ok, "启用成功");
                }
                else
                {
                    return OperContext.PackagingAjaxMsg(AjaxStatu.err, "启用失败");
                }
            }
            else
            {
                int count = AdminsBLL.UpdAdminStatus(MemberID, Convert.ToInt32(flag));
                if (count > 0)
                {
                    return OperContext.PackagingAjaxMsg(AjaxStatu.ok, "禁用成功");
                }
                else
                {
                    return OperContext.PackagingAjaxMsg(AjaxStatu.err, "禁用失败");
                }
            }
        }
        /// <summary>
        /// 修改后台用户信息
        /// </summary>
        /// <returns></returns>
        public ActionResult UpdRegisterData()
        {
            //验证登录
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Login", "Home");
            }
            var ID = Request["ID"];
            var AdminID = Request["AdminID"];           
            var RealName = Request["RealName"];
            var Mobile = Request["Mobile"];
            var NickName = Request["NickName"];          
            tb_Admins am = new tb_Admins();
            am.ID = ID;
            am.AdminID = AdminID;            
            am.RealName = RealName;
            am.Mobile = Mobile;
            am.NickName = NickName;
            int count = AdminsBLL.UpdRegister(am);
            if (count > 0)
            {
                return OperContext.PackagingAjaxMsg(AjaxStatu.ok, "信息修改成功", null, "/Home/AdminList/");
            }
            else
            {
                return OperContext.PackagingAjaxMsg(AjaxStatu.err, "修改失败");
            }


        }
        #endregion

        #region 密码解密 开始
        /// <summary>
        /// 密码解密
        /// </summary>
        /// <returns></returns>
        public ActionResult DesDecode()
        {
            //验证登录
            //if (!User.Identity.IsAuthenticated)
            //{
            //    return RedirectToAction("SignIn", "Home");
            //}
            return View();
        }


        public ActionResult DesDecodeData()
        {
            //验证登录
            //if (!User.Identity.IsAuthenticated)
            //{
            //    return RedirectToAction("SignIn", "Home");
            //}
            string Pwd = string.Empty;
            var LoginPwd = Request["LoginPwd"];//密码           

            Pwd = Utility.DesDecrypt(LoginPwd);

            if (Pwd == null || Pwd == "")
            {
                return OperContext.PackagingAjaxMsg(AjaxStatu.err, "解密失败");
            }
            else
            {
                return OperContext.PackagingAjaxMsg(AjaxStatu.ok, "解密完成", Pwd);
            }
        }
        #endregion

        #region 修改登录密码 开始
        /// <summary>
        /// 修改密码
        /// </summary>
        /// <returns></returns>
        public ActionResult UpdAdminWpd()
        {
            //验证登录
            //if (!User.Identity.IsAuthenticated)
            //{
            //    return RedirectToAction("SignIn", "Home");
            //}           
            return View();
        }
        /// <summary>
        /// 修改密码数据交互
        /// </summary>
        /// <returns></returns>
        public ActionResult UpdAdminWpdData()
        {
            //验证登录
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("SignIn", "Home");
            }
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
            
            int count = AdminsBLL.UpdAdminPwd(UserPhone, LoginPwd, newLoginPwd);
            if (count > 0)
            {
                return OperContext.PackagingAjaxMsg(AjaxStatu.ok, "密码修改成功", null, "/Home/Index/");
            }
            else
            {
                return OperContext.PackagingAjaxMsg(AjaxStatu.err, "密码修改失败");
            }


        }
        #endregion

        #region ---管理员退出 开始---
        /// <summary>
        /// 管理员退出
        /// </summary>
        /// <returns></returns>
        public ActionResult LoginOut()
        {
            Session.Abandon();//清除全部Session
            //删除Forms验证票证
            FormsAuthentication.SignOut();
            //清除cookie
            Response.Cookies["cookie"].Expires = DateTime.Now.AddDays(-1);
            return RedirectToAction("SignIn", "Home");
        }

        #endregion ---管理员退出---

        #region ---管理员数据刷新 开始
        /// <summary>
        /// 刷新用户信息
        /// </summary>
        public static void RefAdmin()
        {
            if (System.Web.HttpContext.Current.User.Identity.IsAuthenticated)
            {
                var cookies = System.Web.HttpContext.Current.Request.Cookies[FormsAuthentication.FormsCookieName];
                var tickets = FormsAuthentication.Decrypt(cookies.Value);
                string role = tickets.UserData;
                if (role.Contains("/"))
                {
                    System.Web.HttpContext.Current.Session.Abandon();//清除全部Session
                    FormsAuthentication.SignOut(); //删除Forms验证票证
                    //清除cookie
                    System.Web.HttpContext.Current.Response.Cookies["cookie"].Expires = DateTime.Now.AddDays(-1);

                }
                else
                {
                    System.Web.HttpContext.Current.Session["NickName"] = role;
                }

                //获取信息
                var adminData = AdminsBLL.GetAdminInfoByAdmin(System.Web.HttpContext.Current.User.Identity.Name);
                if (adminData != null)
                {
                    if (adminData.Memo == "超级管理员")
                    {
                        System.Web.HttpContext.Current.Session["Role"] = true;
                    }
                    else
                    {
                        System.Web.HttpContext.Current.Session["Role"] = false;
                    }
                }

            }
            //else
            //{
            //     System.Web.HttpContext.Current.Response.Redirect("/Home/SignIn/");// RedirectToAction("SignIn", "Home");
            //}
        }
        #endregion
    }
}