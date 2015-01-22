using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace MedievalWarfare.Client
{
    public class PlayerNameRule : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {

            if (value == null)
                return new ValidationResult(false, "Name cannot be empty.");
            else
            {
                var match =Regex.Match(((string)value), "\\w*", RegexOptions.IgnoreCase);

                if (match.Length != ((string)value).Length)
                {
                    return new ValidationResult(false, "Name contains illegal characters. It can only contain a-z, A-Z, 0-9 and _");

                }
            }
            return ValidationResult.ValidResult;
            
        }


    }
}
