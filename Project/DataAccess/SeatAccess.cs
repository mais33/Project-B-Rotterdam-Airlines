using Microsoft.Data.Sqlite;
using Dapper;

public class SeatAccess
{
    private readonly SqliteConnection _connection =
        new SqliteConnection("Data Source=DataSources/database.db");

    public List<SeatModel> GetSeatsByPlaneId(Int64 planeId)
    {
        string sql = @"
            SELECT
                id AS Id,
                plane_id AS PlaneId,
                seat_number AS SeatNumber,
                row_number AS RowNumber,
                price AS Price,
                is_reserved AS IsReserved
            FROM Seat
            WHERE plane_id = @PlaneId
            ORDER BY row_number ASC, seat_number ASC";

        return _connection.Query<SeatModel>(sql, new { PlaneId = planeId }).ToList();
    }

    public SeatModel? GetSeatById(Int64 id)
    {
        string sql = @"
            SELECT
                id AS Id,
                plane_id AS PlaneId,
                seat_number AS SeatNumber,
                row_number AS RowNumber,
                price AS Price,
                is_reserved AS IsReserved
            FROM Seat
            WHERE id = @Id";

        return _connection.QueryFirstOrDefault<SeatModel>(sql, new { Id = id });
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
                    INSERT INTO Plane (name, model, capacity)
                    VALUES ('Rotterdam Airlines 01', 'Boeing 737-800', 96);
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
                for (int col = 0; col < seatLetters.Length; col++)
                {
                    string seatNumber = $"{row}{seatLetters[col]}";
                    _connection.Execute(
                        @"INSERT INTO Seat (plane_id, seat_number, row_number, price, is_reserved)
                          VALUES (@PlaneId, @SeatNumber, @RowNumber, @Price, 0)",
                        new
                        {
                            PlaneId = planeId,
                            SeatNumber = seatNumber,
                            RowNumber = row,
                            Price = price
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
                s.is_reserved AS IsReserved
            FROM Seat s
            WHERE s.plane_id = @PlaneId
              AND s.id NOT IN (
                  SELECT sa.seat_id
                  FROM SeatAssignment sa
                  INNER JOIN PassengerReservation pr ON pr.id = sa.passenger_reservation_id
                  INNER JOIN Reservation r ON r.id = pr.reservation_id
                  WHERE r.flight_id = @FlightId
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
                  SELECT sa.seat_id
                  FROM SeatAssignment sa
                  INNER JOIN PassengerReservation pr ON pr.id = sa.passenger_reservation_id
                  INNER JOIN Reservation r ON r.id = pr.reservation_id
                  WHERE r.flight_id = @FlightId
              )";

        return _connection.ExecuteScalar<int>(
            sql,
            new { PlaneId = planeId, FlightId = flightId }
        );
    }

    public void SetSeatReserved(Int64 seatId, bool isReserved)
    {
        string sql = "UPDATE Seat SET is_reserved = @IsReserved WHERE id = @Id";
        _connection.Execute(sql, new { IsReserved = isReserved ? 1 : 0, Id = seatId });
    }

    public Int64 AddSeatAssignment(SeatAssignmentModel assignment)
    {
        string sql = @"
            INSERT INTO SeatAssignment (passenger_reservation_id, seat_id)
            VALUES (@PassengerReservationId, @SeatId);
            SELECT last_insert_rowid();";

        assignment.Id = _connection.ExecuteScalar<Int64>(sql, assignment);
        return assignment.Id;
    }
}
