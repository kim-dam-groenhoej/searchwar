using System;
using System.Reflection;

/// <summary>
/// Copyright 2010 © Kim Dam Grønhøj
/// Free use of code for Jesper Dam
/// </summary>
namespace SearchWar.ObjectHelper {
    public static class ObjectHelper {

        /// <summary>
        /// Convert anymous objects into AnonymousObjectCollection
        /// </summary>
        /// <param name="data">Objects</param>
        /// <returns>reutrn AnonymousObjectCollection with AnonymousObjects</returns>
        private static AnonymousObjectCollection TurnObjectIntoAnonymousObjectCollection(object data) {

            const BindingFlags attr = BindingFlags.Public | BindingFlags.Instance;
            var dict = new AnonymousObjectCollection();

                PropertyInfo[] prop = data.GetType().GetProperties(attr);

                if (prop != null)
                {
                    foreach (var property in prop)
                    {
                        if (property.CanRead)
                        {

                            AnonymousObject CreateObject = new AnonymousObject();
                            CreateObject.KeyName = property.Name;
                            CreateObject.SetValue(property.GetValue(data, null));

                            dict.Add(CreateObject);

                        }
                    }
                }
                else
                {
                    throw (new ArgumentNullException("AnonymousObjectCollection", "no propertys for " + attr));
                }

            return dict;

        }

        /// <summary>
        /// Convert objects into AnonymousObjectCollection with AnonymousObjects
        /// </summary>
        /// <param name="obj">Insert the objects</param>
        /// <returns>Return AnonymousObjectCollection with AnonymousObjects</returns>
        public static AnonymousObjectCollection ToAnonymousObjectCollection(this object obj) {

            return TurnObjectIntoAnonymousObjectCollection(obj);

        }

    }
}