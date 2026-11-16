public class TicketLogic
{
    private TicketAccess _access = new();
    
    public TicketDisplayModel DisplayTicket(Int64 ticketId)
    {
        return _access.DisplayTicket(ticketId);
    }
}
