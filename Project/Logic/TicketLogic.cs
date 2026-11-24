
public class TicketLogic
{
    private readonly TicketAccess _access = new();

    public List<TicketModel> GetTicketsForAccount(Int64 accountId)
    {
        return _access.GetByAccountId(accountId);
    }
}
