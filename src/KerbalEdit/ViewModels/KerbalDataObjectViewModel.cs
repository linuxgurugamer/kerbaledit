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
    using System.Reflection;
    using System.Text;

    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;

    using KerbalData;
    using KerbalData.Models;

    /// <summary>
    /// TODO: Class Summary
    /// </summary>
    public class KerbalDataObjectViewModel : TreeViewItemViewModel
    {
        private bool childrenLoaded;
        private ObservableCollection<MappedPropertyViewModel> mappedProperties;
        private ObservableCollection<UnMappedPropertyViewModel> unmappedProperties;

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
            
            private set
            {
                mappedProperties = value;
                OnPropertyChanged("MappedProperties", mappedProperties);
                IsDirty = true;
            }
        }

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

        private IList<string> BuildRegisteredNames(PropertyInfo[] infoArray)
        {
            var list = new List<string>();

            // NOTE: We are only recgnonizing names that have had thier JSON Attribute explicitly set
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
