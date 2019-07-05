namespace CurrencyConverter.Tests
{
    using Repository;
    using Xunit;

    public class CurrencyConverterTests
    {
        [Theory]
        [InlineData("0","Amount is not valid",true,true,true)]
        [InlineData("1234.34", "One Thousand Two Hundred and Thirty Four dollars  and Thirty Four cents", true, true, true)]
        [InlineData("123345", "One Hundred and Twenty Three Thousand Three Hundred and Forty Five dollars", true, true, true)]
        [InlineData("12.34567", "Twelve dollars  and Thirty Four Thousand Five Hundred and Sixty Seven cents", true, true, true)]
        [InlineData("12345678901", "Twelve Billion Three Hundred and Forty Five Million Six Hundred and Seventy Eight Thousand Nine Hundred and One dollars", true, true, true)]
        [InlineData("12345678901.98708098098", "Twelve Billion Three Hundred and Forty Five Million Six Hundred and Seventy Eight Thousand Nine Hundred and One dollars  and Ninety Eight Billion Seven Hundred and Eight Million Ninety Eight Thousand Ninety Eight cents", true, true, true)]
        [InlineData("1234_34", "Amount is not valid", true, true, true)]
        [InlineData("-1234.34", "Amount is not valid", true, true, true)]
        [InlineData(".2345", "two thousand three hundred and Forty five cents", true, true, true)]
        public void ConvertCurrencyIntoWords(string amount,string expected,bool ignorecase,bool ignoreLineEndingDifferences,bool ignoreWhiteSpaceDifferences)
        {
            var service = new Converter();
            var retValue = service.ConvertCurrency(amount);
            Assert.Equal(expected, retValue, ignorecase, ignoreLineEndingDifferences, ignoreWhiteSpaceDifferences);
        }
    }
}