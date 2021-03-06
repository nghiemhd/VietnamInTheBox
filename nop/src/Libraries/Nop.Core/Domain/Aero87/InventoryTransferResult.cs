﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Nop.Core.Domain.Aero87
{
    public class InventoryTransferResult : BaseEntity
    {
        public int StoreId { get; set; }

        public string StoreCode { get; set; }

        public int ToStoreId { get; set; }

        public string ToStoreCode { get; set; }

        public string TransferNo { get; set; }

        public DateTime TransferDate { get; set; }

        public string Desc { get; set; }

        public int Qty { get; set; }

        public int Status { get; set; }

        public string StatusName {
            get {
                return CommonHelper.DisplayForEnumValue((ITStatus)Status); ;
            }
        }

        public string CreatedUser { get; set; }
    }
}