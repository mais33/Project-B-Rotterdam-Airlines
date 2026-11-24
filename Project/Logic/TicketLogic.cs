
public class TicketLogic
{
    private TicketAccess _access = new();
    
    public List<TicketDisplayModel> GetTicketsByAccountId(Int64 accountId)
    {
        return _access.GetTicketsByAccountId(accountId);
    }

    public Int64 AddTicket(TicketModel ticket)
    {
        return _access.AddTicket(ticket);
    }

    public List<TicketDisplayModel> GetTicketsByReservationId(Int64 reservationId)
    {
        return _access.GetTicketsByReservationId(reservationId);
    }
    public List<TicketModel> GetTicketsForAccount(Int64 accountId)
    {
        return _access.GetByAccountId(accountId);
    }
}
