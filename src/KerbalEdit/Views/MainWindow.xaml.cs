// -----------------------------------------------------------------------
// <copyright file="MainWindow.cs" company="OpenSauceSolutions">
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
    using System.Windows.Controls;
    using System.Windows.Data;
    using System.Windows.Documents;
    using System.Windows.Input;
    using System.Windows.Media;
    using System.Windows.Media.Imaging;
    using System.Windows.Shapes;
    using KerbalData;
    using ViewModels;

    /// <summary>
    /// TODO: Class Summary
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void TreeViewItem_PreviewMouseRightButtonDown(object sender, MouseEventArgs e)
        {
            var item = sender as TreeViewItem;
            var obj = item.Header;

            if (obj.GetType().IsGenericType 
                && obj.GetType().GetGenericTypeDefinition() == typeof(StorableObjectsViewModel<>))
            {
                var vm = (TreeViewItemViewModel)obj;
                var menu = new ContextMenu();

                menu.Items.Add(
                    new MenuItem()
                    {
                        Header = "Save",
                        IsEnabled = ((TreeViewItemViewModel)obj).IsDirty,
                        Command = new DelegateCommand(
                            () =>
                            {
                                // IStorable is the base type for any class that can be saved back to a data source. 
                                // At this point we know our object data will be at minmum an implementor of IStorable
                                foreach (var child in vm.Children.Where(c => c.IsDirty))
                                {
                                    ((IStorable)child.Object).Save();
                                }

                                ResetDirtyFlags(vm);
                            })
                    });

                item.ContextMenu = menu;

                return;
            }

            if (obj.GetType().IsGenericType
                && obj.GetType().GetGenericTypeDefinition() == typeof(StorableObjectViewModel<>))
            {
                var vm = (TreeViewItemViewModel)obj;
                var menu = new ContextMenu();

                menu.Items.Add(
                    new MenuItem()
                    {
                        Header = "Save",
                        IsEnabled = ((TreeViewItemViewModel)obj).IsDirty,
                        Command = new DelegateCommand(
                            () =>
                            {
                                // IStorable is the base type for any class that can be saved back to a data source. 
                                // At this point we know our object data will be at minmum an implementor of IStorable
                                var data = (IStorable)vm.Object;
                                data.Save();
                                ResetDirtyFlags(vm);
                                RefreshDirtyFlag(vm.Parent);
                            })
                    });

                item.ContextMenu = menu;

                return;
            }

            if (obj.GetType() == typeof(KerbalDataObjectListViewModel))
            {
                var menu = new ContextMenu();
                menu.Items.Add(new MenuItem() { Header = "KerbalDataObjectListViewModel Items" });
                item.ContextMenu = menu;

                return;
            }

            if (obj.GetType() ==  typeof(KerbalDataObjectViewModel))
            {
                var menu = new ContextMenu();
                menu.Items.Add(new MenuItem() { Header = "KerbalDataObjectViewModel Items" });
                item.ContextMenu = menu;

                return;
            }
        }

        private void RefreshDirtyFlag(IViewModel vm)
        {
            var tvm = vm as TreeViewItemViewModel;
            bool dirty = false;

            if (tvm == null || tvm.Children == null || tvm.Children.Count == 0)
            {
                return;
            }

            foreach (var child in tvm.Children)
            {
                if (child.IsDirty)
                {
                    dirty = true;
                    break;
                }
            }

            vm.IsDirty = dirty;

            if (vm.Parent != null && (dirty != vm.Parent.IsDirty))
            {
                RefreshDirtyFlag(vm.Parent);
            }
        }

        private void ResetDirtyFlags(TreeViewItemViewModel vm)
        {
            vm.IsDirty = false;

            if (vm.Children == null || vm.Children.Count == 0)
            {
                return;
            }

            if (vm.GetType() == typeof(KerbalDataObjectViewModel) ||  vm.GetType().IsSubclassOf(typeof(KerbalDataObjectViewModel)))
            {
                var kvm = (KerbalDataObjectViewModel)vm;

                foreach (var prop in kvm.MappedProperties.Where(p => p.IsDirty))
                {
                    prop.IsDirty = false;
                }

                foreach (var prop in kvm.UnmappedProperties.Where(p => p.IsDirty))
                {
                    prop.IsDirty = false;
                }
            }

            foreach (var child in vm.Children)
            {
                ResetDirtyFlags(child);
            }
        }
    }
}
