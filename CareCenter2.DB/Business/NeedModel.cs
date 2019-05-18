using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CareCenter2.DB
{
    /// <summary>
    /// 需求组合实体
    /// </summary>
    public class NeedModel
    {
        public string NeedID { get; set; }
        public string UserID { get; set; }
        public string NeedTitle { get; set; }
        public string NeedType { get; set; }
        public string NeedIntor { get; set; }
        public int NeedClassify { get; set; }
        public int Status { get; set; }
        public string Memo { get; set; }
        public System.DateTime CreateDate { get; set; }
        /// <summary>
        /// 图片地址
        /// </summary>
        public string PicUrl { get; set; }
        /// <summary>
        /// 昵称
        /// </summary>
        public string NickName { get; set; }
    }
}
