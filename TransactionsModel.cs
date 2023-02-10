using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace group_project_bank_csharp
{
    public class TransactionsModel
    {
        public int id { get; set; }

        public string name { get; set; } = string.Empty;

        public int user_id { get; set; }

        public int from_acount_id { get; set; }

        public int to_acount_id { get; set; }

        public DateTime timestamp { get; set; }

        public decimal amount_sender { get; set; }

        public int currency_id_sender { get; set; }

        public decimal amount_receiver { get; set; }

        public int currency_id_receiver { get; set; }
    }

}
