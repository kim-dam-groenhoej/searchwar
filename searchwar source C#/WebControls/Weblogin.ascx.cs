using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Security;
using SearchWar.SiteMap;

public partial class WebControls_Weblogin : System.Web.UI.UserControl
{

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {

            int langid = new SearchWar.LangSystem.LangaugeSystem().CurrentLangId;

            CustomSiteMapNode csm = new CustomSiteMapNode();
            cSiteMapNode homeCSiteMapNode = csm.GetSiteMapNode(8, langid);
            string pathCreateUser = homeCSiteMapNode.SiteMapNodePath;
            if (!string.IsNullOrEmpty(homeCSiteMapNode.SiteMapNodeRewrittedPath))
            {
                pathCreateUser = homeCSiteMapNode.SiteMapNodeRewrittedPath;
            }

            cSiteMapNode homeCSiteMapNode2 = csm.GetSiteMapNode(9, langid);
            string pathForgotpw = homeCSiteMapNode2.SiteMapNodePath;
            if (!string.IsNullOrEmpty(homeCSiteMapNode2.SiteMapNodeRewrittedPath))
            {
                pathForgotpw = homeCSiteMapNode2.SiteMapNodeRewrittedPath;
            }

            ((HyperLink)UserLogin.FindControl("HyperCreateUser")).NavigateUrl = pathCreateUser;
            ((HyperLink)UserLogin.FindControl("HyperForgotPassword")).NavigateUrl = pathForgotpw;
        }


    }

    protected void Login(object sender, LoginCancelEventArgs e)
    {
        var getControl = (Login)sender;
        e.Cancel = FormsAuthentication.Authenticate(getControl.UserName, getControl.Password);
    }
}