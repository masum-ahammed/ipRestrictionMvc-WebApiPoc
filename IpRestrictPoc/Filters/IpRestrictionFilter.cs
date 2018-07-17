using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http.Controllers;
using System.Web.Mvc;
using IpRestrictPoc.Extentions;
using System.Net;
using IpRestrictPoc.Utility;
using NetTools;
using System.Runtime.Remoting.Contexts;

namespace IpRestrictPoc.Filters
{
    public class IpRestrictionFilter :DelegatingHandler,  IActionFilter
    {
        private static readonly log4net.ILog _Logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
           

            //Retrieve user's IP
            string usersIpAddress = request.GetClientIpAddress();
            _Logger.Debug("CurrentIp : " + usersIpAddress);
            //_Logger.Debug("Ip from GetIp method : " + this.GetIp());
            if (this.IsValidIp(usersIpAddress))
            {
               return await base.SendAsync(request, cancellationToken);
            }
            return  request.CreateErrorResponse(HttpStatusCode.Forbidden, "This ip is restricted.");
          
        }
        public void OnActionExecuted(ActionExecutedContext filterContext)
        {
          
            string usersIpAddress = filterContext.RequestContext.HttpContext.Request.UserHostAddress;
            _Logger.Debug("CurrentIp in MVC : " + usersIpAddress);
            if (!this.IsValidIp(usersIpAddress))
            {
                filterContext.Result = new HttpStatusCodeResult(403);
                //throw new UnauthorizedAccessException("Ip restricted");
            }
           
            //_Logger.Debug("Ip from GetIp method : " + this.GetIp());
            //throw new NotImplementedException();
        }

        public void OnActionExecuting(ActionExecutingContext filterContext)
        {
           // throw new NotImplementedException();
        }

        private bool IsValidIp(string ip)
        {
            if(ip == "127.0.0.1")
            {
                ip = GetIp();
            }
            string ipRenge = Config.IpRenge;
            if (ipRenge.Contains("-"))
            {
                string[] split = ipRenge.Split('-');
                var rangeA1 = IPAddressRange.Parse(split[0]);
                var rangeA2 = IPAddressRange.Parse(split[1]);
                var rangeB1 = IPAddressRange.Parse($"{rangeA1.Begin}-{rangeA2.End}");
                return rangeB1.Contains(IPAddress.Parse(ip));
            }
            else
            {
                var rangeA1 = IPAddressRange.Parse(ipRenge);
               return   rangeA1.Contains(IPAddress.Parse(ip));
            }
         
        }

        private string GetIp()
        {
            string hostName = Dns.GetHostName(); // Retrive the Name of HOST  
            Console.WriteLine(hostName);
            // Get the IP  
            string myIP = Dns.GetHostEntry(hostName).AddressList[1].ToString();
            return myIP;
        }
    }
}