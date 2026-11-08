using Microsoft.Data.Sqlite;
using Dapper;

public class TicketAccess
{
    private readonly SqliteConnection _connection =
        new SqliteConnection("Data Source=DataSources/database.db");

    public List<TicketModel> GetByAccountId(Int64 accountId)
    {
        string sql = @"
            SELECT
                pr.id AS Id,
                r.id AS ReservationId,
                r.account_id AS AccountId,
                r.flight_id AS FlightId,
                p.first_name AS FirstName,
                p.last_name AS LastName,
                COALESCE(s.seat_number, 'Unassigned') AS Seat
            FROM Reservation r
            INNER JOIN PassengerReservation pr ON pr.reservation_id = r.id
            INNER JOIN Passenger p ON p.id = pr.passenger_id
            LEFT JOIN SeatAssignment sa ON sa.passenger_reservation_id = pr.id
            LEFT JOIN Seat s ON s.id = sa.seat_id
            WHERE r.account_id = @AccountId
            ORDER BY r.id ASC, pr.id ASC";

        return _connection.Query<TicketModel>(sql, new { AccountId = accountId }).ToList();
    }

    public List<TicketModel> GetByReservationId(Int64 reservationId)
    {
        string sql = @"
            SELECT
                pr.id AS Id,
                r.id AS ReservationId,
                r.account_id AS AccountId,
                r.flight_id AS FlightId,
                p.first_name AS FirstName,
                p.last_name AS LastName,
                COALESCE(s.seat_number, 'Unassigned') AS Seat
            FROM Reservation r
            INNER JOIN PassengerReservation pr ON pr.reservation_id = r.id
            INNER JOIN Passenger p ON p.id = pr.passenger_id
            LEFT JOIN SeatAssignment sa ON sa.passenger_reservation_id = pr.id
            LEFT JOIN Seat s ON s.id = sa.seat_id
            WHERE r.id = @ReservationId
            ORDER BY pr.id ASC";

        return _connection.Query<TicketModel>(sql, new { ReservationId = reservationId }).ToList();
    }

    public int CountTicketsForFlight(Int64 flightId)
    {
        string sql = @"
            SELECT COUNT(*)
            FROM PassengerReservation pr
            INNER JOIN Reservation r ON r.id = pr.reservation_id
            WHERE r.flight_id = @FlightId";

        return _connection.ExecuteScalar<int>(sql, new { FlightId = flightId });
    }

    public Int64 GetLatestReservationId(Int64 accountId)
    {
        string sql = "SELECT id FROM Reservation WHERE account_id = @AccountId ORDER BY id DESC LIMIT 1";
        return _connection.QueryFirstOrDefault<Int64>(sql, new { AccountId = accountId });
    }
}
