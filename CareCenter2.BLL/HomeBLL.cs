using CareCenter2.DB;
using CareCenter2.Util.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CareCenter2.BLL
{
    public class HomeBLL
    {
        /// <summary>
        /// 登录验证
        /// </summary>
        /// <param name="AdminID"></param>
        /// <param name="AdminPwd"></param>
        /// <returns></returns>
        public static bool GetUserInfo(string AdminID, string AdminPwd)
        {
            bool flag = false;
            // DataTable dt = null;
            try
            {
                var userList = GetLoginInfo(AdminID, AdminPwd);

                if (userList != null)
                {

                    var status = userList.Status.ToString();//判断当前登录用户的状态
                    if (status == "1")//在职
                    {
                        flag = true;
                    }
                    else//离职
                    {
                        flag = false;
                    }
                }
                return flag;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 根据用户ID和密码查询
        /// </summary>
        /// <param name="phone"></param>
        /// <param name="LoginPwd"></param>
        /// <returns></returns>
        public static tb_User GetLoginInfo(string phone, string LoginPwd)
        {
            using (CareCenterEntities entity = new CareCenterEntities())
            {
                var user = entity.tb_User.FirstOrDefault(item => item.UserPhone == phone  && item.LoginPwd == LoginPwd);
                return user;
            }
        }
        /// <summary>
        /// 根据手机获取信息
        /// </summary>
        /// <param name="phone"></param>
        /// <returns></returns>
        public static tb_User GetUserInfoByPhone(string phone)
        {
            using (CareCenterEntities entity = new CareCenterEntities())
            {
                var user = entity.tb_User.FirstOrDefault(item => item.UserPhone == phone || item.UserEmail == phone);
                return user;
            }
        }

        /// <summary>
        /// 更新登录时间和次数
        /// </summary>
        /// <param name="userid">用户ID</param>
        /// <param name="lastlogintime">登录时间</param>       
        /// <returns></returns>
        public static bool UpdateLastLoginTime(string userid, DateTime lastlogintime)
        {
            using (CareCenterEntities entity = new CareCenterEntities())
            {
                var user = entity.tb_User.FirstOrDefault(item => item.ID == userid);
                if (user != null)
                {
                    user.LastLognTime = lastlogintime;//时间                 
                    entity.Entry(user).State = System.Data.Entity.EntityState.Modified;//修改登录时间
                    entity.SaveChanges();
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        /// <summary>
        /// 注册
        /// </summary>
        /// <param name="userInfo"></param>
        /// <returns></returns>
        public static int SetUserInfo(tb_User userInfo)
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
                        userInfo.Role = 0;
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
        /// 修改密码
        /// </summary>
        /// <param name="userphone"></param>
        /// <param name="userpwd"></param>
        /// <returns></returns>
        public static int UpdUserPwd(string userphone,string userpwd)
        {
            try
            {
                using (CareCenterEntities entity = new CareCenterEntities())
                {
                    int count = 0;
                    var user = entity.tb_User.FirstOrDefault(item => item.UserPhone == userphone);
                    if (user != null)
                    {
                        user.LoginPwd = userpwd;
                        entity.Entry(user).State = System.Data.Entity.EntityState.Modified;
                        entity.SaveChanges();
                        count = 1;
                    }
                    return count;
                }
                    
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
