using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;

/// <summary>
/// Removes auto-generated style="border-width:0px;" from Image so that a border can be applied through CSS
/// </summary>
public class BorderlessImage : System.Web.UI.WebControls.Image
{

    // Fixing imagecontrol bug
    public override string ImageUrl {
        get {
            return base.ImageUrl;
        }
        set {
            if (value != null)
            {
                
                base.ImageUrl = ResolveUrl(value);
            }
        }
    }

    public override string AlternateText {
        get {
            if (string.IsNullOrEmpty(base.AlternateText)) {
                return base.ImageUrl;
            } else {
                return base.AlternateText;
            }
        }
        set {
            base.AlternateText = value;
        }
    }

    public override Unit BorderWidth
    {
        get
        {
            if (base.BorderWidth.IsEmpty)
            {
                return Unit.Pixel(0);
            }
            else
            {
                return base.BorderWidth;
            }
        }
        set
        {
            base.BorderWidth = value;
        }
    }
}
