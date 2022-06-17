namespace AxisAvaloniaApp.Services.Validation
{
    public interface IValidationService
    {
        /// <summary>
        /// Check if a character is a digit.
        /// </summary>
        /// <param name="character">Character to check.</param>
        /// <returns>Returns true if a character is a digit; otherwise returns false.</returns>
        /// <date>10.06.2022.</date>
        bool IsDigit(char character);

        /// <summary>
        /// Check if a character is a letter.
        /// </summary>
        /// <param name="character">Character to check.</param>
        /// <returns>Returns true if a character is a letter; otherwise returns false.</returns>
        /// <date>10.06.2022.</date>
        bool IsLetter(char character);

        /// <summary>
        /// Check if a character is a letter or space.
        /// </summary>
        /// <param name="character">Character to check.</param>
        /// <returns>Returns true if a character is a letter or space; otherwise returns false.</returns>
        /// <date>10.06.2022.</date>
        bool IsLetterOrSpace(char character);

        /// <summary>
        /// Check if a text is a IBAN.
        /// </summary>
        /// <param name="iBAN">Text to check.</param>
        /// <returns>Returns true if a text is a IBAN; otherwise returns false.</returns>
        /// <date>10.06.2022.</date>
        bool IsIBAN(string iBAN);

        /// <summary>
        /// Check if a text is a tax number.
        /// </summary>
        /// <param name="taxNumber">Text to check.</param>
        /// <returns>Returns true if a text is tax number; otherwise returns false.</returns>
        /// <date>10.06.2022.</date>
        bool IsTaxNumber(string taxNumber);

        /// <summary>
        /// Check if a text is a VAT number.
        /// </summary>
        /// <param name="vATNumber">Text to check.</param>
        /// <returns>Returns true if a text is a VAT number; otherwise returns false.</returns>
        /// <date>10.06.2022.</date>
        bool IsVATNumber(string vATNumber);

        /// <summary>
        /// Check if a text is a barcode.
        /// </summary>
        /// <param name="barcode">Text to check.</param>
        /// <returns>Returns true if a text is a barcode; otherwise returns false.</returns>
        /// <date>10.06.2022.</date>
        bool IsBarcode(string barcode);

        /// <summary>
        /// Check if a text is a e-mail.
        /// </summary>
        /// <param name="email">Text to check.</param>
        /// <returns>Returns true if a text is a e-mail; otherwise returns false.</returns>
        /// <date>10.06.2022.</date>
        bool IsEmail(string email);

        /// <summary>
        /// Check if IBAN is valid for country in which the current app is working.
        /// </summary>
        /// <param name="iBAN">IBAN to validate.</param>
        /// <returns>Returns true if IBAN is valid; otherwise returns false.</returns>
        /// <date>13.06.2022.</date>
        bool IsValidIBAN(string iBAN);

        /// <summary>
        /// Check if tax number is valid for country in which the current app is working.
        /// </summary>
        /// <param name="taxNumber">Tax number to validate.</param>
        /// <returns>Returns true if tax number is valid; otherwise returns false.</returns>
        /// <date>13.06.2022.</date>
        bool IsValidTaxNumber(string taxNumber);

        /// <summary>
        /// Check if VAT number is valid for country in which the current app is working.
        /// </summary>
        /// <param name="vATNumber">VAT number to validate.</param>
        /// <returns>Returns true if VAT number is valid; otherwise returns false.</returns>
        /// <date>13.06.2022.</date>
        bool IsValidVATNumber(string vATNumber);

        /// <summary>
        /// Check if a text is IP address.
        /// </summary>
        /// <param name="iPAddress">Text to check.</param>
        /// <returns>Returns true if text is IP address; otherwise returns false.</returns>
        /// <date>15.06.2022.</date>
        bool IsValidIPAddress(string iPAddress);

        /// <summary>
        /// Check if a text is IP port.
        /// </summary>
        /// <param name="iPPort">Text to check.</param>
        /// <returns>Returns true if text is IP port; otherwise returns false.</returns>
        /// <date>15.06.2022.</date>
        bool IsValidIPPort(int iPPort);
    }
}
