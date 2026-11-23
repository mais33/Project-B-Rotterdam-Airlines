using Microsoft.Data.Sqlite;

using Dapper;
public class SeatAccess
{
    private readonly SqliteConnection _connection = new SqliteConnection($"Data Source=DataSources/database.db");

    private const string Table = "Seat";

    public void Write(SeatModel seat)
    {
        string sql = $@"INSERT INTO {Table} (seat_number, row_number, price, is_reserved, tier)
            VALUES (@SeatNumber, @RowNumber, @Price, @IsReserved, @Tier)";
        _connection.Execute(sql, seat);
    }

    public SeatModel GetSeatById(Int64 id)
    {
        string sql = $@"SELECT id AS Id, seat_number AS SeatNumber, row_number AS RowNumber, price AS Price, is_reserved AS IsReserved
            FROM {Table} WHERE id = @Id";
        return _connection.QueryFirstOrDefault<SeatModel>(sql, new { Id = id });
    }

    public List<SeatModel> GetAllSeats()
    {
        string sql = $@"SELECT id AS Id, seat_number AS SeatNumber, row_number AS RowNumber, price AS Price,
             is_reserved AS IsReserved, tier AS Tier FROM {Table}";
        return _connection.Query<SeatModel>(sql).ToList();
    }

    public void ClearSeats()
    {
        // string sql = @"DELETE FROM Account;
        //     DELETE FROM sqlite_sequence WHERE name = 'Seat';";
        // _connection.Execute(sql);

        // 1. Temporarily disable foreign key constraints for this operation
    _connection.Execute("PRAGMA foreign_keys = OFF;");

    // 2. Perform your clear/delete operation on the Seat table
    _connection.Execute("DELETE FROM Seat");

    // 3. Re-enable foreign key constraints to keep your database secure
    _connection.Execute("PRAGMA foreign_keys = ON;");
    }

    public void SetSeatAsReserved(SeatModel seat)
    {
        string sql = $@"Update {Table} Set is_reserved = 1 WHERE id = @Id";
        _connection.Execute(sql, new { Id = seat.Id });
    }

    public void SetSeatAsUnReserved(SeatModel seat)
    {
        string sql = $@"Update {Table} Set is_reserved = 0 WHERE id = @Id";
        _connection.Execute(sql, new { Id = seat.Id });
    }
}