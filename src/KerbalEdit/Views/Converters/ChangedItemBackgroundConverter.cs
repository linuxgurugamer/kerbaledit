// -----------------------------------------------------------------------
// <copyright file="ChangedItemBackgroundConverter.cs" company="OpenSauceSolutions">
// © 2013 OpenSauce Solutions
// </copyright>
// -----------------------------------------------------------------------

namespace KerbalEdit.Views.Converters
{
    using System;
    using System.Windows.Data;
    using System.Windows.Media;

    /// <summary>
    /// Converter used to change the background of an element based upon the provided value (typically the IsDirty property)
    /// </summary>
    public class ChangedItemBackgroundConverter : IValueConverter
    {
        // TODO: There are better ways to do this with pure XAML Im sure, just need to fogure it out

        /// <summary>
        /// Converts the provided value to the target type and culture
        /// </summary>	
        /// <param name="value">value to convert</param>
        /// <param name="targetType">type for return object</param>
        /// <param name="parameter">optional parameter value</param>
        /// <param name="culture">culture to use for type conversions</param>
        /// <returns>type converted value instance</returns>
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value == null || !(value is bool))
            {
                return Brushes.White;
            }

            return (bool)value ? Brushes.LightGoldenrodYellow : Brushes.White;
        }

        /// <summary>
        /// NOT IMPLEMENTED
        /// </summary>
        /// <param name="value">value to convert</param>
        /// <param name="targetType">type for return object</param>
        /// <param name="parameter">optional parameter value</param>
        /// <param name="culture">culture to use for type conversions</param>
        /// <returns>type converted value instance</returns>
        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
