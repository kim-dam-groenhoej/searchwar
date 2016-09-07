using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using System.Web;

/// <summary>
/// Summary description for ControlNameOptimze
/// </summary>
public class ControlNameOptimze : System.Web.UI.Control {
    public ControlNameOptimze() {
        //
        // TODO: Add constructor logic here
        //
        
    }

    private int _controlId = 0;

    /// <summary>
    /// Override to force simple IDs all around
    /// </summary>
    public override string ID {
        get {
            _controlId += 1;
            return _controlId.ToString();
        }

    }

}
