using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using Amazon.S3;
using Amazon;
using Amazon.CloudFront;
using Amazon.S3.Model;
using Amazon.S3.Util;
using System.IO;
using System.Text;
using System.IO.Compression;
using System.Globalization;
using System.Net;

/// <summary>
/// Summary description for HostNameOptimze
/// </summary>
    public static class HostNameOptimze {

    private static string CurrentUserIP = "62.107.21.129";

    private static HttpContext context = HttpContext.Current;
    private static HttpRequest _request = context.Request;
    private static bool _isLocal = _request.IsLocal;
    //private static string _amazonBucket = "searchwar2";
    private static string _amazonID = "id here";
    private static string _amazonSecretKey = "key here";
    private static string _bucketServerName = "searchwar-eu";


    private static Boolean AcceptGzip()
    {
        string gzip = _request.Headers["Accept-Encoding"];

        if (!string.IsNullOrEmpty(gzip))
        {
            if (gzip.Contains("gzip")) {
                return true;
            }
        }

        return true;
    }


        private static string P
        {
            get
            {
                string protocol = "http://";
                    return protocol;
            }
        }


        public static void AmazonUpload(string virtuelpath)
        {
            if (context.Cache[virtuelpath + _bucketServerName + "-file"] == null)
            {

                string filePath = context.Request.MapPath(virtuelpath);
                string contentType = "image/" + Path.GetExtension(filePath).Replace(".", null);
                // Create a signature for this operation
                PutObjectRequest putObjReq = new PutObjectRequest();
                putObjReq.WithBucketName(_bucketServerName);
                putObjReq.WithContentType(contentType);
                putObjReq.WithFilePath(filePath);
                putObjReq.WithKey(Path.GetFileName(filePath));
                putObjReq.WithCannedACL(S3CannedACL.PublicRead);
                var headers = new System.Collections.Specialized.NameValueCollection();
                headers.Add("Expires", TimeZoneManager.DateTimeNow.AddMonths(6).ToString("ddd, dd MMM yyyy HH:mm:ss K"));


                putObjReq.AddHeaders(headers);


                //// COMPRESS file
                //MemoryStream ms = new MemoryStream();

                //using (GZipStream zip = new GZipStream(ms, CompressionMode.Compress, true))
                //{
                //    byte[] buffer = File.ReadAllBytes(filePath);
                //    zip.Write(buffer, 0, buffer.Length);
                //    zip.Flush();
                //}

                //ms.Position = 0;
                //// Create a signature for this operation
                //PutObjectRequest putObjReqGZ = new PutObjectRequest();
                //putObjReqGZ.WithBucketName(_bucketServerName);
                //putObjReqGZ.WithContentType(contentType);
                //putObjReqGZ.WithInputStream(ms);
                //putObjReqGZ.WithKey(Path.GetFileName(filePath) + ".gz");
                //putObjReqGZ.WithCannedACL(S3CannedACL.PublicRead);
                //var headersGZ = new System.Collections.Specialized.NameValueCollection();
                //headersGZ.Add("Content-Encoding", "gzip");
                //headersGZ.Add("Expires", TimeZoneManager.DateTimeNow.AddMonths(6).ToString("ddd, dd MMM yyyy HH:mm:ss K"));


                //putObjReqGZ.AddHeaders(headersGZ);

                // connect client
                AmazonS3 s3Client = AWSClientFactory.CreateAmazonS3Client(_amazonID, _amazonSecretKey);
                s3Client.PutObject(putObjReq); // upload file
                //s3Client.PutObject(putObjReqGZ); // upload file

                context.Cache.Add(virtuelpath.GetHashCode() + _bucketServerName + "-file", "isUploaded",
                      null,
                      TimeZoneManager.DateTimeNow.AddDays(30),
                      System.Web.Caching.Cache.NoSlidingExpiration,
                      System.Web.Caching.CacheItemPriority.Normal,
                      null);


                // clean amazon
                s3Client.Dispose();
                //ms.Close();
                //ms.Flush();
                //ms.Dispose();
            }
        }


        public static void AmazonUploadContent(string virtuelpath)
        {

            if (context.Cache[virtuelpath + _bucketServerName + "-content"] == null)
            {
                Uri path = new Uri(GetSiteRoot() + "/" + virtuelpath);
                string q = path.Query;
                string qp = HttpUtility.ParseQueryString(q).Get("p");
                string qpageId = "-" + qp;

                string source = null;
                    // Create a request using a URL
                    WebClient wrGETURL = new System.Net.WebClient();
                    wrGETURL.CachePolicy = new System.Net.Cache.RequestCachePolicy(System.Net.Cache.RequestCacheLevel.NoCacheNoStore);
                    wrGETURL.UseDefaultCredentials = false;
                    wrGETURL.Encoding = Encoding.UTF8;

                    source = wrGETURL.DownloadString(path);


                    wrGETURL.Dispose();
                    wrGETURL.CancelAsync();

                string contentType = "text/javascript";
                // Create a signature for this operation
                PutObjectRequest putObjReq = new PutObjectRequest();
                putObjReq.WithBucketName(_bucketServerName);
                putObjReq.WithContentType(contentType);
                putObjReq.WithContentBody(source);
                putObjReq.WithKey(virtuelpath.Remove(0).GetHashCode() + "-" + qp + ".js");
                putObjReq.WithCannedACL(S3CannedACL.PublicRead);
                var headers = new System.Collections.Specialized.NameValueCollection();
                headers.Add("Expires", TimeZoneManager.DateTimeNow.AddMonths(6).ToString("ddd, dd MMM yyyy HH:mm:ss K"));

                putObjReq.AddHeaders(headers);


                // COMPRESS file
                MemoryStream ms = new MemoryStream();

                using (GZipStream zip = new GZipStream(ms, CompressionMode.Compress, true))
                {
                    byte[] buffer = Encoding.UTF8.GetBytes(source);
                    zip.Write(buffer, 0, buffer.Length);
                    zip.Flush();
                }

                ms.Position = 0;
                // Create a signature for this operation
                PutObjectRequest putObjReqGZ = new PutObjectRequest();
                putObjReqGZ.WithBucketName(_bucketServerName);
                putObjReqGZ.WithContentType(contentType);
                putObjReqGZ.WithInputStream(ms);
                putObjReqGZ.WithKey(virtuelpath.Remove(0).GetHashCode() + "-" + qp + ".js.gz");
                putObjReqGZ.WithCannedACL(S3CannedACL.PublicRead);

                var headersGZ = new System.Collections.Specialized.NameValueCollection();
                headersGZ.Add("Content-Encoding", "gzip");
                headersGZ.Add("Expires", TimeZoneManager.DateTimeNow.AddMonths(6).ToString("ddd, dd MMM yyyy HH:mm:ss K"));

                putObjReqGZ.AddHeaders(headersGZ);

                // connect client
                AmazonS3 s3Client = AWSClientFactory.CreateAmazonS3Client(_amazonID, _amazonSecretKey);
                s3Client.PutObject(putObjReq); // upload file
                s3Client.PutObject(putObjReqGZ); // upload file

                context.Cache.Add(virtuelpath + _bucketServerName + "-content", "isUploaded",
                      null,
                      TimeZoneManager.DateTimeNow.AddDays(30),
                      System.Web.Caching.Cache.NoSlidingExpiration,
                      System.Web.Caching.CacheItemPriority.Normal,
                      null);


                // clean amazon
                s3Client.Dispose();
                ms.Close();
                ms.Flush();
                ms.Dispose();
            }
        }


        public static string ChangeToImageHost(this string virtuelpath)
        {
            virtuelpath = VirtualPathUtility.MakeRelative("~/", "~" + virtuelpath.Replace("~", null)).Replace("searchwar", "").Replace(".ashx", null);

            if (_isLocal)
            {
                return GetSiteRoot() + "/" + virtuelpath + ".ashx";

            } else
            {
                AmazonUpload(virtuelpath);
                return P + "d2eiw75x9n1bsg.cloudfront.net/" + Path.GetFileName(virtuelpath);
            }
        }

        public static string GetSiteRoot()
        {
            string port = System.Web.HttpContext.Current.Request.ServerVariables["SERVER_PORT"];
            if (port == null || port == "80" || port == "443")
                port = "";
            else
                port = ":" + port;

            string protocol = System.Web.HttpContext.Current.Request.ServerVariables["SERVER_PORT_SECURE"];
            if (protocol == null || protocol == "0")
                protocol = "http://";
            else
                protocol = "https://";

            string sOut = protocol + GetDomain.GetDomainFromUrl(context.Request.Url.ToString()) + port + System.Web.HttpContext.Current.Request.ApplicationPath;

            if (sOut.EndsWith("/"))
            {
                sOut = sOut.Substring(0, sOut.Length - 1);
            }

            return sOut;
        }



        public static string ChangeToJsHost(this string virtuelpath) {

            string fileGzType = null;
            if (AcceptGzip())
            {
                fileGzType = ".gz";
            }

            if (_isLocal || WebAppSettings.Get().OptimzeUrl == false)
            {
                return GetSiteRoot() + "/" + VirtualPathUtility.MakeRelative("~/", virtuelpath);
            }
            else
            {
                AmazonUploadContent(virtuelpath);

                Uri path = new Uri(GetSiteRoot() + "/" + VirtualPathUtility.MakeRelative("~/", virtuelpath));
                string q = path.Query;
                string qp = HttpUtility.ParseQueryString(q).Get("p");

                return P + "d2eiw75x9n1bsg.cloudfront.net/" + virtuelpath.Remove(0).GetHashCode() + "-" + context.Server.UrlEncode(qp) + ".js" + fileGzType;
            }

        }


        public static string ChangeToCssHost(this string virtuelpath)
        {

            if (_isLocal || WebAppSettings.Get().OptimzeUrl == false)
            {
                return virtuelpath;
            }
            else
            {
                return virtuelpath;
            }

        }


    }

