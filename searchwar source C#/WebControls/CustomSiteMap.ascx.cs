using System;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using SearchWar.SiteMap;
using SearchWar.ObjectHelper;

public partial class CustomSiteMap : System.Web.UI.UserControl
{
    protected void Page_Load(object sender, EventArgs e)
    {


    }


    #region dataSource

    private object _dataSource;
    private string _currentUserIP;

    /// <summary>
    /// Set/Get dataSourceId
    /// </summary>
    public object DataSource {
        get {

            return _dataSource;

        }
        set {

            _dataSource = value;

        }
    }

    public string CurrentUserIP
    {
        get {

            return _currentUserIP;

        }
        set {

            _currentUserIP = value;

        }       
    }


    /// <summary>
    /// Bind all data
    /// </summary>
    public override void DataBind() {

        _shortLang = ANOProfile.GetCookieValues(_currentUserIP).ShortLang;
     
        base.DataBind();
        this.RSiteMap.DataSource = _dataSource;
        this.RSiteMap.DataBind();

    } 
    #endregion


    #region selectCurrentPath
    private string _selectedColor;
    private int _menuFirstCharFix = 0;

    /// <summary>
    /// Set/Get selected color with Color HEX
    /// </summary>
    public string SelectedColor {
        get {

            return this._selectedColor;

        }
        set {

            this._selectedColor = value;

        }
    }

    // shortlang
    private string _shortLang = null;


    protected void CheckCurrentPath(object sender, RepeaterItemEventArgs e) {

        if (e.Item.ItemType == ListItemType.AlternatingItem || e.Item.ItemType == ListItemType.Item) {

            // Get dataitem SiteMapNodePath
            cSiteMapNode cSiteMap = (cSiteMapNode)e.Item.DataItem;
            if (cSiteMap == null) throw new NotImplementedException();

            string getPath = cSiteMap.SiteMapNodePath;
            int getLangId = cSiteMap.LangId;


            // Get Current SiteMapNodePath
            cSiteMapNode currentCSiteMap = new CustomSiteMapNode().GetCurrentSiteMapNode(getLangId);
            string getCurrentSiteMapNodePath = currentCSiteMap.SiteMapNodePath;


            // Check item SiteMapNodePath is current SiteMapNodePath
            if (getPath == getCurrentSiteMapNodePath) {

                // Change color
                HyperLink getCurrentHyper = (HyperLink)e.Item.FindControl("HyperNodePath");
                getCurrentHyper.Attributes.Add("class", "selected");

            }

            if (!string.IsNullOrEmpty(cSiteMap.SiteMapNodeRewrittedPath))
            {
                string rewrittedPath = cSiteMap.SiteMapNodeRewrittedPath;
                if (!string.IsNullOrEmpty(rewrittedPath))
                {
                    getPath = rewrittedPath;
                }
            }

            // change url on path (Now with shortlang)
            HyperLink hyperNodePath = (HyperLink) e.Item.FindControl("HyperNodePath");

            hyperNodePath.NavigateUrl = getPath;

        }

    } 
    #endregion
}