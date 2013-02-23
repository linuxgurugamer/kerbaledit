// -----------------------------------------------------------------------
// <copyright file="BooleanVisbilityConverter.cs" company="OpenSauceSolutions">
// © 2013 OpenSauce Solutions
// </copyright>
// -----------------------------------------------------------------------

namespace KerbalEdit.Views.Converters
{
    using System;
    using System.Windows;
    using System.Windows.Data;

    /// <summary>
    /// Converts a Boolean value into a visibility value
    /// </summary>
    public class BooleanVisbilityConverter : IValueConverter
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
            var param = (Visibility)parameter;

            var trueParam = param == Visibility.Visible ? Visibility.Visible : Visibility.Hidden;
            var falseParam = param == Visibility.Visible ? Visibility.Hidden : Visibility.Visible;

            if (value == null || !(value is bool))
            {
                return false;
            }

            return (bool)value ? trueParam : falseParam;            
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
