// -----------------------------------------------------------------------
// <copyright file="StorableObjectsTreeViewItemViewModel.cs" company="OpenSauceSolutions">
// © 2013 OpenSauce Solutions
// </copyright>
// -----------------------------------------------------------------------

namespace KerbalEdit.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    using KerbalData;

    /// <summary>
    /// TODO: Class Summary
    /// </summary>
    public class StorableObjectsViewModel<T> : TreeViewItemViewModel where T : class, IStorable, new()
    {
        private StorableObjects<T> objects;

        /// <summary>
        /// Initializes a new instance of the <see cref="StorableObjectsViewModel{T}" /> class.
        /// </summary>	
        public StorableObjectsViewModel(StorableObjects<T> objects, string name)
            : base(name)
        {
            this.objects = objects;
        }

        public StorableObjects<T> Objects { get { return objects; } }

        protected override void LoadChildren()
        {
            foreach (var name in objects.Names)
            {
                base.Children.Add(new StorableObjectViewModel<T>(name, this));
            }
        }


        
    }
}
