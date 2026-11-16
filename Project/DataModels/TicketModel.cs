public class TicketModel
{
    public Int64 Id {get;set;}
    public Int64 PassengerId {get;set;}
    public Int64 ReservationId {get;set;}
    public Int64 SeatId {get;set;}

    public TicketModel(){}

    public TicketModel(Int64 id, Int64 passengerId, Int64 reservationId, Int64 seatId)
    {
        Id = id;
        PassengerId = passengerId;
        ReservationId = reservationId;
        SeatId = seatId;
    }
}