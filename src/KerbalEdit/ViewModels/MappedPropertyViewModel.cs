// -----------------------------------------------------------------------
// <copyright file="MappedPropertyViewModel.cs" company="OpenSauceSolutions">
// © 2013 OpenSauce Solutions
// </copyright>
// -----------------------------------------------------------------------

namespace KerbalEdit.ViewModels
{
    using System;
    using System.ComponentModel;
    using System.Reflection;

    using KerbalData;

    /// <summary>
    /// Model for a Mapped Property on a <see cref="IKerbalDataObject"/>
    /// </summary>
    public class MappedPropertyViewModel : INotifyPropertyChanged
    {
        private readonly string name;
        private readonly IKerbalDataObject parent;
        private readonly Type type;
        private readonly TreeViewItemViewModel viewModelParent;

        private PropertyInfo property;
        private bool isDirty;

        /// <summary>
        /// Initializes a new instance of the <see cref="MappedPropertyViewModel" /> class.
        /// </summary>	
        /// <param name="name">Name of the property</param>
        /// <param name="parent">object containing the mapped properties</param>
        /// <param name="viewModelParent">model that manages this instance</param>
        public MappedPropertyViewModel(string name, IKerbalDataObject parent, TreeViewItemViewModel viewModelParent)
        {
            this.name = name;
            this.parent = parent;
            this.viewModelParent = viewModelParent;

            type = parent.GetType();

            if (type.IsSubclassOf(typeof(KerbalDataObject)))
            {
                ((KerbalDataObject)parent).PropertyChanged += kdo_PropertyChanged;
            }

            Init();
        }

        /// <summary>
        /// Gets the name of the property. 
        /// </summary>
        /// <remarks>
        /// <para>Property is read only as Mapped Properties come from typed model properties which have set names. The attribute data on these properties ensures that the naming when saved to a file is correct.</para>
        /// </remarks>
        public string Name
        {
            get
            {
                return name;
            }
        }

        /// <summary>
        /// Gets or sets the value data of the property.
        /// </summary>
        /// <exception cref="KerbalEditException">thrown when provided value cannot be converted to target type for property name</exception>
        /// <remarks>
        /// <para>Stores the data into the underlying mapped property on the model. Type conversion is automatically handled by this property's mapping. If this value is bound to an open control and the user provides a value that cannot be converted an exception will be thrown.</para>
        /// </remarks>
        public object Value
        {
            get
            {
                return property.GetValue(parent, BindingFlags.GetProperty, null, null, null);
            }

            set
            {
                try
                {
                    property.SetValue(parent, ConvertInput(property.PropertyType, value), BindingFlags.SetProperty, null, null, null);
                }
                catch (Exception ex)
                {
                    throw new KerbalEditException("An error has occurred converting the provided value to the correct type, see the inner exception for details.", ex);
                }

                OnPropertyChanged("Value", value);

                IsDirty = true;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the mapped property data has been changed
        /// </summary>
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
                
                if (viewModelParent.IsDirty != isDirty)
                {
                    viewModelParent.IsDirty = isDirty;
                }
            }
        }

        /// <summary>
        /// Event hook for changes to properties
        /// </summary>
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

        private static object ConvertInput(Type type, object value)
        {
            // Base Types
            if (type == typeof(string))
            {
                return value.ToString();
            }
            else if (type == typeof(int))
            {
                int val;
                if (int.TryParse(value.ToString(), out val))
                {
                    return val;
                }
            }
            else if (type == typeof(double))
            {
                double val;
                if (double.TryParse(value.ToString(), out val))
                {
                    return val;
                }
            }
            else if (type == typeof(decimal))
            {
                decimal val;
                if (decimal.TryParse(value.ToString(), out val))
                {
                    return val;
                }
            }
            else if (type == typeof(bool))
            {
                bool val;
                if (bool.TryParse(value.ToString(), out val))
                {
                    return val;
                }
            }
            else if (type == typeof(byte))
            {
                byte val;
                if (byte.TryParse(value.ToString(), out val))
                {
                    return val;
                }
            }
            else if (type == typeof(char))
            {
                char val;
                if (char.TryParse(value.ToString(), out val))
                {
                    return val;
                }
            }
            else if (type == typeof(DateTime))
            {
                DateTime val;
                if (DateTime.TryParse(value.ToString(), out val))
                {
                    return val;
                }
            }
            else if (type == typeof(long))
            {
                long val;
                if (long.TryParse(value.ToString(), out val))
                {
                    return val;
                }
            }
            else if (type == typeof(short))
            {
                short val;
                if (short.TryParse(value.ToString(), out val))
                {
                    return val;
                }
            }
            else if (type == typeof(Single))
            {
                Single val;
                if (Single.TryParse(value.ToString(), out val))
                {
                    return val;
                }
            }

            return null;
        }

        private void kdo_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == Name)
            {
                OnPropertyChanged("Value", Value);
                IsDirty = true;
            }
        }
    }
}
