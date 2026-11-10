public class TicketLogic
{
    private readonly TicketAccess _access = new();
    private readonly SeatAccess _seatAccess = new();
    private readonly FlightAccess _flightAccess = new();
    private readonly ReservationAccess _reservationAccess = new();
    private readonly Random _random = new();

    public TicketLogic()
    {
    }

    public string MakeBookingReference(Int64 reservationId)
    {
        string characters = "ABCDEFGHJKLMNPQRSTUVWXYZ23456789";
        long encoded = ((reservationId * 694837L) + 123457L) & 0xFFFFFL;
        string reference = "RA";

        for (int i = 0; i < 4; i++)
        {
            int index = (int)(encoded % characters.Length);
            reference += characters[index];
            encoded /= characters.Length;
        }

        return reference;
    }

    public string MakeBookingReference()
    {
        string characters = "ABCDEFGHJKLMNPQRSTUVWXYZ23456789";
        string reference = "RA";

        for (int i = 0; i < 4; i++)
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

    public TicketModel CreateTicket(
        Int64 reservationId,
        Int64 passengerId,
        Int64 accountId,
        FlightModel flight,
        string bookingReference,
        string firstName,
        string lastName)
    {
        SeatModel? assignedSeat = _seatAccess.GetNextAvailableSeatForFlight(flight.Id, flight.PlaneId);
        string seatNumber;
        Int64 seatId = 0;

        if (assignedSeat != null)
        {
            seatId = assignedSeat.Id;
            seatNumber = assignedSeat.SeatNumber;
        }
        else
        {
            int ticketsOnFlight = _access.CountTicketsForFlight(flight.Id);
            seatNumber = GetSeat(Math.Max(0, ticketsOnFlight - 1));
        }

        TicketModel ticketRow = new TicketModel(0, passengerId, reservationId, seatId);
        Int64 ticketId = _access.AddTicket(ticketRow);

        TicketModel ticket = new TicketModel(
            ticketId,
            reservationId,
            accountId,
            flight.Id,
            bookingReference,
            firstName,
            lastName,
            seatNumber
        );
        ticket.PassengerId = passengerId;
        ticket.SeatId = seatId;

        return ticket;
    }

    public TicketModel CreateTicket(
        Int64 accountId,
        Int64 flightId,
        string bookingReference,
        string firstName,
        string lastName)
    {
        Int64 reservationId = _access.GetLatestReservationId(accountId);
        FlightModel flight = _flightAccess.GetById(flightId)
            ?? new FlightModel(flightId, 0, 0, 1, 0, "", "");

        Int64 passengerId = _reservationAccess.AddPassenger(new PassengerModel(0, firstName, lastName, ""));

        string resolvedReference = reservationId > 0
            ? MakeBookingReference(reservationId)
            : bookingReference;

        return CreateTicket(
            reservationId,
            passengerId,
            accountId,
            flight,
            resolvedReference,
            firstName,
            lastName
        );
    }

    public List<TicketModel> GetTicketsForAccount(Int64 accountId)
    {
        List<TicketModel> tickets = _access.GetByAccountId(accountId);
        for (int i = 0; i < tickets.Count; i++)
        {
            tickets[i].BookingReference = MakeBookingReference(tickets[i].ReservationId);
        }

        return tickets;
    }
}
