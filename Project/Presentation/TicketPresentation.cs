public class TicketPresentation
{
    private static TicketLogic ticketLogic = new();
    public static List<TicketDisplayModel> AllAccTickets = new();
    private static FlightLogic _flightLogic = new();
    private static AirportLogic _airportLogic = new();

    public static void Start(AccountModel account)
    {
        Console.Clear();
        AllAccTickets = ticketLogic.GetTicketsByAccountId(account.Id);

        if (AllAccTickets.Count > 0)
        {
            foreach (var ticket in AllAccTickets)
            {
                Console.WriteLine(ticket);
            }
        }
        else
        {
            Console.WriteLine("  > You don't have any tickets yet");
            Console.WriteLine("  Press any key to continue...");
            Console.ReadKey(true);
            return;
        }

        Console.WriteLine("  \n[1] return to customer menu");
        string choice = Console.ReadLine() ?? "";

        switch (choice)
        {
            case "1":
                Console.WriteLine("  > Returning to Customer menu");
                Console.WriteLine("  Press any key to continue...");
                Console.ReadKey(true);
                return;

            default:
                Console.WriteLine("  > Invalid option. Please choose 1, 2, 3 or 4.");
                Console.WriteLine();
                Console.WriteLine("  Press any key to continue...");
                Console.ReadKey(true);
                break;
        }



        Console.WriteLine("\nPress any key to return...");
        Console.ReadKey(true);
    }

    // Shown right after the customer confirms a reservation
    public static void ShowConfirmation(
        List<TicketModel> tickets,
        FlightModel? flight = null,
        List<TicketModel>? returnTickets = null,
        FlightModel? returnFlight = null)
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
            flight = _flightLogic.GetFlightById(tickets[0].Id);
        }

        string departureCity = flight != null ? _airportLogic.GetDepartureCity(flight) : "Rotterdam";
        string destination = flight != null ? _airportLogic.GetDestinationCity(flight) : "Unknown";
        string departure = flight != null ? flight.DepartureDatetime : "Unknown";

        Console.WriteLine($"  Booking reference:  {tickets[0].BookingReference}");
        Console.WriteLine();
        Console.WriteLine("  FLIGHT");
        Console.WriteLine("  ------------------------------------------------");
        Console.WriteLine($"  Route:           {departureCity} -> {destination}");
        Console.WriteLine($"  Date:            {_flightLogic.GetDate(departure)}");
        Console.WriteLine($"  Departure time:  {_flightLogic.GetTime(departure)}");
        Console.WriteLine();
        Console.WriteLine("  PASSENGERS");
        Console.WriteLine("  ------------------------------------------------");

        for (int i = 0; i < tickets.Count; i++)
        {
            Console.WriteLine($"  {i + 1}. {tickets[i].FirstName} {tickets[i].LastName}");
            Console.WriteLine($"     Ticket number: #{tickets[i].Id}    Seat: {tickets[i].Seat}");
            Console.WriteLine();
        }

        if (returnFlight != null && returnTickets != null && returnTickets.Count > 0)
        {
            string returnDepartureCity = _airportLogic.GetDepartureCity(returnFlight);
            string returnDestination = _airportLogic.GetDestinationCity(returnFlight);
            string returnDeparture = returnFlight.DepartureDatetime;

            Console.WriteLine("  RETURN FLIGHT");
            Console.WriteLine("  ------------------------------------------------");
            Console.WriteLine($"  Route:           {returnDepartureCity} -> {returnDestination}");
            Console.WriteLine($"  Date:            {_flightLogic.GetDate(returnDeparture)}");
            Console.WriteLine($"  Departure time:  {_flightLogic.GetTime(returnDeparture)}");
            Console.WriteLine();
            Console.WriteLine("  PASSENGERS (RETURN)");
            Console.WriteLine("  ------------------------------------------------");

            for (int i = 0; i < returnTickets.Count; i++)
            {
                Console.WriteLine($"  {i + 1}. {returnTickets[i].FirstName} {returnTickets[i].LastName}");
                Console.WriteLine($"     Ticket number: #{returnTickets[i].Id}    Seat: {returnTickets[i].Seat}");
                Console.WriteLine();
            }
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

        // Tickets with the same booking reference and flight belong to the same flight section
        string currentReference = "";
        Int64 currentFlightId = -1;

        for (int i = 0; i < tickets.Count; i++)
        {
            if (tickets[i].BookingReference != currentReference || tickets[i].FlightId != currentFlightId)
            {
                currentReference = tickets[i].BookingReference;
                currentFlightId = tickets[i].FlightId;

                FlightModel? flight = _flightLogic.GetFlightById(tickets[i].FlightId);
                string departureCity = flight != null ? _airportLogic.GetDepartureCity(flight) : "Rotterdam";
                string destination = flight != null ? _airportLogic.GetDestinationCity(flight) : "Unknown";
                string departure = flight != null ? flight.DepartureDatetime : "Unknown";

                Console.WriteLine("==================================================");
                Console.WriteLine($"  Booking reference:  {currentReference}");
                Console.WriteLine("--------------------------------------------------");
                Console.WriteLine($"  Route:           {departureCity} -> {destination}");
                Console.WriteLine($"  Date:            {_flightLogic.GetDate(departure)}");
                Console.WriteLine($"  Departure time:  {_flightLogic.GetTime(departure)}");
                Console.WriteLine();
                Console.WriteLine("  Passengers:");
            }

            Console.WriteLine($"   - {tickets[i].FirstName} {tickets[i].LastName}");
            Console.WriteLine($"     Ticket number: #{tickets[i].Id}    Seat: {tickets[i].Seat}");

            // Add an empty line after the last ticket of this flight section
            bool isLastTicket = i == tickets.Count - 1;
            if (isLastTicket || tickets[i + 1].BookingReference != currentReference || tickets[i + 1].FlightId != currentFlightId)
            {
                Console.WriteLine();
            }
        }

        Console.WriteLine("==================================================");
        Console.WriteLine("  Press any key to return...");
        Console.ReadKey();
    }

    public static void RemoveTicketForCustomer(AccountModel account) //ticket will still exist in database, but user does not see it anymore
    {
        Console.Clear();

        if (AllAccTickets.Count == 0)
        {
            Console.WriteLine("You have no tickets to remove");
            Console.WriteLine("\nPress any key to return...");
            Console.ReadKey(true);
            return;
        }

        bool canceling = true;

        while (canceling)
        {
            Console.Clear();
            Console.WriteLine("What ticket do you want to cancel (press ESC to exit)");
            string input = Console.ReadLine() ?? "";

            ConsoleKeyInfo action = Console.ReadKey(true);

            if (action.Key == ConsoleKey.Escape)
            {
                canceling = false;
            }

            if (int.TryParse(input, out int ticketId))
            {
                for (int i = 0; i < AllAccTickets.Count; i++)
                {
                    if (AllAccTickets[i].Id == ticketId)
                    {

                        AllAccTickets.RemoveAt(i);
                        Console.ReadKey(true);
                        break;

                    }
                }

            }
            else
            {
                Console.WriteLine();
                Console.WriteLine("  > Invalid choice. Please choose a number from the list.");
                Console.WriteLine();
                Console.WriteLine("  Press any key to continue...");
                Console.ReadKey();
            }
        }

    }
}