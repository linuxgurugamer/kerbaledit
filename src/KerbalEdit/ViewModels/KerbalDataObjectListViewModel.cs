// -----------------------------------------------------------------------
// <copyright file="KerbalDataObjectViewModel.cs" company="OpenSauceSolutions">
// © 2013 OpenSauce Solutions
// </copyright>
// -----------------------------------------------------------------------

namespace KerbalEdit.ViewModels
{
    using System.Collections;
    using KerbalData;

    /// <summary>
    /// Model for a list of <see cref="KerbalDataObject"/>s
    /// </summary>
    public class KerbalDataObjectListViewModel : TreeViewItemViewModel
    {
        private bool childrenLoaded;
        private readonly ICollection objs;

        /// <summary>
        /// Initializes a new instance of the <see cref="KerbalDataObjectViewModel" /> class.
        /// </summary>	
        public KerbalDataObjectListViewModel(string displayName, TreeViewItemViewModel parent, ICollection objs)
            : base(displayName, parent)
        {
            this.objs = objs;
        }

        /// <summary>
        /// Gets the <see cref="IKerbalDataObject"/>s managed by this model.
        /// </summary>
        public ICollection Objects
        {
            get { return objs; }
        }

        /// <summary>
        /// Loads children in lazy, loading scenarios
        /// </summary>
        protected override void LoadChildren()
        {
            if (childrenLoaded)
            {
                return;
            }

            RemoveDummyChild();

            var enumerator = objs.GetEnumerator();
            var i = 0;
            while (enumerator.MoveNext())
            {
                Children.Add(new KerbalDataObjectViewModel(this, (IKerbalDataObject)enumerator.Current));
                i++;
            }

            childrenLoaded = true;
            
        }
    }
}
