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
    
    public partial class tb_User
    {
        public string ID { get; set; }
        public string LoginID { get; set; }
        public string NickName { get; set; }
        public string LoginPwd { get; set; }
        public string RealName { get; set; }
        public string UserEmail { get; set; }
        public string UserPhone { get; set; }
        public int Sex { get; set; }
        public string Education { get; set; }
        public string YearWork { get; set; }
        public System.DateTime LastLognTime { get; set; }
        public decimal GoldMoney { get; set; }
        public decimal SGoldMoney { get; set; }
        public int Points { get; set; }
        public Nullable<int> Status { get; set; }
        public string ImgUrl { get; set; }
        public string Memo { get; set; }
        public System.DateTime CreateDate { get; set; }
        public int Role { get; set; }
        public int RoleStatus { get; set; }
    }
}
