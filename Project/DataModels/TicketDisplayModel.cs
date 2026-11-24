public class TicketDisplayModel
{
    public Int64 Id { get; set; }
    public string SeatNumber { get; set; }
    public int RowNumber { get; set; }
    public decimal Price { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Dob { get; set; }
    public decimal TicketPrice { get; set; }
    public string Status { get; set; }
    public string Email { get; set; }
    public Int64 FlightId { get; set; }
    public string ArrivalDatetime { get; set; }
    public string DepartureDatetime { get; set; }
    public string Destination { get; set;}
    public string Origin { get; set;}

    public override string ToString()
    {
        return $@"
========================================
            BOARDING PASS #{Id}
========================================
Flight    : {Origin} -> {Destination}
Passenger : {FirstName} {LastName} (DOB: {Dob})
Email     : {Email}
Flight ID : {FlightId}
Departure : {DepartureDatetime}
Arrival   : {ArrivalDatetime}
Seat      : Row {RowNumber}, Seat {SeatNumber}
Price     : €{TicketPrice:F2} (Seat: €{Price:F2})
Status    : {Status}
========================================";
    }
}