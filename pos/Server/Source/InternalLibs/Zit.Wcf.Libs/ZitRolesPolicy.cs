using System;
using System.Collections.Generic;
using System.Text;
using System.IdentityModel.Policy;
using System.IdentityModel.Claims;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.Net;
using Zit.Security;
using Microsoft.Practices.ServiceLocation;
using Microsoft.Practices.EnterpriseLibrary.Caching;

namespace Zit.Wcf.Libs
{
    public class ZitRolesPolicy : IAuthorizationPolicy
    {
        Guid id = Guid.NewGuid();
        public bool Evaluate(EvaluationContext evaluationContext, ref object state)
        {
            IZitPrincipal pricipal = ZitSession.Current.Principal;
            evaluationContext.Properties["Principal"] = pricipal??(new ZitPrincipal());
            return true;
        }

        public System.IdentityModel.Claims.ClaimSet Issuer
        {
            get { return ClaimSet.System; }
        }

        public string Id
        {
            get { return id.ToString(); }
        }
    }
}
