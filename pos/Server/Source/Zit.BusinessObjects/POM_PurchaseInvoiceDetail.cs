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
    
    public partial class POM_PurchaseInvoiceDetail : EntityBase
    {
        public int Id { get; set; }
        public int PurchaseInvoiceId { get; set; }
        public int BarcodeId { get; set; }
        public int Qty { get; set; }
        public decimal Price { get; set; }
        public decimal Amount { get; set; }
        public string CreatedUser { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public string UpdatedUser { get; set; }
        public Nullable<System.DateTime> UpdatedDate { get; set; }
        public byte[] VersionNo { get; set; }
    
        public virtual IVM_Barcode IVM_Barcode { get; set; }
        public virtual POM_PurchaseInvoice POM_PurchaseInvoice { get; set; }
    }
}
