using Microsoft.Data.Sqlite;
using Dapper;

public class ReservationAccess
{
    private readonly SqliteConnection _connection =
        new SqliteConnection("Data Source=DataSources/database.db");

    public void CreatePassengerReservationTable()
    {
        string sql = @"
            CREATE TABLE IF NOT EXISTS PassengerReservation (
                id INTEGER PRIMARY KEY AUTOINCREMENT,
                passenger_id INTEGER,
                reservation_id INTEGER,
                FOREIGN KEY (passenger_id) REFERENCES Passenger (id),
                FOREIGN KEY (reservation_id) REFERENCES Reservation (id)
            )";

        _connection.Execute(sql);
    }

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
            (first_name, last_name, dob)
            VALUES
            (@FirstName, @LastName, @Dob);
            SELECT last_insert_rowid();";

        passenger.Id = _connection.ExecuteScalar<Int64>(sql, passenger);
        return passenger.Id;
    }

    public Int64 AddPassengerReservation(PassengerReservationModel passengerReservation)
    {
        CreatePassengerReservationTable();

        string sql = @"
            INSERT INTO PassengerReservation
            (passenger_id, reservation_id)
            VALUES
            (@PassengerId, @ReservationId);
            SELECT last_insert_rowid();";

        passengerReservation.Id = _connection.ExecuteScalar<Int64>(sql, passengerReservation);
        return passengerReservation.Id;
    }

    public List<PassengerReservationModel> GetByReservationId(Int64 reservationId)
    {
        CreatePassengerReservationTable();

        string sql = @"
            SELECT
                id AS Id,
                passenger_id AS PassengerId,
                reservation_id AS ReservationId
            FROM PassengerReservation
            WHERE reservation_id = @ReservationId";

        return _connection.Query<PassengerReservationModel>(
            sql,
            new { ReservationId = reservationId }
        ).ToList();
    }
}
