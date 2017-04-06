using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Nop.Core.Domain.Aero87
{
    public class InventoryTransactionResult : BaseEntity
    {
        public int RefId { get; set; }
        public DateTime TransDate { get; set; }
        public int TypeId { get; set; }
        public string TransNo { get; set; }
        public int StoreId { get; set; }
        public string StoreCode { get; set; }
        public string ProductCode { get; set; }
        public int ProductId { get; set; }
        public string Barcode { get; set; }
        public string AttributeDesc { get; set; }
        public string PictureUrl { get; set; }
        public int Qty { get; set; }
        public string CreatedUser { get; set; }
    }
}