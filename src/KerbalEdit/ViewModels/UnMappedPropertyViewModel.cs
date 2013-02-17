// -----------------------------------------------------------------------
// <copyright file="UnMappedPropertyViewModel.cs" company="OpenSauceSolutions">
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
    using System.Reflection;
    using System.Text;

    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;

    using KerbalData;
    using KerbalData.Models;


    public class UnMappedPropertyViewModel : INotifyPropertyChanged
    {
        private string key;
        private JToken value;
        private TreeViewItemViewModel viewModelParent;

        private bool isDirty;

        public UnMappedPropertyViewModel(string key, ObservableDictionary<string, JToken> parent, TreeViewItemViewModel viewModelParent)
        {
            Parent = parent;
            this.key = key;
            value = Parent[key];
            this.viewModelParent = viewModelParent;
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

                IsDirty = true;
            }
        }

        public string Value
        {
            get
            {
                return value.ToString(Formatting.None);
            }
            set
            {
                this.value = value;
                Parent[key] = value;
                OnPropertyChanged("Value", this.value);

                IsDirty = true;
            }
        }

        public bool IsDirty
        {
            get
            {
                return isDirty;
            }

            set
            {
                isDirty = value;
                OnPropertyChanged("IsDirty");

                viewModelParent.IsDirty = true;
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
}
