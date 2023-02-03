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

        public enum UserRoles
        {
            admin = 1,
            client = 2,
            clientAdmin = 3
        }

        enum UserBranchs
        {
            Stockholm = 1,
            Malmö = 2
        }

        public static string GetInputFirstName()
        {
            //get a valid first name
            Console.Write("\nPlease enter FirstName: ");
            string? firstName = Console.ReadLine();
            while (firstName == "" || firstName == null)
            {
                Program.InvalidInput(firstName);
                Console.Write("\nPlease enter FirstName: ");
                firstName = Console.ReadLine();
            }
            return firstName;
        }

        public static string GetInputLastName()
        {
            //get a valid last name
            Console.Write("\nPlease enter LastName: ");
            string? lastName = Console.ReadLine();
            while (lastName == "" || lastName == null)
            {
                Program.InvalidInput(lastName);
                Console.Write("\nPlease enter LastName: ");
                lastName = Console.ReadLine();
            }
            return lastName;
        }

        public static string GetInputPinCode()
        {
            //get a valid pincode with 4 digits
            Console.Write("\nPlease enter PinCode: ");
            string? pinCode = Console.ReadLine();
            while (pinCode?.Length != 4)
            {
                Program.InvalidInput(pinCode);
                Console.WriteLine("You need to enter a 4-digit pincode");
                Console.Write("\nPlease enter PinCode: ");
                pinCode = Console.ReadLine();
            }
            return pinCode;
        }

        public static int GetInputRoleID()
        {
            //get a valid role id
            Console.Write("\nPlease enter Role ID: ");
            int.TryParse(Console.ReadLine(), out int roleId);
            while (roleId != (int)UserRoles.admin && roleId != (int)UserRoles.client && roleId != (int)UserRoles.clientAdmin)
            {
                Program.InvalidInput(roleId.ToString());
                Console.WriteLine("You need to enter a valid Role ID");
                Console.Write("\nPlease enter Role ID: ");
                int.TryParse(Console.ReadLine(), out roleId);
            }
            return roleId;
        }

        public static int GetInputBranchId()
        {
            //get a valid branch id
            Console.Write("\nPlease enter Branch ID: ");
            int.TryParse(Console.ReadLine(), out int branchId);
            while (branchId != (int)UserBranchs.Stockholm && branchId != (int)UserBranchs.Malmö)
            {
                Program.InvalidInput(branchId.ToString());
                Console.WriteLine("You need to enter a valid Branch ID");
                Console.Write("\nPlease enter Branch ID: ");
                int.TryParse(Console.ReadLine(), out branchId);
            }
            return branchId;
        }



    }
}