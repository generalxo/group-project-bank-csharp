using System.Runtime.InteropServices;
using System.Security.Principal;

namespace group_project_bank_csharp
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine(" Press any key to continue");
            Console.ReadKey();

            StartMenu();
        }

        #region StartMenu, BankMenu, AsciiArt, DrawMenu, Help
        public static void AsciiArt()
        {
            Console.Clear();

            string asciiLayer1 = "         .___,   ";
            string asciiLayer2 = "      ___('v')___";
            string asciiLayer3 = "      '\"-\\._./-\"'";
            string asciiLayer4 = "          ^ ^    ";

            Console.WriteLine();
            Console.WriteLine(asciiLayer1);
            Console.WriteLine(asciiLayer2);
            Console.WriteLine(asciiLayer3);
            Console.WriteLine(asciiLayer4);
            Console.WriteLine();
            Console.WriteLine(" Welcome to Owl Banking");
            Console.WriteLine(" ----------------------");

            Help();

        }

        static int menuIndex = 0;
        public static void StartMenu()
        {
            Console.CursorVisible = false;
            AsciiArt();

            string menuMsg = " Welcome to Owl Banking\n Please select an option";

            // Declaration of list and items.
            List<string> menuItems = new()
            {
                "Log in",
                "Help",
                "Exit"
            };

            while (true)
            {
                string selectedMenuItem = DrawMenu(menuItems, menuMsg);
                //Switch for the selectedMenuItem string that DrawMenu returns
                switch (selectedMenuItem)
                {
                    case "Log in":
                        Login();

                        //Console.WriteLine(" Login would start here");
                        //Console.WriteLine(" Enter any key to continue");
                        //Console.ReadKey();
                        break;
                    case "Help":
                        Help();
                        break;
                    case "Exit":
                        Environment.Exit(0);
                        break;
                }
            }

        }

        static void BankMenu(List<UserModel> currentUser)
        {
            bool runMenu = true;
            string menuMsg = " Welcome to Owl Banking\n Please select an option";

            List<string> menuItems = new()
            {
                "Balance",
                "Transfer",
                "Withdraw",
                "Loan",
                "Account",
                "Logout"
            };

            while (runMenu)
            {
                string selectedMenuItems = DrawMenu(menuItems, menuMsg);
                switch (selectedMenuItems)
                {
                    case "Balance":
                        //Console.WriteLine(" Balance Would start here");
                        //Console.WriteLine(" Press any key to continue");
                        //Console.ReadKey();
                        List<BankAccountModel> bankAccounts = SQLconnection.LoadBankAccounts(currentUser[0].id);

                        for (int i = 0; i < bankAccounts.Count; i++)
                        {
                            //Console.WriteLine(bankAccounts[i].balance);
                            bankAccounts[i].Info();
                        }
                        Console.WriteLine(" Press any key to continue");
                        Console.ReadKey();

                        break;
                    case "Transfer":
                        
                        Console.WriteLine(" Transfer would start here");
                        Console.WriteLine(" Press any key to continue");
                        Console.ReadKey();
                        break;
                    case "Withdraw":
                        Deposit(currentUser[0].id);
                        Console.WriteLine(" Press any key to continue");
                        Console.ReadKey();
                        break;
                    case "Loan":
                        Console.WriteLine(" Loan would start here");
                        Console.WriteLine(" Press any key to continue");
                        Console.ReadKey();
                        break;
                    case "Account":
                        OpenAccount(currentUser[0].id);
                        Console.WriteLine(" Press any key to continue");
                        Console.ReadKey();
                        break;
                    case "Logout":
                        menuIndex = 0;
                        runMenu = false;
                        break;
                }
            }
        }

        public static string DrawMenu(List<string> menuItem, string menuMsg)
        {
            //Method takes in a List of strings and displays them. The static int menuIndex indicates which item the user is selecting
            //Method allows user to navigare the List of strings and select a string and return it

            Console.Clear();
            Console.WriteLine("");

            //Msg that can be displayed in menu
            Console.WriteLine(menuMsg);
            Console.WriteLine("");

            //looping through menuItems and displaying them. if menuIndex == i add "["  "]" to menuItem[i]
            for (int i = 0; i < menuItem.Count; i++)
            {
                if (i == menuIndex)
                {
                    Console.WriteLine($"[{menuItem[i]}]");
                }
                else
                {
                    Console.WriteLine($" {menuItem[i]} ");
                }
            }

            //Check for key input
            ConsoleKeyInfo ckey = Console.ReadKey();

            //Down arrow key check
            if (ckey.Key == ConsoleKey.DownArrow)
            {
                if (menuIndex == menuItem.Count - 1) { }
                else { menuIndex++; }
            }
            //Up arrow key check
            else if (ckey.Key == ConsoleKey.UpArrow)
            {
                if (menuIndex <= 0) { }
                else { menuIndex--; }
            }
            //Left arrow key check
            else if (ckey.Key == ConsoleKey.LeftArrow)
            {
                //Console.Clear();
            }
            //Right arrow key check
            else if (ckey.Key == ConsoleKey.RightArrow)
            {
                return menuItem[menuIndex];
            }
            //Enter key check
            else if (ckey.Key == ConsoleKey.Enter)
            {
                return menuItem[menuIndex];
            }
            else
            {
                return "";
            }

            return "";
        }

        public static void Help()
        {
            Console.WriteLine("");
            Console.WriteLine(" You can navigate the menu by using up and down arrows");
            Console.WriteLine(" To select an option press the enter key or the right arrow key!");
            Console.WriteLine("");
            Console.WriteLine(" Press any key to continue");
            Console.ReadKey();
            Console.Clear();
        }
        public static void Deposit(int userID)
        {
            List<BankAccountModel> checkaccounts = SQLconnection.LoadBankAccounts(userID);

            for(int i = 0; i < checkaccounts.Count; i++)
            {
                Console.WriteLine($"{checkaccounts[i].id}: {checkaccounts[i].name}");
            }

            Console.WriteLine("\nWhich account do you wish to withdraw money from?");
            Console.Write("===> ");
            string accountChoice = Console.ReadLine();
            int.TryParse(accountChoice, out int accountID);

            Console.WriteLine("\nHow much do you want to withdraw to your account?");
            Console.Write("===> ");
            string? transfer = Console.ReadLine();
            decimal amount;
            decimal.TryParse(transfer, out amount);

            Console.WriteLine(checkaccounts.Count);

            if (amount <= 0)
            {
                Console.WriteLine("Amount can be not negative"); //message for negative amount
            }
            /*
            else if (checkaccounts[userID].balance < amount)
            {
                Console.WriteLine("\n\tERROR! Not allowed. You don't have enough money");
            }
            */
            else
            {
                checkaccounts[userID].balance -= amount;
                SQLconnection.UpdateAccountBalance(userID, accountID, amount);
            }
        }

        public static void OpenAccount(int userID)
        {
            Console.WriteLine("\nWhat type of account do you want to open?");
            Console.WriteLine("\nChecking");
            Console.WriteLine("Salary");
            Console.WriteLine("Savings");
            Console.Write("===> ");
            string accountType = Console.ReadLine();

            List<BankAccountModel> checkAccounts = SQLconnection.LoadBankAccounts(userID);

            if (!string.IsNullOrEmpty(accountType)) //Checks to see if the account type isn't null or empty before proceeding
            {
                bool hasAccountType = false;

                if (accountType == "Checking")
                {
                    foreach (BankAccountModel bankAccountModel in checkAccounts)
                    {
                        if (bankAccountModel.name == "Checking")
                        {
                            hasAccountType = true;
                            break;
                        }
                    }
                }
                else if (accountType == "Salary")
                {
                    foreach (BankAccountModel bankAccountModel in checkAccounts)
                    {
                        if (bankAccountModel.name == "Salary")
                        {
                            hasAccountType = true;
                            break;
                        }
                    }
                }
                else if (accountType == "Savings")
                {
                    foreach (BankAccountModel bankAccountModel in checkAccounts)
                    {
                        if (bankAccountModel.name == "Savings")
                        {
                            hasAccountType = true;
                            break;
                        }
                    }
                }

                if (hasAccountType)
                {
                    Console.WriteLine("\nERROR: You have already opened an account of this type"); //The account already exists in the database
                }
                else
                {
                    BankAccountModel bankAccountModel = new BankAccountModel //Details of the new account
                    { 
                        name = accountType,
                        interest_rate = 0,
                        user_id = userID,
                        currency_id = 1,
                        balance = 0

                    };
                    SQLconnection.OpenAccount(bankAccountModel);
                    Console.WriteLine("\nAccount successfully opened");
                }
            }
            else
            {
                Console.WriteLine("\nERROR: This account type is null or empty"); //The account type is either null or empty
            }
        }

        public static void Login()
        {
            string? firstName, lastName, pinCode;

            Console.WriteLine("");
            Console.Write($" Please enter your first name: ");
            firstName = Console.ReadLine();

            Console.Write($" Please enter your last name: ");
            lastName = Console.ReadLine();

            Console.Write($" Please enter your Pin code: ");
            pinCode = Console.ReadLine();

            if (firstName == null || lastName == null || pinCode == null)
            {
                Console.WriteLine("User does not exist.\nPlease try again.");
            }
            else
            {
                List<UserModel> checkUsers = SQLconnection.CheckLogin(firstName, lastName, pinCode);

                if (checkUsers.Count < 1)
                {
                    Console.WriteLine("Failed loggin attempt.");
                    Console.WriteLine(checkUsers.Count);
                    Console.ReadLine();
                }
                else
                {
                    checkUsers[0].Info();
                    BankMenu(checkUsers);
                }
            }
        }
        #endregion
    }
}