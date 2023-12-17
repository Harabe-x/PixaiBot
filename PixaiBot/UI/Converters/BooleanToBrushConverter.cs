﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Media;

namespace PixaiBot.UI.Converters
{
    internal class BooleanToBrushConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null) // if value is null return Regular brush
                return new SolidColorBrush((Color)ColorConverter.ConvertFromString("#8964ff"));

            return (bool) value 
                ? new SolidColorBrush((Color)ColorConverter.ConvertFromString("#e74c3c")) // if value is true returns red brush
                : new SolidColorBrush((Color)ColorConverter.ConvertFromString("#8964ff")); // if value is false returns regular purple brush
                    
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();    
        }
    }
}