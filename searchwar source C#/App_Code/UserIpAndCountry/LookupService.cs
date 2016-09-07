using System;
using System.Web;
using CountryLookupProj;

public class LookUpService {
    public LookUpService() {
    }

    public static string GetCountry(string ipAddress) {

        try {
            
            

            HttpContext c = HttpContext.Current;
            CountryLookup cl = new CountryLookup(c.Server.MapPath("~/GeoIpDB/GeoIP.dat"));

            object userCountry = c.Cache["country-" + ipAddress];
            if (userCountry == null)
            {
                userCountry = cl.lookupCountryCode(ipAddress);

                // add Cache
                c.Cache.Add("country-" + ipAddress, userCountry,
                            null,
                            TimeZoneManager.DateTimeNow.AddDays(3),
                            System.Web.Caching.Cache.NoSlidingExpiration,
                            System.Web.Caching.CacheItemPriority.Normal,
                            null);
                
            }

            
            

            return userCountry.ToString();
        } catch {
            return null;
        }

    }

    public static string GetIpAddress() {

        #region Finding Visitors IP Address
        string VisitorsIPAddr = string.Empty;

        //Users IP Address.                
        if (HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"] != null) {
            //To get the IP address of the machine and not the proxy
            VisitorsIPAddr = HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"].ToString();
        } else if (HttpContext.Current.Request.UserHostAddress.Length != 0) {
            VisitorsIPAddr = HttpContext.Current.Request.UserHostAddress;
        }
        #endregion

        return VisitorsIPAddr;
    }

}