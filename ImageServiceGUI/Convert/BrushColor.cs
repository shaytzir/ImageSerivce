using ImageService.Logging.Modal;
using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;


namespace ImageServiceGUI
{
    class BrushColor : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (targetType != typeof(System.Windows.Media.Brush))
                throw new InvalidOperationException("Convert to brush");

            MessageTypeEnum typeEnum = (MessageTypeEnum)value;
            
            if (typeEnum == MessageTypeEnum.INFO)
            {
                return Brushes.DarkSeaGreen;
            }
            else if (typeEnum == MessageTypeEnum.WARNING)
            {
                return Brushes.Honeydew;
            }
            else
            {
                return Brushes.MediumVioletRed;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new InvalidOperationException();
        }
    }
}
