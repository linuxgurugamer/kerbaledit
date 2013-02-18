// -----------------------------------------------------------------------
// <copyright file="ImportDataDialogViewModel.cs" company="OpenSauceSolutions">
// © 2013 OpenSauce Solutions
// </copyright>
// -----------------------------------------------------------------------

namespace KerbalEdit.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Linq;
    using System.Reflection;
    using System.Text;
    using System.Windows.Input;

    using KerbalData;
    using KerbalData.Serialization;

    /// <summary>
    /// TODO: Class Summary
    /// </summary>
    public class ImportDataDialogViewModel : INotifyPropertyChanged
    {
        private string path;
        private IStorableObjects storableObjects;
        private ProcessorRegistry registry;

        private IStorable obj;
        private bool importComplete;

        /// <summary>
        /// Initializes a new instance of the <see cref="SaveAsDialogViewModel" /> class.
        /// </summary>	
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
                    var createMethod =
                        regType.GetMethods().Where(m => m.Name == "Create").FirstOrDefault().MakeGenericMethod(storableObjects.GetType().GetGenericArguments());

                    var loadMethopd =
                        typeof(KspData).GetMethods().Where(m => m.Name == "LoadKspFile").Where(m => m.GetParameters()[1].ParameterType != typeof(string))
                        .FirstOrDefault().MakeGenericMethod(storableObjects.GetType().GetGenericArguments());

                    obj = (IStorable)loadMethopd.Invoke(null, new object[] { path, createMethod.Invoke(registry, null) });
                    
                    ImportComplete = true;
                });
        }

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

        public ICommand ImportStorableCommand { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
