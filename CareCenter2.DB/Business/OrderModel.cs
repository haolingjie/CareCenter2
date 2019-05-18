using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CareCenter2.DB
{
    /// <summary>
    /// 订单组合实体
    /// </summary>
    public class OrderModel
    {
        public string OrderID { get; set; }
        public string UserID { get; set; }
        public string OrderTitle { get; set; }
        public string OrderIntor { get; set; }
        public int OrderClassify { get; set; }
        public int Status { get; set; }
        public string Memo { get; set; }
        public System.DateTime CreateDate { get; set; }
        public string OrderType { get; set; }
        public string AdminUserID { get; set; }
        /// <summary>
        /// 昵称
        /// </summary>
        public string NickName { get; set; }
        /// <summary>
        /// 图片地址
        /// </summary>
        public string PicUrl { get; set; }
        /// <summary>
        /// 视频地址
        /// </summary>
        public string VideoUrl { get; set; }
        /// <summary>
        /// 电话号码
        /// </summary>
        public string UserPhone { get; set; }

        /// <summary>
        /// 服务方昵称
        /// </summary>
        public string AdminNickName { get; set; }
        /// <summary>
        /// 服务方电话号码
        /// </summary>
        public string AdminPhone { get; set; }
      
        public string PersonInro { get; set; }
        public string Address { get; set; }
        public string CeraRange { get; set; }
        public string DateType { get; set; }
        public string CareAddress { get; set; }
        public string CarePerson { get; set; }
    }
}
