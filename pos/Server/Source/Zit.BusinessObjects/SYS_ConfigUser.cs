//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Zit.BusinessObjects
{
    using Zit.Entity;
    using System;
    using System.Collections.Generic;
    
    public partial class SYS_ConfigUser : EntityBase
    {
        public int ConfigUserID { get; set; }
        public string GroupCode { get; set; }
        public string Val { get; set; }
        public string Description { get; set; }
        public string UserName { get; set; }
        public string ConfigKey { get; set; }
        public string CreatedUser { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public string UpdatedUser { get; set; }
        public Nullable<System.DateTime> UpdatedDate { get; set; }
        public byte[] VersionNo { get; set; }
    }
}
