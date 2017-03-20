using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zit.Security
{
    public interface IRequestContainer
    {
        bool Contain(string key);
        T Get<T>(string key) where T : class;
        void Set<T>(string key, T data) where T : class;
    }
}
