namespace group_project_bank_csharp
{
    public class BankAccountModel
    {
        public int id { get; set; }

        public string name { get; set; }

        public decimal interest_rate { get; set; }

        public decimal balance { get; set; }

        public int user_id { get; set; }

        public int currency_id { get; set; }

        public void Info()
        {
            Console.WriteLine($" ID: {id}\n Account name: {name}\n Interest rate: {interest_rate}\n Balance: {balance}\n User ID: {user_id}\n Currency ID: {currency_id}");
        }
    }
}
