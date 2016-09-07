using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Searchwar_netModel;

/// <summary>
/// Summary description for DatabaseLookup
/// </summary>
public class DatabaseLookup
{
	public DatabaseLookup()
	{
		//
		// TODO: Add constructor logic here
		//
	}

    private static Searchwar_netEntities _db = null;

    public static Searchwar_netEntities DB
    {
        get { return _db; }
        set
        {
            value.Connection.Open();
            _db = value;
        }
    }

    public static void CloseAndClean()
    {
        DB.Connection.Close();
        DB.Dispose();
    }
}