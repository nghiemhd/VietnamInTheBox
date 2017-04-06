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
    
    public partial class IVM_Barcode : EntityBase
    {
        public IVM_Barcode()
        {
            this.IVM_AdjDetail = new HashSet<IVM_AdjDetail>();
            this.IVM_BarcodeDetail = new HashSet<IVM_BarcodeDetail>();
            this.IVM_InvTransferDetail = new HashSet<IVM_InvTransferDetail>();
            this.IVM_Transaction = new HashSet<IVM_Transaction>();
            this.POM_PurchaseInvoiceDetail = new HashSet<POM_PurchaseInvoiceDetail>();
            this.POS_SaleOrderDetail = new HashSet<POS_SaleOrderDetail>();
        }
    
        public int Id { get; set; }
        public string Barcode { get; set; }
        public int ProductId { get; set; }
    
        public virtual ICollection<IVM_AdjDetail> IVM_AdjDetail { get; set; }
        public virtual Product Product { get; set; }
        public virtual ICollection<IVM_BarcodeDetail> IVM_BarcodeDetail { get; set; }
        public virtual ICollection<IVM_InvTransferDetail> IVM_InvTransferDetail { get; set; }
        public virtual ICollection<IVM_Transaction> IVM_Transaction { get; set; }
        public virtual ICollection<POM_PurchaseInvoiceDetail> POM_PurchaseInvoiceDetail { get; set; }
        public virtual ICollection<POS_SaleOrderDetail> POS_SaleOrderDetail { get; set; }
    }
}
