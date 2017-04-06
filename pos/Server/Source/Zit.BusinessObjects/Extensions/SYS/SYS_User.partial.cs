using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Zit.BusinessObjects.Enums;

namespace Zit.BusinessObjects
{
    public partial class SYS_User
    {
        public UserStatus UserStatus
        {
            get
            {
                return (UserStatus)this.Status;
            }
            set
            {
                this.Status = (short)value;
            }
        }
    }
}
