using System;
using System.Collections;
using System.Text;
using System.Configuration;
using System.Xml;

namespace SearchWar.Optimize
{
    public class DataPageSection : ConfigurationSection
    {
        // Create a "enable" attribute.
        [ConfigurationProperty("enable", DefaultValue = true, IsRequired = false)]
        public bool enable
        {
            get
            {
                return (bool)this["enable"]; 
            }
            set
            {
                this["enable"] = value; 
            }
        }

        [ConfigurationProperty("Pages", IsDefaultCollection = false)]
        public PageElementCollection Pages {
            get
            {
                return (PageElementCollection)base["Pages"];
            }
        }
    }

    public class PageElementCollection
          : ConfigurationElementCollection {

        public PageElementCollection() {
        }

        public override ConfigurationElementCollectionType CollectionType {
            get {
                return
                    ConfigurationElementCollectionType.AddRemoveClearMap;
            }
        }

        public PageElement this[int index] {
            get { return (PageElement)base.BaseGet(index); }
        }

        public new PageElement this[string name] {
            get { return (PageElement)base.BaseGet(name); }
        }

        protected override ConfigurationElement CreateNewElement() {
            return new PageElement();
        }

        protected override object GetElementKey(
              ConfigurationElement element) {
            return ((PageElement)element).Name;
        }

        public int IndexOf(PageElement csspageelement) {
            return BaseIndexOf(csspageelement);
        }

        public void Add(PageElement csspageelement) {
            BaseAdd(csspageelement);
        }

        public void Remove(PageElement csspageelement) {
            if (BaseIndexOf(csspageelement) > 0) {
                BaseRemove(csspageelement.Name);
            }
        }

        public void RemoveAt(int index) {
            BaseRemoveAt(index);
        }

        public void Remove(string name) {
            BaseRemove(name);
        }

        public void Clear() {
            BaseClear();
        }

    }


    // Define the "PageElement" element
    // with "pagepath" attributes.
    public class PageElement : ConfigurationElement {

        [ConfigurationProperty("name", DefaultValue = "Unknown",
           IsKey = true, IsRequired = true)]
        [StringValidator(MinLength = 1, MaxLength = 50)]
        public String Name {
            get
            {
                return (String)base["name"];
            }
            set
            {
                base["name"] = value;
            }
        }

        [ConfigurationProperty("pagePath", DefaultValue = "~/", IsRequired = true)]
        [StringValidator(MinLength = 1, MaxLength = 700)]
        public string pagePath {
            get {
                return (string)this["pagePath"];
            }
            set {
                this["pagePath"] = value;
            }
        }

        [ConfigurationProperty("Csss", IsDefaultCollection = false)]
        public CssElementCollection Csss {
            get
            {
                return (CssElementCollection)base["Csss"];
            }
        }

        [ConfigurationProperty("Jss", IsDefaultCollection = false)]
        public JsElementCollection Jss {
            get {
                return (JsElementCollection)base["Jss"];
            }
        }
    }


    public class CssElementCollection
          : ConfigurationElementCollection {

        public CssElementCollection() {
        }

        public override ConfigurationElementCollectionType CollectionType {
            get {
                return
                    ConfigurationElementCollectionType.AddRemoveClearMap;
            }
        }

        public CssElement this[int index] {
            get { return (CssElement)base.BaseGet(index); }
        }

        public new CssElement this[string name] {
            get { return (CssElement)base.BaseGet(name); }
        }

        protected override ConfigurationElement CreateNewElement() {
            return new CssElement();
        }

        protected override object GetElementKey(
              ConfigurationElement element) {
            return ((CssElement)element).Name;
        }

        public int IndexOf(CssElement csselement) {
            return BaseIndexOf(csselement);
        }

        public void Add(CssElement csselement) {
            BaseAdd(csselement);
        }

        public void Remove(CssElement csselement) {
            if (BaseIndexOf(csselement) > 0) {
                BaseRemove(csselement.Name);
            }
        }

        public void RemoveAt(int index) {
            BaseRemoveAt(index);
        }

        public void Remove(string name) {
            BaseRemove(name);
        }

        public void Clear() {
            BaseClear();
        }

    }


    // Define the "CssElement" element
    // with "name" and "path" attributes.
    public class CssElement : ConfigurationElement
    {
        [ConfigurationProperty("name", DefaultValue = "Unknown",
           IsKey = true, IsRequired = true)]
        [StringValidator(MinLength = 1, MaxLength = 50)]
        public String Name {
            get
            {
                return (String)base["name"];
            }
            set
            {
                base["name"] = value;
            }
        }


        [ConfigurationProperty("path", DefaultValue = "~/", IsRequired = true)]
        [StringValidator(MinLength = 1, MaxLength = 700)]
        public string Path
        {
            get
            {
                return (string)this["path"];
            }
            set
            {
                this["path"] = value;
            }
        }
    }

    public class JsElementCollection
              : ConfigurationElementCollection {

        public JsElementCollection() {
        }

        public override ConfigurationElementCollectionType CollectionType {
            get {
                return
                    ConfigurationElementCollectionType.AddRemoveClearMap;
            }
        }

        public JsElement this[int index] {
            get { return (JsElement)base.BaseGet(index); }
        }

        public new JsElement this[string name] {
            get { return (JsElement)base.BaseGet(name); }
        }

        protected override ConfigurationElement CreateNewElement() {
            return new JsElement();
        }

        protected override object GetElementKey(
              ConfigurationElement element) {
            return ((JsElement)element).Name;
        }

        public int IndexOf(JsElement csselement) {
            return BaseIndexOf(csselement);
        }

        public void Add(JsElement csselement) {
            BaseAdd(csselement);
        }

        public void Remove(JsElement csselement) {
            if (BaseIndexOf(csselement) > 0) {
                BaseRemove(csselement.Name);
            }
        }

        public void RemoveAt(int index) {
            BaseRemoveAt(index);
        }

        public void Remove(string name) {
            BaseRemove(name);
        }

        public void Clear() {
            BaseClear();
        }

    }


    // Define the "JsElement" element
    // with "name" and "path" attributes.
    public class JsElement : ConfigurationElement {
        [ConfigurationProperty("name", DefaultValue = "Unknown",
           IsKey = true, IsRequired = true)]
        [StringValidator(MinLength = 1, MaxLength = 50)]
        public String Name {
            get {
                return (String)base["name"];
            }
            set {
                base["name"] = value;
            }
        }


        [ConfigurationProperty("path", DefaultValue = "~/", IsRequired = true)]
        [StringValidator(MinLength = 1, MaxLength = 700)]
        public string Path {
            get {
                return (string)this["path"];
            }
            set {
                this["path"] = value;
            }
        }
    }

}

