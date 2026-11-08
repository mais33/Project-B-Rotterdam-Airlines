using Microsoft.Data.Sqlite;
using Dapper;

public class PlaneAccess
{
    private readonly SqliteConnection _connection =
        new SqliteConnection("Data Source=DataSources/database.db");

    public List<PlaneModel> GetAll()
    {
        string sql = "SELECT id AS Id, name AS Name, model AS Model, capacity AS Capacity FROM Plane";
        return _connection.Query<PlaneModel>(sql).ToList();
    }

    public PlaneModel? GetById(Int64 id)
    {
        string sql = "SELECT id AS Id, name AS Name, model AS Model, capacity AS Capacity FROM Plane WHERE id = @Id";
        return _connection.QueryFirstOrDefault<PlaneModel>(sql, new { Id = id });
    }
}
