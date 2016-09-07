using System;
using System.IO;
using System.Web;
using System.IO.Compression;
using System.Collections.Generic;
using System.Web.UI;
using System.Web.Compilation;
using System.Net;
using System.Text;

/// <summary>
/// Some of this code is made by Kurt - URL: http://kurtschindler.net/blog/post/Removing-whitespace-and-compressing-CSS-files-with-an-HTTP-Handler.aspx
/// </summary>
namespace SearchWar.Optimize.Css {
    public class ResourceHandler : IHttpHandler {

        public bool IsReusable {
            get {
                return true;
            }
        }

        public string GetCss(HttpContext context, 
            string getp, 
            ref string pageId)
        {
            string output = null;
            
                    #region Search/Save path
                    //Get the list of files specified in the FileSet
                    List<string> fileNames = new List<string>();

                    // Get CssPaths in web.config
                    DataPageSection config =
                        (DataPageSection)System.Configuration.ConfigurationManager.GetSection(
                                              "DataPageHolderGroup/DataPageHolder");

                    // Load CssPaths in web.config
                    if (config.enable) {
                        CssElementCollection cssElements = null;
                        CssElementCollection masterCssElements = null;
                        // Find page css paths
                        foreach (PageElement cssPage in config.Pages) {
                            if (cssPage.pagePath.ToLower() == getp.ToLower())
                            {
                                cssElements = cssPage.Csss;
                            }

                            if (cssPage.pagePath.ToLower() == "~/MasterPage.master".ToLower()) {
                                masterCssElements = cssPage.Csss;
                            }

                        }

                        // Load master css paths
                        if (masterCssElements != null) {
                            foreach (CssElement css in masterCssElements) {
                                if (css.Name != "CssIe6") {
                                    fileNames.Add(css.Path);
                                } else {
                                    // IE6 Fix
                                    if (context.Request.Browser.Type.Equals("IE6")) {
                                        fileNames.Add(css.Path);
                                        // Change pageid (create new cache for IE6)
                                        pageId += "-IE6";
                                    }
                                }
                            }
                        }

                        // Load css paths
                        if (cssElements != null) {
                            foreach (CssElement css in cssElements) {
                                fileNames.Add(css.Path);
                            }
                        }
                    }
                    #endregion

                    // loop all files
                    if (fileNames.Count > 0)
                    {

                        //Write each files
                        foreach (string file in fileNames)
                        {


                            // Create a request using a URL
                            StreamReader getStream = File.OpenText(context.Request.MapPath(file));

                            output += getStream.ReadToEnd();

                            getStream.Dispose();
                            getStream.Close();


                        }

                        fileNames.Clear();

                        return output;
                    }

                    return null;
        }

        private static string P
        {
            get
            {
                string protocol = "http://";
                return protocol;
            }
        }

        public void ProcessRequest(HttpContext context)
        {
            
            if (context.Request.Url.AbsoluteUri.Contains("multiCss.ashx?p="))
            {

                string getp = context.Request.QueryString["p"];
                string pageId = "-" + getp;
                
                string output = (string)context.Cache["cssMulti" + pageId];

                // stop saving paths if cached
                if (string.IsNullOrEmpty(output))
                {


                        // remove whitespace
                        output = StripWhitespace(GetCss(context, getp, ref pageId));

                        // Make sure css isn't empty
                        if (!string.IsNullOrEmpty(output))
                        {

                            // add Cache
                            context.Cache.Add("cssMulti" + pageId, output, null, TimeZoneManager.DateTimeNow.AddDays(30),
                                              System.Web.Caching.Cache.NoSlidingExpiration,
                                              System.Web.Caching.CacheItemPriority.Normal,
                                              null);
                        }

                    
                }

                string rEtag = context.Request.Headers["If-None-Match"];

                // cache settings
                context.Response.Cache.VaryByHeaders["Accept-Encoding"] = true;
                context.Response.Cache.SetExpires(TimeZoneManager.DateTimeNow.ToUniversalTime().AddDays(30));
                context.Response.Cache.SetCacheability(HttpCacheability.Public);
                context.Response.Cache.SetRevalidation(HttpCacheRevalidation.AllCaches);
                context.Response.Cache.SetLastModified(TimeZoneManager.DateTimeNow.ToUniversalTime());
                context.Response.ContentType = "text/css";
                context.Response.Cache.VaryByHeaders["If-Modified-Since"] = true;
                context.Response.Cache.VaryByHeaders["If-None-Match"] = true;

                context.Response.Cache.SetMaxAge(new TimeSpan(30, 0, 0, 0));
                context.Response.Cache.SetRevalidation(HttpCacheRevalidation.AllCaches);
                context.Response.AppendHeader("ETag", "\"" + output.GetHashCode().ToString() + "\"");
                context.Response.Cache.SetETag("\"" + output.GetHashCode().ToString() + "\"");

                if (rEtag != "\"" + output.GetHashCode().ToString() + "\"")
                {

                    //write out css
                    context.Response.Write(output);

                    // compress
                    Compress(context);

                } else
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


        /// <summary>
        /// Strips the whitespace from any .css file.
        /// </summary>
        public static string StripWhitespace(string body) {

            Miron.Web.MbCompression.CssMinifier createMinifier = new Miron.Web.MbCompression.CssMinifier();
            return createMinifier.Minify(new StringReader(body));
        }

        public string StripWhitespace2(string body)
        {

            Miron.Web.MbCompression.CssMinifier createMinifier = new Miron.Web.MbCompression.CssMinifier();
            return createMinifier.Minify(new StringReader(body));
        }


        #region Compression

        private const string GZIP = "gzip";
        private const string DEFLATE = "deflate";

        private static void Compress(HttpContext context) {
            if (context.Request.UserAgent != null && context.Request.UserAgent.Contains("MSIE 6"))
                return;

            if (IsEncodingAccepted(GZIP)) {
                context.Response.Filter = new GZipStream(context.Response.Filter, CompressionMode.Compress);
                SetEncoding(GZIP);
            } else if (IsEncodingAccepted(DEFLATE)) {
                context.Response.Filter = new DeflateStream(context.Response.Filter, CompressionMode.Compress);
                SetEncoding(DEFLATE);
            }
        }

        /// <summary>
        /// Checks the request headers to see if the specified
        /// encoding is accepted by the client.
        /// </summary>
        private static bool IsEncodingAccepted(string encoding) {
            return HttpContext.Current.Request.Headers["Accept-encoding"] != null && HttpContext.Current.Request.Headers["Accept-encoding"].Contains(encoding);
        }

        /// <summary>
        /// Adds the specified encoding to the response headers.
        /// </summary>
        /// <param name="encoding"></param>
        private static void SetEncoding(string encoding) {
            HttpContext.Current.Response.AppendHeader("Content-encoding", encoding);
        }

        #endregion
    }
}