using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.Web;

namespace HVMAPP.Models
{
    public class IPADDRESS
    {
        public string GetGlobalIpAdress()
        {
            try
            {
                string ClientIp = String.Empty;
                if (HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"] != null)
                {
                    ClientIp = HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"].ToString();
                }
                else if (HttpContext.Current.Request.UserHostAddress.Length != 0)
                {
                    ClientIp = HttpContext.Current.Request.UserHostAddress;
                }
                return ClientIp;
            }
            catch
            {
                throw;
            }
        }
    }
}