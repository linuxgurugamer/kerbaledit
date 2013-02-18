// -----------------------------------------------------------------------
// <copyright file="SaveAsDialogViewModel.cs" company="OpenSauceSolutions">
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

    /// <summary>
    /// TODO: Class Summary
    /// </summary>
    public class SaveAsDialogViewModel : INotifyPropertyChanged
    {
        private string name;
        private IStorable storable;

        private bool saveComplete;

        /// <summary>
        /// Initializes a new instance of the <see cref="SaveAsDialogViewModel" /> class.
        /// </summary>	
        public SaveAsDialogViewModel(IStorable storable)
        {
            this.storable = storable;

            SaveStorableCommand = new DelegateCommand(
                () =>
                {
                    storable.Save(name);
                    SaveComplete = true;
                });
        }

        public string Name
        {
            get
            {
                return name;
            }

            set
            {
                name = value;
                OnPropertyChanged("Name");
            }
        }

        public bool SaveComplete
        {
            get
            {
                return saveComplete;
            }

            set
            {
                saveComplete = value;
                OnPropertyChanged("SaveComplete");
            }
        }

        public ICommand SaveStorableCommand { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
