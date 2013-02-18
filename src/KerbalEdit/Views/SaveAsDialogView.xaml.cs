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
    /// Interaction logic for SaveAsDialogView.xaml
    /// </summary>
    public partial class SaveAsDialogView : Window
    {
        public SaveAsDialogView(SaveAsDialogViewModel vm)
        {
            DataContext = vm;
            InitializeComponent();
        }
    }
}
