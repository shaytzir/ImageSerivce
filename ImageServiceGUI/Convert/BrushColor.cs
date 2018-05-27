using ImageService.Logging.Modal;
using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace ImageServiceGUI
{
    class BrushColor : IValueConverter
    {
        /// <summary>
        /// Define the brush color of the tyoe message of the output by the type of the message.
        /// </summary>
        /// <param name="value">the type of the message</param>
        /// <param name="targetType"></param>
        /// <param name="parameter"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (targetType != typeof(Brush)) {
                throw new InvalidOperationException("Convert to brush");
            }
            //Define the brush color of the tyoe message of the output by the type of the message.
            MessageTypeEnum typeEnum = (MessageTypeEnum)value;
            if (typeEnum == MessageTypeEnum.INFO)
            {
                return Brushes.DarkSeaGreen;
            }
            else if (typeEnum == MessageTypeEnum.WARNING)
            {
                return Brushes.Honeydew;
            } else {
                return Brushes.MediumVioletRed;
            }
        }

        /// <summary>
        /// throw an InvalidOperationException if the targetType isn't of a brush type.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="targetType"></param>
        /// <param name="parameter"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new InvalidOperationException();
        }
    }
}
