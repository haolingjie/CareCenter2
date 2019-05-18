using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CareCenter2.Util.Common
{
   
    /// <summary>
    /// 获取手机验证码类
    /// </summary>
    public class SendPhoneCode
    {
        /// <summary>
        /// 修改手机号码短信变量
        /// </summary>
        static Dictionary<string, PhoneParameter> ChangePhoneUserCode = new Dictionary<string, PhoneParameter>();

        /// <summary>
        /// 发送手机验证码
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static string SendVerifyCode(string phone, out DateTime Vdate)
        {
            string code = new Random().Next(100000, 999999).ToString();
            PhoneParameter sms = new PhoneParameter();
            if (ChangePhoneUserCode.ContainsKey(phone))
            {
                ChangePhoneUserCode[phone].PhoneCode = code;
                ChangePhoneUserCode[phone].createTime = DateTime.Now;
            }
            else
            {

                sms.PhoneCode = code;
                sms.createTime = DateTime.Now;
                ChangePhoneUserCode.Add(phone, sms);
            }

            if (Utility.SendPhone(phone, code))
            {
                Vdate = ChangePhoneUserCode[phone].createTime; ;
                return code;//发送成功则提示返回当前页面；

            }
            else
            {
                Vdate = ChangePhoneUserCode[phone].createTime;
                return "";
            }
        }
    }
    /// <summary>
    /// 发送手机号码实体
    /// </summary>
    public class PhoneParameter
    {
        /// <summary>
        /// 手机号码
        /// </summary>
        public String PhoneCode { get; set; }

        /// <summary>
        /// 时间
        /// </summary>
        public DateTime createTime { get; set; }
    }
}
