using Microinvest.CommonLibrary.Enums;
using Microinvest.CommonLibrary.Helpers.Validator;
using System.Text.RegularExpressions;

namespace AxisAvaloniaApp.Services.Validation
{
    public class ValidationService : IValidationService
    {
        private const string digits = @"[\d]";
        private const string letters = @"^[\p{L}]+$";
        private const string lettersAndSpaces = @"((^[\p{L}])|[\s])+$";
        private BaseValidator validator;

        /// <summary>
        /// Initializes a new instance of the <see cref="ValidationService"/> class.
        /// </summary>
        /// <param name="settingsService">Settings of the app to set the validator in according to the country in which the current app is working.</param>
        public ValidationService(Settings.ISettingsService settingsService)
        {
            if (settingsService.Country == ECountries.Bulgaria)
            {
                validator = new BulgariaValidator();
            }
            else if (settingsService.Country == ECountries.Ukraine)
            {
                validator = new UkraineValidator();
            }
            else if (settingsService.Country == ECountries.Georgia)
            {
                validator = new GeorgiaValidator();
            }
            else if (settingsService.Country == ECountries.Russia)
            {
                validator = new RussiaValidator();
            }
            else
            {
                throw new System.Exception(string.Format("Validator for {0} is not supported!", settingsService.Country));
            }
        }

        /// <summary>
        /// Check if a character is a digit.
        /// </summary>
        /// <param name="character">character to check.</param>
        /// <returns>Returns true if a character is a digit; otherwise returns false.</returns>
        /// <date>10.06.2022.</date>
        public bool IsDigit(char character)
        {
            return Regex.IsMatch(character.ToString(), digits);
        }

        // <summary>
        /// Check if a character is a letter.
        /// </summary>
        /// <param name="character">Character to check.</param>
        /// <returns>Returns true if a character is a letter; otherwise returns false.</returns>
        /// <date>10.06.2022.</date>
        public bool IsLetter(char character)
        {
            return Regex.IsMatch(character.ToString(), letters);
        }

        /// <summary>
        /// Check if a character is a letter or space.
        /// </summary>
        /// <param name="character">Character to check.</param>
        /// <returns>Returns true if a character is a letter or space; otherwise returns false.</returns>
        /// <date>10.06.2022.</date>
        public bool IsLetterOrSpace(char character)
        {
            return Regex.IsMatch(character.ToString(), lettersAndSpaces);
        }

        /// <summary>
        /// Check if a text is a IBAN.
        /// </summary>
        /// <param name="iBAN">Text to check.</param>
        /// <returns>Returns true if a text is a IBAN; otherwise returns false.</returns>
        /// <date>10.06.2022.</date>
        public bool IsIBAN(string iBAN)
        {
            return BaseValidator.IsIBAN(iBAN);
        }

        /// <summary>
        /// Check if a text is a tax number.
        /// </summary>
        /// <param name="taxNumber">Text to check.</param>
        /// <returns>Returns true if a text is tax number; otherwise returns false.</returns>
        /// <date>10.06.2022.</date>
        public bool IsTaxNumber(string taxNumber)
        {
            return BaseValidator.IsTaxNumber(ref taxNumber);
        }

        /// <summary>
        /// Check if a text is a VAT number.
        /// </summary>
        /// <param name="vATNumber">Text to check.</param>
        /// <returns>Returns true if a text is a VAT number; otherwise returns false.</returns>
        /// <date>10.06.2022.</date>
        public bool IsVATNumber(string vATNumber)
        {
            return BaseValidator.IsVATNumber(vATNumber);
        }

        /// <summary>
        /// Check if a text is a barcode.
        /// </summary>
        /// <param name="barcode">Text to check.</param>
        /// <returns>Returns true if a text is a barcode; otherwise returns false.</returns>
        /// <date>10.06.2022.</date>
        public bool IsBarcode(string barcode)
        {
            return BaseValidator.IsBarcode(ref barcode);
        }

        /// <summary>
        /// Check if a text is a e-mail.
        /// </summary>
        /// <param name="email">Text to check.</param>
        /// <returns>Returns true if a text is a e-mail; otherwise returns false.</returns>
        /// <date>10.06.2022.</date>
        public bool IsEmail(string email)
        {
            return BaseValidator.IsEMail(email);
        }

        /// <summary>
        /// Check if IBAN is valid for country in which the current app is working.
        /// </summary>
        /// <param name="iBAN">IBAN to validate.</param>
        /// <returns>Returns true if IBAN is valid; otherwise returns false.</returns>
        /// <date>13.06.2022.</date>
        public bool IsValidIBAN(string iBAN)
        {
            return validator.IsValidIBAN(iBAN);
        }

        /// <summary>
        /// Check if tax number is valid for country in which the current app is working.
        /// </summary>
        /// <param name="taxNumber">Tax number to validate.</param>
        /// <returns>Returns true if tax number is valid; otherwise returns false.</returns>
        /// <date>13.06.2022.</date>
        public bool IsValidTaxNumber(string taxNumber)
        {
            return validator.IsValidTaxNumber(ref taxNumber);
        }

        /// <summary>
        /// Check if VAT number is valid for country in which the current app is working.
        /// </summary>
        /// <param name="vATNumber">VAT number to validate.</param>
        /// <returns>Returns true if VAT number is valid; otherwise returns false.</returns>
        /// <date>13.06.2022.</date>
        public bool IsValidVATNumber(string vATNumber)
        {
            return validator.IsValidVATNumber(vATNumber);
        }
    }
}
