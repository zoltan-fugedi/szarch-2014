using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media.Imaging;

namespace MedievalWarfare.TestClient.Utils
{
    class StringToImageConverter :IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string format = value as string;
            if (format == "Unit")
            {
                return Application.Current.FindResource("unit_yellow") as BitmapImage;
                
            }
            if (format == "Building")
            {
                return Application.Current.FindResource("castle") as BitmapImage;

            }
            if (format == "Treasure")
            {
                return Application.Current.FindResource("coin_game") as BitmapImage;

            }
            return Application.Current.FindResource("blank") as BitmapImage;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
