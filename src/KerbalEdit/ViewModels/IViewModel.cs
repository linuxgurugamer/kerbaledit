// -----------------------------------------------------------------------
// <copyright file="IIViewModel.cs" company="OpenSauceSolutions">
// © 2013 OpenSauce Solutions
// </copyright>
// -----------------------------------------------------------------------

namespace KerbalEdit.ViewModels
{
    using System.ComponentModel;

    /// <summary>
    /// Base view model contract for all VMs
    /// </summary>
    public interface IViewModel : INotifyPropertyChanged
    {
        /// <summary>
        /// Gets the Parent instance
        /// </summary>
        IViewModel Parent { get; }

        /// <summary>
        /// Gets or sets a value indicating weather the data object or any of its children contain changed data.
        /// </summary>
        bool IsDirty { get; set;}
    }
}
