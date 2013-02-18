namespace KerbalEdit.Views
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Data;
    using System.Windows.Documents;
    using System.Windows.Input;
    using System.Windows.Media;
    using System.Windows.Media.Imaging;
    using System.Windows.Shapes;

    using ViewModels;

    /// <summary>
    /// Interaction logic for ExportDataDialogView.xaml
    /// </summary>
    public partial class ExportDataDialogView : Window
    {
        public ExportDataDialogView(ExportDataDialogViewModel vm)
        {
            DataContext = vm;
            InitializeComponent();
        }
    }
}
