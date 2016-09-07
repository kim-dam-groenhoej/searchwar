using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI.WebControls;
using SearchWar.SiteMap;
using SearchWar.SiteMap.MetaTags;
using SearchWar.LangSystem;
using SearchWar.ObjectHelper;
using Searchwar_netModel;

public partial class MasterPage : System.Web.UI.MasterPage
{
    // Current User
    private const string CurrentUserIP = "62.107.21.129";
    private readonly string CurrentUserCountry = LookUpService.GetCountry(CurrentUserIP);

    protected void Page_Load(object sender, EventArgs e)
    {

        // Select current langauge
        string currentLang = new LangaugeSystem().CurrentLang;

        // Show if javascript is disable
        ErrorJS.InnerText = GetLocalResourceObject("JavascriptBoxErrorMessageError").ToString();

        // url for JS
        string pageId = Context.Request.AppRelativeCurrentExecutionFilePath.ToString();
        string jsurl = ("~/js.axd?p=" + HttpUtility.UrlEncode(pageId)).ChangeToJsHost();
        scriptsall.Scripts.Add(new System.Web.UI.ScriptReference(jsurl));


        if (Page.IsPostBack) return;


        // get timezone
        ANO_User GetUserCookie = ANOProfile.GetCookieValues(CurrentUserIP);
        string currentZone = GetUserCookie.TimeZone;
        if (string.IsNullOrEmpty(currentZone))
        {
            DivTimeZone.Visible = true;
        }
        else
        {
            DivTimeZone.Visible = false;
        }


        
        // check cache
        System.Collections.ObjectModel.ReadOnlyCollection<TimeZoneInfo> getTimeZones = (System.Collections.ObjectModel.ReadOnlyCollection<TimeZoneInfo>)Cache["masterpage_timezones"];
        if (getTimeZones == null)
        {
            getTimeZones = TimeZoneInfo.GetSystemTimeZones();
            if (getTimeZones != null)
            {
                // add Cache
                Cache.Add("masterpage_timezones", getTimeZones, null, TimeZoneManager.DateTimeNow.AddDays(30),
                          System.Web.Caching.Cache.NoSlidingExpiration,
                          System.Web.Caching.CacheItemPriority.Normal,
                          null);
            }
        }

        DdlTimeZones.Items.Clear();
        foreach (var zone in getTimeZones)
        {
            DdlTimeZones.Items.Add(new ListItem(zone.DisplayName.ToString(), zone.BaseUtcOffset.ToString()));
        }
        if (!string.IsNullOrEmpty(currentZone))
        {
            DdlTimeZones.Items.FindByValue(currentZone).Selected = true;
        }


        // Insert Langauges in DropDownList
        DropDownList ddlLang = DdlSelectLang;
        ddlLang.Items.Clear();

        // check cache

        List<SW_Lang> langs = (List<SW_Lang>) Cache["masterpage_langs"];
        LangaugeSystem ls = new LangaugeSystem();
        if (langs == null)
        {
            langs = ls.GetLangs();
            if (langs != null)
            {
                // add Cache
                Cache.Add("masterpage_langs", langs, null, TimeZoneManager.DateTimeNow.AddDays(10),
                          System.Web.Caching.Cache.NoSlidingExpiration,
                          System.Web.Caching.CacheItemPriority.Normal,
                          null);
            }
        }
        
        foreach (var lang in langs)
        {
            ddlLang.Items.Add(new ListItem(lang.LangName, lang.LangShortname));
        }

        
        // check cache
        int? currentLangId = (int?)Cache["masterpage_LangId" + currentLang];
        if (currentLangId.HasValue == false) {
            currentLangId = ls.GetLang(currentLang).LangId;
            if (currentLangId.HasValue == true) {
                // add Cache
                Cache.Add("masterpage_LangId" + currentLang, currentLangId, null, TimeZoneManager.DateTimeNow.AddDays(10),
                          System.Web.Caching.Cache.NoSlidingExpiration,
                          System.Web.Caching.CacheItemPriority.Normal,
                          null);
            }
        }

        if (!string.IsNullOrEmpty(currentLang))
        {
            DdlSelectLang.Items.FindByValue(currentLang).Selected = true;
        }



        // change url for logo (Now with shortlang)

        CustomSiteMapNode csm = new CustomSiteMapNode();
        cSiteMapNode homeCSiteMapNode = csm.GetSiteMapNode(2, currentLangId.Value);
        string getPath = homeCSiteMapNode.SiteMapNodePath;
        if (!string.IsNullOrEmpty(homeCSiteMapNode.SiteMapNodeRewrittedPath))
        {
            getPath = homeCSiteMapNode.SiteMapNodeRewrittedPath;
        }
        HyperImgLogo.NavigateUrl = getPath;

        // Insert menu data
        // check cache
        List<cSiteMapNode> sitemapnodes = (List<cSiteMapNode>)Cache["masterpage_nodes" + currentLangId];
        
        if (sitemapnodes == null) {
            sitemapnodes = csm.GetSiteMapNodes((int)currentLangId, false);
            if (sitemapnodes != null)
            {
                // add Cache
                Cache.Add("masterpage_nodes" + currentLangId, sitemapnodes, null, TimeZoneManager.DateTimeNow.AddDays(1),
                          System.Web.Caching.Cache.NoSlidingExpiration,
                          System.Web.Caching.CacheItemPriority.Normal,
                          null);
            }
        }
        CMenu.DataSource = sitemapnodes;
        CMenu.CurrentUserIP = CurrentUserIP;
        CMenu.DataBind();

            

        // Properties for metatags event
        SiteMapNodeMetatagsObject getMapMetaTags = null;
        cSiteMapNode getCurrentSiteMap = null;
        int currentSiteMapId;

        // Get CurrentSiteMapNode
        getCurrentSiteMap = csm.GetCurrentSiteMapNode((int)currentLangId);
        currentSiteMapId = getCurrentSiteMap.SiteMapNodeId;

        // Get Metatags for current cSiteMapNode
        // check cache
        getMapMetaTags = (SiteMapNodeMetatagsObject)Cache["masterpage_NodeMetatags" + currentSiteMapId.ToString() + currentLangId.ToString()];
        SiteMapNodeMetaTags smm = new SiteMapNodeMetaTags();
        if (getMapMetaTags == null)
        {
            getMapMetaTags = smm.GetMetaTags(currentSiteMapId, (int)currentLangId);
            if (getMapMetaTags != null)
            {
                // add cache
                Cache.Add("masterpage_NodeMetatags" + currentSiteMapId.ToString() + currentLangId.ToString(),
                          getMapMetaTags,
                          null,
                          TimeZoneManager.DateTimeNow.AddDays(4),
                          System.Web.Caching.Cache.NoSlidingExpiration,
                          System.Web.Caching.CacheItemPriority.Normal,
                          null);

                
            }
        }
        
            
        // Insert MetaTags
        string pageTitle = Request.Url.Host;
        if (getMapMetaTags != null)
        {
            MetaAuthor.Content = getMapMetaTags.MetaTagAuthor;
            MetaCache.Content = getMapMetaTags.MetaTagCache;
            MetaCopyright.Content = getMapMetaTags.MetaTagCopyright;
            MetaDescription.Content = getMapMetaTags.MetaTagDescription;
            MetaKeywords.Content = getMapMetaTags.MetaTagKeywords;
            MetaLanguage.Content = getMapMetaTags.MetaTagLanguage;
            MetaPublisher.Content = getMapMetaTags.MetaTagPublisher;
            MetaRobots.Content = getMapMetaTags.MetaTagRobots;
            MetaRevisitAfter.Content = getMapMetaTags.MetaTagRevisitAfter;
            MetaCacheControl.Content = getMapMetaTags.MetaTagCacheControl;
            pageTitle += " - " + getMapMetaTags.MetaTagTitle;
        }


        Page.Title = pageTitle;
    }


    protected void DdlTimeZones_SelectedIndexChanged(object sender, EventArgs e)
    {
        Response.Clear();

        DivTimeZone.Visible = false;

        // Save cookie
        ANOProfile.SaveTimeZone(CurrentUserIP, DdlTimeZones.SelectedValue);


        Response.Redirect("~/");
        Response.End();

    }


    protected void DdlSelectLang_SelectedIndexChanged(object sender, EventArgs e)
    {
        Response.Clear();

        string selectedLang = ((DropDownList)sender).SelectedItem.Value;

        // Save cookie
        ANOProfile.SaveCookies(selectedLang, CurrentUserIP, null);

        CustomSiteMapNode csm = new CustomSiteMapNode();
        LangaugeSystem ls = new LangaugeSystem();
        Page.Culture = selectedLang;
        Page.UICulture = selectedLang;

        cSiteMapNode homeSiteMapNode = csm.GetSiteMapNode(2, ls.GetLang(selectedLang).LangId);
        string getPath = homeSiteMapNode.SiteMapNodePath;
        if (homeSiteMapNode.SiteMapNodeRewrittedPath != null)
        {
            getPath = "http://" + selectedLang.Split('-')[0] + "." + GetDomain.GetDomainFromUrl(Request.Url.ToString()) + VirtualPathUtility.ToAbsolute(homeSiteMapNode.SiteMapNodeRewrittedPath);
        }


        Response.Redirect(getPath);
        Response.End();
        
    }

}