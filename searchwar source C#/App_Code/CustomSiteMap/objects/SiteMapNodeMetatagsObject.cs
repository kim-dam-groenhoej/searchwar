using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for SiteMapNodeMetatagsObject
/// </summary>
    public class SiteMapNodeMetatagsObject
    {
        public SiteMapNodeMetatagsObject()
        {
            //
            // TODO: Add constructor logic here
            //
        }

    private int _metaID;
    private DateTime _MetaTagAddedDate;
    private DateTime _MetaTagEditDate;
    private Guid _MetaTagAddedUserId;
    private Guid _MetaTagEditUserId;
    private int _LangId;

    public int MetaId1
    {
        get { return _metaID; }
        set { _metaID = value; }
    }

    public DateTime MetaTagAddedDate
    {
        get { return _MetaTagAddedDate; }
        set { _MetaTagAddedDate = value; }
    }

    public DateTime MetaTagEditDate
    {
        get { return _MetaTagEditDate; }
        set { _MetaTagEditDate = value; }
    }

    public Guid MetaTagAddedUserId
    {
        get { return _MetaTagAddedUserId; }
        set { _MetaTagAddedUserId = value; }
    }

    public Guid MetaTagEditUserId
    {
        get { return _MetaTagEditUserId; }
        set { _MetaTagEditUserId = value; }
    }

    public int LangId
    {
        get { return _LangId; }
        set { _LangId = value; }
    }

    private string _metaTagTitle;
        private string _metaTagDescription;
        private string _metaTagKeywords;
        private string _metaTagLanguage;
        private string _metaTagAuthor;
        private string _metaTagPublisher;
        private string _metaTagCopyright;
        private string _metaTagRevisitAfter;
        private string _metaTagRobots;
        private string _metaTagCache;
        private string _metaTagCacheControl;

    public int MetaId
    {
        get { return _metaID; }
        set { _metaID = value; }
    }

    public string MetaTagTitle
        {
            get { return _metaTagTitle; }
            set { _metaTagTitle = value; }
        }

        public string MetaTagDescription
        {
            get { return _metaTagDescription; }
            set { _metaTagDescription = value; }
        }

        public string MetaTagKeywords
        {
            get { return _metaTagKeywords; }
            set { _metaTagKeywords = value; }
        }

        public string MetaTagLanguage
        {
            get { return _metaTagLanguage; }
            set { _metaTagLanguage = value; }
        }

        public string MetaTagAuthor
        {
            get { return _metaTagAuthor; }
            set { _metaTagAuthor = value; }
        }

        public string MetaTagPublisher
        {
            get { return _metaTagPublisher; }
            set { _metaTagPublisher = value; }
        }

        public string MetaTagCopyright
        {
            get { return _metaTagCopyright; }
            set { _metaTagCopyright = value; }
        }

        public string MetaTagRevisitAfter
        {
            get { return _metaTagRevisitAfter; }
            set { _metaTagRevisitAfter = value; }
        }

        public string MetaTagRobots
        {
            get { return _metaTagRobots; }
            set { _metaTagRobots = value; }
        }

        public string MetaTagCache
        {
            get { return _metaTagCache; }
            set { _metaTagCache = value; }
        }

        public string MetaTagCacheControl
        {
            get { return _metaTagCacheControl; }
            set { _metaTagCacheControl = value; }
        }
    }
