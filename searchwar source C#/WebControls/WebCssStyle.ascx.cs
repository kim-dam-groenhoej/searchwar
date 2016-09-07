using System;
using System.Collections.Generic;
using System.Net.Mime;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;

public partial class WebControls_WebCssStyle : System.Web.UI.UserControl {
    protected void Page_Load(object sender, EventArgs e) {

    }

    private string _cssCode;
    private string _media = "media";
    private string _type = "text/css";
    
    public string CssCode
    {
        get
        {
            return _cssCode;
        }
        set
        {
            _cssCode = value;
        }
    }

    public string Media
    {
        get
        {
            return CssMultiStyle.Attributes["media"];
        }
        set
        {
            CssMultiStyle.Attributes["media"] = value;
        }
    }

    public string Type {
        get {
            return CssMultiStyle.Attributes["type"];
        }
        set {
            CssMultiStyle.Attributes["type"] = value;
        }
    }

    public override void DataBind() {

        CssMultiStyle.InnerHtml = _cssCode;

        // fixing a strange bug (if not: the hold output code will be CSS)
        Response.ContentType = "text/HTML";


        base.DataBind();
    }
}
