using CareCenter2.DB;
using CareCenter2.Util.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CareCenter2.BLL
{
    /// <summary>
    /// 后台业务逻辑
    /// </summary>
    public class AdminsBLL
    {
        #region ***管理员验证登录--首页
        /// <summary>
        /// 验证管理员登录信息
        /// </summary>
        /// <param name="adminid"></param>
        /// <returns></returns>
        public static bool ChkAdminInfo(string adminid, string admipwd)
        {
            using (CareCenterEntities entity = new CareCenterEntities())
            {
               
                var admin = entity.tb_Admins.Where(item => item.AdminPwd == admipwd && item.Status == 1).ToList();
                if (adminid.Length == 11)
                {
                    admin = admin.Where(item => item.Mobile == adminid).ToList();
                }
                else
                {
                    admin = admin.Where(item => item.AdminID == adminid).ToList();
                }
                if (admin.Count>0 )
                {
                    return true;
                }
                return false;
            }
        }

        /// <summary>
        /// 获取所有管理员信息
        /// </summary>
        /// <returns></returns>
        public static List<tb_Admins> GetAlladminInfo()
        {
            using (CareCenterEntities entity = new CareCenterEntities())
            {
                var admin = entity.tb_Admins.ToList();
                return admin;
            }
        }
        /// <summary>
        /// 根据用户名或手机号码获取信息
        /// </summary>
        /// <param name="adminid"></param>
        /// <returns></returns>
        public static tb_Admins GetAdminInfoByAdmin(string adminPhone)
        {
            using (CareCenterEntities entity = new CareCenterEntities())
            {
                tb_Admins refAdmin =new tb_Admins() ;
                               
                if (adminPhone.Length == 11)
                {
                    refAdmin = entity.tb_Admins.FirstOrDefault(item => item.Mobile == adminPhone);
                }
                else
                {
                    refAdmin = entity.tb_Admins.FirstOrDefault(item => item.AdminID == adminPhone);
                }
               
                return refAdmin;
            }
        }


        /// <summary>
        ///  修改登录时间
        /// </summary>
        /// <param name="adminid"></param>
        /// <returns></returns>
        public static int UpdateLastDateTime(string adminid)
        {
            using (CareCenterEntities entity = new CareCenterEntities())
            {
                var admin = entity.tb_Admins.FirstOrDefault(item => item.AdminID == adminid||item.Mobile==adminid);
                if (admin != null)
                {
                    admin.LastLoginTime = DateTime.Now;
                    entity.Entry(admin).State = System.Data.Entity.EntityState.Modified;
                    entity.SaveChanges();
                    return 1;
                }
                else
                {
                    return -1;
                }
            }
        }

        /// <summary>
        /// 根据用户adminID获取菜单
        /// </summary>
        /// <param name="adminid"></param>
        /// <returns></returns>
        //public static List<eo_admins_menu> GetAdminMenu(string adminid)
        //{
        //    var strSql = new StringBuilder();
        //    strSql.Append(@"SELECT M.MenuID 
        //                          ,M.MenuName 
        //                          ,M.FMenuID 
        //                          ,M.FMenuName 
        //                          ,M.MenuUrl 
        //                          ,M.FMenuUrl                                   
        //                          ,M.Status
        //                          ,M.Memo
        //                          ,M.CreateDate
        //                     FROM eo_admins A  
        //                     LEFT JOIN eo_admins_role R ON R.RoleID=A.RoleID AND R.Status=1
        //                     LEFT JOIN eo_admins_menu M ON (SELECT CHARINDEX(M.MenuID,R.MenuID))>0 AND M.Status=1
        //                     WHERE A.AdminID='" + adminid + "' AND A.Status=1");
        //    using (eo_dbEntities entity = new eo_dbEntities())
        //    {
        //        var menu = entity.Database.SqlQuery<eo_admins_menu>(strSql.ToString()).ToList();
        //        return menu;
        //    }
        //}
        #endregion

        #region***后台用户管理***

        /// <summary>
        /// 禁用/启用管理员
        /// </summary>
        /// <returns></returns>
        public static int UpdAdminStatus(string id, int status)
        {
            using (CareCenterEntities entity = new CareCenterEntities())
            {


                var admin = entity.tb_Admins.FirstOrDefault(item => item.ID == id);
                if (admin != null)
                {
                    admin.Status = status;
                    entity.Entry(admin).State = System.Data.Entity.EntityState.Modified;
                    entity.SaveChanges();
                    return 1;
                }
                else
                {
                    return -1;
                }

            }
        }

        /// <summary>
        /// 管理员注册
        /// </summary>
        /// <param name="am"></param>
        /// <returns></returns>
        public static int Register(tb_Admins am)
        {
            using (CareCenterEntities entity = new CareCenterEntities())
            {

                int count = 0;
                var admin = entity.tb_Admins.FirstOrDefault(item => item.AdminID == am.AdminID);
                if (admin == null)
                {
                    am.ID = Utility.GetNewGUID();
                    am.Status = 1;
                    am.CreateDate = DateTime.Now;
                    am.LastLoginTime = DateTime.Now;
                    //am.Memo = null;
                    entity.Entry(am).State = System.Data.Entity.EntityState.Added;
                    entity.SaveChanges();
                    count= 1;
                }
                return count;

            }
        }

        /// <summary>
        /// 后台用户信息修改
        /// </summary>
        /// <param name="am"></param>
        /// <returns></returns>
        public static int UpdRegister(tb_Admins am)
        {
            using (CareCenterEntities entity = new CareCenterEntities())
            {

                int count = 0;
                var admin = entity.tb_Admins.FirstOrDefault(item => item.ID == am.ID);
                if (admin != null)
                {
                    admin.AdminID = am.AdminID;
                    admin.NickName = am.NickName;
                    admin.RealName = am.RealName;
                    admin.Mobile = am.Mobile;
                    admin.Memo = null;
                    entity.Entry(admin).State = System.Data.Entity.EntityState.Modified;
                    entity.SaveChanges();
                    count = 1;
                }
                return count;

            }
        }
        /// <summary>
        /// 修改登录密码
        /// </summary>
        /// <param name="phone"></param>
        /// <param name="pwd"></param>
        /// <param name="newpwd"></param>
        /// <returns></returns>
        public static int UpdAdminPwd(string phone,string pwd,string newpwd)
        {
            using (CareCenterEntities entity = new CareCenterEntities())
            {

                int count = 0;
                var admin = entity.tb_Admins.FirstOrDefault(item => item.Mobile == phone&&item.AdminPwd==pwd);
                if (admin != null)
                {
                    admin.AdminPwd = newpwd;
                   
                    entity.Entry(admin).State = System.Data.Entity.EntityState.Modified;
                    entity.SaveChanges();
                    count = 1;
                }
                return count;

            }
        }
        #endregion

        #region ***看护方需求审批***
        /// <summary>
        /// 获取需求数据
        /// </summary>
        /// <returns></returns>
        public static List<OrderModel> GetOrderList()
        {
            var strSql = new StringBuilder();
            strSql.Append(@"SELECT O.OrderID
                                ,O.UserID
                                ,O.OrderTitle
                                ,O.OrderType
                                ,O.OrderIntor
                                ,O.OrderClassify
                                ,O.Status
                                ,O.Memo
                                ,O.CreateDate
                                ,O.AdminUserID
	                            ,P.PicUrl
                                ,U.NickName
                                ,AU.NickName AS AdminNickName
                                ,AU.UserPhone AS AdminPhone
		                        ,U.UserPhone
                                ,V.VideoUrl
                            FROM tb_Order O
                            LEFT JOIN tb_Pictuer P ON P.OrderID=O.OrderID
                            LEFT JOIN tb_User U ON U.ID=O.UserID
                            LEFT JOIN tb_User AU ON AU.ID=O.AdminUserID
                            LEFT JOIN tb_Video V ON V.OrderID=O.OrderID");

            using (CareCenterEntities entity=new CareCenterEntities())
            {
                var orderdata = entity.Database.SqlQuery<OrderModel>(strSql.ToString()).ToList();
                return orderdata;
            }
        }
        /// <summary>
        /// 审批需求
        /// </summary>
        /// <returns></returns>
        public static int UpdOrderStatus(string orderid, int flag)
        {
            using (CareCenterEntities entity = new CareCenterEntities())
            {
                int count = 0;
                try
                {
                    var order = entity.tb_Order.FirstOrDefault(item => item.OrderID == orderid);
                    if (order != null)
                    {
                        order.Status = flag;                       
                        entity.Entry(order).State = System.Data.Entity.EntityState.Modified;
                        entity.SaveChanges();
                        count = 1;
                    }
                    return count;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }
        /// <summary>
        /// 给需求派发服务人员
        /// </summary>
        /// <param name="orderid"></param>
        /// <param name="adminuserid"></param>
        /// <returns></returns>
        public static int UpdOrderAdminUserID(string orderid,string adminuserid)
        {
            using (CareCenterEntities entity = new CareCenterEntities())
            {
                int count = 0;
                try
                {
                    var order = entity.tb_Order.FirstOrDefault(item => item.OrderID == orderid);
                    if (order != null)
                    {
                        order.AdminUserID = adminuserid;
                        entity.Entry(order).State = System.Data.Entity.EntityState.Modified;
                        entity.SaveChanges();
                        count = 1;
                    }
                    return count;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }
        #endregion

        #region ***服务方管理***
        /// <summary>
        /// 获取服务方
        /// </summary>
        /// <returns></returns>
        public static List<tb_User> GetCareParsonList()
        {
            

            using (CareCenterEntities entity = new CareCenterEntities())
            {
                var parson = entity.tb_User.Where(item=>item.Status==1).ToList();
                return parson;
            }
        }
        /// <summary>
        /// 服务方职位申请审核
        /// </summary>
        /// <param name="userid"></param>
        /// <param name="flag"></param>
        /// <returns></returns>
        public static int UpdRoleStatus(string userid, int flag)
        {
            using (CareCenterEntities entity = new CareCenterEntities())
            {
                int count = 0;
                try
                {
                    var order = entity.tb_User.FirstOrDefault(item => item.ID == userid);
                    if (order != null)
                    {
                        order.RoleStatus = flag;
                        entity.Entry(order).State = System.Data.Entity.EntityState.Modified;
                        entity.SaveChanges();
                        count = 1;
                    }
                    return count;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }
        /// <summary>
        /// 删除申请职位
        /// </summary>
        /// <param name="userid"></param>
        /// <returns></returns>
        public static int DelPart(string userid)
        {
            using (CareCenterEntities entity = new CareCenterEntities())
            {
                int count = 0;
                try
                {
                    var order = entity.tb_PartTime.FirstOrDefault(item => item.UserID == userid);
                    if (order != null)
                    {                       
                        entity.Entry(order).State = System.Data.Entity.EntityState.Deleted;
                        entity.SaveChanges();
                        count = 1;
                    }
                    return count;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        /// <summary>
        /// 清除职位还原普通员工
        /// </summary>
        /// <param name="userid"></param>
        /// <param name="flag"></param>
        /// <returns></returns>
        public static int UpdOldRoleStatus(string userid)
        {
            using (CareCenterEntities entity = new CareCenterEntities())
            {
                int count = 0;
                try
                {
                    var order = entity.tb_User.FirstOrDefault(item => item.ID == userid);
                    if (order != null)
                    {
                        order.RoleStatus = 1;
                        order.Role = 0;
                        entity.Entry(order).State = System.Data.Entity.EntityState.Modified;
                        entity.SaveChanges();
                        count = 1;
                    }
                    return count;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        /// <summary>
        /// 根据用户ID获取兼职申请
        /// </summary>
        /// <param name="userid"></param>
        /// <returns></returns>
        public static tb_PartTime GetPartTimeData(string userid)
        {
            using (CareCenterEntities entity = new CareCenterEntities())
            {
                try
                {

                    var teacher = entity.tb_PartTime.FirstOrDefault(item => item.UserID == userid && item.Status == 1);

                    return teacher;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }
        /// <summary>
        /// 添加前台新用户(管理员)
        /// </summary>
        /// <param name="userInfo"></param>
        /// <returns></returns>
        public static int AddUserInfoByAdmin(tb_User userInfo)
        {
            using (CareCenterEntities entity = new CareCenterEntities())
            {
                try
                {
                    int count = 0;
                    var user = entity.tb_User.FirstOrDefault(item => item.UserPhone == userInfo.UserPhone);
                    if (user == null)
                    {
                        userInfo.ID = Utility.GetNewGUID_Tab();
                        userInfo.LoginID = userInfo.UserPhone;
                        userInfo.Sex = 1;
                        userInfo.Status = 1;
                        //userInfo.Role = 0;
                        userInfo.RoleStatus = 1;
                        userInfo.ImgUrl = "/Content/img/HeadImg/login.png";
                        userInfo.LastLognTime = DateTime.Now;
                        userInfo.CreateDate = DateTime.Now;
                        entity.Entry(userInfo).State = System.Data.Entity.EntityState.Added;
                        entity.SaveChanges();
                        count = 1;
                    }
                    return count;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }
        /// <summary>
        /// 添加兼职新人(管理员)
        /// </summary>
        /// <param name="pt"></param>
        /// <returns></returns>
        public static int AddPartTimeInfoByAdmin(tb_PartTime pt)
        {
            using (CareCenterEntities entity = new CareCenterEntities())
            {
                try
                {
                    int count = 0;
                    var teacher = entity.tb_PartTime.FirstOrDefault(item => item.UserID == pt.UserID && item.Status != -1);
                    if (teacher == null)
                    {
                        pt.PartTimeID = Utility.GetNewGUID_Tab();
                        pt.Status = 1;//待确定
                        pt.CreateDate = Utility.GetSysDateTime();
                        entity.Entry(pt).State = System.Data.Entity.EntityState.Added;
                        entity.SaveChanges();
                        count = 1;
                    }
                    return count;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }
        #endregion

        #region ***获取图片，视频***
        /// <summary>
        /// 获取图片
        /// </summary>
        /// <returns></returns>
        public static List<tb_Pictuer> GetPicList()
        {
            using (CareCenterEntities entity = new CareCenterEntities())
            {
                var picdata = entity.tb_Pictuer.ToList();
                return picdata;
            }
        }
        /// <summary>
        /// 获取视频
        /// </summary>
        /// <returns></returns>
        public static List<tb_Video> GetVideoList()
        {
            using (CareCenterEntities entity = new CareCenterEntities())
            {
                var videodata = entity.tb_Video.ToList();
                return videodata;
            }
        }
        #endregion

        #region 删除图片、视频***
        /// <summary>
        /// 删除图片
        /// </summary>
        /// <param name="picid"></param>
        /// <returns></returns>
        public static int DelPicData(string picid)
        {
            using (CareCenterEntities entity = new CareCenterEntities())
            {
                int count = 0;
                try
                {
                    var picdata = entity.tb_Pictuer.FirstOrDefault(item=>item.PicID==picid);
                    if (picdata != null)
                    {
                        entity.Entry(picdata).State = System.Data.Entity.EntityState.Deleted;
                        entity.SaveChanges();
                        count = 1;
                    }
                    return count;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
               
               
            }
        }
        /// <summary>
        /// 删除视频
        /// </summary>
        /// <param name="videoid"></param>
        /// <returns></returns>
        public static int DelVideoData(string videoid)
        {
            using (CareCenterEntities entity = new CareCenterEntities())
            {
                int count = 0;
                try
                {
                    var videodata = entity.tb_Video.FirstOrDefault(item => item.VideoID == videoid);
                    if (videodata != null)
                    {
                        entity.Entry(videodata).State = System.Data.Entity.EntityState.Deleted;
                        entity.SaveChanges();
                        count = 1;
                    }
                    return count;
                }
                catch (Exception ex)
                {
                    throw ex;
                }


            }
        }
        #endregion

        #region ***修改图片 开始***
        /// <summary>
        /// 修改图片标题简介
        /// </summary>
        /// <param name="picid"></param>
        /// <param name="title">标题</param>
        /// <param name="memo">简介</param>
        /// <returns></returns>
        public static int EditPicData(string picid,string title,string memo)
        {
            using (CareCenterEntities entity = new CareCenterEntities())
            {
                int count = 0;
                try
                {
                    var picdata = entity.tb_Pictuer.FirstOrDefault(item => item.PicID == picid);
                    if (picdata != null)
                    {
                        picdata.PicTitle = title;
                        picdata.Memo = memo;
                        entity.Entry(picdata).State = System.Data.Entity.EntityState.Modified;
                        entity.SaveChanges();
                        count = 1;
                    }
                    return count;
                }
                catch (Exception ex)
                {
                    throw ex;
                }


            }
        }
        #endregion

        #region *** 家园建设 开始***
        /// <summary>
        /// 获取文本数据
        /// </summary>
        /// <returns></returns>
        public static List<tb_Context> GetContextList()
        {
            using (CareCenterEntities entity = new CareCenterEntities())
            {
                var cotdata = entity.tb_Context.ToList();
                return cotdata;
            }
        }
        /// <summary>
        /// 新增文本数据
        /// </summary>
        /// <param name="con"></param>
        /// <returns></returns>
        public static int AddContextData(tb_Context con)
        {
            using (CareCenterEntities entity = new CareCenterEntities())
            {
                int count = 0;
                try
                {
                    var context = entity.tb_Context.FirstOrDefault(item => item.Type == con.Type && item.Status == 1);
                    if (context == null)
                    {
                        con.ContextID = Utility.GetNewGUID_Tab();
                        con.Status = 1;
                        con.CreateDate = DateTime.Now;
                        entity.Entry(con).State = System.Data.Entity.EntityState.Added;
                        entity.SaveChanges();
                        count = 1;
                    }
                    return count;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }
        /// <summary>
        /// 修改文本数据
        /// </summary>
        /// <param name="con"></param>
        /// <returns></returns>
        public static int UpdContextData(tb_Context con)
        {
            using (CareCenterEntities entity = new CareCenterEntities())
            {
                int count = 0;
                try
                {
                    var context = entity.tb_Context.FirstOrDefault(item => item.ContextID == con.ContextID && item.Status == 1);
                    if (context != null)
                    {
                        context.Title = con.Title;
                        context.Explain = con.Explain;
                        context.Intro = con.Intro;
                        context.Title = con.Title;                        
                        entity.Entry(context).State = System.Data.Entity.EntityState.Modified;
                        entity.SaveChanges();
                        count = 1;
                    }
                    return count;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }
        /// <summary>
        /// 删除文本
        /// </summary>
        /// <param name="contextid"></param>
        /// <returns></returns>
        public static int DelContext(string contextid)
        {
            using (CareCenterEntities entity = new CareCenterEntities())
            {
                int count = 0;
                try
                {
                    var context = entity.tb_Context.FirstOrDefault(item => item.ContextID == contextid );
                    if (context != null)
                    {                       
                        entity.Entry(context).State = System.Data.Entity.EntityState.Deleted;
                        entity.SaveChanges();
                        count = 1;
                    }
                    return count;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        #endregion
    }
}
