using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for CacheImages
/// </summary>
namespace SearchWar.Optimize {
    public class CacheImages : IHttpHandler
    {

        public bool IsReusable {
            get {
                return true;
            }
        }


            public void ProcessRequest(HttpContext context)
            {

                string filePath = context.Request.RawUrl.ToLower();

                var fileTypes = new List<string> {"png", "jpg", "jpeg", "gif"};
                var fileContentTypes = new List<string> {"image/png", "image/jpg", "image/jpeg", "image/gif"};

                string fileType = fileTypes.Single(f => filePath.Contains(f) == true);

                if (!string.IsNullOrEmpty(fileType) && filePath.Contains(".ashx"))
                {

                    string rEtag = context.Request.Headers["If-None-Match"];
                    DateTime lastWriteTime = File.GetLastWriteTime(filePath);

                    // cache settings
                    context.Response.Cache.VaryByHeaders["Accept-Encoding"] = true;
                    context.Response.Cache.SetExpires(TimeZoneManager.DateTimeNow.ToUniversalTime().AddDays(30));
                    context.Response.Cache.SetCacheability(HttpCacheability.Public);
                    context.Response.Cache.SetRevalidation(HttpCacheRevalidation.AllCaches);
                    context.Response.ContentType = fileContentTypes.Single<string>(t => t.Contains(fileType));
                    context.Response.Cache.VaryByHeaders["If-Modified-Since"] = true;
                    context.Response.Cache.SetLastModified(TimeZoneManager.DateTimeNow.ToUniversalTime());
                    context.Response.Cache.VaryByHeaders["If-None-Match"] = true;

                    context.Response.Cache.SetMaxAge(new TimeSpan(30, 0, 0, 0));
                    context.Response.Cache.SetRevalidation(HttpCacheRevalidation.AllCaches);
                    context.Response.AppendHeader("ETag", "\"" + lastWriteTime.GetHashCode().ToString() + "\"");
                    context.Response.Cache.SetETag("\"" + lastWriteTime.GetHashCode().ToString() + "\"");

                    if (rEtag != "\"" + lastWriteTime.GetHashCode().ToString() + "\"")
                    {

                        context.Response.WriteFile(context.Server.MapPath(filePath).Replace(".ashx", null));

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
