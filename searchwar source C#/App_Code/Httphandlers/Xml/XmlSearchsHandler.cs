using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using System.Xml;
using System.Web;
using System.IO;
using System.Text;
using SearchWar.SearchEngine;
using SearchWar.ObjectHelper;
using System.Globalization;
using System.Web.SessionState;
using System.Globalization;
using SearchWar.SearchEngine.Validate;
using System.Threading;
using SearchWar.LangSystem;

/// <summary>
/// Summary description for XmlSearchsHandler
/// </summary>
namespace SearchWar.SearchEngine {
    public class XmlSearchsHandler : IHttpHandler, IRequiresSessionState {
        public XmlSearchsHandler() {
            //
            // TODO: Add constructor logic here
            //
        }


        public bool IsReusable {
            get {
                return true;
            }
        }

        private string _currentUserIp;

        public void ProcessRequest(HttpContext context) {
            // Set defaults values
            string data = null;
            string currentUserIp = "62.107.21.129";
            HttpRequest currentR = context.Request;
            HttpServerUtility currentS = context.Server;
            int currentLangId;
            bool isValid = true;
            bool isUpdateInfoSucces = true;
            string errorMessage = "succes";

            _currentUserIp = currentUserIp;

            string sLangId = context.Request.QueryString["li"];
            if (string.IsNullOrEmpty(sLangId)) {
                isValid = false;
                errorMessage = "You forgot langid (querystring li)";
            }

            string userAction = context.Request.QueryString["a"];
            if (string.IsNullOrEmpty(userAction)) {
                isValid = false;
                errorMessage = "You forgot action (querystring a)";
            }

            // Convert langid querystring to int32
            if (!Int32.TryParse(sLangId, out currentLangId)) {
                isValid = false;
                errorMessage = "Problem with converting langauge ID (querystring li)";
            }

            

            #region Go search

            SearchEngine engine = new SearchEngine();

            // default option
            SearchEngine.UserSearchOption userOption = SearchEngine.UserSearchOption.UpdateUserSearch;


            #region Set default user data

            var otherData = new
            {
                UserID = (Guid)ANOProfile.GetCookieValues(currentUserIp, context).UserID,
                UserIpAddress = (string)currentUserIp
            };

            #endregion

            // Create user default information
            UserSearchInfo usi = new UserSearchInfo
                                   {
                                       UserID = otherData.UserID,
                                       UserIpAddress = otherData.UserIpAddress
                                   };

            // CREATE match search!
            if (userAction == "1") {

                #region Set ints data (Get data by QueryStrings)

                var dataConvertToInts = new
                {
                    clanSkillID = (string)currentR.QueryString["cs"],
                    clanContinentID = (string)currentR.QueryString["cct"],
                    clanCountryID = (string)currentR.QueryString["cc"],
                    searchContinentID = (string)currentR.QueryString["sct"],
                    searchCountryID = (string)currentR.QueryString["sc"],
                    searchGameID = (string)currentR.QueryString["sg"],
                    searchGameModeID = (string)currentR.QueryString["sgt"],
                    searchXvs = (string)currentR.QueryString["sxv"],
                    searchvsX = (string)currentR.QueryString["svx"]
                }.ToAnonymousObjectCollection();

                int MaxIntValue = int.MaxValue;
                var intdata = new
                {
                    clanSkillID = (int?)null,
                    clanContinentID = (int)MaxIntValue,
                    clanCountryID = (int)MaxIntValue,
                    searchContinentID = (int)MaxIntValue,
                    searchCountryID = (int?)null,
                    searchGameID = (int)MaxIntValue,
                    searchGameModeID = (int?)null,
                    searchXvs = (int?)null,
                    searchvsX = (int?)null,
                    searchYearTo = (int)MaxIntValue,
                    searchYearFrom = (int)MaxIntValue,
                    searchDayTo = (int)MaxIntValue,
                    searchDayFrom = (int)MaxIntValue,
                    searchMonthTo = (int)MaxIntValue,
                    searchMonthFrom = (int)MaxIntValue,
                    searchHourFrom = (int)MaxIntValue,
                    searchHourTo = (int)MaxIntValue,
                    searchMinutesFrom = (int)MaxIntValue,
                    searchMinutesTo = (int)MaxIntValue
                }.ToAnonymousObjectCollection();

                #endregion

                #region validate and convert properties to ints

                for (int i = 0; i < dataConvertToInts.Count; i++) {
                    AnonymousObject o = dataConvertToInts.GetAnonymousObject(i);

                    if (!string.IsNullOrEmpty(o.GetValue<string>())) {

                        int result;
                        if (int.TryParse(o.GetValue<string>(), out result)) {

                            intdata.GetAnonymousObject(o.KeyName).SetValue(result);

                        }

                    }

                    if (intdata.GetAnonymousObject(o.KeyName).GetValue_UnknownObject() != null
                        &&
                        Convert.ToInt32(intdata.GetAnonymousObject(o.KeyName).GetValue_UnknownObject()) == MaxIntValue) {
                        isValid = false;
                        errorMessage = "'" + o.KeyName +
                                       "' much be more than empty";
                    }
                }

                #endregion

                #region Set strings data (convert to HtmlEncode strings)

                var stringData = new
                {
                    ClanName = (string)currentS.HtmlEncode(currentS.UrlDecode(currentR.QueryString["cn"])),
                    SearchMap = (string)currentS.HtmlEncode(currentS.UrlDecode(currentR.QueryString["sm"])),
                };

                #endregion

                #region Set datetime data (Replace + and . (This chars is used to avoid problems))

                var datetimeData = new
                {
                    SearchMatchStart = (DateTime)DateTime.ParseExact(currentS.UrlDecode(currentR.QueryString["sfd"]), "dd-MM-yyyy HH:mm:ss", new DateTimeFormatInfo())

                };

                #endregion

                // Edit/Create user search information
                usi.ClanName = stringData.ClanName;
                usi.ClanSkillID = intdata.GetAnonymousObject("clanSkillID").GetValue<int?>();
                usi.ClanContinentID = intdata.GetAnonymousObject("clanContinentID").GetValue<int>();
                usi.ClanCountryID = intdata.GetAnonymousObject("clanCountryID").GetValue<int>();
                usi.SearchContinentID = intdata.GetAnonymousObject("searchContinentID").GetValue<int>();
                usi.SearchCountryID = intdata.GetAnonymousObject("searchCountryID").GetValue<int?>();
                usi.SearchGameID = intdata.GetAnonymousObject("searchGameID").GetValue<int>();
                usi.SearchGameModeID = intdata.GetAnonymousObject("searchGameModeID").GetValue<int?>();
                usi.SearchMap = stringData.SearchMap;
                usi.SearchXvs = intdata.GetAnonymousObject("searchXvs").GetValue<int>();
                usi.SearchvsX = intdata.GetAnonymousObject("searchvsX").GetValue<int>();
                usi.SearchMatchStart = new TimeZoneManager(currentUserIp).ConvertDateTimeToUtc(datetimeData.SearchMatchStart);

                userOption = SearchEngine.UserSearchOption.CreateUserSearch;
            }


            // Update user activity
            if (userAction == "2") {
                userOption = SearchEngine.UserSearchOption.UpdateUserSearch;

                // Get UserSearchInfo from database
                usi.SearchMatchID = ANOProfile.GetCookieValues(usi.UserIpAddress, context).SearchMatchID;
                isUpdateInfoSucces = engine.GetUserMatchInfo(usi);
            }

            if (userAction == "3") {
                userOption = SearchEngine.UserSearchOption.UpdateActivity;

                // Get UserSearchInfo from database
                usi.SearchMatchID = ANOProfile.GetCookieValues(usi.UserIpAddress, context).SearchMatchID;
                isUpdateInfoSucces = engine.GetUserMatchInfo(usi);

                if (isValid == true) {
                    errorMessage = "UpdatingUserActivity";
                }
            }


            // Validate DateTimes
            if (isUpdateInfoSucces == true) {

                    if (TimeNowValidate(usi.SearchMatchStart) == true) {

                        if (DateNowValidate(usi.SearchMatchStart) != true)
                        {
                                isValid = false;
                                errorMessage = "FromDateIsSmallerThanDateNow";
                        }

                    } else {
                        isValid = false;
                        errorMessage = "FromTimeIsSmallerThanTimeNow";
                    }

            } else {

                isValid = false;
                errorMessage = "UserSearchMatchIsNotOnline";

            }

            SearchEngine.SearchNoticeMessage searchNotice = SearchEngine.SearchNoticeMessage.Searching;
            const int defaultMaxResult = 10;
            const int defaultPageIndex = 0;
            const int defaultMaxSearchTimeSeconds = 1200;
            const int defaultMinUserActivitySeconds = 10;
            const int defaultFromLastSeconds = 10;
            List<SearchObject> searchResult = engine.UserSearchMatch(usi,
                                                            userOption,
                                                            defaultMaxResult,
                                                            defaultPageIndex,
                                                            currentLangId,
                                                            defaultMaxSearchTimeSeconds,
                                                            defaultMinUserActivitySeconds,
                                                            out searchNotice,
                                                            defaultFromLastSeconds,
                                                            context);
            

            #endregion
            LangaugeSystem ls = new LangaugeSystem();
            string getLang = ls.GetLang(Convert.ToInt32(sLangId)).LangShortname;
            Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture(getLang);
            Thread.CurrentThread.CurrentUICulture = new CultureInfo(getLang);
            

            // Create documet and first element called "ss" for "Searchs"
            XDocument createXmlSearchs = new XDocument(
                new XDeclaration("1.0", "iso-8859-1", "true"),
                new XElement("ssi"));

            // Get element "ssi" for "Searchs"
            XElement getSearchsElement = createXmlSearchs.Descendants("ssi").Single();

            // items element
            getSearchsElement.Add(new XElement("is"));

            // Get element "is" for "Items"
            XElement getItemsElement = getSearchsElement.Descendants("is").Single();


            // check the result is  0
            if (searchResult != null) {

                if (searchResult.Count() > 0) {

                    TimeZoneManager mngInfo = new TimeZoneManager(currentUserIp);
                    
                    // Insert/Create data as xml
                    for (int i = 0; i < searchResult.Count(); i++) {
                        var s = searchResult[i];

                        // Create element data
                        getItemsElement.Add(
                            new XElement("i",
                                         new XAttribute("id", s.SearchWarID.ToString()),
                                         new XElement("cn", s.ClanName),
                                         new XElement("ct", new XAttribute("i", s.ClanContinentData.SearchWarContinentId),
                                                      s.ClanContinentData.SearchWarContinentName),
                                         new XElement("cy", new XAttribute("i", s.ClanCountryData.SearchWarCountrytId),
                                                      new XAttribute("t", s.ClanCountryData.SearchWarCountryTLD),
                                                      s.ClanCountryData.SearchWarCountryName),
                                         new XElement("cs",
                                                      new XAttribute("i",
                                                                     s.ClanSkillData != null
                                                                         ? s.ClanSkillData.SearchWarSkillId.ToString()
                                                                         : ""),
                                                      s.ClanSkillData != null ? s.ClanSkillData.SearchWarSkillName : ""),
                                         new XElement("st", new XAttribute("i", s.SearchContinentData.SearchWarContinentId),
                                                      s.SearchContinentData.SearchWarContinentName),
                                         new XElement("sy",
                                                      new XAttribute("i",
                                                                     s.SearchCountryData != null
                                                                         ? s.SearchCountryData.SearchWarCountrytId.ToString()
                                                                         : ""),
                                                      new XAttribute("t",
                                                                     s.SearchCountryData != null
                                                                         ? s.SearchCountryData.SearchWarCountryTLD
                                                                         : ""),
                                                      s.SearchCountryData != null
                                                          ? s.SearchCountryData.SearchWarCountryName
                                                          : ""),
                                         new XElement("ss",
                                                      new XAttribute("i",
                                                                     s.SearchSkillData != null
                                                                         ? s.SearchSkillData.SearchWarSkillId.ToString()
                                                                         : ""),
                                                      s.SearchSkillData != null ? s.SearchSkillData.SearchWarSkillName : ""),
                                         new XElement("g", new XAttribute("i", s.SearchGame.SearchWarGameId),
                                                      s.SearchGame.SearchWarGameName),
                                         new XElement("gt",
                                                      new XAttribute("i",
                                                                     s.SearchGameType != null
                                                                         ? s.SearchGameType.SearchWarGameTypeId.ToString()
                                                                         : ""),
                                                      s.SearchGameType != null ? s.SearchGameType.SearchWarGameTypeName : ""),
                                         new XElement("fd", mngInfo.ConvertDateTimeFromUtc(s.SearchMatchStart).ToString()),
                                         new XElement("x", s.SearchXvs),
                                         new XElement("y", s.SearchvsX),
                                         new XElement("m", s.SearchMap)));
                    }
                } else {
                    if (isValid == true) {
                        isValid = false;
                        errorMessage = "NoResults";


                        if (userAction == "3") {
                            isValid = true;
                            errorMessage = "UpdatingUserActivity";
                        }
                    }

                }


            } else {
                if (isValid == true) {
                    isValid = false;
                    errorMessage = "NoResults";


                    if (userAction == "3") {
                        isValid = true;
                        errorMessage = "UpdatingUserActivity";
                    }
                }
            }

            // write status for result
            getSearchsElement.Add(new XElement("status", new XAttribute("bool", isValid.ToString()), errorMessage));

            // Write/save data
            StringWriter sw = new StringWriter();
            XmlWriter w = XmlWriter.Create(sw);
            createXmlSearchs.Save(w);

            w.Close();

            data = sw.ToString();
            sw.Close();

            context.Response.ContentType = "text/xml";
            context.Response.ContentEncoding = Encoding.UTF8;
            context.Response.Write(XmlWhiteSpaceModule.RemoveWhitespace(data));

            // release it
            context.Response.Flush();

            engine.CleanUpAndClose();

        }

        private readonly SearchValidate _validator = new SearchValidate();


        protected bool TimeNowValidate(DateTime fromDate) {
            bool valid = true;

            SearchValidate.DateValidateMsg msg =
                _validator.ValidateDates(fromDate, true, _currentUserIp);

            if (msg == SearchValidate.DateValidateMsg.FromTimeIsSmallerThanDateTimeNow) {
                valid = false;
            }

            return valid;
        }

        protected bool DateNowValidate(DateTime fromDate) {
            bool valid = true;

            SearchValidate.DateValidateMsg msg =
                _validator.ValidateDates(
                    fromDate, false, _currentUserIp);

            if (msg == SearchValidate.DateValidateMsg.FromDateIsSmallerThanDateTimeNow) {
                valid = false;
            }

            return valid;
        }

    }
}