// -----------------------------------------------------------------------
// <copyright file="ExportDataDialogViewModel.cs" company="OpenSauceSolutions">
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
    public class ExportDataDialogViewModel : INotifyPropertyChanged
    {
        private string path;
        private IStorable storable;
        private ProcessorRegistry registry; 

        private bool exportComplete;

        /// <summary>
        /// Initializes a new instance of the <see cref="SaveAsDialogViewModel" /> class.
        /// </summary>	
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
                        regType.GetMethods().Where(m => m.Name == "Create").FirstOrDefault().MakeGenericMethod(new Type[] { storable.GetType() });

                    var saveMethod = 
                        typeof(KspData).GetMethods().Where(m => m.Name == "SaveFile").Where(m => m.GetParameters()[2].ParameterType != typeof(string))
                        .FirstOrDefault().MakeGenericMethod(new Type[] { storable.GetType() });

                    saveMethod.Invoke(null, new object[] { path, storable, createMethod.Invoke(registry, null) });
 
                    ExportComplete = true;
                });
        }

        public string Name
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

        public ICommand ExportStorableCommand { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
