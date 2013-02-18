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
    using KerbalData.Models;

    using ViewModels;

    /// <summary>
    /// TODO: Class Summary
    /// </summary>
    public partial class MainWindow : Window
    {
        private MainWindowViewModel windowVm;
        // NOTE: I still don't really have the hang of TreeView and how to wireup a dynamic context model. The idea was to avoid repition in eith XAML or CS but that is not the case right now
        // It looks like Blend or a different pattern would be a better choice. This can be hard to read but it works for now. 
        // Some of this should be fixed by changed to the Models in KerbalData's ConsumerAPI

        public MainWindow()
        {
            windowVm = new MainWindowViewModel();
            DataContext = windowVm;
            InitializeComponent();
        }

        private void TreeViewItem_PreviewMouseRightButtonDown(object sender, MouseEventArgs e)
        {
            var item = sender as TreeViewItem;
            var obj = item.Header;

            var vm = obj as TreeViewItemViewModel;

            // This prevents invalid calls and errornous calls like when a user rightclicks on the expansion arrorw.
            if (vm == null)            {
                item.ContextMenu = new ContextMenu() { Visibility = Visibility.Hidden };
                return; 
            }

            vm.InitData();

            // StorableObjectsViewModel{T} - List of Tree Obejects that can be stored/saved
            if (obj.GetType().IsGenericType 
                && obj.GetType().GetGenericTypeDefinition() == typeof(StorableObjectsViewModel<>))
            {
                //var vm = (TreeViewItemViewModel)obj;
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

                item.ContextMenu = menu.Items.Count > 0 ? menu : new ContextMenu() { Visibility = Visibility.Hidden };

                return;
            }

            // StorableObjectViewModel{T} - Individual Tree Obeject that can be stored/saved
            if (obj.GetType().IsGenericType
                && obj.GetType().GetGenericTypeDefinition() == typeof(StorableObjectViewModel<>))
            {
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

                menu.Items.Add(
                    new MenuItem()
                    {
                        Header = "Save As",
                        IsEnabled = true,
                        Command = new DelegateCommand(
                            () =>
                            {
                                (new SaveAsDialogView(new SaveAsDialogViewModel((IStorable)vm.Object)) { Owner = this }).ShowDialog();
                                ((IStorableObjectsViewModel)((TreeViewItemViewModel)vm.Parent)).Refresh();
                            })
                    });

                menu.Items.Add(new Separator());

                menu.Items.Add(
                    new MenuItem()
                    {
                        Header = "Export",
                        IsEnabled = true,
                        Command = new DelegateCommand(
                            () =>
                            {
                                //var registry = 
                                //var registry = ((KerbalDataViewModel)((TreeViewItemViewModel)vm.Parent).Parent).Data.ProcRegistry;
                                (new ExportDataDialogView(new ExportDataDialogViewModel((IStorable)vm.Object, windowVm.Data.Data.ProcRegistry)) { Owner = this }).ShowDialog();
                            })
                    });

                item.ContextMenu = menu.Items.Count > 0 ? menu : new ContextMenu() { Visibility = Visibility.Hidden };

                return;
            }

            // KerbalDataObjectListViewModel - List of Tree Obejects
            if (obj.GetType() == typeof(KerbalDataObjectListViewModel))
            {
                var collection = ((KerbalDataObjectListViewModel)vm).Objects;

                if (collection.Count == 0)
                {
                    return;
                }

                Type itemType = typeof(object);
                var menu = new ContextMenu();

                var list = new List<object>();

                foreach (var colItem in collection)
                {
                    itemType = colItem.GetType();
                    list.Add(colItem);
                }

                // SaveFile Types
                if (itemType == typeof(Resource))
                {
                    menu.Items.Add(
                        new MenuItem()
                        {
                            Header = "Fill",
                            IsEnabled = list.Any(r => !((Resource)r).IsFull),
                            Command = new DelegateCommand(
                                () =>
                                {
                                    foreach (var resource in list)
                                    {
                                        ((Resource)resource).Fill();
                                    }

                                    vm.IsDirty = true;
                                    RefreshDirtyFlag(vm.Parent);
                                })
                        });

                    menu.Items.Add(
                        new MenuItem()
                        {
                            Header = "Empty",
                            IsEnabled = list.Any(r => !((Resource)r).IsEmpty),
                            Command = new DelegateCommand(
                                () =>
                                {
                                    foreach (var resource in list)
                                    {
                                        ((Resource)resource).Empty();
                                    }

                                    vm.IsDirty = true;
                                    RefreshDirtyFlag(vm.Parent);
                                })
                        });
                }

                if (itemType == typeof(Vessel))
                {
                    menu.Items.Add(
                        new MenuItem()
                        {
                            Header = "Fill",
                            IsEnabled = list.Any(v => ((Vessel)v).Parts.Where(p => p.Resources != null && p.Resources.Count > 0).Any(p => p.Resources.Any(r => !r.IsFull))),
                            Command = new DelegateCommand(
                                () =>
                                {
                                    foreach (var vessel in list)
                                    {
                                        ((Vessel)vessel).FillResources();
                                    }

                                    vm.IsDirty = true;
                                    RefreshDirtyFlag(vm.Parent);
                                })
                        });

                    menu.Items.Add(
                        new MenuItem()
                        {
                            Header = "Empty",
                            IsEnabled = list.Any(v => ((Vessel)v).Parts.Where(p => p.Resources != null && p.Resources.Count > 0).Any(p => p.Resources.Any(r => !r.IsEmpty))),
                            Command = new DelegateCommand(
                                () =>
                                {
                                    foreach (var vessel in list)
                                    {
                                        ((Vessel)vessel).EmptyResources();
                                    }

                                    vm.IsDirty = true;
                                    RefreshDirtyFlag(vm.Parent);
                                })
                        });
                }

                item.ContextMenu = menu.Items.Count > 0 ? menu : new ContextMenu() { Visibility = Visibility.Hidden };

                return;
            }

            // KerbalDataObjectViewModel - Individual Tree Obeject
            if (obj.GetType() ==  typeof(KerbalDataObjectViewModel))
            {
                var kdo = vm.Object;
                var kdoType = kdo.GetType();

                var menu = new ContextMenu();

                // Save File Types
                if (kdoType == typeof(Resource))
                {
                    var resource = (Resource)kdo;

                    menu.Items.Add(
                        new MenuItem()
                        {
                            Header = "Fill",
                            IsEnabled = !resource.IsFull,
                            Command = new DelegateCommand(
                                () =>
                                {
                                    resource.Fill();

                                    vm.IsDirty = true;
                                    RefreshDirtyFlag(vm.Parent);
                                })
                        });

                    menu.Items.Add(
                        new MenuItem()
                        {
                            Header = "Empty",
                            IsEnabled = !resource.IsEmpty,
                            Command = new DelegateCommand(
                                () =>
                                {
                                    resource.Empty();

                                    vm.IsDirty = true;
                                    RefreshDirtyFlag(vm.Parent);
                                })
                        });

                    item.ContextMenu = menu.Items.Count > 0 ? menu : new ContextMenu() { Visibility = Visibility.Hidden };

                    return;
                }

                if (kdoType == typeof(FlightState))
                {
                    var fs = (FlightState)kdo;

                    menu.Items.Add(
                        new MenuItem()
                        {
                            Header = "Fill Resources",
                            IsEnabled = fs.Vessels.Any(v => ((Vessel)v).Parts.Where(p => p.Resources != null && p.Resources.Count > 0).Any(p => p.Resources.Any(r => !r.IsFull))),
                            Command = new DelegateCommand(
                                () =>
                                {
                                    fs.FillResources();

                                    vm.IsDirty = true;
                                    RefreshDirtyFlag(vm.Parent);
                                })
                        });

                    menu.Items.Add(
                        new MenuItem()
                        {
                            Header = "Empty Resources",
                            IsEnabled = fs.Vessels.Any(v => ((Vessel)v).Parts.Where(p => p.Resources != null && p.Resources.Count > 0).Any(p => p.Resources.Any(r => !r.IsEmpty))),
                            Command = new DelegateCommand(
                                () =>
                                {
                                    fs.EmptyResources();

                                    vm.IsDirty = true;
                                    RefreshDirtyFlag(vm.Parent);
                                })
                        });

                    menu.Items.Add(new Separator());

                    menu.Items.Add(
                        new MenuItem()
                        {
                            Header = "Clear Debris/Unknown",
                            IsEnabled = true,
                            Command = new DelegateCommand(
                                () =>
                                {
                                    fs.ClearDebris();

                                    var parent =  (TreeViewItemViewModel)vm.Parent;
                                    var vmo = vm.Object;

                                    parent.Children.Remove(vm);
                                    vm = new KerbalDataObjectViewModel(parent, vmo);
                                    parent.Children.Add(vm);

                                    vm.IsDirty = true;
                                    RefreshDirtyFlag(vm.Parent);
                                })
                        });

                    item.ContextMenu = menu.Items.Count > 0 ? menu : new ContextMenu() { Visibility = Visibility.Hidden };

                    return;
                }

                if (kdoType == typeof(Vessel))
                {
                    var vessel = (Vessel)kdo;

                    menu.Items.Add(
                        new MenuItem()
                        {
                            Header = "Fill Resources",
                            IsEnabled = vessel.Parts.Where(p => p.Resources != null && p.Resources.Count > 0).Any(p => p.Resources.Any(r => !r.IsFull)),
                            Command = new DelegateCommand(
                                () =>
                                {
                                    vessel.FillResources();

                                    vm.IsDirty = true;
                                    RefreshDirtyFlag(vm.Parent);
                                })
                        });

                    menu.Items.Add(
                        new MenuItem()
                        {
                            Header = "Empty Resources",
                            IsEnabled = vessel.Parts.Where(p => p.Resources != null && p.Resources.Count > 0).Any(p => p.Resources.Any(r => !r.IsEmpty)),
                            Command = new DelegateCommand(
                                () =>
                                {
                                    vessel.EmptyResources();

                                    vm.IsDirty = true;
                                    RefreshDirtyFlag(vm.Parent);
                                })
                        });

                    menu.Items.Add(new Separator());

                    menu.Items.Add(
                        new MenuItem()
                        {
                            Header = "Change Orbit",
                            IsEnabled = true,
                            Command = new DelegateCommand(
                                () =>
                                {
                                    (new ChangeOrbitDialogView(new ChangeOrbitDialogViewModel(vm.Children.Where(c => c.Object is Orbit).FirstOrDefault())) { Owner = this }).ShowDialog();
                                })
                        });

                    item.ContextMenu = menu.Items.Count > 0 ? menu : new ContextMenu() { Visibility = Visibility.Hidden };

                    return;
                }

                if (kdoType == typeof(Part))
                {
                    var part = (Part)kdo;

                    if (part.Resources != null)
                    {
                        menu.Items.Add(
                            new MenuItem()
                            {
                                Header = "Fill Resources",
                                IsEnabled = part.Resources.Any(r => !r.IsFull),
                                Command = new DelegateCommand(
                                    () =>
                                    {
                                        part.FillResources();

                                        vm.IsDirty = true;
                                        RefreshDirtyFlag(vm.Parent);
                                    })
                            });

                        menu.Items.Add(
                            new MenuItem()
                            {
                                Header = "Empty Resources",
                                IsEnabled = part.Resources.Any(r => !r.IsEmpty),
                                Command = new DelegateCommand(
                                    () =>
                                    {
                                        part.EmptyResources();

                                        vm.IsDirty = true;
                                        RefreshDirtyFlag(vm.Parent);
                                    })
                            });
                    }

                    item.ContextMenu = menu.Items.Count > 0 ? menu : new ContextMenu() { Visibility = Visibility.Hidden };

                    return;
                }

                if (kdoType == typeof(Orbit))
                {
                    var orbit = (Orbit)kdo;

                    menu.Items.Add(
                        new MenuItem()
                        {
                            Header = "Change Orbit",
                            IsEnabled = true,
                            Command = new DelegateCommand(
                                () =>
                                {
                                    // The dialog edits the main data model values directly, there is no need to store any pointers here
                                    (new ChangeOrbitDialogView(new ChangeOrbitDialogViewModel(vm)) { Owner = this }).ShowDialog();
                                })
                        });

                    item.ContextMenu = menu.Items.Count > 0 ? menu : new ContextMenu() { Visibility = Visibility.Hidden };

                    return;
                }
            }

            item.ContextMenu = new ContextMenu() { Visibility = Visibility.Hidden };
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
