// -----------------------------------------------------------------------
// <copyright file="UnMappedPropertyViewModel.cs" company="OpenSauceSolutions">
// © 2013 OpenSauce Solutions
// </copyright>
// -----------------------------------------------------------------------

namespace KerbalEdit.ViewModels
{
    using System.ComponentModel;

    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;

    using KerbalData;

    /// <summary>
    /// Model for a UnMapped Property on a <see cref="IKerbalDataObject"/>
    /// </summary>
    public class UnMappedPropertyViewModel : INotifyPropertyChanged
    {
        private string key;
        private JToken value;
        private TreeViewItemViewModel viewModelParent;

        private bool isDirty;

        /// <summary>
        /// Initializes a new instance of the <see cref="MappedPropertyViewModel" /> class.
        /// </summary>	
        /// <param name="key">key of the unmapped property</param>
        /// <param name="parent">object containing the mapped properties</param>
        /// <param name="viewModelParent">model that manages this instance</param>
        public UnMappedPropertyViewModel(string key, ObservableDictionary<string, JToken> parent, TreeViewItemViewModel viewModelParent)
        {
            Parent = parent;
            this.key = key;
            value = Parent[key];
            this.viewModelParent = viewModelParent;
        }

        /// <summary>
        /// Gets or sets the key of the unmapped property. 
        /// </summary>
        /// <remarks>Unlike mapped properties, unmapped properties are simple dictionary entries which can have key names easily changed. Use unmapped properties to change existing keys and add completely new values to the data. </remarks>
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

        /// <summary>
        /// Gets or sets the underlying unmapped property value
        /// </summary>
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

        /// <summary>
        /// Gets or sets a value indicating whether the property has changed since load or last save.
        /// </summary>
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

                if (viewModelParent.IsDirty != isDirty)
                {
                    viewModelParent.IsDirty = isDirty;
                }
            }
        }

        /// <summary>
        /// Gets the parent dictionary that contains the property
        /// </summary>
        public ObservableDictionary<string, JToken> Parent { get; private set; }

        /// <summary>
        /// Event hook for property change events.
        /// </summary>
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
