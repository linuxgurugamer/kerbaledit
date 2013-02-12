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
    public class KerbalDataObjectListViewModel : TreeViewItemViewModel
    {
        private ICollection objs;
        /// <summary>
        /// Initializes a new instance of the <see cref="KerbalDataObjectViewModel" /> class.
        /// </summary>	
        public KerbalDataObjectListViewModel(string displayName, TreeViewItemViewModel parent, ICollection objs)
            : base(displayName, parent)
        {
            this.objs = objs;
        }

        protected override void LoadChildren()
        {
            var enumerator = objs.GetEnumerator();
            var i = 0;
            while (enumerator.MoveNext())
            {
                Children.Add(new KerbalDataObjectViewModel(this, (IKerbalDataObject)enumerator.Current));
                i++;
            }
            
        }
    }
}
