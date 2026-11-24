public class ReservationDetailsModel
{
    public long ReservationId { get; set; }
    public long AccountId { get; set; }
    public string CustomerName { get; set; } = "";
    public long FlightId { get; set; }
    public string DepartureCity { get; set; } = "";
    public string DestinationCity { get; set; } = "";
    public string DepartureDateTime { get; set; } = "";
    public double TotalPrice { get; set; }
    public string Status { get; set; } = "";
    public long? BookingGroupId { get; set; }

    public List<ReservationPassengerDetailsModel> Passengers { get; set; } = new();
}

public class ReservationPassengerDetailsModel
{
    public string FirstName { get; set; } = "";
    public string LastName { get; set; } = "";
    public string SeatNumber { get; set; } = "";
}
