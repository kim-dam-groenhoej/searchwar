#region Using

using System;
using System.IO;
using System.Web;
using System.IO.Compression;
using System.Text;
using System.Linq;
using System.Text.RegularExpressions;

#endregion

/// <summary>
/// Removes whitespace from the webpage.
/// </summary>
namespace SearchWar.Optimize {
    public class WhitespaceModule : IHttpModule {

        #region IHttpModule Members

        void IHttpModule.Dispose() {
            // Nothing to dispose; 
        }

        void IHttpModule.Init(HttpApplication context) {
            context.BeginRequest += new EventHandler(context_BeginRequest);
        }

        #endregion

        void context_BeginRequest(object sender, EventArgs e) {
            HttpApplication app = sender as HttpApplication;
            if (app.Request.Path.Contains(".aspx") && app.Request.RawUrl.Contains("Default.js.aspx") == false) {

                
                

                if (app.Request.UserAgent != null && app.Request.UserAgent.Contains("MSIE 6"))
                {
                    // nothing
                } else {

                if (IsEncodingAccepted("gzip"))
                    {

                        app.Response.Filter = new GZipStream(app.Response.Filter,
                                                             CompressionMode.Compress);
                        SetEncoding("gzip");
                    }
                    else if (IsEncodingAccepted("deflate"))
                    {
                        app.Response.Filter = new DeflateStream(app.Response.Filter,
                                                                CompressionMode.Compress);
                        SetEncoding("deflate");
                    }
                }

                
                

            }
        }

        private bool IsEncodingAccepted(string encoding) {
            return HttpContext.Current.Request.Headers["Accept-encoding"] != null &&
                HttpContext.Current.Request.Headers["Accept-encoding"].Contains(encoding);
        }

        private void SetEncoding(string encoding) {
            HttpContext.Current.Response.AppendHeader("Content-encoding", encoding);
        }


        #region Stream filter

        private class WhitespaceFilter : Stream {

            public WhitespaceFilter(Stream sink) {
                _sink = sink;
            }

            private Stream _sink;
            private static Regex reg = new Regex(@"(?<=[^])\t{2,}|(?<=[>])\s{2,}(?=[<])|(?<=[>])\s{2,11}(?=[<])|(?=[\n])\s{2,}");

            #region Properites

            public override bool CanRead {
                get { return true; }
            }

            public override bool CanSeek {
                get { return true; }
            }

            public override bool CanWrite {
                get { return true; }
            }

            public override void Flush() {
                _sink.Flush();
            }

            public override long Length {
                get { return 0; }
            }

            private long _position;
            public override long Position {
                get { return _position; }
                set { _position = value; }
            }

            #endregion

            #region Methods

            public override int Read(byte[] buffer, int offset, int count) {
                return _sink.Read(buffer, offset, count);
            }

            public override long Seek(long offset, SeekOrigin origin) {
                return _sink.Seek(offset, origin);
            }

            public override void SetLength(long value) {
                _sink.SetLength(value);
            }

            public override void Close() {
                _sink.Close();
            }

            public override void Write(byte[] buffer, int offset, int count) {
                byte[] data = new byte[count];
                Buffer.BlockCopy(buffer, offset, data, 0, count);
                string html = System.Text.Encoding.Default.GetString(buffer);

                html = reg.Replace(html, string.Empty);
                html = Regex.Replace(html, @"(?<=[^])\t{2,}|(?<=[>])\s{2,3}(?=[<])|(?<=[>])\s{2,3}(?=[<])|(?=[\n])\s{2,3}", String.Empty);
                html = html.Replace(";\n", ";");

                /*(?<=((src=)|(href=))[^><]*?)&(?!.{1,4};)
                This part checks for & in href and src properties.
                 *
                */
                Regex allToBeChanged = new Regex("(?<=((src=)|(href=))[^><]*?)&(?!.{1,4};)", RegexOptions.Singleline | RegexOptions.IgnoreCase);
                html = allToBeChanged.Replace(html, new MatchEvaluator(this.GetReplaceText));

                byte[] outdata = System.Text.Encoding.Default.GetBytes(html);
                _sink.Write(outdata, 0, outdata.GetLength(0));
            }

            public string GetReplaceText(Match m) {
                string x = m.Value;
                switch (x) {
                    case " ": // it matches the last space in a img tag that doesn't have an alt tag
                        return " alt=\"\" ";
                    case "&":
                        return "&amp;";
                }
                //throw new Exception("There was no matching string, the regular expression needs fixing");
                return x;
            }

            #endregion

        }

        #endregion

    }
}