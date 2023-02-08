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
                int selectedMenuItem = DrawMenu(menuItems, menuMsg);
                //Switch for the selectedMenuItem string that DrawMenu returns
                switch (selectedMenuItem)
                {
                    case 0:
                        Console.Clear();
                        Login();
                        //Console.WriteLine(" Login would start here");
                        //Console.WriteLine(" Enter any key to continue");
                        //Console.ReadKey();
                        break;
                    case 1:
                        Help();
                        break;
                    case 2:
                        Environment.Exit(0);
                        break;
                }
            }

        }
        static void BankMenu(List<UserModel> currentUser)
        {
            bool runMenu = true;
            string menuMsg = " Welcome to Owl Banking\n Please select an option";

            List<string> menuItems = new List<string>();
            int userTypeId = currentUser[0].role_id;

            //if admin or clientAdmin
            if (userTypeId == (int)UserModel.UserRoles.admin || userTypeId == (int)UserModel.UserRoles.clientAdmin)
            {
                menuItems.AddRange(new List<string>()
                {
                    "Create New User",
                    "AdminOption1",
                    "AdminOption2",
                });
            }

            //if client or clientAdmin
            if (userTypeId == (int)UserModel.UserRoles.client || userTypeId == (int)UserModel.UserRoles.clientAdmin)
            {
                menuItems.AddRange(new List<string>()
                {
                    "Balance",
                    "Transfer",
                    "Withdraw",
                    "Loan",
                    "Account",
                    "Create New User",
                    "Logout"
                });
            }

            while (runMenu)
            {
                int selectedMenuItems = DrawMenu(menuItems, menuMsg);
                switch (selectedMenuItems)
                {
                    case 0:
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
                    case 1:
                        Transfer(currentUser[0].id);
                        Console.WriteLine(" Press any key to continue");
                        Console.ReadKey();
                        break;
                    case 2:
                        Withdraw(currentUser[0].id);
                        break;
                    case 3:
                        Console.WriteLine(" Loan would start here");
                        Console.WriteLine(" Press any key to continue");
                        Console.ReadKey();
                        break;

                    case 4:
                        bool repeat = true;
                        do
                        {
                            Console.Clear();
                            Console.WriteLine("\n ========== Account ==========");
                            Console.WriteLine("\n Please select an option:");
                            Console.WriteLine("\n 1. Create account");
                            Console.WriteLine(" 2. Return of interest");
                            Console.WriteLine(" 3. Exit");
                            Console.Write("\n ===>");
                            string? userOption = Console.ReadLine();

                            if (userOption == "1")
                            {
                                Console.Clear();
                                OpenAccount(currentUser[0].id);
                                Console.WriteLine(" Press any key to continue");
                                Console.ReadKey();
                            }
                            else if (userOption == "2")
                            {
                                Console.Clear();
                                ReturnOnInterest(currentUser[0].id, 1000);
                                Console.WriteLine(" Press any key to continue");
                                Console.ReadKey();
                            }
                            else if (userOption == "3")
                            {
                                repeat = false;
                                break;
                            }
                            else
                            {
                                Console.WriteLine(" ERROR: Please input a number shown on the menu.");
                                Console.WriteLine(" Press any key to continue");
                                Console.ReadKey();
                            }
                        } while (repeat);
                        break;

                    case 5:
                        CreateNewUser();
                        Console.WriteLine(" Press any key to continue");
                        Console.ReadKey();
                        break;

                    case 6:
                        menuIndex = 0;
                        runMenu = false;
                        break;
                }
            }
        }

        public static int DrawMenu(List<string> menuItem, string menuMsg)
        {

            //int menuIndex = 0;

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
                return menuIndex;
            }
            //Enter key check
            else if (ckey.Key == ConsoleKey.Enter)
            {
                return menuIndex;
            }
            else
            {
                return 100;
            }

            return 100;
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
        #endregion

        public static decimal CurrencyExchange(decimal amountFrom, int fromAccountID, int toAccountID, List<BankAccountModel> checkaccounts)
        {
            CurrencyConverter currencyConverter = new CurrencyConverter();
            List<CurrencyConverter> currencyDB = SQLconnection.LoadBankCurrency();
            double convertedAmountAsDouble, convertedAmountToSek;
            double amountToDouble = Decimal.ToDouble(amountFrom);
            decimal amountTo = 0;

            //loop between currencies
            for (int i = 0; i < currencyDB.Count; i++)
            {
                //Console.WriteLine($"{currencyDB[i].id} {currencyDB[i].name} || Rate: {currencyDB[i].exchange_rate}");

                //if currency to withdraw is SEK
                if (checkaccounts[fromAccountID].currency_id == 1)
                {
                    convertedAmountAsDouble = currencyConverter.CurrencyConverterCalculatorSEKToSomeCurrency(amountToDouble, currencyDB[i].exchange_rate);
                    amountTo = Convert.ToDecimal(convertedAmountAsDouble);
                }

                else
                {
                    if (checkaccounts[fromAccountID].currency_id == 2 && checkaccounts[toAccountID].currency_id == 1) //if is dollar to sek
                    {
                        convertedAmountToSek = currencyConverter.CurrencyConverterCalculatorSomeCurrencyToSEK(amountToDouble, currencyDB[i].exchange_rate);
                        amountTo = Convert.ToDecimal(convertedAmountToSek);
                    }

                    else //if to withdraw is another currency other than sek and dollar
                    {
                        convertedAmountToSek = currencyConverter.CurrencyConverterCalculatorSomeCurrencyToSEK(amountToDouble, currencyDB[i].exchange_rate);
                        convertedAmountAsDouble = currencyConverter.CurrencyConverterCalculatorSEKToSomeCurrency(convertedAmountToSek, currencyDB[i].exchange_rate);
                        amountTo = Convert.ToDecimal(convertedAmountAsDouble);
                    }
                }
            }
            return amountTo;
        }

        public static void Transfer(int userID)
        {
            decimal amount, amountTo, balanceAccount;
            int fromAccountID, toAccountID;
            List<BankAccountModel> checkaccounts = SQLconnection.LoadBankAccounts(userID);
            int currencyIdSender, currencyIdReceiver;

            Console.WriteLine("\nWhich account do you wish to transfer money FROM?");
            fromAccountID = DisplayAndSelectAccount(checkaccounts); //menu option with available accounts to transfer from
            bool userChoiceAccountIsValid = fromAccountID != -1; //check input for account's type

            Console.WriteLine("\nWhich account do you wish to transfer money TO?");
            toAccountID = DisplayAndSelectAccount(checkaccounts); //menu option with available accounts to transfer to
            bool userChoiceTargetAccountIsValid = toAccountID != -1; //check input for account's type

            balanceAccount = checkaccounts[fromAccountID].balance;
            currencyIdSender = checkaccounts[fromAccountID].currency_id; //currency id from_account
            currencyIdReceiver = checkaccounts[toAccountID].currency_id; //currency id to_account

            if (userChoiceAccountIsValid && userChoiceTargetAccountIsValid)
            {
                Console.Clear();
                amount = GetTransferAmount();

                //check balance to withdraw amount "from" account
                if (amount > balanceAccount)
                {
                    Console.WriteLine("ERROR! Insuficient amount");
                }
                //if there is enough money, get data "to" deposit
                else
                {
                    //check currencies
                    if (currencyIdSender != currencyIdReceiver)
                    {
                        //transaction between different currencies
                        amountTo = CurrencyExchange(amount, fromAccountID, toAccountID, checkaccounts);
                    }
                    else
                    {
                        //transaction between same currency
                        amountTo = amount;
                    }

                    try
                    {
                        //execute transaction
                        SQLconnection.TransferMoney(userID, checkaccounts[fromAccountID].id, checkaccounts[toAccountID].id, amount, amountTo, currencyIdSender, currencyIdReceiver);
                        Console.ForegroundColor = ConsoleColor.Cyan;
                        Console.WriteLine("\nMoney transfered!".ToUpper());
                        Console.ResetColor();
                    }
                    catch (Npgsql.PostgresException e)
                    {
                        Console.WriteLine("Something strange happened. Unauthorized transaction" +
                            "Please try again later");
                        Console.WriteLine(e.ErrorCode);
                    }
                }
            }
            else
            {
                InvalidInput("");
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
            //To Display and select available accounts
            for (int i = 0; i < checkaccounts.Count; i++)
            {
                Console.WriteLine($"\n{i + 1}: {checkaccounts[i].name}");
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
            string? input;
            decimal balance;
            int accountId;
            menuIndex = 0;
            bool runMenu = true;
            decimal amount;
            string menuMsg = $"\n Please select an option ";
            List<BankAccountModel> checkAccounts = SQLconnection.LoadBankAccounts(userID);
            List<string> menuItems = new List<string>();

            for (int i = 0; i < checkAccounts.Count; i++)
            {
                menuItems.Add(checkAccounts[i].name);
            }
            menuItems.Add("Exit");


            while (runMenu)
            {
                int selectedMenuItems = DrawMenu(menuItems, menuMsg);

                //Exit case
                if (selectedMenuItems == menuItems.Count - 1)
                {
                    runMenu = false;
                }
                else if (selectedMenuItems <= menuItems.Count - 1)
                {
                    balance = checkAccounts[selectedMenuItems].balance;
                    accountId = checkAccounts[selectedMenuItems].id;
                    Console.Clear();
                    Console.WriteLine($"\n\n {checkAccounts[selectedMenuItems].name} was selected\n Balance: {balance} \n");
                    //Console.WriteLine($" account id: {accountId}");

                    Console.WriteLine($" Enter amount you wish to withdraw: ");
                    input = Console.ReadLine();
                    decimal.TryParse(input, out amount);
                    if (amount < 0)
                    {
                        Console.WriteLine(" Amount to witdraw cannot be a negative value."); //message for negative amount
                    }
                    else if (checkAccounts[selectedMenuItems].balance < amount)
                    {
                        Console.WriteLine("\n ERROR! Not allowed. You don't have enough money");
                    }
                    else
                    {
                        amount = checkAccounts[selectedMenuItems].balance -= amount;
                        string transactionName = "Withdraw";
                        Console.WriteLine($"\n Account: {checkAccounts[selectedMenuItems].name} New balance: {amount}");
                        SQLconnection.UpdateAccountBalance(transactionName, amount, checkAccounts[selectedMenuItems].id, userID);
                        Console.WriteLine(" Press any key to continue");
                        Console.ReadKey();
                    }

                }
                else { }

            }
            menuIndex = 0;
            //decimal amount;
            //List<BankAccountModel> checkAccounts = SQLconnection.LoadBankAccounts(userID);

            //Console.Clear();
            //Console.WriteLine("\n Which account do you wish to withdraw money from?\n");

            //for (int i = 0; i < checkAccounts.Count; i++)
            //{
            //    Console.WriteLine($" {i + 1}: {checkAccounts[i].name} | Balance: {checkAccounts[i].balance}");
            //}

            //Console.Write("\n ===> ");
            //string? accountChoice = Console.ReadLine();

            //int.TryParse(accountChoice, out int accountID);
            //accountID -= 1;

            //Console.WriteLine("\n Amount to withdraw from your account?\n");
            //Console.Write(" ===> ");
            //string? transfer = Console.ReadLine();
            //decimal.TryParse(transfer, out amount);

            //if (amount <= 0)
            //{
            //    Console.WriteLine(" Amount to witdraw cannot be a negative value."); //message for negative amount
            //}
            //else if (checkAccounts[accountID].balance < amount)
            //{
            //    Console.WriteLine("\n ERROR! Not allowed. You don't have enough money");
            //}
            //else
            //{
            //    amount = checkAccounts[accountID].balance -= amount;
            //    Console.WriteLine($"\n Account: {checkAccounts[accountID].name} New balance: {amount}");
            //    SQLconnection.UpdateAccountBalance(amount, checkAccounts[accountID].id, userID);
            //}
        }

        public static void OpenAccount(int userID)
        {
            Console.WriteLine("\n What type of account do you want to open?");
            Console.WriteLine("\n Checking");
            Console.WriteLine(" Salary");
            Console.WriteLine(" Savings");
            Console.Write("===> ");
            string? accountType = Console.ReadLine();

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
                        interest_rate = 0.75M,
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

        public static void ReturnOnInterest(int userID, decimal input)
        {
            List<BankAccountModel> Accounts = SQLconnection.LoadBankAccounts(userID);
            List<CurrencyConverter> AccountCurrency = SQLconnection.LoadBankCurrency();
            bool hasSavingsAccount = false;

            foreach (BankAccountModel Account in Accounts)
            {
                decimal interestRate = Account.interest_rate;

                if (Account.name == "Savings" && Account.interest_rate > 0)
                {
                    Console.Clear();
                    Console.WriteLine("\n The information below shows the return of the interest rate for your deposit to your savings account.");
                    Console.WriteLine("\n Return from deposit:");
                    Console.WriteLine($" 1 month:  {Math.Truncate(input * (decimal)Math.Pow((1 + (double)interestRate / 12), 1) * 100) / 100} {AccountCurrency[userID - 1].name}");
                    Console.WriteLine($" 3 months: {Math.Truncate(input * (decimal)Math.Pow((1 + (double)interestRate / 12), 3) * 100) / 100} {AccountCurrency[userID - 1].name}");
                    Console.WriteLine($" 6 months: {Math.Truncate(input * (decimal)Math.Pow((1 + (double)interestRate / 12), 6) * 100) / 100} {AccountCurrency[userID - 1].name}");
                    Console.WriteLine($" 1 year:   {Math.Truncate(input * (decimal)Math.Pow((1 + (double)interestRate), 1) * 100) / 100} {AccountCurrency[userID - 1].name}");
                    Console.WriteLine($" 5 years:  {Math.Truncate(input * (decimal)Math.Pow((1 + (double)interestRate), 5) * 100) / 100} {AccountCurrency[userID - 1].name}\n");
                    hasSavingsAccount = true;
                    continue;
                }
            }
            if (hasSavingsAccount == false)
            {
                Console.Clear();
                Console.WriteLine("\n ERROR: No account with a set interest rate found.");
            }
        }

        public static void CreateNewUser()
        {
            UserModel newUser = new UserModel
            {
                first_name = UserModel.GetInputFirstName(),
                last_name = UserModel.GetInputLastName(),
                pin_code = UserModel.GetInputPinCode(),
                role_id = UserModel.GetInputRoleID(),
                branch_id = UserModel.GetInputBranchId(),
            };
            SQLconnection.SaveBankUser(newUser);
        }

        public static void InvalidInput(string? input)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"INVALID INPUT {input}! Try again.");
            Console.ResetColor();
        }

        public static void Login()
        {
            string? firstName, lastName, pinCode;

            int loginAttempts = 3; //The number of attempts that the user starts out with

            for (int i = 0; i < loginAttempts; i--) //For each time that the user fails to login an attempt will be used up
            {
                Console.WriteLine("\n Please enter your details");
                Console.Write($"\n First name: ");
                firstName = Console.ReadLine();

                Console.Write($" Last name: ");
                lastName = Console.ReadLine();

                Console.Write($" PIN code: ");
                pinCode = Console.ReadLine();

                if (firstName == null || lastName == null || pinCode == null)
                {
                    Console.WriteLine("\n ERROR: User does not exist");
                    loginAttempts--;
                }
                else
                {
                    List<UserModel> checkUsers = SQLconnection.CheckLogin(firstName, lastName, pinCode);

                    if (checkUsers.Count < 1)
                    {
                        Console.WriteLine("\n ERROR: Login failed");
                        //Console.WriteLine(checkUsers.Count);
                        //Console.ReadLine();
                        loginAttempts--;
                    }
                    else
                    {
                        Console.WriteLine("\n Login successful");
                        checkUsers[0].Info();
                        BankMenu(checkUsers);
                        break;
                    }
                }

                if (loginAttempts == 2)
                {
                    Console.WriteLine(" You have two attempts left.");
                    Console.WriteLine(" Press any key to continue");
                    Console.ReadKey();
                    Console.Clear();
                }
                else if (loginAttempts == 1)
                {
                    Console.WriteLine(" You have one attempt left.");
                    Console.WriteLine(" Press any key to continue");
                    Console.ReadKey();
                    Console.Clear();
                }
                else
                {
                    Console.WriteLine(" You have used up all of your login attempts. The application will now shut down");
                    Environment.Exit(0);
                    break;
                }
            }
        }
    }
}