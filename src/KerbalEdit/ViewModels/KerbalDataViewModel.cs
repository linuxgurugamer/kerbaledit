// -----------------------------------------------------------------------
// <copyright file="KerbalDataViewModel.cs" company="OpenSauceSolutions">
// © 2013 OpenSauce Solutions
// </copyright>
// -----------------------------------------------------------------------

namespace KerbalEdit.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.ComponentModel;

    using KerbalData;
    using KerbalData.Models;

    /// <summary>
    /// Top level model for mapping KSP data.
    /// </summary>
    public sealed class KerbalDataViewModel : ISelectedViewModel, INotifyPropertyChanged, IDisposable
    {
        private readonly ObservableCollection<TreeViewItemViewModel> objects; 
        private KerbalData kd;
        private IViewModel selectedItem;

        private bool isDirty;

        /// <summary>
        /// Initializes a new instance of the <see cref="KerbalDataViewModel" /> class.
        /// </summary>	
        public KerbalDataViewModel(KerbalData kd)
        {
            this.kd = kd;

            objects = new ObservableCollection<TreeViewItemViewModel>(new List<TreeViewItemViewModel>()
            {
                new StorableObjectsViewModel<SaveFile>(kd.Saves, "Saves", this),
                new StorableObjectsViewModel<SaveFile>(kd.Scenarios, "Scenarios", this),
                new StorableObjectsViewModel<SaveFile>(kd.TrainingScenarios, "TrainingScenarios", this),
                new StorableObjectsViewModel<CraftFile>(kd.CraftInVab, "CraftInVab", this),
                new StorableObjectsViewModel<CraftFile>(kd.CraftInSph, "CraftInSph", this),
                new StorableObjectsViewModel<PartFile>(kd.Parts, "Parts", this),
                new StorableObjectsViewModel<ConfigFile>(kd.KspSettings, "KspSettings", this)
            });
        }

        /// <summary>
        /// Gets the core <see cref="KerbalData"/> instance driving all data transactions. 
        /// </summary>
        public KerbalData Data
        {
            get
            {
                return kd;
            }
        }

        /// <summary>
        /// Gets the parent - Always null, <see cref="KerbalDataViewModel"/> is top level for bound KSP data. 
        /// </summary>
        public IViewModel Parent
        {
            get { return null; }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the <see cref="KerbalData"/> instance or any of it's child data has changed since load or last save.
        /// </summary>
        public bool IsDirty
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
        /// Gets the collection of top level models for each type of data file as mapped from the <see cref="KerbalData"/> instance
        /// </summary>
        public ObservableCollection<TreeViewItemViewModel> Objects
        {
            get { return objects; }
        }

        /// <summary>
        /// Gets or sets the item instance that has been selected.
        /// </summary>
        public IViewModel SelectedItem
        {
            get
            {
                return selectedItem;
            }

            set
            {
                selectedItem = value;
                OnPropertyChanged("SelectedItem", selectedItem);
            }
        }

        /// <summary>
        /// Event hook for property change notifications on this objects properties.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Disposes of object and it's child instances
        /// </summary>
        /// <remarks>
        /// <para>This is part of an attempt to handle a memory leak with the view model pattern at present, this is to be corrected and removed.</para>
        /// </remarks>
        public void Dispose()
        {
            objects.Clear();
            kd = null;
            selectedItem = null;
        }

        private void OnPropertyChanged(string name, object value = null)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(name));
            }
        }
    }
}
