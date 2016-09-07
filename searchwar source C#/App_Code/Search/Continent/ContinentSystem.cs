using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Searchwar_netModel;

/// <summary>
/// Summary description for ContinentSystem
/// </summary>
namespace SearchWar.ContinentSystem {
    public class ContinentSystem {
        public ContinentSystem() {
            //
            // TODO: Add constructor logic here
            //
        }

        /// <summary>
        /// Get one continent
        /// </summary>
        /// <param name="LangId">Insert langid</param>
        /// <param name="ContinentName">Insert ContinentId</param>
        /// <returns>anonymous object</returns>
        public static object GetContinent(int LangId, int ContinentId) {
            Searchwar_netEntities db = new Searchwar_netEntities();

                var r = (from C in db.SW_SearchWarContinent
                         join cd in db.SW_SearchWarContinentData
                             on C.SearchWarContinentId equals cd.SearchWarContinentId
                         where cd.LangId == LangId && C.SearchWarContinentId == ContinentId
                         orderby cd.SearchWarContinentName
                         select new
                                    {
                                        SearchWarContinentId = C.SearchWarContinentId,
                                        SearchWarContinentAddedDate = C.SearchWarContinentAddedDate,
                                        SearchWarContinentEditDate = C.SearchWarContinentEditDate,
                                        SearchWarContinentAddedUserId = C.SearchWarContinentAddedUserId,
                                        SearchWarContinentEditUserId = C.SearchWarContinentEditUserId,
                                        SearchWarContinentName = cd.SearchWarContinentName,
                                        LangId = cd.LangId
                                    }).SingleOrDefault();

                return r;

        }


        /// <summary>
        /// Get one continent
        /// </summary>
        /// <param name="LangId">Insert langid</param>
        /// <param name="continentName">Insert name of a continent</param>
        /// <returns>anonymous object</returns>
        public dynamic GetContinent(int LangId, string continentName) {


            continentName = HttpContext.Current.Server.HtmlDecode(continentName);

            using (Searchwar_netEntities db = new Searchwar_netEntities())
            {

                var r = (from C in db.SW_SearchWarContinent
                         join cd in db.SW_SearchWarContinentData
                             on C.SearchWarContinentId equals cd.SearchWarContinentId
                         where cd.LangId == LangId && cd.SearchWarContinentName == continentName
                         orderby cd.SearchWarContinentName
                         select new
                                    {
                                        SearchWarContinentId = C.SearchWarContinentId,
                                        SearchWarContinentAddedDate = C.SearchWarContinentAddedDate,
                                        SearchWarContinentEditDate = C.SearchWarContinentEditDate,
                                        SearchWarContinentAddedUserId = C.SearchWarContinentAddedUserId,
                                        SearchWarContinentEditUserId = C.SearchWarContinentEditUserId,
                                        SearchWarContinentName = cd.SearchWarContinentName,
                                        LangId = cd.LangId
                                    }).SingleOrDefault();

                return r.ConvertAnoToExpa();
            }

        }

        /// <summary>
        /// get all continents
        /// </summary>
        /// <param name="LangId">Insert LangId</param>
        /// <returns>List of anonymous objects</returns>
        public List<dynamic> GetContinents(int LangId) {

            using (Searchwar_netEntities db = new Searchwar_netEntities())
            {

                var r = (from C in db.SW_SearchWarContinent
                         join cd in db.SW_SearchWarContinentData
                             on C.SearchWarContinentId equals cd.SearchWarContinentId
                         where cd.LangId == LangId
                         orderby cd.SearchWarContinentName
                         select new
                                    {
                                        SearchWarContinentId = C.SearchWarContinentId,
                                        SearchWarContinentAddedDate = C.SearchWarContinentAddedDate,
                                        SearchWarContinentEditDate = C.SearchWarContinentEditDate,
                                        SearchWarContinentAddedUserId = C.SearchWarContinentAddedUserId,
                                        SearchWarContinentEditUserId = C.SearchWarContinentEditUserId,
                                        SearchWarContinentName = cd.SearchWarContinentName,
                                        LangId = cd.LangId
                                    });


                return r.ConvertListAnoToExpa();
            }

        }

        /// <summary>
        /// get all continent ids
        /// </summary>
        /// <param name="LangId">Insert LangId</param>
        /// <returns>List of anonymous objects</returns>
        public static List<object> GetContinentIds(int LangId) {

            Searchwar_netEntities db = new Searchwar_netEntities();

                var r = (from C in db.SW_SearchWarContinent
                         join cd in db.SW_SearchWarContinentData
                             on C.SearchWarContinentId equals cd.SearchWarContinentId
                         where cd.LangId == LangId
                         select new
                                    {
                                        SearchWarContinentId = C.SearchWarContinentId,
                                    }).Cast<object>().ToList<object>();

                return r;

        }

    }
}
