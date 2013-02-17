// -----------------------------------------------------------------------
// <copyright file="MappedPropertyViewModel.cs" company="OpenSauceSolutions">
// © 2013 OpenSauce Solutions
// </copyright>
// -----------------------------------------------------------------------

namespace KerbalEdit.ViewModels
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.ComponentModel;
    using System.Linq;
    using System.Reflection;
    using System.Text;

    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;

    using KerbalData;
    using KerbalData.Models;

    public class MappedPropertyViewModel : INotifyPropertyChanged
    {
        private string name;
        private IKerbalDataObject parent;
        private Type type;
        private PropertyInfo property;
        private TreeViewItemViewModel viewModelParent;

        private bool isDirty;

        public MappedPropertyViewModel(string name, IKerbalDataObject parent, TreeViewItemViewModel viewModelParent)
        {
            this.name = name;
            this.parent = parent;
            this.viewModelParent = viewModelParent;

            type = parent.GetType();

            Init();
        }

        public string Name
        {
            get
            {
                return name;
            }
        }

        public object Value
        {
            get
            {
                return property.GetValue(parent);
            }
            set
            {
                property.SetValue(parent, ConvertInput(property.PropertyType, value));
                OnPropertyChanged("Value", value);

                IsDirty = true;
            }
        }

        public bool IsDirty
        {
            get
            {
                return isDirty;
            }

            set
            {
                isDirty = value;
                OnPropertyChanged("IsDirty");

                viewModelParent.IsDirty = true;
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void Init()
        {
            property = type.GetProperty(name);

            if (property == null)
            {
                throw new KerbalEditException("An error has occured while attempting to load object data");
            }
        }

        private void OnPropertyChanged(string name, object value = null)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(name));
            }
        }

        private object ConvertInput(Type type, object value)
        {
            // Base Types
            if (type.Equals(typeof(string)))
            {
                return value.ToString();
            }
            else if (type.Equals(typeof(int)))
            {
                int val;
                if (int.TryParse(value.ToString(), out val))
                {
                    return val;
                }
            }
            else if (type.Equals(typeof(double)))
            {
                double val;
                if (double.TryParse(value.ToString(), out val))
                {
                    return val;
                }
            }
            else if (type.Equals(typeof(decimal)))
            {
                decimal val;
                if (decimal.TryParse(value.ToString(), out val))
                {
                    return val;
                }
            }
            else if (type.Equals(typeof(bool)))
            {
                bool val;
                if (bool.TryParse(value.ToString(), out val))
                {
                    return val;
                }
            }
            else if (type.Equals(typeof(byte)))
            {
                byte val;
                if (byte.TryParse(value.ToString(), out val))
                {
                    return val;
                }
            }
            else if (type.Equals(typeof(char)))
            {
                char val;
                if (char.TryParse(value.ToString(), out val))
                {
                    return val;
                }
            }
            else if (type.Equals(typeof(DateTime)))
            {
                DateTime val;
                if (DateTime.TryParse(value.ToString(), out val))
                {
                    return val;
                }
            }
            else if (type.Equals(typeof(long)))
            {
                long val;
                if (long.TryParse(value.ToString(), out val))
                {
                    return val;
                }
            }
            else if (type.Equals(typeof(short)))
            {
                short val;
                if (short.TryParse(value.ToString(), out val))
                {
                    return val;
                }
            }
            else if (type.Equals(typeof(Single)))
            {
                Single val;
                if (Single.TryParse(value.ToString(), out val))
                {
                    return val;
                }
            }

            return null;
        }
    }
}
