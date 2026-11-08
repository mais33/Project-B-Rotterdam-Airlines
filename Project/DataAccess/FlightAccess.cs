using Microsoft.Data.Sqlite;
using Dapper;

public class FlightAccess
{
    private SqliteConnection _connection = new SqliteConnection($"Data Source=DataSources/database.db");

    public List<FlightModel> GetAll()
    {
        string sql = @"SELECT
            id AS Id,
            plane_id AS PlaneId,
            destination_id AS DestinationId,
            start_airport_id AS StartAirportId,
            price AS Price,
            arrival_datetime AS ArrivalDatetime,
            departure_datetime AS DepartureDatetime
            FROM Flight";
        return _connection.Query<FlightModel>(sql).ToList();
    }

    public FlightModel? GetById(Int64 id)
    {
        string sql = @"SELECT
            id AS Id,
            plane_id AS PlaneId,
            destination_id AS DestinationId,
            start_airport_id AS StartAirportId,
            price AS Price,
            arrival_datetime AS ArrivalDatetime,
            departure_datetime AS DepartureDatetime
            FROM Flight WHERE id = @Id";
        return _connection.QueryFirstOrDefault<FlightModel>(sql, new { Id = id });
    }
}
