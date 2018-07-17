using System;
using System.Collections.Generic;

using System.Linq;
using System.Web;

namespace IpRestrictPoc.Utility
{
    public class Config
    {
        public static string IpRenge
        {
            get
            {
                return System.Configuration.ConfigurationManager.AppSettings["IpRenge"];
            }
        }
    }
}