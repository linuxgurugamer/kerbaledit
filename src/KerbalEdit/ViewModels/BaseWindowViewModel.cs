// -----------------------------------------------------------------------
// <copyright file="BaseWindowViewModel.cs" company="OpenSauceSolutions">
// © 2013 OpenSauce Solutions
// </copyright>
// -----------------------------------------------------------------------

namespace KerbalEdit.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// TODO: Class Summary
    /// </summary>
    public abstract class BaseWindowViewModel : IViewModel, INotifyPropertyChanged
    {
        private bool isDirty;

        public virtual IViewModel Parent
        {
            // All Window ViewModels are penultimate objects (ie no parent ever)
            get { return null; }
        }

        public virtual  bool IsDirty
        {
            get
            {
                return isDirty;
            }
            set
            {
                isDirty = value;
                OnPropertyChanged("IsDirty");
            }
        }

        public virtual event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string name, object value = null)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(name));
            }
        }

    }
}
