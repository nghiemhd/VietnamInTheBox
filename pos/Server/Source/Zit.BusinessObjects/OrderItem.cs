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
    
    public partial class OrderItem : EntityBase
    {
        public OrderItem()
        {
            this.GiftCard = new HashSet<GiftCard>();
        }
    
        public int Id { get; set; }
        public System.Guid OrderItemGuid { get; set; }
        public int OrderId { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPriceInclTax { get; set; }
        public decimal UnitPriceExclTax { get; set; }
        public decimal PriceInclTax { get; set; }
        public decimal PriceExclTax { get; set; }
        public decimal DiscountAmountInclTax { get; set; }
        public decimal DiscountAmountExclTax { get; set; }
        public string AttributeDescription { get; set; }
        public string AttributesXml { get; set; }
        public int DownloadCount { get; set; }
        public bool IsDownloadActivated { get; set; }
        public Nullable<int> LicenseDownloadId { get; set; }
        public Nullable<decimal> ItemWeight { get; set; }
        public int ProductId { get; set; }
        public decimal OriginalProductCost { get; set; }
    
        public virtual ICollection<GiftCard> GiftCard { get; set; }
        public virtual Order Order { get; set; }
        public virtual Product Product { get; set; }
    }
}