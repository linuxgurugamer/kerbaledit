// -----------------------------------------------------------------------
// <copyright file="MainWindowViewModel.cs" company="OpenSauceSolutions">
// © 2013 OpenSauce Solutions
// </copyright>
// -----------------------------------------------------------------------

namespace KerbalEdit.ViewModels
{
    using System;
    using System.IO;
    using System.Windows.Input;
    using System.Windows.Forms;
    using KerbalData;

    /// <summary>
    /// Model to support the top level application window
    /// </summary>
    public class MainWindowViewModel : BaseWindowViewModel
    {
        private const string PropNameKerbalData = "Data";
        private const string PropNameInstallPath = "InstallPath";

        private string installPath;
        private KerbalDataViewModel kerbalData;

        /// <summary>
        /// Initializes a new instance of the <see cref="MainWindowViewModel" /> class.
        /// </summary>	
        public MainWindowViewModel()
        {
            Init();
        }

        /// <summary>
        /// Gets the command hook that handles collecting folder input information using a folder selector.
        /// </summary>
        public ICommand OpenKspInstallFolderCommand
        {
            get; private set;
        }

        /// <summary>
        /// Gets or sets the data model handling KSP data.
        /// </summary>
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

        /// <summary>
        /// Gets or sets the install path for KSP data.
        /// </summary>
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
            // fairly irregularly
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
