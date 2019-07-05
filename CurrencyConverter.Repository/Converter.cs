namespace CurrencyConverter.Repository
{
    using Interfaces;
    using System;
    using System.Text.RegularExpressions;

    public class Converter : IConverter
    {
        private const string AndString = " and ";
        private const string CentsString = " cents ";
        private const string DollarString = " dollars ";

        public string ConvertCurrency(string amount)
        {
            var centsString = string.Empty;
            var isValid = ValidateCurrency(amount);
            var dollarAmount = amount;

            if (!isValid)
            {
                return "Amount is not valid";
            }

            var centsIndex = amount.IndexOf('.');
            if (centsIndex >= 0)
            {
                dollarAmount = amount.Substring(0, centsIndex);

                var cents = amount.Substring(centsIndex + 1);
                var centValue = ConvertDollarValueToWords(cents);

                centsString = $"{centValue}{CentsString}";
                if (!string.IsNullOrEmpty(dollarAmount))
                {
                    centsString = string.Concat(AndString, centsString);
                }
            }

            var convertedCurrency = string.IsNullOrEmpty(dollarAmount) ? $"{centsString}".TrimEnd() : $"{ConvertDollarValueToWords(dollarAmount)}{DollarString}{centsString}".TrimEnd();

            return convertedCurrency.ToUpperInvariant();
        }

        private string ConvertDollarValueToWords(string dollarValue)
        {
            var word = string.Empty;
            try
            {
                var isConversionDone = false;
                var dblDollarValue = Convert.ToDouble(dollarValue);
                if (dblDollarValue > 0)
                {
                    var numberOfDigits = dollarValue.Length;
                    var pos = 0;
                    var grouping = string.Empty;
                    switch (numberOfDigits)
                    {
                        case 1:
                            word = WordInOnesPosition(dollarValue);
                            isConversionDone = true;
                            break;
                        case 2:
                            word = WordInTensPosition(dollarValue);
                            isConversionDone = true;
                            break;
                        case 3:
                            pos = numberOfDigits % 3 + 1;
                            grouping = " Hundred ";
                            break;
                        case 4:
                        case 5:
                        case 6:
                            pos = numberOfDigits % 4 + 1;
                            grouping = " Thousand ";
                            break;
                        case 7:
                        case 8:
                        case 9:
                            pos = numberOfDigits % 7 + 1;
                            grouping = " Million ";
                            break;
                        case 10:
                        case 11:
                        case 12:

                            pos = numberOfDigits % 10 + 1;
                            grouping = " Billion ";
                            break;
                        case 13:
                            pos = numberOfDigits % 13 + 1;
                            grouping = " Trillion ";
                            break;
                        default:
                            isConversionDone = true;
                            break;
                    }

                    if (string.Equals(grouping, " Hundred "))
                    {
                        grouping = string.Concat(grouping, "and ");
                    }
                    if (!isConversionDone)
                    {
                        if (dollarValue.Substring(0, pos) != "0" && dollarValue.Substring(pos) != "0")
                        {
                            try
                            {
                                word = ConvertDollarValueToWords(dollarValue.Substring(0, pos)) + grouping +
                                       ConvertDollarValueToWords(dollarValue.Substring(pos));
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine(ex.Message);
                            }
                        }
                        else
                        {
                            word = ConvertDollarValueToWords(dollarValue.Substring(0, pos)) +
                                   ConvertDollarValueToWords(dollarValue.Substring(pos));
                        }
                    }

                    if (word.Trim().Equals(grouping.Trim()))
                    {
                        word = string.Empty;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return word.Trim();
        }

        private string WordInOnesPosition(string amount)
        {
            var number = Convert.ToInt32(amount);
            var name = string.Empty;
            switch (number)
            {
                case 1:
                    name = "One";
                    break;
                case 2:
                    name = "Two";
                    break;
                case 3:
                    name = "Three";
                    break;
                case 4:
                    name = "Four";
                    break;
                case 5:
                    name = "Five";
                    break;
                case 6:
                    name = "Six";
                    break;
                case 7:
                    name = "Seven";
                    break;
                case 8:
                    name = "Eight";
                    break;
                case 9:
                    name = "Nine";
                    break;
            }

            return name;
        }

        private string WordInTensPosition(string amount)
        {
            var number = Convert.ToInt32(amount);
            string name = null;
            switch (number)
            {
                case 10:
                    name = "Ten";
                    break;
                case 11:
                    name = "Eleven";
                    break;
                case 12:
                    name = "Twelve";
                    break;
                case 13:
                    name = "Thirteen";
                    break;
                case 14:
                    name = "Fourteen";
                    break;
                case 15:
                    name = "Fifteen";
                    break;
                case 16:
                    name = "Sixteen";
                    break;
                case 17:
                    name = "Seventeen";
                    break;
                case 18:
                    name = "Eighteen";
                    break;
                case 19:
                    name = "Nineteen";
                    break;
                case 20:
                    name = "Twenty";
                    break;
                case 30:
                    name = "Thirty";
                    break;
                case 40:
                    name = "Forty";
                    break;
                case 50:
                    name = "Fifty";
                    break;
                case 60:
                    name = "Sixty";
                    break;
                case 70:
                    name = "Seventy";
                    break;
                case 80:
                    name = "Eighty";
                    break;
                case 90:
                    name = "Ninety";
                    break;
                default:
                    if (number > 0)
                    {
                        name = WordInTensPosition(amount.Substring(0, 1) + "0") + " " +
                               WordInOnesPosition(amount.Substring(1));
                    }

                    break;
            }

            return name;
        }

        private bool ValidateCurrency(string enteredCurrency)
        {
            var isValid = enteredCurrency != "0";
            if (isValid)
            {
                var regex = @"^(\d+)?(\.\d+)?$";
                var match = Regex.Match(enteredCurrency, regex, RegexOptions.IgnoreCase);
                if (!match.Success)
                {
                    isValid = false;
                }
            }

            return isValid;
        }
    }
}