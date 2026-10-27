using Microsoft.Data.Sqlite;

using Dapper;


public class AccountsAccess
{
    private readonly SqliteConnection _connection = new SqliteConnection($"Data Source=DataSources/database.db");

    private const string Table = "Account";

    public void Write(AccountModel account)
    {
        string sql = $@"INSERT INTO {Table} (first_name, last_name, email, phone_number, role, is_blocked, password)
            VALUES (@FirstName, @LastName, @Email, @PhoneNumber, @Role, @IsBlocked, @Password)";
        _connection.Execute(sql, account);
    }

    public AccountModel? GetByEmail(string email)
    {
        string sql = $"SELECT * FROM {Table} WHERE email = @Email";
        return _connection.QueryFirstOrDefault<AccountModel>(sql, new { Email = email });
    }

    public void Update(AccountModel account)
    {
        string sql = $@"UPDATE {Table} SET first_name = @FirstName, last_name = @LastName,
            email = @Email, phone_number = @PhoneNumber, role = @Role, is_blocked = @IsBlocked, password = @Password WHERE id = @Id";
        _connection.Execute(sql, account);
    }
    public void UpdatePhoneNumber(AccountModel account)
    {
        string sql = $"UPDATE {Table} SET phone_number = @PhoneNumber WHERE id = @Id";
        _connection.Execute(sql, new{PhoneNumber = account.PhoneNumber, Id = account.Id});
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

    public void ClearAccountsExceptAdmin()
    {
        string sql = $@"DELETE FROM {Table} WHERE role != @AdminRole;";
        _connection.Execute(sql, new{ AdminRole = (long)UserRole.Admin });
    }


    public void BlockAccount(AccountModel account)
    {   
        string sql = $"Update {Table} Set is_blocked = 1 WHERE id = @Id";
        _connection.Execute(sql, new { Id = account.Id });
    }

    public void UnblockAccount(AccountModel account)
    {
        string sql = $"Update {Table} Set is_blocked = 0 WHERE id = @Id";
        _connection.Execute(sql, new { Id = account.Id });
    }

    public AccountModel? GetAccountById(Int64 id)
    {
        string sql = $@"SELECT * FROM {Table} WHERE id = @Id";
        return _connection.QueryFirstOrDefault<AccountModel>(sql, new { Id = id });
    }

    public List<AccountModel> GetAllAccounts()
    {
        string sql = $"SELECT * FROM {Table}";
        List<AccountModel> accounts = _connection.Query<AccountModel>(sql).ToList();

        return accounts;
    }


}