public class ReservationModel
{
    public Int64 Id { get; set; }
    public Int64 AccountId { get; set; }
    public Int64 FlightId { get; set; }
    public double TicketPrice { get; set; }
    public string Status { get; set; }

    public ReservationModel(
        Int64 id,
        Int64 accountId,
        Int64 flightId,
        double ticketPrice,
        string status)
    {
        Id = id;
        AccountId = accountId;
        FlightId = flightId;
        TicketPrice = ticketPrice;
        Status = status;
    }
}