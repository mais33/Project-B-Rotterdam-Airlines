using Microsoft.Data.Sqlite;

using Dapper;


public class AccountsAccess
{
    private SqliteConnection _connection = new SqliteConnection($"Data Source=DataSources/database.db");

    private readonly string Table = "Account";

    public void Write(AccountModel account)
    {
        string sql = $@"INSERT INTO {Table} (first_name, last_name, email, phone_number, role, password)
            VALUES (@FirstName, @LastName, @Email, @PhoneNumber, @Role, @Password)";
        _connection.Execute(sql, account);
    }

    public AccountModel GetByEmail(string email)
    {
        string sql = $"SELECT * FROM {Table} WHERE email = @Email";
        return _connection.QueryFirstOrDefault<AccountModel>(sql, new { Email = email });
    }

    public void Update(AccountModel account)
    {
        string sql = $@"UPDATE {Table} SET first_name = @FirstName, last_name = @LastName,
            email = @Email, phone_number = @PhoneNumber, role = @Role, password = @Password WHERE id = @Id";
        _connection.Execute(sql, account);
    }

    public void Delete(AccountModel account)
    {
        string sql = $"DELETE FROM {Table} WHERE id = @Id";
        _connection.Execute(sql, new { Id = account.Id });
    }

    public void ClearAccounts()
    {
        string sql = @"DELETE FROM Account;
            DELETE FROM sqlite_sequence WHERE name = 'Account';";
        _connection.Execute(sql);
    }



}