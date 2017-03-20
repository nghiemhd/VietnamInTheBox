using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Zit.Security
{
    public interface ITokenContainer
    {
        string GetToken();
        void SetToken(string token);
        string Issue();
    }
}
