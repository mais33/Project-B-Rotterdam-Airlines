
using Microsoft.Data.Sqlite;
using Dapper;

public class TicketAccess
{
    private readonly SqliteConnection _connection =
        new SqliteConnection("Data Source=DataSources/database.db");

    public List<TicketModel> GetByAccountId(Int64 accountId)
    {
        const string sql = @"
            SELECT
                t.id AS Id,
                t.reservation_id AS ReservationId,
                r.account_id AS AccountId,
                r.flight_id AS FlightId,
                'RES-' || r.id AS BookingReference,
                p.first_name AS FirstName,
                p.last_name AS LastName,
                s.seat_number AS Seat
            FROM Ticket t
            INNER JOIN Reservation r
                ON r.id = t.reservation_id
            INNER JOIN Passenger p
                ON p.id = t.passenger_id
            LEFT JOIN Seat s
                ON s.id = t.seat_id
            WHERE r.account_id = @AccountId
              AND r.status = 'Confirmed'
            ORDER BY r.id, t.id";

        return _connection.Query<TicketModel>(
            sql,
            new { AccountId = accountId }
        ).ToList();
    }
}
