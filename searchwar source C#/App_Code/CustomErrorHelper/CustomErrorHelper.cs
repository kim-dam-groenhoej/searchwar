using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;
using System.Collections;
using System.Text.RegularExpressions;

/// <summary>
/// Get urls helper
/// </summary>

namespace SearchWar.CustomErrorHelper
{
    public class CustomErrorHelper
    {
        public CustomErrorHelper()
        {
            //
            // TODO: Add constructor logic here
            //
        }

        /// <summary>
        /// Get urls of required number chars
        /// </summary>
        /// <param name="SearchingUrl">That Url you want to search after</param>
        /// <param name="Database">Database(true) or webserver(false)</param>
        /// <returns>A List of HelperUrl objects</returns>
        public static List<HelperUrl> GetUrls(string MatchUrl, bool Database)
        {
            List<HelperUrl> GetAddedaspxFiles = new List<HelperUrl>();
            List<string> aspxFiles = new List<string>();
            List<string> Dirs = new List<string>();
            string aspxFile = null;
            string[] aspxFileChars = null;
            string MatchFile = null;
            string[] MatchFileChars = null;
            int NumberOfTrue;
            string ConvertPathToWeb;
            string AppPath;

            // Get AppFolder for website
            AppPath = HttpContext.Current.Request.ApplicationPath;

            // Database or webserver
            if (Database == true)
            {
                // !!DATABASE SEARCHING!!



            }
            else
            {
                // !!WEBSERVER SEARCHING!!

                // Get all aspx files from AppFolder and add to List
                aspxFiles.AddRange(Directory.GetFiles(HttpContext.Current.Request.MapPath(AppPath), "*.aspx"));
                // Get all Dirs in AppFolder and add to List
                Dirs.AddRange(Directory.GetDirectories(HttpContext.Current.Request.MapPath(AppPath)));

                // Get all aspx files in dirs
                foreach (string Dir in Dirs)
                {

                    // Get all aspx files and add to List
                    aspxFiles.AddRange(Directory.GetFiles(Dir, "*.aspx"));
                    
                }


                foreach (string File in aspxFiles)
                {
                    if (File != HttpContext.Current.Request.MapPath(HttpContext.Current.Request.AppRelativeCurrentExecutionFilePath))
                    {

                        HelperUrl CreateNewUrl = new HelperUrl();
                        ConvertPathToWeb = PhysicalToVirtual.Convert(File, true);
                        CreateNewUrl.Title = ConvertPathToWeb;
                        CreateNewUrl.Url = ConvertPathToWeb;

                        // Fixed url/filename values
                        aspxFile = Path.GetFileNameWithoutExtension(File);
                        aspxFileChars = Regex.Split(aspxFile, "");
                        MatchFile = Path.GetFileNameWithoutExtension(MatchUrl);
                        MatchFileChars = Regex.Split(MatchFile, "");

                        // Checking chars and search (SearchEngine)
                        NumberOfTrue = 0;
                        foreach (string Char in aspxFileChars)
                        {

                            if (NumberOfTrue == ((aspxFile.Length / 2) + 1) && GetAddedaspxFiles.Contains(CreateNewUrl).Equals(false))
                            {
                                // Add url in list
                                GetAddedaspxFiles.Add(CreateNewUrl);
                            }

                            if (MatchFile.Contains(Char))
                            {
                                NumberOfTrue++;
                            }
                        }

                    }
                }

            }

            return GetAddedaspxFiles;
        }
    }
}