using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for BlogObject
/// </summary>
public class BlogObject {
    public BlogObject() {
        //
        // TODO: Add constructor logic here
        //
    }

    private int _blogId;
    private string _blogTitle;
    private string _blogText;
    private DateTime _BlogAddedDate;
    private DateTime _BlogEditDate;
    private Guid _BlogAddedUserId;
    private Guid _BlogEditUserId;

    public int BlogId {
        get {
            return _blogId;
        }
        set {
            _blogId = value;
        }
    }

    public string BlogTitle {
        get {
            return _blogTitle;
        }
        set {
            _blogTitle = value;
        }
    }

    public string BlogText {
        get {
            return _blogText;
        }
        set {
            _blogText = value;
        }
    }

    public DateTime BlogAddedDate {
        get {
            return _BlogAddedDate;
        }
        set {
            _BlogAddedDate = value;
        }
    }

    public DateTime BlogEditDate {
        get {
            return _BlogEditDate;
        }
        set {
            _BlogEditDate = value;
        }
    }

    public Guid BlogAddedUserId {
        get {
            return _BlogAddedUserId;
        }
        set {
            _BlogAddedUserId = value;
        }
    }

    public Guid BlogEditUserId {
        get {
            return _BlogEditUserId;
        }
        set {
            _BlogEditUserId = value;
        }
    }
}
