using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Searchwar_netModel;

/// <summary>
/// Summary description for CountrySystem
/// </summary>
namespace SearchWar.ContinentSystem {
    public class CountrySystem {
        public CountrySystem() {
            //
            // TODO: Add constructor logic here
            //
        }

        /// <summary>
        /// Get country
        /// </summary>
        /// <param name="langId">Insert LangId</param>
        /// <param name="countryId">Insert Countryid</param>
        /// <returns>Get anonymous object</returns>
        public static object GetCountry(int langId, int countryId) {

            Searchwar_netEntities db = new Searchwar_netEntities();

                var r = (from C in db.SW_SearchWarCountry
                         join cd in db.SW_SearchWarCountryData
                             on C.SearchWarCountryId equals cd.SearchWarCountrytId
                         where langId == cd.LangId && C.SearchWarCountryId == countryId
                         select new
                                    {
                                        SearchWarCountryId = C.SearchWarCountryId,
                                        SearchWarCountryAddedDate = C.SearchWarCountryAddedDate,
                                        SearchWarCountryEditdate = C.SearchWarCountryEditDate,
                                        SearchWarCountryAddedUserId = C.SearchWarCountryAddedUserId,
                                        SearchWarCountryEditUserId = C.SearchWarCountryEditUserId,
                                        SearchWarContinentId = C.SearchWarContinentId,
                                        SearchWarCountryName = cd.SearchWarCountryName,
                                        SearchWarCountryTLD = cd.SearchWarCountryTLD
                                    }).SingleOrDefault();


                return r;

        }

        /// <summary>
        /// Get country
        /// </summary>
        /// <param name="langId">Insert LangId</param>
        /// <param name="countryName">Insert CountryName</param>
        /// <returns>Get anonymous object</returns>
        public dynamic GetCountry(int langId, string countryName) {

            countryName = HttpContext.Current.Server.HtmlDecode(countryName).ToLower();

            using (Searchwar_netEntities db = new Searchwar_netEntities())
            {

                var r = (from C in db.SW_SearchWarCountry
                         join cd in db.SW_SearchWarCountryData
                             on C.SearchWarCountryId equals cd.SearchWarCountrytId
                         where langId == cd.LangId && cd.SearchWarCountryName.ToLower() == countryName
                         select new
                                    {
                                        SearchWarCountryId = C.SearchWarCountryId,
                                        SearchWarCountryAddedDate = C.SearchWarCountryAddedDate,
                                        SearchWarCountryEditdate = C.SearchWarCountryEditDate,
                                        SearchWarCountryAddedUserId = C.SearchWarCountryAddedUserId,
                                        SearchWarCountryEditUserId = C.SearchWarCountryEditUserId,
                                        SearchWarContinentId = C.SearchWarContinentId,
                                        SearchWarCountryName = cd.SearchWarCountryName,
                                        SearchWarCountryTLD = cd.SearchWarCountryTLD
                                    }).SingleOrDefault();

                return r.ConvertAnoToExpa();
            }
        }

        /// <summary>
        /// Get all countries
        /// </summary>
        /// <param name="LangId">Insert Langid</param>
        /// <returns>List of anonymous objects</returns>
        public List<dynamic> GetCountries(int LangId) {


            using (Searchwar_netEntities db = new Searchwar_netEntities())
            {

                var r = (from C in db.SW_SearchWarCountry
                         join cd in db.SW_SearchWarCountryData
                             on C.SearchWarCountryId equals cd.SearchWarCountrytId
                         where LangId == cd.LangId
                         orderby cd.SearchWarCountryName
                         select new
                                    {
                                        SearchWarCountryId = C.SearchWarCountryId,
                                        SearchWarCountryAddedDate = C.SearchWarCountryAddedDate,
                                        SearchWarCountryEditdate = C.SearchWarCountryEditDate,
                                        SearchWarCountryAddedUserId = C.SearchWarCountryAddedUserId,
                                        SearchWarCountryEditUserId = C.SearchWarCountryEditUserId,
                                        SearchWarContinentId = C.SearchWarContinentId,
                                        SearchWarCountryName = cd.SearchWarCountryName,
                                        SearchWarCountryTLD = cd.SearchWarCountryTLD
                                    });

                return r.ConvertListAnoToExpa();
            }
        }

        /// <summary>
        /// Get all country ids
        /// </summary>
        /// <param name="LangId">Insert Langid</param>
        /// <returns>List of anonymous objects</returns>
        public List<dynamic> GetCountryIds(int LangId) {

            using (Searchwar_netEntities db = new Searchwar_netEntities())
            {

                var r = (from C in db.SW_SearchWarCountry
                         join cd in db.SW_SearchWarCountryData
                             on C.SearchWarCountryId equals cd.SearchWarCountrytId
                         where LangId == cd.LangId
                         select new
                                    {
                                        SearchWarCountryId = C.SearchWarCountryId,
                                    });

                return r.ConvertListAnoToExpa();
            }
        }

        /// <summary>
        /// Get all countries by ContinentId
        /// </summary>
        /// <param name="LangId">Insert Langid</param>
        /// <param name="ContinentId">Insert ContinentId</param>
        /// <returns>List of anonymous objects</returns>
        public List<dynamic> GetCountries(int LangId, int ContinentId) {

            using (Searchwar_netEntities db = new Searchwar_netEntities())
            {

                var r = (from C in db.SW_SearchWarCountry
                         join cd in db.SW_SearchWarCountryData
                             on C.SearchWarCountryId equals cd.SearchWarCountrytId
                         where LangId == cd.LangId && C.SearchWarContinentId == ContinentId
                         orderby cd.SearchWarCountryName
                         select new
                                    {
                                        SearchWarCountryId = C.SearchWarCountryId,
                                        SearchWarCountryAddedDate = C.SearchWarCountryAddedDate,
                                        SearchWarCountryEditdate = C.SearchWarCountryEditDate,
                                        SearchWarCountryAddedUserId = C.SearchWarCountryAddedUserId,
                                        SearchWarCountryEditUserId = C.SearchWarCountryEditUserId,
                                        SearchWarContinentId = C.SearchWarContinentId,
                                        SearchWarCountryName = cd.SearchWarCountryName,
                                        SearchWarCountryTLD = cd.SearchWarCountryTLD
                                    });

                return r.ConvertListAnoToExpa();
            }
        }
    }
}
