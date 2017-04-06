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
    
    public partial class IVM_Adj : EntityBase
    {
        public IVM_Adj()
        {
            this.IVM_AdjDetail = new HashSet<IVM_AdjDetail>();
        }
    
        public int Id { get; set; }
        public string AdjNo { get; set; }
        public System.DateTime AdjDate { get; set; }
        public string Desc { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public string CreatedUser { get; set; }
        public string UpdatedUser { get; set; }
        public Nullable<System.DateTime> UpdatedDate { get; set; }
        public byte[] VersionNo { get; set; }
        public int Status { get; set; }
        public int DebitAcctId { get; set; }
        public Nullable<int> ObjId { get; set; }
        public decimal CostAmount { get; set; }
        public int Qty { get; set; }
        public int StoreId { get; set; }
    
        public virtual CF_Acct CF_Acct { get; set; }
        public virtual CF_Obj CF_Obj { get; set; }
        public virtual CF_Store CF_Store { get; set; }
        public virtual ICollection<IVM_AdjDetail> IVM_AdjDetail { get; set; }
    }
}
