using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;

/// <summary>
/// Summary description for MuchBetterFindControl
/// </summary>
public static class BetterFindControl {

    public static T MuchBetterFindControl<T>(this Control control, Func<T, bool> func) where T : Control {
        var result = false;
        var foundControl = control as T;

        if (foundControl != null) {
            result = func(foundControl);
        }

        if (control != null) {
            if (result)
                return control as T;

            if (control.HasControls()) {
                foreach (Control c in control.Controls) {
                    foundControl = c.MuchBetterFindControl<T>(func);
                    if (foundControl != null) return foundControl as T;
                }
            }

        }

        return null;
    }
}
