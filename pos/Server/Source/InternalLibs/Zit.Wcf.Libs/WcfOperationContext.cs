using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using System.ServiceModel;

namespace Zit.Wcf.Libs
{
    public class WcfOperationContext : IExtension<OperationContext>
    {
        private readonly IDictionary items;

        private WcfOperationContext()
        {
            items = new Hashtable();
        }

        public IDictionary Items
        {
            get { return items; }
        }

        public static WcfOperationContext Current
        {
            get
            {
                WcfOperationContext context = OperationContext.Current.Extensions.Find<WcfOperationContext>();
                if (context == null)
                {
                    context = new WcfOperationContext();
                    OperationContext.Current.Extensions.Add(context);
                }
                return context;
            }
        }

        public void Attach(OperationContext owner) { }

        public void Detach(OperationContext owner) { }
    }
}
