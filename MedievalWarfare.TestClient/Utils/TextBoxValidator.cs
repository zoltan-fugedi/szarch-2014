using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace MedievalWarfare.TestClient.Utils
{
    public class TextBoxValidator : ValidationRule
    {
        public override ValidationResult Validate(object value, System.Globalization.CultureInfo cultureInfo)
        {
            if (value == null)
            {
                return new ValidationResult(false, "value cannot be empty.");
            }
            else
            {
                if (value.ToString().Length > 3)
                    return new ValidationResult(false, "value cannot be more than 3 characters long.");
                int i = 0;
                try
                {
                    i = Convert.ToInt32(value);
                }
                catch (Exception e)
                {

                }
                if (i < 0)
                    return new ValidationResult(false, "value cannot be negative");
            }
            return ValidationResult.ValidResult;
        }
    }
}
