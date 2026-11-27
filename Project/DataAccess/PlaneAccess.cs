using Microsoft.Data.Sqlite;
using Dapper;

public class PlaneAccess
{

    private SqliteConnection _connection =
        new SqliteConnection("Data Source=DataSources/database.db");

    private const string Table = "Plane";
    public List<PlaneModel> GetAllPlanes()
    {
        string sql = $@"SELECT
                           id AS Id,
                           name AS Name,
                           Model As model
                       FROM {Table}";

        return _connection.Query<PlaneModel>(sql).ToList();
    }

    public void UpdatePlaneName(PlaneModel plane)
    {
        string sql = @"UPDATE Plane 
                   SET name = @Name 
                   WHERE id = @Id";

        _connection.Execute(sql, new { Name = plane.Name, Id = plane.Id });
    }

    public void UpdatePlaneModel(PlaneModel plane)
    {
        string sql = @"UPDATE Plane 
                   SET model = @Model 
                   WHERE id = @Id";

        _connection.Execute(sql, new { Model = plane.Model, Id = plane.Id });
    }

    public PlaneModel? GetById(long id)
    {
        string sql = @"SELECT 
                          id AS Id, 
                          name AS Name, 
                          model AS Model 
                       FROM Plane 
                       WHERE id = @Id";

        return _connection.QueryFirstOrDefault<PlaneModel>(sql, new { Id = id });
    }

}