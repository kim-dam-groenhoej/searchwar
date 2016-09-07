using System;
using System.Web;

/// <summary>
/// Summary description for ANO_User
/// </summary>
public class ANO_User
{
	public ANO_User()
	{
		
	}

    public ANO_User(Guid userId, string userIpAddress)
    {
        string getCurrentUserIp = userIpAddress;

        

        if (getCurrentUserIp.Equals(_ipAddress))
        {


        }

    }

    private Guid _userId;
    private string _shortLang; // Short LanguageInfo
    private string _ipAddress;
    private DateTime _dateAdded; // Date when it was added
    private Guid? _searchMatchID;
    private string _timeZone;

    public Guid UserID
    {
        get
        {
            return _userId;
        }
        set
        {
            _userId = value;
        }
    }

    public string ShortLang
    {
        get
        {
            return _shortLang;
        }
        set
        {
            _shortLang = value;
        }
    }

    public string IPAddress
    {
        get
        {
            return _ipAddress;
        }
        set
        {

            if (!string.IsNullOrEmpty(value))
            {

                //// Check the ipaddress is correct
                //if (System.Text.RegularExpressions.Regex.IsMatch(value, @"\b(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\b").Equals(true))
                //{

                    _ipAddress = value;

                //}
                //else
                //{

                //    throw (new ArgumentException(_ipAddress + " is not a valid ip address", "Wrong ip address"));

                //}

            }
            else
            {
                throw (new ArgumentException("No IpAddress added", "Empty IpAddress"));
            }

        }
    }

    public Guid? SearchMatchID
    {
        get
        {
            return _searchMatchID;
        }
        set
        {
            _searchMatchID = value;
        }
    }

    public DateTime DateAdded {
        get
        {
            return _dateAdded;
        }
        set
        {
            _dateAdded = value;
        }
    }

    public string TimeZone
    {
        get {
            return _timeZone;
        }
        set
        {
            _timeZone = value;
        }
    }

}
