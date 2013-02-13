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
    using KerbalData;
    using KerbalData.Models;
using Newtonsoft.Json.Linq;

    /// <summary>
    /// TODO: Class Summary
    /// </summary>
    public sealed class KerbalDataViewModel : ISelectedViewModel, INotifyPropertyChanged
    {

        private readonly ReadOnlyCollection<TreeViewItemViewModel> objects; //= new ReadOnlyCollection<string>(new List<string>() { "Save Games", "Scenarios", "Training Scenarios", "VAB Craft", "SPH Craft", "Settings/Configuration" });
        private KerbalData kd;
        private ISelectedViewModel parent;
        private IViewModel selectedItem;
        private ObservableCollection<UnMappedProp> unmappedProperties;

        /// Initializes a new instance of the <see cref="KerbalDataViewModel" /> class.
        /// </summary>	
        public KerbalDataViewModel(KerbalData kd)
        {
            this.kd = kd;
            this.parent = null;

            objects = new ReadOnlyCollection<TreeViewItemViewModel>(new List<TreeViewItemViewModel>()
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

        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;

        public ReadOnlyCollection<TreeViewItemViewModel> Objects { get { return objects; } }

        public IViewModel SelectedItem
        {
            get
            {
                return selectedItem;
            }

            set
            {
                if (value is TreeViewItemViewModel)
                {
                    selectedItem = (TreeViewItemViewModel)value;
                    OnPropertyChanged("SelectedItem", selectedItem);

                    var list = new ObservableCollection<UnMappedProp>();
                    foreach (var kvp in ((TreeViewItemViewModel)selectedItem).Object)
                    {
                        list.Add(new UnMappedProp(kvp.Key, (ObservableDictionary<string, JToken>)((TreeViewItemViewModel)selectedItem).Object));
                    }

                    UnmappedProperties = list;
                }
                else
                {
                    throw new KerbalEditException("Provided value is not of type TreeViewItemViewModel. This ViewModel is only wired to work with TreeViewModels");
                }
            }
        }

        public ObservableCollection<UnMappedProp> UnmappedProperties
        {
            get
            {
                return unmappedProperties;
            }
            set
            {
                unmappedProperties = value;
                OnPropertyChanged("UnmappedProperties", unmappedProperties);
            }
        }

        private void OnPropertyChanged(string name, object value = null)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(name));
            }
        }

        public class UnMappedProp : INotifyPropertyChanged
        {
            private string key;
            private JToken value;

            public UnMappedProp(string key, ObservableDictionary<string, JToken> parent)
            {
                Parent = parent;
                this.key = key;
                value = Parent[key];

            }

            public string Key
            {
                get
                {
                    return key;
                }
                set
                {
                    var newKey = value;

                    if (Parent.ContainsKey(key))
                    {
                        Parent.Remove(key);
                    }

                    if (Parent.ContainsKey(newKey))
                    {
                        Parent.Remove(newKey);
                    }

                    Parent.Add(newKey, Value);
                    
                    key = newKey;
                    OnPropertyChanged("Key", key);
                }
            }

            public string Value
            {
                get
                {
                    return value.ToString();
                }
                set
                {
                    this.value = value;
                    Parent[key] = value;
                    OnPropertyChanged("Value", this.value);
                }
            }

            public ObservableDictionary<string, JToken> Parent { get; private set; }

            public event PropertyChangedEventHandler PropertyChanged;

            private void OnPropertyChanged(string name, object value = null)
            {
                if (PropertyChanged != null)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs(name));
                }
            }
        }
    }
}
