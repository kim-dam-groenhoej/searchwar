using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Profile;
using System.Web.Security;
using SearchWar.ActionProtection;

/// <summary>
/// DOS attack protection
/// </summary>
public class PageActionFixer : System.Web.UI.Page {
    public PageActionFixer() {
        //
        // TODO: Add constructor logic here
        //
    }
    
    protected override void OnInit(EventArgs e) {
        base.OnInit(e);
        
        // Check if revisit is valid or not
        if (!base.IsPostBack) {
            ANO_User cooike = ANOProfile.GetCookieValues(HttpContext.Current.Request.UserHostAddress);

            if (cooike != null) {
                if (!ActionValidator.IsValid(ActionValidator.ActionTypeEnum.ReVisit))
                    Response.End();
            } else {
                if (!ActionValidator.IsValid(ActionValidator.ActionTypeEnum.FirstVisit))
                    Response.End();
            }
        } else {
            // Limit number of postbacks
            if (!ActionValidator.IsValid(ActionValidator.ActionTypeEnum.Postback))
                Response.End();
        }

    }
}
