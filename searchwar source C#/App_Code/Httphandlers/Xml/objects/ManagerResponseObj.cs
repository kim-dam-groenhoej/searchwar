using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml.Linq;

/// <summary>
/// Summary description for ManagerResponseObj
/// </summary>
public class ManagerResponseObj
{
	public ManagerResponseObj()
	{
		//
		// TODO: Add constructor logic here
		//
	}

    private XElement _xml;
    private object _DataObject;

    public XElement Xml
    {
        get
        {
            return _xml;
        }
        set
        {
            _xml = value;
        }
    }

    public object DataObject
    {
        get
        {
            return _DataObject;
        }
        set
        {
            _DataObject = value;
        }
    }
}