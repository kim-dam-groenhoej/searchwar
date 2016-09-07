using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Searchwar_netModel;

/// <summary>
/// Summary description for cSiteMapNode
/// </summary>

    public class cSiteMapNode
    {
        public cSiteMapNode()
        {
            
        }

        private int _SiteMapNodeID;
        private int? _SiteMapNodeSubID;
        private string _SiteMapNodePath;
        private string _SiteMapNodeRewrittedPath;
        private Guid _SiteMapNodeAddedUserId;
        private Guid _SiteMapNodeEditUserId;
        private bool _SiteMapNodeShow;
        private DateTime _SiteMapNodeAddedDate;
        private DateTime _SiteMapNodeEditDate;
        private string _SiteMapNodeTitle;
        private int _SiteMapNodeSort;
        private int _LangId;
        private IEnumerable<SW_SiteMapNodeRole> _siteMapNodeRoles;

        public int SiteMapNodeId
        {
            get { return _SiteMapNodeID; }
            set { _SiteMapNodeID = value; }
        }

        public int? SiteMapNodeSubId
        {
            get { return _SiteMapNodeSubID; }
            set { _SiteMapNodeSubID = value; }
        }

        public string SiteMapNodePath
        {
            get { return _SiteMapNodePath; }
            set { _SiteMapNodePath = value; }
        }

        public string SiteMapNodeRewrittedPath
        {
            get { return _SiteMapNodeRewrittedPath; }
            set { _SiteMapNodeRewrittedPath = value; }
        }

        public Guid SiteMapNodeAddedUserId
        {
            get { return _SiteMapNodeAddedUserId; }
            set { _SiteMapNodeAddedUserId = value; }
        }

        public Guid SiteMapNodeEditUserId
        {
            get { return _SiteMapNodeEditUserId; }
            set { _SiteMapNodeEditUserId = value; }
        }

        public bool SiteMapNodeShow
        {
            get { return _SiteMapNodeShow; }
            set { _SiteMapNodeShow = value; }
        }

        public DateTime SiteMapNodeAddedDate
        {
            get { return _SiteMapNodeAddedDate; }
            set { _SiteMapNodeAddedDate = value; }
        }

        public DateTime SiteMapNodeEditDate
        {
            get { return _SiteMapNodeEditDate; }
            set { _SiteMapNodeEditDate = value; }
        }

        public string SiteMapNodeTitle
        {
            get { return _SiteMapNodeTitle; }
            set { _SiteMapNodeTitle = value; }
        }

        public int SiteMapNodeSort
        {
            get { return _SiteMapNodeSort; }
            set { _SiteMapNodeSort = value; }
        }

        public int LangId
        {
            get { return _LangId; }
            set { _LangId = value; }
        }

        public IEnumerable<SW_SiteMapNodeRole> SiteMapNodeRoles
        {
            get { return _siteMapNodeRoles; }
            set { _siteMapNodeRoles = value; }
        }
    }
