using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.IO;
using SearchWar.SearchEngine;

public partial class WebControls_WebNewestSearchs : System.Web.UI.UserControl {
    protected void Page_Load(object sender, EventArgs e) {
        
    }

    private object _dataSource;

    public object DataSource
    {
        get
        {
            return _dataSource;
        }
        set
        {
            _dataSource = value;
        }
    }

    public override void DataBind() {
        base.DataBind();

        RNewestMatchSearchs.DataSource = _dataSource;
        RNewestMatchSearchs.DataBind();

        if (!(RNewestMatchSearchs.Items.Count > 0))
        {
            LblNullNewestSearchs.Visible = true;
        }

    }

    protected void BoundData(object sender, RepeaterItemEventArgs e) {

        if (e.Item.ItemType == ListItemType.AlternatingItem || e.Item.ItemType == ListItemType.Item)
        {
            // Data
            var DataItem = (NewestSearchObject)e.Item.DataItem;

            // Controls
            Label LblGameAndMode = (Label)e.Item.FindControl("LblGameAndMode");
            Label lblClanName = (Label)e.Item.FindControl("LblClanName");
            Label lblCountry = (Label)e.Item.FindControl("LblClanCountry");
            Label lblFromDate = (Label)e.Item.FindControl("LblFromDate");
            Image ImgTLD = (Image)e.Item.FindControl("ImgTLD");
            Label LblSearchPlayers = (Label)e.Item.FindControl("LblSearchPlayers");

            // Bind data
            LblGameAndMode.Text = DataItem.SearchGame.SearchWarGameName;
            if (DataItem.SearchGameType != null)
            {
                LblGameAndMode.Text += " - " + DataItem.SearchGameType.SearchWarGameTypeName;
            }

            string imgfile = ResolveUrl("~/flags/" + DataItem.ClanCountryData.SearchWarCountryTLD + ".gif");
            if (File.Exists(HttpContext.Current.Request.MapPath(imgfile)))
            {
                ImgTLD.ImageUrl = imgfile + ".ashx";
            } else
            {
                ImgTLD.Visible = false;
            }

            LblSearchPlayers.Text = DataItem.SearchXvs.ToString() + " / " + DataItem.SearchvsX.ToString();
            lblClanName.Text = string.IsNullOrEmpty(DataItem.ClanName) ? GetLocalResourceObject("LblClanName.Text").ToString() : DataItem.ClanName;
            lblCountry.Text = DataItem.ClanCountryData.SearchWarCountryName;
            lblFromDate.Text = DataItem.SearchMatchStart.ToString();

        }

    }
}
