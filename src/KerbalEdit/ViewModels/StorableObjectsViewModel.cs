// -----------------------------------------------------------------------
// <copyright file="StorableObjectsTreeViewItemViewModel.cs" company="OpenSauceSolutions">
// © 2013 OpenSauce Solutions
// </copyright>
// -----------------------------------------------------------------------

namespace KerbalEdit.ViewModels
{
    using System.Linq;

    using KerbalData;

    /// <summary>
    /// Model for a <see cref="StorableObjects{T}"/> instance
    /// </summary>
    public class StorableObjectsViewModel<T> : TreeViewItemViewModel, IStorableObjectsViewModel where T : class, IStorable, new()
    {
        private StorableObjects<T> objects;

        /// <summary>
        /// Initializes a new instance of the <see cref="StorableObjectsViewModel{T}" /> class.
        /// </summary>	
        /// <param name="objects"><see cref="StorableObjects{T}"/>instance to be managed</param>
        /// <param name="name">UI name to use for the node</param>
        /// <param name="parent">Parent model containing this instance.</param>
        public StorableObjectsViewModel(StorableObjects<T> objects, string name, ISelectedViewModel parent)
            : base(name, parent)
        {
            this.objects = objects;
        }

        /// <summary>
        /// Gets the data model managed by this instance
        /// </summary>
        public IStorableObjects Objects
        {
            get { return objects; }
        }

        /// <summary>
        /// Refreshes the <see cref="StorableObjects{T}"/> instance to expose new data
        /// </summary>
        public void Refresh()
        {
            foreach (var name in Objects.Names)
            {
                if (!Children.Any(c => c.DisplayName == name))
                {
                    base.Children.Add(new StorableObjectViewModel<T>(name, this));
                }
            }
        }

        /// <summary>
        /// Loads children objects for lazy loading
        /// </summary>
        protected override void LoadChildren()
        {
            foreach (var name in objects.Names)
            {
                base.Children.Add(new StorableObjectViewModel<T>(name, this));
            }
        }

        /// <summary>
        /// Handler for selected item change events
        /// </summary>
        /// <param name="item">item instance that has been selected</param>
        protected override void OnSelectedItemChanged(TreeViewItemViewModel item)
        {
            base.OnSelectedItemChanged(item);
            ((ISelectedViewModel)Parent).SelectedItem = item;
        }
    }
}
