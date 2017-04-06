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
    
    public partial class Product : EntityBase
    {
        public Product()
        {
            this.BackInStockSubscription = new HashSet<BackInStockSubscription>();
            this.IVM_Barcode = new HashSet<IVM_Barcode>();
            this.OrderItem = new HashSet<OrderItem>();
            this.Product_Category_Mapping = new HashSet<Product_Category_Mapping>();
            this.Product_Manufacturer_Mapping = new HashSet<Product_Manufacturer_Mapping>();
            this.Product_Picture_Mapping = new HashSet<Product_Picture_Mapping>();
            this.ProductReview = new HashSet<ProductReview>();
            this.Product_SpecificationAttribute_Mapping = new HashSet<Product_SpecificationAttribute_Mapping>();
            this.Product_ProductAttribute_Mapping = new HashSet<Product_ProductAttribute_Mapping>();
            this.ProductVariantAttributeCombination = new HashSet<ProductVariantAttributeCombination>();
            this.ShoppingCartItem = new HashSet<ShoppingCartItem>();
            this.TierPrice = new HashSet<TierPrice>();
            this.Discount = new HashSet<Discount>();
            this.ProductTag = new HashSet<ProductTag>();
            this.IVM_Transaction = new HashSet<IVM_Transaction>();
        }
    
        public int Id { get; set; }
        public string Name { get; set; }
        public string ShortDescription { get; set; }
        public string FullDescription { get; set; }
        public string AdminComment { get; set; }
        public int ProductTemplateId { get; set; }
        public int VendorId { get; set; }
        public bool ShowOnHomePage { get; set; }
        public string MetaKeywords { get; set; }
        public string MetaDescription { get; set; }
        public string MetaTitle { get; set; }
        public bool AllowCustomerReviews { get; set; }
        public int ApprovedRatingSum { get; set; }
        public int NotApprovedRatingSum { get; set; }
        public int ApprovedTotalReviews { get; set; }
        public int NotApprovedTotalReviews { get; set; }
        public bool SubjectToAcl { get; set; }
        public bool LimitedToStores { get; set; }
        public bool Published { get; set; }
        public bool Deleted { get; set; }
        public System.DateTime CreatedOnUtc { get; set; }
        public System.DateTime UpdatedOnUtc { get; set; }
        public int ProductTypeId { get; set; }
        public int ParentGroupedProductId { get; set; }
        public string SKU { get; set; }
        public string ManufacturerPartNumber { get; set; }
        public string Gtin { get; set; }
        public bool IsGiftCard { get; set; }
        public int GiftCardTypeId { get; set; }
        public bool RequireOtherProducts { get; set; }
        public string RequiredProductIds { get; set; }
        public bool AutomaticallyAddRequiredProducts { get; set; }
        public bool IsDownload { get; set; }
        public int DownloadId { get; set; }
        public bool UnlimitedDownloads { get; set; }
        public int MaxNumberOfDownloads { get; set; }
        public Nullable<int> DownloadExpirationDays { get; set; }
        public int DownloadActivationTypeId { get; set; }
        public bool HasSampleDownload { get; set; }
        public int SampleDownloadId { get; set; }
        public bool HasUserAgreement { get; set; }
        public string UserAgreementText { get; set; }
        public bool IsRecurring { get; set; }
        public int RecurringCycleLength { get; set; }
        public int RecurringCyclePeriodId { get; set; }
        public int RecurringTotalCycles { get; set; }
        public bool IsShipEnabled { get; set; }
        public bool IsFreeShipping { get; set; }
        public decimal AdditionalShippingCharge { get; set; }
        public bool IsTaxExempt { get; set; }
        public int TaxCategoryId { get; set; }
        public int ManageInventoryMethodId { get; set; }
        public int StockQuantity { get; set; }
        public bool DisplayStockAvailability { get; set; }
        public bool DisplayStockQuantity { get; set; }
        public int MinStockQuantity { get; set; }
        public int LowStockActivityId { get; set; }
        public int NotifyAdminForQuantityBelow { get; set; }
        public int BackorderModeId { get; set; }
        public bool AllowBackInStockSubscriptions { get; set; }
        public int OrderMinimumQuantity { get; set; }
        public int OrderMaximumQuantity { get; set; }
        public string AllowedQuantities { get; set; }
        public bool DisableBuyButton { get; set; }
        public bool DisableWishlistButton { get; set; }
        public bool AvailableForPreOrder { get; set; }
        public bool CallForPrice { get; set; }
        public decimal Price { get; set; }
        public decimal OldPrice { get; set; }
        public decimal ProductCost { get; set; }
        public Nullable<decimal> SpecialPrice { get; set; }
        public Nullable<System.DateTime> SpecialPriceStartDateTimeUtc { get; set; }
        public Nullable<System.DateTime> SpecialPriceEndDateTimeUtc { get; set; }
        public bool CustomerEntersPrice { get; set; }
        public decimal MinimumCustomerEnteredPrice { get; set; }
        public decimal MaximumCustomerEnteredPrice { get; set; }
        public bool HasTierPrices { get; set; }
        public bool HasDiscountsApplied { get; set; }
        public decimal Weight { get; set; }
        public decimal Length { get; set; }
        public decimal Width { get; set; }
        public decimal Height { get; set; }
        public Nullable<System.DateTime> AvailableStartDateTimeUtc { get; set; }
        public Nullable<System.DateTime> AvailableEndDateTimeUtc { get; set; }
        public bool VisibleIndividually { get; set; }
        public int DisplayOrder { get; set; }
        public Nullable<System.DateTime> PreOrderAvailabilityStartDateTimeUtc { get; set; }
        public int DeliveryDateId { get; set; }
        public int WarehouseId { get; set; }
    
        public virtual ICollection<BackInStockSubscription> BackInStockSubscription { get; set; }
        public virtual ICollection<IVM_Barcode> IVM_Barcode { get; set; }
        public virtual ICollection<OrderItem> OrderItem { get; set; }
        public virtual ICollection<Product_Category_Mapping> Product_Category_Mapping { get; set; }
        public virtual ICollection<Product_Manufacturer_Mapping> Product_Manufacturer_Mapping { get; set; }
        public virtual ICollection<Product_Picture_Mapping> Product_Picture_Mapping { get; set; }
        public virtual ICollection<ProductReview> ProductReview { get; set; }
        public virtual ICollection<Product_SpecificationAttribute_Mapping> Product_SpecificationAttribute_Mapping { get; set; }
        public virtual ICollection<Product_ProductAttribute_Mapping> Product_ProductAttribute_Mapping { get; set; }
        public virtual ICollection<ProductVariantAttributeCombination> ProductVariantAttributeCombination { get; set; }
        public virtual ICollection<ShoppingCartItem> ShoppingCartItem { get; set; }
        public virtual ICollection<TierPrice> TierPrice { get; set; }
        public virtual ICollection<Discount> Discount { get; set; }
        public virtual ICollection<ProductTag> ProductTag { get; set; }
        public virtual ICollection<IVM_Transaction> IVM_Transaction { get; set; }
    }
}