using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;

/// <summary>
/// Removes auto-generated style="border-width:0px;" from Image so that a border can be applied through CSS
/// </summary>
public class PanelFix : Panel {

    public override string BackImageUrl
    {
        get
        {
            return base.BackImageUrl;
        }
        set
        {
            if (value != null) {

                base.BackImageUrl = ResolveUrl(value);
            }
        }
    }


}
