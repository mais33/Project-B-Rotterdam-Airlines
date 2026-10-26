using Microsoft.Data.Sqlite;

using Dapper;


public class FlightAccess
{
    private SqliteConnection _connection = new SqliteConnection($"Data Source=DataSources/database.db");

    public List<FlightModel> GetAll()
    {
        string sql = @"SELECT Flight.id AS Id, Flight.price AS Price, Flight.departure_datetime AS DepartureDatetime,
                Start.city AS StartCity, Destination.city AS DestinationCity
            FROM Flight
            JOIN Airport AS Start ON Start.id = Flight.start_airport_id
            JOIN Airport AS Destination ON Destination.id = Flight.destination_id";
        return _connection.Query<FlightModel>(sql).ToList();
    }
}
