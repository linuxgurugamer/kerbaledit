// -----------------------------------------------------------------------
// <copyright file="ImportDataDialogViewModel.cs" company="OpenSauceSolutions">
// © 2013 OpenSauce Solutions
// </copyright>
// -----------------------------------------------------------------------

namespace KerbalEdit.ViewModels
{
    using System.ComponentModel;
    using System.Linq;
    using System.Windows.Input;

    using KerbalData;

    /// <summary>
    /// Model class for supporting data import operations
    /// </summary>
    public class ImportDataDialogViewModel : INotifyPropertyChanged
    {
        private string path;
        private IStorableObjects storableObjects;
        private ProcessorRegistry registry;

        private IStorable obj;
        private bool importComplete;

        /// <summary>
        /// Initializes a new instance of the <see cref="ImportDataDialogViewModel" /> class.
        /// </summary>	
        /// <param name="storableObjects"><see cref="IStorableObjects"/> instance to import data into</param>
        /// <param name="registry">configured <see cref="ProcessorRegistry"/> instance to use for data serialization</param>
        public ImportDataDialogViewModel(IStorableObjects storableObjects, ProcessorRegistry registry)
        {
            this.storableObjects = storableObjects;
            this.registry = registry;

            ImportStorableCommand = new DelegateCommand(
                () =>
                {
                    // TODO: Anywhere reflection is required due to the use of generics in KerbalData are places
                    // that should be reviewed for re-factoring on either the App or the API
                    var regType = registry.GetType();
                    var baseCreateMethod = regType.GetMethods().FirstOrDefault(m => m.Name == "Create");
                    if (baseCreateMethod != null)
                    {
                        var createMethod =
                            baseCreateMethod.MakeGenericMethod(storableObjects.GetType().GetGenericArguments());

                        var methodInfo = typeof (KspData).GetMethods().Where(m => m.Name == "LoadKspFile").FirstOrDefault(m => m.GetParameters()[1].ParameterType != typeof (string));
                        if (methodInfo != null)
                        {
                            var loadMethopd =
                                methodInfo.MakeGenericMethod(storableObjects.GetType().GetGenericArguments());

                            obj = (IStorable)loadMethopd.Invoke(null, new object[] { path, createMethod.Invoke(registry, null) });
                        }
                    }

                    ImportComplete = true;
                });
        }

        /// <summary>
        /// Gets or sets the file path to be used when loading data. 
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
        /// Gets or sets a value indicating whether the import process has been completed
        /// </summary>
        public bool ImportComplete
        {
            get
            {
                return importComplete;
            }

            set
            {
                importComplete = value;
                OnPropertyChanged("ImportComplete");
            }
        }

        /// <summary>
        /// Gets or sets the object data to import
        /// </summary>
        public IStorable Object
        {
            get
            {
                return obj;
            }

            set
            {
                obj = value;
                OnPropertyChanged("Object");
            }
        }

        /// <summary>
        /// Gets or sets the command for processing import data
        /// </summary>
        public ICommand ImportStorableCommand { get; set; }

        /// <summary>
        /// <see cref="INotifyPropertyChanged"/> event handler requirement implementation. 
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
