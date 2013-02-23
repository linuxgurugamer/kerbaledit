// -----------------------------------------------------------------------
// <copyright file="ExportDataDialogViewModel.cs" company="OpenSauceSolutions">
// © 2013 OpenSauce Solutions
// </copyright>
// -----------------------------------------------------------------------

namespace KerbalEdit.ViewModels
{
    using System;
    using System.ComponentModel;
    using System.Linq;
    using System.Windows.Input;

    using KerbalData;

    /// <summary>
    /// Model class to support data export.
    /// </summary>
    public class ExportDataDialogViewModel : INotifyPropertyChanged
    {
        private string path;
        private IStorable storable;
        private ProcessorRegistry registry; 

        private bool exportComplete;

        /// <summary>
        /// Initializes a new instance of the <see cref="ExportDataDialogViewModel" /> class.
        /// </summary>	
        /// <param name="storable"><see cref="IStorable"/> instance to copy and export</param>
        /// <param name="registry">configured <see cref="ProcessorRegistry"/> instance used for serialization during export.</param>
        public ExportDataDialogViewModel(IStorable storable, ProcessorRegistry registry)
        {
            this.storable = storable;
            this.registry = registry;

            ExportStorableCommand = new DelegateCommand(
                () =>
                {
                    // TODO: Anywhere reflection is required due to the use of generics in KerbalData are places
                    // that should be reviewed for re-factoring on either the App or the API
                    var regType = registry.GetType();
                    var createMethod =
                        regType.GetMethods().FirstOrDefault(m => m.Name == "Create").MakeGenericMethod(new Type[] { storable.GetType() });

                    var saveMethod =
                        typeof(KspData).GetMethods().Where(m => m.Name == "SaveFile").FirstOrDefault(m => m.GetParameters()[2].ParameterType != typeof(string))
                        .MakeGenericMethod(new Type[] { storable.GetType() });

                    saveMethod.Invoke(null, new object[] { path, storable, createMethod.Invoke(registry, null) });
 
                    ExportComplete = true;
                });
        }

        /// <summary>
        /// Gets or sets the path to save the data to
        /// </summary>
        public string Path
        {
            get
            {
                return path;
            }

            set
            {
                path = value;
                OnPropertyChanged("Path");
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the export process has completed. 
        /// </summary>
        public bool ExportComplete
        {
            get
            {
                return exportComplete;
            }

            set
            {
                exportComplete = value;
                OnPropertyChanged("ExportComplete");
            }
        }

        /// <summary>
        /// Command binding used for execution of the export process resulting in the selected data to be saved as a file to the desired path. 
        /// </summary>
        public ICommand ExportStorableCommand { get; set; }

        /// <summary>
        /// Event handler used for signaling property value changes
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
