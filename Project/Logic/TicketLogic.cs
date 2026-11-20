public class TicketLogic
{
    private TicketAccess _access = new();
    
    public List<TicketDisplayModel> GetTicketsByAccountId(Int64 accountId)
    {
        return _access.GetTicketsByAccountId(accountId);
    }

    public Int64 AddTicket(Int64 passengerId, Int64 reservationId, Int64 seatId)
    {
        return _access.AddTicket(new TicketModel(0, passengerId, reservationId, seatId));
    }

    public List<TicketDisplayModel> GetTicketsByReservationId(Int64 reservationId)
    {
        return _access.GetTicketsByReservationId(reservationId);
    }
}
