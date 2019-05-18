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
    /// 兼职申请逻辑层
    /// </summary>
    public  class PartTimeBLL
    {
        /// <summary>
        /// 兼职申请
        /// </summary>
        /// <returns></returns>
        public static int SetPartTimeTeacher(tb_PartTime pt)
        {
            using (CareCenterEntities entity = new CareCenterEntities())
            {
                try
                {
                    int count = 0;
                    var teacher = entity.tb_PartTime.FirstOrDefault(item => item.UserID == pt.UserID && item.Status != -1);
                    if (teacher == null)
                    {
                        pt.PartTimeID= Utility.GetNewGUID_Tab();
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

        /// <summary>
        /// 兼职申请用户表角色更新
        /// </summary>
        /// <returns></returns>
        public static int UpdUserRole(string userid,int flag)
        {
            using (CareCenterEntities entity = new CareCenterEntities())
            {
                try
                {
                    int count = 0;
                    var teacher = entity.tb_User.FirstOrDefault(item => item.ID == userid && item.Status == 1);
                    if (teacher != null)
                    {
                        teacher.Role = flag;
                        teacher.RoleStatus = 0;//待确定                       
                        entity.Entry(teacher).State = System.Data.Entity.EntityState.Modified;
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
        public static List<tb_PartTime> GetPartTimeData(string userid)
        {
            using (CareCenterEntities entity = new CareCenterEntities())
            {
                try
                {
                  
                    var teacher = entity.tb_PartTime.Where(item => item.UserID == userid && item.Status != -1).ToList();
                    
                    return teacher;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

       
    }
}
