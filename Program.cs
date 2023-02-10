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

            List<string> menuItems = new List<string>()
            {
                "Balance",
                "Transfer",
                "Withdraw",
                "Loan",
                "Account",
                "Create New User",
                "Logout"
            };

            int userTypeId = currentUser[0].role_id;

            while (runMenu)
            {
                int selectedMenuItems = DrawMenu(menuItems, menuMsg);
                switch (selectedMenuItems)
                {
                    case 0:
                        // Display account information method begins here
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
                                //check if account currency id == currencies_id --> toDisplay currency_name
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
                        // Transfer method begins here
                        Transfer(currentUser[0].id);

                        break;

                    case 2:
                        // Withdraw method begins here
                        Withdraw(currentUser[0].id);
                        break;

                    case 3:
                        // Loan method begins here
                        Console.WriteLine(" Loan would start here");
                        Console.WriteLine(" Press any key to continue");
                        Console.ReadKey();
                        break;

                    case 4:
                        // Open Account method begins here
                        OpenAccount(currentUser[0].id);
                        break;

                    case 5:
                        // Create user method begins here
                        if (userTypeId == (int)UserModel.UserRoles.client)
                        {
                            Console.Clear();
                            Console.WriteLine("\n You do not have access to this function.");
                        }
                        else if (userTypeId == (int)UserModel.UserRoles.clientAdmin || userTypeId == (int)UserModel.UserRoles.admin)
                        {
                            Console.Clear();
                            CreateNewUser();
                        }
                        Console.WriteLine("\n Press any key to continue");
                        Console.ReadKey();
                        break;

                    // Logout method begins here
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
            menuIndex = 0;

            //Declatation
            List<BankAccountModel> checkaccounts = SQLconnection.LoadBankAccounts(userID);
            List<string> menuItems = new List<string>();
            bool runMenu = true;
            bool runMenu2 = true;
            string? senderAccountName, reciverAccountName, input;
            int senderAccountId, reciverAccountId, senderAccountPos, reciverAccountPos, senderCurrencyId, receiverCurrencyId;
            decimal senderBalance, reciverBalance;
            string menuMsg = " Please select an account to transfer from\n ";
            string menuMsg2 = "\n Please select a reciver account";

            //Create menu Items
            for (int i = 0; i < checkaccounts.Count; i++)
            {
                menuItems.Add(checkaccounts[i].name);
            }
            menuItems.Add("Exit");

            //Menu Start
            while (runMenu)
            {
                int selectedMenuItems = DrawMenu(menuItems, menuMsg);

                //Exit case
                if (selectedMenuItems == menuItems.Count - 1)
                {
                    runMenu = false;
                }
                //select account case
                else if (selectedMenuItems <= menuItems.Count - 1)
                {
                    Console.Clear();
                    senderAccountId = checkaccounts[selectedMenuItems].id;
                    senderAccountName = checkaccounts[selectedMenuItems].name;
                    senderBalance = checkaccounts[selectedMenuItems].balance;
                    senderCurrencyId = checkaccounts[selectedMenuItems].currency_id; //currency id from_account
                    senderAccountPos = selectedMenuItems;
                    Console.WriteLine($"\n {checkaccounts[selectedMenuItems].name} account was selected");
                    menuMsg2 = menuMsg2 + $"\n Sender account: {checkaccounts[selectedMenuItems].name}";
                    Console.ReadKey();
                    runMenu2 = true;
                    menuIndex = 0;

                    //Submenu Start
                    while (runMenu2)
                    {
                        selectedMenuItems = DrawMenu(menuItems, menuMsg2);
                        reciverAccountName = "";
                        //Exit case
                        if (selectedMenuItems == menuItems.Count - 1)
                        {
                            runMenu2 = false;
                        }
                        else if (selectedMenuItems <= menuItems.Count - 1)
                        {

                            Console.Clear();
                            reciverAccountId = checkaccounts[selectedMenuItems].id;
                            reciverAccountName = checkaccounts[selectedMenuItems].name;
                            reciverAccountPos = selectedMenuItems;

                            //Same account was selected
                            if (senderAccountName == reciverAccountName)
                            {
                                Console.Clear();
                                Console.WriteLine($"\n Can not select the same account");
                                Console.WriteLine($" Press any key to continue");
                                Console.ReadKey();
                            }
                            //Select reciver account
                            else
                            {
                                reciverBalance = checkaccounts[selectedMenuItems].balance;
                                receiverCurrencyId = checkaccounts[selectedMenuItems].currency_id; //currency id to_account

                                Console.WriteLine($"\n {checkaccounts[selectedMenuItems].name} account was selected");
                                Console.ReadKey();
                                Console.Clear();
                                Console.WriteLine($"\n Sender account: {senderAccountName}: {senderBalance}");
                                Console.WriteLine($" Reciver account: {reciverAccountName}: {reciverBalance}");
                                Console.Write($"\n Enter amount you wish to transfer: ");
                                input = Console.ReadLine();


                                decimal.TryParse(input, out decimal transferAmount);
                                //Transfer amount is negative
                                if (transferAmount <= 0)
                                {
                                    Console.Clear();
                                    Console.WriteLine($"\n Amount can not be negative");
                                    Console.WriteLine($" Press any key to continue");
                                    Console.ReadKey();
                                    runMenu = false;
                                    runMenu2 = false;
                                }
                                //Transfer amount larger than balance
                                else if (transferAmount > senderBalance)
                                {
                                    Console.Clear();
                                    Console.WriteLine($"\n Transfer amount exceeds account balance");
                                    Console.WriteLine($" Press any key to continue");
                                    Console.ReadKey();
                                    runMenu = false;
                                    runMenu2 = false;
                                }
                                //Transfer Start
                                else
                                {

                                    //check currencies
                                    if (checkaccounts[senderAccountPos].currency_id != checkaccounts[reciverAccountPos].currency_id)
                                    {
                                        //transaction between different currencies
                                        transferAmount = CurrencyExchange(transferAmount, senderAccountPos, reciverAccountPos, checkaccounts);
                                        SQLconnection.TransferMoney(userID, senderAccountId, reciverAccountId, reciverBalance, transferAmount, senderCurrencyId, receiverCurrencyId);
                                        Console.WriteLine($"\n {Math.Truncate(transferAmount * 100) / 100} was transfered to {reciverAccountName}");
                                        Console.WriteLine($"\n Press any key to continue");
                                        Console.ReadKey();

                                        runMenu = false;
                                        runMenu2 = false;
                                    }
                                    //transaction between same currency
                                    else
                                    {
                                        //execute transaction
                                        SQLconnection.TransferMoney(userID, senderAccountId, reciverAccountId, reciverBalance, transferAmount, senderCurrencyId, receiverCurrencyId);
                                        Console.WriteLine($"\n {Math.Truncate(transferAmount * 100) / 100} was transfered to {reciverAccountName}");
                                        Console.WriteLine($"\n Press any key to continue");
                                        Console.ReadKey();
                                        runMenu = false;
                                        runMenu2 = false;
                                    }

                                }

                            }
                        }

                    }

                }

            }
            menuIndex = 0;
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

                    Console.Write($" Enter amount you wish to withdraw: ");

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
                        decimal newBalance = checkAccounts[selectedMenuItems].balance -= amount;
                        int currencyId = checkAccounts[selectedMenuItems].currency_id;
                        Console.WriteLine($"\n Account: {checkAccounts[selectedMenuItems].name} New balance: {newBalance}");
                        SQLconnection.UpdateAccountBalanceWithdraw(amount, checkAccounts[selectedMenuItems].id, userID, currencyId);
                        Console.WriteLine(" Press any key to continue");
                        Console.ReadKey();
                    }

                }
                else { }

            }
            menuIndex = 0;
        }

        public static void OpenAccount(int userID)
        {
            menuIndex = 0;
            bool runMenu = true;
            bool runMenu2 = true;
            bool hasAccountType = false;
            string newAccountName = "";
            string menuMsg = $" Please select an option ";
            List<BankAccountModel> checkAccounts = SQLconnection.LoadBankAccounts(userID);
            List<string> menuItems = new List<string>() { "Open Account", "Exit" };

            while (runMenu)
            {
                int selectedMenuItems = DrawMenu(menuItems, menuMsg);

                if (selectedMenuItems == 0)
                {
                    menuIndex = 0;
                    List<string> menuItems2 = new List<string>() { "Checking", "Salary", "Savings", "Exit" };
                    Console.WriteLine($"{menuItems[selectedMenuItems]}");

                    runMenu2 = true;
                    newAccountName = "";
                    hasAccountType = false;

                    while (runMenu2)
                    {
                        int selectedMenuItems2 = DrawMenu(menuItems2, menuMsg);

                        if (selectedMenuItems2 == 0)
                        {
                            foreach (BankAccountModel bankAccountModel in checkAccounts)
                            {
                                if (bankAccountModel.name == "Checking")
                                {
                                    // Check if checking account exist
                                    hasAccountType = true;
                                    menuIndex = 0;
                                    break;
                                }
                                else
                                {
                                    newAccountName = "Checking";
                                }
                            }
                        }
                        else if (selectedMenuItems2 == 1)
                        {
                            foreach (BankAccountModel bankAccountModel in checkAccounts)
                            {
                                if (bankAccountModel.name == "Salary")
                                {
                                    // Check if salary account exist
                                    hasAccountType = true;
                                    menuIndex = 0;
                                    break;
                                }
                                else
                                {
                                    newAccountName = "Salary";
                                }
                            }
                        }
                        else if (selectedMenuItems2 == 2)
                        {
                            foreach (BankAccountModel bankAccountModel in checkAccounts)
                            {
                                if (bankAccountModel.name == "Savings")
                                {
                                    // Check if savings account exist
                                    hasAccountType = true;
                                    menuIndex = 0;
                                    break;
                                }
                                else
                                {
                                    newAccountName = "Savings";
                                }
                            }
                        }
                        else if (selectedMenuItems2 == 3)
                        {
                            // This exits the menu
                            menuIndex = 0;
                            runMenu2 = false;
                        }
                        else { }

                        if (hasAccountType)
                        {
                            Console.WriteLine("\n ERROR: You have already opened an account of this type"); //The account already exists in the database
                            Console.WriteLine("\n Press Enter to continue.");
                            Console.ReadKey();
                            runMenu2 = false;
                        }
                        else
                        {
                            if (newAccountName == "" || newAccountName == null)
                            {

                            }
                            else
                            {
                                BankAccountModel bankAccountModel = new BankAccountModel //Details of the new account
                                {
                                    name = newAccountName,
                                    interest_rate = 0.75M,
                                    user_id = userID,
                                    currency_id = 1,
                                    balance = 0

                                };
                                SQLconnection.OpenAccount(bankAccountModel);
                                Console.WriteLine("\n Account successfully opened");
                                Console.WriteLine("\n Press Enter to continue.");
                                Console.ReadKey();
                                runMenu2 = false;
                            }
                        }
                    }
                }
                else if (selectedMenuItems == 1)
                {
                    // This exits this menu
                    runMenu = false;
                }
                else { }
            }
            menuIndex = 0;
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

        public static string DisplayTransactions(int userId)
        {
            Console.Clear();
            List<TransactionsModel> transactions = SQLconnection.LoadTransactions(userId);
            List<CurrencyConverter> currencyDB = SQLconnection.LoadBankCurrency();

            //map currencies Ids where the key is the currencyId and the value is the currency name
            Dictionary<int, string> currencyMap = currencyDB.ToDictionary(x => x.id, x => x.name);

            string toDisplay = "";

            for (int i = 0; i < transactions.Count; i++)
            {
                int currencyId = transactions[i].currency_id_sender;
                string currencyName = currencyMap.ContainsKey(currencyId) ? currencyMap[currencyId] : "Unknown Currency";
                toDisplay += $"\n {i + 1}: {transactions[i].name}, {transactions[i].amount_sender} {currencyName}, {transactions[i].timestamp}\n";
            }

            if (toDisplay == "")
            {
                toDisplay = "No transactions to display";
            }

            return toDisplay;
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