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
    
    public partial class Seq_No : EntityBase
    {
        public Seq_No()
        {
            this.Seq_Group = new HashSet<Seq_Group>();
        }
    
        public int SeqNoID { get; set; }
        public string TranType { get; set; }
        public string Prefix { get; set; }
        public Nullable<int> LastNumber { get; set; }
        public Nullable<int> Length { get; set; }
        public Nullable<byte> Year { get; set; }
        public Nullable<byte> Month { get; set; }
        public Nullable<int> LastYear { get; set; }
        public Nullable<int> LastMonth { get; set; }
        public string YearSeparate { get; set; }
        public string MonthSeparate { get; set; }
        public Nullable<bool> IsMultiGroup { get; set; }
    
        public virtual ICollection<Seq_Group> Seq_Group { get; set; }
    }
}