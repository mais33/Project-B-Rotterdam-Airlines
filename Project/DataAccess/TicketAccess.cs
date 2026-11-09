using Microsoft.Data.Sqlite;
using Dapper;

public class TicketAccess
{
    private SqliteConnection _connection = new SqliteConnection($"Data Source=DataSources/database.db");

    private readonly string Table = "Ticket";

    public void CreateTable()
    {
        string sql = $@"CREATE TABLE IF NOT EXISTS {Table} (
            id INTEGER PRIMARY KEY AUTOINCREMENT,
            reservation_id INTEGER,
            account_id INTEGER,
            flight_id INTEGER,
            booking_reference TEXT,
            first_name TEXT,
            last_name TEXT,
            seat TEXT)";
        _connection.Execute(sql);
    }

    public void Write(TicketModel ticket)
    {
        string sql = $@"INSERT INTO {Table} (reservation_id, account_id, flight_id, booking_reference, first_name, last_name, seat)
            VALUES (@ReservationId, @AccountId, @FlightId, @BookingReference, @FirstName, @LastName, @Seat);
            SELECT last_insert_rowid();";
        ticket.Id = _connection.ExecuteScalar<long>(sql, ticket);
    }

    public List<TicketModel> SaveBooking(long accountId, long flightId, double price,
        string reference, List<string> firstNames, List<string> lastNames, List<string> birthDates)
    {
        _connection.Open();
        try
        {
            using var transaction = _connection.BeginTransaction();
            long reservationId = _connection.ExecuteScalar<long>(
                "INSERT INTO Reservation (account_id, flight_id, ticket_price, status) VALUES (@accountId, @flightId, @price, 'Confirmed'); SELECT last_insert_rowid();",
                new { accountId, flightId, price }, transaction);
            int seatNumber = _connection.ExecuteScalar<int>(
                "SELECT COUNT(*) FROM Ticket WHERE flight_id = @flightId", new { flightId }, transaction);
            List<TicketModel> tickets = new List<TicketModel>();
            for (int i = 0; i < firstNames.Count; i++)
            {
                string firstName = firstNames[i];
                string lastName = lastNames[i];
                string dob = birthDates[i];
                _connection.Execute(
                    "INSERT INTO Passenger (account_id, first_name, last_name, dob) VALUES (@accountId, @firstName, @lastName, @dob)",
                    new { accountId, firstName, lastName, dob }, transaction);
                string seat = (seatNumber / 6 + 1).ToString() + "ABCDEF"[seatNumber % 6];
                TicketModel ticket = new TicketModel(0, reservationId, accountId, flightId, reference, firstName, lastName, seat);
                ticket.Id = _connection.ExecuteScalar<long>(
                    "INSERT INTO Ticket (reservation_id, account_id, flight_id, booking_reference, first_name, last_name, seat) VALUES (@ReservationId, @AccountId, @FlightId, @BookingReference, @FirstName, @LastName, @Seat); SELECT last_insert_rowid();",
                    ticket, transaction);
                tickets.Add(ticket);
                seatNumber++;
            }
            transaction.Commit();
            return tickets;
        }
        finally
        {
            _connection.Close();
        }
    }

    public List<TicketModel> GetByAccountId(Int64 accountId)
    {
        string sql = $@"SELECT id AS Id, reservation_id AS ReservationId, account_id AS AccountId, flight_id AS FlightId,
            booking_reference AS BookingReference, first_name AS FirstName, last_name AS LastName, seat AS Seat
            FROM {Table} WHERE account_id = @AccountId";
        return _connection.Query<TicketModel>(sql, new { AccountId = accountId }).ToList();
    }

    public int CountTicketsForFlight(Int64 flightId)
    {
        string sql = $"SELECT COUNT(*) FROM {Table} WHERE flight_id = @FlightId";
        return _connection.ExecuteScalar<int>(sql, new { FlightId = flightId });
    }

    public Int64 GetLatestReservationId(Int64 accountId)
    {
        string sql = "SELECT id FROM Reservation WHERE account_id = @AccountId ORDER BY id DESC LIMIT 1";
        return _connection.QueryFirstOrDefault<Int64>(sql, new { AccountId = accountId });
    }
}
