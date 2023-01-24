using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace group_project_bank_csharp
{
    public class UserModel
    {
        public int id { get; set; }

        public string first_name { get; set; }

        public string last_name { get; set; }

        public string pin_code { get; set; }

        public int role_id { get; set; }

        public int branch_id { get; set; }

        public void info()
        {
            Console.WriteLine($"ID: {id}\nFirst name: {first_name}\nLast name: {last_name}\nPin Code: {pin_code}\nRole ID: {role_id}\nBranch ID: {branch_id}");
        }

        /*
        public string FullName
        {
            get
            {
                return $"{FirstName} {LastName}";
            }
        }
        */
    }
}