namespace group_project_bank_csharp
{
    public class UserModel
    {
        public int id { get; set; }

        public string? first_name { get; set; } = string.Empty;

        public string? last_name { get; set; } = string.Empty;

        public string? pin_code { get; set; } = string.Empty;

        public int role_id { get; set; }

        public int branch_id { get; set; }

        public void Info()
        {
            Console.WriteLine($" ID: {id}\n First name: {first_name}\n Last name: {last_name}\n Pin Code: {pin_code}\n Role ID: {role_id}\n Branch ID: {branch_id}");
            Console.WriteLine(" Press any key to continue");
            Console.ReadKey();
        }

    }
}