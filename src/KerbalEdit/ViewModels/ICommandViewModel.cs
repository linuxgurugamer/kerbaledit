// -----------------------------------------------------------------------
// <copyright file="ICommandViewModel.cs" company="OpenSauceSolutions">
// © 2013 OpenSauce Solutions
// </copyright>
// -----------------------------------------------------------------------

namespace KerbalEdit.ViewModels
{
    using System.Windows.Input;

    /// <summary>
    /// TODO: Interface Summary
    /// </summary>
    public interface ICommandViewModel<T> where T : ICommand
    {
        string DisplayName { get; set; }
        T Command { get; set; }
    }
}
