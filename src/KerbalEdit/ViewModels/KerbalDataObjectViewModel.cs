// -----------------------------------------------------------------------
// <copyright file="KerbalDataObjectViewModel.cs" company="OpenSauceSolutions">
// © 2013 OpenSauce Solutions
// </copyright>
// -----------------------------------------------------------------------

namespace KerbalEdit.ViewModels
{
    using System.Collections;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Reflection;

    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;

    using KerbalData;

    /// <summary>
    /// Model for a <see cref="KerbalDataObject"/> instance
    /// </summary>
    public class KerbalDataObjectViewModel : TreeViewItemViewModel
    {
        private bool childrenLoaded;
        private ObservableCollection<MappedPropertyViewModel> mappedProperties;
        private ObservableCollection<UnMappedPropertyViewModel> unmappedProperties;

        /// <summary>
        /// Initializes a new instance of the <see cref="KerbalDataObjectViewModel" /> class.
        /// </summary>	
        /// <param name="displayName">desired UI display name</param>
        /// <param name="parent">parent instance to use</param>
        public KerbalDataObjectViewModel(string displayName, TreeViewItemViewModel parent)
            : base(displayName, parent)
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="KerbalDataObjectViewModel" /> class.
        /// </summary>	
        public KerbalDataObjectViewModel(TreeViewItemViewModel parent, IKerbalDataObject obj) : base(obj.DisplayName, parent)
        {
            Object = obj;
        }

        /// <summary>
        /// Gets or sets a value indicating whether the object has been selected.
        /// </summary>
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

        /// <summary>
        /// Gets a collection of mapped (strongly typed) properties and their value handled by the <see cref="KerbalDataObject"/> instance. 
        /// </summary>
        public ObservableCollection<MappedPropertyViewModel> MappedProperties
        {
            get
            {
                if (Object != null && mappedProperties == null)
                {
                    var props = new List<MappedPropertyViewModel>();
                    mappedProperties =
                        new ObservableCollection<MappedPropertyViewModel>(BuildRegisteredNames(Object.GetType().GetProperties())
                            .Select(n => new MappedPropertyViewModel(n, Object, this)).ToList());
                }

                return mappedProperties;
            }
            /*
            private set
            {
                mappedProperties = value;
                OnPropertyChanged("MappedProperties", mappedProperties);
                IsDirty = true;
            }*/
        }

        /// <summary>
        /// Gets a collection of unmapped (un-typed string dictionary) properties and their value handled by the <see cref="KerbalDataObject"/> instance. 
        /// </summary>
        public ObservableCollection<UnMappedPropertyViewModel> UnmappedProperties
        {
            get
            {
                if (Object != null && unmappedProperties == null)
                {
                    unmappedProperties =
                            new ObservableCollection<UnMappedPropertyViewModel>(
                                Object.Select(kvp => new UnMappedPropertyViewModel(
                                    kvp.Key,
                                    (ObservableDictionary<string, JToken>)Object, this)).ToList());
                }

                return unmappedProperties;
            }
            set
            {
                unmappedProperties = value;
                OnPropertyChanged("UnmappedProperties", unmappedProperties);
                IsDirty = true;
            }
        }

        /// <summary>
        /// Handles loading of children in lazy load scenarios
        /// </summary>
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
                        var obj = (IKerbalDataObject)prop.GetValue(Object, BindingFlags.GetProperty, null, null, null);

                        if (obj != null)
                        {
                            Children.Add(new KerbalDataObjectViewModel(this, obj));
                        }
                    }

                    if (prop.PropertyType.IsGenericType && (prop.PropertyType.GetGenericTypeDefinition() == typeof(IList<>)))
                    {
                        var val = prop.GetValue(Object, BindingFlags.GetProperty, null, null, null);

                        if (val != null && val.GetType().GetGenericArguments()[0].GetInterfaces().Contains(typeof(IKerbalDataObject)))
                        {
                            Children.Add(new KerbalDataObjectListViewModel(prop.Name, this, (ICollection)val));
                        }
                    }
                }

            childrenLoaded = true;
        }

        private static IList<string> BuildRegisteredNames(PropertyInfo[] infoArray)
        {
            var list = new List<string>();

            // NOTE: We are only pulling  names that have had their JSON Attribute explicitly set
            for (var i = 0; i < infoArray.Count(); i++)
            {
                var propInfo = infoArray[i];

                // Complex types and lists are handled by the tree view, TODO: need support for lists/arrays of primitives. 
                if (propInfo.PropertyType == typeof(string)
                    || propInfo.PropertyType == typeof(int)
                    || propInfo.PropertyType == typeof(decimal)
                    || propInfo.PropertyType == typeof(float)
                    || propInfo.PropertyType == typeof(double)
                    || propInfo.PropertyType == typeof(long)
                    || propInfo.PropertyType == typeof(char)
                    || propInfo.PropertyType == typeof(bool)
                    || propInfo.PropertyType == typeof(string))
                {
                    list.AddRange(((JsonPropertyAttribute[])propInfo.GetCustomAttributes(typeof(JsonPropertyAttribute), true)).Select(p => propInfo.Name));
                }
            }

            return list;
        }

    }
}
