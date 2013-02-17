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
    using System.Linq;
    using System.Text;

    using Newtonsoft.Json.Linq;

    using KerbalData;
    using KerbalData.Models;

    /// <summary>
    /// TODO: Class Summary
    /// </summary>
    public sealed class KerbalDataViewModel : ISelectedViewModel, INotifyPropertyChanged, IDisposable
    {
        private readonly ObservableCollection<TreeViewItemViewModel> objects; 
        private KerbalData kd;
        private ISelectedViewModel parent;
        private IViewModel selectedItem;

        private bool isDirty;

        /// <summary>
        /// Initializes a new instance of the <see cref="KerbalDataViewModel" /> class.
        /// </summary>	
        public KerbalDataViewModel(KerbalData kd)
        {
            this.kd = kd;
            this.parent = null;

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

        public KerbalData Data
        {
            get
            {
                return kd;
            }
        }

        public IViewModel Parent
        {
            get { return parent; }
        }

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

        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;

        public ObservableCollection<TreeViewItemViewModel> Objects { get { return objects; } }

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

        private void OnPropertyChanged(string name, object value = null)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(name));
            }
        }

        public void Dispose()
        {
            objects.Clear();
            kd = null;
            parent = null;
            selectedItem = null;
        }
    }
}
