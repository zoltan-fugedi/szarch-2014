using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace MedievalWarfare.TestClient.Utils
{
    public class TypeToTabIndexConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string format = value as string;
            if (format == "Unit") { return 0; }
            if (format == "Building") { return 1; }
            if (format == "Treasure") { return 2; }
            return -1;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
