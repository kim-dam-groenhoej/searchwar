using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SearchWar.LangSystem;
using SearchWar.ObjectHelper;
using SearchWar.ContinentSystem;
using System.Text;
using System.Web.UI.HtmlControls;
using SearchWar.SearchEngine.Games;
using SearchWar.SearchEngine.Skills;
using SearchWar.SearchEngine.Validate;
using SearchWar.SiteMap;
using Searchwar_netModel;
using System.Dynamic;

public partial class WebControls_WebSearchl : System.Web.UI.UserControl {



    // Current User
    private string CurrentUserCountry;
    private SearchValidate _validator = new SearchValidate();

    //protected void TimeValidate(object source, ServerValidateEventArgs args) {

    //    args.IsValid = true;

    //    SearchValidate.DateValidateMsg msg =
    //        _validator.ValidateDates(
    //            new DateTime(Convert.ToInt32(DdlSearchYearFrom.SelectedValue),
    //                         Convert.ToInt32(DdlSearchMonthFrom.SelectedValue),
    //                         Convert.ToInt32(DdlSearchDayFrom.SelectedValue),
    //                         Convert.ToInt32(DdlSearchFromTimeHour.SelectedValue),
    //                         Convert.ToInt32(DdlSearchFromTimeMinute.SelectedValue),
    //                         0),
    //            new DateTime(Convert.ToInt32(DdlSearchYearTo.SelectedValue), 
    //                         Convert.ToInt32(DdlSearchMonthTo.SelectedValue), 
    //                         Convert.ToInt32(DdlSearchDayTo.SelectedValue), 
    //                         Convert.ToInt32(DdlSearchToTimeHour.SelectedValue), 
    //                         Convert.ToInt32(DdlSearchToTimeMinute.SelectedValue), 
    //                         0), true, CurrentUserIP);

    //    if (msg == SearchValidate.DateValidateMsg.FromTimeIsHigherThanToTime) {
    //        args.IsValid = false;
    //    }

        
    //}

    //protected void DateValidate(object source, ServerValidateEventArgs args) {

    //    args.IsValid = true;

    //    SearchValidate.DateValidateMsg msg =
    //        _validator.ValidateDates(
    //            new DateTime(Convert.ToInt32(DdlSearchYearFrom.SelectedValue),
    //                         Convert.ToInt32(DdlSearchMonthFrom.SelectedValue),
    //                         Convert.ToInt32(DdlSearchDayFrom.SelectedValue),
    //                         Convert.ToInt32(DdlSearchFromTimeHour.SelectedValue),
    //                         Convert.ToInt32(DdlSearchFromTimeMinute.SelectedValue),
    //                         0),
    //            new DateTime(Convert.ToInt32(DdlSearchYearTo.SelectedValue), 
    //                         Convert.ToInt32(DdlSearchMonthTo.SelectedValue),
    //                         Convert.ToInt32(DdlSearchDayTo.SelectedValue),
    //                         Convert.ToInt32(DdlSearchToTimeHour.SelectedValue),
    //                         Convert.ToInt32(DdlSearchToTimeMinute.SelectedValue),
    //                         0), false, CurrentUserIP);

    //    if (msg == SearchValidate.DateValidateMsg.FromDateIsHigherThanToDate) {
    //        args.IsValid = false;
    //    }

    //}

    //protected void TimeNowValidate(object source, ServerValidateEventArgs args) {

    //    args.IsValid = true;

    //    SearchValidate.DateValidateMsg msg =
    //        _validator.ValidateDates(
    //            new DateTime(Convert.ToInt32(DdlSearchYearFrom.SelectedValue),
    //                         Convert.ToInt32(DdlSearchMonthFrom.SelectedValue),
    //                         Convert.ToInt32(DdlSearchDayFrom.SelectedValue), 
    //                         Convert.ToInt32(DdlSearchFromTimeHour.SelectedValue), 
    //                         Convert.ToInt32(DdlSearchFromTimeMinute.SelectedValue), 
    //                         0),
    //            null, true, CurrentUserIP);

    //    if (msg == SearchValidate.DateValidateMsg.FromTimeIsSmallerThanDateTimeNow) {
    //        args.IsValid = false;
    //    }

    //}

    //protected void DateNowValidate(object source, ServerValidateEventArgs args) {

    //    args.IsValid = true;

    //    SearchValidate.DateValidateMsg msg =
    //        _validator.ValidateDates(
    //            new DateTime(Convert.ToInt32(DdlSearchYearFrom.SelectedValue),
    //                         Convert.ToInt32(DdlSearchMonthFrom.SelectedValue),
    //                         Convert.ToInt32(DdlSearchDayFrom.SelectedValue),
    //                         Convert.ToInt32(DdlSearchFromTimeHour.SelectedValue),
    //                         Convert.ToInt32(DdlSearchFromTimeMinute.SelectedValue), 0),
    //            null, false, CurrentUserIP);

    //    if (msg == SearchValidate.DateValidateMsg.FromDateIsSmallerThanDateTimeNow)
    //    {
    //        args.IsValid = false;
    //    }

    //}

    public string CurrentLang {
        get {
            return ViewState[this.UniqueID + "CurrentLang"].ToString();
        }
        set {
            ViewState[this.UniqueID + "CurrentLang"] = value;
        }
    }

    public int CurrentLangId
    {
        get
        {
            return Convert.ToInt32(ViewState[this.UniqueID + "CurrentLangId"]);
        }
        set
        {
            ViewState[this.UniqueID + "CurrentLangId"] = value;
        }
    }

    public string CurrentUserIP
    {
        get
        {
            return ViewState[this.UniqueID + "CurrentUserIP"].ToString();
        }
        set
        {
            ViewState[this.UniqueID + "CurrentUserIP"] = value;
        }
    }
 

    protected void Page_Load(object sender, EventArgs e) {

        // Disable gametype
        DdlSearchGameType.Enabled = false;

    }


    protected override void Render(HtmlTextWriter writer) {




        GameModeSystem gms = new GameModeSystem();
        // Register GameTypeIds for security validation!
        List<dynamic> getGameTypes = gms.GetGameTypeIds();
        foreach (dynamic T in getGameTypes)
        {
            Page.ClientScript.RegisterForEventValidation(DdlSearchGameType.UniqueID, T.gameTypeId.ToString());
        }

        // Register CountryIds for security validation!
        // check cache
        CountrySystem CS = new CountrySystem();
        List<dynamic> getCountyIds = (List<dynamic>)Cache["WebSearch_reggetCountyIds" + CurrentLangId];
        if (getCountyIds == null) {
            getCountyIds = CS.GetCountryIds(CurrentLangId);
            if (getCountyIds != null)
            {
                // add Cache
                Cache.Add("WebSearch_reggetCountyIds" + CurrentLangId, getCountyIds, null, TimeZoneManager.DateTimeNow.AddDays(1),
                          System.Web.Caching.Cache.NoSlidingExpiration,
                          System.Web.Caching.CacheItemPriority.Normal,
                          null);
            }
        }
        foreach (dynamic C in getCountyIds) {
            Page.ClientScript.RegisterForEventValidation(DdlClanCountry.UniqueID, C.SearchWarCountryId.ToString());
            Page.ClientScript.RegisterForEventValidation(DdlSearchCountry.UniqueID, C.SearchWarCountryId.ToString());
        }

        base.Render(writer);

        
        
    }

    public override void DataBind() {
        
        LangaugeSystem ls = new LangaugeSystem();
        CurrentUserCountry = ls.CurrentUserCountry;

        TxtClanName.Text = "GuestClan" + new Random().Next(1, 9999).ToString();


        SkillsSystem ss = new SkillsSystem();
            // !Skills!
            // check cache
        List<dynamic> getSkills = (List<dynamic>)Cache["WebSearch_getSkills" + CurrentLangId];
            if (getSkills == null) {
                getSkills = ss.GetSkills(CurrentLangId);
                if (getSkills != null) {
                    // add Cache
                    Cache.Add("WebSearch_getSkills" + CurrentLangId, getSkills, null, TimeZoneManager.DateTimeNow.AddDays(5),
                              System.Web.Caching.Cache.NoSlidingExpiration,
                              System.Web.Caching.CacheItemPriority.Normal,
                              null);
                }
            }

            DdlClanSkill.Items.Clear();
            DdlClanSkill.Items.Add(new ListItem(GetLocalResourceObject("ListItemChooseResource1").ToString(), string.Empty));
            DdlSearchSkill.Items.Clear();
            DdlSearchSkill.Items.Add(new ListItem(GetLocalResourceObject("ListItemChooseResource1").ToString(), string.Empty));
            foreach (dynamic g in getSkills)
            {

                DdlClanSkill.Items.Add(new ListItem(g.SearchWarSkillName, g.SearchWarSkillId.ToString()));
                DdlSearchSkill.Items.Add(new ListItem(g.SearchWarSkillName, g.SearchWarSkillId.ToString()));

                // DdlSearchSkill.Items.Add(new ListItem(currentSkill.GetAnonymousObject("SearchWarSkillName").GetValue<string>(), currentSkill.GetAnonymousObject("SearchWarSkillId").GetValue<int>().ToString()));
            }

            // !Games! 
            // check cache
            GamesSystem gs = new GamesSystem();
            List<SW_SearchWarGame> getGames = (List<SW_SearchWarGame>)Cache["WebSearch_getGames"];
            if (getGames == null) {
                getGames = gs.GetGames().OrderBy(g => g.SearchWarGameName).ToList<SW_SearchWarGame>();
                if (getGames != null)
                {
                    // add Cache
                    Cache.Add("WebSearch_getGames", getGames, null, TimeZoneManager.DateTimeNow.AddDays(1),
                              System.Web.Caching.Cache.NoSlidingExpiration,
                              System.Web.Caching.CacheItemPriority.Normal,
                              null);
                }
            }

            DdlSearchGame.Items.Clear();
            DdlSearchGame.Items.Add(new ListItem(GetLocalResourceObject("ListItemChooseResource1").ToString(), string.Empty));
            for (int i = 0; i < getGames.Count(); i++) {
                var g = getGames[i];
                DdlSearchGame.Items.Add(new ListItem(g.SearchWarGameName, g.SearchWarGameId.ToString()));
            }


            DdlSearchGameType.Items.Clear();
            DdlSearchGameType.Items.Add(new ListItem(GetLocalResourceObject("ListItemChooseResource1").ToString(), string.Empty));
            HfSearchGameType.Value = DdlSearchGameType.Items.FindByText(GetLocalResourceObject("ListItemChooseResource1").ToString()).Value;


            // !Continents!
            // check cache
            ContinentSystem cs = new ContinentSystem();

            List<dynamic> getContinents = (List<dynamic>)Cache["WebSearch_getContinents" + CurrentLangId];
            if (getContinents == null) {
                getContinents = cs.GetContinents(CurrentLangId);
                if (getContinents != null)
                {
                    // add Cache
                    Cache.Add("WebSearch_getContinents" + CurrentLangId, getContinents, null, TimeZoneManager.DateTimeNow.AddDays(1),
                              System.Web.Caching.Cache.NoSlidingExpiration,
                              System.Web.Caching.CacheItemPriority.Normal,
                              null);
                }
            }
            if (getContinents != null && getContinents.Count > 0) {
                DdlClanContinent.Items.Clear();
                DdlClanContinent.Items.Add(new ListItem(GetLocalResourceObject("ListItemChooseResource1").ToString(),
                                                        string.Empty));

                for (int i = 0; i < getContinents.Count(); i++) {
                    dynamic currentContinent = getContinents[i];

                    DdlClanContinent.Items.Add(
                        new ListItem(currentContinent.SearchWarContinentName,
                                     currentContinent.SearchWarContinentId.ToString()));
                }
                DdlClanContinent.DataBind();


                DdlSearchContinent.Items.Clear();
                DdlSearchContinent.Items.Add(new ListItem(GetLocalResourceObject("ListItemChooseResource1").ToString(),
                                                          string.Empty));

                for (int i = 0; i < getContinents.Count(); i++) {
                    dynamic currentContinent = getContinents[i];

                    DdlSearchContinent.Items.Add(
                        new ListItem(currentContinent.SearchWarContinentName,
                                     currentContinent.SearchWarContinentId.ToString()));
                }
                DdlSearchContinent.DataBind();
            }


            // !Countries!
            CountrySystem CS = new CountrySystem();
            List<dynamic> getCountries = CS.GetCountries(CurrentLangId);

            if (getCountries != null && getCountries.Count > 0) {

                // find contient for usercountry
                for (int i = 0; i < getCountries.Count(); i++) {
                    dynamic currentCountry = getCountries[i];

                    if (CurrentUserCountry ==
                        currentCountry.SearchWarCountryTLD) {
                        DdlClanContinent.Items.FindByValue(
                            currentCountry.SearchWarContinentId.ToString()).
                            Selected = true;
                    }
                }
                // Remove countries there is not in the continent
                getCountries =
                    getCountries.Where(
                        C =>
                        C.SearchWarContinentId ==
                        Convert.ToInt32(DdlClanContinent.SelectedValue)).ToList<dynamic>();



                // Add "Select" for dropdownbox
                DdlClanCountry.Items.Clear();
                DdlClanCountry.Items.Add(new ListItem(GetLocalResourceObject("ListItemChooseResource1").ToString(),
                                                      string.Empty));

                // Loop countries
                for (int i = 0; i < getCountries.Count(); i++) {
                    dynamic currentCountry = getCountries[i];

                    // Add Country to dropdownbox
                    DdlClanCountry.Items.Add(
                        new ListItem(currentCountry.SearchWarCountryName,
                                     currentCountry.SearchWarCountryId.ToString()));

                    // Select current country
                    if (CurrentUserCountry ==
                        currentCountry.SearchWarCountryTLD) {
                        
                        // Set current country in dropdownbox
                        DdlClanCountry.Items.FindByValue(
                            currentCountry.SearchWarCountryId.ToString()).Selected
                            = true;

                        // set value in hiddenfield
                            HfClanCountry.Value =
                                DdlClanCountry.Items.FindByValue(currentCountry.SearchWarCountryId.ToString()).
                                    Value;
                    }
                }


                // Add "Select" for dropdownbox
                DdlSearchCountry.Items.Clear();
                DdlSearchCountry.Items.Add(new ListItem(GetLocalResourceObject("ListItemChooseResource1").ToString(),
                                                        string.Empty));
                // Loop all countries
                for (int i = 0; i < getCountries.Count(); i++) {
                    dynamic currentCountry = getCountries[i];

                    // Add country to dropdownbox
                    DdlSearchCountry.Items.Add(
                        new ListItem(currentCountry.SearchWarCountryName,
                                     currentCountry.SearchWarCountryId.ToString()));

                    // Select that current country
                    if (CurrentUserCountry ==
                        currentCountry.SearchWarCountryTLD) {
                        
                        // Set current continent for country
                        DdlSearchContinent.Items.FindByValue(
                            currentCountry.SearchWarContinentId.ToString()).Selected = true;

                        // set value for country ("empty" because the system need to search after more than one country)
                            HfSearchCountry.Value = "";
                    }
                }
            }


            // !Players!
            const int maxPlayer = 16;

            DdlSearchXvsX.Items.Clear();
            for (int p = 1; p <= maxPlayer; p++) {
                DdlSearchXvsX.Items.Add(new ListItem(p.ToString() + GetLocalResourceObject("LblVersusResource1.Text") + p.ToString(), p.ToString() + "-" + p.ToString()));
            }
            DdlSearchXvsX.Items.FindByValue("5-5").Selected = true;

            // !DATETIME!
            TimeZoneManager timeMng = new TimeZoneManager(CurrentUserIP);

            DateTime datetimeNow = timeMng.ConvertDateTimeFromUtc(TimeZoneManager.DateTimeNow).AddMinutes(20);
            DateTime getDateTimePlusHour = datetimeNow.AddHours(1);

            txtFromDate.Text = datetimeNow.Day.ToString("00") + "/" + datetimeNow.Month.ToString("00") + "/" + datetimeNow.Year.ToString("0000");
            amorpm.InnerText = datetimeNow.ToString("tt");
            if (CurrentLang.ToLower() == "da-dk")
            {
                txtFromTime.Text = datetimeNow.ToString("HH:mm");
                amorpm.Attributes.Add("style", "display: none");
            }
            else
            {
                txtFromTime.Text = datetimeNow.ToString("hh:mm");
            }



        
        

    }

    public static int FixMinute(int number) {
        if (number >= 60) {
            return 00;
        } else {
            return number;
        }
    }


    protected void ChangeCountries(object sender, EventArgs e) {

        DropDownList getDdlContinents = (DropDownList)sender;

        CountrySystem cs = new CountrySystem();
        List<dynamic> getCountries = cs.GetCountries(CurrentLangId, Convert.ToInt32(getDdlContinents.SelectedValue));
        
        DdlClanCountry.Items.Clear();
        DdlClanCountry.Items.Add(new ListItem(GetLocalResourceObject("ListItemChooseResource1").ToString(), string.Empty));
        foreach (var c in getCountries) {
            dynamic currentCountry = c;

            DdlClanCountry.Items.Add(new ListItem(currentCountry.SearchWarCountryName, currentCountry.SearchWarCountryId.ToString()));

            if (CurrentUserCountry == currentCountry.SearchWarCountryTLD) {
                DdlClanCountry.Items.FindByValue(currentCountry.SearchWarCountryId.ToString()).Selected = true;
            }
        }

    }

    protected void BtnSearchWar_OnClick(object sender, EventArgs e) {
        if (Page.IsValid) {

            string[] fromDate = txtFromDate.Text.Split(Char.Parse("/"));
            string[] fromTime = txtFromTime.Text.Split(Char.Parse(":"));

            if (!string.IsNullOrEmpty(amorpm.InnerText) && amorpm.InnerText.ToLower() == "pm")
            {
                fromTime[1] = fromTime[1] + ":00" + " " + amorpm.InnerText.ToLower();
            }

            string getDatetime = fromDate[2] + "/" + fromDate[1] + "/" + fromDate[0] + " " + fromTime[0] + ":" + fromTime[1];

            cSiteMapNode homeCSiteMapNode2 = new CustomSiteMapNode().GetSiteMapNode(7, new SearchWar.LangSystem.LangaugeSystem().CurrentLangId);
            string searchpath = homeCSiteMapNode2.SiteMapNodePath;
            if (!string.IsNullOrEmpty(homeCSiteMapNode2.SiteMapNodeRewrittedPath))
            {
                searchpath = homeCSiteMapNode2.SiteMapNodeRewrittedPath;
            }

                Response.Clear();
                string url = searchpath
                             + "?cn=" + Server.UrlEncode(TxtClanName.Text)
                             +
                             (!string.IsNullOrEmpty(DdlClanSkill.SelectedValue)
                                  ? "&cs=" + Server.UrlEncode(DdlClanSkill.SelectedItem.Text)
                                  : "")
                             + "&cct=" + Server.UrlEncode(DdlClanContinent.SelectedItem.Text)
                             + "&cc=" + Server.UrlEncode(DdlClanCountry.SelectedItem.Text)
                             + "&sg=" + Server.UrlEncode(DdlSearchGame.SelectedItem.Text)
                             +
                             (!string.IsNullOrEmpty(DdlSearchGameType.SelectedValue)
                                  ? "&sgt=" + Server.UrlEncode(HfSearchGameType.Value)
                                  : "")
                             +
                             (!string.IsNullOrEmpty(DdlSearchSkill.SelectedValue)
                                  ? "&ss=" + Server.UrlEncode(DdlSearchSkill.SelectedItem.Text)
                                  : "")
                             + "&sct=" + Server.UrlEncode(DdlSearchContinent.SelectedItem.Text)
                             +
                             (!string.IsNullOrEmpty(DdlSearchCountry.SelectedValue)
                                  ? "&sc=" + Server.UrlEncode(DdlSearchCountry.SelectedItem.Text)
                                  : "")
                             + "&sxv=" + DdlSearchXvsX.SelectedItem.Value.Split(Char.Parse("-"))[0]
                             + "&svx=" + DdlSearchXvsX.SelectedItem.Value.Split(Char.Parse("-"))[1]
                             + (!string.IsNullOrEmpty(TxtMap.Text) ? "&sm=" + Server.UrlEncode(TxtMap.Text) : "")
                + "&sfd=" + Server.UrlEncode(DateTime.Parse(getDatetime).ToString("dd-MM-yyyy HH:mm:ss"));

            Response.Redirect(url, false);

            }
    }

}
