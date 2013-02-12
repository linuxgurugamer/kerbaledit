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
    using System.Linq;
    using System.Text;
    using KerbalData;
    using KerbalData.Models;

    /// <summary>
    /// TODO: Class Summary
    /// </summary>
    public class KerbalDataObjectViewModel : TreeViewItemViewModel
    {
        private IKerbalDataObject obj;
        /// <summary>
        /// Initializes a new instance of the <see cref="KerbalDataObjectViewModel" /> class.
        /// </summary>	
        public KerbalDataObjectViewModel(string displayName, TreeViewItemViewModel parent, IKerbalDataObject obj) : base(displayName, parent)
        {
            this.obj = obj;
        }

        protected override void LoadChildren()
        {
                foreach (var prop in obj.GetType().GetProperties())
                {
                    if (prop.PropertyType.GetInterfaces().Any(i => i.FullName.Contains("IKerbalDataObject")))
                    {
                        Children.Add(new KerbalDataObjectViewModel(prop.Name, this, (IKerbalDataObject)prop.GetValue(obj)));
                    }

                    
                    //var interfaces = prop.PropertyType.GetInterfaces().Where(i => i.FullName.Contains("IList"));
                    // if (prop.PropertyType.GetInterfaces().Any(i => i.FullName.Contains("IList") && i.GetGenericArguments().Any(a => a.GetInterfaces().Any(ai => ai.FullName.Contains("IKerbalDataObject")))))
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
