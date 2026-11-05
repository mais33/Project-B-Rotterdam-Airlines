using Microsoft.Data.Sqlite;

using Dapper;


public class AirportAccess
{
    private SqliteConnection _connection = new SqliteConnection($"Data Source=DataSources/database.db");

    public AirportModel GetById(Int64 id)
    {
        string sql = "SELECT id AS Id, city AS City, name AS Name FROM Airport WHERE id = @Id";
        return _connection.QueryFirstOrDefault<AirportModel>(sql, new { Id = id });
    }
}
