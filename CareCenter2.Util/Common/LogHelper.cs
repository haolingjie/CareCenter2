using CareCenter2.Util.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

[assembly: log4net.Config.XmlConfigurator(ConfigFile = "Log4net.config", Watch = true)]
namespace CareCenter2.Util.Common
{
    /// <summary>
    /// LOG出力类
    /// </summary>
    public class LogHelper
    {
        /// <summary>
        /// LOG配置文件
        /// </summary>
        public const string CONFIG_FILE = @"Log4net.config";

        private static readonly log4net.ILog logerror = log4net.LogManager.GetLogger("Error_Log");
        private static readonly log4net.ILog loginfo = log4net.LogManager.GetLogger("Info_Log");
        private static readonly log4net.ILog logdebug = log4net.LogManager.GetLogger("Debug_Log");

        /// <summary>
        /// 类构造
        /// </summary>
        static LogHelper()
        {
        }

        /// <summary>
        /// 信息LOG
        /// </summary>
        /// <param name="info">信息</param>
        public static void SetConfig(string rootpath)
        {
            FileInfo configFile = new FileInfo(System.IO.Path.Combine(rootpath, CONFIG_FILE));
            log4net.Config.XmlConfigurator.Configure(configFile);
        }

        /// <summary>
        /// 信息LOG
        /// </summary>
        /// <param name="info">信息</param>
        public static void Info(string info)
        {
            if (loginfo.IsInfoEnabled)
            {
                loginfo.Info(info);
            }
        }

        /// <summary>
        /// 错误LOG
        /// </summary>
        /// <param name="info">错误信息</param>
        /// <param name="se">异常</param>
        public static void Error(string info, Exception se)
        {
            if (logerror.IsErrorEnabled)
            {
                logerror.Error(info, se);

                // 调用方法堆栈信息
                logerror.Error(GetMethodStackTrace());
            }

#if DEBUG
            System.Diagnostics.Debug.WriteLine(DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss") + " " + info);
            System.Diagnostics.Debug.WriteLine(DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss") + " " + GetMethodStackTrace());
#endif
        }

        /// <summary>
        /// 错误LOG
        /// </summary>
        /// <param name="info">错误信息</param>
        public static void Error(string info)
        {
            if (logerror.IsErrorEnabled)
            {
                logerror.Error(info);
            }

#if DEBUG
            System.Diagnostics.Debug.WriteLine(DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss") + " " + info);
#endif
        }

        /// <summary>
        /// 错误LOG
        /// </summary>
        /// <param name="info">错误信息</param>
        /// <param name="se">异常</param>
        public static void Error(Exception se)
        {
            if (logerror.IsErrorEnabled)
            {
                logerror.Error("系统错误。", se);

                // 调用方法堆栈信息
                logerror.Error(GetMethodStackTrace());
            }

#if DEBUG
            System.Diagnostics.Debug.WriteLine(DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss") + " " + se.Message);
            System.Diagnostics.Debug.WriteLine(DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss") + " " + GetMethodStackTrace());
#endif
        }

        /// <summary>
        /// 调试信息LOG
        /// </summary>
        /// <param name="info">调试信息</param>
        public static void Debug(string info)
        {
            if (logdebug.IsDebugEnabled)
            {
                logdebug.Debug(info);
            }
        }

        /// <summary>
        /// 调试信息LOG
        /// </summary>
        /// <param name="info">调试信息</param>
        public static void Debug(string info, object obj)
        {
            if (logdebug.IsDebugEnabled)
            {
                logdebug.Debug(info + "\r\n" + Utility.DumpProperties(obj));
            }

        }

        /// <summary>
        /// 获取方法堆栈信息
        /// </summary>
        /// <returns></returns>
        static string GetMethodStackTrace()
        {
            System.Text.StringBuilder bf = new System.Text.StringBuilder();
            // 当前堆栈信息
            System.Diagnostics.StackTrace st = new System.Diagnostics.StackTrace();
            System.Diagnostics.StackFrame[] sfs = st.GetFrames();
            bf.AppendLine("方法调用堆栈信息:");
            // 忽略自己（GetMethodStackTrace）
            for (int i = 1; i < sfs.Length; ++i)
            {
                System.Diagnostics.StackFrame sf = sfs[i];

                // 非用户代码,系统方法及后面的都是系统调用，不获取用户代码调用结束
                if (System.Diagnostics.StackFrame.OFFSET_UNKNOWN == sf.GetILOffset())
                    break;

                // 忽略LogHelper:Error
                if ("Error".Equals(sf.GetMethod().Name))
                    continue;

                // 没有PDB文件的情况下将始终返回0
                if (sf.GetFileLineNumber() == 0)
                    bf.AppendFormat("   *** 在 {0}.{1}() \r\n", new string[] { sf.GetMethod().DeclaringType.FullName, sf.GetMethod().Name });
                else
                    bf.AppendFormat("   *** 在 {0}.{1}() 位置 {2}:行号 {3}\r\n", new string[] { sf.GetMethod().DeclaringType.FullName, sf.GetMethod().Name, sf.GetFileName(), sf.GetFileLineNumber().ToString() });
            }
            return bf.ToString();
        }

    }
}
