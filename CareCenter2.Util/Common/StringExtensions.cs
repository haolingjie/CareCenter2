using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace CareCenter2.Util.Common
{
    public static class StringExtensions
    {
        public static bool IsNullOrEmpty(this string str)
        {
            return string.IsNullOrEmpty(str);
        }

        /// <summary>
        /// 移除字符串中空格，并忽略大小写进行比对
        /// </summary>
        /// <param name="str">源字符串</param>
        /// <param name="target">目标比较字符串</param>
        /// <returns></returns>
        public static bool FullEquals(this string str, string target)
        {
            if (!string.IsNullOrEmpty(str) && !string.IsNullOrEmpty(target))
            {
                string nstr = str.Replace(" ", "");
                string ntarget = target.Replace(" ", "");
                return nstr.Equals(ntarget, StringComparison.OrdinalIgnoreCase);
            }
            else
                return false;
        }

        /// <summary>
        /// 强制转换成整形，失败返回0
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static int TryParseInt(this string str)
        {
            int val = 0;
            int.TryParse(str, out val);
            return val;
        }

        public static double TryParseDouble(this string str)
        {
            double val = 0;
            double.TryParse(str, out val);
            return val;
        }

        public static decimal TryParseDecimal(this string str)
        {
            decimal val = 0;
            decimal.TryParse(str, out val);
            return val;
        }

        public static List<string> ToSpiltList(this String str, string spilt)
        {
            string[] list = str.Split(spilt.ToArray(), StringSplitOptions.RemoveEmptyEntries);
            return list.ToList();
        }

        public static string TranslatePLPStatus(this string status)
        {
            string result = string.Empty;
            switch (status.ToLower())
            {
                case "submit":
                    result = "Submit";
                    break;
                case "resubmit":
                    result = "ReSubmit";
                    break;
                case "adminaskformore":
                case "crosscheckaskformore":
                case "approveraskformore":
                    result = "AskForMore";
                    break;
                case "crosschecking":
                case "crosschecked":
                case "approving":
                case "officiallaunch":
                    result = "Approve";
                    break;
                case "crosscheckrejected":
                case "approverrejected":
                    result = "Reject";
                    break;
                case "prelaunch":
                    result = "Prelaunch";
                    break;
                case "publish":
                    result = "Publish";
                    break;
                case "terminate":
                    result = "Terminate";
                    break;
                default:
                    break;
            }
            return result;
        }

        public static string TranslatePWPStatus(this string status)
        {
            string result = string.Empty;
            switch (status.ToLower())
            {
                case "submit":
                    result = "Submit";
                    break;
                case "resubmit":
                    result = "ReSubmit";
                    break;
                case "adminaskformore":
                case "manageraskformore":
                case "revieweraskformore":
                    result = "AskForMore";
                    break;
                case "checking":
                case "checked":
                case "reviewing":
                case "reviewed":
                case "crosschecking":
                case "crosschecked":
                case "approving":
                case "approved":
                    result = "Approve";
                    break;
                case "managerrejected":
                case "crosscheckrejected":
                case "approverrejected":
                    result = "Reject";
                    break;
                case "withdraw":
                    result = "Publish";
                    break;
                case "terminate":
                    result = "Terminate";
                    break;
                default:
                    break;
            }

            return result;
        }

        ///   <summary>
        ///   去除HTML标记
        ///   </summary>
        ///   <param   name="NoHTML">包括HTML的源码   </param>
        ///   <returns>已经去除后的文字</returns>
        public static string NoHTML(string Htmlstring)
        {
            //删除脚本
            Htmlstring = Regex.Replace(Htmlstring, @"<script[^>]*?>.*?</script>", "",
              RegexOptions.IgnoreCase);
            //删除HTML
            Htmlstring = Regex.Replace(Htmlstring, @"<(.[^>]*)>", "",
              RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"([\r\n])[\s]+", "",
              RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"-->", "", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"<!--.*", "", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&(quot|#34);", "\"",
              RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&(amp|#38);", "&",
              RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&(lt|#60);", "<",
              RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&(gt|#62);", ">",
              RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&(nbsp|#160);", "   ",
              RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&(iexcl|#161);", "\xa1",
              RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&(cent|#162);", "\xa2",
              RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&(pound|#163);", "\xa3",
              RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&(copy|#169);", "\xa9",
              RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&#(\d+);", "",
              RegexOptions.IgnoreCase);

            Htmlstring.Replace("<", "");
            Htmlstring.Replace(">", "");
            Htmlstring.Replace("\r\n", "");
            Htmlstring = Htmlstring.Trim();

            return Htmlstring;
        }

        public static string FilterTag(string html)
        {
            return (html.IndexOf("[") > -1) ? html.Substring(0, html.IndexOf("[")) : html;
        }

        public static string HomeSubstring(string content)
        {
            return (content.Length > 16 ? string.Format("{0}...", content.Substring(0, 16)) : content);
        }

        public static string FilterClubName(string value)
        {
            value = NoHTML(value);
            value = (value.IndexOf("(") > -1) ? value.Substring(0, value.IndexOf("(")) : value;
            value = (value.IndexOf("[") > -1) ? value.Substring(0, value.IndexOf("[")) : value;

            return value;
        }

        public static string OrderContentShow(string content, string hostName, string guestName)
        {
            switch (content)
            {
                case "胜":
                    return hostName;
                case "负":
                    return guestName;
                default:
                    return content;
            }
        }
        public static string OrderContentShowShort(string content, string hostName, string guestName)
        {
            switch (content)
            {
                case "胜":
                    return "主队";
                case "负":
                    return "客队";
                default:
                    return content;
            }
        }
        // 获取指定网址下的html页面中的图片文件路径列表
        public static string getImageUrl(string strSource)
        {
            Regex reg = new Regex(@"http://\S+\.(gif|bmp|png|jpg)");
            MatchCollection mc = reg.Matches(strSource);
            if (mc.Count > 0)
            {
                for (int i = 0; i < mc.Count; i++)
                {
                    return mc[i].Value;
                }
            }
            
            return "";
        }

    }
}
