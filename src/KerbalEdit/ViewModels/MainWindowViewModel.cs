// -----------------------------------------------------------------------
// <copyright file="MainWindowViewModel.cs" company="OpenSauceSolutions">
// © 2013 OpenSauce Solutions
// </copyright>
// -----------------------------------------------------------------------

namespace KerbalEdit.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.ComponentModel;
    using System.Windows.Input;
    using System.Windows.Forms;

    using KerbalData;

    /// <summary>
    /// TODO: Class Summary
    /// </summary>
    public class MainWindowViewModel : BaseWindowViewModel, INotifyPropertyChanged
    {
        const string PropNameKerbalData = "Data";
        const string PropNameInstallPath = "InstallPath";
        const string PropNameSelectedItem = "SelectedItem";

        private string installPath;
        private KerbalDataViewModel kerbalData;
        private TreeViewItemViewModel selectedItem;

        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Initializes a new instance of the <see cref="MainWindowViewModel" /> class.
        /// </summary>	
        public MainWindowViewModel()
        {
            Init();
        }

        public ICommand OpenKspInstallFolderCommand { get; private set; }

        public KerbalDataViewModel Data
        {
            get
            {
                return kerbalData;
            }
            set
            {
                if (value != kerbalData)
                {
                    kerbalData = value;
                    OnPropertyChanged(PropNameKerbalData);
                }
            }
        }

        public string InstallPath
        {
            get
            {
                return installPath;
            }
            set
            {
                if (value != installPath)
                {
                    installPath = value;
                    OnPropertyChanged(PropNameInstallPath);
                    UpdateInstallPath();
                }

            }
        }

        private void Init()
        {
            OpenKspInstallFolderCommand = new DelegateCommand(
                () =>
                {
                    var dlg = new FolderBrowserDialog();
                    System.Windows.Forms.DialogResult result = dlg.ShowDialog();

                    if (result.ToString() == "OK")
                    {
                        InstallPath = dlg.SelectedPath;
                    }
                });
        }

        private void UpdateInstallPath()
        {
            Data = null;
            Data = new KerbalDataViewModel(KerbalData.Create(installPath));
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
