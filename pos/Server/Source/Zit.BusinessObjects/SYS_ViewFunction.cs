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
    
    public partial class SYS_ViewFunction : EntityBase
    {
        public int ViewFunctionID { get; set; }
        public int ViewID { get; set; }
        public int FunctionID { get; set; }
        public string CreatedUser { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public string UpdatedUser { get; set; }
        public Nullable<System.DateTime> UpdatedDate { get; set; }
        public byte[] VersionNo { get; set; }
        public bool IsDefault { get; set; }
        public Nullable<bool> IsDisplayed { get; set; }
    
        public virtual SYS_Function SYS_Function { get; set; }
        public virtual SYS_View SYS_View { get; set; }
    }
}
