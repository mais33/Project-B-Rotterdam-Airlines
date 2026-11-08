public class SeatAssignmentModel
{
    public Int64 Id { get; set; }
    public Int64 PassengerReservationId { get; set; }
    public Int64 SeatId { get; set; }

    public SeatAssignmentModel()
    {
    }

    public SeatAssignmentModel(Int64 id, Int64 passengerReservationId, Int64 seatId)
    {
        Id = id;
        PassengerReservationId = passengerReservationId;
        SeatId = seatId;
    }
}
