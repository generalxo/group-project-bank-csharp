namespace group_project_bank_csharp
{
    public class CurrencyConverter
    {
        public int id { get; set; }
        public double exchange_rate { get; set; }
        public string name { get; set; } = string.Empty;
        //public decimal CurrencyAmount { get; set; }

        public double CurrencyConverterCalculatorSomeCurrencyToSEK(double currencyAmount, double exchange_rate)
        {
            return currencyAmount * exchange_rate;

        }
        public double CurrencyConverterCalculatorSEKToSomeCurrency(double currencyAmount, double exchange_rate)
        {
            return currencyAmount / exchange_rate;

        }

        public string Info()
        {
            return $" id: {id}\n name: {name}\n exchange_rate: {exchange_rate}";
        }
    }
}
