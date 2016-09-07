using System;

/// <summary>
/// Copyright 2010 © Kim Dam Grønhøj
/// Free use of code for Jesper Dam
/// </summary>
namespace SearchWar.ObjectHelper {
    public class AnonymousObject {
        public AnonymousObject() {
            //
            // TODO: Add constructor logic here
            //
        }

        // Properties
        private string _keyName;
        private object _value;


        public string KeyName {
            get {
                return _keyName;
            }
            set {
                _keyName = value;
            }
        }

        public T GetValue<T>() {
            return (T)_value;
        }

        public object GetValue_UnknownObject() {
            return _value;
        }

        public void SetValue(object obj) {
            _value = obj;
        }
    }
}