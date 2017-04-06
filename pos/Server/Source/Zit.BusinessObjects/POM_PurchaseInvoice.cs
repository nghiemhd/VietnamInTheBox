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
    
    public partial class POM_PurchaseInvoice : EntityBase
    {
        public POM_PurchaseInvoice()
        {
            this.POM_PurchaseInvoiceDetail = new HashSet<POM_PurchaseInvoiceDetail>();
        }
    
        public int Id { get; set; }
        public string InvoiceNo { get; set; }
        public string RefNo { get; set; }
        public System.DateTime InvoiceDate { get; set; }
        public decimal Amount { get; set; }
        public string Desc { get; set; }
        public string CreatedUser { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public string UpdatedUser { get; set; }
        public Nullable<System.DateTime> UpdatedDate { get; set; }
        public byte[] VersionNo { get; set; }
        public int Status { get; set; }
        public Nullable<int> CreditAcctId { get; set; }
        public Nullable<int> ObjId { get; set; }
        public int StoreId { get; set; }
    
        public virtual CF_Store CF_Store { get; set; }
        public virtual ICollection<POM_PurchaseInvoiceDetail> POM_PurchaseInvoiceDetail { get; set; }
    }
}