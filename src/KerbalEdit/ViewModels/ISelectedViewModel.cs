// -----------------------------------------------------------------------
// <copyright file="IISelectedViewModel.cs" company="OpenSauceSolutions">
// © 2013 OpenSauce Solutions
// </copyright>
// -----------------------------------------------------------------------

namespace KerbalEdit.ViewModels
{
    /// <summary>
    /// Defines the contract for a class that can be selected. 
    /// </summary>
    public interface ISelectedViewModel : IViewModel
    {
        /// <summary>
        /// Gets or sets the viewmodel that is selected.
        /// </summary>
        IViewModel SelectedItem { get; set; }
    }
}
