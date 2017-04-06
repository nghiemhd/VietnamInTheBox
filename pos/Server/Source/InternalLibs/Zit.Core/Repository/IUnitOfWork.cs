using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Zit.Core.Repository;

namespace Zit.Core.Repository {
    public interface IUnitOfWork : IDisposable 
    {
        IObjectContext Context { get;}
        void Commit();
    }
}
