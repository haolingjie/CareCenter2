//------------------------------------------------------------------------------
// <auto-generated>
//    此代码是根据模板生成的。
//
//    手动更改此文件可能会导致应用程序中发生异常行为。
//    如果重新生成代码，则将覆盖对此文件的手动更改。
// </auto-generated>
//------------------------------------------------------------------------------

namespace CareCenter2.DB
{
    using System;
    using System.Collections.Generic;
    
    public partial class tb_Admins
    {
        public string ID { get; set; }
        public string AdminID { get; set; }
        public string NickName { get; set; }
        public string AdminPwd { get; set; }
        public string RealName { get; set; }
        public string Mobile { get; set; }
        public System.DateTime LastLoginTime { get; set; }
        public int Status { get; set; }
        public string Memo { get; set; }
        public System.DateTime CreateDate { get; set; }
    }
}
