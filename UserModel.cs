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