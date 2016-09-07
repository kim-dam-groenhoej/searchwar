using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SearchWar.SearchEngine;
using SearchWar.ObjectHelper;
using SearchWar.SearchEngine.Validate;
using System.Xml.Linq;
using System.Threading;
using SearchWar.LangSystem;
using System.Globalization;

/// <summary>
/// Summary description for SearchEngineMng
/// </summary>
public class SearchEngineMng
{
    private UserSearchInfo usi = new UserSearchInfo();
    private HttpRequest currentR;
    private HttpServerUtility currentS;
    private HttpContext context;
    private int currentLangId;
    private string errorMessage = "";
    public bool isValid = true;

    private SearchEngine engine = new SearchEngine();
    private readonly SearchValidate _validator = new SearchValidate();
    private string _currentUserIp;
    private SearchWar.SearchEngine.SearchEngine.UserSearchOption userOption;
    private TimeZoneManager mngInfo;

	public SearchEngineMng(string currentUserIp, 
                           HttpContext context,
                           string action)
	{
        _currentUserIp = currentUserIp;
        this.context = context;
        currentR = context.Request;
        currentS = context.Server;
        mngInfo = new TimeZoneManager(currentUserIp);

        string sLangId = context.Request.QueryString["li"];
        if (string.IsNullOrEmpty(sLangId))
        {
            isValid = false;
            errorMessage = "You forgot langid (querystring li)";
        }

        // Convert langid querystring to int32
        if (!Int32.TryParse(sLangId, out currentLangId))
        {
            isValid = false;
            errorMessage = "Problem with converting langauge ID (querystring li)";
        }

        if (context.Session["usi"] == null || action == "1")
        {

            // Create user default information
            usi.UserID = (Guid)ANOProfile.GetCookieValues(currentUserIp, context).UserID;
            usi.UserIpAddress = currentUserIp;

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
                searchYearFrom = (int)MaxIntValue,
                searchDayFrom = (int)MaxIntValue,
                searchMonthFrom = (int)MaxIntValue,
                searchHourFrom = (int)MaxIntValue,
                searchMinutesFrom = (int)MaxIntValue,
            }.ToAnonymousObjectCollection();

            #endregion

            #region validate and convert properties to ints

            for (int i = 0; i < dataConvertToInts.Count; i++)
            {
                AnonymousObject o = dataConvertToInts.GetAnonymousObject(i);

                if (!string.IsNullOrEmpty(o.GetValue<string>()))
                {

                    int result;
                    if (int.TryParse(o.GetValue<string>(), out result))
                    {

                        intdata.GetAnonymousObject(o.KeyName).SetValue(result);

                    }

                }

                if (intdata.GetAnonymousObject(o.KeyName).GetValue_UnknownObject() != null
                    &&
                    Convert.ToInt32(intdata.GetAnonymousObject(o.KeyName).GetValue_UnknownObject()) == MaxIntValue)
                {
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

            if (string.IsNullOrEmpty(currentR.QueryString["sfd"]))
            {
                isValid = false;
                errorMessage = "'SearchMatchStart' much be more than empty";
            }
            else
            {
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

                userOption = SearchWar.SearchEngine.SearchEngine.UserSearchOption.CreateUserSearch;
                context.Session["usi"] = usi;
            }
        }
        else
        {
            usi = (UserSearchInfo)context.Session["usi"];
        }

        if (isValid == true) {
            LangaugeSystem ls = new LangaugeSystem();
            string getLang = ls.GetLang(Convert.ToInt32(sLangId)).LangShortname;
            Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture(getLang);
            Thread.CurrentThread.CurrentUICulture = new CultureInfo(getLang);
        }

	}




    public ManagerResponseObj SearchNow()
    {
        bool isUpdateInfoSucces = true;
        List<SearchObject> searchResult = new List<SearchObject>();

        // default option
        if (userOption != SearchWar.SearchEngine.SearchEngine.UserSearchOption.CreateUserSearch) {
            userOption = SearchWar.SearchEngine.SearchEngine.UserSearchOption.UpdateUserSearch;
        }

        if (isValid == true)
        {
            // Update user activity
            if (userOption == SearchWar.SearchEngine.SearchEngine.UserSearchOption.UpdateUserSearch)
            {
                isUpdateInfoSucces = engine.GetUserMatchInfo(usi);
            }


            // Validate DateTimes
            if (isUpdateInfoSucces == true)
            {

                if (TimeNowValidate(usi.SearchMatchStart) == true)
                {

                    if (DateNowValidate(usi.SearchMatchStart) != true)
                    {
                        isValid = false;
                        errorMessage = "FromDateIsSmallerThanDateNow";
                    }

                }
                else
                {
                    isValid = false;
                    errorMessage = "FromTimeIsSmallerThanTimeNow";
                }

            }
            else
            {

                isValid = false;
                errorMessage = "UserSearchMatchIsNotOnline";

            }


            if (isValid == true)
            {
                SearchEngine.SearchNoticeMessage searchNotice = SearchWar.SearchEngine.SearchEngine.SearchNoticeMessage.Searching;
                const int defaultMaxResult = 10;
                const int defaultPageIndex = 0;
                const int defaultMaxSearchTimeSeconds = 1200;
                const int defaultMinUserActivitySeconds = 10;
                const int defaultFromLastSeconds = 10;
                searchResult = engine.UserSearchMatch(usi,
                                                                userOption,
                                                                defaultMaxResult,
                                                                defaultPageIndex,
                                                                currentLangId,
                                                                defaultMaxSearchTimeSeconds,
                                                                defaultMinUserActivitySeconds,
                                                                out searchNotice,
                                                                defaultFromLastSeconds,
                                                                context);
            }
        }

        // create search root
        XElement getSearchsElement = new XElement("search");

        ManagerResponseObj mro = new ManagerResponseObj();
        mro.DataObject = searchResult;
        mro.Xml = getSearchsElement;

        // items element
        getSearchsElement.Add(new XElement("is"));

        // Get element "is" for "Items"
        XElement getItemsElement = getSearchsElement.Descendants("is").Single();


        if (isValid == true)
        {
            if (searchResult != null)
            {

                if (searchResult.Count() > 0)
                {

                    // Insert/Create data as xml
                    for (int i = 0; i < searchResult.Count(); i++)
                    {
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
                }
                else
                {
                    return mro;

                }


            }
            else
            {
                return mro;
            }
        }

        // write status for result
        getSearchsElement.Add(new XElement("status", new XAttribute("bool", isValid.ToString()), errorMessage));

        
        return mro;

    }

    public void Close() {

        engine.CleanUpAndClose();

    }
    

    protected bool TimeNowValidate(DateTime fromDate)
    {
        bool valid = true;

        SearchValidate.DateValidateMsg msg =
            _validator.ValidateDates(fromDate, true, _currentUserIp);

        if (msg == SearchValidate.DateValidateMsg.FromTimeIsSmallerThanDateTimeNow)
        {
            valid = false;
        }

        return valid;
    }

    protected bool DateNowValidate(DateTime fromDate)
    {
        bool valid = true;

        SearchValidate.DateValidateMsg msg =
            _validator.ValidateDates(
                fromDate, false, _currentUserIp);

        if (msg == SearchValidate.DateValidateMsg.FromDateIsSmallerThanDateTimeNow)
        {
            valid = false;
        }

        return valid;
    }


}