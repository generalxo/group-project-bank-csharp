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

        public void Info()
        {
            Console.WriteLine($"ID: {id}\nFirst name: {first_name}\nLast name: {last_name}\nPin Code: {pin_code}\nRole ID: {role_id}\nBranch ID: {branch_id}");
            Console.WriteLine("Press any key to continue");
            Console.ReadKey();
        }

    }
}