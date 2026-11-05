using Microsoft.Data.Sqlite;

using Dapper;
public class SeatAccess
{
    private readonly SqliteConnection _connection = new SqliteConnection($"Data Source=DataSources/database.db");

    private const string Table = "Seat";

    public void Write(SeatModel seat)
    {
        string sql = $@"INSERT INTO {Table} (seat_number, row_number, price, is_reserved)
            VALUES (@SeatNumber, @RowNumber, @Price, @IsReserved)";
        _connection.Execute(sql, seat);
    }

    public SeatModel GetById(Int64 id)
    {
        string sql = @"SELECT id AS Id, seat_number AS SeatNumber, row_number AS RowNumber, price AS Price, is_reserved AS IsReserved
            FROM Seat WHERE id = @Id";
        return _connection.QueryFirstOrDefault<SeatModel>(sql, new { Id = id });
    }
}