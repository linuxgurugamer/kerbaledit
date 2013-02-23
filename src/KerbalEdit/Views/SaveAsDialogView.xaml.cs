namespace KerbalEdit.Views
{
    using ViewModels;

    /// <summary>
    /// Interaction logic for SaveAsDialogView.xaml
    /// </summary>
    public partial class SaveAsDialogView
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SaveAsDialogView" /> class.
        /// </summary>	
        /// <param name="vm">model instance to use with the view</param>
        public SaveAsDialogView(SaveAsDialogViewModel vm)
        {
            DataContext = vm;
            InitializeComponent();
        }
    }
}
