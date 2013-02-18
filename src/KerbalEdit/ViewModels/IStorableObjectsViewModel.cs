// -----------------------------------------------------------------------
// <copyright file="IIStorableObjectsViewModel.cs" company="OpenSauceSolutions">
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
    /// TODO: Interface Summary
    /// </summary>
    public interface IStorableObjectsViewModel
    {
        IStorableObjects Objects { get; }
        void Refresh();
    }
}
