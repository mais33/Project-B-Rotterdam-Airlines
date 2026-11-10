public class PassengerReservationModel
{
    public Int64 Id { get; set; }
    public Int64 PassengerId { get; set; }
    public Int64 ReservationId { get; set; }

    public PassengerReservationModel()
    {
    }

    public PassengerReservationModel(Int64 id, Int64 passengerId, Int64 reservationId)
    {
        Id = id;
        PassengerId = passengerId;
        ReservationId = reservationId;
    }
}
