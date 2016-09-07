using System;
using System.Linq;
using System.Web;
using System.IO;
using System.Globalization;
using System.Threading;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO.Compression;
using SearchWar.ViewStateOptimze;
using System.Web.Configuration;
using System.Text.RegularExpressions;
using System.Xml.Linq;
using System.Xml;
using Searchwar_netModel;

/// <summary>
/// Some of this code is by Bilal Haidar and ScotGu
/// </summary>
public static class Compressor {

    public static byte[] Compress(byte[] data) {
        using (MemoryStream output = new MemoryStream()) {
            using (GZipStream gzip = new GZipStream(output, CompressionMode.Compress, true)) {
                gzip.Write(data, 0, data.Length);
                gzip.Close();
                return output.ToArray();
            }
        }
    }

    public static byte[] Decompress(byte[] data) {
        using (MemoryStream input = new MemoryStream()) {
            input.Write(data, 0, data.Length);
            input.Position = 0;
            using (GZipStream gzip = new GZipStream(input, CompressionMode.Decompress, true)) {
                using (MemoryStream output = new MemoryStream()) {
                    byte[] buff = new byte[64];
                    int read = -1;
                    read = gzip.Read(buff, 0, buff.Length);
                    while (read > 0) {
                        output.Write(buff, 0, read);
                        read = gzip.Read(buff, 0, buff.Length);
                    }
                    gzip.Close();
                    return output.ToArray();
                }
            }
        }
    }
}

/// <summary>
/// Summary description for BasePage
/// </summary>
public class BasePage : System.Web.UI.Page
{

    /// <summary>
    /// Stores the HiddenField key representing the GUID-FileName of the ViewState of the current page
    /// </summary>
    public string VsKey {
        get {
            HiddenField vsHiddenKey = null;
            string vsKey = null;

            // Get the HiddenField Key from the page
            vsHiddenKey = FindControlRecursive(this, "vsKey") as HiddenField;

            // Get the HiddenField value from the page
            string vsHiddenKeyValue = GetControlValue(vsHiddenKey.UniqueID.ToString());
            if (vsHiddenKeyValue != null) {
                vsKey = vsHiddenKeyValue;
            }

            // First time access, generate a key for the ViewState file
            if (!Page.IsPostBack) {
                vsKey = GenerateGuid();
                vsHiddenKey.Value = vsKey;
            }

            // Return the VS key
            return vsKey;
        }
    }

    #region Helper Methods for viewstate
    /// <summary>
    /// Finds a control recursively
    /// </summary>
    /// <param name="pRoot"></param>
    /// <param name="pControlId"></param>
    /// <returns></returns>
    public static Control FindControlRecursive(Control pRoot, string pControlId) {
        if (pRoot.ID == pControlId) {
            return pRoot;
        }

        foreach (Control c in pRoot.Controls) {
            Control t = FindControlRecursive(c, pControlId);
            if (t != null) {
                return t;
            }
        }

        return null;
    }

    /// <summary>
    /// Returns the value of the posted back control
    /// </summary>
    /// <param name="pUniqueId"></param>
    /// <returns></returns>
    public static string GetControlValue(string pUniqueId) {
        return System.Web.HttpContext.Current.Request.Form[pUniqueId];
    }
    #endregion

    #region ViewState Handling
    /// <summary>
    /// Saves the view state to the Web server file system.
    /// </summary>
    protected override void SavePageStateToPersistenceMedium(object state) {

        // save the view state to disk based on the user's ID and the URL

        // serialize the view state into a base-64 encoded string
        StringWriter stringWriter = new StringWriter();

        System.Web.UI.LosFormatter los = new System.Web.UI.LosFormatter();
        StringWriter sw = new StringWriter();
        los.Serialize(sw, state);
        string vs = sw.ToString();
        byte[] bytes = Convert.FromBase64String(vs);
        // bytes = Compressor.Compress(bytes);

        System.Web.UI.ScriptManager sm = System.Web.UI.ScriptManager.GetCurrent(this);
        if (sm != null && sm.IsInAsyncPostBack) {
            System.Web.UI.ScriptManager.RegisterHiddenField(this, "__VSTATE", vs);
        }

        String folderPath = Path.Combine(Request.PhysicalApplicationPath, "PersistedViewState");
        // check folder exists
        if (Directory.Exists(folderPath) == false)
        {
            Directory.CreateDirectory(folderPath);
        }

        // save the string to disk
        StreamWriter streamWriter = File.CreateText(ViewStateFilePath);
        streamWriter.Write(Convert.ToBase64String(bytes));
        streamWriter.Close();
        sw.Close();

    }

    /// <summary>
    /// Loads the page's view state from the Web server's file system.
    /// </summary>
    protected override object LoadPageStateFromPersistenceMedium() {

        // determine the file to access
        if (!File.Exists(ViewStateFilePath))
            return null;
        else {   // Remove all files that have been there for X days!
            CleanupFiles();

            // open the file
            StreamReader streamReader = File.OpenText(ViewStateFilePath);
            string state = streamReader.ReadToEnd();
            streamReader.Close();

            // deserialize the string
            byte[] bytes = Convert.FromBase64String(state);
            // bytes = Compressor.Decompress(bytes);   

            return (new System.Web.UI.LosFormatter().Deserialize(Convert.ToBase64String(bytes)));
        }
    }

    /// <summary>
    /// The path for this user's/page's view state information.
    /// </summary>
    public string ViewStateFilePath {
        get {
            string folderName = Path.Combine(Request.PhysicalApplicationPath, "PersistedViewState");
            string fileName = VsKey + "-" + Path.GetFileNameWithoutExtension(Request.Path).Replace("/", "-") + ".vs";
            return Path.Combine(folderName, fileName);
        }
    }

    /// <summary>
    /// A GUID is created to store the file names
    /// </summary>
    private string GenerateGuid() {
        return System.Guid.NewGuid().ToString("B");
    }

    /// <summary>
    /// Clean up files to gain space on the server
    /// </summary>
    private void CleanupFiles() {
        try { 

            // Create a reference to the folder holding all viewstate files
            string folderName = Path.Combine(Request.PhysicalApplicationPath, "PersistedViewState");

            // Create a reference to the VS directory
            DirectoryInfo directory = new DirectoryInfo(folderName);

            // Create an array representing the files in the current directory.
            FileInfo[] files = directory.GetFiles();

            // Delete all files whose CreationTime is > X days.
            // By default its 7 days, 1 week.
            DateTime threshold = TimeZoneManager.DateTimeNow.AddDays(-2);

            // Read the value from web.config
            if (WebConfigurationManager.AppSettings["vsDays"] != null) {
                if (WebConfigurationManager.AppSettings["vsDays"].ToString() != "") {
                    int vsDays = Convert.ToInt32(WebConfigurationManager.AppSettings["vsDays"].ToString()) * (-1);
                    threshold = TimeZoneManager.DateTimeNow.AddDays(vsDays);
                }
            }

            // for each file whose creation time is less then
            // or equal to the threshold, delete it.
            foreach (FileInfo file in files) {
                if (file.CreationTime <= threshold)
                    file.Delete();
            } 

        } catch (Exception ex) {
            throw new ApplicationException("CleanupFiles in PageBase");
        }
    }
    #endregion

}

