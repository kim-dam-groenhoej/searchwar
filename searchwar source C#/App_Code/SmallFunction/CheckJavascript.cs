using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// http://msdn.microsoft.com/en-us/library/system.web.configuration.httpcapabilitiesbase.javascript.aspx
/// If the browser supports JavaScript but scripting is disabled through a security setting, the JavaScript property will return true but script will not execute on the browser.
/// 
/// The following code example shows how to determine whether the browser supports JavaScript.
/// </summary>
public class CheckJavascript {
    public CheckJavascript() {
        //
        // TODO: Add constructor logic here
        //
    }

    public bool IsActavted
    {
        get
        {
            return IsJavascriptActivated();
        }
    }

    private static bool IsJavascriptActivated()
    {
        System.Web.HttpBrowserCapabilities myBrowserCaps = HttpContext.Current.Request.Browser;
        return ((System.Web.Configuration.HttpCapabilitiesBase)myBrowserCaps).JavaScript;
    }
}
