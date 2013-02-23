namespace KerbalEdit.Views
{
    using System.Windows;

    using ViewModels;

    /// <summary>
    /// Interaction logic for ExportDataDialogView
    /// </summary>
    public partial class ExportDataDialogView : Window
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MappedPropertyViewModel" /> class.
        /// </summary>	
        /// <param name="vm">model instance to use with the view</param>
        public ExportDataDialogView(ExportDataDialogViewModel vm)
        {
            DataContext = vm;
            InitializeComponent();
        }
    }
}
