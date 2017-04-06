using Nop.Core.Domain.Aero87;
using Nop.Web.Framework.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Nop.Admin.Models.Aero87
{
    public class GeneralLedgerSearch : BaseNopEntityModel
    {
        [Display(Name="Từ Ngày")]
        public DateTime? FromDate { get; set; }
        [Display(Name = "Đến ngày")]
        public DateTime? ToDate { get; set; }

        [Display(Name = "Loại chứng từ")]
        public DocType? DocType { get; set; }
        [Display(Name = "Số chứng từ")]
        public string DocNo { get; set; }
        [Display(Name = "Mã tài khoản")]
        public string AcctCode { get; set; }
        [Display(Name = "Mã đối tượng")]
        public string ObjCode { get; set; }
        [Display(Name = "Tên đối tượng")]
        public string ObjName { get; set; }
    }
}