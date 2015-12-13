using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Data;
using System.Windows;

namespace FBDMaker
{
   
    [ValueConversion(typeof(Double), typeof(Double))]
    public class WPFConvCellsWith : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            //Double proc = Double.Parse((String)parameter);
            return ((Double)value)-17;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {

            return null;
        }
    }

    [ValueConversion(typeof(Double), typeof(Double))]
    public class WPFConvCheckVisibl: IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            //Double proc = Double.Parse((String)parameter);
            return ((Boolean)value? Visibility.Visible: Visibility.Collapsed ) ;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {

            return null;
        }
    }
}
