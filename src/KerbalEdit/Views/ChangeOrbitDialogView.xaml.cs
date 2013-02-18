// -----------------------------------------------------------------------
// <copyright file="ChangeOrbitDialogView.cs" company="OpenSauceSolutions">
// © 2013 OpenSauce Solutions
// </copyright>
// -----------------------------------------------------------------------

namespace KerbalEdit.Views
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Windows;

    using ViewModels;

    /// <summary>
    /// TODO: Class Summary
    /// </summary>
    public partial class ChangeOrbitDialogView : Window
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ChangeOrbitDialogView" /> class.
        /// </summary>	
        public ChangeOrbitDialogView(ChangeOrbitDialogViewModel vm)
        {
            DataContext = vm;
            InitializeComponent();
        }
    }
}
