using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Searchwar_netModel;
using SearchWar.LangSystem;

/// <summary>
/// Summary description for LangaugeSystem
/// </summary>

namespace SearchWar.LangSystem {
    public class LangaugeSystem
    {
	    public LangaugeSystem()
	    {
		    //
		    // TODO: Add constructor logic here
		    //
	    }

        private const string _currentUserIp = "62.107.21.129";

        public string CurrentUserCountry {
            get {
                const string CurrentUserIP = _currentUserIp;
                string CurrentUserCountry = LookUpService.GetCountry(CurrentUserIP);

                // Set values of current lang
                return CurrentUserCountry;
            }
        }

        public string CurrentLang
        {
            get
            {
                return System.Globalization.CultureInfo.CurrentCulture.Name;
            }
        }

        public int CurrentLangId
        {
            get
            {
                // Set values of current lang
                LangaugeSystem ls = new LangaugeSystem();
                return ls.GetLang(CurrentLang).LangId;
            }
        }

        public List<SW_Lang> GetLangs()
        {
            using (Searchwar_netEntities db = new Searchwar_netEntities())
            {

                List<SW_Lang> result = (from L in db.SW_Lang
                                        select L).ToList<SW_Lang>();




                return result;
            }
        }

        public SW_Lang GetLang(int langaugeId)
        {

            using (Searchwar_netEntities db = new Searchwar_netEntities())
            {

                SW_Lang Result = (from L in db.SW_Lang
                                  where L.LangId.Equals(langaugeId)
                                  select L).SingleOrDefault<SW_Lang>();




                return Result;
            }
        }

        public SW_Lang GetLang(string langShortName) {

            HttpContext c = HttpContext.Current;

            SW_Lang getLang = (SW_Lang)c.Cache["getlang-" + langShortName];

                if (getLang == null) {

                    Searchwar_netEntities db = new Searchwar_netEntities();

                    getLang = (from L in db.SW_Lang
                               where L.LangShortname.ToLower() == langShortName
                               select L).SingleOrDefault<SW_Lang>();

                    // add Cache
                    c.Cache.Add("getlang-" + langShortName, getLang,
                                null,
                                TimeZoneManager.DateTimeNow.AddMonths(1),
                                System.Web.Caching.Cache.NoSlidingExpiration,
                                System.Web.Caching.CacheItemPriority.Normal,
                                null);

                }

                
                

                return getLang;

        }
    }
}