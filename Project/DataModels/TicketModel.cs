public class TicketModel
{
    public Int64 Id { get; set; }
    public Int64 PassengerId { get; set; }
    public Int64 ReservationId { get; set; }
    public Int64 SeatId { get; set; }
    public Int64 AccountId { get; set; }
    public Int64 FlightId { get; set; }
    public string BookingReference { get; set; } = "";
    public string FirstName { get; set; } = "";
    public string LastName { get; set; } = "";
    public string Seat { get; set; } = "";

    public TicketModel()
    {
    }

    public TicketModel(
        Int64 id,
        Int64 passengerId,
        Int64 reservationId,
        Int64 seatId)
    {
        Id = id;
        PassengerId = passengerId;
        ReservationId = reservationId;
        SeatId = seatId;
    }

    public TicketModel(
        Int64 id,
        Int64 reservationId,
        Int64 accountId,
        Int64 flightId,
        string bookingReference,
        string firstName,
        string lastName,
        string seat)
    {
        Id = id;
        ReservationId = reservationId;
        AccountId = accountId;
        FlightId = flightId;
        BookingReference = bookingReference;
        FirstName = firstName;
        LastName = lastName;
        Seat = seat;
    }
}
