using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for AppSettingsObj
/// </summary>
public class AppSettingsObj
{
	public AppSettingsObj()
	{
		//
		// TODO: Add constructor logic here
		//
	}

    private bool _CacheAll;
    private bool _OptimzeUrl;

    public bool CacheAll
    {
        get
        {
            return _CacheAll;
        }
        set
        {
            _CacheAll = value;
        }
    }

    public bool OptimzeUrl
    {
        get
        {
            return _OptimzeUrl;
        }
        set
        {
            _OptimzeUrl = value;
        }
    }
}