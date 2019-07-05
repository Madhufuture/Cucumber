namespace CurrencyConverter.ViewModels
{
    using System.ComponentModel.DataAnnotations;

    public class UserDetailsVm
    {
        [Required]
        public string Name { get; set; }

        [DataType(DataType.Currency)] public float Amount { get; set; }
    }
}