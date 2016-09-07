using System;
using System.Web;

/// <summary>
/// Summary description for ANOProfile
/// </summary>
public class ANOProfile : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
    }

	public ANOProfile()
	{
		//
		// TODO: Add constructor logic here
		//
	}

    private const string cookieName = "SearchWar_User";
    private const string cookieName2 = "SearchWar_User2";

    public static ANO_User GetCookieValues(string userIpAddress, 
        HttpContext context)
    {
        
        

        // Get UserIpAddress
        string getUserIp = userIpAddress;
        ANO_User createrANOuser = new ANO_User();


        // Get Cookie
        HttpCookie getCookie = context.Request.Cookies.Get(cookieName);
        if (getCookie != null) {
            if (getCookie.Values["IpAddress"].Equals(getUserIp.EncodeToMd5())) {

                createrANOuser.IPAddress = getUserIp;
                createrANOuser.DateAdded = new DateTime(Convert.ToInt64(getCookie.Values["DateAdded"]));

                // Check lang
                if (System.Text.RegularExpressions.Regex.Match(getCookie.Values["ShortLang"].ToString(), "^[a-z]{2}-[A-Z]{2}$").Success.Equals(true)) {
                    createrANOuser.ShortLang = context.Server.HtmlEncode(getCookie.Values["ShortLang"].ToString());
                } else {
                    throw (new ArgumentException(getCookie.Values["ShortLang"].ToString() + " dont match", "Wrong lang (Not match)"));
                }

                createrANOuser.SearchMatchID = !string.IsNullOrEmpty(getCookie.Values["SearchMatchID"])
                                                   ? (Guid?)new Guid(getCookie.Values["SearchMatchID"])
                                                   : null;

                createrANOuser.UserID = new Guid(getCookie.Values["UserId"]);

                if (!string.IsNullOrEmpty(getCookie.Values["TimeZone"])) {
                    createrANOuser.TimeZone = HttpUtility.UrlDecode(getCookie.Values["TimeZone"]);
                } else {
                    createrANOuser.TimeZone = null;
                }
                

                
                

                return createrANOuser;

            }
        }

        return null;
    }

    public static ANO_User GetCookieValues(string userIpAddress)
    {
        return GetCookieValues(userIpAddress, HttpContext.Current);
    }

    public static void SaveCookies(string userIpAddress, 
        Guid? searchMatchID) {

            SaveCookies(GetCookieValues(userIpAddress).ShortLang,
                searchMatchID,
                userIpAddress,
                null,
                null,
                null);
    }

    public static void SaveCookies(string userIpAddress,
        Guid? searchMatchID, 
        Guid? userid)
    {

        SaveCookies(GetCookieValues(userIpAddress).ShortLang,
            searchMatchID,
            userIpAddress,
            null, 
            null,
            userid);
    }


    public static void SaveCookies(string shortlang,
        string userIpAddress,
        Guid? searchMatchID) {

        SaveCookies(shortlang, 
            searchMatchID, 
            userIpAddress, 
            HttpContext.Current, 
            null,
            null);

    }

    public static void SaveTimeZone(string userIpAddress, 
        string timeZone)
    {

        SaveCookies(GetCookieValues(userIpAddress).ShortLang, 
            null,
            userIpAddress, 
            HttpContext.Current, 
            timeZone, 
            null);

    }

    public static void SaveTimeZone(string userIpAddress,
        string timeZone, 
        HttpContext context)
    {

        SaveCookies(GetCookieValues(userIpAddress).ShortLang, 
            null,
            userIpAddress, 
            context, 
            timeZone,
            null);

    }

    public static void SaveCookies(string shortlang,
        Guid? searchMatchID,
        string userIpAddress, 
        HttpContext context, 
        string timeZone,
        Guid? userID)
    {

        if (context == null)
        {
            context = HttpContext.Current;
        }

        // Get UserIpAddress
        string getUserIp = userIpAddress;

        // Get Cookie
        HttpCookie getCookie = context.Request.Cookies[cookieName];

        // Set values
        HttpCookie createCookie = new HttpCookie(cookieName);
        createCookie.Values.Add("ShortLang", shortlang.ToString());
        createCookie.Values.Add("IPAddress", getUserIp.EncodeToMd5());
        if (context.Request.Cookies.Get(cookieName) == null) {
            if (userID.HasValue)
            {
                createCookie.Values.Add("UserId", userID.ToString());
            }
            else
            {
                createCookie.Values.Add("UserId", Guid.NewGuid().ToString());
            }
            createCookie.Values.Add("DateAdded", DateTime.UtcNow.Ticks.ToString());
        } else
        {
            createCookie.Values.Add("DateAdded", getCookie.Values["DateAdded"]);
            if (userID.HasValue)
            {
                createCookie.Values.Add("UserId", userID.ToString());
            }
            else
            {
                createCookie.Values.Add("UserId", getCookie.Values["UserId"]);
            }
            
        }

        if (searchMatchID.HasValue) {
            createCookie.Values.Add("SearchMatchID", searchMatchID.ToString());
        } else
        {
            if (context.Request.Cookies.Get(cookieName) != null) {
                if (!string.IsNullOrEmpty(getCookie.Values["SearchMatchID"]))
                {
                    createCookie.Values.Add("SearchMatchID", getCookie.Values["SearchMatchID"]);
                }
            }
        }

        if (!string.IsNullOrEmpty(timeZone))
        {
            createCookie.Values.Add("TimeZone", HttpUtility.UrlEncode(timeZone));
        }
        else
        {
            if (context.Request.Cookies.Get(cookieName) != null)
            {
                if (!string.IsNullOrEmpty(getCookie.Values["TimeZone"]))
                {
                    createCookie.Values.Add("TimeZone", getCookie.Values["TimeZone"]);
                }
            }
        }

        createCookie.Path = "/";
        createCookie.Secure = false;
        createCookie.HttpOnly = true;
        createCookie.Domain = GetDomain.GetDomainFromUrl(context.Request.Url.ToString());


        // Create or Set cookie
        if (context.Request.Cookies.Get(cookieName) != null) {
            context.Response.Cookies.Set(createCookie);
        } else {
            context.Response.Cookies.Add(createCookie);
        }
        
    }

    public static void DeleteCookies()
    {

        HttpContext.Current.Request.Cookies.Remove(cookieName);

    }

    public static void DeleteCookies(HttpContext context) {

        context.Request.Cookies.Remove(cookieName);

    }
}
