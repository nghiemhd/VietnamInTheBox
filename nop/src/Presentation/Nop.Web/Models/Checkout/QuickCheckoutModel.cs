using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Nop.Web.Models.Checkout
{
    public class QuickCheckoutModel
    {
        public string CustomerName { get; set; }
        [RegularExpression("[0-9 ]*",ErrorMessage="Số điện thoại không hợp lệ")]
        public string Phone { get; set; }
        [EmailAddress(ErrorMessage="Địa chỉ email không hợp lệ")]
        public string Email { get; set; }
        public string AddressHCM { get; set; }
        public string AddressOther { get; set; }

        public string Address
        {
            get
            {
                if (Province == CheckoutProvince.HCM)
                {
                    return AddressHCM;
                }
                else
                    return AddressOther;
            }
        }

        public bool IsFreeship { get; set; }

        [MaxLength(1000, ErrorMessage="Nhập tối đa 1000 ký tự")]
        public string Note { get; set; }
        public bool IsClose { get; set; }
        public CheckoutProvince? Province { get; set; }
    }

    public enum CheckoutProvince
    {
        HCM,
        Other
    }
}