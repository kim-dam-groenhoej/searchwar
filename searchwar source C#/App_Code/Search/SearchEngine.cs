using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Data.Linq;
using System.Threading;
using System.Web;
using SearchWar.ObjectHelper;
using System.Activities.Statements;
using Searchwar_netModel;
using System.Data.Objects;
using System.Data.Objects.SqlClient;

/// <summary>
/// Summary description for SearchEngine
/// </summary>
namespace SearchWar.SearchEngine {

    public class SearchEngine {
        public SearchEngine() {

            db = new Searchwar_netEntities();
            db.Connection.Open();
            db.ExecuteStoreCommand("SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED;");

        }

        public void CleanUpAndClose()
        {
            db.Connection.Dispose();
            db.Connection.Close();
            db.Dispose();

        }

        private Searchwar_netEntities db;
        private TransactionScope _transaction;
        private static List<Guid> _listOfNoSearching_MatchIds;


        #region UserSearch

        public enum UserSearchOption
        {
            CreateUserSearch = 1,
            UpdateUserSearch = 2,
            UpdateActivity = 3
        }

        public enum SearchNoticeMessage {
            NoResults,
            Searching,
            NoConnection,
            UnknownError
        }

        // check max idle time
        private SearchNoticeMessage CheckUserIdleActivity(UserSearchInfo usi, 
            int maxIdleSearch_Seconds, 
            HttpContext context, 
            SearchNoticeMessage noticemessage)
        {
            Guid? searchID = ANOProfile.GetCookieValues(usi.UserIpAddress, context).SearchMatchID;

                SW_SearchWar obj =
                    db.SW_SearchWar.SingleOrDefault<SW_SearchWar>(s => s.SearchWarUserID == usi.UserID && (s.SearchWarId == searchID));

                DateTime getAddedDateTime;
                if (obj != null)
                {
                    getAddedDateTime = obj.SearchWarAddedDate;
                } else
                {
                    return SearchNoticeMessage.NoResults;
                }

                return getAddedDateTime.AddSeconds(maxIdleSearch_Seconds) > TimeZoneManager.DateTimeNow ? noticemessage : SearchNoticeMessage.NoResults;
        }

        #region get user search info (CRAZY shit)

        private struct GetUserMatchInfoColumnNoPosition {
            public Guid SearchWarUserId;
            public Guid SearchWarId;
        }

        private static readonly Func<Searchwar_netEntities, GetUserMatchInfoColumnNoPosition, SW_SearchWar>
            GetUserSearchInfoFast = System.Data.Objects.CompiledQuery.Compile<Searchwar_netEntities, GetUserMatchInfoColumnNoPosition, SW_SearchWar>
                                          ((db, p) => (from s in db.SW_SearchWar
                                                       where s.SearchWarUserID == p.SearchWarUserId && 
                                                       s.SearchWarId == p.SearchWarId &&
                                                       s.SearchWarIsRunning == true
                                                       select s).SingleOrDefault<SW_SearchWar>());


        public Boolean GetUserMatchInfo(UserSearchInfo usi)
        {
                SW_SearchWar getSearch = null;
                if (usi.SearchMatchID.HasValue && usi.UserID != null)
                {
                    GetUserMatchInfoColumnNoPosition parameters = new GetUserMatchInfoColumnNoPosition
                                                                      {
                                                                          SearchWarId = usi.SearchMatchID.Value,
                                                                          SearchWarUserId = usi.UserID
                                                                      };

                    getSearch = GetUserSearchInfoFast(db, parameters);
                }

                if (getSearch != null)
                {
                    usi.SearchGameID = getSearch.SearchWarSearchGameId;
                    usi.SearchMatchStart = getSearch.SearchWarSearchMatchStart;
                    usi.ClanSkillID = getSearch.SearchWarClanSkillId;
                    usi.ClanName = getSearch.SearchWarClanName;
                    if (getSearch.SearchWarClanSkillId.HasValue)
                    {
                        usi.ClanCountryID = getSearch.SearchWarClanCountryId.Value;
                    }
                    usi.ClanContinentID = getSearch.SearchWarClanContinentId.Value;
                    if (getSearch.SearchWarSearchContinentId.HasValue)
                    {
                        usi.SearchContinentID = getSearch.SearchWarSearchContinentId.Value;
                    }
                    usi.SearchCountryID = getSearch.SearchWarSearchCountryId;
                    usi.SearchGameModeID = getSearch.SearchWarSearchGameTypeId;
                    usi.SearchMap = getSearch.SearchWarSearchMap;
                    usi.SearchSkillID = getSearch.SearchWarSearchSkillId;
                    usi.SearchvsX = getSearch.SearchWarSearchvsX;
                    usi.SearchXvs = getSearch.SearchWarSearchXvs;

                    return true;

                } else
                {
                    return false;
                }

        }
        #endregion

        // updating user activity
        private SearchNoticeMessage UpdateUserActivity(UserSearchInfo usi,
            int userActivityMinSeconds,
            HttpContext context,
            SearchNoticeMessage noticemessage)
        {
            DateTime DateTimeNow = TimeZoneManager.DateTimeNow;

            Guid? searchId = ANOProfile.GetCookieValues(usi.UserIpAddress, context).SearchMatchID;

                SW_SearchWar getSearch =
                    db.SW_SearchWar.SingleOrDefault<SW_SearchWar>(
                        s =>
                        s.SearchWarUserID == usi.UserID &&
                        (s.SearchWarId == searchId ||
                         s.SearchWarId == usi.SearchMatchID));
                
                if (getSearch != null)
                {
                    if (getSearch.SearchWarEditDate.AddSeconds(userActivityMinSeconds) > DateTimeNow)
                    {
                        getSearch.SearchWarEditDate = DateTimeNow.AddSeconds(5);
                        db.SaveChanges();

                        return noticemessage;
                    }
                }

                return SearchNoticeMessage.NoConnection;
        }

        private HttpContext _context;

        /// <summary>
        /// Searching and matching other matchs/clanwars
        /// </summary>
        /// <param name="userSearchInfo">insert UserSearchInfo object</param>
        /// <param name="option">are you starting searching OR just update search results?</param>
        /// <param name="maxResults">max results you want</param>
        /// <param name="pageIndex">page index (fx. what page you are on)</param>
        /// <param name="langId">insert langid for langauge</param>
        /// <param name="minNoUserActivityTimeSeconds">If there is no activity for the user (fx. internet connection lost)</param>
        /// <param name="noticemessage">notice if user still can search OR errors</param>
        /// <param name="fromLastSeconds">Write the number of seconds you want to get all "Searchs" from</param>
        /// <param name="context">current httpcontext</param>
        /// <param name="maxIdleTimeSeconds">How long the user can max search in time</param>
        /// <returns>anynmous object</returns>
        public List<SearchObject> UserSearchMatch(UserSearchInfo userSearchInfo, 
            UserSearchOption option, 
            int maxResults, 
            int pageIndex, 
            int langId,
            int maxIdleTimeSeconds,
            int minNoUserActivityTimeSeconds,
            out SearchNoticeMessage noticemessage, 
            int fromLastSeconds, 
            HttpContext context) {

            _context = context;

            noticemessage = SearchNoticeMessage.Searching; // default status value

            // Create search
            if (option == UserSearchOption.CreateUserSearch)
            {
                InsertusiResult result = CreateUserSearch(userSearchInfo);
                if (result == InsertusiResult.Error)
                {
                    noticemessage = SearchNoticeMessage.UnknownError;
                }
            }

            

            // Check max idle/searching time
            if (option == UserSearchOption.UpdateUserSearch)
            {

                // Update user activity
                noticemessage = UpdateUserActivity(userSearchInfo, 
                    minNoUserActivityTimeSeconds, 
                    context,
                    noticemessage);

                noticemessage = CheckUserIdleActivity(userSearchInfo,
                                                      maxIdleTimeSeconds,
                                                      context,
                                                      noticemessage);

            }

            if (option == UserSearchOption.UpdateActivity)
            {

                // Update user activity
                noticemessage = UpdateUserActivity(userSearchInfo,
                    minNoUserActivityTimeSeconds,
                    context,
                    noticemessage);

                noticemessage = CheckUserIdleActivity(userSearchInfo,
                                                      maxIdleTimeSeconds,
                                                      context, 
                                                      noticemessage);

                return null;

            }

            // SEARCH NOW!!!!
            var itemResult = noticemessage == SearchNoticeMessage.Searching
                             ? SearchMatchs(userSearchInfo,
                                            maxResults,
                                            pageIndex,
                                            langId,
                                            fromLastSeconds)
                             : null;


            // no result change noticemsg
            if (itemResult == null)
            {
                noticemessage = SearchNoticeMessage.NoResults;
            }


            // return search/match results
            return itemResult;
            
        }
        #endregion
        
        #region newest user match searchs (CRAZY shit)
        /// <summary>
        /// Get newest user match searchs
        /// </summary>
        /// <param name="maxResults"></param>
        /// <param name="langId"></param>
        /// <returns>return list of NewestSearchObject</returns>
        public static List<NewestSearchObject> GetNewestMatchSearchs(int maxResults, 
            int langId, 
            int fromLastSeconds)
        {
            
            

            Searchwar_netEntities db = new Searchwar_netEntities();

            NewestSearchColumnNoPosition parameters = new NewestSearchColumnNoPosition
                {
                    LangId = langId,
                    PageLimit = maxResults,
                    FromLastSeconds = fromLastSeconds,
                    DateTimeNow = TimeZoneManager.DateTimeNow
                };

            
            

                return GetNewestSearchResult(db, parameters).ToList<NewestSearchObject>();
            
        }


        private struct NewestSearchColumnNoPosition {
            public int LangId;
            public int PageLimit;
            public int FromLastSeconds;
            public DateTime DateTimeNow;
        }

        private static readonly Func<Searchwar_netEntities, NewestSearchColumnNoPosition, IQueryable<NewestSearchObject>>
            GetNewestSearchResult = System.Data.Objects.CompiledQuery.Compile<Searchwar_netEntities, NewestSearchColumnNoPosition, IQueryable<NewestSearchObject>>
                                          ((db, p) => (from s in db.SW_SearchWar
                                                       join d in db.SW_SearchWarContinentData on s.SearchWarClanContinentId equals d.SearchWarContinentId
                                                       join l in db.SW_SearchWarCountryData on s.SearchWarClanCountryId equals l.SearchWarCountrytId
                                                       where d.LangId == p.LangId && l.LangId == p.LangId &&
                                                       EntityFunctions.AddSeconds(s.SearchWarEditDate,p.FromLastSeconds) > p.DateTimeNow && s.SearchWarIsRunning == true
                                                       orderby
                                                           s.SearchWarAddedDate descending 
                                                       select new NewestSearchObject
                                                       {
                                                                            SearchWarID = s.SearchWarId,
                                                                            ClanName = s.SearchWarClanName,
                                                                            ClanContinentData = d,
                                                                            ClanCountryData = l,
                                                                            SearchGame = s.SW_SearchWarGame,
                                                                            SearchGameType = s.SearchWarSearchGameTypeId != null ? s.SW_SearchWarGameType : null,
                                                                            SearchXvs = s.SearchWarSearchXvs,
                                                                            SearchvsX = s.SearchWarSearchvsX,
                                                                            SearchMatchStart = s.SearchWarSearchMatchStart
                                                       }).Take(p.PageLimit));

        #endregion
        
        #region Insert UserSearch information in database

            #region Result Enum
            private enum InsertusiResult
            {
                Done = 1,
                Error = 2
            }
            #endregion

        private InsertusiResult CreateUserSearch(UserSearchInfo usi)
        {
            TimeZoneManager mngInfo = new TimeZoneManager(usi.UserIpAddress);
            DateTime nowDatetime = TimeZoneManager.DateTimeNow;
                

                    List<SW_SearchWar> getUserMatchs = (from s in db.SW_SearchWar
                                                       where s.SearchWarUserID == usi.UserID &&
                                                       s.SearchWarIsRunning == true
                                                       select s).ToList();

                    foreach (SW_SearchWar s in getUserMatchs)
                    {
                        s.SearchWarIsRunning = false;
                    }

                    SW_SearchWar createusi = new SW_SearchWar
                                                 {
                                                     SearchWarId = Guid.NewGuid(),
                                                     SearchWarClanName = usi.ClanName,
                                                     SearchWarClanContinentId = usi.ClanContinentID,
                                                     SearchWarClanCountryId = usi.ClanCountryID,
                                                     SearchWarClanSkillId = usi.SearchSkillID,
                                                     SearchWarSearchGameId = usi.SearchGameID,
                                                     SearchWarSearchGameTypeId = usi.SearchGameModeID,
                                                     SearchWarSearchContinentId = usi.SearchContinentID,
                                                     SearchWarSearchCountryId = usi.SearchCountryID,
                                                     SearchWarSearchMap = usi.SearchMap,
                                                     SearchWarIsRunning = true,
                                                     SearchWarSearchSkillId = usi.SearchSkillID,
                                                     SearchWarSearchvsX = usi.SearchvsX,
                                                     SearchWarSearchXvs = usi.SearchXvs,
                                                     SearchWarSearchMatchStart = usi.SearchMatchStart,
                                                     SearchWarUserIpAddress = usi.UserIpAddress,
                                                     SearchWarUserID = usi.UserID,
                                                     SearchWarAddedDate = nowDatetime,
                                                     SearchWarEditDate = nowDatetime
                                                 };

                    db.SW_SearchWar.AddObject(createusi);
                    db.SaveChanges();

                    usi.SearchMatchID = createusi.SearchWarId;
                    _context.Session["usi"] = usi;

                    return InsertusiResult.Done;

        }

        #endregion

        #region search function (CRAZY shit)
        /// Please read this links before reading code:
        /// http://www.jdconley.com/blog/archive/2007/11/28/linq-to-sql-surprise-performance-hit.aspx
        /// http://blog.linqexchange.com/index.php/how-to-use-compiled-queries-in-linq-to-sql-for-high-demand-asp-net-websites/

        /// <summary>
        /// Main search function
        /// </summary>
        /// <param name="usi">Insert UserSearchInfo object</param>
        /// <param name="pagelimit">Insert resultlimit</param>
        /// <param name="pageindex">Insert index</param>
        /// <param name="langId">Insert langid for what langauge you want the result in</param>
        /// <param name="fromLastSeconds"></param>
        /// <returns>return SearchObject object</returns>
        private List<SearchObject> SearchMatchs(UserSearchInfo usi, 
            int pagelimit, 
            int pageindex, 
            int langId,
            int fromLastSeconds)
        {

            Searchwar_netEntities db = new Searchwar_netEntities();

            List<Guid> lastIds = new List<Guid>();
            if (_context.Session["searchresult"] != null) {
                lastIds = (List<Guid>)_context.Session["searchresult"];
            }

            SearchColumns parameters = new SearchColumns
                                                    {
                                                        Usi = usi,
                                                        LangId = langId,
                                                        PageIndex = pageindex,
                                                        PageLimit = pagelimit,
                                                        FromLastSeconds = fromLastSeconds,
                                                        DateTimeNow = TimeZoneManager.DateTimeNow,
                                                        LastIds = lastIds
                                                    };

            List<SearchObject> matchsFound = SearchResults(db, parameters).Skip(parameters.PageIndex == 0 ? (parameters.PageIndex * parameters.PageLimit) : 0).Take(
                                             parameters.PageLimit).ToList<SearchObject>();

            return matchsFound;

        }

        private struct SearchColumns
        {
            public int LangId;
            public UserSearchInfo Usi;
            public int PageLimit;
            public int PageIndex;
            public int FromLastSeconds;
            public DateTime DateTimeNow;
            public IEnumerable<Guid> LastIds;
        }

        private static readonly Func<Searchwar_netEntities, SearchColumns, IQueryable<SearchObject>>
            SearchResults = System.Data.Objects.CompiledQuery.Compile<Searchwar_netEntities, SearchColumns, IQueryable<SearchObject>>
                                          ((db, p) => (from s in db.SW_SearchWar.Include("SW_SearchWarContinent").Include("SW_SearchWarCountry").Include("SW_SearchWarGame").Include("SW_SearchWarSkill")
                                                       join cCt in db.SW_SearchWarContinentData on s.SearchWarClanContinentId equals cCt.SearchWarContinentId
                                                       join cCy in db.SW_SearchWarCountryData on s.SearchWarClanCountryId equals cCy.SearchWarCountrytId
                                          where
                                              ((s.SearchWarSearchMatchStart >= p.DateTimeNow) &&
                                              s.SearchWarSearchGameId == p.Usi.SearchGameID &&
                                              s.SearchWarSearchContinentId == p.Usi.SearchContinentID &&
                                              EntityFunctions.AddSeconds(s.SearchWarEditDate, p.FromLastSeconds) > p.DateTimeNow) &&
                                              cCt.LangId == p.LangId && cCy.LangId == p.LangId &&
                                              s.SearchWarUserID != p.Usi.UserID && s.SearchWarIsRunning == true && p.LastIds.Contains(s.SearchWarId) == false
                                          orderby s.SearchWarAddedDate descending
                                          ,
                                              s.SW_SearchWarCountry.SearchWarContinentId == p.Usi.SearchContinentID descending 
                                          ,
                                              s.SW_SearchWarCountry1.SearchWarContinentId == p.Usi.SearchContinentID descending 
                                          ,
                                              s.SearchWarSearchCountryId == p.Usi.SearchCountryID descending 
                                          ,
                                              s.SearchWarSearchCountryId == p.Usi.ClanCountryID descending 
                                          ,
                                              s.SearchWarSearchMap.ToLower() == p.Usi.SearchMap.ToLower() descending 
                                          ,
                                              s.SW_SearchWarSkill.SearchWarSkillSort ==
                                              db.SW_SearchWarSkill.FirstOrDefault(S => S.SearchWarSkillId == p.Usi.ClanSkillID)
                                                  .SearchWarSkillSort descending 
                                          ,
                                              s.SearchWarSearchXvs == p.Usi.SearchXvs descending 
                                          ,
                                              s.SearchWarSearchvsX == p.Usi.SearchvsX descending 
                                          select new SearchObject
                                                     {
                                                         ClanName = s.SearchWarClanName,
                                                         ClanContinentData = cCt,
                                                         ClanCountryData = cCy,
                                                         ClanSkillData = s.SW_SearchWarSkill.SW_SearchWarSkillData.Where(S => S.SearchWarSkillId == s.SearchWarClanSkillId).FirstOrDefault(),
                                                         SearchSkillData = s.SW_SearchWarSkill1.SW_SearchWarSkillData.Where(S => S.SearchWarSkillId == s.SearchWarSearchSkillId).FirstOrDefault(),
                                                         SearchGame = s.SW_SearchWarGame,
                                                         SearchGameType = s.SW_SearchWarGameType,
                                                         SearchMap = s.SearchWarSearchMap,
                                                         SearchXvs = s.SearchWarSearchXvs,
                                                         SearchvsX = s.SearchWarSearchvsX,
                                                         SearchContinentData = s.SW_SearchWarContinent1.SW_SearchWarContinentData.Where(C => C.SearchWarContinentId == s.SearchWarSearchContinentId && C.LangId == p.LangId).FirstOrDefault(),
                                                         SearchCountryData = s.SW_SearchWarCountry1.SW_SearchWarCountryData.Where(C => C.SearchWarCountrytId == s.SearchWarSearchCountryId && C.LangId == p.LangId).FirstOrDefault(),
                                                         SearchMatchStart = s.SearchWarSearchMatchStart,
                                                         SearchAddedDate = s.SearchWarAddedDate,
                                                         SearchEditDate = s.SearchWarEditDate,
                                                         UserID = s.SearchWarUserID,
                                                         SearchWarID = s.SearchWarId
                                                     }));

        #endregion
    }
}
