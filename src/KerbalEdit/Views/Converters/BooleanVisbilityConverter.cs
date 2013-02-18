// -----------------------------------------------------------------------
// <copyright file="BooleanVisbilityConverter.cs" company="OpenSauceSolutions">
// © 2013 OpenSauce Solutions
// </copyright>
// -----------------------------------------------------------------------

namespace KerbalEdit.Views.Converters
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Windows;
    using System.Windows.Data;

    /// <summary>
    /// TODO: Class Summary
    /// </summary>
    public class BooleanVisbilityConverter : IValueConverter
    {
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

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
