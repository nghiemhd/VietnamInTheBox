using Nop.Core.Domain.Aero87;
using Nop.Web.Framework.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Nop.Admin.Models.Aero87
{
    public class AccountBalanceSearch : BaseNopEntityModel
    {
        [Display(Name = "Từ Ngày")]
        public DateTime FromDate { get; set; }
        [Display(Name = "Đến ngày")]
        public DateTime ToDate { get; set; }

        [Display(Name = "Đối tượng")]
        public int? ObjId { get; set; }
    }
}