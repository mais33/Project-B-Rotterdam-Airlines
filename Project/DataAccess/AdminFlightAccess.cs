using Microsoft.Data.Sqlite;
using Dapper;

public class AdminFlightAccess
{
    private readonly string _connectionString =
        "Data Source=DataSources/database.db";

    // Get all airports from the database.
    public List<AirportModel> GetAirports()
    {
        // Automatically close and dispose of the database connection after use. 
        using SqliteConnection connection = 
            new SqliteConnection(_connectionString);

        string sql = @"
            SELECT
                id AS Id,
                city AS City,
                name AS Name
            FROM Airport";

        return connection.Query<AirportModel>(sql).ToList();
    }

    // Get all planes from the database.
    public List<PlaneModel> GetPlanes()
    {
        using SqliteConnection connection =
            new SqliteConnection(_connectionString);

        string sql = @"
            SELECT
                id AS Id,
                name AS Name,
                model AS Model,
                capacity AS Capacity
            FROM Plane";

        return connection.Query<PlaneModel>(sql).ToList();
    }

    // Save a new flight in the database.
    public void AddFlight(FlightModel flight)
    {
        using SqliteConnection connection =
            new SqliteConnection(_connectionString);

        string sql = @"
            INSERT INTO Flight
            (
                plane_id,
                destination_id,
                start_airport_id,
                price,
                arrival_datetime,
                departure_datetime
            )
            VALUES
            (
                @PlaneId,
                @DestinationId,
                @StartAirportId,
                @Price,
                @ArrivalDatetime,
                @DepartureDatetime
            )";

        connection.Execute(sql, flight);
    }
}