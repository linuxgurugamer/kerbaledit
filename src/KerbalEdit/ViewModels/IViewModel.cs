// -----------------------------------------------------------------------
// <copyright file="IIViewModel.cs" company="OpenSauceSolutions">
// © 2013 OpenSauce Solutions
// </copyright>
// -----------------------------------------------------------------------

namespace KerbalEdit.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// TODO: Interface Summary
    /// </summary>
    public interface IViewModel : INotifyPropertyChanged
    {
        IViewModel Parent { get; }
        bool IsDirty { get; set;}
    }
}
