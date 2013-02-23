// -----------------------------------------------------------------------
// <copyright file="IIStorableObjectsViewModel.cs" company="OpenSauceSolutions">
// © 2013 OpenSauce Solutions
// </copyright>
// -----------------------------------------------------------------------

namespace KerbalEdit.ViewModels
{
    using KerbalData;

    /// <summary>
    /// Data files the contract for data models that can be processed or stored. 
    /// </summary>
    public interface IStorableObjectsViewModel
    {
        /// <summary>
        /// Gets the collection data managed by the model
        /// </summary>
        IStorableObjects Objects { get; }

        /// <summary>
        /// Refreshes the Objects collection by re-querying the underlying store. Changes to already loaded models should not be overwritten when called.
        /// </summary>
        void Refresh();
    }
}
