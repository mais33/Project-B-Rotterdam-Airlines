using Microsoft.Data.Sqlite;

using Dapper;
public class SeatAccess
{
    private readonly SqliteConnection _connection = new SqliteConnection($"Data Source=DataSources/database.db");

    private const string Table = "Seat";


    public void Write(SeatModel seat, Int64 planeId)
    {
        const string sql = @"
        INSERT INTO Seat
            (plane_id, seat_number, row_number,
             price, is_reserved, tier)
        VALUES
            (@PlaneId, @SeatNumber, @RowNumber,
             @Price, @IsReserved, @Tier)";

        _connection.Execute(sql, new
        {
            PlaneId = planeId,
            seat.SeatNumber,
            seat.RowNumber,
            seat.Price,
            seat.IsReserved,
            seat.Tier
        });
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


    public List<SeatModel> GetSeatsByPlaneId(Int64 planeId)
    {
        const string sql = @"
        SELECT
            id AS Id,
            seat_number AS SeatNumber,
            row_number AS RowNumber,
            price AS Price,
            is_reserved AS IsReserved,
            tier AS Tier
        FROM Seat
        WHERE plane_id = @PlaneId
        ORDER BY row_number, seat_number";

        return _connection.Query<SeatModel>(
            sql,
            new { PlaneId = planeId }
        ).ToList();
    }


    public List<SeatModel> GetSeatsByFlightId(Int64 flightId)
    {
        const string sql = @"
        SELECT
            s.id AS Id,
            s.seat_number AS SeatNumber,
            s.row_number AS RowNumber,
            s.price AS Price,
            CASE
                WHEN EXISTS (
                    SELECT 1
                    FROM Ticket t
                    INNER JOIN Reservation r
                        ON r.id = t.reservation_id
                    WHERE t.seat_id = s.id
                      AND r.flight_id = @FlightId
                      AND r.status = 'Confirmed'
                )
                THEN 1
                ELSE 0
            END AS IsReserved,
            s.tier AS Tier
        FROM Seat s
        INNER JOIN Flight f
            ON f.plane_id = s.plane_id
        WHERE f.id = @FlightId
        ORDER BY s.row_number, s.seat_number";

        return _connection.Query<SeatModel>(
            sql,
            new { FlightId = flightId }
        ).ToList();
    }


    public List<Int64> GetPlaneIds()
    {
        const string sql = "SELECT id FROM Plane ORDER BY id";

        return _connection.Query<Int64>(sql).ToList();
    }


}