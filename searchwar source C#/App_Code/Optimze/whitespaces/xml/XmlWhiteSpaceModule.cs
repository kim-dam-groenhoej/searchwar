using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text.RegularExpressions;

/// <summary>
/// Summary description for XmlWhiteSpaceModule
/// </summary>
public class XmlWhiteSpaceModule {
    public XmlWhiteSpaceModule() {
        //
        // TODO: Add constructor logic here
        //
    }

    public static string RemoveWhitespace(string xml) {
        Regex regex = new Regex(@">\s*<");
        xml = regex.Replace(xml, "><");

        return xml.Trim();
    }
}
