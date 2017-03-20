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
    
    public partial class ReturnRequest : EntityBase
    {
        public int Id { get; set; }
        public int StoreId { get; set; }
        public int OrderItemId { get; set; }
        public int CustomerId { get; set; }
        public int Quantity { get; set; }
        public string ReasonForReturn { get; set; }
        public string RequestedAction { get; set; }
        public string CustomerComments { get; set; }
        public string StaffNotes { get; set; }
        public int ReturnRequestStatusId { get; set; }
        public System.DateTime CreatedOnUtc { get; set; }
        public System.DateTime UpdatedOnUtc { get; set; }
    
        public virtual Customer Customer { get; set; }
    }
}
