static class TicketPresentation
{
    private static TicketLogic ticketLogic = new TicketLogic();
    private static FlightLogic flightLogic = new FlightLogic();

    public static void ShowConfirmation(List<TicketModel> tickets, FlightModel? flight = null)
    {
        Console.Clear();
        Console.WriteLine();
        Console.WriteLine("==================================================");
        Console.WriteLine("               BOOKING CONFIRMED                  ");
        Console.WriteLine("==================================================");
        Console.WriteLine();

        if (tickets.Count == 0)
        {
            Console.WriteLine("  > No tickets were created.");
            Console.WriteLine();
            Console.WriteLine("  Press any key to return...");
            Console.ReadKey();
            return;
        }

        if (flight == null)
        {
            flight = flightLogic.GetFlightById(tickets[0].FlightId);
        }

        string destination = flight != null ? flightLogic.GetDestinationCity(flight) : "Unknown";
        string departure = flight != null ? flight.DepartureDatetime : "Unknown";

        Console.WriteLine("  Booking Details:");
        Console.WriteLine("  ------------------------------------------------");
        Console.WriteLine($"  Booking Reference:  {tickets[0].BookingReference}");
        Console.WriteLine($"  Flight:             Rotterdam -> {destination} (Flight #{tickets[0].FlightId})");
        Console.WriteLine($"  Departure:          {departure}");
        Console.WriteLine("  ------------------------------------------------");
        Console.WriteLine();
        Console.WriteLine("  Issued Tickets:\n");

        for (int i = 0; i < tickets.Count; i++)
        {
            Console.WriteLine($"  [{i + 1}] Ticket Number:    #{tickets[i].Id}");
            Console.WriteLine($"      Passenger:        {tickets[i].FirstName} {tickets[i].LastName}");
            Console.WriteLine($"      Seat:             {tickets[i].Seat}");
            Console.WriteLine();
        }

        Console.WriteLine("--------------------------------------------------");
        Console.WriteLine("  Thank you for flying with Rotterdam Airlines!");
        Console.WriteLine();
        Console.WriteLine("  Press any key to return to the menu...");
        Console.ReadKey();
    }

    public static void ShowMyTickets()
    {
        Console.Clear();
        Console.WriteLine();
        Console.WriteLine("==================================================");
        Console.WriteLine("                    MY TICKETS                    ");
        Console.WriteLine("==================================================");
        Console.WriteLine();

        AccountModel? account = AccountsLogic.CurrentAccount;

        if (account == null)
        {
            Console.WriteLine("  > Please log in first to view your tickets.");
            Console.WriteLine();
            Console.WriteLine("  Press any key to return...");
            Console.ReadKey();
            return;
        }

        List<TicketModel> tickets = ticketLogic.GetTicketsForAccount(account.Id);

        if (tickets.Count == 0)
        {
            Console.WriteLine("  > You do not have any tickets yet.");
            Console.WriteLine();
            Console.WriteLine("  Press any key to return...");
            Console.ReadKey();
            return;
        }

        Console.WriteLine($"  Tickets for {account.FirstName} {account.LastName}:\n");

        for (int i = 0; i < tickets.Count; i++)
        {
            FlightModel? flight = flightLogic.GetFlightById(tickets[i].FlightId);
            string destination = flight != null ? flightLogic.GetDestinationCity(flight) : "Unknown";
            string departure = flight != null ? flight.DepartureDatetime : "Unknown";

            Console.WriteLine($"  [{i + 1}] Ticket Number:    #{tickets[i].Id}");
            Console.WriteLine($"      Booking Ref:      {tickets[i].BookingReference}");
            Console.WriteLine($"      Passenger:        {tickets[i].FirstName} {tickets[i].LastName}");
            Console.WriteLine($"      Flight:           Rotterdam -> {destination} (Flight #{tickets[i].FlightId})");
            Console.WriteLine($"      Departure:        {departure}");
            Console.WriteLine($"      Seat:             {tickets[i].Seat}");
            Console.WriteLine();
        }

        Console.WriteLine("--------------------------------------------------");
        Console.WriteLine("  Press any key to return...");
        Console.ReadKey();
    }
}
