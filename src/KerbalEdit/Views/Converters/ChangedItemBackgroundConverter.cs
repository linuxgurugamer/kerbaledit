// -----------------------------------------------------------------------
// <copyright file="ChangedItemBackgroundConverter.cs" company="OpenSauceSolutions">
// © 2013 OpenSauce Solutions
// </copyright>
// -----------------------------------------------------------------------

namespace KerbalEdit.Views.Converters
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Windows.Data;
    using System.Windows.Media;

    /// <summary>
    /// TODO: Class Summary
    /// </summary>
    public class ChangedItemBackgroundConverter : IValueConverter
    {
        // TODO: There are better ways to do this with pure XAML Im sure, just need to fogure it out
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value == null || !(value is bool))
            {
                return Brushes.White;
            }

            return (bool)value ? Brushes.LightGoldenrodYellow : Brushes.White;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
