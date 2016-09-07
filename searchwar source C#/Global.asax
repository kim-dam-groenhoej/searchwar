<%@ Application Language="C#" %>
<%@ Import Namespace="SearchWar.SiteMap" %>
<%@ Import Namespace="Searchwar_netModel" %>

<script runat="server">

    void Application_Start(object sender, EventArgs e) 
    {

    }

    private string CheckShortLang(string shortLang)
    {

        using (Searchwar_netEntities db = new Searchwar_netEntities())
        {

            if (!string.IsNullOrEmpty(shortLang))
            {


                var getShortLang = (from l in db.SW_Lang
                                    where
                                        l.LangShortname.ToLower().Contains(shortLang) ==
                                        true
                                    select l).SingleOrDefault();

                if (getShortLang != null)
                {

                    return getShortLang.LangShortname;
                }
            }

            return null;
        }
    }
    
    private const string CurrentUserIP = "62.107.21.129";
    private readonly string CurrentUserCountry = LookUpService.GetCountry(CurrentUserIP);
    
    protected void Application_BeginRequest(object sender, EventArgs e)
    {
        
        HttpContext context = HttpContext.Current;
        string oldPath = context.Request.Path.ToLower();

        if (WebAppSettings.Get().CacheAll == false)
        {
            List<string> keys = new List<string>();

            // retrieve application Cache enumerator
            IDictionaryEnumerator enumerator = context.Cache.GetEnumerator();

            // copy all keys that currently exist in Cache
            while (enumerator.MoveNext())
            {
                keys.Add(enumerator.Key.ToString());
            }

            // delete every key from cache
            for (int i = 0; i < keys.Count; i++)
            {
                context.Cache.Remove(keys[i]);
            }
        }

        if (!oldPath.Contains(".png") && !oldPath.Contains(".jpg") && !oldPath.Contains(".gif") && !oldPath.Contains(".ashx") && !oldPath.Contains(".jpeg") && !oldPath.Contains(".axd"))
        {
            string hostname = GetDomain.GetDomainFromUrl(context.Request.Url.ToString());
            string currentLang = context.Request.Url.Host.Split('.')[0];
            

            // www is default empty
            if (currentLang != "www")
            {
                currentLang = CheckShortLang(currentLang);
            }
            else
            {
                currentLang = "";
            }

            if (string.IsNullOrEmpty(currentLang))
            {
                if (string.IsNullOrEmpty(CurrentUserCountry))
                {
                    currentLang = System.Threading.Thread.CurrentThread.CurrentCulture.Name;
                }
                else
                {
                    currentLang = CurrentUserCountry;
                    currentLang = CheckShortLang(currentLang);
                }

                ANOProfile.SaveCookies(currentLang, CurrentUserIP, null);
                context.Response.Clear();
                context.Response.RedirectPermanent("http://" + currentLang.Split('-')[0] + "." + hostname);
                context.Response.End();
            }


            // if there is no cookie or an error
            try
            {
                // Get Cookie
                ANO_User getProfile = ANOProfile.GetCookieValues(CurrentUserIP);

                // Check there is a cookie
                if (getProfile != null)
                {

                    currentLang = CheckShortLang(currentLang);

                    if (!string.IsNullOrEmpty(currentLang))
                    {
                        // Save cookie as default
                        ANOProfile.SaveCookies(System.Threading.Thread.CurrentThread.CurrentCulture.Name, CurrentUserIP, getProfile.SearchMatchID);
                        
                        System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo(currentLang);
                        System.Threading.Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo(currentLang);
                    }
                    else
                    {
                        // Save
                        currentLang = getProfile.ShortLang;
                        System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo(currentLang);
                        System.Threading.Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo(currentLang);
                    }
                }
                else
                {
                    currentLang = CheckShortLang(CurrentUserCountry);

                    if (!string.IsNullOrEmpty(currentLang))
                    {
                        currentLang = System.Threading.Thread.CurrentThread.CurrentCulture.Name;
                            
                        // Save cookie as default
                        ANOProfile.SaveCookies(currentLang, CurrentUserIP, getProfile.SearchMatchID);
                        
                        System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo(currentLang);
                        System.Threading.Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo(currentLang);
                    }
                    else
                    {

                        // Check CurrentUserCountry from IP was found
                        if (!string.IsNullOrEmpty(CurrentUserCountry))
                        {
                            // Get shortname of country by database
                            currentLang = CheckShortLang(CurrentUserCountry);

                            System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo(currentLang);
                            System.Threading.Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo(currentLang);

                            // Save cookie
                            ANOProfile.SaveCookies(currentLang, CurrentUserIP, null);
                        }
                        else
                        {
                            currentLang = System.Threading.Thread.CurrentThread.CurrentCulture.Name;
                            
                            // Save cookie as default
                            ANOProfile.SaveCookies(currentLang, CurrentUserIP, getProfile.SearchMatchID);

                            System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo(currentLang);
                            System.Threading.Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo(currentLang);

                        }
                        
                    }
                }

            }
            catch
            {
                // Check CurrentUserCountry from IP was found
                if (!string.IsNullOrEmpty(CurrentUserCountry))
                {
                    currentLang = CheckShortLang(CurrentUserCountry);

                    System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo(currentLang);
                    System.Threading.Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo(currentLang);

                    // Delete cookie and save after
                    ANOProfile.DeleteCookies();
                    ANOProfile.SaveCookies(currentLang, CurrentUserIP, null);
                }
                else
                {
                    currentLang = System.Threading.Thread.CurrentThread.CurrentCulture.Name;
                    
                    System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo(currentLang);
                    System.Threading.Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo(currentLang);

                    // Save cookie as default
                    ANOProfile.DeleteCookies();
                    ANOProfile.SaveCookies(currentLang, CurrentUserIP, null);

                }

            }

            List<SiteMapNodeRegex> sitemapNodes = (List<SiteMapNodeRegex>)context.Cache["sitemapnodesregex" + currentLang];
            if (sitemapNodes == null)
            {
                sitemapNodes = new SearchWar.SiteMap.CustomSiteMapSystem().GetAllSiteMapNodeRegexAndUrl();
                context.Cache.Add("sitemapnodesregex" + currentLang, sitemapNodes, null, TimeZoneManager.DateTimeNow.AddDays(1),
                                  System.Web.Caching.Cache.NoSlidingExpiration,
                                  System.Web.Caching.CacheItemPriority.Normal,
                                  null);
            }

            for (int i = 0; i < sitemapNodes.Count; i++)
            {
                var node = sitemapNodes[i];
                if (!string.IsNullOrEmpty(node.Regex))
                {

                    Regex regex = new Regex(node.Regex, RegexOptions.IgnorePatternWhitespace | RegexOptions.IgnoreCase);
                    if (regex.IsMatch(oldPath))
                    {

                        node.ToUrl = Regex.Replace(oldPath, node.Regex, node.ToUrl);

                        string querystring = Request.Url.Query;
                        context.RewritePath(node.ToUrl + querystring, false);
                        context.Application["rawurl"] = node.ToUrl;

                    }

                }
            }
            
        }
    }
    
    void Application_End(object sender, EventArgs e) 
    {
        //  Code that runs on application shutdown
    }
        
    void Application_Error(object sender, EventArgs e) 
    { 
        // Code that runs when an unhandled error occurs

    }

    void Session_Start(object sender, EventArgs e) 
    {
        // Code that runs when a new session is started

    }

    void Session_End(object sender, EventArgs e) 
    {
        // Code that runs when a session ends. 
        // Note: The Session_End event is raised only when the sessionstate mode
        // is set to InProc in the Web.config file. If session mode is set to StateServer 
        // or SQLServer, the event is not raised.

    }
       
</script>
