using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.Adapters;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

namespace HyperLinkTest
{
    /// <summary>
    /// Summary description for HyperLinkControlAdapter
    /// </summary>
    public class HyperLinkControlAdapter : ControlAdapter
    {
        protected override void Render(HtmlTextWriter writer)
        {
            HyperLink hl = this.Control as HyperLink;
            if (hl == null)
            {
                base.Render(writer);
                return;
            }

            // This code is copied from HyperLink.RenderContents (using
            // Reflector). References to "this" have been changed to
            // "hl", and we have to render the begin and end tags.
            string imageUrl = hl.ImageUrl;
            if (imageUrl.Length > 0)
            {
                // Let the HyperLink render its begin tag
                hl.RenderBeginTag(writer);

                Image image = new Image();

                // I think the next line is the bug. The URL gets
                // resolved here, but the Image.UrlResolved property
                // doesn't get set. So another attempt to resolve the
                // URL is made in Image.AddAttributesToRender. It's in
                // the callstack above that method that the exception
                // or improperly resolved URL happens.
                //image.ImageUrl = base.ResolveClientUrl(imageUrl);
                image.ImageUrl = Page.ResolveUrl(imageUrl);

                imageUrl = hl.ToolTip;
                if (imageUrl.Length != 0)
                {
                    image.ToolTip = imageUrl;
                }

                imageUrl = hl.Text;
                if (imageUrl.Length != 0)
                {
                    image.AlternateText = imageUrl;
                }

                image.RenderControl(writer);

                // Wrap up by letting the HyperLink render its end tag
                hl.RenderEndTag(writer);
            }
            else
            {
                // HyperLink.RenderContents handles a couple of other
                // cases if its ImageUrl property hasn't been set. We
                // delegate to that behavior here.
                base.Render(writer);
            }
        }
    }
}
