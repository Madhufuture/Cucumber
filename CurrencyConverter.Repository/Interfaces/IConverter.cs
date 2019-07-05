namespace CurrencyConverter.Repository.Interfaces
{
    using System.Threading.Tasks;

    public interface IConverter
    {
        string ConvertCurrency(string amount);
    }
}