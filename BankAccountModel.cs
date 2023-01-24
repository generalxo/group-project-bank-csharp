using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        public void info()
        {
            Console.WriteLine($"ID: {id}\nAccount name: {name}\nInterest rate: {interest_rate}\nBalance: {balance}\nUser ID: {user_id}\nCurrency ID: {currency_id}");
        }
    }
}
