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
            VALUES (@ReservationId, @AccountId, @FlightId, @BookingReference, @FirstName, @LastName, @Seat)";
        _connection.Execute(sql, ticket);
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
