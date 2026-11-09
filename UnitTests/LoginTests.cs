using Dapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.Data.Sqlite;

namespace UnitTests;



[TestClass]
public sealed class Test1
{


    [DataTestMethod]
    [DataRow("kevin@kevin.nl", "kevin")]
    public void LoginValidCredentials(string m, string p)
    {
        string previousDirectory = Directory.GetCurrentDirectory();
        string testDirectory = Directory.CreateTempSubdirectory().FullName;
        try
        {
            Directory.CreateDirectory(Path.Combine(testDirectory, "DataSources"));
            SqliteConnection.ClearAllPools();
            Directory.SetCurrentDirectory(testDirectory);
            using SqliteConnection connection = new("Data Source=DataSources/database.db");
            connection.Execute(@"CREATE TABLE Account (id INTEGER PRIMARY KEY, first_name TEXT,
                last_name TEXT, email TEXT, phone_number TEXT, role INTEGER, is_blocked INTEGER, password TEXT)");

            AccountModel account = new(0, "Kevin", "Test", m, "0612345678", UserRole.Customer, false, "");
            PasswordHasher<AccountModel> hasher = new();
            account.Password = hasher.HashPassword(account, p);
            new AccountsAccess().Write(account);

            AccountsLogic logic = new();
            AccountModel result = logic.CheckLogin(m, p);
            Assert.IsNotNull(result);
            Assert.AreEqual(m, result.Email);
            Assert.IsNull(logic.CheckLogin(m, "wrong"));
        }
        finally
        {
            Directory.SetCurrentDirectory(previousDirectory);
            SqliteConnection.ClearAllPools();
            Directory.Delete(testDirectory, true);
        }
    }

    [DataTestMethod]
    [DataRow("kevin@kevin.nl", "wrong")]
    [DataRow("wrong1", "kevin")]
    [DataRow("wrong2", "wrong")]
    [DataRow("", "")]
    [DataRow(null, null)]
    public void LoginInvalidCredentials(string m, string p)
    {
        AccountsLogic l = new();

        AccountModel result = l.CheckLogin(m, p);

        Assert.IsNull(result);
    }
}
