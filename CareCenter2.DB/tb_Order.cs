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
    
    public partial class tb_Order
    {
        public string OrderID { get; set; }
        public string UserID { get; set; }
        public string OrderTitle { get; set; }
        public string OrderIntor { get; set; }
        public int OrderClassify { get; set; }
        public int Status { get; set; }
        public string Memo { get; set; }
        public System.DateTime CreateDate { get; set; }
        public string OrderType { get; set; }
        public string AdminUserID { get; set; }
        public string PersonInro { get; set; }
        public string Address { get; set; }
        public string CeraRange { get; set; }
        public string DateType { get; set; }
        public string CareAddress { get; set; }
        public string CarePerson { get; set; }
    }
}
