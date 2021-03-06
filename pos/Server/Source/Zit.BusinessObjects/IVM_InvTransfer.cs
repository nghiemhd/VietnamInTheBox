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
    
    public partial class IVM_InvTransfer : EntityBase
    {
        public IVM_InvTransfer()
        {
            this.IVM_InvTransferDetail = new HashSet<IVM_InvTransferDetail>();
        }
    
        public int Id { get; set; }
        public int StoreId { get; set; }
        public int ToStoreId { get; set; }
        public string TransferNo { get; set; }
        public System.DateTime TransferDate { get; set; }
        public string Desc { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public string CreatedUser { get; set; }
        public string UpdatedUser { get; set; }
        public Nullable<System.DateTime> UpdatedDate { get; set; }
        public byte[] VersionNo { get; set; }
        public int Status { get; set; }
        public int Qty { get; set; }
    
        public virtual CF_Store CF_Store { get; set; }
        public virtual CF_Store CF_Store1 { get; set; }
        public virtual ICollection<IVM_InvTransferDetail> IVM_InvTransferDetail { get; set; }
    }
}
