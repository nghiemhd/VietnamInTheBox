using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zit.Client.Wpf.Messages
{
    public class CommandMsg : GalaSoft.MvvmLight.Messaging.MessageBase
    {
        public string Data { get; set; }
    }
}
