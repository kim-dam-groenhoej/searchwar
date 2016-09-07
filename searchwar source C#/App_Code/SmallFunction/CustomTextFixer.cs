using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for CustomTextFixer
/// </summary>
public class CustomTextFixer
{

    /// <summary>
    /// Make breaklines and generating html tags to tagnames
    /// </summary>
    /// <param name="text">string here</param>
    /// <returns>Return string</returns>
    public static string BreakLinesAndNoHtml(string text)
    {

        // Aendre html tags til tekst(Kodenavne)
        text = HttpContext.Current.Server.HtmlEncode(text);
        // Laver breaklines
        text = text.Replace(Environment.NewLine, "<br />");

        return text;
    }
}
