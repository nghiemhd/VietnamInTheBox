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
    
    public partial class GJ_Tran : EntityBase
    {
        public GJ_Tran()
        {
            this.GJ_TranDetail = new HashSet<GJ_TranDetail>();
        }
    
        public int Id { get; set; }
        public string DocNo { get; set; }
        public System.DateTime DocDate { get; set; }
        public string Desc { get; set; }
        public int TypeId { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public string CreatedUser { get; set; }
        public string UpdatedUser { get; set; }
        public Nullable<System.DateTime> UpdatedDate { get; set; }
        public byte[] VersionNo { get; set; }
    
        public virtual ICollection<GJ_TranDetail> GJ_TranDetail { get; set; }
    }
}
