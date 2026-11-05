static class TicketPresentation
{
    private static TicketLogic ticketLogic = new TicketLogic();
    private static FlightLogic flightLogic = new FlightLogic();

    // Shown right after the customer confirms a reservation
    public static void ShowConfirmation(List<TicketModel> tickets, FlightModel? flight = null)
    {
        Console.Clear();
        Console.WriteLine();
        Console.WriteLine("==================================================");
        Console.WriteLine("                BOOKING CONFIRMED                 ");
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

        Console.WriteLine($"  Booking reference:  {tickets[0].BookingReference}");
        Console.WriteLine();
        Console.WriteLine("  FLIGHT");
        Console.WriteLine("  ------------------------------------------------");
        Console.WriteLine($"  Route:           Rotterdam -> {destination}");
        Console.WriteLine($"  Date:            {flightLogic.GetDate(departure)}");
        Console.WriteLine($"  Departure time:  {flightLogic.GetTime(departure)}");
        Console.WriteLine();
        Console.WriteLine("  PASSENGERS");
        Console.WriteLine("  ------------------------------------------------");

        for (int i = 0; i < tickets.Count; i++)
        {
            Console.WriteLine($"  {i + 1}. {tickets[i].FirstName} {tickets[i].LastName}");
            Console.WriteLine($"     Ticket number: #{tickets[i].Id}    Seat: {tickets[i].Seat}");
            Console.WriteLine();
        }

        Console.WriteLine("--------------------------------------------------");
        Console.WriteLine("  You can see this again under 'Booking confirmations'");
        Console.WriteLine("  in the Customer Menu.");
        Console.WriteLine();
        Console.WriteLine("  Press any key to return to the menu...");
        Console.ReadKey();
    }

    // Shown from the Customer Menu: all bookings of the logged in customer
    public static void ShowMyTickets(AccountModel? account = null)
    {
        Console.Clear();
        Console.WriteLine();
        Console.WriteLine("==================================================");
        Console.WriteLine("               BOOKING CONFIRMATIONS              ");
        Console.WriteLine("==================================================");
        Console.WriteLine();

        if (account == null)
        {
            account = AccountsLogic.CurrentAccount;
        }

        if (account == null)
        {
            Console.WriteLine("  > Please log in first to view your bookings.");
            Console.WriteLine();
            Console.WriteLine("  Press any key to return...");
            Console.ReadKey();
            return;
        }

        List<TicketModel> tickets = ticketLogic.GetTicketsForAccount(account.Id);

        if (tickets.Count == 0)
        {
            Console.WriteLine("  > You do not have any bookings yet.");
            Console.WriteLine("  > Choose 'Make a reservation' in the menu to book a flight.");
            Console.WriteLine();
            Console.WriteLine("  Press any key to return...");
            Console.ReadKey();
            return;
        }

        Console.WriteLine($"  Bookings of {account.FirstName} {account.LastName}");
        Console.WriteLine();

        // Tickets with the same booking reference belong to the same booking
        string currentReference = "";

        for (int i = 0; i < tickets.Count; i++)
        {
            if (tickets[i].BookingReference != currentReference)
            {
                currentReference = tickets[i].BookingReference;

                FlightModel? flight = flightLogic.GetFlightById(tickets[i].FlightId);
                string destination = flight != null ? flightLogic.GetDestinationCity(flight) : "Unknown";
                string departure = flight != null ? flight.DepartureDatetime : "Unknown";

                Console.WriteLine("==================================================");
                Console.WriteLine($"  Booking reference:  {currentReference}");
                Console.WriteLine("--------------------------------------------------");
                Console.WriteLine($"  Route:           Rotterdam -> {destination}");
                Console.WriteLine($"  Date:            {flightLogic.GetDate(departure)}");
                Console.WriteLine($"  Departure time:  {flightLogic.GetTime(departure)}");
                Console.WriteLine();
                Console.WriteLine("  Passengers:");
            }

            Console.WriteLine($"   - {tickets[i].FirstName} {tickets[i].LastName}");
            Console.WriteLine($"     Ticket number: #{tickets[i].Id}    Seat: {tickets[i].Seat}");

            // Add an empty line after the last ticket of this booking
            bool isLastTicket = i == tickets.Count - 1;
            if (isLastTicket || tickets[i + 1].BookingReference != currentReference)
            {
                Console.WriteLine();
            }
        }

        Console.WriteLine("==================================================");
        Console.WriteLine("  Press any key to return...");
        Console.ReadKey();
    }
}
