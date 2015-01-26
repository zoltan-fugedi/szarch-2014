using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace MedievalWarfare.TestClient.Utils
{
    public class BoolToColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var boolObject = value as bool?;
            if (boolObject.HasValue)
            {
                if (boolObject.Value)
                {
                    return Color.Green;
                }
                else
                {
                    return Color.Red;
                }
            }
            return Color.Red;

        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
