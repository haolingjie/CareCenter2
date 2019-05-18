using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CareCenter2.Util.Common
{
    /// <summary>
    /// 对ajax请求处理结果的封装
    /// </summary>
    public class AjaxMsgParameter
    {
        /// <summary>
        /// 消息内容
        /// </summary>
        public string Msg { get; set; }

        /// <summary>
        /// 消息状态
        /// </summary>
        public AjaxStatu Statu { get; set; }

        /// <summary>
        /// 处理的数据
        /// </summary>
        public object Data { get; set; }

        /// <summary>
        /// 返回路径
        /// </summary>
        public string BackUrl { get; set; }
    }
}
