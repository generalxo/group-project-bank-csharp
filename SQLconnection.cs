using Dapper;
using Npgsql;
using System.Configuration;
using System.Data;

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
        // We want to check >>>>>>>>LASTNAME<<<<<<<<<<<<<  together with >>>>>>>>FIRSTNAME<<<<<<<<<<<<< 
        public static List<UserModel> CheckLogin(string firstName, string lastName, string pinCode)
        {
            using (IDbConnection cnn = new NpgsqlConnection(LoadConnectionString()))
            {

                var output = cnn.Query<UserModel>($"SELECT * FROM bank_user WHERE first_name = '{firstName}' AND last_name = '{lastName}' AND pin_code = '{pinCode}'", new DynamicParameters());
                //Console.WriteLine(output);
                return output.ToList();
            }
            // Kopplar upp mot DB:n
            // läser ut alla Users
            // Returnerar en lista av Users
        }
        public static decimal UpdateBalanceForWithdraw(int userId, decimal amount, int id, decimal balance)
        {

            using (IDbConnection cnn = new NpgsqlConnection(LoadConnectionString()))
            {
                var newBalance = cnn.Execute($"UPDATE bank_account SET balance = '{balance - amount}' WHERE id = '{id}' AND user_id = '{userId}'");
                return newBalance;
            }
        }


        public static decimal UpdateBalanceForDeposit(int userId, decimal amount, int id, decimal balance)
        {

            using (IDbConnection cnn = new NpgsqlConnection(LoadConnectionString()))
            {
                var newBalance = cnn.Execute($"UPDATE bank_account SET balance = '{balance + amount}' WHERE id = '{id}' AND user_id = '{userId}'");
                return newBalance;
            }
        }

        public static List<BankAccountModel> LoadBankAccounts(int user_id)
        {
            using (IDbConnection cnn = new NpgsqlConnection(LoadConnectionString()))
            {
                var output = cnn.Query<BankAccountModel>($"SELECT * FROM bank_account WHERE user_id = '{user_id}'", new DynamicParameters());
                return output.ToList();
            }
        }

        public static void SaveBankUser(UserModel user)
        {
            using (IDbConnection cnn = new NpgsqlConnection(LoadConnectionString()))
            {
                cnn.Execute("insert into bank_users (first_name, last_name, pin_code) values (@first_name, @last_name, @pin_code)", user);

            }
        }

        private static string LoadConnectionString(string id = "Default")
        {
            return ConfigurationManager.ConnectionStrings[id].ConnectionString;
        }
    }
}