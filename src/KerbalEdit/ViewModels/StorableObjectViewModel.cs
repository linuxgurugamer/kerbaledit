// -----------------------------------------------------------------------
// <copyright file="StorableObjectViewModel.cs" company="OpenSauceSolutions">
// © 2013 OpenSauce Solutions
// </copyright>
// -----------------------------------------------------------------------

namespace KerbalEdit.ViewModels
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using KerbalData;
    using KerbalData.Models;
    /// <summary>
    /// TODO: Class Summary
    /// </summary>
    public class StorableObjectViewModel<T> : TreeViewItemViewModel where T : class, IStorable, new()
    {
        private T obj;
        /// <summary>
        /// Initializes a new instance of the <see cref="StorableObjectViewModel" /> class.
        /// </summary>	
        public StorableObjectViewModel(string displayName, StorableObjectsViewModel<T> parent) : base(displayName, parent)
        {

        }

        protected override void LoadChildren()
        {
            if (obj == null)
            {
                obj = ((StorableObjectsViewModel<T>)Parent).Objects[DisplayName];

                foreach (var prop in obj.GetType().GetProperties())
                {
                    if (prop.PropertyType.GetInterfaces().Any(i => i.FullName.Contains("IKerbalDataObject")))
                    {
                        Children.Add(new KerbalDataObjectViewModel(prop.Name, this, (IKerbalDataObject)prop.GetValue(obj)));
                    }

                    if (prop.PropertyType.IsGenericType && (prop.PropertyType.GetGenericTypeDefinition() == typeof(IList<>)))
                    {
                        var val = prop.GetValue(obj);
                        if (val != null && val.GetType().GetGenericArguments()[0].GetInterfaces().Contains(typeof(IKerbalDataObject)))
                        {
                            Children.Add(new KerbalDataObjectListViewModel(prop.Name, this, (ICollection)prop.GetValue(obj)));
                        }
                    }
                }
            }
        }
    }
}
