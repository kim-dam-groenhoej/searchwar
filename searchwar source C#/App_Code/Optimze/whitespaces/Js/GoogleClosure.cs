using System.IO;
using System.Net;
using System.Web;
using System.Xml;
using System;
using System.Text;
using System.Text.RegularExpressions;

/// <summary>
/// A C# wrapper around the Google Closure Compiler web service.
/// </summary>
public class GoogleClosure
{
    private const string PostData = "js_code={0}&output_format=xml&output_info=compiled_code&compilation_level=SIMPLE_OPTIMIZATIONS";
	private const string ApiEndpoint = "http://closure-compiler.appspot.com/compile";

	/// <summary>
	/// Compresses the specified file using Google's Closure Compiler algorithm.
	/// <remarks>
	/// The file to compress must be smaller than 200 kilobytes.
	/// </remarks>
	/// </summary>
    /// <param name="virtuelpath">The virtuel file path to the javascript file to compress.</param>
	/// <returns>A compressed version of the specified JavaScript file.</returns>
    public string Compress(string virtuelpath)
	{
	    HttpContext context = HttpContext.Current;
	    string source = null;
        string compiledJsCode = null;
        StringWriter sw = null;
        XmlDocument xml = null;


        compiledJsCode = (string)context.Cache["GC-jscode" + virtuelpath];
        if (string.IsNullOrEmpty(compiledJsCode))
        {
            sw = new StringWriter();
            context.Server.Execute(virtuelpath, sw, false);
            source = sw.ToString();
            sw.Close();

            compiledJsCode = null;
            try
            {
                xml = CallApi(source);
                compiledJsCode = xml.SelectSingleNode("//compiledCode").InnerText;
            }
            catch
            {
                compiledJsCode = StripWhitespace(source);
            }


                // add Cache
                context.Cache.Add("GC-jscode" + virtuelpath, compiledJsCode, null, TimeZoneManager.DateTimeNow.AddDays(30),
                          System.Web.Caching.Cache.NoSlidingExpiration,
                          System.Web.Caching.CacheItemPriority.Normal,
                          null);
        
        }

        return compiledJsCode;
	}

    /// <summary>
    /// Compresses the specified file using Google's Closure Compiler algorithm.
    /// <remarks>
    /// The code must be smaller than 200 kilobytes.
    /// </remarks>
    /// </summary>
    /// <param name="code">The javascript code to compress.</param>
    /// <returns>A compressed version of the specified JavaScript file.</returns>
    public string CompressCode(string code) {
        XmlDocument xml = CallApi(code);

        return xml.SelectSingleNode("//compiledCode").InnerText;
    }

	/// <summary>
	/// Calls the API with the source file as post data.
	/// </summary>
	/// <param name="source">The content of the source file.</param>
	/// <returns>The Xml response from the Google API.</returns>
	private static XmlDocument CallApi(string source)
	{
		using (WebClient client = new WebClient())
		{
			client.Headers.Add("content-type", "application/x-www-form-urlencoded");
			string data = string.Format(PostData, HttpUtility.UrlEncode(source));
			string result = client.UploadString(ApiEndpoint, data);

			XmlDocument doc = new XmlDocument();
			doc.LoadXml(result);
			return doc;
		}
	}

    /// <summary>
    /// Strips the whitespace from any .js file.
    /// THIS FUNCTION IS ONLY USED IF GOOGLE DONT ACCEPT
    /// </summary>
    private static string StripWhitespace(string body) {
        string[] lines = body.Split(new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
        StringBuilder emptyLines = new StringBuilder();
        foreach (string line in lines) {
            string s = line.Trim();
            if (s.Length > 0 && !s.StartsWith("//"))
                emptyLines.AppendLine(s.Trim());
        }

        body = emptyLines.ToString();

        // remove C styles comments
        body = Regex.Replace(body, "/\\*.*?\\*/", String.Empty, RegexOptions.Compiled | RegexOptions.Singleline);
        //// trim left
        body = Regex.Replace(body, "^\\s*", String.Empty, RegexOptions.Compiled | RegexOptions.Multiline);
        //// trim right
        body = Regex.Replace(body, "\\s*[\\r\\n]", "\r\n", RegexOptions.Compiled | RegexOptions.ECMAScript);
        // remove whitespace beside of left curly braced
        body = Regex.Replace(body, "\\s*{\\s*", "{", RegexOptions.Compiled | RegexOptions.ECMAScript);
        // remove whitespace beside of coma
        body = Regex.Replace(body, "\\s*,\\s*", ",", RegexOptions.Compiled | RegexOptions.ECMAScript);
        // remove whitespace beside of semicolon
        body = Regex.Replace(body, "\\s*;\\s*", ";", RegexOptions.Compiled | RegexOptions.ECMAScript);
        // remove newline after keywords
        body = Regex.Replace(body, "\\r\\n(?<=\\b(abstract|boolean|break|byte|case|catch|char|class|const|continue|default|delete|do|double|else|extends|false|final|finally|float|for|function|goto|if|implements|import|in|instanceof|int|interface|long|native|new|null|package|private|protected|public|return|short|static|super|switch|synchronized|this|throw|throws|transient|true|try|typeof|var|void|while|with)\\r\\n)", " ", RegexOptions.Compiled | RegexOptions.ECMAScript);

        return body;
    }
}