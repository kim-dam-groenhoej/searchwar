#region Using

using System;
using System.IO;
using System.Web;
using System.Text;
using System.Linq;
using System.Text.RegularExpressions;
using System.Diagnostics;
using System.Security.Principal;

#endregion

/// <summary>
/// Removes whitespace from the webpage.
/// </summary>
namespace SearchWar.Optimize {
    public class ImageOptimzer : IHttpModule {

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
            if (app.Request.Url.AbsoluteUri.ToLower().Contains(".png") == true) {


                bool isCompressed = false;

                object obj = app.Context.Cache[app.Request.RawUrl + "-IsCompressed"];
                if (obj != null)
                {
                    isCompressed = (bool)obj;
                }

                if (isCompressed != true)
                {
                    // PNG file path.
                    string f = app.Request.MapPath(app.Request.RawUrl);
                    
                    // Run OPTIPNG with level 7 compression.
                    ProcessStartInfo info = new ProcessStartInfo
                                                {
                                                    FileName = app.Request.MapPath("~/optipng.exe"),
                                                    WindowStyle = ProcessWindowStyle.Hidden,
                                                    Arguments = "\"" + f + "\" -o7",
                                                    UseShellExecute = false,
                                                    RedirectStandardInput = true,
                                                    WorkingDirectory = app.Request.MapPath("~/")
                                                };

                    // Use Process for the application.
                    Process exe = Process.Start(info);
                    exe.WaitForExit();

                    exe.Dispose();
                    exe.Close();

                    app.Context.Cache.Add(app.Request.RawUrl + "-IsCompressed", true, null, TimeZoneManager.DateTimeNow.AddMonths(6),
                                          System.Web.Caching.Cache.NoSlidingExpiration,
                                          System.Web.Caching.CacheItemPriority.Normal,
                                          null);
                }

            }
        }

    }
}