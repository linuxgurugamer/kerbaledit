// -----------------------------------------------------------------------
// <copyright file="IsDirtyFontStyleConverter.cs" company="OpenSauceSolutions">
// © 2013 OpenSauce Solutions
// </copyright>
// -----------------------------------------------------------------------

namespace KerbalEdit.Views.Converters
{
    using System;
    using System.Windows;
    using System.Windows.Data;


    /// <summary>
    /// Conversion of IsDirty flag to Oblique or Normal font formats for display.
    /// </summary>
    public class IsDirtyFontStyleConverter : IValueConverter
    {
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
                return FontStyles.Normal;
            }

            return (bool)value ? FontStyles.Oblique : FontStyles.Normal;
        }

        /// <summary>
        /// NOT IMPLMENTED
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
