// -----------------------------------------------------------------------
// <copyright file="SaveAsDialogViewModel.cs" company="OpenSauceSolutions">
// © 2013 OpenSauce Solutions
// </copyright>
// -----------------------------------------------------------------------

namespace KerbalEdit.ViewModels
{
    using System.ComponentModel;
    using System.Windows.Input;

    using KerbalData;

    /// <summary>
    /// Model for the SaveAs dialog
    /// </summary>
    public class SaveAsDialogViewModel : INotifyPropertyChanged
    {
        private string name;
        private IStorable storable;

        private bool saveComplete;

        /// <summary>
        /// Initializes a new instance of the <see cref="SaveAsDialogViewModel" /> class.
        /// </summary>	
        /// <param name="storable"><see cref="IStorable"/> instance to save</param>
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

        /// <summary>
        /// Gets or sets the name to use when saving
        /// </summary>
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

        /// <summary>
        /// Gets or sets a value indicating whether the save process has completed
        /// </summary>
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

        /// <summary>
        /// Gets or sets the command to save the provided <see cref="IStorable"/> instance
        /// </summary>
        public ICommand SaveStorableCommand { get; set; }

        /// <summary>
        /// Event hook to receive property change events
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
