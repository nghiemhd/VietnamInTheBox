using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zit.Client.Wpf.Infractstructure
{
    public interface IScanner : IDisposable
    {
        bool TryOpen();
        event Action<string> ScanEvent;
        bool Status { get; }
    }
}
