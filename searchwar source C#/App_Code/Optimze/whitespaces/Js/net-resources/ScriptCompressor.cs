#region Using

using System;
using System.IO;
using System.Web;
using System.Web.UI;
using System.Text;
using System.Text.RegularExpressions;
using System.IO.Compression;
using System.Web.Caching;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Collections.Generic;
using SearchWar.Optimize;
using System.Web.Compilation;

#endregion
/// <summary>
/// Code made by Mads Kristensen
/// </summary>
public class ScriptCompressorHandler : IHttpHandler
{
	private const int DAYS_IN_CACHE = 30;

    private const string CurrentUserIP = "62.107.21.129";

	/// <summary>
	/// Enables processing of HTTP Web requests by a custom 
	/// HttpHandler that implements the <see cref="T:System.Web.IHttpHandler"></see> interface.
	/// </summary>
	/// <param name="context">An <see cref="T:System.Web.HttpContext"></see> object that provides 
	/// references to the intrinsic server objects 
	/// (for example, Request, Response, Session, and Server) used to service HTTP requests.
	/// </param>
	public void ProcessRequest(HttpContext context)
	{
		string content = string.Empty;
        string getddd = "";
        if (context.Request.QueryString["p"] != null) {
            getddd = context.Request.QueryString["p"];
        }

        string getP = HttpUtility.UrlDecode(getddd);
        string pageId = "-" + HttpUtility.HtmlEncode(getP);

            content = GetJsCode(getP, pageId, context);

            if (!string.IsNullOrEmpty(content))
            {
                SetHeaders(content.GetHashCode(), context);

                string rEtag = context.Request.Headers["If-None-Match"];

                if (rEtag != "\"" + content.GetHashCode().ToString() + "\"")
                {

                    context.Response.Write(content);
                    context.Response.StatusCode = 200;

                    Compress(context);
                }
                else
                {
                    context.Response.SuppressContent = true;
                    context.Response.StatusCode = 304;
                    context.Response.StatusDescription = "Not Modified";
                    // Explicitly set the Content-Length header so the client doesn't wait for
                    // content but keeps the connection open for other requests
                    context.Response.AddHeader("Content-Length", "0");
                }


            }
	}


    public string GetJsCode(string getP, 
        string pageId, 
        HttpContext context)
    {

        string content = (string)context.Cache[pageId + "js"];

        if (content == null)
        {

            // Load all js scripts for web.config
            #region Search/Save path
            //Get the list of files specified in the FileSet
            List<string> fileNames = new List<string>();

            // Get CssPaths in web.config
            DataPageSection config =
                (DataPageSection)System.Configuration.ConfigurationManager.GetSection(
                                      "DataPageHolderGroup/DataPageHolder");

            // Load CssPaths in web.config
            if (config.enable)
            {
                JsElementCollection jsElements = new JsElementCollection();
                JsElementCollection masterJsElements = new JsElementCollection();
                // Find page css paths
                foreach (PageElement page in config.Pages)
                {
                    if (page.pagePath.ToLower() == getP.ToString().ToLower())
                    {
                        jsElements = page.Jss;
                    }

                    if (page.pagePath.ToLower() == "~/MasterPage.master".ToLower())
                    {
                        masterJsElements = page.Jss;
                    }

                }

                // Load master css paths
                if (masterJsElements != null)
                {
                    foreach (JsElement js in masterJsElements)
                    {
                        fileNames.Add(js.Path);
                    }
                }

                // Load css paths
                if (jsElements != null)
                {
                    foreach (JsElement js in jsElements)
                    {
                        fileNames.Add(js.Path);
                    }
                }
            }
            #endregion
            if (fileNames.Count > 0)
            {
                
                        //Write each files
                        foreach (string file in fileNames)
                        {


                            // Create a request using a URL
                            StreamReader getStream = File.OpenText(context.Request.MapPath(file));

                            content += getStream.ReadToEnd();

                            getStream.Dispose();
                            getStream.Close();

                        }

                fileNames.Clear();
            }


            content = StripWhitespace(content);
            var getCssCode = new SearchWar.Optimize.Css.ResourceHandler();

            string csscode = getCssCode.StripWhitespace2(getCssCode.GetCss(context, getP, ref pageId)).Replace("'", string.Empty);
            csscode = ChangeBackgroundImageUrls(csscode, "http://localhost");

            content += "\n function addCss(cssCode) {" +
                        "var styleElement = document.createElement('style');" +
                        "  styleElement.type = 'text/css';" +
                        "  if (styleElement.styleSheet) {" +
                        "    styleElement.styleSheet.cssText = cssCode;" +
                        "  } else {" +
                        "    styleElement.appendChild(document.createTextNode(cssCode));" +
                        "  }" + "  document.getElementsByTagName('head')[0].appendChild(styleElement);}" +
                        "addCss('" + csscode + "');";

            // bug fix
            context.Response.ContentType = "text/javascript";

            // add Cache
            context.Cache.Add(pageId + "js", content, null, TimeZoneManager.DateTimeNow.AddDays(DAYS_IN_CACHE),
                      System.Web.Caching.Cache.NoSlidingExpiration,
                      System.Web.Caching.CacheItemPriority.High,
                      null);

        }

        return content;
    }


    private string ChangeBackgroundImageUrls(string csscode, string absolutePath) {
        string str = null;
         
        str = Regex.Replace(csscode, "url\\((.*?)\\)", delegate(Match match)
        {
            string v = match.ToString().Replace("url(", string.Empty).Replace(".ashx", "");
            v = v.Remove(v.Length - 1, 1);

            return "url(" + (v).ChangeToImageHost() + ")";
        }, RegexOptions.IgnoreCase);

        return str;
    }


    private static string P
    {
        get
        {
            string protocol = "http://";
            return protocol;
        }
    }

    /// <summary>
    /// Strips the whitespace from any .js file.
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

	/// <summary>
	/// This will make the browser and server keep the output
	/// in its cache and thereby improve performance.
	/// </summary>
	private static void SetHeaders(int hash, HttpContext context)
	{
		context.Response.ContentType = "text/javascript";
		context.Response.Cache.VaryByHeaders["Accept-Encoding"] = true;
        context.Response.Cache.VaryByHeaders["If-Modified-Since"] = true;
        context.Response.Cache.VaryByHeaders["If-None-Match"] = true;

		context.Response.Cache.SetExpires(TimeZoneManager.DateTimeNow.ToUniversalTime().AddDays(DAYS_IN_CACHE));
		context.Response.Cache.SetCacheability(HttpCacheability.Public);
		context.Response.Cache.SetMaxAge(new TimeSpan(DAYS_IN_CACHE, 0, 0, 0));
		context.Response.Cache.SetRevalidation(HttpCacheRevalidation.AllCaches);
        context.Response.Cache.SetLastModified(TimeZoneManager.DateTimeNow.ToUniversalTime());
        context.Response.AppendHeader("ETag", "\"" + hash.ToString() + "\"");
		context.Response.Cache.SetETag("\"" + hash.ToString() + "\"");
	}

	#region Compression

	private const string GZIP = "gzip";
	private const string DEFLATE = "deflate";

	private static void Compress(HttpContext context)
	{
		if (context.Request.UserAgent != null && context.Request.UserAgent.Contains("MSIE 6"))
            return;

		if (IsEncodingAccepted(GZIP))
		{
			context.Response.Filter = new GZipStream(context.Response.Filter, CompressionMode.Compress);
			SetEncoding(GZIP);
		}
		else if (IsEncodingAccepted(DEFLATE))
		{
			context.Response.Filter = new DeflateStream(context.Response.Filter, CompressionMode.Compress);
			SetEncoding(DEFLATE);
		}
	}

	/// <summary>
	/// Checks the request headers to see if the specified
	/// encoding is accepted by the client.
	/// </summary>
	private static bool IsEncodingAccepted(string encoding)
	{
		return HttpContext.Current.Request.Headers["Accept-encoding"] != null && HttpContext.Current.Request.Headers["Accept-encoding"].ToLower().Contains(encoding);
	}

	/// <summary>
	/// Adds the specified encoding to the response headers.
	/// </summary>
	/// <param name="encoding"></param>
	private static void SetEncoding(string encoding)
	{
		HttpContext.Current.Response.AppendHeader("Content-encoding", encoding);
	}

	#endregion

	/// <summary>
	/// Gets a value indicating whether another request can use the <see cref="T:System.Web.IHttpHandler"></see> instance.
	/// </summary>
	/// <value></value>
	/// <returns>true if the <see cref="T:System.Web.IHttpHandler"></see> instance is reusable; otherwise, false.</returns>
	public bool IsReusable
	{
		get { return true; }
	}

}