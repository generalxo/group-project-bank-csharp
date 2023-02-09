using Dapper;
using Npgsql;
using System.Configuration;
using System.Data;
using System.Globalization;

namespace group_project_bank_csharp
{
    public class SQLconnection
    {
        public static List<UserModel> LoadBankUsers()
        {
            using (IDbConnection cnn = new NpgsqlConnection(LoadConnectionString()))
            {

                var output = cnn.Query<UserModel>($"SELECT * FROM bank_user", new DynamicParameters());
                //Console.WriteLine(output);
                return output.ToList();
            }
            // Kopplar upp mot DB:n
            // läser ut alla Users
            // Returnerar en lista av Users
        }

        //public static list<bankaccountmodel> getuseraccounts(int user_id)
        //{
        //    using (idbconnection cnn = new npgsqlconnection(loadconnectionstring()))
        //    {

        //        var output = cnn.query<bankaccountmodel>($"select bank_account.*, bank_currency.name as currency_name, bank_currency.exchange_rate as currency_exchange_rate from bank_account, bank_currency where user_id = '{user_id}' and bank_account.currency_id = bank_currency.id", new dynamicparameters());
        //        //console.writeline(output);
        //        return output.tolist();
        //    }
        //    // denna funktion ska leta upp användarens konton från databas och returnera dessa som en lista
        //    // vad behöver denna funktion för information för att veta vems konto den ska hämta
        //    // vad har den för information att tillgå?
        //    // vilken typ av sql-query bör vi använda, insert, update eller select?
        //    // ...?

        //}

        public static List<BankAccountModel> LoadBankAccounts(int user_id)
        {
            using (IDbConnection cnn = new NpgsqlConnection(LoadConnectionString()))
            {
                var output = cnn.Query<BankAccountModel>($"SELECT * FROM bank_account WHERE user_id = '{user_id}'", new DynamicParameters());
                return output.ToList();
            }
        }

        // We want to check >>>>>>>>LASTNAME<<<<<<<<<<<<<  together with >>>>>>>>FIRSTNAME<<<<<<<<<<<<< 
        public static List<UserModel> CheckLogin(string firstName, string lastName, string pinCode)
        {
            using (IDbConnection cnn = new NpgsqlConnection(LoadConnectionString()))
            {

                var output = cnn.Query<UserModel>($"SELECT * FROM bank_user WHERE first_name = '{firstName}' AND last_name = '{lastName}' AND pin_code = '{pinCode}'", new DynamicParameters());
                //Console.WriteLine(output);
                return output.ToList();
            }
        }

        public static bool UpdateAccountBalanceWithdraw(decimal amount, int id, int user_id, int currency_id)
        {
            using (IDbConnection cnn = new NpgsqlConnection(LoadConnectionString()))
            {
                cnn.Open();
                try
                {
                    var numberFormat = new NumberFormatInfo
                    {
                        NumberDecimalSeparator = ".",
                        NumberGroupSeparator = ""
                    };

                    using (var transaction = cnn.BeginTransaction())
                    {
                        cnn.Execute($@"
                        UPDATE bank_account 
                        SET balance = balance + '{amount.ToString(numberFormat)}'
                        WHERE id = '{id}' AND user_id = '{user_id}';
                        INSERT INTO bank_transaction (name, user_id, from_account_id, amount_sender, currency_id_sender)
                        VALUES ('Withdraw', '{user_id}', '{id}', '{amount.ToString(numberFormat)}', '{currency_id}');", new DynamicParameters());
                        transaction.Commit();
                        return true;
                    }
                }
                catch (Npgsql.PostgresException)
                {
                    //Console.WriteLine(e.Message);
                    return false;
                }
            }
        }

        public static bool UpdateAccountBalanceDeposit(decimal amount, int id, int user_id, int currency_id)
        {
            using (IDbConnection cnn = new NpgsqlConnection(LoadConnectionString()))
            {
                cnn.Open();
                try
                {
                    var numberFormat = new NumberFormatInfo
                    {
                        NumberDecimalSeparator = ".",
                        NumberGroupSeparator = ""
                    };

                    using (var transaction = cnn.BeginTransaction())
                    {
                        cnn.Execute($@"
                        UPDATE bank_account 
                        SET balance = balance - '{amount.ToString(numberFormat)}'
                        WHERE id = '{id}' AND user_id = '{user_id}';
                        INSERT INTO bank_transaction (name, user_id, from_account_id, amount_sender, currency_id_sender)
                        VALUES ('Deposit', '{user_id}', '{id}', '{amount.ToString(numberFormat)}', '{currency_id}');", new DynamicParameters());
                        transaction.Commit();
                        return true;
                    }
                }
                catch (Npgsql.PostgresException)
                {
                    //Console.WriteLine(e.Message);
                    return false;
                }
            }
        }


        public static bool TransferMoney(int user_id, int from_account_id, int to_account_id, decimal amountFrom, decimal amountTo, int currencySenderId, int currencyReceiverId)
        {
            using (IDbConnection cnn = new NpgsqlConnection(LoadConnectionString()))
            {
                cnn.Open();

                using (var transaction = cnn.BeginTransaction())
                {
                    try
                    {

                        var numberFormat = new NumberFormatInfo
                        {
                            NumberDecimalSeparator = ".",
                            NumberGroupSeparator = ""
                        };

                        cnn.Execute($@"
                        UPDATE bank_account 
                        SET balance = balance - '{amountFrom.ToString(numberFormat)}'
                            WHERE id = '{from_account_id}' AND user_id = '{user_id}' AND currency_id = '{currencySenderId}';
                        UPDATE bank_account 
                        SET balance = balance + '{amountTo.ToString(numberFormat)}'
                        WHERE id = '{to_account_id}' AND user_id = '{user_id}' AND currency_id = '{currencyReceiverId}';
                        INSERT INTO bank_transaction (name, user_id, from_account_id, to_account_id, amount_sender, amount_receiver, currency_id_sender, currency_id_receiver)
                        VALUES ('Transfer', '{user_id}', '{from_account_id}', '{to_account_id}', '{amountFrom.ToString(numberFormat)}', '{amountTo.ToString(numberFormat)}', '{currencySenderId}', '{currencyReceiverId}');", new DynamicParameters());
                        transaction.Commit();
                        return true;
                    }
                    catch (Npgsql.PostgresException)
                    {
                        //Console.WriteLine(e.Message);
                        return false;
                    }
                }
            }
        }

        public static List<TransactionsModel> LoadTransactions(int user_id)
        {
            using (IDbConnection cnn = new NpgsqlConnection(LoadConnectionString()))
            {
                var output = cnn.Query<TransactionsModel>($"SELECT * FROM bank_transaction WHERE user_id = '{user_id}'", new DynamicParameters());
                return output.ToList();
            }
        }

        public static void SaveBankUser(UserModel user)
        {
            using (IDbConnection cnn = new NpgsqlConnection(LoadConnectionString()))
            {
                cnn.Execute("INSERT INTO bank_user (first_name, last_name, pin_code, role_id, branch_id) VALUES (@first_name, @last_name, @pin_code, @role_id, @branch_id)", user);

            }
        }

        public static List<CurrencyConverter> LoadBankCurrency()
        {
            using (IDbConnection cnn = new NpgsqlConnection(LoadConnectionString()))
            {
                var output = cnn.Query<CurrencyConverter>($"SELECT * FROM bank_currency", new DynamicParameters());
                return output.ToList();
            }
        }

        public static void OpenAccount(BankAccountModel user)
        {
            using (IDbConnection cnn = new NpgsqlConnection(LoadConnectionString()))
            {
                cnn.Execute("INSERT INTO bank_account (name, interest_rate, user_id, currency_id, balance) VALUES (@name, @interest_rate, @user_id, @currency_id, @balance)", user);
            }
        }

        private static string LoadConnectionString(string id = "Default")
        {
            return ConfigurationManager.ConnectionStrings[id].ConnectionString;
        }

    }
}