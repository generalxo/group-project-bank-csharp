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
                        Transfer(currentUser[0].id);
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

                        Console.WriteLine(" Account would start here");
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

        public static void Transfer(int userID)
        {
            List<BankAccountModel> checkaccounts = SQLconnection.LoadBankAccounts(userID);

            Console.WriteLine("\nWhich account do you wish to transfer money from?");

            int accountID = DisplayAndSelectAccount(checkaccounts);

            Console.WriteLine($"Balance transfer from {checkaccounts[accountID].name}: {checkaccounts[accountID].balance}");
            decimal balanceTransferFrom = checkaccounts[accountID].balance;

            decimal amount = GetTransferAmount();

            if (amount > balanceTransferFrom)
            {
                Console.WriteLine("ERROR! Invalid amount");
            }
            else
            {
                decimal newWithdraw = SQLconnection.UpdateBalanceForWithdraw(userID, amount, checkaccounts[accountID].id, balanceTransferFrom);

                Console.WriteLine("\nWhich account do you wish to transfer money to?");

                int accountIdTransfer = DisplayAndSelectAccount(checkaccounts);

                Console.WriteLine($"Balance transfer to {checkaccounts[accountIdTransfer].name}: {checkaccounts[accountIdTransfer].balance}");
                decimal balanceTransferTo = checkaccounts[accountIdTransfer].balance;

                decimal newDeposit = SQLconnection.UpdateBalanceForDeposit(userID, amount, checkaccounts[accountIdTransfer].id, balanceTransferTo);

                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine("\nMoney transfered!".ToUpper());
                Console.ResetColor();
            }
        }

        public static decimal GetTransferAmount()
        {
            Console.WriteLine("\nHow much do you want to transfer?");
            Console.Write("===> ");
            string? transfer = Console.ReadLine();
            decimal amount;
            decimal.TryParse(transfer, out amount);
            return amount;

        }

        public static int DisplayAndSelectAccount(List<BankAccountModel> checkaccounts)
        {
            for (int i = 0; i < checkaccounts.Count; i++)
            {
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine($"\n{i + 1}: {checkaccounts[i].name}");
                Console.ResetColor();
            }

            Console.Write("\nType ===> ");
            string? accountChoice = Console.ReadLine();
            int.TryParse(accountChoice, out int accountID);
            return accountID -= 1;
        }
        public static void Deposit(int userID)
        {
            List<BankAccountModel> checkaccounts = SQLconnection.LoadBankAccounts(userID);

            for (int i = 0; i < checkaccounts.Count; i++)
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