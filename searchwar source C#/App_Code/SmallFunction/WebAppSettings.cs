using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for WebAppSettings
/// </summary>
public class WebAppSettings
{
	public WebAppSettings()
	{
		//
		// TODO: Add constructor logic here
		//
	}

    public static AppSettingsObj Get()
    {

        AppSettingsObj obj = new AppSettingsObj();

        System.Configuration.Configuration rootWebConfig1 =
    System.Web.Configuration.WebConfigurationManager.OpenWebConfiguration(null);
        if (rootWebConfig1.AppSettings.Settings.Count > 0)
        {
            obj.CacheAll = Convert.ToBoolean(rootWebConfig1.AppSettings.Settings["CacheAll"]);

            obj.OptimzeUrl = Convert.ToBoolean(rootWebConfig1.AppSettings.Settings["OptimzeUrl"]);

        }

        return obj;
    }
}