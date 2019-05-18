using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CareCenter2.DB
{
    /// <summary>
    /// 评论数据组合实体
    /// </summary>
    public class CommentModel
    {
        public string CommentID { get; set; }
        public string FaBuUserID { get; set; }
        public string OrderID { get; set; }
        public string ComIntor { get; set; }
        public int Status { get; set; }
        public string Memo { get; set; }
        public System.DateTime CreateDate { get; set; }
        /// <summary>
        /// 昵称
        /// </summary>
        public string NickName { get; set; }
        /// <summary>
        /// 头像
        /// </summary>
        public string ImgUrl { get; set; }
    }
}
