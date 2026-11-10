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

    public Int64 AddReservation(ReservationModel reservation)
    {
        string sql = @"
            INSERT INTO Reservation
            (account_id, flight_id, ticket_price, status)
            VALUES
            (@AccountId, @FlightId, @TicketPrice, @Status);
            SELECT last_insert_rowid();";

        reservation.Id = _connection.ExecuteScalar<Int64>(sql, reservation);
        return reservation.Id;
    }

    public Int64 AddPassenger(PassengerModel passenger)
    {
        string sql = @"
            INSERT INTO Passenger
            (first_name, last_name, date_of_birth, passport_number)
            VALUES
            (@FirstName, @LastName, @DateOfBirth, @PassportNumber);
            SELECT last_insert_rowid();";

        passenger.Id = _connection.ExecuteScalar<Int64>(sql, passenger);
        return passenger.Id;
    }

    public List<ReservationModel> GetReservationsByAccountId(Int64 accountId)
    {
        string sql = @"
            SELECT
                id AS Id,
                account_id AS AccountId,
                flight_id AS FlightId,
                ticket_price AS TicketPrice,
                status AS Status
            FROM Reservation
            WHERE account_id = @AccountId
            ORDER BY id ASC";

        return _connection.Query<ReservationModel>(sql, new { AccountId = accountId }).ToList();
    }
}
