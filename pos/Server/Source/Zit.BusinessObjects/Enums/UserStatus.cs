using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zit.BusinessObjects.Enums
{
    public enum UserStatus
    {
        [Display(Name = "Hết hiệu lực")]
        Stop = 1,
        [Display(Name = "Hiệu lực")]
        Active = 2
    }
}
