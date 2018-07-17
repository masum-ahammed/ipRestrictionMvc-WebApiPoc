using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NetTools;
using System.Net;

namespace ipRenge.test
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            var rangeB1 = IPAddressRange.Parse("192.168.0.10 - 192.168.10.20");
           // rangeB1.ToCidrString();  // is 192.168.0.0/24
            bool result =   rangeB1.Contains(IPAddress.Parse("192.168.3.45"));
        }

        [TestMethod]
        public void TestMethodRangeTest()
        {

            var rangeA1 = IPAddressRange.Parse("192.168.0.0/24");
            var rangeA2 = IPAddressRange.Parse("192.168.10.0/24");
            var rangeB1 = IPAddressRange.Parse($"{rangeA1.Begin}-{rangeA2.End}");
            bool result = rangeB1.Contains(IPAddress.Parse("192.168.3.45"));
        }
    }
}
