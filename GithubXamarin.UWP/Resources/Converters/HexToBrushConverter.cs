using System;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Media;

namespace GithubXamarin.UWP.Resources.Converters
{
    public class HexToBrushConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            return GetSolidColorBrush(value.ToString());
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }

        public SolidColorBrush GetSolidColorBrush(string hexString)
        {
            if (hexString.Length != 7)
            {
                hexString = "FF" + hexString;
            }
            hexString = hexString.Replace("#", string.Empty);

            var a = (byte)(System.Convert.ToUInt32(hexString.Substring(0, 2), 16));
            var r = (byte)(System.Convert.ToUInt32(hexString.Substring(2, 2), 16));
            var g = (byte)(System.Convert.ToUInt32(hexString.Substring(4, 2), 16));
            var b = (byte)(System.Convert.ToUInt32(hexString.Substring(6, 2), 16));
            var myBrush = new SolidColorBrush(Windows.UI.Color.FromArgb(a, r, g, b));

            return myBrush;
        }
    }
}
