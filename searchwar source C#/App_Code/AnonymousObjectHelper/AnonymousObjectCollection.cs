using System;

/// <summary>
/// Copyright 2010 © Kim Dam Grønhøj
/// Free use of code for Jesper Dam
/// </summary>
namespace SearchWar.ObjectHelper {
    public class AnonymousObjectCollection : System.Collections.CollectionBase {
        public AnonymousObjectCollection() {
            //
            // TODO: Add constructor logic here
            //
        }

        /// <summary>
        /// Get AnonymousObject from collection
        /// </summary>
        /// <param name="keyName">Insert KeyName of AnonymousObject</param>
        /// <returns>return AnonymousObject</returns>
        public AnonymousObject GetAnonymousObject(string keyName) {

            int i = 0;
            for (i = 0; i < List.Count; i++) {
                var ao = (AnonymousObject)List[i];

                if (ao.KeyName != keyName) continue;

                return ao;
            }

            if (i == 0) {
                throw (new ArgumentNullException("AnonymousObjectCollection GetAnonymousObject - " + keyName + " dont exists", "No object with '" + keyName + "' exists"));
            }

            return null;

        }

        /// <summary>
        /// Get AnonymousObject from collection
        /// </summary>
        /// <param name="keyName">Insert KeyName of AnonymousObject</param>
        /// <returns>return AnonymousObject</returns>
        public AnonymousObject GetAnonymousObject(int index) {

            int i = 0;
            for (i = 0; i < List.Count; i++) {
                var ao = (AnonymousObject)List[i];

                if (List.IndexOf(ao) != index) continue;
                
                return ao;
            }

            if (i == 0) {
                throw (new ArgumentNullException("AnonymousObjectCollection GetAnonymousObject - " + index + " dont exists", "No object with '" + index + "' exists"));
            }

            return null;

        }

        /// <summary>
        /// Add AnoymousObject to collection
        /// </summary>
        /// <param name="aObject">Insert AnonymousObject</param>
        public void Add(AnonymousObject aObject) {

            if (List.Contains(aObject)) {
                throw (new ArgumentNullException("AnonymousObjectCollection Add - " + aObject.KeyName + " already exists", "this keyname '" + aObject.KeyName + "' already exists"));
            } else {
                List.Add(aObject);
            }

        }

        /// <summary>
        /// Remove AnonymousObject by index number
        /// </summary>
        /// <param name="index">index number</param>
        public void Remove(int index) {

            if (index > Count || index < 0) {
                throw (new ArgumentNullException("AnonymousObjectCollection Remove - Index not valid", "Index not valid"));
            } else {
                List.RemoveAt(index);
            }

        }

        public void Remove(string KeyName) {

            int i = 0;
            for (i = 0; i <=List.Count; i++) {
                var AO = (AnonymousObject)List[i];

                if (AO.KeyName != KeyName) continue;
                List.RemoveAt(List.IndexOf(AO));

            }

            if (i == 0) {
                throw (new ArgumentNullException("AnonymousObjectCollection Remove - " + KeyName + " dont exists", "No object with '" + KeyName + "' exists"));
            }

        }

        /// <summary>
        /// Get index of AnonymousObject
        /// </summary>
        /// <param name="keyName">KeyName of AnonymousObject</param>
        /// <returns>return index of AnoynmousObject (int)</returns>
        public int IndexOf(string keyName) {

            int i = 0;
            for (i = 0; i < List.Count; i++) {
                var ao = (AnonymousObject)List[i];

                if (ao.KeyName != keyName) continue;
                return List.IndexOf(ao);

            }

            if (i == 0) {
                throw (new ArgumentNullException("AnonymousObjectCollection IndexOf - Index not valid", "Index not valid"));
            }

            return 0;

        }
    }
}