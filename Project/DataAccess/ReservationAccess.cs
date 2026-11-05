using Microsoft.Data.Sqlite;
using Dapper;

public class ReservationAccess
{
    private readonly SqliteConnection _connection =
        new SqliteConnection("Data Source=DataSources/database.db");

    public List<FlightModel> GetFlights()
    {
        string sql = @"
            SELECT
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

    public void AddReservation(ReservationModel reservation)
    {
        string sql = @"
            INSERT INTO Reservation
            (account_id, flight_id, ticket_price, status)
            VALUES
            (@AccountId, @FlightId, @TicketPrice, @Status)";

        _connection.Execute(sql, reservation);
    }

    public void AddPassenger(PassengerModel passenger)
    {
        string sql = @"
        INSERT INTO Passenger
        (account_id, first_name, last_name, dob)
        VALUES
        (@AccountId, @FirstName, @LastName, @Dob)";

        _connection.Execute(sql, passenger);
    }
}