// -----------------------------------------------------------------------
// <copyright file="TreeViewItemViewModel.cs" company="OpenSauceSolutions">
// © 2013 OpenSauce Solutions
// </copyright>
// -----------------------------------------------------------------------

namespace KerbalEdit.ViewModels
{
    using System;
    using System.Collections.ObjectModel;
    using System.ComponentModel;
    
    using KerbalData;

    /// <summary>
    /// Base class for all ViewModel classes displayed by TreeViewItems.  
    /// This acts as an adapter between a raw data object and a TreeViewItem.
    /// </summary>
    public class TreeViewItemViewModel : IViewModel
    {   
        private static readonly TreeViewItemViewModel DummyChild = new TreeViewItemViewModel();

        private readonly ObservableCollection<TreeViewItemViewModel> children;
        private readonly IViewModel parent;
        private IKerbalDataObject obj;
        private Type objType;

        private bool isExpanded, isSelected, isDirty;
        private string displayName, toolTip;

        /// <summary>
        /// Initializes a new instance of the <see cref="TreeViewItemViewModel" /> class.
        /// </summary>	
        /// <param name="displayName">UI display name to use</param>
        /// <param name="parent">parent model instance</param>
        /// <param name="lazyLoadChildren">should children objects be lazy or actively loaded</param>
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

        /// <summary>
        /// Gets the data object instance managed by the model
        /// </summary>
        public virtual IKerbalDataObject Object
        {
            get 
            { 
                return obj; 
            }

            protected set 
            { 
                obj = value;
                objType = obj.GetType();
            }
        }

        /// <summary>
        /// Gets the type of the data object managed by the model
        /// </summary>
        public virtual Type ObjectType
        {
            get 
            { 
                return objType; 
            }

            protected set 
            { 
                objType = value; 
            }
        }

        /// <summary>
        /// Gets or sets the UI display name of the model
        /// </summary>
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

        /// <summary>
        /// Gets or sets the UI tool tip of the model
        /// </summary>
        /// <remarks>Not currently used</remarks>
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

                InitData();
            }
        }

        /// <summary>
        /// Returns true if this object's Children have not yet been populated.
        /// </summary>
        public bool HasDummyChild
        {
            get { return Children.Count == 1 && Children[0] == DummyChild; }
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
        /// Get's the parent model instance  as a Tree object if relevant
        /// </summary>
        public TreeViewItemViewModel TreeParent
        {
            get { return parent as TreeViewItemViewModel; }
        }

        /// <summary>
        /// Gets the parent view model instance
        /// </summary>
        public IViewModel Parent
        {
            get { return parent; }
        }

        /// <summary>
        /// Event hook for property change events. 
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Initializes data when lazy loading is enabled
        /// </summary>
        public void InitData()
        {
            if (!HasDummyChild)
            {
                return;
            }

            Children.Remove(DummyChild);
            LoadChildren();
        }

        /// <summary>
        /// Invoked when the child items need to be loaded on demand.
        /// Subclasses can override this to populate the Children collection.
        /// </summary>
        protected virtual void LoadChildren()
        {
        }

        /// <summary>
        /// Internal method to allow parents to force loading when lazy loading is enabled under certain conditions.
        /// </summary>
        protected void RemoveDummyChild()
        {
            if (HasDummyChild)
            {
                Children.Remove(DummyChild);
            }
        }

        /// <summary>
        /// Base selected item method, used to bubble up to all objects related to the selected item
        /// </summary>
        /// <param name="item">item instance that has been selected</param>
        protected virtual void OnSelectedItemChanged(TreeViewItemViewModel item)
        {
            if (TreeParent != null)
            {
                TreeParent.OnSelectedItemChanged(item);
            }
        }

        /// <summary>
        /// Base property changed implementation using standard pattern. 
        /// </summary>
        /// <param name="propertyName">name of value that has changed</param>
        /// <param name="value">instance or value that has changed.</param>
        protected virtual void OnPropertyChanged(string propertyName, object value = null)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
