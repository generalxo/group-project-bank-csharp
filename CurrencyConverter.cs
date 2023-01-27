namespace group_project_bank_csharp
{
    public class CurrencyConverter
    {
        public int id { get; set; }
        public double exchange_rate { get; set; }
        public string name { get; set; } = string.Empty;
        //public decimal CurrencyAmount { get; set; }

        public double CurrencyConverterCalculatorDollarToSEK(double currencyAmount, double exchange_rate)
        {
            return currencyAmount * exchange_rate;

        }
        public double CurrencyConverterCalculatorSEKToDollar(double currencyAmount, double exchange_rate)
        {
            return currencyAmount / exchange_rate;

        }

        public string PrintCurrencyConverterToDollar(double currencyAmount)
        {
            return $"\n\tExchange Rate:{exchange_rate}\n\texchanged value: {CurrencyConverterCalculatorSEKToDollar(currencyAmount, exchange_rate)}";
        }

        public string PrintCurrencyConverterToSek(double currencyAmount)
        {
            return $"\n\tExchange Rate:{exchange_rate}\n\texchanged value: {CurrencyConverterCalculatorSEKToDollar(currencyAmount, exchange_rate)}";
        }
    }
}
