// -----------------------------------------------------------------------
// <copyright file="ChangeOrbitDialogViewModel.cs" company="OpenSauceSolutions">
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
    using System.Windows.Input;
    using KerbalData;
    using KerbalData.Models;

    /// <summary>
    /// TODO: Class Summary
    /// </summary>
    public class ChangeOrbitDialogViewModel
    {
        private KerbalDataObjectViewModel orbitVm;

        public ChangeOrbitDialogViewModel(TreeViewItemViewModel vm)
        {
            orbitVm = vm as KerbalDataObjectViewModel;

            if (orbitVm == null)
            {
                throw new KerbalEditException("An application error has occured. In valid ViewModel data provided to Change Orbit Dialog");
            }

            ((Orbit)orbitVm.Object).PropertyChanged += ChangeOrbitDialogViewModel_PropertyChanged;
        }

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

