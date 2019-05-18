using CommonLib.common.math;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Newtonsoft.Json;
using System.Web.Script.Serialization;

namespace CareCenter2.Util.Common
{
    /// <summary>
    /// 工具
    /// </summary>
     public class Utility
    {
        /// <summary>
        /// 加密密钥
        /// </summary>
        private static MyDes m_MyDes;

        public static object LogHelper { get; private set; }

        /// <summary>
        /// 初始化
        /// </summary>
        static Utility()
        {
            string m_MyDesKey = "zy123456";
            string desKey = System.Configuration.ConfigurationManager.AppSettings.Get("DesKey");
            if (!string.IsNullOrEmpty(desKey))
                m_MyDesKey = desKey;
            m_MyDes = new MyDes(m_MyDesKey);

        }

        /// <summary>
        /// Des加密
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public static string DesEncode(string param)
        {
            if (string.IsNullOrEmpty(param))
                return "";
            return m_MyDes.Encrypt(param).ToLower();
        }

        /// <summary>
        /// Des解密
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public static string DesDecode(string param)
        {
            if (string.IsNullOrEmpty(param))
                return "";
            string result = m_MyDes.Decrypt(param);
            return result == null ? param : result.ToLower();
        }

        /// <summary>
        /// 进行DES加密。
        /// </summary>
        /// <param name="pToEncrypt">要加密的字符串。</param>
        /// <param name="sKey">密钥，且必须为8位。</param>
        /// <returns>以Base64格式返回的加密字符串。</returns>
        public static string DesEncrypt(string pToEncrypt, string sKey = "zy123456")
        {
            using (System.Security.Cryptography.DESCryptoServiceProvider des = new System.Security.Cryptography.DESCryptoServiceProvider())
            {
                byte[] inputByteArray = Encoding.UTF8.GetBytes(pToEncrypt);
                des.Key = ASCIIEncoding.ASCII.GetBytes(sKey);
                des.IV = ASCIIEncoding.ASCII.GetBytes(sKey);
                System.IO.MemoryStream ms = new System.IO.MemoryStream();
                using (System.Security.Cryptography.CryptoStream cs = new System.Security.Cryptography.CryptoStream(ms, des.CreateEncryptor(), System.Security.Cryptography.CryptoStreamMode.Write))
                {
                    cs.Write(inputByteArray, 0, inputByteArray.Length);
                    cs.FlushFinalBlock();
                    cs.Close();
                }
                string str = Convert.ToBase64String(ms.ToArray());
                ms.Close();
                return str;
            }
        }

        /// <summary>
        /// 进行DES解密。
        /// </summary>
        /// <param name="pToDecrypt">要解密的以Base64</param>
        /// <param name="sKey">密钥，且必须为8位。</param>
        /// <returns>已解密的字符串。</returns>
        public static string DesDecrypt(string pToDecrypt, string sKey = "zy123456")
        {
            byte[] inputByteArray = Convert.FromBase64String(pToDecrypt);
            using (System.Security.Cryptography.DESCryptoServiceProvider des = new System.Security.Cryptography.DESCryptoServiceProvider())
            {
                des.Key = ASCIIEncoding.ASCII.GetBytes(sKey);
                des.IV = ASCIIEncoding.ASCII.GetBytes(sKey);
                System.IO.MemoryStream ms = new System.IO.MemoryStream();
                using (System.Security.Cryptography.CryptoStream cs = new System.Security.Cryptography.CryptoStream(ms, des.CreateDecryptor(), System.Security.Cryptography.CryptoStreamMode.Write))
                {
                    cs.Write(inputByteArray, 0, inputByteArray.Length);
                    cs.FlushFinalBlock();
                    cs.Close();
                }
                string str = Encoding.UTF8.GetString(ms.ToArray());
                ms.Close();
                return str;
            }
        }

        /// <summary>
        /// Base64加密
        /// </summary>
        /// <param name="codeName">加密采用的编码方式</param>
        /// <param name="source">待加密的明文</param>
        /// <returns></returns>
        public static string EncodeBase64(Encoding encode, string source)
        {
            var str = string.Empty;
            byte[] bytes = encode.GetBytes(source);
            try
            {
                str = Convert.ToBase64String(bytes);
            }
            catch
            {
                str = source;
            }
            return str;
        }

        /// <summary>
        /// Base64加密，采用utf8编码方式加密
        /// </summary>
        /// <param name="source">待加密的明文</param>
        /// <returns>加密后的字符串</returns>
        public static string EncodeBase64(string source)
        {
            return EncodeBase64(Encoding.UTF8, source);
        }

        /// <summary>
        /// Base64解密
        /// </summary>
        /// <param name="encode"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        public static string DecodeBase64(Encoding encode, string result)
        {
            string decode = "";
            byte[] bytes = Convert.FromBase64String(result);
            try
            {
                decode = encode.GetString(bytes);
            }
            catch
            {
                decode = result;
            }
            return decode;
        } 



        /// <summary>
        /// 导出查看属性
        /// </summary>
        /// <param name="target">对象</param>
        /// <returns>DataTable</returns>
        public static string DumpProperties(object target)
        {
            return DumpProperties(target, 1);
        }

        /// <summary>
        /// 导出查看属性
        /// </summary>
        /// <param name="target">对象</param>
        /// <returns>DataTable</returns>
        private static string DumpProperties(object target, int level)
        {
            if (level > 3) return "";

            string space = "".PadLeft(2 * level);
            if (target == null) return space + "null";

            StringBuilder sb = new StringBuilder();

            try
            {
                Type t = target.GetType();

                PropertyInfo[] ps = t.GetProperties();
                sb.Append("[");
                sb.Append(t.Name);
                sb.Append("]:");
                sb.Append(target.ToString());
                sb.Append("\r\n");
                foreach (PropertyInfo pi in ps)
                {
                    sb.Append(space);
                    sb.Append(pi.Name);
                    sb.Append("[");
                    sb.Append(pi.PropertyType.ToString());
                    sb.Append("]:");

                    ParameterInfo[] parms = pi.GetIndexParameters();
                    if (parms != null && parms.Length > 0)
                    {
                        foreach (ParameterInfo p in parms)
                        {
                            sb.Append(p.Name);
                        }
                    }
                    else
                    {
                        object value = pi.GetValue(target, null);
                        if (value == null)
                        {
                            sb.Append("null");
                        }
                        else
                        {
                            sb.Append(value.ToString());
                            if (!value.GetType().IsPrimitive && !value.GetType().IsValueType && value.GetType().IsClass)
                                sb.Append(DumpProperties(value, level + 1));
                        }
                        sb.Append("\r\n");
                    }
                }
            }
            catch
            {
            }
            return sb.ToString();
        }

        /// <summary>
        /// NULL转为空
        /// </summary>
        /// <param name="value">对象</param>
        /// <returns>string</returns>
        public static string Null2Empty(object value)
        {
            if (value == null)
                return string.Empty;

            if (DBNull.Value.Equals(value))
                return string.Empty;

            return value.ToString();
        }

        /// <summary>
        /// NULL转为0
        /// </summary>
        /// <param name="value">对象</param>
        /// <returns>int</returns>
        public static int Null2Zero(object value)
        {
            if (value == null)
                return 0;

            if (DBNull.Value.Equals(value))
                return 0;

            return Convert.ToInt32(value);
        }

        /// <summary>
        /// NULL转为0
        /// </summary>
        /// <param name="value">对象</param>
        /// <returns>decimal</returns>
        public static decimal Null2ZeroDecimal(object value)
        {
            if (value == null)
                return 0;

            if (DBNull.Value.Equals(value))
                return 0;

            return Convert.ToDecimal(value);
        }

        /// <summary>
        /// GUID生成
        /// </summary>
        /// <returns>GUID</returns>
        public static string GetGUID()
        {
            return System.Guid.NewGuid().ToString();
        }

        /// <summary>
        /// 自定义GUID,时间类，主要用于订单类型的ID
        /// </summary>
        /// <returns></returns>
        public static string GetNewGUID()
        {
            string guid = System.Guid.NewGuid().ToString();
            return DateTime.Now.ToString("yyyyMMddHHmmss") + guid.Replace("-", "").Substring(0, 6);
        }

        /// <summary>
        /// 自定义GUID,去除默认GUID中的“-”,主要用于表中ID值
        /// </summary>
        /// <returns></returns>
        public static string GetNewGUID_Tab()
        {
            string guid = System.Guid.NewGuid().ToString();
            return guid.Replace("-", "");
        }

        /// <summary>
        /// 自定义GUID,获取昵称
        /// </summary>
        /// <returns></returns>
        public static string GetNewGUID_NickName()
        {
            string guid = System.Guid.NewGuid().ToString();
            return "eqzx_" + DateTime.Now.Second + guid.Replace("-", "").Substring(0, 2);
        }
        /// <summary>
        /// 自定义GUID,获取后台登录账号
        /// </summary>
        /// <returns></returns>
        public static string GetNewGUID_AdminID()
        {
            string guid = System.Guid.NewGuid().ToString();
            return "zhzx_" + DateTime.Now.Second + guid.Replace("-", "").Substring(0, 2);
        }

        /// <summary>
        /// 通过方法名称执行方法
        /// </summary>
        /// <param name="control">控件</param>
        public static object ExecuteMethod(string className, string method, string assemblyName = null)
        {
            try
            {
                Type tp = GetTypeByName(className, assemblyName);
                if (tp == null)
                    return null;
                return ExecuteMethod(tp, method, new object[] { });
            }
            catch (Exception)
            {
                return null;
            }
        }

        /// <summary>
        /// 通过方法名称执行方法
        /// </summary>
        /// <param name="control">控件</param>
        public static object ExecuteMethod(Type tp, string method)
        {
            return ExecuteMethod(tp, method, new object[] { });
        }

        /// <summary>
        /// 通过方法名称执行方法
        /// </summary>
        /// <param name="control">控件</param>
        public static object ExecuteMethod(Type tp, string method, object[] param)
        {
            if (tp == null)
                return null;
            if (string.IsNullOrEmpty(method))
                return null;

            try
            {
                return tp.InvokeMember(method, BindingFlags.Public | BindingFlags.InvokeMethod | BindingFlags.Static, null, null, param);
            }
            catch (Exception ex)
            {
               /* LogHelper.Error(ex)*/;
                return null;
            }
        }

        /// <summary>
        /// 通过方法名称执行方法
        /// </summary>
        /// <param name="control">控件</param>
        public static object ExecuteMethod(object control, string method, object[] param)
        {
            try
            {
                if (control == null)
                    return null;
                if (string.IsNullOrEmpty(method))
                    return null;

                Type ctlType = control.GetType();

                MethodInfo[] methods = ctlType.GetMethods();//获取所有的公有方法
                if (methods == null)
                    return null;

                foreach (MethodInfo info in methods)
                {
                    if (info.Name.Equals(method))
                    {
                        return info.Invoke(control, param);
                    }
                }

                return null;
            }
            catch (Exception)
            {
                return null;
            }
        }


        /// <summary>
        /// 通过名称获取类型
        /// </summary>
        /// <param name="className">类名</param>
        /// <param name="assemblyName"></param>
        public static Type GetTypeByName(string className, string assemblyName = null)
        {
            if (string.IsNullOrEmpty(className))
                return null;

            try
            {
                if (string.IsNullOrEmpty(assemblyName))
                {
                    return Type.GetType(className);
                }
                else
                {
                    System.Reflection.Assembly assembly = System.Reflection.Assembly.Load(assemblyName);
                    return assembly.GetType(className);
                }
            }
            catch (Exception)
            {
                return null;
            }
        }

        /// <summary>
        /// 通过名称获取实例
        /// </summary>
        /// <param name="className">类名</param>
        /// <param name="assemblyName"></param>
        public static object GetInstanceByName(string className, string assemblyName = null)
        {
            try
            {
                Type tp = GetTypeByName(className, assemblyName);
                if (tp == null)
                    return null;
                return Activator.CreateInstance(tp);
            }
            catch (Exception)
            {
                return null;
            }
        }


        #region ***将DataTable转换为T型***
        /// <summary>
        ///     从 reader 对象中逐行读取记录并将记录转化为 T 类型的集合
        /// </summary>
        /// <typeparam name="T">目标类型参数</typeparam>
        /// <param name="reader">实现 IDataReader 接口的对象。</param>
        /// <returns>指定类型的对象集合。</returns>
        public static List<T> ConvertToObject<T>(IDataReader reader)
            where T : class
        {
            List<T> list = new List<T>();
            T obj = default(T);
            Type t = typeof(T);
            Assembly ass = t.Assembly;

            Dictionary<string, PropertyInfo> propertys = GetFields<T>(reader);
            PropertyInfo p = null;
            if (reader != null)
            {
                while (reader.Read())
                {
                    obj = ass.CreateInstance(t.FullName) as T;
                    foreach (string key in propertys.Keys)
                    {
                        p = propertys[key];
                        p.SetValue(obj, ChangeType(reader[key], p.PropertyType));
                    }
                    list.Add(obj);
                }
            }

            return list;
        }

        /// <summary>
        ///     从 DataTale 对象中逐行读取记录并将记录转化为 T 类型的集合
        /// </summary>
        /// <typeparam name="T">目标类型参数</typeparam>
        /// <param name="reader">DataTale 对象。</param>
        /// <returns>指定类型的对象集合。</returns>
        public static List<T> ConvertToObject<T>(DataTable table)
            where T : class
        {
            return table == null
                ? new List<T>()
                : ConvertToObject<T>(table.CreateDataReader() as IDataReader);
        }

        /// <summary>
        ///     将数据转化为 type 类型
        /// </summary>
        /// <param name="value">要转化的值</param>
        /// <param name="type">目标类型</param>
        /// <returns>转化为目标类型的 Object 对象</returns>
        private static object ChangeType(object value, Type type)
        {
            if (type.FullName == typeof(string).FullName)
            {
                return Convert.ChangeType(Convert.IsDBNull(value) ? null : value, type);
            }
            if (type.IsGenericType && type.GetGenericTypeDefinition().Equals(typeof(Nullable<>)))
            {
                NullableConverter convertor = new NullableConverter(type);
                return Convert.IsDBNull(value) ? null : convertor.ConvertFrom(value);
            }
            return value;
        }

        /// <summary>
        ///     获取reader存在并且在 T 类中包含同名可写属性的集合
        /// </summary>
        /// <param name="reader">
        ///     可写域的集合
        /// </param>
        /// <returns>
        ///     以属性名为键，PropertyInfo 为值得字典对象
        /// </returns>
        private static Dictionary<string, PropertyInfo> GetFields<T>(IDataReader reader)
        {
            Dictionary<string, PropertyInfo> result = new Dictionary<string, PropertyInfo>();
            int columnCount = reader.FieldCount;
            Type t = typeof(T);

            PropertyInfo[] properties = t.GetProperties();
            if (properties != null)
            {
                List<string> readerFields = new List<string>();
                for (int i = 0; i < columnCount; i++)
                {
                    readerFields.Add(reader.GetName(i));
                }
                IEnumerable<PropertyInfo> resList =
                    (from PropertyInfo prop in properties
                     where prop.CanWrite && readerFields.Contains(prop.Name)
                     select prop);

                foreach (PropertyInfo p in resList)
                {
                    result.Add(p.Name, p);
                }
            }
            return result;
        }
        #endregion

        //获取真实外面IP，利用爬虫
        public static string GetIP()
        {
            string tempip = "";
            try
            {
                WebRequest wr = WebRequest.Create("http://2017.ip138.com/ic.asp");
                //WebRequest wr = WebRequest.Create(ConfigHelper.GetAppSettingString("EmailPassword"));
                Stream s = wr.GetResponse().GetResponseStream();
                StreamReader sr = new StreamReader(s, Encoding.Default);
                string all = sr.ReadToEnd(); //读取网站的数据

                int start = all.IndexOf("您的IP是：[") + 7;
                int end = all.IndexOf("]", start);
                tempip = all.Substring(start, end - start);
                sr.Close();
                s.Close();
            }
            catch
            {

            }

            if (string.IsNullOrEmpty(tempip) || tempip == "112.114.91.70")
            {
                HttpRequest request = HttpContext.Current.Request;
                string result = request.ServerVariables["HTTP_X_FORWARDED_FOR"];
                if (string.IsNullOrEmpty(result))
                {
                    result = request.ServerVariables["REMOTE_ADDR"];
                }
                if (string.IsNullOrEmpty(result))
                {
                    result = request.UserHostAddress;
                }
                if (string.IsNullOrEmpty(result))
                {
                    result = "0.0.0.0";
                }

                tempip = result;
            }


            return tempip;
        }

        #region ***获取时间相关类 开始***
        /// <summary>
        /// 获取过去时间
        /// </summary>
        /// <param name="date">时间</param>
        /// <returns></returns>
        public static string GetDuration(DateTime? date)
        {
            if (date == null)
                return "";

            TimeSpan ts = DateTime.Now - (DateTime)date;

            string duration = "";
            if (ts.TotalDays > 3)
            {
                duration = string.Format("{0:d}天", (int)(ts.TotalDays));
            }
            else if (ts.TotalDays > 1)
            {
                if (ts.Hours > 1)
                    duration = string.Format("{0:d}天{1:d}时", (int)(ts.TotalDays), (int)(ts.Hours));
                else
                    duration = string.Format("{0:d}天", (int)(ts.TotalDays));
            }
            else if (ts.TotalHours > 1)
            {
                if (ts.Minutes > 1)
                    duration = string.Format("{0:d}时{1:d}分", (int)(ts.TotalHours), (int)(ts.Minutes));
                else
                    duration = string.Format("{0:d}时", (int)(ts.TotalHours));
            }
            else if (ts.TotalMinutes > 1)
            {
                duration = string.Format("{0:d}分", (int)(ts.TotalMinutes));
            }
            else
            {
                duration = string.Format("{0:d}秒", (int)(ts.TotalSeconds));
            }

            return duration;
        }

        /// <summary>
        /// 获取系统时间
        /// </summary>
        /// <returns></returns>
        public static DateTime GetSysDateTime()
        {
            return DateTime.Now;
        }

        /// <summary>
        /// 获取当前时间戳
        /// </summary>
        /// <param name="bflag">为真时获取10位时间戳,为假时获取13位时间戳.</param>
        /// <returns></returns>
        public static long GetTimeStamp(bool bflag = true)
        {
            TimeSpan ts = DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, 0);
            long ret;
            if (bflag)
                ret = Convert.ToInt64(ts.TotalSeconds);
            else
                ret = Convert.ToInt64(ts.TotalMilliseconds);
            return ret;
        }

        /// <summary>
        /// 计算本周起始日期（礼拜一的日期）
        /// </summary>
        /// <param name="someDate">该周中任意一天</param>
        /// <returns>返回礼拜一日期，后面的具体时、分、秒和传入值相等</returns>
        public static DateTime CalculateFirstDateOfWeek(DateTime dt)
        {
            int i = dt.DayOfWeek - DayOfWeek.Monday;
            if (i == -1) i = 6;// i值 > = 0 ，因为枚举原因，Sunday排在最前，此时Sunday-Monday=-1，必须+7=6。
            TimeSpan ts = new TimeSpan(i, 0, 0, 0);
            return dt.Subtract(ts);
        }
        /**/
        /// <summary>
        /// 计算本周结束日期（礼拜日的日期）
        /// </summary>
        /// <param name="someDate">该周中任意一天</param>
        /// <returns>返回礼拜日日期，后面的具体时、分、秒和传入值相等</returns>
        public static DateTime CalculateLastDateOfWeek(DateTime dt)
        {
            int i = dt.DayOfWeek - DayOfWeek.Sunday;
            if (i != 0) i = 7 - i;// 因为枚举原因，Sunday排在最前，相减间隔要被7减。
            TimeSpan ts = new TimeSpan(i, 0, 0, 0);
            return dt.Add(ts);
        }

        /// <summary>
        /// 计算本月的第一天日期
        /// </summary>
        /// <param name="dt">该月中任意一天</param>
        /// <returns>返回月份第一天</returns>
        public static DateTime CalculateFirstDayofMonth(DateTime dt)
        {
            if (dt == null)
            {
                dt = DateTime.Now;
            }

            return new DateTime(dt.Year, dt.Month, 1);
        }

        /// <summary>  
        /// 计算本月的最后一天日期  
        /// </summary>  
        /// <param name="dt">该月中任意一天</param>  
        /// <returns>返回月份最后一天</returns>  
        public static DateTime CalculateLastDayofMonth(DateTime dt)
        {
            int day = DateTime.DaysInMonth(dt.Year, dt.Month);

            return new DateTime(dt.Year, dt.Month, day);
        }

        /// <summary>
        /// 计算本季度的第一天日期
        /// </summary>
        /// <param name="dt">该季度中任意一天</param>
        /// <returns>返回当前季度的第一天</returns>
        public static DateTime CalculateFirstDayofSeries(DateTime dt)
        {
            DateTime dtime = dt.AddMonths(0 - (dt.Month - 1) % 3).AddDays(1 - dt.Day);  //本季度初
            return dtime;
        }

        public static DateTime CalculateLastDayofSeries(DateTime dt)
        {
            DateTime dtime = CalculateFirstDayofSeries(dt).AddMonths(3).AddDays(-1);  //本季度末  
            return dtime;
        }

        /// <summary>
        /// 根据系统时间，判断当前是周几
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static int GetDayOfWeek(DateTime dt)
        {
            int week = dt.DayOfWeek - DayOfWeek.Monday;
            switch (week)
            {
                case 0: week = 1; break;
                case 1: week = 2; break;
                case 2: week = 3; break;
                case 3: week = 4; break;
                case 4: week = 5; break;
                case 5: week = 6; break;
                case -1: week = 7; break;
            }
            return week;
        }

        /// <summary>
        /// datetime转换为unixtime
        /// </summary>
        /// <param name="time"></param>
        /// <returns></returns>
        public static int ConvertDateTimeInt(System.DateTime time)
        {
            System.DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new System.DateTime(1970, 1, 1));
            return (int)(time - startTime).TotalSeconds;
        }

        #endregion ***获取时间相关类 结束***

        #region ***发送手机验证码***

        /// <summary>
        /// 发送手机验证码
        /// </summary>
        /// <param name="phone"></param>
        /// <param name="code"></param>
        /// <returns></returns>
        public static bool SendPhone(string phone, string code)
        {
            //2.发送短信
            string url2 = "http://v.juhe.cn/sms/send";

            var parameters2 = new Dictionary<string, string>();

            parameters2.Add("mobile", phone); //接收短信的手机号码
            parameters2.Add("tpl_id", ConfigHelper.GetAppSettingString("ModelID")); //短信模板ID，请参考个人中心短信模板设置
            parameters2.Add("tpl_value", "#code#=" + code); //变量名和变量值对。如果你的变量名或者变量值中带有#&amp;=中的任意一个特殊符号，请先分别进行urlencode编码后再传递，&lt;a href=&quot;http://www.juhe.cn/news/index/id/50&quot; target=&quot;_blank&quot;&gt;详细说明&gt;&lt;/a&gt;
            parameters2.Add("key", ConfigHelper.GetAppSettingString("ModelKey"));//你申请的key
            //parameters2.Add("dtype", ""); //返回数据的格式,xml或json，默认json

            string result2 = sendPost(url2, parameters2, "get");
            //var newObj2= JsonConvert.DeserializeObject(result2);
            // //var newObj2 = JsonHelper.ToString<object>(result2);
            // //JsonObject newObj2 = new JsonObject(result2);
            // String errorCode2 = newObj2.ToString();//["error_code"].Value;
            // string aa=errorCode2.["errorCode2"].ToString();
            // if (errorCode2 == "0")
            // {
            //     //Debug.WriteLine("成功");
            //     //Debug.WriteLine(newObj2);
            // }
            // else
            // {
            //     //Debug.WriteLine("失败");
            //     //Debug.WriteLine(newObj2["error_code"].Value + ":" + newObj2["reason"].Value);
            // }
            return true;
        }

        /// <summary>
        /// Http (GET/POST)
        /// </summary>
        /// <param name="url">请求URL</param>
        /// <param name="parameters">请求参数</param>
        /// <param name="method">请求方法</param>
        /// <returns>响应内容</returns>
        static string sendPost(string url, IDictionary<string, string> parameters, string method)
        {
            if (method.ToLower() == "post")
            {
                HttpWebRequest req = null;
                HttpWebResponse rsp = null;
                System.IO.Stream reqStream = null;
                try
                {
                    req = (HttpWebRequest)WebRequest.Create(url);
                    req.Method = method;
                    req.KeepAlive = false;
                    req.ProtocolVersion = HttpVersion.Version10;
                    req.Timeout = 5000;
                    req.ContentType = "application/x-www-form-urlencoded;charset=utf-8";
                    byte[] postData = Encoding.UTF8.GetBytes(BuildQuery(parameters, "utf8"));
                    reqStream = req.GetRequestStream();
                    reqStream.Write(postData, 0, postData.Length);
                    rsp = (HttpWebResponse)req.GetResponse();
                    Encoding encoding = Encoding.GetEncoding(rsp.CharacterSet);
                    return GetResponseAsString(rsp, encoding);
                }
                catch (Exception ex)
                {
                    return ex.Message;
                }
                finally
                {
                    if (reqStream != null) reqStream.Close();
                    if (rsp != null) rsp.Close();
                }
            }
            else
            {
                //创建请求
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url + "?" + BuildQuery(parameters, "utf8"));

                //GET请求
                request.Method = "GET";
                request.ReadWriteTimeout = 5000;
                request.ContentType = "text/html;charset=UTF-8";
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                Stream myResponseStream = response.GetResponseStream();
                StreamReader myStreamReader = new StreamReader(myResponseStream, Encoding.GetEncoding("utf-8"));

                //返回内容
                string retString = myStreamReader.ReadToEnd();
                return retString;
            }
        }

        /// <summary>
        /// 把响应流转换为文本。
        /// </summary>
        /// <param name="rsp">响应流对象</param>
        /// <param name="encoding">编码方式</param>
        /// <returns>响应文本</returns>
        static string GetResponseAsString(HttpWebResponse rsp, Encoding encoding)
        {
            System.IO.Stream stream = null;
            StreamReader reader = null;
            try
            {
                // 以字符流的方式读取HTTP响应
                stream = rsp.GetResponseStream();
                reader = new StreamReader(stream, encoding);
                return reader.ReadToEnd();
            }
            finally
            {
                // 释放资源
                if (reader != null) reader.Close();
                if (stream != null) stream.Close();
                if (rsp != null) rsp.Close();
            }
        }

        /// <summary>
        /// 组装普通文本请求参数。
        /// </summary>
        /// <param name="parameters">Key-Value形式请求参数字典</param>
        /// <returns>URL编码后的请求数据</returns>
        static string BuildQuery(IDictionary<string, string> parameters, string encode)
        {
            StringBuilder postData = new StringBuilder();
            bool hasParam = false;
            IEnumerator<KeyValuePair<string, string>> dem = parameters.GetEnumerator();
            while (dem.MoveNext())
            {
                string name = dem.Current.Key;
                string value = dem.Current.Value;
                // 忽略参数名或参数值为空的参数
                if (!string.IsNullOrEmpty(name))//&& !string.IsNullOrEmpty(value)
                {
                    if (hasParam)
                    {
                        postData.Append("&");
                    }
                    postData.Append(name);
                    postData.Append("=");
                    if (encode == "gb2312")
                    {
                        postData.Append(HttpUtility.UrlEncode(value, Encoding.GetEncoding("gb2312")));
                    }
                    else if (encode == "utf8")
                    {
                        postData.Append(HttpUtility.UrlEncode(value, Encoding.UTF8));
                    }
                    else
                    {
                        postData.Append(value);
                    }
                    hasParam = true;
                }
            }
            return postData.ToString();
        }
        #endregion ***发送手机验证码***

        /// <summary>
        /// 根据传入的内容，生成左位补0
        /// </summary>
        /// <param name="firstNum">首位数字</param>
        /// <param name="account">传入值</param>
        /// <param name="totalWidth">补0的总长度</param>
        /// <param name="paddingChar">补位的数值</param>
        public static string PadLeftStr(int firstNum, int account, int totalWidth, char paddingChar)
        {
            var newAccount = string.Empty;
            newAccount = firstNum.ToString() + account.ToString().PadLeft(totalWidth, paddingChar);
            return newAccount;
        }

        /// <summary>
        /// 是否是图片文件名
        /// </summary>
        /// <returns> </returns>
        public static bool IsImgFileName(string fileName)
        {
            if (fileName.IndexOf(".") == -1)
                return false;

            string tempFileName = fileName.Trim().ToLower();
            string extension = tempFileName.Substring(tempFileName.LastIndexOf("."));
            return extension == ".png" || extension == ".bmp" || extension == ".jpg" || extension == ".jpeg" || extension == ".gif";
        }

        /// <summary>
        /// 获得文件物理路径
        /// </summary>
        /// <returns></returns>
        public static string GetMapPath(string path)
        {
            if (HttpContext.Current != null)
            {
                return HttpContext.Current.Server.MapPath(path);
            }
            else
            {
                return System.Web.Hosting.HostingEnvironment.MapPath(path);
            }
        }

        /// <summary>
        /// 图片上传
        /// </summary>
        /// <param name="image"></param>
        /// <returns></returns>
        public static string ImgUpload(HttpPostedFileBase image, string folder)
        {
            string fileName = image.FileName;
            string extension = Path.GetExtension(fileName);

            int fileSize = image.ContentLength;

            string dirPath = folder;//"/upload/GoodsImgs/"
            string newFileName = string.Format("g_{0}{1}", DateTime.Now.ToString("yyMMddHHmmssfffffff"), extension);//生成文件名
            string newPath = dirPath + newFileName;
            if (!Directory.Exists(dirPath))
                Directory.CreateDirectory(dirPath);
            image.SaveAs(dirPath + newFileName);

            return newFileName;
        }

        /// <summary>
        /// 图片下载
        /// </summary>
        /// <param name="userid">用户编号，文件存放，按照用户编号建立文件夹</param>
        /// <param name="HttpUrl">文件的网络路径</param>
        /// <returns></returns>
        //public static string DownLoad(string userid, string HttpUrl)
        //{
        //    System.Net.WebClient m_hvtWebClient = new System.Net.WebClient();

        //    //生成随机的图片文件名
        //    string localfileName = string.Format("{0}{1}", DateTime.Now.ToString("yyMMddHHmmssfffffff"), ".png");//生成文件名

        //    //文件存放位置
        //    string path = ConfigHelper.GetAppSettingString("PicPath");
        //    path = path + "/" + userid + "/";
        //    if (!Directory.Exists(path))
        //        Directory.CreateDirectory(path);

        //    string m_keleyiPicture = System.Web.HttpContext.Current.Server.MapPath("/Picture/" + userid + "/" + localfileName);

        //    //根据网址下载文件
        //    m_hvtWebClient.DownloadFile(path, m_keleyiPicture);

        //    return path + localfileName;
        //}

        /// <summary>
        /// 
        /// </summary>
        /// <param name="tempUrl">文件路径.可为网络资源，也可以是文件物理路径</param>
        /// <param name="path">图片模板存放位置</param>
        /// <returns></returns>
        public static void DownLoadStream(string tempUrl, string path)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(tempUrl);
            HttpWebResponse response = request.GetResponse() as HttpWebResponse;
            Stream responseStream = response.GetResponseStream();
            Stream stream = new FileStream(@"" + path + "", FileMode.Create);
            byte[] bArr = new byte[8096];
            int size = responseStream.Read(bArr, 0, bArr.Length);
            //LogHelper.Info("初始SIZE:" + size);
            while (size > 0)
            {
                stream.Write(bArr, 0, size);
                size = responseStream.Read(bArr, 0, bArr.Length);
            }
            stream.Close();
            responseStream.Close();
            //LogHelper.Info("下载成功SIZE:" + size);
        }

        /// <summary>
        /// 得到当前完整主机头
        /// </summary>
        /// <returns></returns>
        public static string GetCurrentFullHost()
        {
            HttpRequest request = System.Web.HttpContext.Current.Request;
            if (!request.Url.IsDefaultPort)
                return string.Format("{0}:{1}", request.Url.Host, request.Url.Port.ToString());

            return request.Url.Host;
        }

        #region 工具函数

        /// <summary>
        /// 获取DataTable的值
        /// </summary>
        /// <param name="dt">DataTable</param>
        /// <param name="row">行（从0开始）</param>
        /// <param name="column">列名</param>
        /// <returns>值</returns>
        public static object GetValue(DataTable dt, int row, string column)
        {
            if (dt == null)
                return null;
            if (dt.Rows.Count == 0)
                return null;
            if (string.IsNullOrEmpty(column))
                return null;
            if (!dt.Columns.Contains(column))
                return null;

            return dt.Rows[row][column];
        }

        /// <summary>
        /// 获取DataTable的值（文字）
        /// </summary>
        /// <param name="dt">DataTable</param>
        /// <param name="row">行（从0开始）</param>
        /// <param name="column">列名</param>
        /// <returns>值</returns>
        public static string GetString(DataTable dt, int row, string column)
        {
            object value = GetValue(dt, row, column);

            return Utility.Null2Empty(value);
        }

        /// <summary>
        /// 获取DataTable的值（INT）
        /// </summary>
        /// <param name="dt">DataTable</param>
        /// <param name="row">行（从0开始）</param>
        /// <param name="column">列名</param>
        /// <returns>值</returns>
        public static int GetInt(DataTable dt, int row, string column)
        {
            object value = GetValue(dt, row, column);

            return Utility.Null2Zero(value);
        }

        /// <summary>
        /// 获取DataTable的值（INT）
        /// </summary>
        /// <param name="dt">DataTable</param>
        /// <param name="row">行（从0开始）</param>
        /// <param name="column">列名</param>
        /// <returns>值</returns>
        public static decimal GetDecimal(DataTable dt, int row, string column)
        {
            object value = GetValue(dt, row, column);

            return Utility.Null2ZeroDecimal(value);
        }

        /// <summary>
        /// 获取DataTable的值（DateTime）
        /// </summary>
        /// <param name="dt">DataTable</param>
        /// <param name="row">行（从0开始）</param>
        /// <param name="column">列名</param>
        /// <returns>值</returns>
        public static DateTime GetDateTime(DataTable dt, int row, string column)
        {
            object value = GetValue(dt, row, column);

            if (value == null || DBNull.Value.Equals(value))
                return default(DateTime);

            return Convert.ToDateTime(value);
        }

        /// <summary>
        /// 获取DataTable的值（DateTime）
        /// </summary>
        /// <param name="dt">DataTable</param>
        /// <param name="row">行（从0开始）</param>
        /// <param name="column">列名</param>
        /// <returns>值</returns>
        public static DateTime? GetDateTimeNull(DataTable dt, int row, string column)
        {
            object value = GetValue(dt, row, column);

            if (value == null || DBNull.Value.Equals(value))
                return null;

            return Convert.ToDateTime(value);
        }

        /// <summary>
        /// 获取DataTable的值（DateTime）
        /// </summary>
        /// <param name="dt">DataTable</param>
        /// <param name="row">行（从0开始）</param>
        /// <param name="column">列名</param>
        /// <returns>值</returns>
        public static byte[] GetByteArray(DataTable dt, int row, string column)
        {
            object value = GetValue(dt, row, column);

            if (value == null)
                return null;
            else
                return (byte[])value;
        }

        #endregion

        #region  反射List To DataTable

        /// <summary>
        /// 将List集合类转换成DataTable 
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        public static DataTable ListToDataTable(IList list)
        {
            DataTable result = new DataTable();
            if (list.Count > 0)
            {
                PropertyInfo[] propertys = list[0].GetType().GetProperties();
                foreach (PropertyInfo pi in propertys)
                {
                    //获取类型
                    Type colType = pi.PropertyType;
                    //当类型为Nullable<>时
                    if ((colType.IsGenericType) && (colType.GetGenericTypeDefinition() == typeof(Nullable<>)))
                    {
                        colType = colType.GetGenericArguments()[0];
                    }
                    result.Columns.Add(pi.Name, colType);
                }
                for (int i = 0; i < list.Count; i++)
                {
                    ArrayList tempList = new ArrayList();
                    foreach (PropertyInfo pi in propertys)
                    {
                        object obj = pi.GetValue(list[i], null);
                        tempList.Add(obj);
                    }
                    object[] array = tempList.ToArray();
                    result.LoadDataRow(array, true);
                }
            }
            return result;
        }
        #endregion

        #region DataTable转化为Json String

        /// <summary>
        /// DataTable转化为Json String
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static string DataTableToJsonString(DataTable dt)
        {
            if (dt.Rows.Count == 0)
                return "{}";

            JavaScriptSerializer javaScriptSerializer = new JavaScriptSerializer();
            //取得最大数值         
            javaScriptSerializer.MaxJsonLength = Int32.MaxValue;
            ArrayList arrayList = new ArrayList();
            foreach (DataRow dataRow in dt.Rows)
            {
                Dictionary<string, object> dictionary = new Dictionary<string, object>();  //实例化一个参数集合            
                foreach (DataColumn dataColumn in dt.Columns)
                {
                    dictionary.Add(dataColumn.ColumnName, dataRow[dataColumn.ColumnName].ToString());
                }
                arrayList.Add(dictionary); //ArrayList集合中添加键值         
            }
            return javaScriptSerializer.Serialize(arrayList);  //返回一个json字符串    
        }


        #endregion

        #region  Json转化为Datatable

        /// <summary>
        /// Json转化为Datatable
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        private static DataTable JsonStringToDataTable(string json)
        {
            if (string.IsNullOrWhiteSpace(json))
                return null;

            DataTable dataTable = new DataTable();  //实例化        
            try
            {
                JavaScriptSerializer javaScriptSerializer = new JavaScriptSerializer();
                javaScriptSerializer.MaxJsonLength = Int32.MaxValue; //取得最大数值            
                ArrayList arrayList = javaScriptSerializer.Deserialize<ArrayList>(json);
                if (arrayList.Count > 0)
                {
                    foreach (Dictionary<string, object> dictionary in arrayList)
                    {
                        if (dictionary.Keys.Count<string>() == 0)
                            return dataTable;

                        if (dataTable.Columns.Count == 0)
                        {
                            foreach (string current in dictionary.Keys)
                            {
                                //当arrayList中的值为null时，类型设置为object类型。潜在问题：第一行为null，后面有值时，添加出错/不正确
                                dataTable.Columns.Add(current, dictionary[current] == null ? typeof(object) : dictionary[current].GetType());
                            }
                        }
                        DataRow dataRow = dataTable.NewRow();
                        foreach (string current in dictionary.Keys)
                        {
                            dataRow[current] = (dictionary[current] == null ? DBNull.Value : dictionary[current]);
                        }
                        dataTable.Rows.Add(dataRow); //循环添加行到DataTable中               
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return dataTable;
        }


        #endregion

        #region 数字转换成中文
        /// <summary>
        /// 数字转中文
        /// </summary>
        /// <param name="number">eg: 22</param>
        /// <returns></returns>
        public static string NumberToChinese(int number)
        {
            string res = string.Empty;
            string str = number.ToString();
            string schar = str.Substring(0, 1);
            switch (schar)
            {
                case "1":
                    res = "一";
                    break;
                case "2":
                    res = "二";
                    break;
                case "3":
                    res = "三";
                    break;
                case "4":
                    res = "四";
                    break;
                case "5":
                    res = "五";
                    break;
                case "6":
                    res = "六";
                    break;
                case "7":
                    res = "七";
                    break;
                case "8":
                    res = "八";
                    break;
                case "9":
                    res = "九";
                    break;
                default:
                    res = "零";
                    break;
            }
            if (str.Length > 1)
            {
                switch (str.Length)
                {
                    case 2:
                    case 6:
                        res += "十";
                        break;
                    case 3:
                    case 7:
                        res += "百";
                        break;
                    case 4:
                        res += "千";
                        break;
                    case 5:
                        res += "万";
                        break;
                    default:
                        res += "";
                        break;
                }
                res += NumberToChinese(int.Parse(str.Substring(1, str.Length - 1)));
            }
            return res;
        }
        #endregion

        #region 用正则表达式判断字符是不是汉字
        /// <summary>
        /// 用 正则表达式 判断字符是不是汉字
        /// </summary>
        /// <param name="text">待判断字符或字符串</param>
        /// <returns>真：是汉字；假：不是</returns>
        public static bool CheckStringChineseReg(string text)
        {
            return System.Text.RegularExpressions.Regex.IsMatch(text, @"[\u4e00-\u9fbb]+$");
        }
        #endregion

        #region ***json反序列化 开始***

        /// <summary>
        /// 将json数据反序列化为Dictionary
        /// </summary>
        /// <param name="jsonData">json数据</param>
        /// <returns></returns>
        public static Dictionary<string, object> JsonToDictionary(string jsonData)
        {
            //实例化JavaScriptSerializer类的新实例
            JavaScriptSerializer jss = new JavaScriptSerializer();
            try
            {
                //将指定的 JSON 字符串转换为 Dictionary<string, object> 类型的对象
                return jss.Deserialize<Dictionary<string, object>>(jsonData);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        #endregion

        #region ***泛型中去除重复项只取一条
        public class ListComparer<T> : IEqualityComparer<T>
        {
            public delegate bool EqualsComparer<F>(F x, F y);

            public EqualsComparer<T> equalsComparer;

            public ListComparer(EqualsComparer<T> _euqlsComparer)
            {
                this.equalsComparer = _euqlsComparer;
            }

            public bool Equals(T x, T y)
            {
                if (null != equalsComparer)
                {
                    return equalsComparer(x, y);
                }
                else
                {
                    return false;
                }
            }

            public int GetHashCode(T obj)
            {
                return obj.ToString().GetHashCode();
            }
        }
        #endregion
    }
}
