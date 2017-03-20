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
    
    public partial class IVM_Transaction : EntityBase
    {
        public int Id { get; set; }
        public int TypeId { get; set; }
        public int RefId { get; set; }
        public int BarcodeId { get; set; }
        public int Qty { get; set; }
        public decimal CostPrice { get; set; }
        public decimal CostAmount { get; set; }
        public System.DateTime CreatedDate { get; set; }
        public System.DateTime TransDate { get; set; }
        public int StoreId { get; set; }
        public int ProductId { get; set; }
    
        public virtual CF_Store CF_Store { get; set; }
        public virtual IVM_Barcode IVM_Barcode { get; set; }
        public virtual Product Product { get; set; }
    }
}
