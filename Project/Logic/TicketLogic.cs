public class TicketLogic
{
    private TicketAccess _access = new();
    private Random _random = new();

    public TicketLogic()
    {
        _access.CreateTable();
    }

    public string MakeBookingReference()
    {
        string characters = "ABCDEFGHJKLMNPQRSTUVWXYZ23456789";
        string reference = "";

        for (int i = 0; i < 6; i++)
        {
            int index = _random.Next(characters.Length);
            reference += characters[index];
        }

        return reference;
    }

    public string GetSeat(int seatNumber)
    {
        string seatLetters = "ABCDEF";

        int row = seatNumber / 6 + 1;
        char letter = seatLetters[seatNumber % 6];

        return row.ToString() + letter;
    }

    public TicketModel CreateTicket(Int64 accountId, Int64 flightId, string bookingReference, string firstName, string lastName)
    {
        Int64 reservationId = _access.GetLatestReservationId(accountId);

        int ticketsOnFlight = _access.CountTicketsForFlight(flightId);
        string seat = GetSeat(ticketsOnFlight);

        TicketModel ticket = new TicketModel(0, reservationId, accountId, flightId, bookingReference, firstName, lastName, seat);
        _access.Write(ticket);

        return ticket;
    }

    public List<TicketModel> GetTicketsForAccount(Int64 accountId)
    {
        return _access.GetByAccountId(accountId);
    }

}
