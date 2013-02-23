// -----------------------------------------------------------------------
// <copyright file="BaseWindowViewModel.cs" company="OpenSauceSolutions">
// © 2013 OpenSauce Solutions
// </copyright>
// -----------------------------------------------------------------------

namespace KerbalEdit.ViewModels
{
    using System.ComponentModel;

    /// <summary>
    /// Shared base model for all workspace windows
    /// </summary>
    public abstract class BaseWindowViewModel : IViewModel
    {
        private bool isDirty;

        /// <summary>
        /// Gets the parent IViewModel instance 
        /// </summary>
        /// <remarks>A window is always the top level data object in a VM context. This class always returns null for this property. </remarks>
        public virtual IViewModel Parent
        {
            // All Window ViewModels are penultimate objects (ie no parent ever)
            get { return null; }
        }

        /// <summary>
        /// Gets or sets a value indicating whether this data object or any of its children have been altered since the last load or save operation
        /// </summary>
        public virtual  bool IsDirty
        {
            get
            {
                return isDirty;
            }
            set
            {
                isDirty = value;
                OnPropertyChanged("IsDirty");
            }
        }

        /// <summary>
        /// PropertyChanged field requirement inherited by IViewModel's INotifiyPropertyChanged requirement. Allows for event signaling of property changes on the model.
        /// </summary>
        public virtual event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Handler for OnPropertyChanged events
        /// </summary>
        /// <param name="name">property name that has changed</param>
        /// <param name="value">new value</param>
        protected virtual void OnPropertyChanged(string name, object value = null)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(name));
            }
        }

    }
}
