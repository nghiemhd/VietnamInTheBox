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
    
    public partial class OrderNote : EntityBase
    {
        public int Id { get; set; }
        public int OrderId { get; set; }
        public string Note { get; set; }
        public bool DisplayToCustomer { get; set; }
        public System.DateTime CreatedOnUtc { get; set; }
    
        public virtual Order Order { get; set; }
    }
}
