using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SearchWar.LangSystem;
using SearchWar.SearchEngine;
using SearchWar.ContinentSystem;
using SearchWar.ObjectHelper;
using SearchWar.SearchEngine.Games;
using SearchWar.SearchEngine.Skills;

public partial class Searching : BasePage // BasePage
{
    // Current lang
    public static string CurrentLang = null;
    public static int CurrentLangId;
    // Current User
    private const string CurrentUserIP = "62.107.21.129";
    private readonly string CurrentUserCountry = LookUpService.GetCountry(CurrentUserIP);

    protected void Page_Load(object sender, EventArgs e)
    {

        if (!Page.IsPostBack)
        {

            try
            {

                ImgLoading.ImageUrl = ImgLoading.ImageUrl.ChangeToImageHost();

                H1Results.InnerHtml += " - " + Request.QueryString["sg"]; // insert game name

                #region Search engine
                HttpRequest currentR = Request;

                // Set values of current lang
                LangaugeSystem ls = new LangaugeSystem();
                CurrentLang = ls.CurrentLang;
                CurrentLangId = ls.CurrentLangId;


                #region Get QueryStringsDatas (Convert to htmlencodes)

                var searchQueryDatas = new
                                           {
                                               ClanSkill = currentR.QueryString["cs"],
                                               ClanContinent = currentR.QueryString["cct"],
                                               ClanCountry = currentR.QueryString["cc"],
                                               SearchSkill = currentR.QueryString["ss"],
                                               SearchContinent = currentR.QueryString["sct"],
                                               SearchCountry = (string)currentR.QueryString["sc"],
                                               SearchGame = (string)currentR.QueryString["sg"],
                                               SearchGameMode = (string)currentR.QueryString["sgt"]
                                           }.ToAnonymousObjectCollection();

                foreach (AnonymousObject s in searchQueryDatas)
                {
                    s.SetValue(Server.HtmlEncode(Server.UrlDecode(s.GetValue<string>())));
                }

                #endregion

                #region Get QueryStrings custom text data (no validate or convert)

                var searchQueryCustomTextData = new
                                                {
                                                    ClanName = Server.HtmlEncode(Server.UrlDecode(currentR.QueryString["cn"])),
                                                    SearchMap = Server.HtmlEncode(Server.UrlDecode(currentR.QueryString["sm"]))
                                                };

                #endregion

                #region Get QueryStrings customdata

                var searchQueryCustomData = new
                                                {
                                                    SearchMatchStart = currentR.QueryString["sfd"],
                                                    SearchXvs = currentR.QueryString["sxv"],
                                                    SearchvsX = currentR.QueryString["svx"]
                                                };

                #endregion



                try
                {


                    #region Get IDs of searchQueryDatas

                    int? clanSkillId = null;
                    SkillsSystem ss = new SkillsSystem();
                    if (!string.IsNullOrEmpty(searchQueryDatas.GetAnonymousObject("ClanSkill").GetValue<string>()))
                    {
                        clanSkillId =
                            ((dynamic)ss.GetSkill(searchQueryDatas.GetAnonymousObject("ClanSkill").GetValue<string>(),
                                                  CurrentLangId)).SearchWarSkillId;
                    }


                    int? searchSkillId = null;
                    if (!string.IsNullOrEmpty(searchQueryDatas.GetAnonymousObject("SearchSkill").GetValue<string>()))
                    {
                        searchSkillId =
                            ((dynamic)ss.GetSkill(searchQueryDatas.GetAnonymousObject("SearchSkill").GetValue<string>(),
                                                  CurrentLangId)).SearchWarSkillId;
                    }
                    ContinentSystem cs = new ContinentSystem();
                    int clanContinentId = ((dynamic)cs.GetContinent(CurrentLangId,
                                                                       searchQueryDatas.GetAnonymousObject("ClanContinent").
                                                                           GetValue<string>())).SearchWarContinentId;

                    CountrySystem cys = new CountrySystem();
                    int clanCountryId = ((dynamic)cys.GetCountry(CurrentLangId,
                                                                 searchQueryDatas.GetAnonymousObject("ClanCountry").GetValue
                                                                     <string>())).SearchWarCountryId;


                    int searchContinent = ((dynamic)cs.GetContinent(CurrentLangId,
                                                                       searchQueryDatas.GetAnonymousObject("SearchContinent")
                                                                           .
                                                                           GetValue<string>())).SearchWarContinentId;


                    int? searchCountryId = null;
                    if (!string.IsNullOrEmpty(searchQueryDatas.GetAnonymousObject("SearchCountry").GetValue<string>()))
                    {
                        searchCountryId = ((dynamic)cys.GetCountry(CurrentLangId,
                                                                   searchQueryDatas.GetAnonymousObject("SearchCountry").
                                                                       GetValue
                                                                       <string>())).SearchWarCountryId;
                    }

                    GamesSystem gs = new GamesSystem();
                    int searchGameId = gs.GetGame(searchQueryDatas.GetAnonymousObject("SearchGame").GetValue
                                                               <string>()).SearchWarGameId;

                    int? searchGameModeId = null;
                    if (!string.IsNullOrEmpty(searchQueryDatas.GetAnonymousObject("SearchGameMode").GetValue<string>()))
                    {
                        GameModeSystem gms = new GameModeSystem();
                        searchGameModeId =
                            gms.GetGameType(searchQueryDatas.GetAnonymousObject("SearchGameMode").GetValue
                                                           <string>()).SearchWarGameTypeId;
                    }

                    #endregion


                    #region Create url to the "client javascript" (get "data search results" as xml)

                    ANO_User getprofile = ANOProfile.GetCookieValues(CurrentUserIP);
                    Guid newID = Guid.NewGuid();
                    ANOProfile.SaveCookies(CurrentUserIP, newID);

                    xmlSearchUrl = "http://" + GetDomain.GetDomainFromUrl(Context.Request.Url.ToString()) + "/m.ashx";
                    xmlSearchUrlaction = xmlSearchUrl + "?li= " + CurrentLangId.ToString()
                                    + "&cn=" + Server.UrlEncode(searchQueryCustomTextData.ClanName)
                                    + ((clanSkillId.HasValue) ? "&cs=" + clanSkillId.Value.ToString() : "")
                                    + "&cct=" + clanContinentId.ToString()
                                    + "&cc=" + clanCountryId.ToString()
                                    + "&sg=" + searchGameId.ToString()
                                    + ((searchGameModeId.HasValue) ? "&sgt=" + searchGameModeId.Value.ToString() : "")
                                    + ((searchSkillId.HasValue) ? "&ss=" + searchSkillId.Value.ToString() : "")
                                    + "&sct=" + searchContinent.ToString()
                                    + ((searchCountryId.HasValue) ? "&sc=" + searchCountryId.Value.ToString() : "")
                                    + "&sxv=" + searchQueryCustomData.SearchXvs
                                    + "&svx=" + searchQueryCustomData.SearchvsX
                                    +
                                    (!string.IsNullOrEmpty(searchQueryCustomTextData.SearchMap)
                                         ? "&sm=" + Server.UrlEncode(searchQueryCustomTextData.SearchMap)
                                         : "")
                                    + "&sfd=" + Server.UrlEncode(searchQueryCustomData.SearchMatchStart) + "&option=search,chat&action=1";

                    xmlSearchUrl = xmlSearchUrl + "?option=search,chat&li= " + CurrentLangId.ToString();

                    #endregion

                }
                catch
                {
                    // nothing "error"
                }

            }
            catch
            {

            }

                #endregion
        }

        
    }

    // default values
    protected string xmlSearchUrl = "error.aspx";
    protected string xmlSearchUrlaction = "error.aspx";

}
