using Microsoft.Data.Sqlite;
using Dapper;

public class SeatAccess
{
    private readonly SqliteConnection _connection =
        new SqliteConnection("Data Source=DataSources/database.db");

    private const string Table = "Seat";
    private static readonly HashSet<Int64> _reservedSeatIds = new HashSet<Int64>();

    public void Write(SeatModel seat)
    {
        if (seat.PlaneId <= 0)
        {
            seat.PlaneId = 1;
        }

        string sql = $@"INSERT INTO {Table} (plane_id, seat_number, row_number, price, tier)
            VALUES (@PlaneId, @SeatNumber, @RowNumber, @Price, @Tier)";
        _connection.Execute(sql, seat);
    }

    public SeatModel? GetSeatById(Int64 id)
    {
        string sql = $@"
            SELECT
                id AS Id,
                plane_id AS PlaneId,
                seat_number AS SeatNumber,
                row_number AS RowNumber,
                price AS Price,
                tier AS Tier,
                CASE WHEN id IN (SELECT seat_id FROM Ticket WHERE seat_id IS NOT NULL) THEN 1 ELSE 0 END AS IsReserved
            FROM {Table}
            WHERE id = @Id";

        SeatModel? seat = _connection.QueryFirstOrDefault<SeatModel>(sql, new { Id = id });
        if (seat != null && _reservedSeatIds.Contains(seat.Id))
        {
            seat.IsReserved = true;
        }

        return seat;
    }

    public List<SeatModel> GetAllSeats()
    {
        string sql = $@"
            SELECT
                id AS Id,
                plane_id AS PlaneId,
                seat_number AS SeatNumber,
                row_number AS RowNumber,
                price AS Price,
                tier AS Tier,
                CASE WHEN id IN (SELECT seat_id FROM Ticket WHERE seat_id IS NOT NULL) THEN 1 ELSE 0 END AS IsReserved
            FROM {Table}
            WHERE plane_id = 1
            ORDER BY row_number ASC, seat_number ASC";

        List<SeatModel> seats = _connection.Query<SeatModel>(sql).ToList();
        for (int i = 0; i < seats.Count; i++)
        {
            if (_reservedSeatIds.Contains(seats[i].Id))
            {
                seats[i].IsReserved = true;
            }
        }

        return seats;
    }

    public List<SeatModel> GetSeatsByPlaneId(Int64 planeId)
    {
        string sql = @"
            SELECT
                id AS Id,
                plane_id AS PlaneId,
                seat_number AS SeatNumber,
                row_number AS RowNumber,
                price AS Price,
                tier AS Tier
            FROM Seat
            WHERE plane_id = @PlaneId
            ORDER BY row_number ASC, seat_number ASC";

        return _connection.Query<SeatModel>(sql, new { PlaneId = planeId }).ToList();
    }

    public void ClearSeats()
    {
        _connection.Execute("PRAGMA foreign_keys = OFF;");
        _connection.Execute("DELETE FROM Seat");
        _connection.Execute("PRAGMA foreign_keys = ON;");
        _reservedSeatIds.Clear();
    }

    public void SetSeatAsReserved(SeatModel seat)
    {
        _reservedSeatIds.Add(seat.Id);
        seat.IsReserved = true;
    }

    public void SetSeatAsUnReserved(SeatModel seat)
    {
        _reservedSeatIds.Remove(seat.Id);
        seat.IsReserved = false;
    }

    public Int64 EnsureSeatsForPlane(Int64 planeId)
    {
        if (planeId <= 0)
        {
            planeId = _connection.QueryFirstOrDefault<Int64>(
                "SELECT id FROM Plane ORDER BY id ASC LIMIT 1"
            );

            if (planeId <= 0)
            {
                planeId = _connection.ExecuteScalar<Int64>(@"
                    INSERT INTO Plane (name, model)
                    VALUES ('Rotterdam Airlines 01', 'Boeing 737-800');
                    SELECT last_insert_rowid();");
            }
        }

        int existingCount = _connection.ExecuteScalar<int>(
            "SELECT COUNT(*) FROM Seat WHERE plane_id = @PlaneId",
            new { PlaneId = planeId }
        );

        if (existingCount == 0)
        {
            string seatLetters = "ABCDEF";
            for (int row = 1; row <= 16; row++)
            {
                double price = row <= 2 ? 25.0 : (row <= 5 ? 15.0 : 0.0);
                int tier = row <= 2 ? 3 : (row <= 5 ? 2 : 1);

                for (int col = 0; col < seatLetters.Length; col++)
                {
                    string seatNumber = $"{row}{seatLetters[col]}";
                    _connection.Execute(
                        @"INSERT INTO Seat (plane_id, seat_number, row_number, price, tier)
                          VALUES (@PlaneId, @SeatNumber, @RowNumber, @Price, @Tier)",
                        new
                        {
                            PlaneId = planeId,
                            SeatNumber = seatNumber,
                            RowNumber = row,
                            Price = price,
                            Tier = tier
                        }
                    );
                }
            }
        }

        return planeId;
    }

    public SeatModel? GetNextAvailableSeatForFlight(Int64 flightId, Int64 planeId)
    {
        planeId = EnsureSeatsForPlane(planeId);

        string sql = @"
            SELECT
                s.id AS Id,
                s.plane_id AS PlaneId,
                s.seat_number AS SeatNumber,
                s.row_number AS RowNumber,
                s.price AS Price,
                s.tier AS Tier
            FROM Seat s
            WHERE s.plane_id = @PlaneId
              AND s.id NOT IN (
                  SELECT t.seat_id
                  FROM Ticket t
                  INNER JOIN Reservation r ON r.id = t.reservation_id
                  WHERE r.flight_id = @FlightId
                    AND t.seat_id IS NOT NULL
              )
            ORDER BY s.row_number ASC, s.seat_number ASC
            LIMIT 1";

        return _connection.QueryFirstOrDefault<SeatModel>(
            sql,
            new { PlaneId = planeId, FlightId = flightId }
        );
    }

    public int CountAvailableSeatsForFlight(Int64 flightId, Int64 planeId)
    {
        planeId = EnsureSeatsForPlane(planeId);

        string sql = @"
            SELECT COUNT(*)
            FROM Seat s
            WHERE s.plane_id = @PlaneId
              AND s.id NOT IN (
                  SELECT t.seat_id
                  FROM Ticket t
                  INNER JOIN Reservation r ON r.id = t.reservation_id
                  WHERE r.flight_id = @FlightId
                    AND t.seat_id IS NOT NULL
              )";

        return _connection.ExecuteScalar<int>(
            sql,
            new { PlaneId = planeId, FlightId = flightId }
        );
    }
}
