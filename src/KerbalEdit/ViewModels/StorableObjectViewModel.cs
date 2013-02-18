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
    using System.Reflection;
    using System.Text;
    using KerbalData;
    using KerbalData.Models;

    /// <summary>
    /// TODO: Class Summary
    /// </summary>
    public class StorableObjectViewModel<T> : KerbalDataObjectViewModel, ISelectedViewModel where T : class, IStorable, new()
    {
        private bool childrenLoaded;
        private StorableObjects<T> objects;
        private IViewModel selectedItem;

        //private T obj;
        /// <summary>
        /// Initializes a new instance of the <see cref="StorableObjectViewModel" /> class.
        /// </summary>	
        public StorableObjectViewModel(string displayName, StorableObjectsViewModel<T> parent) : base(displayName, parent)
        {
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

        public override IKerbalDataObject Object
        {
            get
            {
                if (base.Object == null)
                {
                    Object = ((StorableObjects<T>)((StorableObjectsViewModel<T>)Parent).Objects)[DisplayName];
                }

                return base.Object;
            }
            protected set
            {
                base.Object = value;
            }
        }

        public override Type ObjectType
        {
            get
            {
                ObjectType = typeof(StorableObjectViewModel<T>); 
                return base.ObjectType;
            }
            protected set
            {
                base.ObjectType = value;
            }
        }

        public IViewModel SelectedItem
        {
            get
            {
                return selectedItem;
            }

            set
            {
                selectedItem = value;
                OnPropertyChanged("SelectedItem", selectedItem);
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

            // If the type for the object data contains storable collections, add them to children for user access.
            var props = Object.GetType().GetProperties().Where(p => p.PropertyType.FullName.Contains("StorableObjects"));
            
            foreach (var prop in props)
            {
                var propArgs = prop.PropertyType.GetGenericArguments();
                var createVmMetod = typeof(StorableObjectsViewModel<>).MakeGenericType(propArgs).GetConstructor(new Type[] { typeof(StorableObjects<>).MakeGenericType(propArgs), typeof(string), typeof(ISelectedViewModel) });

                var vm = createVmMetod.Invoke(new object[] { prop.GetValue(Object, BindingFlags.GetProperty, null, null, null), prop.Name, this });
                Children.Add((TreeViewItemViewModel)vm);
            }

            childrenLoaded = true;
        }

    }
}
