
using Microsoft.Data.Sqlite;
using Dapper;

public class TicketAccess
{
    private readonly SqliteConnection _connection = new SqliteConnection($"Data Source=DataSources/database.db");

    private const string TicketTable = "Ticket";
    private const string SeatTable = "Seat";
    private const string PassengerTable = "Passenger";
    private const string ReservationTable = "Reservation";
    private const string AccountTable = "Account";
    private const string FlightTable = "Flight";
    private const string AirPortTable = "Airport";

    public List<TicketDisplayModel> GetTicketsByReservationId(Int64 reservationId)
    {
        string sql = $@"
            SELECT 
                t.id AS Id,
                s.seat_number AS SeatNumber,
                s.row_number AS RowNumber,
                s.price AS Price,
                p.first_name AS FirstName,
                p.last_name AS LastName,
                p.date_of_birth AS Dob,
                a.email AS Email,
                r.ticket_price AS TicketPrice,
                r.status AS Status,
                f.id AS FlightId,
                f.arrival_datetime AS ArrivalDatetime,
                f.departure_datetime AS DepartureDatetime,
                orig.city AS Origin,
                dest.city AS Destination
            FROM {TicketTable} t
            INNER JOIN {PassengerTable} p ON t.passenger_id = p.id
            INNER JOIN {SeatTable} s ON t.seat_id = s.id
            INNER JOIN {ReservationTable} r ON t.reservation_id = r.id
            INNER JOIN {AccountTable} a ON r.account_id = a.id
            INNER JOIN {FlightTable} f ON r.flight_id = f.id
            INNER JOIN {AirPortTable} orig ON f.start_airport_id = orig.id
            INNER JOIN {AirPortTable} dest ON f.destination_id = dest.id
            WHERE t.reservation_id = @ReservationId;";

        return _connection.Query<TicketDisplayModel>(sql, new { ReservationId = reservationId }).ToList();
    }

    public List<TicketDisplayModel> GetTicketsByAccountId(Int64 accountId)
    {
        string sql = $@"
            SELECT 
                t.id AS Id,
                s.seat_number AS SeatNumber,
                s.row_number AS RowNumber,
                s.price AS Price,
                p.first_name AS FirstName,
                p.last_name AS LastName,
                p.date_of_birth AS Dob,
                a.email AS Email,
                r.ticket_price AS TicketPrice,
                r.status AS Status,
                f.id AS FlightId,
                f.arrival_datetime AS ArrivalDatetime,
                f.departure_datetime AS DepartureDatetime,
                orig.city AS Origin,
                dest.city AS Destination
            FROM {TicketTable} t
            INNER JOIN {PassengerTable} p ON t.passenger_id = p.id
            INNER JOIN {SeatTable} s ON t.seat_id = s.id
            INNER JOIN {ReservationTable} r ON t.reservation_id = r.id
            INNER JOIN {AccountTable} a ON r.account_id = a.id
            INNER JOIN {FlightTable} f ON r.flight_id = f.id
            INNER JOIN {AirPortTable} orig ON f.start_airport_id = orig.id
            INNER JOIN {AirPortTable} dest ON f.destination_id = dest.id
            WHERE r.account_id = @AccountId
            ORDER BY r.id DESC, t.id ASC;";

        return _connection.Query<TicketDisplayModel>(sql, new { AccountId = accountId }).ToList();
    }

    public Int64 AddTicket(TicketModel ticket)
    {
        string sql = @"
            INSERT INTO Ticket (passenger_id, reservation_id, seat_id)
            VALUES (@PassengerId, @ReservationId, @SeatId);
            SELECT last_insert_rowid();";

        ticket.Id = _connection.ExecuteScalar<Int64>(sql, ticket);
        return ticket.Id;
    }

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
