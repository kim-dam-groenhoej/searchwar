using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Reflection;
using System.Dynamic;
using System.ComponentModel;

/// <summary>
/// Summary description for AnonymousObjectHelper
/// </summary>
public static class AnonymousObjectHelper
{

    // Convert 'List of anonymous objects' with unknown properties
    public static List<dynamic> ConvertListAnoToExpa(this object anonymousObjects)
    {
        List<dynamic> results = new List<dynamic>();

        foreach (var item in (IEnumerable<object>)anonymousObjects)
        {
            IDictionary<string, Object> expando = new ExpandoObject();

            foreach (PropertyDescriptor property in TypeDescriptor.GetProperties(item.GetType()))
                expando.Add(property.Name, property.GetValue(item));


            results.Add((dynamic)expando);
        }

        return results;

    }

    // Convert an 'anonymous object' with unknown properties
    public static dynamic ConvertAnoToExpa(this object anonymousObject)
    {

            IDictionary<string, Object> expando = new ExpandoObject();

            foreach (PropertyDescriptor property in TypeDescriptor.GetProperties(anonymousObject.GetType()))
                expando.Add(property.Name, property.GetValue(anonymousObject));

            return (dynamic)expando;

    }
}