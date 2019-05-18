using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Configuration;

namespace CareCenter2.Util.Common
{
    public class ConfigHelper
    {
        private static Configuration m_Config = null;

        static ConfigHelper()
        {
            string path = null;
            bool isWeb = false;
            if (System.Web.HttpContext.Current != null)
            {
                isWeb = true;
                try
                {
                    path = System.Web.HttpContext.Current.Request.ApplicationPath;
                    //path = HttpRuntime.AppDomainAppPath;
                }
                catch (Exception)
                {
                    path = "~";
                }
            }
            else
            {
                isWeb = false;
                path = System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName;
            }

            SetConfigFile(path, isWeb);
        }

        /// <summary>
        /// 设置配置文件路径
        /// </summary>
        /// <param name="path"></param>
        /// <param name="isWeb"></param>
        /// <returns></returns>
        public static bool SetConfigFile(string path, bool isWeb = true)
        {
            if (string.IsNullOrEmpty(path))
                return false;

            try
            {
                if (isWeb)
                    m_Config = WebConfigurationManager.OpenWebConfiguration(path);
                else
                    m_Config = ConfigurationManager.OpenExeConfiguration(path);

                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }            
        }

        #region "加解密"

        /// <summary>
        /// 加密配置节
        /// </summary>
        /// <param name="setion"></param>
        /// <returns></returns>
        public static bool EncryptSection(string setion = null)
        {
            if (string.IsNullOrEmpty(setion))
                //return false;
                setion = "connectionStrings";

            try
            {
                ConfigurationSection appSec = m_Config.GetSection(setion);

                if (appSec != null)
                {
                    if (!appSec.SectionInformation.IsProtected)
                    {
                        appSec.SectionInformation.ProtectSection("DataProtectionConfigurationProvider");
                        appSec.SectionInformation.ForceSave = true;
                        return Save();
                    }
                    else
                    {
                        return true;
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return false;
        }

        /// <summary>
        /// 解密配置节
        /// </summary>
        /// <param name="setion"></param>
        /// <returns></returns>
        public static bool DecryptSection(string setion = null)
        {
            if (string.IsNullOrEmpty(setion))
                //return false;
                setion = "connectionStrings";

            try
            {
                ConfigurationSection appSec = m_Config.GetSection(setion);

                if (appSec != null)
                {
                    if (appSec.SectionInformation.IsProtected)
                    {
                        appSec.SectionInformation.UnprotectSection();
                        return Save();
                    }
                    return true;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return false;
        }

        /// <summary>
        /// 保存设置
        /// </summary>
        /// <returns></returns>
        private static bool Save()
        {
            if (m_Config == null)
                return false;

            try
            {
                System.IO.FileInfo fs = new System.IO.FileInfo(m_Config.FilePath);
                if (fs.IsReadOnly)
                {
                    FileAttributes fa = fs.Attributes;

                    fs.Attributes = (FileAttributes)((int)fs.Attributes & 0xFFFE);
                    m_Config.Save();
                    fs.Attributes = fa;

                    return true;
                }
                else
                {
                    m_Config.Save();
                    return true;
                }
            }
            catch (Exception ex)
            {
                throw ex;
                return false;
            }
        }

        #endregion

        #region "获取配置值"

        /// <summary>
        /// 获取配置参数(app.config)
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string GetAppSettingString(string key, string def = "")
        {
            try
            {
                if (string.IsNullOrEmpty(key))
                    return def;
                string value = m_Config.AppSettings.Settings[key].Value;
                if (string.IsNullOrEmpty(value))
                    return def;
                return value;
            }
            catch (Exception ex)
            {
                throw ex;
                return def;
            }
        }

        /// <summary>
        /// 获取配置参数(app.config)
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static int GetAppSettingInt(string key, int def = 0)
        {
            string value = GetAppSettingString(key);
            if (string.IsNullOrEmpty(value))
                return def;
            int ret = 0;
            if (int.TryParse(value, out ret))
                return ret;
            else
                return def;
        }

        /// <summary>
        /// 获取配置参数(app.config)
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static decimal GetAppSettingDecimal(string key, int def = 0)
        {
            string value = GetAppSettingString(key);
            if (string.IsNullOrEmpty(value))
                return def;
            decimal ret = 0;
            if (decimal.TryParse(value, out ret))
                return ret;
            else
                return def;
        }

        #endregion
    }
}
