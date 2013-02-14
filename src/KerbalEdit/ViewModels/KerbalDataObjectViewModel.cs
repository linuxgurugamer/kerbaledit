// -----------------------------------------------------------------------
// <copyright file="KerbalDataObjectViewModel.cs" company="OpenSauceSolutions">
// © 2013 OpenSauce Solutions
// </copyright>
// -----------------------------------------------------------------------

namespace KerbalEdit.ViewModels
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.ComponentModel;
    using System.Linq;
    using System.Text;
    using KerbalData;
    using KerbalData.Models;
    using Newtonsoft.Json.Linq;

    /// <summary>
    /// TODO: Class Summary
    /// </summary>
    public class KerbalDataObjectViewModel : TreeViewItemViewModel
    {
        private bool childrenLoaded;
        private ObservableCollection<UnMappedProp> unmappedProperties;

        public KerbalDataObjectViewModel(string displayName, TreeViewItemViewModel parent)
            : base(displayName, parent)
        {

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="KerbalDataObjectViewModel" /> class.
        /// </summary>	
        public KerbalDataObjectViewModel(TreeViewItemViewModel parent, IKerbalDataObject obj) : base(obj.DisplayName, parent)
        {
            Object = obj;
        }

        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;

        public override bool IsSelected
        {
            get
            {
                return base.IsSelected;
            }
            set
            {
                LoadChildren();
                base.IsSelected = value;
            }
        }

        public ObservableCollection<UnMappedProp> UnmappedProperties
        {
            get
            {
                if (Object != null && unmappedProperties == null)
                {
                    unmappedProperties =
                            new ObservableCollection<UnMappedProp>(
                                Object.Select(kvp => new UnMappedProp(
                                    kvp.Key,
                                    (ObservableDictionary<string, JToken>)Object)).ToList());
                }

                return unmappedProperties;
            }
            set
            {
                unmappedProperties = value;
                OnPropertyChanged("UnmappedProperties", unmappedProperties);
            }
        }

        protected override void LoadChildren()
        {
            if (childrenLoaded)
            {
                return;
            }

            RemoveDummyChild();

            foreach (var prop in Object.GetType().GetProperties())
                {
                    if (prop.PropertyType.GetInterfaces().Any(i => i.FullName.Contains("IKerbalDataObject")))
                    {
                        var obj = (IKerbalDataObject)prop.GetValue(Object);

                        if (obj != null)
                        {
                            Children.Add(new KerbalDataObjectViewModel(this, obj));
                        }
                    }

                    if (prop.PropertyType.IsGenericType && (prop.PropertyType.GetGenericTypeDefinition() == typeof(IList<>)))
                    {
                        var val = prop.GetValue(Object);
                        if (val != null && val.GetType().GetGenericArguments()[0].GetInterfaces().Contains(typeof(IKerbalDataObject)))
                        {
                            Children.Add(new KerbalDataObjectListViewModel(prop.Name, this, (ICollection)prop.GetValue(Object)));
                        }
                    }
                }

            childrenLoaded = true;
        }

        public class UnMappedProp : INotifyPropertyChanged
        {
            private string key;
            private JToken value;

            public UnMappedProp(string key, ObservableDictionary<string, JToken> parent)
            {
                Parent = parent;
                this.key = key;
                value = Parent[key];

            }

            public string Key
            {
                get
                {
                    return key;
                }
                set
                {
                    var newKey = value;

                    if (Parent.ContainsKey(key))
                    {
                        Parent.Remove(key);
                    }

                    if (Parent.ContainsKey(newKey))
                    {
                        Parent.Remove(newKey);
                    }

                    Parent.Add(newKey, Value);

                    key = newKey;
                    OnPropertyChanged("Key", key);
                }
            }

            public string Value
            {
                get
                {
                    return value.ToString();
                }
                set
                {
                    this.value = value;
                    Parent[key] = value;
                    OnPropertyChanged("Value", this.value);
                }
            }

            public ObservableDictionary<string, JToken> Parent { get; private set; }

            public event PropertyChangedEventHandler PropertyChanged;

            private void OnPropertyChanged(string name, object value = null)
            {
                if (PropertyChanged != null)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs(name));
                }
            }
        }

        protected override void OnPropertyChanged(string name, object value = null)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(name));
            }
        }
    }
}
