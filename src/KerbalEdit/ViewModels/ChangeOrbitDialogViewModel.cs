// -----------------------------------------------------------------------
// <copyright file="ChangeOrbitDialogViewModel.cs" company="OpenSauceSolutions">
// © 2013 OpenSauce Solutions
// </copyright>
// -----------------------------------------------------------------------

namespace KerbalEdit.ViewModels
{
    using System.ComponentModel;
    using KerbalData.Models;

    /// <summary>
    /// Model class for the Change Orbit dialog box
    /// </summary>
    public class ChangeOrbitDialogViewModel
    {
        private readonly KerbalDataObjectViewModel orbitVm;

        /// <summary>
        /// Initializes a new instance of the <see cref="ChangeOrbitDialogViewModel" /> class.
        /// </summary>
        /// <param name="vm">View model instance of type <see cref="KerbalDataObjectViewModel"/></param>
        public ChangeOrbitDialogViewModel(INotifyPropertyChanged vm)
        {
            orbitVm = vm as KerbalDataObjectViewModel;

            if (orbitVm == null)
            {
                throw new KerbalEditException("An application error has occured. In valid ViewModel data provided to Change Orbit Dialog");
            }

            ((Orbit)orbitVm.Object).PropertyChanged += ChangeOrbitDialogViewModel_PropertyChanged;
        }

        /// <summary>
        /// Gets the <see cref="Orbit"/> instance handled by this view model
        /// </summary>
        public Orbit Orbit
        {
            get
            {
                return (Orbit)orbitVm.Object;
            }
        }

        private void ChangeOrbitDialogViewModel_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            orbitVm.IsDirty = true;
        }
    }
}

