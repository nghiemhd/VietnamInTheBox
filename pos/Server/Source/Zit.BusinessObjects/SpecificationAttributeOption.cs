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
    
    public partial class SpecificationAttributeOption : EntityBase
    {
        public SpecificationAttributeOption()
        {
            this.Product_SpecificationAttribute_Mapping = new HashSet<Product_SpecificationAttribute_Mapping>();
        }
    
        public int Id { get; set; }
        public int SpecificationAttributeId { get; set; }
        public string Name { get; set; }
        public int DisplayOrder { get; set; }
    
        public virtual ICollection<Product_SpecificationAttribute_Mapping> Product_SpecificationAttribute_Mapping { get; set; }
        public virtual SpecificationAttribute SpecificationAttribute { get; set; }
    }
}
