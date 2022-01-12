using System;
using System.Windows.Data;

namespace PL
{
    public class IDToBooleanConverter : IValueConverter
    {
        //converts ID textbox to bbol
        //true when 9 digits
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            bool flag = int.TryParse((string)value, out int num);
            if (!flag || num < 100000000)
                return false;
            return true;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class NameToBooleanConverter : IValueConverter
    {
        //converts name textbox to bbol
        //true when not null
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return !String.IsNullOrEmpty((string)value);
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class PhoneToBooleanConverter : IValueConverter
    {
        //converts phone textbox to bbol
        //true when 10 digits
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            string tmp = (string)value;
            if (tmp.Length != 10)
                return false;
            return true;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class ComboxToBooleanConverter : IValueConverter
    {
        //returns true if there is selected item according to SelectedIndex
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return (int)value != -1;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class DroneIDToBooleanConverter : IValueConverter
    {
        //converts ID textbox to bbol
        //true when 4 digits
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            bool flag = int.TryParse((string)value, out int num);
            if (!flag || num < 1000)
                return false;
            return true;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}

