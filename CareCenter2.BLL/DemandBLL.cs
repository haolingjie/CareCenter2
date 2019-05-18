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
    /// 需求大厅逻辑层
    /// </summary>
    public class DemandBLL
    {
        #region ****需求大厅 开始****
        /// <summary>
        /// 新增需求（订单）
        /// </summary>
        /// <param name="order"></param>
        /// <returns></returns>
        public static int OwnNeedDataIns(tb_Order order)
        {
            using (CareCenterEntities entity = new CareCenterEntities())
            {
                try
                {
                    int count = 0;
                    var orderdata = entity.tb_Order.FirstOrDefault(item => item.OrderID == order.OrderID);
                    if (orderdata == null) {
                        order.Status = 0;
                        order.CreateDate = Utility.GetSysDateTime();
                        entity.Entry(order).State = System.Data.Entity.EntityState.Added;
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
        /// 新增视频
        /// </summary>
        /// <param name="video"></param>
        /// <returns></returns>
        public static int VideoIns(tb_Video video)
        {
            using (CareCenterEntities entity = new CareCenterEntities())
            {
                try
                {
                    int count = 0;
                    video.VideoID = Utility.GetNewGUID();
                    video.Status = 1;
                    video.CreateDate = Utility.GetSysDateTime();
                    entity.Entry(video).State = System.Data.Entity.EntityState.Added;
                    entity.SaveChanges();
                    count = 1;
                    return count;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }
        /// <summary>
        /// 新增图片
        /// </summary>
        /// <param name="pic"></param>
        /// <returns></returns>
        public static int PictureIns(tb_Pictuer pic)
        {
            using (CareCenterEntities entity = new CareCenterEntities())
            {
                try
                {
                    int count = 0;
                    pic.PicID = Utility.GetNewGUID();
                    pic.Status = 1;
                    pic.CreateDate = Utility.GetSysDateTime();
                    entity.Entry(pic).State = System.Data.Entity.EntityState.Added;
                    entity.SaveChanges();
                    count = 1;
                    return count;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }
        /// <summary>
        /// 编辑需求数据
        /// </summary>
        /// <param name="order"></param>
        /// <returns></returns>
        public static int EditOrderData(tb_Order order)
        {
            using (CareCenterEntities entity = new CareCenterEntities())
            {
                try
                {
                    int count = 0;
                    var orderdata = entity.tb_Order.FirstOrDefault(item => item.OrderID == order.OrderID);
                    if (orderdata != null)
                    {
                        orderdata.Memo = order.Memo;
                        orderdata.OrderClassify = order.OrderClassify;
                        orderdata.OrderIntor = order.OrderIntor;
                        orderdata.OrderTitle = order.OrderTitle;
                        orderdata.OrderType = order.OrderType;
                        
                        entity.Entry(orderdata).State = System.Data.Entity.EntityState.Modified;
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

        #region ***家园建设 开始***

        /// <summary>
        /// 获取文本数据列表
        /// </summary>
        /// <returns></returns>
        public static List<tb_Context> GetAllContext()
        {
            using (CareCenterEntities entity = new CareCenterEntities())
            {
                var dataLlist = entity.tb_Context.Where(item=>item.Status==1).ToList();
                return dataLlist;
            }
        }
        /// <summary>
        /// 获取前台上传图片
        /// </summary>
        /// <returns></returns>
        public static List<tb_Pictuer> GetPicList()
        {
            using (CareCenterEntities entity = new CareCenterEntities())
            {
                var picdata = entity.tb_Pictuer.Where(item => item.Status == 1).ToList();
                return picdata;
            }
        }
        #endregion
    }
}
