using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SearchWar.CustomErrorHelper;
using System.Text;
using System.IO;

public partial class error404 : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        string ErrorUrl;

        // Set page url
        if (String.IsNullOrEmpty(Request.QueryString["aspxerrorpath"])) {

            ErrorUrl = Request.Url.AbsoluteUri;

        } else {

            ErrorUrl = Request.QueryString["aspxerrorpath"].ToString();

        }
        HyperUrlError.Text = ErrorUrl;
        HyperUrlError.NavigateUrl = ErrorUrl;

        // Bind examples
        GridViewHelper.DataSource = CustomErrorHelper.GetUrls(ErrorUrl, false);
        GridViewHelper.DataBind();
        

    }
}
