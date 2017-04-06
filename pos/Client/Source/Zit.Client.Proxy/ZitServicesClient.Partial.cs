using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using Zit.Client.Proxy;

namespace Zit.Client.Proxy.ZitServices
{
    public partial class ZitServicesClient : IDisposable
    {
        public void Dispose()
        {
            try
            {
                if (State == CommunicationState.Faulted)
                {
                    this.Abort();
                }
                else
                {
                    this.Close();
                }
            }
            catch
            {
                this.Abort();
            }
        }
    }
}
