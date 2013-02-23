namespace KerbalEdit.Views
{
    using ViewModels;

    /// <summary>
    /// Interaction logic for ImportDataDialogView.xaml
    /// </summary>
    public partial class ImportDataDialogView
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ImportDataDialogView" /> class.
        /// </summary>	
        /// <param name="vm">model instance to use with the view</param>
        public ImportDataDialogView(ImportDataDialogViewModel vm)
        {
            DataContext = vm;
            InitializeComponent();
        }
    }
}
