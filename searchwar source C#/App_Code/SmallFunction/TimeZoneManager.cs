using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for TimeZoneManager
/// </summary>
public class TimeZoneManager
{
	public TimeZoneManager(string currentUserIP)
	{
        _getCookie = ANOProfile.GetCookieValues(currentUserIP);
	}

    private ANO_User _getCookie;

    public DateTime ConvertDateTimeToUtc(DateTime dateTime)
    {

        TimeZoneInfo timeZoneInfo = null;


        if (string.IsNullOrEmpty(_getCookie.TimeZone))
        {
            timeZoneInfo = TimeZoneInfo.Utc;
        }
        else
        {
            foreach (TimeZoneInfo zoneinfo in TimeZoneInfo.GetSystemTimeZones())
            {
                if (zoneinfo.BaseUtcOffset.ToString() == _getCookie.TimeZone)
                {

                    timeZoneInfo = zoneinfo;
                    break;
                }
            }

        }

        return TimeZoneInfo.ConvertTimeToUtc(dateTime,
            timeZoneInfo);

    }

    public DateTime ConvertDateTimeFromUtc(DateTime dateTime) {

        TimeZoneInfo timeZoneInfo = null;

        if (string.IsNullOrEmpty(_getCookie.TimeZone))
        {
            timeZoneInfo = TimeZoneInfo.Utc;
        }
        else
        {
            foreach (TimeZoneInfo zoneinfo in TimeZoneInfo.GetSystemTimeZones()) {
                if (zoneinfo.BaseUtcOffset.ToString() == _getCookie.TimeZone) {

                    timeZoneInfo = zoneinfo;
                    break;
                }
            }

        }

        return TimeZoneInfo.ConvertTimeFromUtc(dateTime, 
            timeZoneInfo);

    }

    public static DateTime DateTimeNow
    {
        get
        {
            return DateTime.UtcNow;
        }
    }
}