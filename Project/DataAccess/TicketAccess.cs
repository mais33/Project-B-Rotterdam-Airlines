using Microsoft.Data.Sqlite;
using Dapper;

public class TicketAccess
{
    private readonly SqliteConnection _connection =
        new SqliteConnection("Data Source=DataSources/database.db");

    public Int64 AddTicket(TicketModel ticket)
    {
        string sql = @"
            INSERT INTO Ticket
            (passenger_id, reservation_id, seat_id)
            VALUES
            (@PassengerId, @ReservationId, @SeatId);
            SELECT last_insert_rowid();";

        ticket.Id = _connection.ExecuteScalar<Int64>(sql, ticket);
        return ticket.Id;
    }

    public List<TicketModel> GetByAccountId(Int64 accountId)
    {
        string sql = @"
            SELECT
                t.id AS Id,
                t.passenger_id AS PassengerId,
                t.reservation_id AS ReservationId,
                t.seat_id AS SeatId,
                r.account_id AS AccountId,
                r.flight_id AS FlightId,
                p.first_name AS FirstName,
                p.last_name AS LastName,
                COALESCE(s.seat_number, 'Unassigned') AS Seat
            FROM Ticket t
            INNER JOIN Reservation r ON r.id = t.reservation_id
            INNER JOIN Passenger p ON p.id = t.passenger_id
            LEFT JOIN Seat s ON s.id = t.seat_id
            WHERE r.account_id = @AccountId
            ORDER BY r.id ASC, t.id ASC";

        return _connection.Query<TicketModel>(sql, new { AccountId = accountId }).ToList();
    }

    public List<TicketModel> GetByReservationId(Int64 reservationId)
    {
        string sql = @"
            SELECT
                t.id AS Id,
                t.passenger_id AS PassengerId,
                t.reservation_id AS ReservationId,
                t.seat_id AS SeatId,
                r.account_id AS AccountId,
                r.flight_id AS FlightId,
                p.first_name AS FirstName,
                p.last_name AS LastName,
                COALESCE(s.seat_number, 'Unassigned') AS Seat
            FROM Ticket t
            INNER JOIN Reservation r ON r.id = t.reservation_id
            INNER JOIN Passenger p ON p.id = t.passenger_id
            LEFT JOIN Seat s ON s.id = t.seat_id
            WHERE t.reservation_id = @ReservationId
            ORDER BY t.id ASC";

        return _connection.Query<TicketModel>(sql, new { ReservationId = reservationId }).ToList();
    }

    public int CountTicketsForFlight(Int64 flightId)
    {
        string sql = @"
            SELECT COUNT(*)
            FROM Ticket t
            INNER JOIN Reservation r ON r.id = t.reservation_id
            WHERE r.flight_id = @FlightId";

        return _connection.ExecuteScalar<int>(sql, new { FlightId = flightId });
    }

    public Int64 GetLatestReservationId(Int64 accountId)
    {
        string sql = "SELECT id FROM Reservation WHERE account_id = @AccountId ORDER BY id DESC LIMIT 1";
        return _connection.QueryFirstOrDefault<Int64>(sql, new { AccountId = accountId });
    }
}
