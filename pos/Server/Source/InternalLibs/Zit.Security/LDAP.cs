using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.DirectoryServices.AccountManagement;
using System.DirectoryServices;

namespace Zit.Security
{
    public class LDAP
    {
        private readonly string _domain;
        private readonly string _container;
        private readonly string _userName;
        private readonly string _pass;

        public LDAP (string domain,string container,string username, string password)
	    {
            _domain = domain;
            _container = container;
            _userName = username;
            _pass = password;
	    }

        public bool Login()
        {
            PrincipalContext ctx = new PrincipalContext(ContextType.Domain, _domain,_container);
            return ctx.ValidateCredentials(_userName, _pass);
        }

        public SearchResult GetProperties()
        {
            DirectoryEntry entry = new DirectoryEntry(string.Format("LDAP://{0}/{1}",_domain,_container), _userName, _pass);
            DirectorySearcher adSearcher = new DirectorySearcher(entry);
            adSearcher.SearchScope = SearchScope.Subtree;
            adSearcher.Filter = "(&(objectClass=user)(samaccountname=" + _userName + "))";
            SearchResult userObject = adSearcher.FindOne();
            return userObject;
        }
    }
}
