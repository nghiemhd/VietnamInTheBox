using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Zit.Client.Proxy;

namespace UnitTest.Services
{
    [TestClass]
    public class AuthenticateClient
    {
        [TestMethod]
        public void Login()
        {
            var client = ProxyFactory.CreateZitServices();
            var rp = client.Login(-1, "admin", "admin", "76205a984a9845b2ee0a9d2aa96e7149c6ec2270");
        }
    }
}
