// -----------------------------------------------------------------------
// <copyright file="ChangeOrbitDialogView.cs" company="OpenSauceSolutions">
// © 2013 OpenSauce Solutions
// </copyright>
// -----------------------------------------------------------------------

namespace KerbalEdit.Views
{
    using ViewModels;

    /// <summary>
    /// View representing the dialog for changing orbit data
    /// </summary>
    public partial class ChangeOrbitDialogView 
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ChangeOrbitDialogView" /> class.
        /// </summary>	
        /// <param name="vm">model instance to use with the view</param>
        public ChangeOrbitDialogView(ChangeOrbitDialogViewModel vm)
        {
            DataContext = vm;
            InitializeComponent();
        }
    }
}
