using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using System.Xml;
using System.Data;
using System.Web;
using System.Text;
using System.IO;
using SearchWar.ContinentSystem;
using SearchWar.SearchEngine;
using SearchWar.ObjectHelper;
using System.Collections;
using SearchWar.SearchEngine.Games;
using Searchwar_netModel;

/// <summary>
/// Summary description for XmlHandler
/// </summary>
public class XmlHandler : IHttpHandler {
    public XmlHandler() {
        //
        // TODO: Add constructor logic here
        //
    }

    public bool IsReusable
    {
        get
        {
            return true;
        }
    }

    public void ProcessRequest(HttpContext context) {
        int getLangId;
        string data = null;

        string getXmlType = context.Request.QueryString["xml"];
        if (string.IsNullOrEmpty(getXmlType)) {
            return;
        }
        getXmlType = HttpContext.Current.Server.HtmlEncode(getXmlType);

        if (getXmlType == "ns")
        {
            string sLangId = context.Request.QueryString["li"];
            if (string.IsNullOrEmpty(sLangId)) {
                return;
            }

            // Convert langid querystring to int32
            if (!Int32.TryParse(sLangId, out getLangId)) {
                context.Response.Status = "404 Bad Request";
            }

            data = XmlGetNewestSearchs(getLangId);
        }

        if (getXmlType == "c") {
            int GetContinentId;

            string sContinentId = context.Request.QueryString["ci"];
            if (string.IsNullOrEmpty(sContinentId)) {
                return;
            }

            // Convert continentid querystring to int32
            if (!Int32.TryParse(sContinentId, out GetContinentId)) {
                context.Response.Status = "404 Bad Request";
            }

            string sLangId = context.Request.QueryString["li"];
            if (string.IsNullOrEmpty(sLangId)) {
                return;
            }

            // Convert langid querystring to int32
            if (!Int32.TryParse(sLangId, out getLangId)) {
                context.Response.Status = "404 Bad Request";
            }

            data = XmlGetCountries(getLangId, GetContinentId);
        }

        if (getXmlType == "gt") {
            int GetGameId;

            string sGameId = context.Request.QueryString["gi"];
            if (string.IsNullOrEmpty(sGameId)) {
                return;
            }

            // Convert gameid querystring to int32
            if (!Int32.TryParse(sGameId, out GetGameId)) {
                context.Response.Status = "404 Bad Request";
            }

            data = XmlGetGameTypes(GetGameId);
        }
        
        if (!string.IsNullOrEmpty(data)) {
            context.Response.ContentType = "text/xml";
            context.Response.ContentEncoding = Encoding.UTF8;
            context.Response.Write(XmlWhiteSpaceModule.RemoveWhitespace(data));
        } else {
            context.Response.Status = "404 Bad Request";
        }
    }


    private static string XmlGetNewestSearchs(int langId)
    {

        // Get newest search from database
        const int defaultFromLastSeconds = 35;
        List<NewestSearchObject> getNewestSearchs = SearchEngine.GetNewestMatchSearchs(5, langId, defaultFromLastSeconds);
        
        // check the result is not null or 0
        if (getNewestSearchs.Count() > 0 && getNewestSearchs != null) {

            // Create documet and first element called "ss" for "Searchs"
            XDocument CreateXmlNewestSearchs = new XDocument(
                new XDeclaration("1.0", "iso-8859-1", "true"),
                new XElement("ss"));

            // Get element "ss" for "Searchs"
            XElement GetNewestSearchsElement = CreateXmlNewestSearchs.Descendants("ss").Single();
            // Insert/Create data as xml
            for (int i = 0; i < getNewestSearchs.Count(); i++) {
                var s = getNewestSearchs[i];

                // Create element data
                GetNewestSearchsElement.Add(
                        new XElement("s",
                            new XAttribute("i", s.SearchWarID.ToString()),
                            new XElement("cn", s.ClanName),
                            new XElement("ct", new XAttribute("i", s.ClanContinentData.SearchWarContinentId), s.ClanContinentData.SearchWarContinentName),
                            new XElement("cy", new XAttribute("i", s.ClanCountryData.SearchWarCountrytId), new XAttribute("t", s.ClanCountryData.SearchWarCountryTLD), s.ClanCountryData.SearchWarCountryName),
                            new XElement("g", new XAttribute("i", s.SearchGame.SearchWarGameId), s.SearchGame.SearchWarGameName),
                            new XElement("gt", new XAttribute("i", s.SearchGameType != null ? s.SearchGameType.SearchWarGameTypeId.ToString() : ""), s.SearchGameType != null ? s.SearchGameType.SearchWarGameTypeName : ""),
                            new XElement("fd", s.SearchMatchStart.ToString()),
                            new XElement("x", s.SearchXvs),
                            new XElement("y", s.SearchvsX)));

            }

            // Write/save data
            StringWriter sw = new StringWriter();
            XmlWriter w = XmlWriter.Create(sw);
            CreateXmlNewestSearchs.Save(w);

            w.Close();

            return sw.ToString();
        } else {
            return null;
        }

    }



    private static string XmlGetGameTypes(int gameId) {

        string cacheXmlGameTypes = (string)HttpContext.Current.Cache["XmlHandler_GetGameTypes" + gameId.ToString()];
        if (!string.IsNullOrEmpty(cacheXmlGameTypes)) {

            return cacheXmlGameTypes;

        } else
        {
            GameModeSystem gms = new GameModeSystem();
            List<SW_SearchWarGameType> GetGameTypes = gms.GetGameTypes(gameId);

            if (GetGameTypes.Count > 0 && GetGameTypes != null)
            {
                XDocument CreateXmlGameTypes = new XDocument(
                    new XDeclaration("1.0", "iso-8859-1", "true"),
                    new XElement("gts"));
                XElement GetGameTypesElement = CreateXmlGameTypes.Descendants("gts").Single();

                // Insert data to xml
                for (int i = 0; i < GetGameTypes.Count(); i++) {
                    var G = GetGameTypes[i];

                    GetGameTypesElement.Add(
                        new XElement("gt",
                                     new XAttribute("id", G.SearchWarGameTypeId.ToString()),
                                     new XElement("gn", G.SearchWarGameTypeName)));
                }

                // Write data
                StringWriter sw = new StringWriter();
                XmlWriter w = XmlWriter.Create(sw);
                CreateXmlGameTypes.Save(w);

                w.Close();

                HttpContext.Current.Cache.Add("XmlHandler_GetGameTypes" + gameId.ToString(), 
                    sw.ToString(), 
                    null, 
                    TimeZoneManager.DateTimeNow.AddDays(5),
                    System.Web.Caching.Cache.NoSlidingExpiration,
                    System.Web.Caching.CacheItemPriority.Normal,
                    null);

                return sw.ToString();
            }
            else
            {
                return null;
            }
        }

    }

    private static string XmlGetCountries(int langId, int continentId) {
        
        string cacheXmlGames = (string)HttpContext.Current.Cache["XmlHandler_GetCountries" + continentId.ToString() + langId.ToString()];
        if (!string.IsNullOrEmpty(cacheXmlGames)) {

            return cacheXmlGames;

        } else
        {

            CountrySystem cs = new CountrySystem();

            List<dynamic> GetCountries = cs.GetCountries(langId, continentId);

            if (GetCountries.Count > 0 && GetCountries != null)
            {
                XDocument CreateXmlCountries = new XDocument(
                    new XDeclaration("1.0", "iso-8859-1", "true"),
                    new XElement("cs"));
                XElement GetCountriesElement = CreateXmlCountries.Descendants("cs").Single();

                // Insert data to xml
                for (int i = 0; i < GetCountries.Count(); i++) {
                    dynamic GetCountryData = GetCountries[i];

                    GetCountriesElement.Add(
                        new XElement("c",
                                     new XAttribute("i",
                                                    GetCountryData.SearchWarCountryId),
                                     new XElement("cn",
                                                  GetCountryData.SearchWarCountryName)));
                }

                // Write data
                StringWriter sw = new StringWriter();
                XmlWriter w = XmlWriter.Create(sw);
                CreateXmlCountries.Save(w);

                w.Close();

                HttpContext.Current.Cache.Add("XmlHandler_GetCountries" + continentId.ToString() + langId.ToString(),
                    sw.ToString(),
                    null,
                    TimeZoneManager.DateTimeNow.AddDays(5),
                    System.Web.Caching.Cache.NoSlidingExpiration,
                    System.Web.Caching.CacheItemPriority.Normal,
                    null);

                return sw.ToString();
            }
            else
            {
                return null;
            }
        }
    }
}
