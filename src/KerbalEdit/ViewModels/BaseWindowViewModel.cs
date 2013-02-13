// -----------------------------------------------------------------------
// <copyright file="BaseWindowViewModel.cs" company="OpenSauceSolutions">
// © 2013 OpenSauce Solutions
// </copyright>
// -----------------------------------------------------------------------

namespace KerbalEdit.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// TODO: Class Summary
    /// </summary>
    public abstract class BaseWindowViewModel : IViewModel
    {
        public virtual event System.ComponentModel.PropertyChangedEventHandler PropertyChanged; 
        public virtual IViewModel Parent
        {
            // All Window ViewModels are penultimate objects (ie no parent ever)
            get { return null; }
        }


    }
}
