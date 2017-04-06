using GalaSoft.MvvmLight.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zit.Client.Wpf.Messages
{
    public class BarcodeDataMsg : MessageBase
    {
        public string Data { get; set; }
        public string CmdData { get; set; }
    }
}
