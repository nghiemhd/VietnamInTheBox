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
    
    public partial class CF_Acct : EntityBase
    {
        public CF_Acct()
        {
            this.GJ_TranDetail = new HashSet<GJ_TranDetail>();
            this.IVM_Adj = new HashSet<IVM_Adj>();
        }
    
        public int Id { get; set; }
        public string AcctCode { get; set; }
        public string AcctName { get; set; }
        public bool RequireObj { get; set; }
    
        public virtual ICollection<GJ_TranDetail> GJ_TranDetail { get; set; }
        public virtual ICollection<IVM_Adj> IVM_Adj { get; set; }
    }
}