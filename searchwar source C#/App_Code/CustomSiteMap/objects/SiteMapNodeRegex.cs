using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for SiteMapNodeRegex
/// </summary>

    public class SiteMapNodeRegex
    {
        public SiteMapNodeRegex()
        {
            //
            // TODO: Add constructor logic here
            //
        }

        private string _regex;
        private string _tourl;
        private string _name;

        public string Name
        {
            get
            {
                return _name;
            }
            set
            {
                _name = value;
            }
        }

        public string ToUrl
        {
            get
            {
                return _tourl;
            }
            set
            {
                _tourl = value;
            }
        }

        public string Regex
        {
            get
            {
                return _regex;
            }
            set
            {
                _regex = value;
            }
        }
    }
