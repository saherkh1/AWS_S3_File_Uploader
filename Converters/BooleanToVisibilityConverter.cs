﻿using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace AWSFileUploader.Converters
{
    public class BooleanToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value is bool boolValue && boolValue ? Visibility.Visible : Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value is Visibility VisValue && VisValue == Visibility.Visible;
        }
    }
}
