using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;
using System.Text.RegularExpressions;

/// <summary>
/// Summary description for PhysicalToVirtual
/// </summary>
public class PhysicalToVirtual
{
	public PhysicalToVirtual()
	{
		//
		// TODO: Add constructor logic here
		//
	}

    public static string Convert(string VirtualPath, bool FullUrl)
    {
        string GetAppFolder;
        string GetVirtualPath;


        // Fixing path
        GetAppFolder = HttpContext.Current.Request.PhysicalApplicationPath;
        GetVirtualPath = VirtualPath.Replace(GetAppFolder, "").Replace(@"\", @"/");

        if (FullUrl == true)
        {
            GetVirtualPath = "~/" + GetVirtualPath;
        }
        else
        {
            GetVirtualPath = @"~/" + GetVirtualPath;
        }

        return GetVirtualPath;
    }
}
