using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for ClientUserDateTime
/// </summary>
public class ClientUserDateTime
{
	public ClientUserDateTime()
	{
		//
		// TODO: Add constructor logic here
		//

	}

    public static void SaveClientDateTimeOffset(string offset)
    {
        
    }

    public static DateTime Now()
    {
        return TimeZoneManager.DateTimeNow;
    }
}
