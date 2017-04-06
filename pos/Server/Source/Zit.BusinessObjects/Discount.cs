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
    
    public partial class Discount : EntityBase
    {
        public Discount()
        {
            this.DiscountRequirement = new HashSet<DiscountRequirement>();
            this.DiscountUsageHistory = new HashSet<DiscountUsageHistory>();
            this.Category = new HashSet<Category>();
            this.Product = new HashSet<Product>();
        }
    
        public int Id { get; set; }
        public string Name { get; set; }
        public int DiscountTypeId { get; set; }
        public bool UsePercentage { get; set; }
        public decimal DiscountPercentage { get; set; }
        public decimal DiscountAmount { get; set; }
        public Nullable<System.DateTime> StartDateUtc { get; set; }
        public Nullable<System.DateTime> EndDateUtc { get; set; }
        public bool RequiresCouponCode { get; set; }
        public string CouponCode { get; set; }
        public int DiscountLimitationId { get; set; }
        public int LimitationTimes { get; set; }
    
        public virtual ICollection<DiscountRequirement> DiscountRequirement { get; set; }
        public virtual ICollection<DiscountUsageHistory> DiscountUsageHistory { get; set; }
        public virtual ICollection<Category> Category { get; set; }
        public virtual ICollection<Product> Product { get; set; }
    }
}
