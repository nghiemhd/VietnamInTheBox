using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Zit.Security
{
    public class ZitMenu : IComparable
    {
        public ZitMenu()
        {
            Menus = new SortedSet<ZitMenu>();
        }
        public string MenuName { get; set; }

        public string MenuCode { get; set; }

        public string MenuTypeInfo { get; set; }

        public string MenuUrl { get; set; }

        public int MenuID { get; set; }

        public int Order { get; set; }

        public bool Visible { get; set; }

        public SortedSet<ZitMenu> Menus { get; set; }

        public int CompareTo(object obj)
        {
            ZitMenu mn = obj as ZitMenu;
            if (mn == null) return -1;
            if (mn.Order < this.Order) return 1;
            if (mn.Order > this.Order) return -1;

            if (mn.MenuID == this.MenuID) return 0;

            return 1;
        }
    }
}
