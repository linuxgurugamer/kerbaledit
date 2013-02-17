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
    using System.IO;

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
        //private TreeViewItemViewModel selectedItem;

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
            // Temporary Fix for memory leak. While this is expensive we only do it when loading/unloading a new file/install which should happen.
            // fairly irrigularly
            // TODO: refactor to address leak.
            if (Data != null)
            {
                Data.Dispose();
                Data = null;

                GC.Collect(); // NOTE: this is the devil, only do GC.Collect in extreme circumstances, avoid going to production with any code conatining GC.Collect at all costs.
            }

            if (!string.IsNullOrEmpty(installPath) && Directory.Exists(installPath))
            {
                Data = new KerbalDataViewModel(KerbalData.Create(installPath));
            }
        }
    }
}
