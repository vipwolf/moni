﻿using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace MONI.Util
{
  public class DurationBGColorConverter : IValueConverter
  {
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture) {
      double duration = (double)value;
      Brush b = Brushes.Transparent;
      if (duration < 8) {
        b = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#B3CD6969"));
      }
      return b;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) {
      throw new NotImplementedException();
    }
  }
}