using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SearchWar.LangSystem;
using SearchWar.SearchEngine;
using SearchWar.BlogSystem;
using SearchWar.ObjectHelper;

public partial class _Default : BasePage
{
    // Current lang
    public static string CurrentLang = null;
    public static int CurrentLangId;
    // Current User
    private const string CurrentUserIP = "62.107.21.129";
    private readonly string CurrentUserCountry = LookUpService.GetCountry(CurrentUserIP);

    protected Boolean _timePickerHourType = false;

    protected void Page_Load(object sender, EventArgs e)
    {

        // Set values of current lang
        LangaugeSystem ls = new LangaugeSystem();
        CurrentLang = ls.CurrentLang;
        CurrentLangId = ls.CurrentLangId;
        
        if (CurrentLang.ToLower() == "da-dk")
        {
            _timePickerHourType = true;
        }

        if (!Page.IsPostBack)
        {


            // Create/bind search control
            Wsc.CurrentLang = CurrentLang;
            Wsc.CurrentLangId = CurrentLangId;
            Wsc.CurrentUserIP = CurrentUserIP;
            Wsc.DataBind();

            // Bind newest match searchs
            const int defaultFromLastSeconds = 10;
            // check cache
            List<NewestSearchObject> newestMatchSearchs = (List<NewestSearchObject>)Cache["default_newestMatchSearch"];
            
            if (newestMatchSearchs == null) {
                newestMatchSearchs = SearchEngine.GetNewestMatchSearchs(3, CurrentLangId, defaultFromLastSeconds);
                
                // add Cache
                Cache.Add("default_newestMatchSearch", newestMatchSearchs, null, TimeZoneManager.DateTimeNow.AddSeconds(defaultFromLastSeconds),
                          System.Web.Caching.Cache.NoSlidingExpiration,
                          System.Web.Caching.CacheItemPriority.Normal,
                          null);
            }
            
            Wnms.DataSource = newestMatchSearchs;
            Wnms.DataBind();

            // Bind blog news
            // Check cache of blog
            BlogObject newestBlog = (BlogObject)Cache["Default_Blogs" + CurrentLangId];
            if (newestBlog == null)
            {
                BlogSystem Bs = new BlogSystem();
                newestBlog = Bs.GetBlogs(CurrentLangId, 0, 1).FirstOrDefault();
                if (newestBlog != null)
                {
                    // add Cache for blog
                    Cache.Add("Default_Blogs" + CurrentLangId, newestBlog, null, TimeZoneManager.DateTimeNow.AddHours(1),
                              System.Web.Caching.Cache.NoSlidingExpiration,
                              System.Web.Caching.CacheItemPriority.Normal,
                              null);
                }
            }
            
            if (newestBlog != null) {

                H4BlogTitle.InnerText = newestBlog.BlogTitle;
                LblBlogDateAdded.Text = newestBlog.BlogAddedDate.ToString();
                LblBlogText.Text = newestBlog.BlogText + "... ";
            
            } else {
                PnlNewestBlog.Visible = false;
            }
        }
        
        
    }

}
