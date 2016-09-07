using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;

/// <summary>
/// Removes auto-generated style="border-width:0px;" from ImageButton so that a border can be applied through CSS
/// </summary>
public class BorderlessImageButton : System.Web.UI.WebControls.ImageButton
{
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
