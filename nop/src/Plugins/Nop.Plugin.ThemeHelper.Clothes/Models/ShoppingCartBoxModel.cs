﻿using Nop.Web.Framework.Mvc;

namespace Nop.Plugin.ThemeHelper.Clothes.Models
{
    public class ShoppingCartBoxModel : BaseNopModel
    {
        public int ItemsCount { get; set; }
        public bool ShoppingCartEnabled { get; set; }
    }
}