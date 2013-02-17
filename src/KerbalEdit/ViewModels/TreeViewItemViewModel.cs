// -----------------------------------------------------------------------
// <copyright file="TreeViewItemViewModel.cs" company="OpenSauceSolutions">
// © 2013 OpenSauce Solutions
// </copyright>
// -----------------------------------------------------------------------

namespace KerbalEdit.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.ComponentModel;
    using System.Linq;
    using System.Text;
using KerbalData;

    /// <summary>
    /// Base class for all ViewModel classes displayed by TreeViewItems.  
    /// This acts as an adapter between a raw data object and a TreeViewItem.
    /// </summary>
    public class TreeViewItemViewModel : IViewModel, INotifyPropertyChanged
    {   
        private static readonly TreeViewItemViewModel DummyChild = new TreeViewItemViewModel();

        private readonly ObservableCollection<TreeViewItemViewModel> children;
        private readonly IViewModel parent;
        private IKerbalDataObject obj;

        private bool isExpanded, isSelected, isDirty;
        private string displayName, toolTip;

        protected TreeViewItemViewModel(string displayName, IViewModel parent = null, bool lazyLoadChildren = true)
        {
            DisplayName = displayName;
            this.parent = parent;

            children = new ObservableCollection<TreeViewItemViewModel>();

            if (lazyLoadChildren)
                children.Add(DummyChild);
        }

        // This is used to create the DummyChild instance.
        private TreeViewItemViewModel()
        {
        }

        public virtual IKerbalDataObject Object
        {
            get { return obj; }
            protected set { obj = value; }
        }

        public string DisplayName
        {
            get
            {
                return displayName;
            }
            set
            {
                displayName = value;
                OnPropertyChanged("DisplayName");
            }
        }

        public string ToolTip
        {
            get
            {
                return toolTip;
            }
            set
            {
                toolTip = value;
                OnPropertyChanged("ToolTip");
            }
        }

        /// <summary>
        /// Gets/sets whether the TreeViewItem 
        /// associated with this object is expanded.
        /// </summary>
        public virtual bool IsExpanded
        {
            get { return isExpanded; }
            set
            {
                if (value != isExpanded)
                {
                    isExpanded = value;
                    OnPropertyChanged("IsExpanded");
                }

                // Expand all the way up to the root.
                if (isExpanded && parent != null && parent is TreeViewItemViewModel)
                {
                    ((TreeViewItemViewModel)parent).IsExpanded = true;
                }

                // Lazy load the child items, if necessary.
                if (this.HasDummyChild)
                {
                    Children.Remove(DummyChild);
                    LoadChildren();
                }
            }
        }

        protected void RemoveDummyChild()
        {
            if (HasDummyChild)
            {
                Children.Remove(DummyChild);
            }
        }

        /// <summary>
        /// Returns true if this object's Children have not yet been populated.
        /// </summary>
        public bool HasDummyChild
        {
            get { return Children.Count == 1 && Children[0] == DummyChild; }
        }

        protected virtual void OnSelectedItemChanged(TreeViewItemViewModel item)
        {
            if (TreeParent != null)
            {
                TreeParent.OnSelectedItemChanged(item);
            }
        }

        /// <summary>
        /// Returns the logical child items of this object.
        /// </summary>
        public virtual ObservableCollection<TreeViewItemViewModel> Children
        {
            get { return children; }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the TreeViewItem 
        /// associated with this object is selected.
        /// </summary>
        public virtual bool IsSelected
        {
            get { return isSelected; }
            set
            {
                if (value != isSelected)
                {
                    isSelected = value;
                    OnPropertyChanged("IsSelected");

                    if (isSelected)
                    {
                        OnSelectedItemChanged(this);
                    }
                }
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the TreeViewItem 
        /// associated with this object is selected.
        /// </summary>
        public virtual bool IsDirty
        {
            get { return isDirty; }
            set
            {
                isDirty = value;
                OnPropertyChanged("IsDirty");

                if (isDirty && parent != null && parent is IViewModel)
                {
                    parent.IsDirty = true;
                }
            }
        }

        /// <summary>
        /// Invoked when the child items need to be loaded on demand.
        /// Subclasses can override this to populate the Children collection.
        /// </summary>
        protected virtual void LoadChildren()
        {
        }

        
        public TreeViewItemViewModel TreeParent
        {
            get { return parent as TreeViewItemViewModel; }
        }

        public IViewModel Parent
        {
            get { return parent; }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName, object value = null)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
