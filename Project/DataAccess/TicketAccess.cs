using Microsoft.Data.Sqlite;
using Dapper;

public class TicketAccess
{
    private SqliteConnection _connection = new SqliteConnection($"Data Source=DataSources/database.db");

    private const string TicketTable = "Ticket";
    private const string SeatTable = "Seat";
    private const string PassengerTable = "Passenger";
    private const string ReservationTable = "Reservation";
    private const string AccountTable = "Account";
    private const string FlightTable = "Flight";


    public TicketDisplayModel? DisplayTicket(Int64 ticketId)
    {
        string sql = $@"
            SELECT 
                {TicketTable}.id AS Id, 
                {SeatTable}.seat_number AS SeatNumber, 
                {SeatTable}.row_number AS RowNumber, 
                {SeatTable}.price AS Price,
                {PassengerTable}.first_name AS FirstName, 
                {PassengerTable}.last_name AS LastName, 
                {PassengerTable}.dob AS Dob,
                {ReservationTable}.ticket_price AS TicketPrice, 
                {ReservationTable}.status AS Status,
                {AccountTable}.email AS Email,
                {FlightTable}.id AS FlightId, 
                {FlightTable}.arrival_datetime AS ArrivalDatetime, 
                {FlightTable}.departure_datetime AS DepartureDatetime
            FROM {TicketTable}
            INNER JOIN {SeatTable}        ON {TicketTable}.seat_id        = {SeatTable}.id
            INNER JOIN {PassengerTable}   ON {TicketTable}.passenger_id   = {PassengerTable}.id
            INNER JOIN {ReservationTable} ON {TicketTable}.reservation_id = {ReservationTable}.id
            INNER JOIN {AccountTable}     ON {ReservationTable}.account_id = {AccountTable}.id
            INNER JOIN {FlightTable} ON {ReservationTable}.flight_id = {FlightTable}.id
            WHERE {TicketTable}.id = @TicketId;";

        return _connection.QueryFirstOrDefault<TicketDisplayModel>(sql, new { TicketId = ticketId });
    }



    public int CountTicketsForFlight(Int64 flightId)
    {
        return 1;
    }

}
