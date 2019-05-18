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
    /// 用户中心
    /// </summary>
    public class UserBLL
    {
        #region 收支记录 开始
        /// <summary>
        /// 获取收支记录
        /// </summary>
        /// <returns></returns>
        public static List<tb_MoneyRecord> GetMoneyRecordList()
        {
            using (CareCenterEntities entity = new CareCenterEntities())
            {
                try
                {                   
                    var MoneyRecordList = entity.tb_MoneyRecord.ToList();                    
                    return MoneyRecordList;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        #endregion

        #region 订单记录 开始
        /// <summary>
        /// 获取订单记录
        /// </summary>
        /// <returns></returns>
        public static List<OrderModel> GetOrderRecordList()
        {
            var sqlStr = new StringBuilder();
            sqlStr.Append(@"SELECT O.OrderID
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
                                        ,U.UserPhone
                                        ,AU.NickName AS AdminNickName
                                        ,AU.UserPhone AS AdminPhone
                                        ,V.VideoUrl
                                        ,O.PersonInro
		                                ,O.Address
		                                ,O.CarePerson
		                                ,O.CareAddress
		                                ,O.CeraRange
		                                ,O.DateType
                                      FROM tb_Order O
                                      LEFT JOIN tb_Pictuer P ON P.OrderID=O.OrderID
                                      LEFT JOIN tb_User U ON U.ID=O.UserID
                                      LEFT JOIN tb_User AU ON AU.ID=O.AdminUserID
                                      LEFT JOIN tb_Video V ON V.OrderID=O.OrderID");
            using (CareCenterEntities entity = new CareCenterEntities())
            {
                try
                {
                    var OrderRecordList = entity.Database.SqlQuery<OrderModel>(sqlStr.ToString()).ToList();
                   
                    return OrderRecordList;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }
        /// <summary>
        /// 根据订单编号获取订单
        /// </summary>
        /// <param name="orderid"></param>
        /// <returns></returns>
        public static tb_Order GetOrderDataBYOrderID(String orderid)
        {
            using (CareCenterEntities entity = new CareCenterEntities())
            {
                try
                {
                    var Orderdata = entity.tb_Order.FirstOrDefault(item => item.OrderID == orderid);

                    return Orderdata;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }
        /// <summary>
        /// 更新订单状态
        /// </summary>
        /// <param name="orderid"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        public static int UpdOrderStatus(string orderid,int status)
        {
            using (CareCenterEntities entity = new CareCenterEntities())
            {
                try
                {
                    int count = 0;
                    var order = entity.tb_Order.FirstOrDefault(item => item.OrderID == orderid);
                    if (order != null)
                    {
                        order.Status = status;
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

        #region 需求数据 开始
        /// <summary>
        /// 我的需求数据查询
        /// </summary>
        /// <param name="order"></param>
        /// <returns></returns>
        public static List<NeedModel> GetOwnNeedData()
        {
            var sqlStr = new StringBuilder();
            sqlStr.Append(@"SELECT N.NeedID
                          ,N.UserID
                          ,N.NeedTitle
                          ,N.NeedType
                          ,N.NeedIntor
                          ,N.NeedClassify
                          ,N.Status
                          ,N.Memo
                          ,N.CreateDate
	                      ,P.PicUrl
                          ,U.NickName
                      FROM tb_Need N
                      LEFT JOIN tb_Pictuer P ON P.OrderID=N.OrderID
                      LEFT JOIN tb_User U ON U.ID=N.UserID");
            using (CareCenterEntities entity = new CareCenterEntities())
            {
                try
                {
                    var NeedRecordList = entity.Database.SqlQuery<NeedModel>(sqlStr.ToString()).ToList();

                    return NeedRecordList;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

       
        #endregion

        #region 评论数据 开始
        /// <summary>
        /// 评论数据查询
        /// </summary>
        /// <returns></returns>
        public static List<CommentModel> GetCommentData()
        {
            var sqlStr = new StringBuilder();
            sqlStr.Append(@"SELECT C.CommentID
                          ,C.FaBuUserID
                          ,C.OrderID
                          ,C.ComIntor
                          ,C.Status
                          ,C.Memo
                          ,C.CreateDate
	                      ,U.NickName
	                      ,U.ImgUrl
                      FROM tb_Comment C
                      LEFT  JOIN tb_User U ON U.ID=C.FaBuUserID");
            using (CareCenterEntities entity = new CareCenterEntities())
            {
                try
                {
                    var CommentList = entity.Database.SqlQuery<CommentModel>(sqlStr.ToString()).ToList();

                    return CommentList;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }
        /// <summary>
        /// 新增评论
        /// </summary>
        /// <param name="com"></param>
        /// <returns></returns>
        public static int SetCommentData(tb_Comment com)
        {
            using (CareCenterEntities entity=new CareCenterEntities())
            {
                try
                {
                    int count = 0;
                    com.CommentID = Utility.GetNewGUID();
                    com.Status = 1;
                    com.CreateDate = Utility.GetSysDateTime();
                    entity.Entry(com).State = System.Data.Entity.EntityState.Added;
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
    }
}
