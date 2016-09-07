using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for TraceManager
/// </summary>
public class TraceManager
{
	public TraceManager()
	{
		//
		// TODO: Add constructor logic here
		//
	}

    public static void WriteTrace(bool enteringFunction)
    {
        HttpContext c = HttpContext.Current;

        if (!c.Trace.IsEnabled)
            return;

        string callingFunctionName = "Undetermined method";
        string action = enteringFunction ? "Entering" : "Exiting";

        try
        {
            //Determine the name of the calling function.
            var stackTrace =
                new System.Diagnostics.StackTrace();
            callingFunctionName =
                stackTrace.GetFrame(1).GetMethod().Name;
        }
        catch
        {
            
        }

        c.Trace.Write(action, callingFunctionName);
    }
}
