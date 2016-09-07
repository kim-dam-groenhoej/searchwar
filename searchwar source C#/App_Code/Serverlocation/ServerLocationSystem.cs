using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Linq;
using System.Web;
using Searchwar_netModel;

/// <summary>
/// Summary description for ServerLocationSystem
/// </summary>
namespace Searchwar.Servers
{
    public class ServerLocationSystem
    {
        public ServerLocationSystem()
        {
            //
            // TODO: Add constructor logic here
            //
        }

        private struct AmazonServerolumnNoPosition
        {
            public string getCountry;
        }


    private static readonly Func<Searchwar_netEntities, AmazonServerolumnNoPosition, string>
            GetFastServerName = System.Data.Objects.CompiledQuery.Compile<Searchwar_netEntities, AmazonServerolumnNoPosition, string>
                                          ((db, p) => (from c in db.SW_ServerContinents
                            where c.SearchwarContinentId == db.SW_SearchWarCountryData.FirstOrDefault(cc => p.getCountry == cc.SearchWarCountryTLD.ToLower()).SW_SearchWarCountry.SW_SearchWarContinent.SearchWarContinentId
                            select new
                            {
                                serverName = db.SW_Server.SingleOrDefault(s => s.serverID == c.serverId).serverName
                            }).SingleOrDefault().serverName);


        public string GetAmazonServerName(string ipAddress)
        {
            string getCountry = LookUpService.GetCountry(ipAddress).ToLower();

            string getServerName = (string)HttpContext.Current.Cache[getCountry + "-serverName"];

            if (string.IsNullOrEmpty(getServerName))
            {

                using (Searchwar_netEntities db = new Searchwar_netEntities()) {

                    AmazonServerolumnNoPosition paramaters = new AmazonServerolumnNoPosition() {
                        getCountry = getCountry
                    };

                    getServerName = GetFastServerName.Invoke(db, paramaters).ToString();
                }

                    HttpContext.Current.Cache.Add(getCountry + "-serverName", getServerName, null, TimeZoneManager.DateTimeNow.AddDays(10),
                          System.Web.Caching.Cache.NoSlidingExpiration,
                          System.Web.Caching.CacheItemPriority.Normal,
                          null);

                
            }

            return getServerName;
        }
    }
}