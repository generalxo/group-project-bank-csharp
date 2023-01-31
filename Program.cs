namespace group_project_bank_csharp
{
    internal class Program
    {
        static void Main(string[] args)
        {
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
                        Console.Clear();
                        Console.WriteLine("");
                        Console.WriteLine(" Your Balance");
                        Console.WriteLine("");

                        //Load current user bank accounts & all currencies
                        List<BankAccountModel> bankAccounts = SQLconnection.LoadBankAccounts(currentUser[0].id);
                        List<CurrencyConverter> currencies = SQLconnection.LoadBankCurrency();

                        //Loop for each account
                        for (int i = 0; i < bankAccounts.Count; i++)
                        {
                            //Loop for each currency
                            for (int j = 0; j < currencies.Count; j++)
                            {
                                //check if account currency id == currencies_id --> display currency_name
                                if (bankAccounts[i].currency_id == currencies[j].id)
                                {
                                    Console.WriteLine($" {bankAccounts[i].name} {bankAccounts[i].balance}: {currencies[j].name}");
                                }
                            }
                        }

                        Console.WriteLine("");
                        Console.WriteLine(" Press any key to continue");
                        Console.ReadKey();

                        break;
                    case "Transfer":
                        Transfer(currentUser[0].id);
                        Console.WriteLine(" Press any key to continue");
                        Console.ReadKey();
                        break;
                    case "Withdraw":
                        Withdraw(currentUser[0].id);
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
            bool userChoiceAccountIsValid = accountID != -1; //boolean to check option input for account's type

            if (userChoiceAccountIsValid)
            {
                decimal balanceAccount = checkaccounts[accountID].balance;
                Console.WriteLine($"Balance transfer from {checkaccounts[accountID].name}: {balanceAccount}");

                decimal amount = GetTransferAmount();

                if (amount > balanceAccount)
                {
                    Console.WriteLine("ERROR! Invalid amount");
                }
                else
                {
                    Console.WriteLine("\nWhich account do you wish to transfer money to?");

                    int accountIdTarget = DisplayAndSelectAccount(checkaccounts);
                    bool userChoiceTargetAccountIsValid = accountIdTarget != -1;

                    if (userChoiceTargetAccountIsValid)
                    {
                        decimal balanceTransferTarget = checkaccounts[accountIdTarget].balance;
                        Console.WriteLine($"\nBalance transfer to {checkaccounts[accountIdTarget].name}: {balanceTransferTarget}");

                        if (checkaccounts[accountID].currency_id != checkaccounts[accountIdTarget].currency_id)
                        {
                            CurrencyConverter currencyConverter = new CurrencyConverter();
                            List<CurrencyConverter> currencyDB = SQLconnection.LoadBankCurrency();

                            //to withdraw
                            SQLconnection.UpdateBalanceForWithdraw(userID, amount, checkaccounts[accountID].id, balanceAccount);

                            double amountToDouble = Decimal.ToDouble(amount);

                            for (int i = 0; i < currencyDB.Count; i++)
                            {
                                Console.WriteLine($"{currencyDB[i].id} {currencyDB[i].name} || Rate: {currencyDB[i].exchange_rate}");

                                if (checkaccounts[accountID].currency_id == 1)
                                {
                                    double convertedAmountAsDouble = currencyConverter.CurrencyConverterCalculatorSEKToDollar(amountToDouble, currencyDB[i].exchange_rate);
                                    decimal convertedAmount = Convert.ToDecimal(convertedAmountAsDouble);

                                    //to deposit
                                    SQLconnection.UpdateBalanceForDeposit(userID, convertedAmount, checkaccounts[accountIdTarget].id, balanceTransferTarget);
                                }

                                if (checkaccounts[accountID].currency_id == 2)
                                {
                                    double convertedAmountAsDouble = currencyConverter.CurrencyConverterCalculatorDollarToSEK(amountToDouble, currencyDB[i].exchange_rate);
                                    decimal convertedAmount = Convert.ToDecimal(convertedAmountAsDouble);

                                    //to deposit
                                    SQLconnection.UpdateBalanceForDeposit(userID, convertedAmount, checkaccounts[accountIdTarget].id, balanceTransferTarget);
                                }
                            }
                        }
                        else
                        {
                            //to withdraw
                            SQLconnection.UpdateBalanceForWithdraw(userID, amount, checkaccounts[accountID].id, balanceAccount);

                            //to deposit
                            SQLconnection.UpdateBalanceForDeposit(userID, amount, checkaccounts[accountIdTarget].id, balanceTransferTarget);
                        }

                        Console.ForegroundColor = ConsoleColor.Cyan;
                        Console.WriteLine("\nMoney transfered!".ToUpper());
                        Console.ResetColor();

                    }
                    else
                    {
                        //wrong target
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("Invalid Account Target".ToUpper());
                        Console.ResetColor();
                    }
                }
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Invalid Account".ToUpper());
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
            int.TryParse(Console.ReadLine(), out int accountID);

            bool isNotValidIndex = accountID > checkaccounts.Count || accountID <= 0;

            if (isNotValidIndex)
            {
                return -1;
            }
            else
            {
                return accountID -= 1;
            }
        }
        public static void Withdraw(int userID)
        {
            decimal amount;
            List<BankAccountModel> checkAccounts = SQLconnection.LoadBankAccounts(userID);

            Console.Clear();
            Console.WriteLine("Which account do you wish to withdraw money from?\n");

            for (int i = 0; i < checkAccounts.Count; i++)
            {
                Console.WriteLine($"{i + 1}: {checkAccounts[i].name} | Balance: {checkAccounts[i].balance}");
            }

            Console.Write("\nType ===> ");
            string accountChoice = Console.ReadLine();

            int.TryParse(accountChoice, out int accountID);
            accountID -= 1;

            Console.WriteLine("\nAmount to withdraw from your account?\n");
            Console.Write("Type ===> ");
            string? transfer = Console.ReadLine();
            decimal.TryParse(transfer, out amount);

            if (amount <= 0)
            {
                Console.WriteLine("Amount to witdraw cannot be a negative value."); //message for negative amount
            }
            else if (checkAccounts[accountID].balance < amount)
            {
                Console.WriteLine("\n\tERROR! Not allowed. You don't have enough money");
            }
            else
            {
                amount = checkAccounts[accountID].balance -= amount;
                Console.WriteLine($"\nAccount: {checkAccounts[accountID].name} New balance: {amount}");
                SQLconnection.UpdateAccountBalance(amount, checkAccounts[accountID].id, userID);
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