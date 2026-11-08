using Microsoft.Data.Sqlite;
using Dapper;

public class UserLoginAccess
{
    private readonly SqliteConnection _connection =
        new SqliteConnection("Data Source=DataSources/database.db");

    public AccountModel? GetAccountByEmail(string email)
    {
        string sql = @"
            SELECT
                id AS Id,
                first_name AS FirstName,
                last_name AS LastName,
                email_adress AS Email,
                phone_number AS PhoneNumber,
                role AS Role,
                is_blocked AS IsBlocked,
                password AS Password
            FROM Account
            WHERE email_adress = @Email";

        return _connection.QueryFirstOrDefault<AccountModel>(
            sql,
            new { Email = email }
        );
    }
}